using System;
using System.Threading;
using ZREL.ZiPago.Sitio.Web.Utility;

namespace ZREL.ZiPago.Sitio.Web.Clients
{
    internal static class ApiClientFactory
    {
        private static Uri apiUri;

        private static readonly Lazy<ApiClient> restClient = new Lazy<ApiClient>(
                                                        () => new ApiClient(apiUri),
                                                        LazyThreadSafetyMode.ExecutionAndPublication
                                                    );

        static ApiClientFactory()
        {
            apiUri = new Uri(ApiClientSettings.ZZiPagoApiUrl);
        }

        public static ApiClient Instance
        {
            get
            {
                return restClient.Value;
            }
        }
    }
}
