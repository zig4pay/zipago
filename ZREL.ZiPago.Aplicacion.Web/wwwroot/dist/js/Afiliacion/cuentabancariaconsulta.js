jQuery(function ($) {
    
    $('#btnCancelar').click(function () {
        LimpiarFormulario();        
    });

    $('#btnConsultar').click(function () {
        ConsultarCuentas();
    });

    $(document).ready(function () {
        $("#numerocuenta").keypress(PermitirSoloNumeros);
        $("#cci").keypress(PermitirSoloNumeros);
    });

});

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
        showPaginationSwitch: false
    });

}