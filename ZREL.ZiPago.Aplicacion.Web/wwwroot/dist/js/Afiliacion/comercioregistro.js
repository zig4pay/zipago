jQuery(function ($) {

    $.validator.setDefaults({
        highlight: function (element) {
            $(element).closest('.form-control').addClass('has-error');
        },
        unhighlight: function (element) {
            $(element).closest('.form-control').removeClass('has-error');
        },
        errorElement: 'span',
        errorClass: 'help-block',
        errorPlacement: function (error, element) {
            if (element.parent('.form-group').length) {
                error.insertAfter(element);
            }
            else if (element.prop('type') === 'radio') {
                error.appendTo($('#divGroupSexo'));
            }
            else if (element.prop('type') === 'checkbox') {
                error.appendTo(element.parent().parent());
            }
            else {
                error.insertAfter(element);
            }
        }
    });

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

        $('#CodigoCuenta').on('change', function () {            
            var intIdCuentaBancaria = $(this).val();
            console.debug(intIdCuentaBancaria);
        });

        console.debug($("#idcomerciozipago").val());
        if ($("#idcomerciozipago").val() > 0) {
            $("[name='codigocomercio']").prop('disabled', true);
        }
        
    });

    $(document).on('change', '[data-cascade-combo]', function (event) {

        var id = $(this).attr('data-cascade-combo');
        console.debug(id);
        var url = $(this).attr('data-cascade-combo-source');
        console.debug(url);
        var paramName = $(this).attr('data-cascade-combo-param-name');

        var data = {};
        data[paramName] = id;

        $.ajax({
            url: url,
            data: {
                idUsuarioZiPago: $('#idusuariozipago').val(),
                idBancoZiPago: $(this).val()
            }
        }).done(function (data) {
            $(id).html('');            
            $.each(data,
                function (index, type) {
                    var content = '<option value="' + type.value + '">' + type.text + '</option>';
                    $(id).append(content);
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
            VerificaExisteComercio();
        }

    });
       
});

function PermitirSoloLetras(e) {
    var regex = new RegExp("^[a-zA-Z \b%.]+$");
    var key = String.fromCharCode(!e.charCode ? e.which : e.charCode);
    if (!regex.test(key)) {
        e.preventDefault();
        return false;
    }
}

function LimpiarFormulario() {
    $('#codigocomercio').val('');    
    $('#descripcioncomercio').val('');
    $('#IdBancoZiPago').val(0);
    $('#CodigoCuenta').val(0);
}

function VerificaExisteComercio() {

    var strCodigoComercio = $("#codigocomercio").val().trim();    
    var DTO = { "strCodigoComercio": strCodigoComercio };
    
    $.ajax(
        {
            url: 'Registrar/VerificarExisteComercioZiPago/',
            type: "GET",
            data: DTO,
            datatype: 'json',
            ContentType: 'application/json;utf-8'
        })
        .done(function (resp) {
            $.each(resp, function (i, field) {
                if (i === "mensaje") {
                    if (field === "Existe") {
                        $("#comercioexiste").show();
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
                }
            });
        })
        .fail(function (err) {
            swal({
                title: "Registro de Comercio",
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
    comercio.CodigoComercio = $("#codigocomercio").val();
    comercio.Descripcion = $("#descripcioncomercio").val();
    comercio.CorreoNotificacion = $("#correonotificacion").val();
    comercio.CodigoCuenta = $("#CodigoCuenta").val();
    console.debug("registrar");
    console.debug($("#CodigoCuenta").val());
    comercios.push(comercio);

    var DTO = { 'comercios': comercios };

    $.ajax(
        {
            url: 'RegistrarComercio/',
            type: "POST",
            data: DTO,
            datatype: 'json',
            ContentType: 'application/json; utf-8'
        })
        .done(function (resp) {
            var content = JSON.parse(resp);            

            swal({
                title: "Registro de Comercios",
                text: content.hizoError ? "Ocurrio un error al registrar el Comercio. Por favor intentelo en unos minutos." : "Datos registrados correctamente.",
                type: content.hizoError ? "error" : "success",
                showCancelButton: false,
                confirmButtonClass: "btn-default",
                confirmButtonText: "Ok",
                closeOnConfirm: false
            }, function () {
                window.location = "/Comercio/Index";
            });

            if (!content.hizoError) {
                LimpiarFormulario();
            }

        })
        .fail(function (err) {
            swal({
                title: "Registro de Comercio",
                text: "Ocurrio un error al registrar el Comercio. Por favor intentelo en unos minutos.",
                type: "error",
                showCancelButton: false,
                confirmButtonClass: "btn-default",
                confirmButtonText: "Ok",
                closeOnConfirm: false
            });
        });


}
