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

});

function MostrarDivJuridica(valor) {
    if (valor) {
        $('#DivJuridica').show();        
    } else {
        $('#DivJuridica').hide();        
    }
}