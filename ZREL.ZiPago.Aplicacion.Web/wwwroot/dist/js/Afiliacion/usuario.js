jQuery(function ($) {

    $(document).bind("contextmenu", function (e) {
        return false;
    });

    $('#optPersonaJuridica').change(function () {
        if ($(this).is(":checked")) {
            MostrarDivJuridica(true);
        }
    });

    $('#optPersonaNatural').change(function () {
        if ($(this).is(":checked")) {
            MostrarDivJuridica(false);
        }
    });
    
});


$(document).ready(function () {

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
            if (element.hasClass('select2') && element.next('.select2-container').length) {
                error.insertAfter(element.next('.select2-container'));
            } else if (element.parent('.input-group').length) {
                error.insertAfter(element.parent());
            }
            else if (element.prop('type') === 'radio' && element.parent('.radio-inline').length) {
                error.insertAfter(element.parent().parent());
            }
            else if (element.prop('type') === 'checkbox' || element.prop('type') === 'radio') {
                error.appendTo(element.parent().parent());
            }
            else {
                error.insertAfter(element);
            }
        }
    });

    //called when key is pressed in textbox
    $("#numeroruc").keypress(SoloNumeros);
    $("#numerodni").keypress(SoloNumeros);
    $("#telefonofijo").keypress(SoloNumeroTelefonico);
    $("#telefonomovil").keypress(SoloNumeroTelefonico);
    $("#apellidopaterno").keypress(PermitirSoloLetras);
    $("#apellidomaterno").keypress(PermitirSoloLetras);

    $('#codigodepartamento').on('change', function () {
        var strCodigoUbigeo = $(this).val();
        $("#codigoprovincia").empty();
        $("#codigodistrito").empty();
        $.getJSON("ListarPorUbigeo", { strCodigoUbigeo: strCodigoUbigeo }, function (data) {
            $("#codigoprovincia").append($("<option>").val("XX").text("Seleccione"));
            $.each(data, function (i, item) {
                $("#codigoprovincia").append($("<option>").val(item.codigoUbigeo).text(item.nombre));
            });
        });        
    });
    
    $("#codigoprovincia").on("change", function () {
        var strCodigoUbigeo = $(this).val();
        $("#codigodistrito").empty();
        $.getJSON("ListarPorUbigeo", { strCodigoUbigeo: strCodigoUbigeo }, function (data) {
            $("#codigodistrito").append($("<option>").val("XX").text("Seleccione"));
            $.each(data, function (i, item) {
                $("#codigodistrito").append($("<option>").val(item.codigoUbigeo).text(item.nombre));
            });
        });        
    });

    $(".select2").on("select2:close", function (e) {
        $(this).valid();
    });

    $('#registrar').click(function () {

        $.validator.addMethod("validarrubronegocio", function (value, element) {
            if (value === "000" && $("#otrorubronegocio").val().trim() === "") {
                return false;
            } else {
                return true;
            }
        });

        $.validator.addMethod("validarpersonajuridica", function (value, element) {
            if ($("#optPersonaJuridica").is(":checked") && value.trim() === "") {
                return false;
            } else {
                return true;
            }
        });

        $.validator.addMethod("validarseleccion", function (value, element) {
            if (value === "" || value === "00" || value === "000" || value === "XX" || value === 0) {
                return false;
            } else {
                return true;
            }
        });

        $.validator.addMethod("validaredad", function (value, element) {

            var hoy = new Date();
            var cumpleanos = new Date(value);
            var edad = hoy.getFullYear() - cumpleanos.getFullYear();
            var m = hoy.getMonth() - cumpleanos.getMonth();

            if (m < 0 || (m === 0 && hoy.getDate() < cumpleanos.getDate())) {
                edad--;
            }

            if (edad < 18) {
                return false;
            } else {
                return true;
            }

        });        

        var validator = $('#frmAfiliacion').validate({
            rules: {
                CodigoTipoPersona: "required",
                codigorubronegocio: {
                    validarrubronegocio: true
                },
                numeroruc: {
                    validarpersonajuridica: true
                },
                razonsocial: {
                    validarpersonajuridica: true
                },
                numerodni: {
                    required: true,
                    minlength: 8
                },
                nombres: {
                    required: true
                },
                apellidopaterno: {
                    required: true
                },
                apellidomaterno: {
                    required: true
                },
                optSexo: "required",
                fechanacimiento: {
                    required: true,
                    validaredad: true
                },
                codigodepartamento: {
                    validarseleccion: true
                },
                codigoprovincia: {
                    validarseleccion: true
                },
                codigodistrito: {
                    validarseleccion: true
                },
                via: "required"
            },
            messages: {
                CodigoTipoPersona: "Por favor seleccione el Tipo de Persona correspondiente.",
                codigorubronegocio: {
                    validarrubronegocio: "Por favor seleccione el Rubro de Negocio al cual pertenece, en caso no lo encuentre ingreselo en la casilla Otro."
                },
                numeroruc: {
                    validarpersonajuridica: "Al seleccionar Persona Juridica debe ingresar el numero de RUC."
                },
                razonsocial: {
                    validarpersonajuridica: "Al seleccionar Persona Juridica debe ingresar la Razon Social segun SUNAT."
                },
                numerodni: {
                    required: "Por favor ingrese un numero de DNI.",
                    minlength: "Por favor ingrese numero de DNI valido."
                },
                nombres: "Por favor ingrese un nombre.",
                apellidopaterno: "Por favor ingrese un Apellido Paterno",
                apellidomaterno: "Por favor ingrese un Apellido Materno",
                optSexo: "Por favor seleccione el sexo correspondiente.",
                fechanacimiento: {
                    required: "Por favor ingrese una fecha valida",
                    validaredad: "Para registrarse debe ser mayor de 18 años."
                },
                codigodepartamento: {
                    validarseleccion: "Por favor seleccione el Departamento al cual pertenece la direccion."
                },
                codigoprovincia: {
                    validarseleccion: "Por favor seleccione la Provincia a la cual pertenece la direccion."
                },
                codigodistrito: {
                    validarseleccion: "Por favor seleccione el Distrito al cual pertenece la direccion."
                },
                via: "Por favor ingrese una direccion"
            }
        });

        if (validator.form()) {
            Registrar();
        }

    });

});


function MostrarDivJuridica(valor) {
    if (valor) {
        $('#DivJuridica').show();
    } else {
        $('#DivJuridica').hide();
    }
}

function SoloNumeros(e) {
    if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {        
        return false;
    }
}

function SoloNumeroTelefonico(e) {
    var regex = new RegExp("^[0-9-]+$");
    var key = String.fromCharCode(!e.charCode ? e.which : e.charCode);
    if (!regex.test(key)) {
        e.preventDefault();
        return false;
    }
}

function PermitirSoloLetras(e) {
    var regex = new RegExp("^[a-zA-Z \b%.]+$");
    var key = String.fromCharCode(!e.charCode ? e.which : e.charCode);
    if (!regex.test(key)) {
        e.preventDefault();
        return false;
    }
}

function PermitirSoloLetrasyNumeros(e) {
    var regex = new RegExp("^[a-zA-Z0-9\b]+$");
    var key = String.fromCharCode(!e.charCode ? e.which : e.charCode);
    if (!regex.test(key)) {
        e.preventDefault();
        return false;
    }
}

function Registrar() {

    var UsuarioVM = new Object();
        
    UsuarioVM.IdUsuarioZiPago = $('#idusuariozipago').val();
    UsuarioVM.Clave1 = $('#clave1').val();
    UsuarioVM.CodigoRubroNegocio = $('#codigorubronegocio').val();
    UsuarioVM.OtroRubroNegocio = $('#otrorubronegocio').val();
    UsuarioVM.CodigoTipoPersona = $('input:radio[name=CodigoTipoPersona]:checked').val();
    UsuarioVM.NumeroRUC = $('#numeroruc').val();
    UsuarioVM.NumeroDNI = $('#numerodni').val();
    UsuarioVM.RazonSocial = $('#razonsocial').val();
    UsuarioVM.ApellidoPaterno = $('#apellidopaterno').val();
    UsuarioVM.ApellidoMaterno = $('#apellidomaterno').val();
    UsuarioVM.Nombres = $('#nombres').val();
    UsuarioVM.Sexo = $('input:radio[name=optSexo]:checked').val();
    UsuarioVM.FechaNacimiento = $('#fechanacimiento').val();
    UsuarioVM.TelefonoMovil = $('#telefonomovil').val();
    UsuarioVM.TelefonoFijo = $('#telefonofijo').val();
    UsuarioVM.CodigoDepartamento = $('#codigodepartamento').val();
    UsuarioVM.CodigoProvincia = $('#codigoprovincia').val();
    UsuarioVM.CodigoDistrito = $('#codigodistrito').val();
    UsuarioVM.Via = $('#via').val();
    UsuarioVM.DireccionFacturacion = $('#direccionfacturacion').val();
    UsuarioVM.Referencia = $('#referencia').val();
    
    var DTO = { 'model': UsuarioVM };

    $.ajax(
        {
            url: 'Registrar/',
            type: "POST",
            data: DTO,
            datatype: 'json',
            ContentType: 'application/json;utf-8'
        })
        .done(function (resp) {
            alert('Registro realizado correctamente.');            
        })
        .error(function (err) {
            alert('Error al registrar:\n' + err);
        });

}