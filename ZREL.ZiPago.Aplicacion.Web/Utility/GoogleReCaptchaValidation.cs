using Newtonsoft.Json;
using NLog;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using ZREL.ZiPago.Aplicacion.Web.Models.Response;

namespace ZREL.ZiPago.Aplicacion.Web.Utility
{
    public class GoogleReCaptchaValidation
    {
        
        public async static Task<bool> ReCaptchaPassed(string gRecaptchaResponse, string secret, Logger logger)
        {
            HttpClient httpClient = new HttpClient();            

            try
            {
                logger.Info("[Aplicacion.Web.Utility.GoogleReCaptchaValidation.ReCaptchaPassed] | gRecaptchaResponse: [{0}] | Inicio.", gRecaptchaResponse);
                var res = httpClient.GetAsync($"https://www.google.com/recaptcha/api/siteverify?secret={secret}&response={gRecaptchaResponse}").Result;

                if (res.StatusCode != HttpStatusCode.OK)
                {
                    logger.Error("[Aplicacion.Web.Utility.GoogleReCaptchaValidation.ReCaptchaPassed] | Error: [Error al enviar request a Google ReCaptcha - HttpStatusCode {0}]", res.StatusCode.ToString());
                    return false;
                }

                string JSONres = await res.Content.ReadAsStringAsync();
                logger.Info("[Aplicacion.Web.Utility.GoogleReCaptchaValidation.ReCaptchaPassed] | Response Site Verify: [{0}]", JSONres);
                
                ResponseGoogleReCaptcha response = new ResponseGoogleReCaptcha();
                response = JsonConvert.DeserializeObject<ResponseGoogleReCaptcha>(JSONres);
                
                if (!response.Success)
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                logger.Error("[Aplicacion.Web.Utility.GoogleReCaptchaValidation.ReCaptchaPassed] | Excepcion: [{0}].", ex.ToString());
                return false;
            }
            return true;
        }

    }
}
