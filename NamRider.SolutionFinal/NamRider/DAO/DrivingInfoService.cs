using NamRider.Model;
using Newtonsoft.Json;
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
    class DrivingInfoService
    {
        public async Task<List<DrivingInfoModel>> GetAllDrivingInfo()
        {
                var clientAllDriving = new HttpClient();
                var resultAllDriving = clientAllDriving.GetAsync(DrivingInfoApiMethod.GetAllDrivingInfo).Result;
                var allDriving = await resultAllDriving.Content.ReadAsStringAsync();
                var returnDriving = JsonConvert.DeserializeObject<List<DrivingInfoModel>>(allDriving);
                return returnDriving;
        }

        public async Task<DrivingInfoModel> GetDrivingById(int id)
        {
                var clientDriving = new HttpClient();
                var resultDriving = clientDriving.GetAsync(string.Format(DrivingInfoApiMethod.GetDrivingById, id)).Result;
                var driving = await resultDriving.Content.ReadAsStringAsync();
                var returnDriving = JsonConvert.DeserializeObject<DrivingInfoModel>(driving);
                return returnDriving;
        }

        public async Task<Response> AddDriving(DrivingInfoInputModel inputModel)
        {
            try
            {
                var clientDriving = new HttpClient();
                var driving = Task.Run(() => JsonConvert.SerializeObject(inputModel)).Result;
                var httpContent = new StringContent(driving, Encoding.UTF8, FormatJson);
                clientDriving.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(FormatJson));
                clientDriving.DefaultRequestHeaders.Add(AuthorizationTitle, Token);
                var resultDriving = clientDriving.PostAsync(DrivingInfoApiMethod.AddDriving, httpContent).Result;
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

        public async Task<Response> DeleteDriving(int id)
        {
            try
            {
                var clientDriving = new HttpClient();
                clientDriving.DefaultRequestHeaders.Add(AuthorizationTitle, Token);
                var resultDriving = clientDriving.DeleteAsync(string.Format(DrivingInfoApiMethod.DeleteDriving, id)).Result;
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


        public async Task<Response> ReportedOutDated(int idDriving, DrivingInfoEditModel editModel)
        {
            try
            {
                var clientDriving = new HttpClient();
                var driving = Task.Run(() => JsonConvert.SerializeObject(editModel)).Result;
                var httpContent = new StringContent(driving, Encoding.UTF8, FormatJson);
                clientDriving.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(FormatJson));
                clientDriving.DefaultRequestHeaders.Add(AuthorizationTitle, Token);

                var resultDriving = clientDriving.PutAsync(string.Format(DrivingInfoApiMethod.ReportedDated, idDriving), httpContent).Result;
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