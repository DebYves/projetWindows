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
    public class EvaluationController : ApiController
    {
        #region Proprities DBAccess and Service Business  methods classes
        private EvaluationDBMethod _evaluationDBMethod = new EvaluationDBMethod();
        private InformationBusiness _informationService = new InformationBusiness();
        private DrivingInfoDBMethod _divingInfoDBMethod = new DrivingInfoDBMethod();
        #endregion

        /// <summary>
        /// GET: api/Evaluation
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public IHttpActionResult Get()
        {
            try
            {
                var listEvaluations = new List<EvaluationModel>();
                var evaluations = _evaluationDBMethod.FindAll();
                if (evaluations != null)
                {
                    foreach (Evaluation i in evaluations)
                    {
                        listEvaluations.Add(new EvaluationModel()
                        {
                            IdDriving = i.IdDriving,
                            IdUser = i.UserId,
                            UserName = i.AspNetUser.UserName,
                            Value = i.Value
                        });
                    }
                    return Ok(listEvaluations);
                }
                return Ok(evaluations);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        /// <summary>
        /// GET_BY_IDDRIVING_AND_USERID: api/Evaluation/GetByIdEvaluation/idDriving/userId
        /// </summary>
        /// <param name="idDriving"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("api/Evaluation/GetByIdEvaluation/{idDriving}/{userId}")]
        [HttpGet]
        public IHttpActionResult Get(int idDriving, string userId)
        {
            try
            {
                var evaluation = _evaluationDBMethod.FindById(userId, idDriving);
                if (evaluation != null)
                {
                    return Ok(new EvaluationModel()
                    {
                        IdDriving = evaluation.IdDriving,
                        IdUser = evaluation.UserId,
                        UserName = evaluation.AspNetUser.UserName,
                        Value = evaluation.Value
                    });
                }
                return Ok(evaluation);
            }   
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        /// <summary>
        /// POST: api/Evaluation
        /// </summary>
        /// <param name="inputModel"></param>
        /// <returns></returns>
        public IHttpActionResult Post([FromBody]EvaluationInputViewModel inputModel)
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
                            var eval = _evaluationDBMethod.FindById(User.Identity.GetUserId(), inputModel.IdDriving);
                            if(eval == null)
                            {
                                Evaluation evaluation = new Evaluation();
                                evaluation.UserId = User.Identity.GetUserId();
                                evaluation.IdDriving = inputModel.IdDriving;
                                evaluation.Value = inputModel.Value;
                                _evaluationDBMethod.Add(evaluation);
                                int drivingSeverity = _informationService.GetValueSeverityInfo(evaluation.IdDriving);
                                var driving = _divingInfoDBMethod.FindById(evaluation.IdDriving);
                                driving.UpdateDrivingInfo(drivingSeverity);
                                _divingInfoDBMethod.Save(driving);
                                response.IdString = evaluation.IdDriving + " UserId: " + evaluation.UserId;
                                response.IsSuccess = true;
                                response.SucessrMsg = Resources.Resources.ValidOperation;
                                return Ok(response);
                            }
                            response.IsSuccess = false;
                            response.ErrorMsg = Resources.Resources.ExistingDrivingEval;
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
        /// DELETE_IDDRIVING_AND_USERID: api/Evaluation/DeleteEvaluation/idDriving/userId
        /// </summary>
        /// <param name="idDriving"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [Route("api/Evaluation/DeleteEvaluation/{idDriving}/{userId}")]
        [HttpDelete]
        public IHttpActionResult Delete(int idDriving, string userId)
        {
            Response response = new Response();
            using (SqlConnection connexion = ParkingInfoDBMethod.GetDatabaseConnection())
            {
                connexion.Open();
                using (SqlTransaction transaction = connexion.BeginTransaction())
                {
                    try
                    {
                        var evaluation = _evaluationDBMethod.FindById(userId, idDriving);
                        if (evaluation != null)
                        {
                            _evaluationDBMethod.Delete(evaluation);
                            int drivingSeverity = _informationService.GetValueSeverityInfo(evaluation.IdDriving);
                            var driving = _divingInfoDBMethod.FindById(evaluation.IdDriving);
                            driving.UpdateDrivingInfo(drivingSeverity);
                            _divingInfoDBMethod.Save(driving);
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
    }
}
