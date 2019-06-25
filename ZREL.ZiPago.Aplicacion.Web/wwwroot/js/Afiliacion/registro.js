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
        if (ValidarCuentas()) {
            AgregarCuentas();
        } else {
            alert("Los datos de la cuenta a ingresar ya se encuentran registrados.");
        }
        
    });

    $(document).on('click', '.elimina', function (event) {
        
        $(this).closest('tr').remove();
        arrCuentas.splice(0, arrCuentas.length);

        var pos = 0;
        var idBanco, NomTipoCuenta, idMoneda, NomMoneda, nroCuenta, nroCCI;

        $("#tblCuentas tbody tr").each(function (index) {
            $(this).children("td").each(function (indextd) {
                switch (indextd) {
                    case 0:
                        idBanco = $(this).text();
                        break;
                    case 3:
                        NomTipoCuenta = $(this).text();
                        break;
                    case 4:
                        idMoneda = $(this).text();
                        break;
                    case 5:
                        NomMoneda = $(this).text();
                        break;
                    case 6:
                        nroCuenta = $(this).text();
                        break;
                    case 7:
                        nroCCI = $(this).text();
                        break;
                }
            });

            arrCuentas[pos] = new Array(4);
            arrCuentas[pos][0] = pos;
            arrCuentas[pos][1] = idBanco;
            arrCuentas[pos][2] = idMoneda;
            arrCuentas[pos][3] = NomTipoCuenta + " - " + NomMoneda + " - Nro. Cta.: " + nroCuenta + " - CCI: " + nroCCI;

            pos++;
        });

        $("#cuentasxbanco").empty();



    });

    $("#idbancozipago1").on("change", function () {
        var strCodigoBanco = $(this).val();
        var cont = 0;

        $("#cuentasxbanco").empty();

        for (var i = 0; i < arrCuentas.length; i++) {
            if (arrCuentas[i][1] == strCodigoBanco) {
                $("#cuentasxbanco").append($("<option>").val(cont).text(arrCuentas[i][3]));
                cont++;
            }
        }
                        
    });

    $('#btnAnadirComercio').click(function () {
        if (ValidarComercios()) {
            AgregarComercios();
        } else {
            alert("Los datos del comercio a ingresar ya se encuentran registrados.");
        }

    });

    $('#finish').click(function () {
        alert("Registrar");
    });

});

var nroFilas = 0;
var arrCuentas = new Array(1);

function MostrarDivJuridica(valor) {
    if (valor) {
        $('#DivJuridica').show();        
    } else {
        $('#DivJuridica').hide();        
    }
}

function ValidarCuentas() {

    var result = true;
    
    $("#tblCuentas tbody tr").each(function (index) {

        var idBanco, idTipoCuenta, idMoneda, nroCuenta, nroCCI;

        $(this).children("td").each(function (indextd) {
            switch (indextd) {
                case 0:
                    idBanco = $(this).text();
                    break;
                case 2:
                    idTipoCuenta = $(this).text();
                    break;
                case 4:
                    idMoneda = $(this).text();
                    break;
                case 6:
                    nroCuenta = $(this).text();
                    break;
                case 7:
                    nroCCI = $(this).text();
                    break;
            }

            if ($("#idbancozipago").val() == idBanco &&
                $("#codigotipocuenta").val() == idTipoCuenta &&
                $("#codigomoneda").val() == idMoneda &&
                $("#numerocuenta").val() == nroCuenta &&
                $("#cci").val() == nroCCI
            ) {
                result = false;
            }
            //$(this).css("background-color", "#ECF8E0");
        });
    });
    return result;
}

function AgregarCuentas() {    

    nroFilas = $("#tblCuentas tr").length - 1;
    
    var htmlTags = '<tr">' +
        '<td style="display:none;">' + $("#idbancozipago").val() + '</td>' +
        '<td>' + $('select[name="idbancozipago"] option:selected').text() + '</td>' +
        '<td style="display:none;">' + $("#codigotipocuenta").val() + '</td>' +
        '<td>' + $('select[name="codigotipocuenta"] option:selected').text() + '</td>' +
        '<td style="display:none;">' + $("#codigomoneda").val() + '</td>' +
        '<td>' + $('select[name="codigomoneda"] option:selected').text() + '</td>' +
        '<td>' + $("#numerocuenta").val() + '</td>' +
        '<td>' + $("#cci").val() + '</td>' +
        '<td><a id="btnQuitarCta" class="btn btn-default elimina"> Quitar </a></td>' +
        '</tr>';

    $('#tblCuentas tbody').append(htmlTags);
        
    arrCuentas[nroFilas] = new Array(4);
    arrCuentas[nroFilas][0] = nroFilas;
    arrCuentas[nroFilas][1] = $("#idbancozipago").val();
    arrCuentas[nroFilas][2] = $("#codigomoneda").val();
    arrCuentas[nroFilas][3] = $('select[name="codigotipocuenta"] option:selected').text() + " - " +
        $('select[name="codigomoneda"] option:selected').text() + " - " +
        "Nro. Cta.: " + $("#numerocuenta").val() + " - " +
        "CCI: " + $("#cci").val();

    $("#numerocuenta").val("");
    $("#cci").val("");
}

function ValidarComercios() {

    var result = true;

    $("#tblComercios tbody tr").each(function (index) {

        var codigocomercio, descripcionCom, correonotificacion, cuenta;

        $(this).children("td").each(function (indextd) {
            switch (indextd) {
                case 0:
                    codigocomercio = $(this).text();
                    break;
                case 1:
                    descripcionCom = $(this).text();
                    break;
                case 2:
                    correonotificacion = $(this).text();
                    break;
                case 3:
                    cuenta = $(this).text();
                    break;
            }

            if ($("#codigocomercio").val() == codigocomercio &&
                $("#descripcionCom").val() == descripcionCom &&
                $("#correonotificacion").val() == correonotificacion &&
                $('select[name="cuentasxbanco"] option:selected').text() == cuenta
            ) {
                result = false;
            }
            //$(this).css("background-color", "#ECF8E0");
        });
    });
    return result;
}

function AgregarComercios() {

    var htmlTags = '<tr">' +
        '<td>' + $("#codigocomercio").val() + '</td>' +
        '<td>' + $("#descripcionCom").val() + '</td>' +
        '<td>' + $("#correonotificacion").val() + '</td>' +
        '<td>' + $('select[name="cuentasxbanco"] option:selected').text() + '</td>' +        
        '<td><a id="btnQuitarComercio" class="btn btn-default eliminaComercio"> Quitar </a></td>' +
        '</tr>';
    
    $('#tblComercios tbody').append(htmlTags);
    
    $("#codigocomercio").val("");
    $("#descripcionCom").val("");
    $("#correonotificacion").val("");
}

