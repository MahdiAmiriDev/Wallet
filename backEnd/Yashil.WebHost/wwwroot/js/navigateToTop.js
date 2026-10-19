
var goTop = function () {
    $(window).scroll(function () {
        var scrollTopEl = $('#scroll-top');
        if ($(this).scrollTop() > 100) {
            scrollTopEl.addClass('show');
        } else {
            scrollTopEl.removeClass('show');
        }
    });

    $("#scroll-top").on("click", function () {
        $("html, body").animate({
            scrollTop: 0
        }, 600);
        return false;
    });
};





