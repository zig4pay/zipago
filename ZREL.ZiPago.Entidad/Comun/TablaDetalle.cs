﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ZREL.ZiPago.Entidad.Comun
{
    public class TablaDetalle
    {

        public TablaDetalle()
        {

        }
                
        public string Cod_Tabla { get; set; }
        public string Valor { get; set; }
        public string Descr_Valor { get; set; }

    }
}
