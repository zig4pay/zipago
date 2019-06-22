jQuery(function ($) {
    
    $(document).bind("contextmenu", function (e) {
        return false;
    });
    
    $('#optPersonaJuridica').change(function () {
        if ($(this).is(":checked")) {
            MostrarDivJuridica(true);
        }        
    });

    $('#optPersonaNatural').change(function () {
        if ($(this).is(":checked")) {            
            MostrarDivJuridica(false);
        }
    });

    $("#codigodepartamento").on("change", function () {
        var strCodigoUbigeo = $(this).val();
        $("#codigoprovincia").empty();
        $.getJSON("ListarPorUbigeo", { strCodigoUbigeo: strCodigoUbigeo }, function (data) {
            $.each(data, function (i, item) {                
                $("#codigoprovincia").append($("<option>").val(item.codigoUbigeo).text(item.nombre));
            });
        });
    });

    $("#codigoprovincia").on("change", function () {
        var strCodigoUbigeo = $(this).val();
        $("#codigodistrito").empty();
        $.getJSON("ListarPorUbigeo", { strCodigoUbigeo: strCodigoUbigeo }, function (data) {
            $.each(data, function (i, item) {
                $("#codigodistrito").append($("<option>").val(item.codigoUbigeo).text(item.nombre));
            });
        });
    });

    $('#btnAnadirCta').click(function () {
        AgregarCuentas();
    });

});

var numFila = 0;

function MostrarDivJuridica(valor) {
    if (valor) {
        $('#DivJuridica').show();        
    } else {
        $('#DivJuridica').hide();        
    }
}

function AgregarCuentas() {
    numFila++;
    var htmlTags = '<tr id="fila'+ numFila +'">' +
            '<td style="display:none;">' + $("#idbancozipago").val() + '</td>' +
            '<td>' + $('select[name="idbancozipago"] option:selected').text() + '</td>' +
            '<td style="display:none;">' + $("#codigotipocuenta").val() + '</td>' +
            '<td>' + $('select[name="codigotipocuenta"] option:selected').text() + '</td>' +
            '<td style="display:none;">' + $("#codigomoneda").val() + '</td>' +
            '<td>' + $('select[name="codigomoneda"] option:selected').text() + '</td>' +
            '<td>' + $("#numerocuenta").val() + '</td>' +
            '<td>' + $("#cci").val() + '</td>' +
            '<td><a id="btnQuitarCta" class="btn btn-success btn-app"><i class="fa fa-trash"></i> Quitar </a></td>' +
        '</tr>';

    $('#tblCuentas tbody').append(htmlTags);

}

