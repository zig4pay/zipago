jQuery(function ($) {

    $.fn.select2.defaults.set('language', 'es');
    $('.select2').select2();

    $(document).ready(function () {

        $('.datepicker').datepicker({
            autoclose: true,
            language: "es",
            format: 'dd/mm/yyyy'
        });

        $(".datepicker").datepicker("update", new Date());        

    });

});