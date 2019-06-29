using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using ZREL.ZiPago.Aplicacion.Web.Validation;

namespace ZREL.ZiPago.Aplicacion.Web.Models.Seguridad
{
    public abstract class GoogleReCaptchaModelBase
    {

        [Required]
        [GoogleReCaptchaValidation]
        [BindProperty(Name = "g-recaptcha-response")]
        public string GoogleReCaptchaResponse { get; set; }

    }
}
