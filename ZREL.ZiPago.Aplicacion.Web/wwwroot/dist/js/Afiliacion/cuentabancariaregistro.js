jQuery(function ($) {

    $('#btnCancelar').click(function () {
        LimpiarFormulario();
    });

    $(document).ready(function () {
        $("#numerocuenta").keypress(PermitirSoloNumeros);
        $("#cci").keypress(PermitirSoloNumeros);
    });

    $('#btnAnadir').click(function () {
        var banco = $("#idbancozipago").val();
        var tipocuenta = $("#codigotipocuenta").val();
        var moneda = $("#codigomoneda").val();
        var numerocuenta = $("#numerocuenta").val();
        var cci = $("#cci").val();

        if (ValidarDatos(banco, tipocuenta, moneda, numerocuenta)) {
            if (ValidarCuentasAgregadas(banco, tipocuenta, moneda, numerocuenta)) {
                AgregarCuentas(banco, tipocuenta, moneda, numerocuenta, cci);
            } else {
                swal({
                    title: "Alerta",
                    text: "Los datos de la cuenta a ingresar ya han sido agregados a la lista.",
                    type: "info",
                    showCancelButton: false,
                    confirmButtonClass: "btn-default",
                    confirmButtonText: "Ok",
                    closeOnConfirm: false
                });
            }
        } else {
            swal({
                title: "Alerta",
                text: "Debe seleccionar un Banco, el Tipo de Cuenta, la Moneda e ingresar el Numero de Cuenta.",
                type: "info",
                showCancelButton: false,
                confirmButtonClass: "btn-default",
                confirmButtonText: "Ok",
                closeOnConfirm: false
            });
        }
    });

    $(document).on('click', '.elimina', function (event) {
        var nro = 0;

        $(this).closest('tr').remove();
        $("#tblCuentas tbody tr").each(function (index) {
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
        var filas = $("#tblCuentas tr").length;

        if (filas > 1) {
            swal({
                title: "Desea registrar Cuentas Bancarias?",
                text: "Se realizara el registro de " + (filas - 1) + " Comercio(s).",
                type: "info",
                showCancelButton: true,
                confirmButtonClass: "btn-primary",
                confirmButtonText: "Si, registrar",
                cancelButtonText: "No, cancelar",
                closeOnConfirm: false
            },
                function () {
                    RegistrarCuentasBancarias();
                });
        } else {
            swal({
                title: "Alerta",
                text: "Debe seleccionar un Banco, el Tipo de Cuenta, la Moneda e ingresar el Numero de Cuenta, luego presionar el boton Anadir.",
                type: "warning",
                showCancelButton: false,
                confirmButtonClass: "btn-default",
                confirmButtonText: "Ok",
                closeOnConfirm: false
            });
        }
    });

});

function LimpiarFormulario() {
    $("#idbancozipago").val(0);
    $("#codigotipocuenta").val("00");
    $("#codigomoneda").val("00");
    $("#numerocuenta").val("");
    $("#cci").val("");
}

function ValidarDatos(banco, tipocuenta, moneda, numerocuenta) {

    if (banco === 0 || tipocuenta === "00" || moneda === "00" || numerocuenta === "") {
        return false;
    } else {
        return true;
    }

}

function ValidarCuentasAgregadas(banco, tipocuenta, moneda, cuenta) {
    var result = true;

    $("#tblCuentas tbody tr").each(function (index) {
        var idBanco, idTipoCuenta, idMoneda, nroCuenta;

        $(this).children("td").each(function (indextd) {
            switch (indextd) {
                case 1:
                    idBanco = $(this).text();
                    break;
                case 3:
                    idTipoCuenta = $(this).text();
                    break;
                case 5:
                    idMoneda = $(this).text();
                    break;
                case 7:
                    nroCuenta = $(this).text();
                    break;
            }
            if (banco === idBanco && tipocuenta === idTipoCuenta && moneda === idMoneda && cuenta === nroCuenta) {
                result = false;
            }
        });
    });
    return result;
}

function AgregarCuentas(banco, tipocuenta, moneda, cuenta, cci) {

    var nro = $("#tblCuentas tr").length;

    var htmlTags = '<tr>' +
        '<td>' + nro + '</td>' +
        '<td style="display:none;">' + banco + '</td>' +
        '<td>' + $('select[name="idbancozipago"] option:selected').text() + '</td>' +
        '<td style="display:none;">' + tipocuenta + '</td>' +
        '<td>' + $('select[name="codigotipocuenta"] option:selected').text() + '</td>' +
        '<td style="display:none;">' + moneda + '</td>' +
        '<td>' + $('select[name="codigomoneda"] option:selected').text() + '</td>' +
        '<td>' + cuenta + '</td>' +
        '<td>' + cci + '</td>' +
        '<td><a id="btnQuitarCta" class="btn btn-danger elimina"> Eliminar </a></td>' +
        '</tr>';

    $('#tblCuentas tbody').append(htmlTags);

    LimpiarFormulario();
}

function RegistrarCuentasBancarias() {

    var cuentas = new Array();

    $("#tblCuentas tbody tr").each(function (index) {

        var cuentaBancaria = new Object();
        cuentaBancaria.IdUsuarioZiPago = $("#idusuariozipago").val();

        $(this).children("td").each(function (indextd) {
            switch (indextd) {
                case 1:
                    cuentaBancaria.IdBancoZiPago = $(this).text();
                    break;
                case 3:
                    cuentaBancaria.CodigoTipoCuenta = $(this).text();
                    break;
                case 5:
                    cuentaBancaria.CodigoTipoMoneda = $(this).text();
                    break;
                case 7:
                    cuentaBancaria.NumeroCuenta = $(this).text();
                    break;
                case 8:
                    cuentaBancaria.CCI = $(this).text();
                    break;
            }
        });
        cuentas.push(cuentaBancaria);

    });

    var DTO = { 'cuentasBancarias': cuentas };

    $.ajax(
    {
        url: 'RegistrarCuentasBancarias/',
        type: "POST",
        data: DTO,
        datatype: 'json',
        ContentType: 'application/json;utf-8'
    })
    .done(function (resp) {
        var content = JSON.parse(resp);
        if (!content.hizoError) {
            $("#tblCuentas > tbody").html("");
            swal("Cuentas Bancarias registradas correctamente", content.mensaje, "success");
        } else {
            swal({
                title: "Error",
                text: "Ocurrio un error al registrar las Cuentas Bancarias. Por favor intentelo en unos minutos.",
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
            text: "Ocurrio un error al registrar las Cuentas Bancarias. Por favor intentelo en unos minutos.",
            type: "error",
            showCancelButton: false,
            confirmButtonClass: "btn-default",
            confirmButtonText: "Ok",
            closeOnConfirm: false
        });
    });    
}