jQuery(function ($) {

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

        var $validator = $("#frmRestablecer").validate({
            rules: {                
                clave2: {
                    required: true,
                    validarcaracterespassword: true
                },
                confirmeclave: {
                    required: true,
                    equalTo: "#clave2"
                }
            },
            messages: {                
                clave2: {
                    required: "Por favor ingrese su nueva contraseña.",
                    validarcaracterespassword:
                        "La contraseña debe contener: <br \> - Por lo menos 8 caracteres.<br \> - Al menos una letra min\u00fascula y una may\u00fascula.<br \> - Por lo menos un numero."
                },
                confirmeclave: {
                    required: "Vuelva a ingresar la nueva contraseña.",
                    equalTo: "La contraseña ingresada no coincide."
                }
            }
        });

    });

    $(document).bind("contextmenu", function (e) {
        return false;
    });

    $('#btnRestablecer').click(function () {
        var $valid = $('#frmRestablecer').valid();
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