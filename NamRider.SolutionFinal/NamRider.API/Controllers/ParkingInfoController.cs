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
using System.Web.Http.ModelBinding;
using Microsoft.AspNet.Identity;

namespace NamRider.API.Controllers
{
    [Authorize]
    public class ParkingInfoController : ApiController
    {
        #region Proprities DBAccess and Service Business  methods classes
        private ParkingInfoDBMethod _parkingInfoDBMethod = new ParkingInfoDBMethod();
        private GeographicPointDBMethod _geographicPointDBMethod = new GeographicPointDBMethod();
        private CriticismParkingDBMethod _criticismParkingDBMethod = new CriticismParkingDBMethod();
        private InformationBusiness _informationService = new InformationBusiness();
        private UserDBMethod _userDBMethod = new UserDBMethod();
        #endregion

        #region CRUD Methods
        /// <summary>
        /// GET: api/ParkingInfo
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public IHttpActionResult Get()
        {
            try
            {
                var listParking = new List<ParkingInfoModel>();
                var parkingInfos = _parkingInfoDBMethod.FindAll();
                var geographicPoints = _geographicPointDBMethod.FindAll();
                if (parkingInfos != null)
                {
                    foreach (ParkingInfo i in parkingInfos)
                    {
                        var point = (from m in geographicPoints where m.IdParking.Equals(i.Id) select m).Single();
                        var latString = _informationService.GetDisplayFormatLatitudeLongitude(point.Latitude);
                        var longString = _informationService.GetDisplayFormatLatitudeLongitude(point.Longitude);
                        listParking.Add(new ParkingInfoModel()
                        {
                            Id = i.Id,
                            Latitude = point.Latitude,
                            Longitude = point.Longitude,
                            LatitudeString = latString,
                            LongitudeString = longString,
                            Rayon = i.Rayon,
                            Type = i.Type,
                            NumberPlace = i.NumberPlace,
                            IsEstimatedPlace = i.IsEstimatedPlace,
                            ValuePertinence = i.ValuePertinence,
                            IsValidatedPertinence = i.IsValidatedPertinence,
                            IsReportedOutDated = i.IsReportedOutDated,
                            Description = i.Description,
                            IdUser = i.IdUserPublication,
                            UserName = i.AspNetUser.UserName,
                        });
                    }
                    return Ok(listParking);
                }
                return Ok(parkingInfos);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        /// <summary>
        /// GET: api/ParkingInfo/id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public IHttpActionResult Get(int id)
        {
            using (SqlConnection connexion = ParkingInfoDBMethod.GetDatabaseConnection())
            {
                connexion.Open();
                using (SqlTransaction transaction = connexion.BeginTransaction())
                {
                    try
                    {
                        var parkingInfo = _parkingInfoDBMethod.FindById(id);
                        if(parkingInfo != null )
                        {
                            var pointGeographic = _geographicPointDBMethod.FindByIdParkng(id);
                            var latString = _informationService.GetDisplayFormatLatitudeLongitude(pointGeographic.Latitude);
                            var longString = _informationService.GetDisplayFormatLatitudeLongitude(pointGeographic.Longitude);
                            return Ok(new ParkingInfoModel()
                            {
                                Id = parkingInfo.Id,
                                Latitude = pointGeographic.Latitude,
                                Longitude = pointGeographic.Longitude,
                                LatitudeString = latString,
                                LongitudeString = longString,
                                Rayon = parkingInfo.Rayon,
                                Type = parkingInfo.Type,
                                NumberPlace = parkingInfo.NumberPlace,
                                IsEstimatedPlace = parkingInfo.IsEstimatedPlace,
                                ValuePertinence = parkingInfo.ValuePertinence,
                                IsValidatedPertinence = parkingInfo.IsValidatedPertinence,
                                IsReportedOutDated = parkingInfo.IsReportedOutDated,
                                Description = parkingInfo.Description,
                                IdUser = parkingInfo.IdUserPublication,
                                UserName = parkingInfo.AspNetUser.UserName,
                            });
                        }
                        return Ok(parkingInfo);
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return Ok(ex.Message);
                    }
                    finally
                    {
                        connexion.Close();
                    }
                }
            }
        }

        /// <summary>
        /// POST: api/ParkingInfo
        /// </summary>
        /// <param name="inputModel"></param>
        /// <returns></returns>
        public IHttpActionResult Post([FromBody]ParkingInfoInputModel inputModel)
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
                            var point = _geographicPointDBMethod.FindById(inputModel.Latitude, inputModel.Longitude);
                            if(point == null)
                            {
                                // public ParkingInfo(DateTime date, int? pertinence, bool isValidPert, bool isReportDate, decimal rayon, string type, int? number, bool? isEstimated, string descrip, string idUser)
                                ParkingInfo parkingInfo = new ParkingInfo(DateTime.Today, false, false, inputModel.Rayon, inputModel.Type, inputModel.NumberPlace, inputModel.IsEstimatedPlace, inputModel.Description, User.Identity.GetUserId());
                                _parkingInfoDBMethod.Add(parkingInfo);
                                var lastPark = _parkingInfoDBMethod.FindAll().LastOrDefault();
                                GeographicPoint geographicPoint = new GeographicPoint(inputModel.Latitude, inputModel.Longitude, lastPark.Id);
                                _geographicPointDBMethod.Add(geographicPoint);

                                response.IsSuccess = true;
                                response.SucessrMsg = Resources.Resources.ValidOperation;
                                response.Id = parkingInfo.Id;
                                return Ok(response);
                            }
                            response.IsSuccess = false;
                            response.ErrorMsg = Resources.Resources.ExistingParking;
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
        /// PUT: api/ParkingInfo/id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="editModel"></param>
        /// <returns></returns>
        public IHttpActionResult Put(int id, [FromBody]ParkingInfoEditModel editModel)
        {
            Response response = new Response();
            using (SqlConnection connexion = ParkingInfoDBMethod.GetDatabaseConnection())
            {
                connexion.Open();
                using (SqlTransaction transaction = connexion.BeginTransaction())
                {
                    try
                    {
                        var parkingInfo = _parkingInfoDBMethod.FindById(id);
                        if (parkingInfo != null)
                        {
                            AspNetUser userAlert = null;
                            if (parkingInfo.AspNetUsers.Count > 0)
                                userAlert = (from m in parkingInfo.AspNetUsers where m.Id.Equals(User.Identity.GetUserId()) select m).Single();
                            if (userAlert == null)
                            {
                                var user = _userDBMethod.FindById(User.Identity.GetUserId());
                                parkingInfo.AspNetUsers.Add(user);
                                _parkingInfoDBMethod.Save(parkingInfo);
                                bool isRepotedOk = _informationService.CalculReportedDatePark(id);
                                parkingInfo.UpdateParkiningInfo(isRepotedOk);
                                _parkingInfoDBMethod.Save(parkingInfo);
                                response.IsSuccess = true;
                                response.SucessrMsg = Resources.Resources.ValidOperation;
                                response.Id = parkingInfo.Id;
                                return Ok(response);
                            }
                            response.IsSuccess = false;
                            response.ErrorMsg = "";
                            return Ok(response);
                        }
                        response.IsSuccess = false;
                        response.ErrorMsg = Resources.Resources.NoExistingDrivingEval;
                        return Ok(response);
                    }
                    catch (Exception ex)
                    {
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

        /// <summary>
        /// DELETE: api/ParkingInfo/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IHttpActionResult Delete(int id)
        {
            Response response = new Response();
            using (SqlConnection connexion = ParkingInfoDBMethod.GetDatabaseConnection())
            {
                connexion.Open();
                using (SqlTransaction transaction = connexion.BeginTransaction())
                {
                    try
                    {
                        var parkingInfo = _parkingInfoDBMethod.FindById(id);
                        var geographicPoint = _geographicPointDBMethod.FindByIdParkng(id);
                        var criticisms = _criticismParkingDBMethod.FindByParking(id);

                        if (parkingInfo != null && geographicPoint != null && criticisms != null)
                        {
                            _geographicPointDBMethod.Delete(geographicPoint);
                            _criticismParkingDBMethod.DeleteCriticismByParking(criticisms);
                            _parkingInfoDBMethod.Delete(parkingInfo);

                            response.IsSuccess = true;
                            response.SucessrMsg = Resources.Resources.ValidOperation;
                            response.Id = parkingInfo.Id;
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

        #region Others Methods
        /// <summary>
        /// GET_BY_TYPE: api/ParkingInfo/GetByType
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("api/ParkingInfo/GetByType")]
        [HttpPost]
        [AllowAnonymous]
        public IHttpActionResult GetByType([FromBody]ParkingInfoFilterModel input)
        {
            try
            {
                var listParking = new List<ParkingInfoModel>();
                var filter = new ParkingInfoFilter(input.AlternancyZone, input.DisckZone, input.FreeZone, input.PayZone, input.SubcriptionZone);
                var listString = _informationService.TransformFilterModelToList(filter);
                var parkingInfos = _parkingInfoDBMethod.FindByType(listString);
                if (parkingInfos != null)
                {
                    foreach (ParkingInfo i in parkingInfos)
                    {
                        listParking.Add(new ParkingInfoModel()
                        {
                            Id = i.Id,
                            Rayon = i.Rayon,
                            Type = i.Type,
                            NumberPlace = i.NumberPlace,
                            IsEstimatedPlace = i.IsEstimatedPlace,
                            Description = i.Description
                        });
                    }
                    return Ok(listParking);
                }
                return Ok(parkingInfos);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        /// <summary>
        /// GET_BY_POINT_GEOGRAPHIC: api/ParkingInfo/GetParkingByPoint
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("api/ParkingInfo/GetParkingByPoint")]
        [HttpPost]
        public IHttpActionResult GetParkingByPoint(GeographicPointModel point)
        {
            try
            {
                var park = _geographicPointDBMethod.FindById(point.Latitude, point.Longitude);
                if(park != null)
                {
                    return Ok(new GeographicPointModel()
                    {
                        Latitude = park.Latitude,
                        Longitude = park.Longitude,
                        IdParking = park.IdParking
                    });
                }
                return Ok(park);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        #endregion
    }
}
