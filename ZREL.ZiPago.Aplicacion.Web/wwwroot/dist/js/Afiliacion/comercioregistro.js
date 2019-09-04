jQuery(function ($) {
    
    $.validator.addMethod("validarcorreo", function (value) {
        var respuesta = true;
        var reg = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        var regOficial = /^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$/;

        respuesta = reg.test(value) && regOficial.test(value) ? true : false;
        return respuesta;
    }, "Por favor ingrese una cuenta de correo electronica valida.");

    $.validator.addMethod("validarseleccion", function (value) {
        var respuesta = true;
        respuesta = parseInt(value) === 0 ? false : true;
        return respuesta;
    }, "Por favor primero seleccione un Banco y a continuacion seleccione una Cuenta Bancaria.");
    
    $(document).bind('contextmenu', function (e) {
        return false;
    });

    $('#btnCancelar').click(function () {
        LimpiarFormulario();
    });

    $('#btnAnadir').click(function () {
        var form = $("#frmRegistro");
        form.validate();
        if (form.valid()) {
            VerificaExisteComercio();
        }

    });

    $(document).ready(function () {
        
        $.validator.setDefaults({});

        $("#frmRegistro").validate({
            rules: {
                codigocomercio: "required",
                correonotificacion: {
                    required: true,
                    validarcorreo: true
                },
                descripcioncomercio: "required",
                cuentasxbanco: "validarseleccion"
            },
            messages: {                
                codigocomercio: "Por favor ingrese un Identificador (ID) de Comercio.",
                correonotificacion: {
                    required: "Por favor ingrese una cuenta de correo electrónica para realizar las notificaciones."
                },
                descripcioncomercio: "Por favor ingrese la Descripcion del Comercio."
            }
        });

        $('#codigocomercio').keypress(PermitirSoloLetrasyNumeros);

        $('#idbancozipago').on('change', function () {
            var intIdUsuarioZiPago = $('#idusuariozipago').val();
            var intIdBancoZiPago = $(this).val();

            $("#cuentasxbanco").empty();
            $.getJSON("ListarCuentasBancarias", { idUsuarioZiPago: intIdUsuarioZiPago, idBancoZiPago: intIdBancoZiPago }, function (data) {
                $("#cuentasxbanco").append($("<option>").val(0).text("Seleccione"));
                $.each(data, function (i, item) {
                    $("#cuentasxbanco").append($("<option>").val(item.IdCuentaBancaria).text(item.Descripcion));
                });
            });
        });

    });

});

function PermitirSoloLetrasyNumeros(e) {
    var regex = new RegExp("^[a-zA-Z0-9\b]+$");
    var key = String.fromCharCode(!e.charCode ? e.which : e.charCode);
    if (!regex.test(key)) {
        e.preventDefault();
        return false;
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

function ValidarSeleccion(valor) {
    return valor === 0 ? false : true;
}

function LimpiarFormulario() {
    $('#codigocomercio').val('');
    $('#correonotificacion').val(''); 
    $('#descripcioncomercio').val('');
    $('#idbancozipago').val(0);
    $('#cuentasxbanco').val(0);
}

function VerificaExisteComercio() {

    var strCodigoComercio = $("#codigocomercio").val().trim();    

    $.ajax(
        {
            url: 'VerificarExisteComercioZiPago/' + strCodigoComercio,
            type: "GET",
            datatype: 'json',
            ContentType: 'application/json;utf-8'
        })
        .done(function (resp) {
            $.each(resp, function (i, field) {
                if (i === "Mensaje") {
                    if (field === "Existe") {
                        $("#comercioexiste").show();
                    } else {
                        if (ValidarComercios()) {
                            AgregarComercios();
                        } else {
                            alert("Los datos del comercio a ingresar ya se encuentran anadidos.");
                        }
                    }
                }
            });
        })
        .error(function (err) {
            alert('Se ha producido un error al validar el Codigo del Comercio, \n por favor intentelo en unos minutos.');
        });
}

function ValidarComercios() {

    var result = true;
    var codigocomercio;

    $("#tblComercios tbody tr").each(function (index) {

        $(this).children("td").each(function (indextd) {
            switch (indextd) {
                case 2:
                    codigocomercio = $(this).text();
                    break;
            }
            if ($("#codigocomercio").val() === codigocomercio) {
                result = false;
            }            
        });
    });
    return result;
}

function AgregarComercios() {

    var htmlTags =  '<tr">' +
                        '<td style="display:none;">' + 1 + '</td>' +        
                        '<td>' + $("#codigocomercio").val() + '</td>' +
                        '<td>' + $("#descripcioncomercio").val() + '</td>' +
                        '<td>' + $("#correonotificacion").val() + '</td>' +
                        '<td>' + $("#cuentasxbanco").val() + '</td>' +        
                        $('select[name="cuentasxbanco"] option:selected').text() + '</td>' +
                        '<td><a id="btnQuitarComercio" class="btn btn-default eliminaComercio"> Quitar </a></td>' +
                    '</tr>';

    $('#tblComercios tbody').append(htmlTags);
    LimpiarFormulario();

}
