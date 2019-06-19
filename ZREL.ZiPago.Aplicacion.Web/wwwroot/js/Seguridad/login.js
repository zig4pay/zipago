jQuery(function ($)
{
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
                    required: "Por favor ingrese su cuenta de correo electronico.",
                    email: "Por favor ingrese una cuenta de correo electronico valida."
                },
                clave2: "Por favor ingrese su contrasena."
            }
        });

    });
        
    $(document).bind("contextmenu", function (e) {
        return false;
    });

    $('#btnLogin').click(function () {
        var $valid = $('#frmLogin').valid();
        if (!$valid) {            
            return false;
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