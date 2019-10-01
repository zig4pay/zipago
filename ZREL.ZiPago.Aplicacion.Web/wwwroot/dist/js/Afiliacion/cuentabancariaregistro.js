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
                numerocuenta: "required"
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
                }                
            }
        });
    });

    $('#btnRegistrar').click(function () {
        var $valid = $('#frmRegistro').valid();

        if (!$valid) {
            return false;
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
    });

});

function LimpiarFormulario() {
    $("#idbancozipago").val(0);
    $("#codigotipocuenta").val("00");
    $("#codigomoneda").val("00");
    $("#numerocuenta").val("");
    $("#cci").val("");
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