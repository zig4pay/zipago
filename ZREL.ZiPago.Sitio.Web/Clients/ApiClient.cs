using Newtonsoft.Json;
using NLog;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ZREL.ZiPago.Sitio.Web.Models.Response;

namespace ZREL.ZiPago.Sitio.Web.Clients
{
    public class ApiClient
    {

        private readonly HttpClient httpClient;
        private Uri BaseEndpoint { get; set; }

        public ApiClient(Uri baseEndpoint)
        {
            BaseEndpoint = baseEndpoint ?? throw new ArgumentNullException("baseEndpoint");
            httpClient = new HttpClient();
        }

        public async Task<ResponseModel<T>> GetAsync<T>(Uri requestUrl)
        {
            var logger = LogManager.GetCurrentClassLogger();
            var response = await httpClient.GetAsync(requestUrl, HttpCompletionOption.ResponseHeadersRead);
            //response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();
            logger.Info("GetAsync: " + data.ToString());
            return JsonConvert.DeserializeObject<ResponseModel<T>>(data);
        }

        public async Task<string> GetJsonAsync(Uri requestUrl)
        {
            var response = await httpClient.GetAsync(requestUrl, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();

            return JsonConvert.SerializeObject(data);
        }

        public async Task<ResponseModel<T>> PostAsync<T>(Uri requestUrl, T content)
        {
            var logger = LogManager.GetCurrentClassLogger();
            var response = await httpClient.PostAsync(requestUrl.ToString(), CreateHttpContent<T>(content));
            //response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();
            logger.Info("PostAsync: " + data.ToString());
            return JsonConvert.DeserializeObject<ResponseModel<T>>(data);
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
            var uriBuilder = new UriBuilder(endpoint)
            {
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
