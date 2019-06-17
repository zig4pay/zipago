jQuery(function ($) {
    $(document).ready(function () {

        var $validator = $("#frmRegistro").validate({
            rules: {
                txtNombres : "required",
                txtApellidos: "required",
                txtCorreo: {
                    required: true,
                    email: true
                },
                txtClave: "required",
                txtConfirmeClave: "required",
                chkAcepto: "required"
            },
            messages: {
                txtNombres: ".",
                txtApellidos: ".",
                txtClave: "",
                txtConfirmeClave: "",
                chkAcepto:""
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

});