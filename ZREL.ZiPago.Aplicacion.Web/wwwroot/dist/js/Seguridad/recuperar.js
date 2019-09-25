﻿jQuery(function ($) {

    $.validator.setDefaults({
        highlight: function (element) {
            $(element).closest('.form-group').addClass('has-error');
        },
        unhighlight: function (element) {
            $(element).closest('.form-group').removeClass('has-error');
        },
        errorElement: 'span',
        errorClass: 'help-block',
        errorPlacement: function (error, element) {
            if (element.parent('.input-group').length) {
                error.insertAfter(element.parent());
            }
            else if (element.prop('type') === 'radio' && element.parent('.radio-inline').length) {
                error.insertAfter(element.parent().parent());
            }
            else if (element.prop('type') === 'checkbox' || element.prop('type') === 'radio') {
                error.insertAfter(element.parent());
            }
            else {
                error.insertAfter(element);
            }
        }
    });

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

    $(document).bind("contextmenu", function (e) {
        return false;
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

function VerificarCaptcha() {
    var response = grecaptcha.getResponse();

    if (response.length === 0) {
        return false;
    } else {
        return true;
    }
}