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

    $(document).ready(function () {

        var $validator = $("#frmRegistro").validate({            
            rules: {
                nombresusuario : "required",
                apellidosusuario: "required",
                clave1: {
                    required: true,
                    validarcorreo: true
                },
                clave2: {
                    required: true,                    
                    minlength: 8
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
                    required: "Por favor ingrese una cuenta de correo electrónica.",
                    validarcorreo: "Por favor ingrese una cuenta de correo electrónica válida."
                },
                clave2: {
                    required: "Por favor ingrese una contraseña.",
                    minlength: "La contraseña debe contener por lo menos 8 caracteres."                    
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

    $('#btnRegistrar').click(function () {
        var $valid = $('#frmRegistro').valid();
        $('#errorCaptcha').hide();
        
        if (!$valid) {                
            return false;
        } else {
            if (!VerificarCaptcha()) {
                $('#errorCaptcha').show();
                return false;
            } else {
                Registrar();
            }
        }
    });

    $('#btnAceptar').click(function () {
        $('#chkAcepto').prop('checked', true);
        $('#btnCancel').click();
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

function Registrar() {

    var UsuarioVM = new Object();

    UsuarioVM.Clave1 = $('#clave1').val();
    UsuarioVM.Clave2 = $('#clave2').val();
    UsuarioVM.ApellidosUsuario = $('#apellidosusuario').val();
    UsuarioVM.NombresUsuario = $('#nombresusuario').val();
    UsuarioVM.AceptoTerminos = 'S';

    var DTO = { 'model': UsuarioVM };

    $.ajax(
        {
            url: 'UsuarioRegistrar/',
            type: "POST",
            data: DTO,
            datatype: 'json',
            ContentType: 'application/json;utf-8'
        })
        .done(function (resp) {
            var data = JSON.parse(resp);
            alert(data.Mensaje);
        })
        .error(function (err) {
            $.each(resp, function (i, field) {
                if (i === "MensajeError") {
                    alert(field);
                }
            });
        });

}
