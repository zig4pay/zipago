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

    $('#btnLimpiar').click(function () {
        LimpiarFormulario();
    });

    $(document).ready(function () {

        $.validator.setDefaults({});

        $.validator.addMethod("validarseleccion", function (value, element) {
            if (value === "" || value === "0" || value === "00" || value === "000" || value === "XX" || value === 0) {
                return false;
            } else {
                return true;
            }
        });

        $.validator.addMethod("validarcci", function () {

            var bancos = $("#bancosafiliados").val().split(",");
            var resultado = false;    

            if (parseInt($('#idbancozipago').val()) === 0) {
                return true;
            }

            for (var i = 0; i < bancos.length; i++) {
                if (parseInt(bancos[i]) === parseInt($('#idbancozipago').val())) {
                    resultado = true;
                }
            }

            if (!resultado) {                
                if ($("#cci").val().trim() !== "") {
                    resultado = true;
                }                
            }
            
            return resultado;
        });

        $("#numerocuenta").keypress(PermitirSoloNumeros);
        $("#cci").keypress(PermitirSoloNumeros);

        var validator = $('#frmRegistro').validate({
            rules: {                
                idbancozipago: {
                    validarseleccion: true
                },
                codigotipocuenta: {
                    validarseleccion: true
                },
                codigomoneda: {
                    validarseleccion: true
                },
                numerocuenta: "required",
                cci: {
                    validarcci: true
                }
            },
            messages: {
                idbancozipago: {
                    validarseleccion: "Por favor seleccione el Banco al cual pertenece la Cuenta Bancaria."
                },
                codigotipocuenta: {
                    validarseleccion: "Por favor seleccione el Tipo de Cuenta Bancaria."
                },
                codigomoneda: {
                    validarseleccion: "Por favor seleccione el Tipo de Moneda de la Cuenta Bancaria."
                },
                numerocuenta: {
                    required: "Por favor ingrese el Numero de la Cuenta Bancaria, sin guiones u otros caracteres."
                },
                cci: {
                    validarcci: "Por favor ingrese el Numero de Codigo de Cuenta Interbancario o CCI."
                }
            }
        });
    });

    $('#idbancozipago').on('change', function () {

        $("#labelCCI").text(" *");
        var bancos = $("#bancosafiliados").val().split(",");
        for (var i = 0; i < bancos.length; i++) {
            if (parseInt(bancos[i]) === parseInt($('#idbancozipago').val())) {                
                $("#labelCCI").text("");
                return;
            }
        }
    });

    $('#btnRegistrar').click(function () {        
        var $valid = $('#frmRegistro').valid();        
        $("#cuentabancariaexiste").hide();        
        if (!$valid) {            
            return false;
        } else {
            VerificaExisteCuentaBancaria();
        }
    });

});

function LimpiarFormulario() {
    $("#idbancozipago").val(0);
    $("#codigotipocuenta").val("00");
    $("#codigomoneda").val("00");
    $("#numerocuenta").val("");
    $("#cci").val("");
}

function VerificaExisteCuentaBancaria() {

    var CuentaBancaria = new Object;
    CuentaBancaria.IdUsuarioZiPago = $("#idusuariozipago").val();
    CuentaBancaria.IdBancoZiPago = $("#idbancozipago").val();
    CuentaBancaria.NumeroCuenta = $("#numerocuenta").val();    
    
    var DTO = { "cuentabancaria": CuentaBancaria };
    
    $.ajax(
        {
            url: 'VerificarExistenciaCuentaBancaria/',
            type: "POST",
            data: DTO,
            datatype: 'json',
            ContentType: 'application/json;utf-8'            
        })
        .done(function (resp) {
            console.debug("VerificaExisteCuentaBancaria - 1");
            $.each(resp, function (i, field) {
                if (i === "mensaje") {
                    if (field === "Existe") {
                        $("#cuentabancariaexiste").show();
                    } else {
                        swal({
                            title: "Registro de Cuenta Bancaria",
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
                title: "Registro de Cuenta Bancaria",
                text: "Se ha producido un error al validar la Cuenta Bancaria, por favor intentelo en unos minutos.",
                type: "error",
                confirmButtonText: "OK",
                confirmButtonClass: 'btn text-white bg-button-acept'
            });
        });
}

function Registrar() {

    var cuentas = new Array();
    var cuentaBancaria = new Object();

    cuentaBancaria.IdUsuarioZiPago = $("#idusuariozipago").val();
    cuentaBancaria.IdBancoZiPago = $("#idbancozipago").val();    
    cuentaBancaria.CodigoTipoCuenta = $("#codigotipocuenta").val();    
    cuentaBancaria.CodigoTipoMoneda = $("#codigomoneda").val();    
    cuentaBancaria.NumeroCuenta = $("#numerocuenta").val();    
    cuentaBancaria.CCI = $("#cci").val();
        
    cuentas.push(cuentaBancaria);
    
    var DTO = { 'cuentasBancarias': cuentas };

    $.ajax(
    {
        url: 'RegistrarCuentasBancarias/',
        type: "POST",
        data: DTO,
        datatype: 'json',
        ContentType: 'application/json;utf-8'
    })
    .done(function (resp) {
        var content = JSON.parse(resp);

        swal({
            title: "Registro de Cuenta Bancaria",
            text: content.hizoError ? "Ocurrio un error al registrar las Cuentas Bancarias. Por favor intentelo en unos minutos." : "Datos registrados correctamente.",
            type: content.hizoError ? "error" : "success",
            showCancelButton: false,
            confirmButtonClass: "btn-default",
            confirmButtonText: "Ok",
            closeOnConfirm: false
        }, function () {
            window.location = "/CuentaBancaria/Index";
        });

        if (!content.hizoError) {
            LimpiarFormulario();
        }
    })
    .fail(function (err) {
        swal({
            title: "Registro de Cuenta Bancaria",
            text: "Ocurrio un error al registrar las Cuentas Bancarias. Por favor intentelo en unos minutos.",
            type: "error",
            showCancelButton: false,
            confirmButtonClass: "btn-default",
            confirmButtonText: "Ok",
            closeOnConfirm: false
        });
    });
}