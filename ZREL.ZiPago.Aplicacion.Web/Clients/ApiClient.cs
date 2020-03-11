using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ZREL.ZiPago.Aplicacion.Web.Models.Response;

namespace ZREL.ZiPago.Aplicacion.Web.Clients
{
    public class ApiClient
    {

        private readonly HttpClient httpClient;
        private Uri BaseEndpoint { get; set; }

        public ApiClient(Uri baseEndpoint)
        {
            var httpClientHandler = new HttpClientHandler();
            BaseEndpoint = baseEndpoint ?? throw new ArgumentNullException("baseEndpoint");
            httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
            httpClient = new HttpClient(httpClientHandler);
        }

        public async Task<ResponseModel<T>> GetAsync<T>(Uri requestUrl)
        {            
            var response = await httpClient.GetAsync(requestUrl, HttpCompletionOption.ResponseHeadersRead);
            //response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();            
            return JsonConvert.DeserializeObject<ResponseModel<T>>(data);
        }

        public async Task<ResponseListModel<T>> GetListAsync<T>(Uri requestUrl)
        {
            var response = await httpClient.GetAsync(requestUrl, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ResponseListModel<T>>(data);
        }

        internal Uri CreateRequestUri(object p)
        {
            throw new NotImplementedException();
        }

        public async Task<string> GetJsonAsync (Uri requestUrl)
        {
            var response = await httpClient.GetAsync(requestUrl, HttpCompletionOption.ResponseHeadersRead);
            //response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();

            return JsonConvert.SerializeObject(data);
        }

        public  async Task<ResponseModel<T>> PostAsync<T>(Uri requestUrl, T content)
        {            
            var response = await httpClient.PostAsync(requestUrl.ToString(), CreateHttpContent<T>(content));
            //response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();            
            return JsonConvert.DeserializeObject<ResponseModel<T>>(data);
        }

        public async Task<ResponseListModel<T>> PostListAsync<T>(Uri requestUrl, T content)
        {
            var response = await httpClient.PostAsync(requestUrl.ToString(), CreateHttpContent<T>(content));
            //response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ResponseListModel<T>>(data);
        }

        public async Task<string> PostJsonAsync<T>(Uri requestUrl, T content)
        {
            var response = await httpClient.PostAsync(requestUrl.ToString(), CreateHttpContent<T>(content));
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();
            return JsonConvert.SerializeObject(data);
        }

        public Uri CreateRequestUri(string relativePath, string queryString = "")
        {
            var endpoint = new Uri(BaseEndpoint, relativePath);
            var uriBuilder = new UriBuilder(endpoint) {
                                    Query = queryString
                                };
            return uriBuilder.Uri;
        }

        public HttpContent CreateHttpContent<T>(T content)
        {   
            var json = JsonConvert.SerializeObject(content,
                                                Formatting.Indented,
                                                new JsonSerializerSettings
                                                {
                                                    NullValueHandling = NullValueHandling.Ignore
                                                });
            return new StringContent(json, Encoding.UTF8, "application/json");            
        }

        private static JsonSerializerSettings MicrosoftDateFormatSettings
        {
            get
            {
                return new JsonSerializerSettings
                {
                    DateFormatHandling = DateFormatHandling.MicrosoftDateFormat
                };
            }
        }
    }
}
