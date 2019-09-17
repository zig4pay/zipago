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
            if (value === "000" && $("#otrorubronegocio").val().trim() === "") {
                return false;
            } else {
                return true;
            }
        }, "Por favor seleccione el Rubro de Negocio al cual pertenece, en caso no lo encuentre ingreselo en la casilla Otro.");

        $.validator.addMethod("validarpersonajuridica", function (value, element) {
            
            if ($("#optPersonaJuridica").is(":checked") && value.trim() === "") {
                return false;
            } else {
                return true;
            }
        });

        $.validator.addMethod("validarseleccion", function (value, element) {            
            if (value === "" || value === "00" || value === "000" || value === "XX" || value === 0) {
                return false;
            } else {
                return true;
            }
        });

        $.validator.addMethod("validaredad", function (value) {

            var hoy = new Date();
            var cumpleanos = new Date(value);
            var edad = hoy.getFullYear() - cumpleanos.getFullYear();
            var m = hoy.getMonth() - cumpleanos.getMonth();

            if (m < 0 || (m === 0 && hoy.getDate() < cumpleanos.getDate())) {
                edad--;
            }

            if (edad < 18) {
                return false;
            } else {
                return true;
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
            Deshabilitar();
        }
        
        //called when key is pressed in textbox
        $("#numeroruc").keypress(SoloNumeros);
        $("#numerodni").keypress(SoloNumeros);
        $("#telefonofijo").keypress(SoloNumeroTelefonico);
        $("#telefonomovil").keypress(SoloNumeroTelefonico);
        $("#apellidopaterno").keypress(PermitirSoloLetras);
        $("#apellidomaterno").keypress(PermitirSoloLetras);
        
        var validator = $('#frmAfiliacion').validate({
            rules: {
                CodigoTipoPersona: "required",
                codigorubronegocio: {
                    validarrubronegocio: true
                },
                numeroruc: {
                    validarpersonajuridica: true
                },
                razonsocial: {
                    validarpersonajuridica: true
                },
                numerodni: {
                    required: true,
                    minlength: 8
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
                codigodepartamento: {
                    validarseleccion: true
                },
                codigoprovincia: {
                    validarseleccion: true
                },
                codigodistrito: {
                    validarseleccion: true
                },
                via: "required"
            },
            messages: {
                CodigoTipoPersona: "Por favor seleccione el Tipo de Persona correspondiente.",
                numeroruc: {
                    validarpersonajuridica: "Al seleccionar Persona Juridica debe ingresar el numero de RUC."
                }, 
                razonsocial: {
                    validarpersonajuridica: "Al seleccionar Persona Juridica debe ingresar la Razon Social segun SUNAT."
                },
                numerodni: {
                    required: "Por favor ingrese un numero de DNI.",
                    minlength: "Por favor ingrese numero de DNI valido."
                },
                nombres: "Por favor ingrese un nombre.",
                apellidopaterno: "Por favor ingrese un Apellido Paterno",
                apellidomaterno: "Por favor ingrese un Apellido Materno",
                Sexo: "Por favor seleccione el sexo correspondiente.",
                fechanacimiento: {
                    required: "Por favor ingrese una fecha valida"
                },
                codigodepartamento: {
                    validarseleccion: "Por favor seleccione el Departamento al cual pertenece la direccion."
                },
                codigoprovincia: {
                    validarseleccion: "Por favor seleccione la Provincia a la cual pertenece la direccion."
                },
                codigodistrito: {
                    validarseleccion: "Por favor seleccione el Distrito al cual pertenece la direccion."
                },
                via: "Por favor ingrese una direccion"
            }
        });

        $('#btnHistorico').click(function () {
            ConsultarDomicilios();
        });

        $('#btnRegistrar').click(function () {
            var $valid = $('#frmAfiliacion').valid();
            
            if (!$valid) {
                return false;
            } else {
                var titulo = $("#EstadoRegistro").val() === "N" ? "Desea registrar los Datos Personales?" : "Desea actualizar el domicilio?";

                swal({
                    title: titulo,
                    text: "Se realizara el registro de los datos ingresados.",
                    type: "info",
                    showCancelButton: true,
                    confirmButtonClass: "btn-primary",
                    confirmButtonText: "Si, registrar",
                    closeOnConfirm: false
                },
                    function () {
                        Registrar();
                });
            }
        });

    });

    $(document).bind("contextmenu", function (e) {
        return false;
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
                    var content = '<option value="' + type.Value + '">' + type.Text + '</option>';
                    $(id).append(content);
                });
        });
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
    
});

function Deshabilitar() {

    $("[name='CodigoTipoPersona']").prop('disabled', true);
    $("#codigorubronegocio").prop('disabled', true);
    $("#otrorubronegocio").prop('disabled', true);
    $("#numeroruc").prop('disabled', true);
    $("#razonsocial").prop('disabled', true);
    $("#numerodni").prop('disabled', true);
    $("#nombres").prop('disabled', true);
    $("#apellidopaterno").prop('disabled', true);
    $("#apellidomaterno").prop('disabled', true);
    $("[name='Sexo']").prop('disabled', true);
    $("#fechanacimiento").prop('disabled', true);
    $("#telefonofijo").prop('disabled', true);
    $("#telefonomovil").prop('disabled', true);

}

function MostrarDivJuridica(valor) {
    if (valor) {
        $('#DivJuridica').show();
    } else {
        $('#DivJuridica').hide();
    }
}

function SoloNumeros(e) {
    if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {        
        return false;
    }
}

function SoloNumeroTelefonico(e) {
    var regex = new RegExp("^[0-9-]+$");
    var key = String.fromCharCode(!e.charCode ? e.which : e.charCode);
    if (!regex.test(key)) {
        e.preventDefault();
        return false;
    }
}

function PermitirSoloLetras(e) {
    var regex = new RegExp("^[a-zA-Z \b%.]+$");
    var key = String.fromCharCode(!e.charCode ? e.which : e.charCode);
    if (!regex.test(key)) {
        e.preventDefault();
        return false;
    }
}

function PermitirSoloLetrasyNumeros(e) {
    var regex = new RegExp("^[a-zA-Z0-9\b]+$");
    var key = String.fromCharCode(!e.charCode ? e.which : e.charCode);
    if (!regex.test(key)) {
        e.preventDefault();
        return false;
    }
}

function Registrar() {

    var DatosPersonalesVM = new Object();
    
    DatosPersonalesVM.IdUsuarioZiPago = $('#idusuariozipago').val();
    DatosPersonalesVM.Clave1 = $('#clave1').val();
    DatosPersonalesVM.CodigoRubroNegocio = $('#codigorubronegocio').val();
    DatosPersonalesVM.OtroRubroNegocio = $('#otrorubronegocio').val();
    DatosPersonalesVM.CodigoTipoPersona = $('input:radio[name=CodigoTipoPersona]:checked').val();
    DatosPersonalesVM.NumeroDocumento = $('#numeroruc').val();
    DatosPersonalesVM.NumeroDocumentoContacto = $('#numerodni').val();
    DatosPersonalesVM.RazonSocial = $('#razonsocial').val();
    DatosPersonalesVM.ApellidoPaterno = $('#apellidopaterno').val();
    DatosPersonalesVM.ApellidoMaterno = $('#apellidomaterno').val();
    DatosPersonalesVM.Nombres = $('#nombres').val();
    DatosPersonalesVM.Sexo = $('input:radio[name=Sexo]:checked').val();
    DatosPersonalesVM.FechaNacimiento = $('#fechanacimiento').val();
    DatosPersonalesVM.TelefonoMovil = $('#telefonomovil').val();
    DatosPersonalesVM.TelefonoFijo = $('#telefonofijo').val();
    DatosPersonalesVM.EstadoRegistro = $('#EstadoRegistro').val();
    DatosPersonalesVM.CodigoDepartamento = $('#departamento').val();
    DatosPersonalesVM.CodigoProvincia = $('#provincia').val();
    DatosPersonalesVM.CodigoDistrito = $('#distrito').val();
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
            swal("Datos Personales registrados correctamente", resp.mensaje, "success");
            $("#EstadoRegistro").val("R");
            Deshabilitar();
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