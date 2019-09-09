jQuery(function ($) {

    $(document).bind('contextmenu', function(e){
        return false;
    });

    $('#btnCancelar').click(function(){
        LimpiarFormulario();
    });

    $('#btnConsultar').click(function(){
        ConsultarComercios();        
    });

    $(document).ready(function(){

        $('#numerocuenta').keypress(PermitirSoloNumeros);       

    });

});

function PermitirSoloNumeros(e){
    if (e.which !== 8 && e.which !== 0 && (e.which < 48 || e.which > 57)) {
        return false;
    }
}

function LimpiarFormulario() {
    $('#codigocomercio').val('');
    $('#descripcion').val('');
    $('#estado').val('0');
    $('#idbancozipago').val(0);
    $('#numerocuenta').val('');
}

function extraServerParams(params) {

    params.IdUsuarioZiPago = $('#idusuariozipago').val();
    params.CodigoComercio = $('#codigocomercio').val().trim();
    params.Descripcion = $('#descripcion').val().trim();
    params.Activo = $('#estado').val();
    params.IdBancoZiPago = $('#idbancozipago').val();
    params.NumeroCuenta = $('#numerocuenta').val().trim();

    return params;
}

function ConsultarComercios() {

    var $table = $('#tblcomercios');    

    $table.bootstrapTable('refreshOptions', {
        url: "ListarComercios",
        showPaginationSwitch: false
    });

}