jQuery(function ($) {

    $(document).ready(function () {
        
        $('#tblcuentasbancarias').DataTable({
            processing: true, 
            serverSide: true, 
            //"filter": true, 
            //"paging": true,
            //"pagingType': 'full_numbers",
            //"pageLength": 5,
            //"order": [[1, "desc"]],
            ajax: {                
                type: 'POST',
                url: 'ListarCuentasBancarias/',
                data: function (data){
                    data.IdUsuarioZiPago = $('#idusuariozipago').val();
                    //data.IdUsuarioZiPago = 1;
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

    });

});