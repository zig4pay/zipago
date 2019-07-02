using Newtonsoft.Json.Linq;
using NLog;
using System.Net;
using System.Net.Http;

namespace ZREL.ZiPago.Aplicacion.Web.Utility
{
    public class GoogleReCaptchaValidation
    {

        public static bool ReCaptchaPassed(string gRecaptchaResponse, string secret, Logger logger)
        {
            HttpClient httpClient = new HttpClient();
            var res = httpClient.GetAsync($"https://www.google.com/recaptcha/api/siteverify?secret={secret}&response={gRecaptchaResponse}").Result;
            if (res.StatusCode != HttpStatusCode.OK)
            {
                logger.Error("Error al enviar request a Google ReCaptcha.");
                return false;
            }

            string JSONres = res.Content.ReadAsStringAsync().Result;
            dynamic JSONdata = JObject.Parse(JSONres);
            if (JSONdata.success != "true")
            {
                return false;
            }

            return true;
        }

    }
}
