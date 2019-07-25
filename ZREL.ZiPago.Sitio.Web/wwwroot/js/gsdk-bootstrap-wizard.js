
searchVisible = 0;
transparent = true;

jQuery(function ($) {
    $.validator.addMethod("validarseleccion", ValidarSeleccion);
    $.validator.addMethod("validarrubronegocio", ValidarRubroNegocio);
    $.validator.addMethod("validarruc", ValidarRUC);
    $.validator.addMethod("validarrazonsocial", ValidarRazonSocial);
    $.validator.addMethod("validaredad", ValidarEdad);
});

$(document).ready(function(){

    /*  Activate the tooltips      */
    $('[rel="tooltip"]').tooltip();

    $("#correonotificacion").val($("#clave1").val());
        
    // Code for the Validator
    var $validator = $('.wizard-card form').validate({
        rules: {
            CodigoTipoPersona: "required",
            codigorubronegocio: {
                validarrubronegocio : true
            },
            numeroruc: {
                validarruc: true
            },
            razonsocial: {
                validarrazonsocial: true
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
            optSexo: "required",
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
            via: "required",
            referencia: "required"
        },
        messages: {
            CodigoTipoPersona: "Por favor seleccione el Tipo de Persona correspondiente.",   
            codigorubronegocio: {
                validarrubronegocio: "Por favor seleccione el Rubro de Negocio al cual pertenece, en caso no lo encuentre ingreselo en la casilla Otro." 
            },                
            numeroruc: {
                validarruc: "Al seleccionar Persona Juridica debe ingresar el numero de RUC."
            },
            razonsocial: {
                validarrazonsocial: "Al seleccionar Persona Juridica debe ingresar la Razon Social segun SUNAT."
            },
            numerodni: {
                required: "Por favor ingrese un numero de DNI.",
                minlength: "Por favor ingrese numero de DNI valido."
            },
            nombres: "Por favor ingrese un nombre.",
            apellidopaterno: "Por favor ingrese un Apellido Paterno",
            apellidomaterno: "Por favor ingrese un Apellido Materno",
            optSexo: "Por favor seleccione el sexo correspondiente.",
            fechanacimiento: {
                required: "Por favor ingrese una fecha valida",
                validaredad: "Para registrarse debe ser mayor de 18 años."
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
            via: "Por favor ingrese una direccion",
            referencia: "Por favor ingrese una referencia valida"
        }
	});

    //called when key is pressed in textbox
    $("#numeroruc").keypress(SoloNumeros);
    $("#numerodni").keypress(SoloNumeros);
    $("#telefonofijo").keypress(SoloNumeroTelefonico);
    $("#telefonomovil").keypress(SoloNumeroTelefonico);
    $("#apellidopaterno").keypress(PermitirSoloLetras);
    $("#apellidomaterno").keypress(PermitirSoloLetras);
    $("#numerocuenta").keypress(SoloNumeroTelefonico);
    $("#cci").keypress(SoloNumeroTelefonico);
    $("#codigocomercio").keypress(PermitirSoloLetrasyNumeros);
    
    // Wizard Initialization
    $('.wizard-card').bootstrapWizard({
        'tabClass': 'nav nav-pills',
        'nextSelector': '.btn-next',
        'previousSelector': '.btn-previous',

        onNext: function (tab, navigation, index) {

        	var $valid = $('.wizard-card form').valid();
        	if(!$valid) {
        		$validator.focusInvalid();
        		return false;
            }

        },

        onInit : function(tab, navigation, index){

            //check number of tabs and fill the entire row
            var $total = navigation.find('li').length;
            $width = 100/$total;
            var $wizard = navigation.closest('.wizard-card');

            $display_width = $(document).width();

            if($display_width < 600 && $total > 3){
                $width = 50;
            }

            navigation.find('li').css('width',$width + '%');
            $first_li = navigation.find('li:first-child a').html();
            $moving_div = $('<div class="moving-tab">' + $first_li + '</div>');
            $('.wizard-card .wizard-navigation').append($moving_div);
            refreshAnimation($wizard, index);
            $('.moving-tab').css('transition','transform 0s');
        },

        onTabClick: function (tab, navigation, index, selectedIndex) {

            var $valid = $('.wizard-card form').valid();
            
            if (!$valid) {
                return false;
            } else {
                //if (index === 1 && selectedIndex === 2 && $("#tblCuentas tr").length < 2) {
                //    return false;
                //} else {
                    return true;
                //}                               
            }  
            
        },      

        onTabShow: function (tab, navigation, index) {
            
            var $total = navigation.find('li').length;
            var $current = index+1;

            var $wizard = navigation.closest('.wizard-card');

            // If it's the last tab then hide the last button and show the finish instead
            if ($current >= $total) {
                $($wizard).find('.btn-next').hide();
                $($wizard).find('.btn-finish').show();
            //} else if ($current === 2 && $("#tblCuentas tr").length  < 2) {                
            //    $($wizard).find('.btn-next').hide();   
            //    $($wizard).find('.btn-finish').hide();
            } else {
                $($wizard).find('.btn-next').show();
                $($wizard).find('.btn-finish').hide();
            }

            button_text = navigation.find('li:nth-child(' + $current + ') a').html();

            setTimeout(function(){
                $('.moving-tab').text(button_text);
            }, 150);

            var checkbox = $('.footer-checkbox');

            if( !index == 0 ){
                $(checkbox).css({
                    'opacity':'0',
                    'visibility':'hidden',
                    'position':'absolute'
                });
            } else {
                $(checkbox).css({
                    'opacity':'1',
                    'visibility':'visible'
                });
            }

            refreshAnimation($wizard, index);
        }
    });
    
    // Prepare the preview for profile picture
    $("#wizard-picture").change(function(){
        readURL(this);
    });

    $('[data-toggle="wizard-radio"]').click(function(){
        wizard = $(this).closest('.wizard-card');
        wizard.find('[data-toggle="wizard-radio"]').removeClass('active');
        $(this).addClass('active');
        $(wizard).find('[type="radio"]').removeAttr('checked');
        $(this).find('[type="radio"]').attr('checked','true');
    });

    $('[data-toggle="wizard-checkbox"]').click(function(){
        if( $(this).hasClass('active')){
            $(this).removeClass('active');
            $(this).find('[type="checkbox"]').removeAttr('checked');
        } else {
            $(this).addClass('active');
            $(this).find('[type="checkbox"]').attr('checked','true');
        }
    });

    $('.set-full-height').css('height', 'auto');

});


 //Function to show image before upload

function readURL(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#wizardPicturePreview').attr('src', e.target.result).fadeIn('slow');
        }
        reader.readAsDataURL(input.files[0]);
    }
}

$(window).resize(function(){
    $('.wizard-card').each(function(){
        $wizard = $(this);
        index = $wizard.bootstrapWizard('currentIndex');
        refreshAnimation($wizard, index);

        $('.moving-tab').css({
            'transition': 'transform 0s'
        });
    });
});

function refreshAnimation($wizard, index){
    total_steps = $wizard.find('li').length;
    move_distance = $wizard.width() / total_steps;
    step_width = move_distance;
    move_distance *= index;

    $wizard.find('.moving-tab').css('width', step_width);
    $('.moving-tab').css({
        'transform':'translate3d(' + move_distance + 'px, 0, 0)',
        'transition': 'all 0.3s ease-out'

    });
}

function debounce(func, wait, immediate) {
	var timeout;
	return function() {
		var context = this, args = arguments;
		clearTimeout(timeout);
		timeout = setTimeout(function() {
			timeout = null;
			if (!immediate) func.apply(context, args);
		}, wait);
		if (immediate && !timeout) func.apply(context, args);
	};
}

function ValidarRubroNegocio(valor) {
    if (valor === "000" && $("#otrorubronegocio").val().trim() === "") {
        return false;
    } else {
        return true;
    }
}

function ValidarSeleccion(valor) {
    if (valor === "" || valor === "00" || valor === "000" || valor === "XX" || valor === 0) {
        return false;
    } else {
        return true;
    }
}

function SoloNumeros (e) {    
    if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {        
        //$("#errmsg").html("Digits Only").show().fadeOut("slow");
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


function ValidarRUC(valor) {
    if ($("#optPersonaJuridica").is(":checked") && valor.trim() == "") {
        return false;   
    } else {
        return true;
    }
}

function ValidarRazonSocial(valor) {
    if ($("#optPersonaJuridica").is(":checked") && valor.trim() == "") {
        return false;
    } else {
        return true;
    }
}

function ValidarEdad(fecha) {

    var hoy = new Date();
    var cumpleanos = new Date(fecha);
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

    

}        