/*=============================
                Preloder
        ==============================*/

(function ($) {
    "use strict";

    var $window = $(window),
        $body = $('body');



    /*=============================
               Preloder
       ==============================*/
    $window.on('load', function () {

        $('.spinner').fadeOut();
        $('.preloader').delay(350).fadeOut(500);
        $body.delay(350).css({
            'overflow': 'visible'
        });

    });




}(jQuery));