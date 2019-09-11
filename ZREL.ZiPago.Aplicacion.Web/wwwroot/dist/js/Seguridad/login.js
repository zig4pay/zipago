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

        var $validator = $("#frmLogin").validate({
            rules: {
                clave1: {
                    required: true,
                    email: true
                },
                clave2: "required"
            },
            messages: {
                clave1: {
                    required: "Por favor ingrese su cuenta de correo electrónico.",
                    email: "Por favor ingrese una cuenta de correo electrónico válida."
                },
                clave2: "Por favor ingrese su contraseña."
            }
        });

        

    });

    $(document).bind("contextmenu", function (e) {
        return false;
    });

    $('#btnLogin').click(function () {
        var $valid = $('#frmLogin').valid();
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

    $('#btnOlvidoClave').click(function () {
        MostrarRecuperarClave(true);
    });

    $('#btnRegresaLogin').click(function () {
        MostrarRecuperarClave(false);
    });

});

function MostrarRecuperarClave(valor) {
    if (valor) {
        $('#olvidoClaveBox').show();
        $('#loginBox').hide();
    } else {
        $('#olvidoClaveBox').hide();
        $('#loginBox').show();
    }
}

function VerificarCaptcha() {
    var response = grecaptcha.getResponse();

    if (response.length === 0) {
        return false;
    } else {
        return true;
    }
}