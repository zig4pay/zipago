﻿using System;
using System.Threading;
using ZREL.ZiPago.Aplicacion.Web.Utility;

namespace ZREL.ZiPago.Aplicacion.Web.Clients
{
    internal static class ApiClientFactory
    {
        private static Uri apiUri;

        private static Lazy<ApiClient> restClient = new Lazy<ApiClient>(
                                                                        () => new ApiClient(apiUri),
                                                                        LazyThreadSafetyMode.ExecutionAndPublication);

        static ApiClientFactory()
        {
            apiUri = new Uri(ApiClientSettings.ZZiPagoUrl);
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
