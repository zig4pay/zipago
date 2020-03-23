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

    $.validator.addMethod("validarcorreo", ValidarCorreo);
    $.validator.addMethod("validarcaracterespassword", ValidarCaracteresPassword);
    
    $(document).ready(function () {

        $("#frmRegistro").validate({            
            rules: {
                nombresusuario : "required",
                apellidosusuario: "required",
                clave1: {
                    required: true,
                    validarcorreo: true
                },
                clave2: {
                    required: true,
                    validarcaracterespassword: true
                },
                confirmeclave: {
                    required: true,
                    equalTo: "#clave2"
                },
                chkAcepto: {
                    required: true
                }
            },
            messages: {
                nombresusuario: "Por favor ingrese sus nombres.",
                apellidosusuario: "Por favor ingrese sus apellidos.",
                clave1: {
                    required: "Por favor ingrese una cuenta de correo electrónico.",
                    validarcorreo: "Por favor ingrese una cuenta de correo electrónico válido."
                },
                clave2: {
                    required: "Por favor ingrese una contraseña.",                    
                    validarcaracterespassword:
                        "La contraseña debe contener: <br \> - Por lo menos 8 caracteres.<br \> - Al menos una letra min\u00fascula y una may\u00fascula.<br \> - Por lo menos un numero."
                },                
                confirmeclave: {
                    required: "Por favor ingrese nuevamente su contraseña.",
                    equalTo: "La contraseña ingresada no coincide."
                },
                chkAcepto: "Es necesario que revise y acepte los términos y condiciones."
            }
        });

    });

    $(document).bind("contextmenu", function (e) {
        return false;
    });

    $('#btnCancelar').click(function () {
        $(location).attr('href', $('#UrlSitioWeb').val());
    });

    $('#btnRegistrar').click(function () {
        var $valid = $('#frmRegistro').valid();
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

    $('#btnAceptar').click(function () {
        $('#chkAcepto').prop('checked', true);
        $('#btnCancel').click();
    });    

    $('#nombresusuario').keypress(PermitirSoloLetras);
    $('#apellidosusuario').keypress(PermitirSoloLetras);

    function PermitirSoloLetras(e) {
        var regex = new RegExp("^[a-zA-Z \b]+$");
        var key = String.fromCharCode(!e.charCode ? e.which : e.charCode);
        if (!regex.test(key)) {
            e.preventDefault();
            return false;
        }
    }

});

function VerificarCaptcha() {
    var response = grecaptcha.getResponse();

    if (response.length === 0) {
        return false;
    } else {
        return true;
    }
}

function ValidarCorreo(mail) {
    var respuesta = true;
    var reg = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    var regOficial = /^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$/;

    if (reg.test(mail) && regOficial.test(mail)) {
        respuesta = true;
    }
    else {
        respuesta = false;
    }
    return respuesta;
}

function ValidarCaracteresPassword(valor) {
    var respuesta = true;

    respuesta = valor.length < 8 ? false : !valor.match(/([a-z].*[A-Z])|([A-Z].*[a-z])/) ? false : !valor.match(/([0-9])/) ? false : true;    

    return respuesta;
}
