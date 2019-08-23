jQuery(function ($) {

    $(document).bind("contextmenu", function (e) {
        return false;
    });


    $(document).ready(function () {

        $('#tblcomercios').DataTable({
            processing: true,
            serverSide: true,
            "filter": true,
            "paging": true,
            "pageLength": 10,
            ajax: {
                type: 'POST',
                url: 'ListarCuentasBancarias/',
                data: function (data) {
                    data.IdUsuarioZiPago = $('#idusuariozipago').val();
                    return data;
                },
                contentType: 'application/json; charset=utf-8'
            },            
            columns: [                
                { 'data': 'Codigo', 'name': 'Codigo', 'autoWidth': true },
                { 'data': 'Descripcion', 'name': 'Descripcion', 'autoWidth': true },
                { 'data': 'CorreoNotificacion', 'name': 'Correo de Notificacion', 'autoWidth': true },
                { 'data': 'Banco', 'name': 'Banco', 'autoWidth': true },
                { 'data': 'CuentaBancaria', 'name': 'Cuenta Bancaria', 'autoWidth': true },
                { 'data': 'Estado', 'name': 'Estado', 'autoWidth': true },
                { 'data': 'FechaCreacion', 'name': 'Fecha de Registro', 'autoWidth': true }
                
            ]
        });

    });

});