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
                error.appendTo(element.parent().parent());
            }
            else {
                error.insertAfter(element);
            }
        }
    });

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
        $("#comercioexiste").hide();
        form.validate();
        if (form.valid()) {
            VerificaExisteComercio();
        }
    });

    $(document).on('click', '.elimina', function (event) {
        var nro = 0;
        $(this).closest('tr').remove();
        $("#tblComercios tbody tr").each(function (index) {
            nro++;
            $(this).children("td").each(function (indextd) {
                switch (indextd) {
                    case 0:
                        $(this).html(nro);
                        break;
                }
            });
        });
    });

    $('#btnRegistrar').click(function () {

        var filas = $("#tblComercios tr").length;

        if (filas > 1) {            
            swal({
                title: "Desea registrar la lista de Comercios?",
                text: "Se realizara el registro de " + (filas - 1) + " Comercio(s).",
                type: "info",
                showCancelButton: true,
                confirmButtonClass: "btn-primary",
                confirmButtonText: "Si, registrar",
                closeOnConfirm: false
                },
                function () {
                    RegistrarComercios();
                });
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
    $('#descripcioncomercio').val('');
    $('#idbancozipago').val(0);
    $('#cuentasxbanco').val(0);
}

function VerificaExisteComercio() {

    var strCodigoComercio = $("#codigocomercio").val().trim();    
    var DTO = { "strCodigoComercio": strCodigoComercio };
    
    $.ajax(
        {
            url: 'VerificarExisteComercioZiPago/',
            type: "GET",
            data: DTO,
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
                            swal({
                                title: "Alerta",
                                text: "Los datos del comercio que desea anadir ya se encuentran en el listado.",
                                type: "warning",
                                confirmButtonText: "OK",
                                confirmButtonClass: 'btn text-white bg-button-acept'
                            });
                        }
                    }
                }
            });
        })
        .fail(function (err) {
            swal({
                title: "Error",
                text: "Se ha producido un error al validar el Codigo del Comercio, por favor intentelo en unos minutos.",
                type: "error",
                confirmButtonText: "OK",
                confirmButtonClass: 'btn text-white bg-button-acept'
            });
        });
}

function ValidarComercios() {

    var result = true;
    var codigocomercio;

    $("#tblComercios tbody tr").each(function (index) {

        $(this).children("td").each(function (indextd) {
            switch (indextd) {
                case 1:
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

    var nro = $("#tblComercios tr").length;

    var htmlTags =  '<tr>' +
                        '<td>' + nro + '</td>' +        
                        '<td>' + $("#codigocomercio").val().toUpperCase() + '</td>' +
                        '<td>' + $("#descripcioncomercio").val() + '</td>' +
                        '<td>' + $("#correonotificacion").val() + '</td>' +
                        '<td>' + $('select[name="idbancozipago"] option:selected').text() + '</td>' +
                        '<td style="display:none;">' + $("#cuentasxbanco").val() + '</td>' +        
                        '<td>' + $('select[name="cuentasxbanco"] option:selected').text() + '</td>' +
                        '<td><a id="btnQuitarComercio" class="btn btn-danger elimina"> Quitar </a></td>' +
                    '</tr>';

    $('#tblComercios tbody').append(htmlTags);
    LimpiarFormulario();

}

function RegistrarComercios() {

    var comercios = new Array();

    $("#tblComercios tbody tr").each(function (index) {        
        var comercio = new Object();
        
        comercio.IdUsuarioZiPago = $('#idusuariozipago').val();
        $(this).children("td").each(function (indextd) {
            switch (indextd) {
                case 1:
                    comercio.CodigoComercio = $(this).text();
                    break;
                case 2:
                    comercio.Descripcion = $(this).text();
                    break;
                case 3:
                    comercio.CorreoNotificacion = $(this).text();
                    break;
                case 5:
                    comercio.CodigoCuenta = $(this).text();
                    break;
            }
        });

        comercios.push(comercio);

    });

    var DTO = { 'comercios': comercios };

    $.ajax(
        {
            url: 'Registrar/',
            type: "POST",
            data: DTO,
            datatype: 'json',
            ContentType: 'application/json; utf-8'
        })
        .done(function (resp) {
            var content = JSON.parse(resp);
            console.log(content);
            if (!content.hizoError) {
                $("#tblComercios > tbody").html("");
                swal("Comercios registrados correctamente", content.mensaje, "success");
            } else {
                swal({
                    title: "Error",
                    text: "Ocurrio un error al registrar los Comercios. Por favor intentelo en unos minutos.",
                    type: "error",
                    showCancelButton: false,
                    confirmButtonClass: "btn-default",
                    confirmButtonText: "Ok",
                    closeOnConfirm: false
                });
            }            
        })
        .fail(function (err) {
            swal({
                title: "Error",
                text: "Ocurrio un error al registrar los Comercios.",
                type: "error",
                showCancelButton: false,
                confirmButtonClass: "btn-default",
                confirmButtonText: "Ok",
                closeOnConfirm: false
            });
        });


}
