jQuery(function ($) {

    $.validator.addMethod("validarcorreo", function (value) {
        var respuesta = true;
        var reg = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        var regOficial = /^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$/;

        respuesta = reg.test(value) && regOficial.test(value) ? true : false;
        return respuesta;
    }, "Por favor ingrese una cuenta de correo electronica valida.");

    $.validator.addMethod("validarseleccion", function (value) {
        var respuesta = true;
        respuesta = parseInt(value) === 0 ? false : true;
        return respuesta;
    }, "Por favor primero seleccione un Banco y a continuacion seleccione una Cuenta Bancaria.");
    
    
    $(document).ready(function () {
        
        $.validator.setDefaults({});

        $("#frmRegistro").validate({
            rules: {
                codigocomercio: "required",
                correonotificacion: {
                    required: true,
                    validarcorreo: true
                },
                descripcioncomercio: "required",
                cuentasxbanco: "validarseleccion"
            },
            messages: {                
                codigocomercio: "Por favor ingrese un Identificador (ID) de Comercio.",
                correonotificacion: {
                    required: "Por favor ingrese una cuenta de correo electrónica para realizar las notificaciones."
                },
                descripcioncomercio: "Por favor ingrese la Descripcion del Comercio."
            }
        });

        $('#codigocomercio').keypress(PermitirSoloLetrasyNumeros);

        $('#idbancozipago').on('change', function () {
            var intIdUsuarioZiPago = $('#idusuariozipago').val();
            var intIdBancoZiPago = $(this).val();

            $("#cuentasxbanco").empty();
            $.getJSON("ListarCuentasBancarias", { idUsuarioZiPago: intIdUsuarioZiPago, idBancoZiPago: intIdBancoZiPago }, function (data) {
                $("#cuentasxbanco").append($("<option>").val(0).text("Seleccione"));
                $.each(data, function (i, item) {
                    $("#cuentasxbanco").append($("<option>").val(item.IdCuentaBancaria).text(item.Descripcion));
                });
            });
        });
                
    });

    $('#btnLimpiar').click(function () {
        LimpiarFormulario();
    });

    $('#btnRegistrar').click(function () {
        var $valid = $('#frmRegistro').valid();

        if (!$valid) {
            return false;
        } else {
            swal({
                title: "Registro de Comercio",
                text: "Desea registrar los datos ingresados?",
                type: "info",
                showCancelButton: true,
                confirmButtonClass: "btn-primary",
                confirmButtonText: "Si, registrar",
                cancelButtonText: "No, cancelar",
                closeOnConfirm: false
            },
                function () {
                    Registrar();
                });
        }

    });


});

function LimpiarFormulario() {
    $('#codigocomercio').val('');    
    $('#descripcioncomercio').val('');
    $('#idbancozipago').val(0);
    $('#cuentasxbanco').val(0);
}

function VerificaExisteComercio() {

    var strCodigoComercio = $("#codigocomercio").val().trim();    
    var DTO = { "strCodigoComercio": strCodigoComercio };
    
    $.ajax(
        {
            url: 'VerificarExisteComercioZiPago/',
            type: "GET",
            data: DTO,
            datatype: 'json',
            ContentType: 'application/json;utf-8'
        })
        .done(function (resp) {
            $.each(resp, function (i, field) {
                if (i === "Mensaje") {
                    if (field === "Existe") {
                        $("#comercioexiste").show();
                    } else {
                        if (ValidarComercios()) {
                            AgregarComercios();
                        } else {
                            swal({
                                title: "Alerta",
                                text: "Los datos del comercio que desea anadir ya se encuentran en el listado.",
                                type: "warning",
                                confirmButtonText: "OK",
                                confirmButtonClass: 'btn text-white bg-button-acept'
                            });
                        }
                    }
                }
            });
        })
        .fail(function (err) {
            swal({
                title: "Error",
                text: "Se ha producido un error al validar el Codigo del Comercio, por favor intentelo en unos minutos.",
                type: "error",
                confirmButtonText: "OK",
                confirmButtonClass: 'btn text-white bg-button-acept'
            });
        });
}

function Registrar() {

    var comercios = new Array();
    var comercio = new Object();
        
        comercio.IdUsuarioZiPago = $('#idusuariozipago').val();
        
                    comercio.CodigoComercio = $(this).text();
        
                    comercio.Descripcion = $(this).text();
        
                    comercio.CorreoNotificacion = $(this).text();
        
                    comercio.CodigoCuenta = $(this).text();
        

        comercios.push(comercio);

    var DTO = { 'comercios': comercios };

    $.ajax(
        {
            url: 'Registrar/',
            type: "POST",
            data: DTO,
            datatype: 'json',
            ContentType: 'application/json; utf-8'
        })
        .done(function (resp) {
            var content = JSON.parse(resp);            
            if (!content.hizoError) {
                $("#tblComercios > tbody").html("");
                swal("Comercios registrados correctamente", content.mensaje, "success");
            } else {
                swal({
                    title: "Error",
                    text: "Ocurrio un error al registrar los Comercios. Por favor intentelo en unos minutos.",
                    type: "error",
                    showCancelButton: false,
                    confirmButtonClass: "btn-default",
                    confirmButtonText: "Ok",
                    closeOnConfirm: false
                });
            }            
        })
        .fail(function (err) {
            swal({
                title: "Error",
                text: "Ocurrio un error al registrar los Comercios. Por favor intentelo en unos minutos.",
                type: "error",
                showCancelButton: false,
                confirmButtonClass: "btn-default",
                confirmButtonText: "Ok",
                closeOnConfirm: false
            });
        });


}
