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
                var res = httpClient.GetAsync($"https://www.google.com/recaptcha/api/siteverify?secret={secret}&response={gRecaptchaResponse}").Result;
                if (res.StatusCode != HttpStatusCode.OK)
                {
                    logger.Error("Error al enviar request a Google ReCaptcha.");
                    return false;
                }

                string JSONres = await res.Content.ReadAsStringAsync();
                logger.Info("ReCaptchaPassed1: JSONres[" + JSONres + "]");

                ResponseGoogleReCaptcha response = new ResponseGoogleReCaptcha();
                response = JsonConvert.DeserializeObject<ResponseGoogleReCaptcha>(JSONres);
                logger.Info("ReCaptchaPassed2: JSONres[" + JSONres + "]");

                if (!response.Success)
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString);
                return false;
            }

            return true;

        }

    }
}
