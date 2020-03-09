﻿using System.Collections.Generic;
using System.Runtime.Serialization;
using ZREL.ZiPago.Entidad.Afiliacion;
using ZREL.ZiPago.Entidad.Comun;

namespace ZREL.ZiPago.Aplicacion.Web.Models.Afiliacion
{
    [DataContract]
    public class ComercioViewModel
    {
        public int IdUsuarioZiPago { get; set; }
        public int IdComercioZiPagoReg { get; set; }
        public string CodigoComercio { get; set; }
        public string Descripcion { get; set; }
        public string CorreoNotificacion { get; set; }
        public string Estado { get; set; }
        public string Activo { get; set; }
        public int CodigoCuenta { get; set; }
        //public string Clave1 { get; set; }
        public List<BancoZiPago> Bancos { get; set; }
        
    }
}
