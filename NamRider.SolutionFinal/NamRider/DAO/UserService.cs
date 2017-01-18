using NamRider.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using static NamRider.Util.ApiConstants;

namespace NamRider.DAO
{
    public class UserService
    {
        public async Task<Response> RegisterUser(UserInput userInput)
        {
            Response response = new Response();
            try
            {
                var clientRegister = new HttpClient();
                var myUser = JsonConvert.SerializeObject(userInput);
                var buffer = Encoding.UTF8.GetBytes(myUser);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue(FormatJson);
                clientRegister.BaseAddress = new Uri(UserApiMethod.Register);
                var resultRegister = clientRegister.PostAsync(UserApiMethod.Register, byteContent).Result;
                if (resultRegister.IsSuccessStatusCode)
                {
                    string user = UsernameToken + userInput.UserName + PasswordToken + userInput.Password + GrantTypeToken;
                    var stringContentToken = new StringContent(user.ToString());
                    var clientToken = new HttpClient();

                    clientToken.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(FormatToken));
                    var resultToken = clientToken.PostAsync(TokenApi, stringContentToken).Result;
                    if (resultToken.IsSuccessStatusCode)
                    {
                        var resultTokenContent = await resultToken.Content.ReadAsStringAsync();
                        var r = JObject.Parse(resultTokenContent);
                        Token = r.SelectToken("token_type").Value<string>() + " " + r.SelectToken("access_token").Value<string>();
                        Username = r.SelectToken("userName").Value<string>();
                        response.IsSuccess = resultToken.IsSuccessStatusCode;

                        UsernameModel usernameModel = new UsernameModel() { UserName = Username };
                        var userReturn = this.GetUserByUsername(usernameModel);
                        UserId = userReturn.UserId;
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMsg = ex.Message;
                return response;
            }
        }

        public async Task<Response> LoginUser(UserLogin userLogin)
        {
            Response response = new Response();
            try
            {
                UsernameModel usernameModel = new UsernameModel() { UserName = userLogin.UserName };
                var userReturn = GetUserByUsername(usernameModel);
                if (userReturn != null)
                {
                    string user = UsernameToken + userLogin.UserName + PasswordToken + userLogin.Password + GrantTypeToken;
                    var stringContent = new StringContent(user);
                    var wc = new HttpClient();
                    wc.BaseAddress = new Uri(TokenApi);
                    wc.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(FormatToken));
                    var result = wc.PostAsync(TokenApi, stringContent).Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var resultContent = await result.Content.ReadAsStringAsync();
                        var r = JObject.Parse(resultContent);
                        Token = r.SelectToken("token_type").Value<string>() + " " + r.SelectToken("access_token").Value<string>();
                        Username = userReturn.UserName;
                        UserId = userReturn.UserId;

                        response.IsSuccess = result.IsSuccessStatusCode;
                        return response;
                    }
                    response.IsSuccess = false;
                    response.ErrorMsg = "Mot de passe incorrecte";
                    return response;
                }
                response.IsSuccess = false;
                response.ErrorMsg = "Nom d'utilisateur incorrecte";
                return response;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMsg = ex.Message;
                return response;
            }
        }

        public UserModel GetUserByUsername(UsernameModel editUsername)
        {
                var clientUser = new HttpClient();
                var myUser = JsonConvert.SerializeObject(editUsername);
                var buffer = Encoding.UTF8.GetBytes(myUser);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue(FormatJson);
                clientUser.BaseAddress = new Uri(UserApiMethod.GetByUsername);
                var resultUser = clientUser.PostAsync(UserApiMethod.GetByUsername, byteContent).Result;
                var user = resultUser.Content.ReadAsStringAsync().Result;
                var userReturn = JsonConvert.DeserializeObject<UserModel>(user);
                return userReturn;
        }

        public async Task<UserModel> GetUserByEmail(UserEmailModel emailInput)
        {
                var clientUser = new HttpClient();
                var myUser = JsonConvert.SerializeObject(emailInput);
                var buffer = Encoding.UTF8.GetBytes(myUser);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue(FormatJson);
                clientUser.BaseAddress = new Uri(UserApiMethod.GetByEmail);
                var resultUser = clientUser.PostAsync(UserApiMethod.GetByEmail, byteContent).Result;
                var user = await resultUser.Content.ReadAsStringAsync();
                var userReturn = JsonConvert.DeserializeObject<UserModel>(user);
                return userReturn;
        }
    }
}
