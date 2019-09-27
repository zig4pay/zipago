jQuery(function ($) {

    $(document).ready(function () {

        $.validator.addMethod("validarcaracterespassword", function (value) {            
            return value.length < 8 ? false : !value.match(/([a-z].*[A-Z])|([A-Z].*[a-z])/) ? false : !value.match(/([0-9])/) ? false : true;            
        }, "La contraseña debe contener: <br \> - Por lo menos 8 caracteres.<br \> - Al menos una letra min\u00fascula y una may\u00fascula.<br \> - Por lo menos un numero.");

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
                    required: "Por favor ingrese su nueva contraseña."                    
                },
                confirmeclave: {
                    required: "Vuelva a ingresar la nueva contraseña.",
                    equalTo: "La contraseña ingresada no coincide."
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

    $('#btnRestablecer').click(function () {
        var $valid = $('#frmRestablecer').valid();
        $('#errorCaptcha').hide();

        if (!$valid) {
            return false;
        } else {
            if (!VerificarCaptcha()) {
                $('#errorCaptcha').show();
                return false;
            } else {
                console.log($("#Clave1").val());
            }
        }
    });

});