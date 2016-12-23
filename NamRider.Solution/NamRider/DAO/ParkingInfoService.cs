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
    public class ParkingInfoService
    {
        public async Task<List<ParkingInfoModel>> GetAllParking()
        {
                var clientAllPark = new HttpClient();
                var resultAllPark = clientAllPark.GetAsync(ParkingInfoApiMethod.GetAllParkingInfo).Result;
                var allPark = await resultAllPark.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<ParkingInfoModel>>(allPark);
            
        }

        public async Task<ParkingInfoModel> GetParkingById(int id)
        {
                var clientPark = new HttpClient();
                var resultPark = clientPark.GetAsync(string.Format(ParkingInfoApiMethod.GetParkingById, id)).Result;
                var park = await resultPark.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ParkingInfoModel>(park);
        }

        public async Task<Response> AddParking(ParkingInfoInputModel inputModel)
        {
            try
            {
                var clientPark = new HttpClient();
                var parking = Task.Run(() => JsonConvert.SerializeObject(inputModel)).Result;
                var httpContent = new StringContent(parking, Encoding.UTF8, FormatJson);
                clientPark.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(FormatJson));
                clientPark.DefaultRequestHeaders.Add(AuthorizationTitle, Token);
                var resultPark = clientPark.PostAsync(ParkingInfoApiMethod.AddParking, httpContent).Result;
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

        public async Task<Response> DeleteParking(int id)
        {
            try
            {
                var clientPark = new HttpClient();
                clientPark.DefaultRequestHeaders.Add(AuthorizationTitle, Token);
                var resultPark = clientPark.DeleteAsync(string.Format(ParkingInfoApiMethod.DeleteParking, id)).Result;
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

        public async Task<List<ParkingInfoModel>> GetParkingByType(ParkingInfoFilterModel inputModel)
        {
                var clientPark = new HttpClient();
                var parking = Task.Run(() => JsonConvert.SerializeObject(inputModel)).Result;
                var httpContent = new StringContent(parking, Encoding.UTF8, FormatJson);
                clientPark.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(FormatJson));
                var resultPark = clientPark.PostAsync(ParkingInfoApiMethod.GetParkingByType, httpContent).Result;
                var listPark = await resultPark.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<ParkingInfoModel>>(listPark);
        }

        public async Task<ParkingInfoModel> GetParkingByPointGeographic(GeographicPointModel point)
        {
                var clientPark = new HttpClient();
                var myPoint = JsonConvert.SerializeObject(point);
                var buffer = Encoding.UTF8.GetBytes(myPoint);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue(FormatJson);
                clientPark.BaseAddress = new Uri(ParkingInfoApiMethod.GetParkingByPointGeographic);
                var resultUser = clientPark.PostAsync(ParkingInfoApiMethod.GetParkingByPointGeographic, byteContent).Result;
                var park = await resultUser.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ParkingInfoModel>(park);
        }

        public async Task<Response> ReportedOutDated(int idParking, ParkingInfoEditModel editModel)
        {
            try
            {
                var clientDriving = new HttpClient();
                var driving = Task.Run(() => JsonConvert.SerializeObject(editModel)).Result;
                var httpContent = new StringContent(driving, Encoding.UTF8, FormatJson);
                clientDriving.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(FormatJson));
                clientDriving.DefaultRequestHeaders.Add(AuthorizationTitle, Token);

                var resultDriving = clientDriving.PutAsync(string.Format(ParkingInfoApiMethod.ReportedDated, idParking), httpContent).Result;
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

