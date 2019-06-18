jQuery(function ($) {
    $(document).ready(function () {

        var $validator = $("#frmRegistro").validate({
            rules: {
                nombresusuario : "required",
                apellidosusuario: "required",
                clave1: {
                    required: true,                    
                    email: true
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
                    required: "Por favor ingrese una cuenta de correo electronica.",
                    email: "Por favor ingrese una cuenta de correo electronica valida."
                },
                clave2: {
                    required: "Por favor ingrese una contrasena.",
                    minlength: "La contrasena debe contener por lo menos 8 caracteres."                    
                },                
                confirmeclave: {
                    equalTo: "La contrasena ingresada no coincide."
                },
                chkAcepto:"Es necesario que revise y acepte los terminos y condiciones."
            }
        });

    });

    $(document).bind("contextmenu", function (e) {
        return false;
    });

    $('#btnRegistrar').click(function () {
        var $valid = $('#frmRegistro').valid();
        if (!$valid) {
            return false;        
        }
    });

});