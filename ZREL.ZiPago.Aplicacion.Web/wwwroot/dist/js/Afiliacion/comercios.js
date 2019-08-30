jQuery(function ($) {

    $(document).bind("contextmenu", function (e) {
        return false;
    });

    $('#btnCancelar').click(function () {
        LimpiarFormulario();
    });

    $('#btnConsultar').click(function () {
        ConsultarComercios();
    });

    $(document).ready(function () {

        $('#tblcomercios').DataTable({                                    
            "autoWidth": false,            
            "info": true,
            "language": {
                "url": "/bower_components/datatables.net-bs/plug-ins/1.10.19/i18n/spanish.json"
            },
            "lengthChange": true,
            "ordering": false,
            "pageLength": 10,
            "paging": true,
            "searching": false
        });

        $("#numerocuenta").keypress(SoloNumeros);       

    });

});


function SoloNumeros(e) {
    if (e.which !== 8 && e.which !== 0 && (e.which < 48 || e.which > 57)) {
        return false;
    }
}

function LimpiarFormulario() {
    $("#codigocomercio").val("");
    $("#descripcion").val("");
    $("#estado").val("0");
    $("#idbancozipago").val(0);
    $("#numerocuenta").val("");
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
        "autoWidth": false,
        "info": true,
        "language": {
            "url": "/bower_components/datatables.net-bs/plug-ins/1.10.19/i18n/spanish.json"
        },
        "lengthChange": true,
        "ordering": false,
        "pageLength": 10,
        "paging": true,
        "processing": true,
        "searching": false,
        "serverSide": true,
        ajax: {
            type: 'POST',
            url: 'ListarComercios/',
            data: DTO,                        
            ContentType: 'application/json; utf-8'
            },       
        columns: [
            { 'data': 'Id', 'name': 'Id' },
            { 'data': 'Codigo', 'name': 'Codigo' },
            { 'data': 'Descripcion', 'name': 'Descripcion'},
            { 'data': 'CorreoNotificacion', 'name': 'Correo de Notificacion' },
            { 'data': 'IdBancoZiPago', 'name': 'Id Banco'},
            { 'data': 'Banco', 'name': 'Banco'},
            { 'data': 'TipoCuentaBancaria', 'name': 'Tipo de Cuenta'},
            { 'data': 'MonedaCuentaBancaria', 'name': 'Moneda'},
            { 'data': 'CuentaBancaria', 'name': 'Nro de Cuenta'},
            { 'data': 'Estado', 'name': 'Estado'},
            { 'data': 'FechaCreacion', 'name': 'Fecha de Registro'}
        ]        
    });

}