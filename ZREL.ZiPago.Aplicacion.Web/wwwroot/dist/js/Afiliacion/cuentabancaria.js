jQuery(function ($) {
    
    $(document).bind("contextmenu", function (e) {
        return false;
    });

    $('#btnAnadirCta').click(function () {

        var banco = $("#idbancozipago").val();
        var tipocuenta = $("#codigotipocuenta").val();
        var moneda = $("#codigomoneda").val();
        var numerocuenta = $("#numerocuenta").val();
        var cci = $("#cci").val();
        
        if (ValidarDatos(banco, tipocuenta, moneda, numerocuenta)) {
            if (ValidarCuentasAgregadas(banco, tipocuenta, moneda, numerocuenta)) {
                AgregarCuentas(banco, tipocuenta, moneda, numerocuenta, cci);
            } else {
                alert("Los datos de la cuenta a ingresar ya se encuentran registrados.");
            }
        } else {
            alert("Debe seleccionar un Banco, el Tipo de Cuenta, la Moneda e ingresar el Numero de Cuenta.");
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

    $('#btnCancelar').click(function () {

        LimpiarFormulario();
        $("#tblCuentas > tbody").html(""); 

    });

    $('#btnRegistrar').click(function () {

        RegistrarCuentasBancarias();

    });

    $(document).ready(function () {
        
        $('#tblcuentasbancarias').DataTable({
            processing: true,
            serverSide: true,
            "filter": true,
            "paging": true,
            "pageLength": 5,
            ajax: {
                type: 'POST',
                url: 'ListarCuentasBancarias/',
                data: function (data) {
                    data.IdUsuarioZiPago = $('#idusuariozipago').val();
                    return data;
                },
                contentType: 'application/json; charset=utf-8'
            },
            columnDefs:
                [{
                    targets: [0],
                    visible: false,
                    searchable: false
                }],
            columns: [
                { 'data': 'IdCuentaBancaria', 'name': 'ID', 'autoWidth': true },
                { 'data': 'Banco', 'name': 'Banco', 'autoWidth': true },
                { 'data': 'TipoCuenta', 'name': 'Tipo de Cuenta', 'autoWidth': true },
                { 'data': 'TipoMoneda', 'name': 'Moneda', 'autoWidth': true },
                { 'data': 'NumeroCuenta', 'name': 'Nro. de Cuenta', 'autoWidth': true },
                { 'data': 'CCI', 'name': 'CCI', 'autoWidth': true },
                { 'data': 'FechaCreacion', 'name': 'Fecha de Registro', 'autoWidth': true },
                { 'data': 'Estado', 'name': 'Estado', 'autoWidth': true }
            ]
        });

        $("#numerocuenta").keypress(SoloNumeros);

        $("#cci").keypress(SoloNumeros);

    });

});

function SoloNumeros(e) {
    if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
        return false;
    }
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

function LimpiarFormulario() {
    $("#idbancozipago").val(0);
    $("#codigotipocuenta").val("00");
    $("#codigomoneda").val("00");
    $("#numerocuenta").val("");
    $("#cci").val("");
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
            alert('Registro realizado correctamente.');
        })
        .fail(function (err) {
            alert('Error al registrar:\n' + err);
        });
        //.always(function () {
        //    $('#tblcuentasbancarias').DataTable().ajax.reload();
        //});
}