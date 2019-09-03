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

    //$('#btnAgregar').click(function(){
    //    location.href = '@Url.Action("Registrar","Comercios")';
    //});

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

function ConsultarComercios() {

    var comerciosVM = new Object();

    comerciosVM.IdUsuarioZiPago = $('#idusuariozipago').val();
    comerciosVM.CodigoComercio = $('#codigocomercio').val().trim();
    comerciosVM.Descripcion = $('#descripcion').val().trim();
    comerciosVM.Activo = $('#estado').val();
    comerciosVM.IdBancoZiPago = $('#idbancozipago').val();
    comerciosVM.NumeroCuenta = $('#numerocuenta').val().trim();

    var DTO = { 'comercioFiltros': comerciosVM };

    $('#tblcomercios').DataTable().destroy();

    $.fn.dataTable.ext.errMode = 'none';

    $('#tblcomercios')
        .on('error.dt', function (e, settings, techNote, message) {
            console.log('An error has been reported by DataTables: ', message);
        })
        .DataTable({
            'autoWidth': false,
            'info': true,
            'language': {
                'url': '/dist/bower_components/datatables/plug-ins/1.10.18/i18n/spanish.json'
            },            
            'ordering': false,
            'paging': true,
            'pageLength': 5,            
            'lengthChange': false,
            'processing': true,
            'searching': false,
            'serverSide': true,
        ajax: {
            type: 'POST',
            url: 'ListarComercios/',
            data: DTO,                        
            ContentType: 'application/json; utf-8'
            },       
        columnDefs: [
            { 'targets': [0], 'visible': false, 'searchable': false },
            { 'targets': [4], 'visible': false, 'searchable': false }            
        ],
        columns: [
            { 'data': 'Id', 'name': 'Id', 'autoWidth':false },
            { 'data': 'Codigo', 'name': 'ID Comercio', 'autoWidth': false },
            { 'data': 'Descripcion', 'name': 'Descripcion', 'autoWidth': false },
            { 'data': 'CorreoNotificacion', 'name': 'Correo de Notificacion', 'autoWidth': false },
            { 'data': 'IdBancoZiPago', 'name': 'IdBancoZiPago', 'autoWidth': false },
            { 'data': 'Banco', 'name': 'Banco', 'autoWidth': false },
            { 'data': 'TipoCuentaBancaria', 'name': 'Tipo de Cuenta', 'autoWidth': false },
            { 'data': 'MonedaCuentaBancaria', 'name': 'Moneda', 'autoWidth': false },
            { 'data': 'CuentaBancaria', 'name': 'Nro. de Cuenta', 'autoWidth': false },
            { 'data': 'Estado', 'name': 'Estado', 'autoWidth': false },
            { 'data': 'FechaCreacion', 'name': 'Fecha Registro', 'autoWidth': false }
        ]        
        });
}