using Newtonsoft.Json;
using System;
using System.Text.Json.Serialization;

namespace ZREL.ZiPago.Sitio.Web.Models.Response
{
    public class ResponseGoogleReCaptcha
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("challenge_ts")]
        public DateTime ChallengeTimestamp { get; set; }

        [JsonPropertyName("hostname")]
        public string Hostname { get; set; }

        [JsonPropertyName("error-codes")]
        public string[] Errors { get; set; }
    }
}
