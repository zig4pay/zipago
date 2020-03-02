jQuery(function ($) {

    $.validator.setDefaults({
        highlight: function (element) {
            $(element).closest('.form-control').addClass('has-error');
        },
        unhighlight: function (element) {
            $(element).closest('.form-control').removeClass('has-error');
        },
        errorElement: 'span',
        errorClass: 'help-block',
        errorPlacement: function (error, element) {
            if (element.parent('.form-group').length) {
                error.insertAfter(element);                
            }
            else if (element.prop('type') === 'radio') {                
                error.appendTo($('#divGroupSexo'));
            }
            else if (element.prop('type') === 'checkbox') {
                error.appendTo(element.parent().parent());                
            }
            else {
                error.insertAfter(element);
            }
        }
    });

    $(document).ready(function () {

        $.validator.setDefaults({});

        $.validator.addMethod("validarrubronegocio", function (value) {
            if (parseInt(value) === 0 && $("#otrorubronegocio").val().trim() === "") {                
                return false;
            } else {
                return true;                
            }
        }, "Por favor seleccione el Rubro de Negocio al cual pertenece, en caso no lo encuentre ingreselo en la casilla Otro.");

        $.validator.addMethod("minlenghtnrodoccontacto", function (value) {
            if (parseInt($("#tipodocidentidad").val()) === 1) {
                cantdigitos = 8;
                return $("#numerodocumentocontacto").val().trim().length < 8 ? false : true;
            } else {
                return true;
            }
        }, "El Numero de Documento de Identidad debe contener minimo 8 digitos.");

        $.validator.addMethod("validarpersonajuridica", function (value, element) {
            
            if ($("#optPersonaJuridica").is(":checked") && value.trim() === "") {
                return false;
            } else {
                return true;
            }
        });

        $.validator.addMethod("validarseleccion", function (value, element) {            
            if (value === "" || value === "0" || value === "00" || value === "000" || value === "XX" || value === 0) {
                return false;
            } else {
                return true;
            }
        });

        $.validator.addMethod("validaredad", function (value) {            
            var hoy = new Date();
            var cumpleanos = new Date(value);
            var edad = hoy.getFullYear() - cumpleanos.getFullYear();

            if (edad < 0 || edad === 0) {                
                return false;
            } else {
                var m = hoy.getMonth() - cumpleanos.getMonth();

                if (m < 0 || (m === 0 && hoy.getDate() < cumpleanos.getDate())) {
                    edad--;
                }
                
                if (edad < 18) {
                    return false;
                } else {
                    return true;
                }
            }

        }, "Para registrarse debe ser mayor de 18 años.");
        
        $('#fechanacimiento').datepicker({
            autoclose: true,
            language: "es",
            format: 'dd/mm/yyyy'
        });

        if ($("#EstadoRegistro").val() === "N") {
            $("#fechanacimiento").datepicker("update", new Date());
        } else {
            var fecha = $('#fecha').val();
            $("#fechanacimiento").datepicker("update", fecha);
            //Deshabilitar();
        }

        if ($('#optPersonaJuridica').is(":checked")) {            
            MostrarDivJuridica(true);
        }

        if ($('#optPersonaNatural').is(":checked")) {            
            MostrarDivJuridica(false);
        }

        $("#numeroruc").keypress(PermitirSoloNumeros);
        $("#numerodocumentocontacto").keypress(DeterminarTipoDato);
        $("#telefonofijo").keypress(SoloNumeroTelefonico);
        $("#telefonomovil").keypress(SoloNumeroTelefonico);        
        $("#nombres").keypress(PermitirSoloLetras);
        $("#apellidopaterno").keypress(PermitirSoloLetras);
        $("#apellidomaterno").keypress(PermitirSoloLetras);
        
        var validator = $('#frmAfiliacion').validate({
            rules: {
                CodigoTipoPersona: "required",
                codigorubronegocio: {
                    validarrubronegocio: true
                },                
                numeroruc: {
                    validarpersonajuridica: true,
                    minlength: 11,
                    maxlength: 11
                },
                razonsocial: {
                    validarpersonajuridica: true
                },
                tipodocidentidad: {
                    validarseleccion: true
                },
                numerodocumentocontacto: {
                    required: true,
                    minlenghtnrodoccontacto: true
                },
                nombres: {
                    required: true
                },
                apellidopaterno: {
                    required: true
                },
                apellidomaterno: {
                    required: true
                },
                Sexo: "required",
                fechanacimiento: {
                    required: true,
                    validaredad: true
                },
                telefonomovil: "required",
                CodigoDepartamento: {
                    validarseleccion: true
                },
                CodigoProvincia: {
                    validarseleccion: true
                },
                CodigoDistrito: {
                    validarseleccion: true
                },
                via: "required"
            },
            messages: {
                CodigoTipoPersona: "Por favor seleccione el Tipo de Persona correspondiente.",
                numeroruc: {
                    validarpersonajuridica: "Al seleccionar Persona Juridica debe ingresar el numero de RUC.",
                    minlength: "El numero de RUC debe contener minimo 11 digitos.",
                    maxlength: "El numero de RUC debe contener maximo 11 digitos."
                }, 
                razonsocial: {
                    validarpersonajuridica: "Al seleccionar Persona Juridica debe ingresar la Razon Social segun SUNAT."
                },
                tipodocidentidad: {
                    validarseleccion: "Por favor seleccione un Tipo de Documento de Identidad."
                },
                numerodocumentocontacto: {
                    required: "Por favor ingrese el numero de Documento de Identidad."
                },
                nombres: "Por favor ingrese un nombre.",
                apellidopaterno: "Por favor ingrese un Apellido Paterno",
                apellidomaterno: "Por favor ingrese un Apellido Materno",
                Sexo: "Por favor seleccione el sexo correspondiente.",
                fechanacimiento: {
                    required: "Por favor ingrese una fecha valida"
                },
                telefonomovil: "Por favor ingrese un numero de telefono movil.",
                CodigoDepartamento: {
                    validarseleccion: "Por favor seleccione el Departamento al cual pertenece la direccion."
                },
                CodigoProvincia: {
                    validarseleccion: "Por favor seleccione la Provincia a la cual pertenece la direccion."
                },
                CodigoDistrito: {
                    validarseleccion: "Por favor seleccione el Distrito al cual pertenece la direccion."
                },
                via: "Por favor ingrese una direccion"
            }
        });

        $('#btnHistorico').click(function () {
            ConsultarDomicilios();
        });

        $('#codigorubronegocio').on('change', function () {            
            if (parseInt($('#codigorubronegocio').val()) > 0) {
                $("#otrorubronegocio").val("");
                $("#otrorubronegocio").prop('disabled', true);
            } else {
                $("#otrorubronegocio").prop('disabled', false);
            }
        });

        $('#btnLimpiar').click(function () {
            Limpiar();
        });

        $('#btnRegistrar').click(function () {
            var $valid = $('#frmAfiliacion').valid();
            
            if (!$valid) {
                return false;
            } else {
                swal({
                    title: "Desea registrar los Datos Personales?",
                    text: "Se realizara el registro de los datos ingresados.",
                    type: "info",
                    showCancelButton: true,
                    confirmButtonClass: "btn-primary",
                    confirmButtonText: "Si, registrar",
                    cancelButtonText: "No, cancelar",
                    closeOnConfirm: false
                },
                function () {
                    Registrar();
                });
            }
        });

    });

    $(document).on('change', '[data-cascade-combo]', function (event) {

        var id = $(this).attr('data-cascade-combo');
        var url = $(this).attr('data-cascade-combo-source');
        var paramName = $(this).attr('data-cascade-combo-param-name');

        var data = {};
        data[paramName] = id;

        $.ajax({
            url: url,
            data: {
                strCodigoUbigeo: $(this).val()
            }
        }).done(function (data) {
            $(id).html('');
            if ($(this).attr('name') === 'departamento') {
                var id2 = $(this).attr('data-cascade-combo2');
                $(id2).html('');
            }
            $.each(data,
                function (index, type) {
                    var content = '<option value="' + type.value + '">' + type.text + '</option>';
                    $(id).append(content);
                });
        });
    });

    $(document).on('change', '#tipodocidentidad', function (event) {
        $("#numerodocumentocontacto").val("");
        switch (parseInt($("#tipodocidentidad").val())) {            
            case 1:
                $("#numerodocumentocontacto").prop('maxLength', 8);
                break;
            case 3:                
                $("#numerodocumentocontacto").prop('maxLength', 12);
                break;
            case 4:                
                $("#numerodocumentocontacto").prop('maxLength', 15);
                break;
        }
    });

    $('#optPersonaJuridica').on('change', function () {
        if ($(this).is(":checked")) {
            MostrarDivJuridica(true);
        }
    });

    $('#optPersonaNatural').on('change', function () {
        if ($(this).is(":checked")) {
            MostrarDivJuridica(false);
        }
    });
        
});

function DeterminarTipoDato(e) {

    var regex;
    var key;

    switch (parseInt($("#tipodocidentidad").val())) {
        case 1:
            if (e.which !== 8 && e.which !== 0 && (e.which < 48 || e.which > 57)) {
                return false;
            }
            break;
        default:
            regex = new RegExp("^[a-zA-Z0-9\b]+$");
            key = String.fromCharCode(!e.charCode ? e.which : e.charCode);
            if (!regex.test(key)) {
                e.preventDefault();
                return false;
            }
    }
}


function Deshabilitar() {

    $("[name='CodigoTipoPersona']").prop('disabled', true);
    $("#codigorubronegocio").prop('disabled', true);
    $("#otrorubronegocio").prop('disabled', true);
    $("#numeroruc").prop('disabled', true);
    $("#razonsocial").prop('disabled', true);
    $("#tipodocidentidad").prop('disabled', true);
    $("#numerodocumentocontacto").prop('disabled', true);
    $("#nombres").prop('disabled', true);
    $("#apellidopaterno").prop('disabled', true);
    $("#apellidomaterno").prop('disabled', true);
    $("[name='Sexo']").prop('disabled', true);
    $("#fechanacimiento").prop('disabled', true);
    $("#telefonofijo").prop('disabled', true);
    $("#telefonomovil").prop('disabled', true);
    $("#CodigoDepartamento").prop('disabled', true);
    $("#CodigoProvincia").prop('disabled', true);
    $("#CodigoDistrito").prop('disabled', true);
    $("#via").prop('disabled', true);
    $("#direccionfacturacion").prop('disabled', true);
    $("#referencia").prop('disabled', true);
    $("#btnLimpiar").prop('disabled', true);
    $("#btnRegistrar").prop('disabled', true);

}

function MostrarDivJuridica(valor) {
    if (valor) {
        $('#DivJuridica').show();
    } else {
        $('#DivJuridica').hide();
    }
}

function Limpiar() {
    $("#codigorubronegocio").val("000");
    $("#otrorubronegocio").val("");
    $("#numeroruc").val("");
    $("#razonsocial").val("");
    $("#tipodocidentidad").val("00");
    $("#numerodocumentocontacto").val("");    
    $("[name='Sexo']").each(function () {
        $(this).removeAttr('checked');
        $("[name='Sexo']").prop('checked', false);
    });
    $("#fechanacimiento").datepicker("update", new Date());
    $("#telefonofijo").val("");
    $("#telefonomovil").val("");
    $("#CodigoDepartamento").val("XX");
    $("#CodigoProvincia").val("XX");
    $("#CodigoDistrito").val("XX");
    $("#via").val("");
    $("#direccionfacturacion").val("");
    $("#referencia").val("");
}

function Registrar() {

    var DatosPersonalesVM = new Object();
    
    DatosPersonalesVM.IdUsuarioZiPago = $('#idusuariozipago').val();
    DatosPersonalesVM.Clave1 = $('#clave1').val();
    DatosPersonalesVM.CodigoRubroNegocio = $('#codigorubronegocio').val();
    DatosPersonalesVM.OtroRubroNegocio = $('#otrorubronegocio').val();
    DatosPersonalesVM.CodigoTipoPersona = $('input:radio[name=CodigoTipoPersona]:checked').val();
    DatosPersonalesVM.NumeroDocumento = $('#numeroruc').val();
    DatosPersonalesVM.RazonSocial = $('#razonsocial').val();
    DatosPersonalesVM.CodigoTipoDocumentoContacto = $('#tipodocidentidad').val();
    DatosPersonalesVM.NumeroDocumentoContacto = $('#numerodocumentocontacto').val();    
    DatosPersonalesVM.ApellidoPaterno = $('#apellidopaterno').val();
    DatosPersonalesVM.ApellidoMaterno = $('#apellidomaterno').val();
    DatosPersonalesVM.Nombres = $('#nombres').val();
    DatosPersonalesVM.Sexo = $('input:radio[name=Sexo]:checked').val();
    DatosPersonalesVM.FechaNacimiento = $('#fechanacimiento').val();
    DatosPersonalesVM.TelefonoMovil = $('#telefonomovil').val();
    DatosPersonalesVM.TelefonoFijo = $('#telefonofijo').val();
    DatosPersonalesVM.EstadoRegistro = $("#EstadoRegistro").val();
    DatosPersonalesVM.CodigoDepartamento = $('#CodigoDepartamento').val();
    DatosPersonalesVM.CodigoProvincia = $('#CodigoProvincia').val();
    DatosPersonalesVM.CodigoDistrito = $('#CodigoDistrito').val();
    DatosPersonalesVM.Via = $('#via').val();
    DatosPersonalesVM.DireccionFacturacion = $('#direccionfacturacion').val();
    DatosPersonalesVM.Referencia = $('#referencia').val();
    
    var DTO = { 'model': DatosPersonalesVM };

    $.ajax(
        {
            url: 'Registrar/',
            type: "POST",
            data: DTO,
            datatype: 'json',
            ContentType: 'application/json;utf-8'
        })
        .done(function (resp) {
            var content = JSON.parse(resp);
            if (!content.hizoError) {
                $("#EstadoRegistro").val("R");
                //Deshabilitar();
                swal("Registro de Datos Personales", content.mensaje, "success");
            } else {
                swal({
                    title: "Registro de Datos Personales",
                    text: "Ocurrio un error al registrar los datos ingresados. Por favor intentelo en unos minutos.",
                    type: "error",
                    showCancelButton: false,
                    confirmButtonClass: "btn-default",
                    confirmButtonText: "Ok",
                    closeOnConfirm: false
                });
                console.log(content.mensajeError);
            }
        })
        .fail(function (err) {
            swal({
                title: "Error",
                text: "Ocurrio un error al registrar los Datos Personales. Por favor intentelo en unos minutos.",
                type: "error",
                showCancelButton: false,
                confirmButtonClass: "btn-default",
                confirmButtonText: "Ok",
                closeOnConfirm: false
            });
        });

}

function extraServerParams(params) {
    params.IdUsuarioZiPago = $('#idusuariozipago').val();
    return params;
}

function ConsultarDomicilios() {

    var $table = $('#tbldomicilios');

    $table.bootstrapTable('refreshOptions', {
        url: "ListarDomiciliosHistorico",
        showPaginationSwitch: false
    });

}

