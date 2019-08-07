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

    $.validator.addMethod("validarseleccion", ValidarSeleccion);
    $.validator.addMethod("validarrubronegocio", ValidarRubroNegocio);
    $.validator.addMethod("validarruc", ValidarRUC);
    $.validator.addMethod("validarrazonsocial", ValidarRazonSocial);
    $.validator.addMethod("validaredad", ValidarEdad);

    $('#finish').click(function () {
        Registrar();
    });
    
});


$(document).ready(function () {

    $('#codigodepartamento').on('select2:select', function (e) {
        var data = e.params.data;
        console.log(data);
    });

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

    var $validator = $('#frmAfiliacion').validate({
        rules: {
            CodigoTipoPersona: "required",
            codigorubronegocio: {
                validarrubronegocio: true
            },
            numeroruc: {
                validarruc: true
            },
            razonsocial: {
                validarrazonsocial: true
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
                validarruc: "Al seleccionar Persona Juridica debe ingresar el numero de RUC."
            },
            razonsocial: {
                validarrazonsocial: "Al seleccionar Persona Juridica debe ingresar la Razon Social segun SUNAT."
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

    //called when key is pressed in textbox
    $("#numeroruc").keypress(SoloNumeros);
    $("#numerodni").keypress(SoloNumeros);
    $("#telefonofijo").keypress(SoloNumeroTelefonico);
    $("#telefonomovil").keypress(SoloNumeroTelefonico);
    $("#apellidopaterno").keypress(PermitirSoloLetras);
    $("#apellidomaterno").keypress(PermitirSoloLetras);    
    
});

function MostrarDivJuridica(valor) {
    if (valor) {
        $('#DivJuridica').show();
    } else {
        $('#DivJuridica').hide();
    }
}

function ValidarRubroNegocio(valor) {
    if (valor === "000" && $("#otrorubronegocio").val().trim() === "") {
        return false;
    } else {
        return true;
    }
}

function ValidarSeleccion(valor) {
    if (valor === "" || valor === "00" || valor === "000" || valor === "XX" || valor === 0) {
        return false;
    } else {
        return true;
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


function ValidarRUC(valor) {
    if ($("#optPersonaJuridica").is(":checked") && valor.trim() == "") {
        return false;
    } else {
        return true;
    }
}

function ValidarRazonSocial(valor) {
    if ($("#optPersonaJuridica").is(":checked") && valor.trim() == "") {
        return false;
    } else {
        return true;
    }
}

function ValidarEdad(fecha) {

    var hoy = new Date();
    var cumpleanos = new Date(fecha);
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
       
}  



function Registrar() {

    var RegistroVM = new Object();
    var arrComercio = new Array(1);
    var arrCuenta = new Array(1);    
    
    RegistroVM.IdUsuarioZiPago = $('#idusuariozipago').val();
    RegistroVM.Clave1 = $('#clave1').val();
    RegistroVM.CodigoRubroNegocio = $('#codigorubronegocio').val();
    RegistroVM.OtroRubroNegocio = $('#otrorubronegocio').val();
    RegistroVM.CodigoTipoPersona = $('input:radio[name=CodigoTipoPersona]:checked').val();
    RegistroVM.NumeroRUC = $('#numeroruc').val();
    RegistroVM.NumeroDNI = $('#numerodni').val();
    RegistroVM.RazonSocial = $('#razonsocial').val();
    RegistroVM.ApellidoPaterno = $('#apellidopaterno').val();
    RegistroVM.ApellidoMaterno = $('#apellidomaterno').val();
    RegistroVM.Nombres = $('#nombres').val();
    RegistroVM.Sexo = $('input:radio[name=optSexo]:checked').val();
    RegistroVM.FechaNacimiento = $('#fechanacimiento').val();
    RegistroVM.TelefonoMovil = $('#telefonomovil').val();
    RegistroVM.TelefonoFijo = $('#telefonofijo').val();
    RegistroVM.CodigoDepartamento = $('#codigodepartamento').val();
    RegistroVM.CodigoProvincia = $('#codigoprovincia').val();
    RegistroVM.CodigoDistrito = $('#codigodistrito').val();
    RegistroVM.Via = $('#via').val();
    RegistroVM.DireccionFacturacion = $('#direccionfacturacion').val();
    RegistroVM.Referencia = $('#referencia').val();
    RegistroVM.ComerciosZiPago = arrComercio;
    RegistroVM.CuentasBancariaZiPago = arrCuenta;

    var DTO = { 'model': RegistroVM };

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