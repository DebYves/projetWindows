using NamRider.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using static NamRider.Util.ApiConstants;

namespace NamRider.DAO
{
    public class EvaluationService
    {
        public async Task<List<EvaluationModel>> GetAllEvalution()
        {
                var clientAllEval = new HttpClient();
                var resultAllEval = clientAllEval.GetAsync(EvaluationApiMethod.GetAllEvaluation).Result;
                var allEval = await resultAllEval.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<EvaluationModel>>(allEval);
            
        }

        public async Task<EvaluationModel> GetEvaluationDrivingById(string userId, int idDriving)
        {
                var clientEval = new HttpClient();
                var resultEval = clientEval.GetAsync(string.Format(EvaluationApiMethod.GetEvaluationById, idDriving, userId)).Result;
                var evaluation = await resultEval.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<EvaluationModel>(evaluation);
        }

        public async Task<Response> AddEvaluationDriving(EvaluationInputViewModel inputModel)
        {
            try
            {
                var clientDriving = new HttpClient();
                var drivingEval = Task.Run(() => JsonConvert.SerializeObject(inputModel)).Result;
                var httpContent = new StringContent(drivingEval, Encoding.UTF8, FormatJson);
                clientDriving.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(FormatJson));
                clientDriving.DefaultRequestHeaders.Add(AuthorizationTitle, Token);
                var resultDriving = clientDriving.PostAsync(EvaluationApiMethod.AddEvaluation, httpContent).Result;
                var result = await resultDriving.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Response>(result);
            }
            catch (Exception ex)
            {
                Response response = new Response();
                response.IsSuccess = false;
                response.ErrorMsg = ex.Message;
                return response;
            }
        }

    }
}
