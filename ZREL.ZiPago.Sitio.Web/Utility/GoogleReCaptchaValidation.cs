using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using ZREL.ZiPago.Sitio.Web.Models.Response;

namespace ZREL.ZiPago.Sitio.Web.Utility
{
    public class GoogleReCaptchaValidation
    {

        public async static Task<bool> ReCaptchaPassed(string gRecaptchaResponse, string secret)
        {
            HttpClient httpClient = new HttpClient();

            try
            {
                var res = httpClient.GetAsync($"https://www.google.com/recaptcha/api/siteverify?secret={secret}&response={gRecaptchaResponse}").Result;
                if (res.StatusCode != HttpStatusCode.OK)
                {
                    Log.InvokeAppendLogError("GoogleReCaptchaValidation.ReCaptchaPassed", "Error al enviar request a Google ReCaptcha.");                    
                    return false;
                }

                string JSONres = await res.Content.ReadAsStringAsync();
                Log.InvokeAppendLog("GoogleReCaptchaValidation.ReCaptchaPassed", "Response.Content [" + JSONres + "]");

                ResponseGoogleReCaptcha response = new ResponseGoogleReCaptcha();
                response = JsonSerializer.Deserialize<ResponseGoogleReCaptcha>(JSONres);
                Log.InvokeAppendLogError("GoogleReCaptchaValidation.ReCaptchaPassed", "ResponseGoogleReCaptcha [" + JsonSerializer.Serialize(response) + "]");

                if (!response.Success)
                {
                    if (response.Errors != null) {
                        string errores = string.Join(" | ", response.Errors);
                        Log.InvokeAppendLogError("GoogleReCaptchaValidation.ReCaptchaPassed", "Error(es): " + errores);
                    }                    
                    return false;
                }
            }
            catch (Exception ex)
            {
                Log.InvokeAppendLogError("GoogleReCaptchaValidation.ReCaptchaPassed", "Exception: " + ex.ToString());
                return false;
            }

            return true;
        }

    }
}
