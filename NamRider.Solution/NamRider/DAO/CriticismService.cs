using NamRider.Model;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using static NamRider.Util.ApiConstants;

namespace NamRider.DAO
{
    public class CriticismService
    {
        public async Task<List<CriticismDrivingModel>> GetAllDrivingCritism()
        {
                var clientAllDriving = new HttpClient();
                var resultAllDriving = clientAllDriving.GetAsync(CriticismDrivingApiMethod.GetAllCriticismDriving).Result;
                var allDriving = await resultAllDriving.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<CriticismDrivingModel>>(allDriving);
            
        }

        public async Task<CriticismDrivingModel> GetCritismDrivingById(string userId, int idDriving)
        {
            
                var clientDriving = new HttpClient();
                var resultDriving = clientDriving.GetAsync(string.Format(CriticismDrivingApiMethod.GetCriticismDrivingById, idDriving, userId)).Result;
                var driving = await resultDriving.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<CriticismDrivingModel>(driving);
            
        }

        public async Task<Response> AddCritismDriving(CriticismDrivingInputModel inputModel)
        {
            try
            {
                var clientDriving = new HttpClient();
                var drivingCritism = Task.Run(() => JsonConvert.SerializeObject(inputModel)).Result;
                var httpContent = new StringContent(drivingCritism, Encoding.UTF8, FormatJson);
                clientDriving.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(FormatJson));
                clientDriving.DefaultRequestHeaders.Add(AuthorizationTitle, Token);
                var resultDriving = clientDriving.PostAsync(CriticismDrivingApiMethod.AddCriticismDriving, httpContent).Result;
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

        // Critism Parking
        public async Task<List<CriticismParkingModel>> GetAllCritismParking()
        {
            
                var clientAllPark = new HttpClient();
                var resultAllPark = clientAllPark.GetAsync(CriticismParkingApiMethod.GetAllCriticismParking).Result;
                var allPark = await resultAllPark.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<CriticismParkingModel>>(allPark);
            
        }

        public async Task<CriticismParkingModel> GetCritismParkingInfoById(string userId, int idPark)
        {
            
                var clientPark = new HttpClient();
                var resultPark = clientPark.GetAsync(string.Format(CriticismParkingApiMethod.GetCriticismParkingById, idPark, userId)).Result;
                var park = await resultPark.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<CriticismParkingModel>(park);
            
        }

        public async Task<Response> AddCritismParking(CriticismParkingInputModel inputModel)
        {
            try
            {
                var clientPark = new HttpClient();
                var parkCritism = Task.Run(() => JsonConvert.SerializeObject(inputModel)).Result;
                var httpContent = new StringContent(parkCritism, Encoding.UTF8, FormatJson);
                clientPark.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(FormatJson));
                clientPark.DefaultRequestHeaders.Add(AuthorizationTitle, Token);
                var resultPark = clientPark.PostAsync(CriticismParkingApiMethod.AddCriticismParking, httpContent).Result;
                var result = await resultPark.Content.ReadAsStringAsync();
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
