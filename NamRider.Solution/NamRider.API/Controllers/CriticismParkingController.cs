using Microsoft.AspNet.Identity;
using NamRider.API.Models;
using NamRider.API.NamRider.Service;
using NamRider.API.NamRiderAPI.Persistence;
using NamRider.API.NamRiderDBAccess;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace NamRider.API.Controllers
{
    [Authorize]
    public class CriticismParkingController : ApiController
    {
        #region Proprities DBAccess and Service Business  methods classes
        private CriticismParkingDBMethod _criticismParkingDBMethod = new CriticismParkingDBMethod();
        private ParkingInfoDBMethod _parkingInfoDBMethod = new ParkingInfoDBMethod();
        private InformationBusiness _informationService = new InformationBusiness();
        #endregion

        #region CRUD Methods
        /// <summary>
        ///  GET: api/CriticismParking
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public IHttpActionResult Get()
        {
            try
            {
                var listCriticisms = new List<CriticismParkingModel>();
                var criticisms = _criticismParkingDBMethod.FindAll();
                if (criticisms != null)
                {
                    foreach (var i in criticisms)
                    {
                        listCriticisms.Add(new CriticismParkingModel()
                        {
                            IdParking = i.IdParking,
                            IdUser = i.UserId,
                            UserName = i.AspNetUser.UserName,
                            Value = i.Value
                        });
                    }
                    return Ok(listCriticisms);
                }
                return Ok(criticisms);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        /// <summary>
        /// GET_BY_IDPARK_AND_USERID: api/CriticismParking/GetByIdParkingCritism/idPark/userId
        /// </summary>
        /// <param name="idPark"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("api/CriticismParking/GetByIdParkingCritism/{idPark}/{userId}")]
        [HttpGet]
        public IHttpActionResult Get(int idPark, string userId)
        {
            try
            {
                var criticism = _criticismParkingDBMethod.FindById(userId, idPark);
                if (criticism != null)
                {
                    return Ok(new CriticismParkingModel()
                    {
                        IdParking = criticism.IdParking,
                        IdUser = criticism.UserId,
                        UserName = criticism.AspNetUser.UserName,
                        Value = criticism.Value
                    });
                }
                return Ok(criticism);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        /// <summary>
        /// POST: api/CriticismParking
        /// </summary>
        /// <param name="inputModel"></param>
        /// <returns></returns>
        public IHttpActionResult Post([FromBody]CriticismParkingInputModel inputModel)
        {
            Response response = new Response();
            if (ModelState.IsValid)
            {
                using (SqlConnection connexion = ParkingInfoDBMethod.GetDatabaseConnection())
                {
                    connexion.Open();
                    using (SqlTransaction transaction = connexion.BeginTransaction())
                    {
                        try
                        {
                            var critism = _criticismParkingDBMethod.FindById(User.Identity.GetUserId(), inputModel.IdParking);
                            if(critism == null )
                            {
                                var criticism = new CriticismParkingInfo();
                                criticism.IdParking = inputModel.IdParking;
                                criticism.UserId = User.Identity.GetUserId();
                                criticism.Value = inputModel.Value;
                                _criticismParkingDBMethod.Add(criticism);
                                int valuePert = _informationService.CalculValuePertinencePark(inputModel.IdParking);
                                bool isValuePert = _informationService.IsPertinence(valuePert);
                                var parking = _parkingInfoDBMethod.FindById(inputModel.IdParking);
                                parking.UpdateParkiningInfo(valuePert, isValuePert);
                                _parkingInfoDBMethod.Save(parking);

                                response.IdString = criticism.IdParking + " " + criticism.UserId;
                                response.IsSuccess = true;
                                response.SucessrMsg = Resources.Resources.ValidOperation;
                                return Ok(response);
                            }
                            response.IsSuccess = false;
                            response.ErrorMsg = Resources.Resources.ExistingParkCritism;
                            return Ok(response);
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            response.IsSuccess = false;
                            response.ErrorMsg = ex.Message;
                            return Ok(response);
                        }
                        finally
                        {
                            connexion.Close();
                        }
                    }
                }

            }
            response.IsSuccess = false;
            response.ErrorMsg = Resources.Resources.InvalidInputModel;
            return Ok(response);
        }

        /// <summary>
        /// DELETE_BY_IDPARK_AND_USERID: api/CriticismParking/DeleteParkingCritism/idPark/userId
        /// </summary>
        /// <param name="idPark"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [Route("api/CriticismParking/DeleteParkingCritism/{idPark}/{userId}")]
        [HttpDelete]
        public IHttpActionResult DeleteCriticismParking(int idPark, string userId)
        {
            Response response = new Response();
            using (SqlConnection connexion = ParkingInfoDBMethod.GetDatabaseConnection())
            {
                connexion.Open();
                using (SqlTransaction transaction = connexion.BeginTransaction())
                {
                    try
                    {
                        var critism = _criticismParkingDBMethod.FindById(userId, idPark);
                        if (critism != null)
                        {
                            _criticismParkingDBMethod.Delete(critism);
                            int valuePert = _informationService.CalculValuePertinencePark(idPark);
                            bool isValuePert = _informationService.IsPertinence(valuePert);
                            var parking = _parkingInfoDBMethod.FindById(idPark);
                            parking.UpdateParkiningInfo(valuePert, isValuePert);
                            _parkingInfoDBMethod.Save(parking);
                            response.IsSuccess = true;
                            response.SucessrMsg = Resources.Resources.ValidOperation;
                            return Ok(response);
                        }
                        response.IsSuccess = false;
                        response.ErrorMsg = Resources.Resources.NoExistingDrivingEval;
                        return Ok(response);
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        response.IsSuccess = false;
                        response.ErrorMsg = ex.Message;
                        return Ok(response);
                    }
                    finally
                    {
                        connexion.Close();
                    }
                }
            }
        }
                   
        #endregion
    }
}
