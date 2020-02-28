//using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ZREL.ZiPago.Sitio.Web.Models.Response;
using ZREL.ZiPago.Sitio.Web.Utility;

namespace ZREL.ZiPago.Sitio.Web.Clients
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
            Log.InvokeAppendLog("ApiClient.GetAsync", "requestUrl: [" + requestUrl.ToString() + "]");
            var response = await httpClient.GetAsync(requestUrl, HttpCompletionOption.ResponseHeadersRead);
            //response.EnsureSuccessStatusCode();
            Log.InvokeAppendLog("ApiClient.GetAsync", "response: [" + JsonSerializer.Serialize(response) + "]");
            var data = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ResponseModel<T>>(data);
        }

        public async Task<string> GetJsonAsync(Uri requestUrl)
        {
            Log.InvokeAppendLog("ApiClient.GetJsonAsync", "requestUrl: [" + requestUrl.ToString() + "]");
            var response = await httpClient.GetAsync(requestUrl, HttpCompletionOption.ResponseHeadersRead);            
            response.EnsureSuccessStatusCode();
            Log.InvokeAppendLog("ApiClient.GetJsonAsync", "response: [" + JsonSerializer.Serialize(response) + "]");
            var data = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Serialize(data);
        }

        public async Task<ResponseModel<T>> PostAsync<T>(Uri requestUrl, T content)
        {            
            Log.InvokeAppendLog("ApiClient.PostAsync", "requestUrl: [" + requestUrl.ToString() + "]");
            var response = await httpClient.PostAsync(requestUrl.ToString(), CreateHttpContent<T>(content));
            Log.InvokeAppendLog("ApiClient.PostAsync", "response: [" + JsonSerializer.Serialize(response) + "]");            
            var data = await response.Content.ReadAsStringAsync();            
            return JsonSerializer.Deserialize<ResponseModel<T>>(data);
        }

        public async Task<string> PostJsonAsync<T>(Uri requestUrl, T content)
        {
            Log.InvokeAppendLog("ApiClient.PostJsonAsync", "requestUrl: [" + requestUrl.ToString() + "]");
            var response = await httpClient.PostAsync(requestUrl.ToString(), CreateHttpContent<T>(content));
            Log.InvokeAppendLog("ApiClient.PostJsonAsync", "response: [" + JsonSerializer.Serialize(response) + "]");
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Serialize(data);
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
            var options = new JsonSerializerOptions {
                WriteIndented = true,
                IgnoreNullValues = true                
            };
            var json = JsonSerializer.Serialize(content, options);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }

    }
}
