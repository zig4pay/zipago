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
            $("#codigoprovincia").append($("<option>").val("XX").text("Seleccione"));
            $.each(data, function (i, item) {
                $("#codigoprovincia").append($("<option>").val(item.codigoUbigeo).text(item.nombre));
            });
        });
    });
    
    $("#codigoprovincia").on("change", function () {
        var strCodigoUbigeo = $(this).val();
        $("#codigodistrito").empty();        
        $.getJSON("ListarPorUbigeo", { strCodigoUbigeo: strCodigoUbigeo }, function (data) {
            $("#codigodistrito").append($("<option>").val("XX").text("Seleccione"));
            $.each(data, function (i, item) {
                $("#codigodistrito").append($("<option>").val(item.codigoUbigeo).text(item.nombre));
            });
        });
    });

    $('#btnAnadirCta').click(function () {

        if (ValidarDatos()) {
            if (ValidarCuentasExistentes()) {
                AgregarCuentas();
                if ($("#tblCuentas tr").length > 1) {
                    $(".btn-next").show();
                }
            } else {
                alert("Los datos de la cuenta a ingresar ya se encuentran registrados.");
            }
        } else {
            alert("Debe seleccionar un Banco, el Tipo de Cuenta, Moneda e ingresar los numeros de Cuenta y CCI.");
        }
        
    });

    $(document).on('click', '.elimina', function (event) {
        
        $(this).closest('tr').remove();
        arrCuentas.splice(0, arrCuentas.length);

        var pos = 0;
        var idCuentaAlt, idBanco, nomBanco, idTipoCuenta, NomTipoCuenta, idMoneda, NomMoneda, nroCuenta, nroCCI;

        $("#tblCuentas tbody tr").each(function (index) {
            $(this).children("td").each(function (indextd) {
                switch (indextd) {
                    case 0:
                        idCuentaAlt = $(this).text();
                        break;
                    case 1:
                        idBanco = $(this).text();
                        break;
                    case 2:
                        nomBanco = $(this).text();
                        break;
                    case 3:
                        idTipoCuenta = $(this).text();
                        break;
                    case 4:
                        NomTipoCuenta = $(this).text();
                        break;
                    case 5:
                        idMoneda = $(this).text();
                        break;
                    case 6:
                        NomMoneda = $(this).text();
                        break;
                    case 7:
                        nroCuenta = $(this).text();
                        break;
                    case 8:
                        nroCCI = $(this).text();
                        break;
                }
            });

            arrCuentas[pos] = new Array(7);
            arrCuentas[pos][0] = idCuentaAlt;
            arrCuentas[pos][1] = idBanco;
            arrCuentas[pos][2] = nroCuenta;
            arrCuentas[pos][3] = idTipoCuenta;
            arrCuentas[pos][4] = idMoneda;
            arrCuentas[pos][5] = nroCCI;
            arrCuentas[pos][6] = NomTipoCuenta + " - " + NomMoneda + " - Nro. Cta.: " + nroCuenta + " - CCI: " + nroCCI;

            pos++;
        });

        $("#cuentasxbanco").empty();

        if ($("#tblCuentas tr").length < 2) {
            $(".btn-next").hide();
        }

    });

    $("#idbancozipago1").on("change", function () {
        var strCodigoBanco = $(this).val();
        var cont = 0;

        $("#cuentasxbanco").empty();

        for (var i = 0; i < arrCuentas.length; i++) {
            if (arrCuentas[i][1] == strCodigoBanco) {
                $("#cuentasxbanco").append($("<option>").val(arrCuentas[i][0]).text(arrCuentas[i][6]));
                cont++;
            }
        }

    });

    $('#btnAnadirComercio').click(function () {
        $("#ComercioExiste").hide();
        if (ValidarDatosComercios()) {
            VerificaExisteComercio();
        } else {
            alert("Debe ingresar un Identificador y Descripcion de Comercio, \n un correo electronico para la notificacion \n y seleccionar una cuenta bancaria.");
        }
    });

    $(document).on('click', '.eliminaComercio', function (event) {
        $(this).closest('tr').remove();        
    });

    $('#finish').click(function () {
        Registrar();
    });

});

var nroFilas = 0;
var idCuenta = 0;
var arrCuentas = new Array(1);

function MostrarDivJuridica(valor) {
    if (valor) {
        $('#DivJuridica').show();        
    } else {
        $('#DivJuridica').hide();        
    }
}

function ValidarCuentasExistentes() {

    var result = true;
    
    $("#tblCuentas tbody tr").each(function (index) {

        var idBanco, idTipoCuenta, idMoneda, nroCuenta, nroCCI;
        
        $(this).children("td").each(function (indextd) {
            switch (indextd) {
                case 1:
                    idBanco = $(this).text();
                    break;
                case 3:
                    idTipoCuenta = $(this).text();
                    break;
                case 5:
                    idMoneda = $(this).text();
                    break;
                case 7:
                    nroCuenta = $(this).text();
                    break;
                case 8:
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

function ValidarDatos() {    

    if ($("#idbancozipago").val() === 0 ||
        $("#codigotipocuenta").val() === "00" ||
        $("#codigomoneda").val() === "00" ||
        $("#numerocuenta").val().trim() === "" ||
        $("#cci").val().trim() === "") {
        return false;
    } else {
        return true;
    }        

}

function AgregarCuentas() {    

    nroFilas = $("#tblCuentas tr").length - 1;
    idCuenta++;
    var htmlTags = '<tr">' +
        '<td style="display:none;">' + idCuenta + '</td>' +
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
    
    arrCuentas[nroFilas] = new Array(7);
    arrCuentas[nroFilas][0] = idCuenta;
    arrCuentas[nroFilas][1] = $("#idbancozipago").val();
    arrCuentas[nroFilas][2] = $("#numerocuenta").val();
    arrCuentas[nroFilas][3] = $("#codigotipocuenta").val();
    arrCuentas[nroFilas][4] = $("#codigomoneda").val();
    arrCuentas[nroFilas][5] = $("#cci").val();
    arrCuentas[nroFilas][6] = $('select[name="codigotipocuenta"] option:selected').text() + " - " +
        $('select[name="codigomoneda"] option:selected').text() + " - " +
        "Nro. Cta.: " + $("#numerocuenta").val() + " - " +
        "CCI: " + $("#cci").val();

    $("#numerocuenta").val("");
    $("#cci").val("");

    $("#idbancozipago1").empty();
    $("#cuentasxbanco").empty();
    $("#idbancozipago1").append($("<option>").val(0).text("Seleccione"));
    
    var idBanco, nombreBanco, existe, nroFila;
    var arrBancos = new Array();    

    $("#tblCuentas tbody tr").each(function (index) {

        $(this).children("td").each(function (indextd) {
            switch (indextd) {
                case 1:
                    idBanco = $(this).text();
                    break;
                case 2:
                    nombreBanco = $(this).text();
                    break;
            }
        });

        existe = false;
        nroFila = arrBancos.length;
        
        for (var i = 0; i < nroFila; i++) {
            if (arrBancos[i][0] === idBanco) {
                existe = true;  
                break;
            }
        }

        if (!existe) {
            arrBancos[nroFila] = new Array(2);
            arrBancos[nroFila][0] = idBanco;
            arrBancos[nroFila][1] = nombreBanco;
            $("#idbancozipago1").append($("<option>").val(idBanco).text(nombreBanco));
        }

    });

}

function ValidarDatosComercios() {

    if ($("#codigocomercio").val().trim() === "" ||
        $("#descripcionCom").val().trim() === "" ||
        $("#correonotificacion").val().trim() === "" ||
        $("#idbancozipago1").val().trim() === 0 ||
        $("#cuentasxbanco").val() === null
        ) {
        return false;
    } else {
        return true;
    }

}

function ValidarComercios() {

    var result = true;

    $("#tblComercios tbody tr").each(function (index) {

        var codigocomercio;

        $(this).children("td").each(function (indextd) {
            switch (indextd) {
                case 2:
                    codigocomercio = $(this).text();
                    break;
            }

            if ($("#codigocomercio").val() == codigocomercio) {
                result = false;
            }
            //$(this).css("background-color", "#ECF8E0");
        });
    });
    return result;
}

function AgregarComercios() {

    var htmlTags = '<tr">' +
        '<td style="display:none;">' + $("#cuentasxbanco").val() + '</td>' +
        '<td style="display:none;">' + $("#idusuariozipago").val() + '</td>' +
        '<td>' + $("#codigocomercio").val() + '</td>' +
        '<td>' + $("#descripcionCom").val() + '</td>' +
        '<td>' + $("#correonotificacion").val() + '</td>' +
        '<td>' + $('select[name="idbancozipago1"] option:selected').text() + " - " +
                 $('select[name="cuentasxbanco"] option:selected').text() + '</td>' +        
        '<td><a id="btnQuitarComercio" class="btn btn-default eliminaComercio"> Quitar </a></td>' +
        '</tr>';
    
    $('#tblComercios tbody').append(htmlTags);
    
    $("#codigocomercio").val("");
    $("#descripcionCom").val("");
    $("#idbancozipago1").val(0);
    $("#cuentasxbanco").empty();
}

function VerificaExisteComercio() {

    var strCodigoComercio = $("#codigocomercio").val().trim();
    var DTO = { 'strCodigoComercio': strCodigoComercio };

    $.ajax(
        {
            url: '/Afiliacion/VerificarExisteComercioZiPago/' + strCodigoComercio,
            type: "GET",    
            data: DTO,
            datatype: 'json',
            ContentType: 'application/json;utf-8'
        })
        .done(function (resp) {
            $.each(resp, function (i, field) {                
                if (i === "Mensaje") {
                    if (field === "Existe") {
                        $("#ComercioExiste").show();                    
                    } else {
                        if (ValidarComercios()) {
                            AgregarComercios();
                        } else {
                            alert("Los datos del comercio a ingresar ya se encuentran registrados.");
                        }
                    }
                }
            });
        })
        .error(function (err) {
            alert('Se ha producido un error al validar el Codigo del Comercio, \n por favor intentelo en unos minutos.');
        });
}

function Registrar() {
    
    var RegistroVM = new Object();    
    var arrComercio = new Array(1);
    var arrCuenta = new Array(1);
    var CodigoCuenta, IdBancoZiPago, NumeroCuenta, CodigoTipoCuenta, CodigoTipoMoneda, CCI;
    var CodigoComercio, IdUsuarioZiPago, Descripcion, CorreoNotificacion;
    var Cont = 0;

    $("#tblComercios tbody tr").each(function (index) {

        $(this).children("td").each(function (indextd) {
            switch (indextd) {
                case 0:
                    CodigoCuenta = $(this).text();
                    break;
                case 1:
                    IdUsuarioZiPago = $(this).text();
                    break;
                case 2:
                    CodigoComercio = $(this).text();
                    break;
                case 3:
                    Descripcion = $(this).text();
                    break;
                case 4:
                    CorreoNotificacion = $(this).text();
                    break;
            }
        });

        var ComercioZiPago = {
            CodigoCuenta: CodigoCuenta,
            CodigoComercio: CodigoComercio,
            IdUsuarioZiPago: IdUsuarioZiPago,
            Descripcion: Descripcion,
            CorreoNotificacion: CorreoNotificacion
        };

        arrComercio[Cont] = new Array(1);
        arrComercio[Cont] = ComercioZiPago;
        Cont++;
    });            

    Cont = 0;
    
    $("#tblCuentas tbody tr").each(function (index) {

        $(this).children("td").each(function (indextd) {
            switch (indextd) {
                case 0:
                    CodigoCuenta = $(this).text();
                    break;
                case 1:
                    IdBancoZiPago = $(this).text();
                    break;                
                case 3:
                    CodigoTipoCuenta = $(this).text();
                    break;
                case 5:
                    CodigoTipoMoneda = $(this).text();
                    break;                
                case 7:
                    NumeroCuenta = $(this).text();
                    break;
                case 8:
                    CCI = $(this).text();
                    break;
            }
        });

        var CuentaBancariaZiPago = {
            CodigoCuenta: CodigoCuenta,
            IdBancoZiPago: IdBancoZiPago,
            CodigoTipoCuenta: CodigoTipoCuenta,
            CodigoTipoMoneda: CodigoTipoMoneda,
            NumeroCuenta: NumeroCuenta,
            CCI: CCI
        };

        arrCuenta[Cont]= new Array(1);
        arrCuenta[Cont]= CuentaBancariaZiPago;

        Cont++;
    });
    
    RegistroVM.IdUsuarioZiPago = $('#idusuariozipago').val();
    RegistroVM.Clave1 = $('#clave1').val();
    RegistroVM.CodigoRubroNegocio = $('#codigorubronegocio').val();
    RegistroVM.OtroRubroNegocio = $('#otrorubronegocio').val();
    RegistroVM.CodigoTipoPersona = $('input:radio[name=CodigoTipoPersona]:checked').val();
    RegistroVM.NumeroRUC = $('#numeroruc').val();
    RegistroVM.NumeroDNI = $('#numerodni').val();
    RegistroVM.RazonSocial = $('#razonsocial').val();
    RegistroVM.ApellidoPaterno = $('#apellidopaterno').val();
    RegistroVM.ApellidoMaterno = $('#apellidomaterno').val();
    RegistroVM.Nombres = $('#nombres').val();
    RegistroVM.Sexo = $('input:radio[name=optSexo]:checked').val();    
    RegistroVM.FechaNacimiento = $('#fechanacimiento').val();
    RegistroVM.TelefonoMovil = $('#telefonomovil').val();
    RegistroVM.TelefonoFijo = $('#telefonofijo').val();
    RegistroVM.CodigoDepartamento = $('#codigodepartamento').val();
    RegistroVM.CodigoProvincia = $('#codigoprovincia').val();
    RegistroVM.CodigoDistrito = $('#codigodistrito').val();
    RegistroVM.Via = $('#via').val();
    RegistroVM.DireccionFacturacion = $('#direccionfacturacion').val();
    RegistroVM.Referencia = $('#referencia').val();
    RegistroVM.ComerciosZiPago = arrComercio;
    RegistroVM.CuentasBancariaZiPago = arrCuenta;
    
    var DTO = { 'model': RegistroVM };

    $.ajax(
    {
        url: '/Afiliacion/Registrar',
        type: "POST",
        data: DTO,
        datatype: 'json',
        ContentType: 'application/json;utf-8'
    })
    .done(function (resp) {
        alert('Registro realizado correctamente.');
        //if (resp['mensaje'] == "Registro realizado correctamente.") {
        //    alert('Registro realizado correctamente.');                                
        //}
        //if (resp.length > 1) {
        //    alert('Error al registrar:\n' + resp.mensajeError);                
        //}
    })
    .error(function (err) {
        alert('Error al registrar:\n' + err);            
    });

}