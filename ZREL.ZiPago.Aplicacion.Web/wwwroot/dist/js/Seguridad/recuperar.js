jQuery(function ($) {

    $(document).ready(function () {

        $.validator.addMethod("validarcorreo", function (value) {
            var respuesta = true;
            var reg = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
            var regOficial = /^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$/;
            respuesta = reg.test(value) && regOficial.test(value) ? true : false;
            return respuesta;
        }, "Por favor ingrese una cuenta de correo electrónica válida.");

        $("#frmRecuperar").validate({
            rules: {
                clave1: {
                    required: true,
                    validarcorreo: true
                }
            },
            messages: {
                clave1: {
                    required: "Por favor ingrese su cuenta de correo electrónico."
                }
            }
        });

    });

    $(window).on("load", function () {
        if ($('#jsparam1').val()) {
            swal({
                title: $('#jsparam2').val() ? "Error" : "Mensaje",
                text: $('#jsparam3').val(),
                type: $('#jsparam2').val() ? "error" : "success",
                showCancelButton: false,
                confirmButtonClass: "btn-default",
                confirmButtonText: "Ok",
                closeOnConfirm: false
            });
            $('#jsparam1').val(false);
        }
    });
        
    $('#btnRecuperar').click(function () {        
        var $valid = $('#frmRecuperar').valid();
        $('#errorCaptcha').hide();

        if (!$valid) {
            return false;
        } else {
            if (!VerificarCaptcha()) {
                $('#errorCaptcha').show();
                return false;
            }
        }        
    });
    
});