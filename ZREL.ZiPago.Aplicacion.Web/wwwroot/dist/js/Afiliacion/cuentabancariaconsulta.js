jQuery(function ($) {
    
    $(document).bind("contextmenu", function (e) {
        return false;
    });

    $('#btnCancelar').click(function () {
        LimpiarFormulario();        
    });

    $('#btnConsultar').click(function () {
        ConsultarCuentas();
    });

    $(document).ready(function () {
        $("#numerocuenta").keypress(SoloNumeros);
        $("#cci").keypress(SoloNumeros);
    });

});

function SoloNumeros(e) {
    if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
        return false;
    }
}

function LimpiarFormulario() {
    $("#idbancozipago").val(0);
    $("#tipocuenta").val("00");
    $("#tipomoneda").val("00");
    $("#estado").val("0");
    $("#numerocuenta").val("");
}

function extraServerParams(params) {

    params.idUsuarioZiPago = $('#idusuariozipago').val();
    params.idBancoZiPago = $('#idbancozipago').val();
    params.codigoTipoCuenta = $('#tipocuenta').val();
    params.codigoTipoMoneda = $('#tipomoneda').val();
    params.activo = $('#estado').val();
    params.numeroCuenta = $('#numerocuenta').val().trim();

    return params;
}

function ConsultarCuentas() {

    var $table = $('#tblcuentas');

    $table.bootstrapTable('refreshOptions', {
        url: "Listar",
        showPaginationSwitch: false
    });

}