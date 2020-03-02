namespace ZREL.ZiPago.Libreria
{
    public static class Constantes
    {
        public enum RegistroUsuario {
            UsuarioRegistradoCorrectamente = 1,
            UsuarioYaExiste = 2,
            ErrorAlRegistrar = 3
        }

        public enum EstadoComercio
        {
            PendienteDeActivar = 1,
            Activo = 2
        }


        //[TD_TABLA_TABLAS]
        public const string strCodTablaRubroNegocio = "RUBRO_NEGOCIO";

        public const string strCodTablaTipoCuenta = "TIPO_CUENTA";

        public const string strCodTablaTipoMoneda = "TIPO_MONEDA";

        public const string strCodTablaTipoPersona = "TIPO_PERSONA";
        public const string strTipoPersonaJuridica = "01";

        public const string strCodTablaTipoDocIdentidad = "TIPO_DOC_ID";
        public const string strTipoDocIdDNI_Codigo = "01";
        public const string strTipoDocIdRUC_Codigo = "02";
        public const string strTipoDocIdRUC_Descripcion = "RUC - Registro Unico de Contribuyente";

        public const string strCodTablaEstadoTransaccion = "ESTADO_TXN";

        public const string strCodTablaServiciosRecaudacion = "SERVICIO_ZIPAGO";

        public const string strValor_Activo = "S";
        public const string strValor_NoActivo = "N";

        //SEGURIDAD_USUARIO_REGISTRAR
        public const string strMensajeUsuarioIncorrecto = "El usuario o contraseña ingresados son incorrectos.";
        public const string strMensajeUsuarioRegistroCorrecto = "Usuario {0} registrado correctamente. A continuacion sera redirigido a nuestro portal web.";
        public const string strMensajeUsuarioNoRegistrado = "El Usuario ingresado no se encuentra registrado.";
        public const string strMensajeUsuarioYaExiste = "El Id ZiPago {0} ya existe. Intente registrarse con otra cuenta de correo electronico.";
        public const string strMensajeUsuarioError = "Error al intentar registrar el usuario.";
        public const string strMensajeErrorValidarCaptcha = "No hemos podido validar que no seas un robot.";
        public const string strMensajeDatosIncorrectos = "No se ha ingresado correctamente todos los datos solicitados.";
        public const string strMensajeEnvioMail = "Envio de correo electrónico realizado.";
        public const string strMensajeErrorEnvioMail = "No se pudo realizar el envio del correo electrónico.";


        public const string strMensajeContrasenaRestablecida = "La contraseña ha sido restablecida correctamente.";
        public const string strMensajeErrorRestablecerContrasena = "No se pudo restablecer la contraseña, por favor intente en unos minutos.";
        public const string strMensajeEnvioEnlace = "Se realizo el envio de un enlace a su correo electrónico para que pueda restablecer su contraseña.";
        public const string strMensajeErrorEnvioEnlace = "No se pudo enviar a su correo electrónico el procedimiento para que pueda restablecer su contraseña, por favor intente en unos minutos.";

        public const string strUsuarioZiPago_AceptoTerminos = "S";        
        public const string strEstadoRegistro_Nuevo = "N";
        public const string strEstadoRegistro_ConDatosPersonales = "R";
        public const string strEstadoRegistro_DatosActualizados = "A";

        //UBIGEO
        public const string strUbigeoZiPago_Departamentos = "00";

        public const string strConsultaRealizada = "Consulta realizada correctamente.";
        public const string strRegistroRealizado = "Registro realizado correctamente.";

        //AFILIACION
        public const string strComerciosTexto = "Comercios registrados";
        public const string strCuentasBancariasTexto = "Cuentas Bancarias registradas";
        public const string strTransaccionesTexto = "Transacciones registradas";
        public const string strMontoPagosTexto = "Pagos registrados";

        //ESTADOS COMERCIO
        public const string strEstadoComercio_Pendiente = "Pendiente de Activar";
        public const string strEstadoComercio_Activo = "Activo";

        //BANCOS AFILIADOS
        public const string strBancosAfiliados_Codigos = "[2],[5],[7],[9]";

        //Google ReCaptcha
        public const string GoogleRecaptchaSecretKey = "6Le0OasUAAAAALW4hoX9kXI_61sI-lVxim3MTBlc";
        public const string GoogleRecaptchaSiteKey = "6Le0OasUAAAAALRk-9Pue53r-TIJib5dWqnHX7w6";

        public const string ZiPago_Session = "ZiPago.Session";
    }
}
