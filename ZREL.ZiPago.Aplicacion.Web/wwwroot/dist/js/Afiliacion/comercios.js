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
            "pageLength": 5,
            ajax: {
                type: 'POST',
                url: 'ListarComercios/',
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

        $('#idbancozipago').on('change', function () {
            var intIdUsuarioZiPago = $('#idusuariozipago').val();
            var intIdBancoZiPago = $(this).val();

            $("#cuentasxbanco").empty();            
            $.getJSON("ListarCuentasBancarias", { idUsuarioZiPago: intIdUsuarioZiPago, idBancoZiPago: intIdBancoZiPago}, function (data) {
                $("#cuentasxbanco").append($("<option>").val(0).text("Seleccione"));
                $.each(data, function (i, item) {
                    $("#cuentasxbanco").append($("<option>").val(item.IdCuentaBancaria).text(item.Descripcion));
                });
            });
        });
        
    });

});