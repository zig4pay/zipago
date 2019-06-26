using System;
using System.Collections.Generic;
using System.Text;

namespace ZREL.ZiPago.Libreria
{
    public static class Constantes
    {

        //[TD_TABLA_TABLAS]
        public const string strCodTablaRubroNegocio = "RUBRO_NEGOCIO";

        public const string strCodTablaTipoCuenta = "TIPO_CUENTA";

        public const string strCodTablaTipoMoneda = "TIPO_MONEDA";

        public const string strCodTablaTipoPersona = "TIPO_PERSONA";
        public const string strTipoPersonaJuridica = "01";

        //
        public const string strValor_Activo = "A";
        public const string strValor_NoActivo = "N";

        //SEGURIDAD_USUARIO_REGISTRAR
        public const string strMensajeUsuarioIncorrecto = "El usuario o contrasena ingresados son incorrectos.";
        public const string strMensajeUsuarioRegistrado = "Usuario registrado correctamente.";
        public const string strMensajeUsuarioNoRegistrado = "El Usuario no se encuentra registrado.";
        public const string strMensajeUsuarioYaExiste = "El Id ZiPago {0} ya se encuentra registrado.";
        public const string strMensajeUsuarioError = "Error al intentar registrar el usuario.";

        public const string strUsuarioZiPago_AceptoTerminos = "S";

        //UBIGEO
        public const string strUbigeoZiPago_Departamentos = "00";

        public const string strConsultaRealizada = "Consulta realizada correctamente.";
        public const string strRegistroRealizado = "Registro realizado correctamente.";
    }
}
