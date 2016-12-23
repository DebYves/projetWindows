using NamRider.API.NamRider.Service;
using NamRider.API.NamRiderAPI.Persistence;
using NamRider.API.NamRiderDBAccess;
using NamRider.API.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;

namespace NamRider.API.Controllers
{
    [Authorize]
    public class CriticismDrivingController : ApiController
    {
        #region Proprities DBAccess and Service Business  methods classes
        private CriticismDrivingDBMethod _criticismDrivingDBMethod = new CriticismDrivingDBMethod();
        private DrivingInfoDBMethod _drivingInfoDBMethod = new DrivingInfoDBMethod();
        private InformationBusiness _informationService = new InformationBusiness();
        #endregion

        #region CRUD Methods
        /// <summary>
        /// GET_ALL: api/CriticismDriving
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public IHttpActionResult Get()
        {
            try
            {
                var listCriticisms = new List<CriticismDrivingModel>();
                var criticisms = _criticismDrivingDBMethod.FindAll();
                if (criticisms != null)
                {
                    foreach (CriticismDrivingInfo i in criticisms)
                    {
                        listCriticisms.Add(new CriticismDrivingModel()
                        {
                            IdDriving = i.IdDriving,
                            IdUser = i.UserId,
                            UserName = i.UserCritismDriving.UserName,
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
        /// GET_BY_IDRIVIN_AND_USERID: api/CriticismDriving/GetByIdDrivingCritism/idDriving}/userId
        /// </summary>
        /// <param name="idDriving"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [AllowAnonymous] 
        [Route("api/CriticismDriving/GetByIdDrivingCritism/{idDriving}/{userId}")]
        [HttpGet]
        public IHttpActionResult Get(int idDriving, string userId)
        {
            try
            {
                var criticism = _criticismDrivingDBMethod.FindById(userId, idDriving);
                if (criticism != null)
                {
                    return Ok(new CriticismDrivingModel()
                    {
                        IdDriving = criticism.IdDriving,
                        IdUser = criticism.UserId,
                        UserName = criticism.UserCritismDriving.UserName,
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
        /// POST: api/CriticismDriving
        /// </summary>
        /// <param name="inputModel"></param>
        /// <returns></returns>
        public IHttpActionResult Post([FromBody]CriticismDrivingInputModel inputModel)
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
                            var critism = _criticismDrivingDBMethod.FindById(User.Identity.GetUserId(), inputModel.IdDriving);
                            if(critism == null)
                            {
                                var criticism = new CriticismDrivingInfo();
                                criticism.IdDriving = inputModel.IdDriving;
                                criticism.UserId = User.Identity.GetUserId();
                                criticism.Value = inputModel.Value;
                                _criticismDrivingDBMethod.Add(criticism);
                                int valuePert = _informationService.CalculValuePertinenceDriving(inputModel.IdDriving);
                                bool isValuePert = _informationService.IsPertinence(valuePert);
                                var driving = _drivingInfoDBMethod.FindById(inputModel.IdDriving);
                                driving.UpdateDrivingInfo(valuePert, isValuePert);
                                _drivingInfoDBMethod.Save(driving);
                                response.IdString = criticism.IdDriving + " " + criticism.UserCritismDriving;
                                response.IsSuccess = true;
                                response.SucessrMsg = Resources.Resources.ValidOperation;
                                return Ok(response);
                            }
                            response.IsSuccess = false;
                            response.ErrorMsg = Resources.Resources.ExistingDrivingCritism;
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
        /// DELETE_BY_IDRIVIN_AND_USERID: api/CriticismDriving/DeleteDrivingCritism/idDriving/idUser
        /// </summary>
        /// <param name="idDriving"></param>
        /// <param name="idUser"></param>
        /// <returns></returns>
        [Route("api/CriticismDriving/DeleteDrivingCritism/{idDriving}/{idUser}")]
        [HttpDelete]
        public IHttpActionResult DeleteDrivingCritism(int idDriving, string idUser)
        {
            Response response = new Response();
            using (SqlConnection connexion = ParkingInfoDBMethod.GetDatabaseConnection())
            {
                connexion.Open();
                using (SqlTransaction transaction = connexion.BeginTransaction())
                {
                    try
                    {
                        var critism = _criticismDrivingDBMethod.FindById(idUser, idDriving);
                        if (critism != null)
                        {
                            _criticismDrivingDBMethod.Delete(critism);
                            int valuePert = _informationService.CalculValuePertinenceDriving(idDriving);
                            bool isValuePert = _informationService.IsPertinence(valuePert);
                            var driving = _drivingInfoDBMethod.FindById(idDriving);
                            driving.UpdateDrivingInfo(valuePert, isValuePert);
                            _drivingInfoDBMethod.Save(driving);
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
