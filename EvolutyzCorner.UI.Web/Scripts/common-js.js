/**
 * common-js.js
 * 
 * Copyright 2018 - Giridhar K.s
 *
 */
!function($) {
    $('#mySidenav').hide();
    

    //$("img").error(function () {
    //    $(this).unbind("error").attr("src", "/Content/images/user.png");
    //});

    //$("img").on("error", function () {
    //    $(this).unbind("error").attr("src", "/Content/images/user.png");
    //});
    $('img').on("error", function () {
        $(this).attr('src', '/Content/images/user.png');
    });
    //$('[data-toggle="modal"]').click(function () {
    //    $(".modal").modal({ backdrop: "static", keyboard: false });
    //});
}(window.jQuery);

$(window).load(function () {
    $('img').each(function () {
        if (!this.complete || typeof this.naturalWidth === "undefined" || this.naturalWidth === 0) {
            this.src = '/Content/images/user.png';
        }
    });
});