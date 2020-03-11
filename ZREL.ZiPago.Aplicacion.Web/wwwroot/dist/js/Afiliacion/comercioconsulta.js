jQuery(function ($) {

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

function LimpiarFormulario() {
    $('#codigocomercio').val('');
    $('#descripcion').val('');
    $('#estado').val('0');
    $('#idbancozipago').val(0);
    $('#numerocuenta').val('');
}

function extraServerParams(params) {
    console.debug("extraServerParams");
    params.IdUsuarioZiPago = $('#idusuariozipago').val();
    params.CodigoComercio = $('#codigocomercio').val().trim();
    params.Descripcion = $('#descripcion').val().trim();
    params.Estado = $('#estado').val();
    params.IdBancoZiPago = $('#idbancozipago').val();
    params.NumeroCuenta = $('#numerocuenta').val().trim();

    return params;
}

function ConsultarComercios() {

    var $table = $('#tblcomercios');    
    console.debug("ConsultarComercios1");
    $table.bootstrapTable('refreshOptions', {        
        showPaginationSwitch: false
    });
    console.debug("ConsultarComercios2");
}

function editarFormatter(value, row, index) {
    console.debug("editarFormatter");
    return [
        '<a class="btn btn-warning edit">',
            '<i class="fa fa-edit"></i> Editar',
        '</a >'
    ].join('');
}

window.editarEvents = {    
    'click .edit': function (e, value, row, index) {        
        window.location = "/Comercio/Editar/" + row.idComercio;
    }
};

function EditarComercio(codigoComercio) {

    var comercio = new Object();
    comercio.CodigoComercio = codigoComercio;
    
    var DTO = { 'comercio': comercio };

    $.ajax(
        {
            url: 'Editar/',
            type: "GET",
            data: DTO,
            datatype: 'json',
            ContentType: 'application/json; utf-8'            
        })
        .fail(function (err) {            
            swal({
                title: "Editar Comercio",
                text: "Ocurrio un error al editar el Comercio. Por favor intentelo en unos minutos.",
                type: "error",
                showCancelButton: false,
                confirmButtonClass: "btn-default",
                confirmButtonText: "Ok",
                closeOnConfirm: false
            });
        });

}
