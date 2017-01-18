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
    public class DrivingInfoController : ApiController
    {
        #region Proprities DBAccess and Service Business  methods classes
        private DrivingInfoDBMethod _drivingInfoDBMethod = new DrivingInfoDBMethod();
        private EvaluationDBMethod _evaluationDBMethod = new EvaluationDBMethod();
        private CriticismDrivingDBMethod _criticismDBMethod = new CriticismDrivingDBMethod();
        private InformationBusiness _informationService = new InformationBusiness();
        #endregion

        #region CRUD Methods
        /// <summary>
        /// GET: api/DrivingInfo
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public IHttpActionResult Get()
        {
            try
            {
                var listDrivingInfo = new List<DrivingInfoModel>();
                var drivingInfos = _drivingInfoDBMethod.FindAll();
                if (drivingInfos != null)
                {
                    foreach (DrivingInfo i in drivingInfos)
                    {
                        var latString = _informationService.GetDisplayFormatLatitudeLongitude(i.Latitude);
                        var longString = _informationService.GetDisplayFormatLatitudeLongitude(i.Longitude);
                        listDrivingInfo.Add(new DrivingInfoModel()
                        {
                            Id = i.Id,
                            Date = i.Date,
                            ValuePertinence = i.ValuePertinence,
                            IsValidatedPertinence = i.IsValidatedPertinence,
                            IsReportedOutDated = i.IsReportedOutDated,
                            Latitude = i.Latitude,
                            Longitude = i.Longitude,
                            LatitudeString = latString,
                            LongitudeString = longString,
                            StreetName = i.StreetName,
                            Description = i.Description,
                            Severity = i.Severity,
                            AdditionalInfo = i.AdditionalInfo,
                            IdUser = i.IdUserPublication,
                            UserName = i.UserPublication.UserName
                        });
                    }
                    return Ok(listDrivingInfo);
                }
                return Ok(drivingInfos);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        /// <summary>
        /// GET: api/DrivingInfo/id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public IHttpActionResult Get(int id)
        {
            try
            {
                var drivingInfo = _drivingInfoDBMethod.FindById(id);
                if (drivingInfo != null)
                {
                    var latString = _informationService.GetDisplayFormatLatitudeLongitude(drivingInfo.Latitude);
                    var longString = _informationService.GetDisplayFormatLatitudeLongitude(drivingInfo.Longitude);
                    return Ok(new DrivingInfoModel()
                    {
                        Id = drivingInfo.Id,
                        Date = drivingInfo.Date,
                        ValuePertinence = drivingInfo.ValuePertinence,
                        IsValidatedPertinence = drivingInfo.IsValidatedPertinence,
                        IsReportedOutDated = drivingInfo.IsReportedOutDated,
                        Latitude = drivingInfo.Latitude,
                        Longitude = drivingInfo.Longitude,
                        LatitudeString = latString,
                        LongitudeString = longString,
                        StreetName = drivingInfo.StreetName,
                        Description = drivingInfo.Description,
                        Severity = drivingInfo.Severity,
                        AdditionalInfo = drivingInfo.AdditionalInfo,
                        IdUser = drivingInfo.IdUserPublication,
                        UserName = drivingInfo.UserPublication.UserName
                    });
                }
                return Ok(drivingInfo);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        /// <summary>
        /// POST: api/DrivingInfo
        /// </summary>
        /// <param name="inputModel"></param>
        /// <returns></returns>
        public IHttpActionResult Post([FromBody]DrivingInfoInputModel inputModel)
        {
            Response response = new Response();
            if (ModelState.IsValid)
            {
                try
                {
                    DrivingInfo drivingInfo = new DrivingInfo(DateTime.Today, null, false, false, inputModel.AdditionalInfo,  inputModel.Latitude, inputModel.Longitude, inputModel.StreetName, inputModel.Description, null, User.Identity.GetUserId());
                    _drivingInfoDBMethod.Add(drivingInfo);
                    response.IsSuccess = true;
                    response.SucessrMsg = Resources.Resources.ValidOperation;
                    response.Id = drivingInfo.Id;
                    return Ok(response);
                }
                catch (Exception ex)
                {
                    response.IsSuccess = false;
                    response.ErrorMsg = ex.Message;
                    return Ok(response);
                }  
            }
            response.IsSuccess = false;
            response.ErrorMsg = Resources.Resources.InvalidInputModel;
            return Ok(response);
        }

        /// <summary>
        /// PUT: api/DrivingInfo/id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="editModel"></param>
        /// <returns></returns>
        public IHttpActionResult Put(int id, [FromBody]DrivingInfoEditModel editModel)
        {
            Response response = new Response();
            using (SqlConnection connexion = ParkingInfoDBMethod.GetDatabaseConnection())
            {
                connexion.Open();
                using (SqlTransaction transaction = connexion.BeginTransaction())
                {
                    try
                    {
                        DrivingInfo drivingInfo =  _drivingInfoDBMethod.FindById(id);
                        if (drivingInfo != null)
                        {
                            AspNetUser userAlert = null;
                            if (drivingInfo.AspNetUsers.Count > 0)
                                userAlert = (from m in drivingInfo.AspNetUsers where m.Id.Equals(User.Identity.GetUserId()) select m).Single();

                            if (userAlert == null)
                            {
                                var _userDBMethod = new UserDBMethod();
                                var user = _userDBMethod.FindById(User.Identity.GetUserId());
                                drivingInfo.AspNetUsers.Add(user);
                                _drivingInfoDBMethod.Save(drivingInfo);
                                bool isRepotedOk = _informationService.CalculReportedDateDriving(id);
                                drivingInfo.UpdateDrivingInfo(isRepotedOk);
                                _drivingInfoDBMethod.Save(drivingInfo);
                                response.IsSuccess = true;
                                response.SucessrMsg = Resources.Resources.ValidOperation;
                                response.Id = drivingInfo.Id;
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
                        response.ErrorMsg = Resources.Resources.OccuredError;
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
        /// DELETE: api/DrivingInfo/id
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
                        var drivingInfo = _drivingInfoDBMethod.FindById(id);
                        var evaluations = _evaluationDBMethod.FindByDrivingId(id);
                        var criticisms = _criticismDBMethod.FindByDriving(id);
                        if (drivingInfo != null && evaluations != null && criticisms != null)
                        {
                            _evaluationDBMethod.DeleteEvaluationByDrivingId(evaluations);
                            _criticismDBMethod.DeleteCriticismByInformation(criticisms);
                            _drivingInfoDBMethod.Delete(drivingInfo);
  
                            response.IsSuccess = true;
                            response.SucessrMsg = Resources.Resources.ValidOperation;
                            response.Id = drivingInfo.Id;
                            return Ok(response);
                        }
                        response.IsSuccess = false;
                        response.ErrorMsg = Resources.Resources.OccuredError;
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
