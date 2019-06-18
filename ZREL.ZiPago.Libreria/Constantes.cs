using System;
using System.Collections.Generic;
using System.Text;

namespace ZREL.ZiPago.Libreria
{
    public static class Constantes
    {
        public const string strMensajeUsuarioIncorrecto = "El usuario o contrasena ingresados son incorrectos.";
        public const string strMensajeUsuarioRegistrado = "Usuario registrado correctamente.";
        public const string strMensajeUsuarioYaExiste = "El Id ZiPago {0} ya se encuentra registrado.";
        public const string strMensajeUsuarioError = "Error al intentar registrar el usuario.";

        public const string strUsuarioZiPago_Activo = "A";
        public const string strUsuarioZiPago_NoActivo = "N";
        public const string strUsuarioZiPago_AceptoTerminos = "S";        
    }
}
