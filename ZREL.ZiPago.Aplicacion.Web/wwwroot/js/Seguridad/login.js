jQuery(function ($)
{
    $(document).ready(function () {

        var $validator = $("#frmLogin").validate({
            rules: {
                txtCorreo: {
                    required: true,
                    email: true
                },
                txtClave: "required"
            },
            messages: {
                txtCorreo: "Por favor ingrese una cuenta de correo electronico valida.",
                txtClave: "Por favor ingrese su contrasena."
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
        } else {
            AutenticarUsuario();
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

function AutenticarUsuario() {

    var UsuarioViewModel = new Object();
    UsuarioViewModel.Clave1 = document.getElementById('txtCorreo').value;
    UsuarioViewModel.Clave2 = document.getElementById('txtClave').value;
    var DTO = { 'model': UsuarioViewModel };

    $.ajax(
        {
            url: 'Seguridad/UsuarioAutenticar',
            type: "POST",
            data: DTO,
            datatype: 'json',
            ContentType: 'application/json;utf-8',
            beforeSend: function () {
                $('#divBtnLogin').hide();
                $('#divSpace').hide();
                $('#divProgress').show();
            },
            success: function (resp) {
                if (resp.Mensaje == "1") {
                    window.location.href = "Afiliacion/Index";
                } else {
                    alert('Error al validar usuario: ' + resp.MensajeError);
                }                
            },
            failure: function (data) {
                alert('Error al validar usuario:' + data.responseText);
            },
            error: function (data) {
                alert('Error al validar usuario:' + data.responseText);
            },
            complete: function () {
                $('#divBtnLogin').show();
                $('#divSpace').show();
                $('#divProgress').hide();
            }
        });

}
