using AutoMapper.Internal;
using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Newtonsoft.Json;
using System.Text;

namespace MagicVilla_Web.Services
{
    public class BaseService : IBaseService
    {
        public APIResponse responseModel { get; set; }
        public IHttpClientFactory httpClient { get; set; }
        private readonly ITokenProvider _tokenProvider;
        protected readonly string VillaApiUrl;
        public IHttpContextAccessor _contextAccessor;
        public BaseService(IHttpContextAccessor httpContextAccessor,IConfiguration configuration,IHttpClientFactory httpClient, ITokenProvider tokenProvider)
        {
            _contextAccessor = httpContextAccessor;
            this.httpClient = httpClient;
            this.responseModel = new APIResponse();
            _tokenProvider = tokenProvider;
            VillaApiUrl = configuration.GetValue<string>("ServiceUrls:VillaAPI");

        }

        public async Task<T> SendAsync<T>(APIRequest apiRequest, bool withBearer = true)
        {
            try
            {
                var client = httpClient.CreateClient("MagicAPI");

                var messageFactory = () =>
                {
                    HttpRequestMessage message = new();
                    if (apiRequest.ContentType == SD.ContentType.MultipartFormData)
                    {
                        message.Headers.Add("Accept", "*/*");
                    }
                    else
                    {
                        message.Headers.Add("Accept", "application/json");
                    }
                    message.RequestUri = new Uri(apiRequest.Url);
                    if (withBearer && _tokenProvider.GetToken() != null)
                    {
                        var token = _tokenProvider.GetToken();
                        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token.AccessToken);
                    }

                    if (apiRequest.ContentType == SD.ContentType.MultipartFormData)
                    {
                        var content = new MultipartFormDataContent();
                        foreach (var prop in apiRequest.Data.GetType().GetProperties())
                        {

                            var value = prop.GetValue(apiRequest.Data);
                            if (value is FormFile)
                            {
                                var file = (FormFile)value;
                                if (file != null)
                                {
                                    content.Add(new StreamContent(file.OpenReadStream()), prop.Name, file.FileName);
                                }
                            }
                            else
                            {
                                content.Add(new StringContent(value == null ? "" : value.ToString()), prop.Name);
                            }

                        }
                        message.Content = content;
                    }
                    else
                    {
                        if (apiRequest.Data != null)
                        {
                            message.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Data),
                                Encoding.UTF8, "application/json");
                        }
                    }
                    switch (apiRequest.ApiType)
                    {
                        case SD.ApiType.POST:
                            message.Method = HttpMethod.Post;
                            break;
                        case SD.ApiType.PUT:
                            message.Method = HttpMethod.Put;
                            break;
                        case SD.ApiType.DELETE:
                            message.Method = HttpMethod.Delete;
                            break;
                        default:
                            message.Method = HttpMethod.Get;
                            break;
                    }
                    return message;

                };


                HttpResponseMessage apiResponse = null;




                apiResponse = await SendWithRefreshTokenAsync(client, messageFactory, withBearer);
                var apiContent = await apiResponse.Content.ReadAsStringAsync();
                try
                {
                    APIResponse ApiResponse = JsonConvert.DeserializeObject<APIResponse>(apiContent);
                    if (ApiResponse != null && (apiResponse.StatusCode == System.Net.HttpStatusCode.BadRequest ||
                        apiResponse.StatusCode == System.Net.HttpStatusCode.NotFound))
                    {
                        ApiResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                        ApiResponse.IsSuccess = false;
                        var res = JsonConvert.SerializeObject(ApiResponse);
                        var returnObj = JsonConvert.DeserializeObject<T>(res);
                        return returnObj;
                    }
                }
                catch (Exception ex)
                {


                    var exceptionResponse = JsonConvert.DeserializeObject<T>(apiContent);
                    return exceptionResponse;
                }
                var APIResponse = JsonConvert.DeserializeObject<T>(apiContent);
                return APIResponse;



            }
            catch (Exception ex)
            {

                var dto = new APIResponse
                {
                    ErrorMessages = new List<string> { Convert.ToString(ex.Message) },
                    IsSuccess = false
                };
                var res = JsonConvert.SerializeObject(dto);
                var APIResponse = JsonConvert.DeserializeObject<T>(res);
                return APIResponse;
            }
        }

        private async Task<HttpResponseMessage> SendWithRefreshTokenAsync(HttpClient httpClient,
            Func<HttpRequestMessage> httpRequestMessageFactory, bool withBearer = true)
        {
            if (!withBearer)
            {
                return await httpClient.SendAsync(httpRequestMessageFactory());
            }
            else
            {
                TokenDTO tokenDTO = _tokenProvider.GetToken();
                if (tokenDTO != null && string.IsNullOrEmpty(tokenDTO.AccessToken))
                {
                    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenDTO.AccessToken);

                }

                try
                {
                    var response=await httpClient.SendAsync(httpRequestMessageFactory());
                    if (response.IsSuccessStatusCode)
                    {
                        return response;
                    }

                    // if everything is not vlid to call end point to pass refresh token

                    return response;
                }
                catch (Exception ex) {

                    throw;
                }
            }
        }

        private async Task InvokeRefreshTokenEndpoint(HttpClient client,
            string existingAccessToken,string existingRefreshToken)
        {
            HttpRequestMessage message = new();
            message.Headers.Add("Accept", "application/json");
            message.RequestUri = new Uri($"{VillaApiUrl}/api/{SD.CurrentAPIVersion}/UsersAuth/refresh");
            message.Method = HttpMethod.Post;
            message.Content = new StringContent(JsonConvert.SerializeObject(new TokenDTO()
            {
                AccessToken = existingAccessToken,
                RefreshToken = existingRefreshToken
            }), Encoding.UTF8, "application/json");

            var response=await client.SendAsync(message);
            var content =await response.Content.ReadAsStringAsync();
            var apiResponse=JsonConvert.DeserializeObject<APIResponse>(content);
            if (apiResponse?.IsSuccess != null)
            {
                await _contextAccessor.HttpContext.SignOutAsync();
                _tokenProvider.ClearToken();
            }
            else
            {
                var tokenDataStr = JsonConvert.SerializeObject(apiResponse.Result);
                var tokenDto = JsonConvert.DeserializeObject<TokenDTO>(tokenDataStr);
                if (tokenDto != null&&!string.IsNullOrEmpty(tokenDto.AccessToken)) {

                    client.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenDto.AccessToken);

                }
            }
        }


    }
}
