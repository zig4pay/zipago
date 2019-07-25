using Newtonsoft.Json;
using System;

namespace ZREL.ZiPago.Sitio.Web.Models.Response
{
    public class ResponseGoogleReCaptcha
    {
        [JsonProperty(PropertyName = "success")]
        public bool Success { get; set; }

        [JsonProperty(PropertyName = "challenge_ts")]
        public DateTime ChallengeTimestamp { get; set; }

        [JsonProperty(PropertyName = "hostname")]
        public string Hostname { get; set; }

        [JsonProperty(PropertyName = "error-codes")]
        public string[] Errors { get; set; }
    }
}
