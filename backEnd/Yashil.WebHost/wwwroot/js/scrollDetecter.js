const topMenuContainer = document.querySelector('.menu-position');
var lastScrollTop = 0;
function scrollDetecter() {
    var scrollTop = window.pageYOffset || document.documentElement.scrollTop;
    if (scrollTop > lastScrollTop) {
        // downscroll code
        topMenuContainer.classList.add('header-banner');
    } else if (scrollTop < lastScrollTop && window.pageYOffset <= 65) {
        // upscroll code
        topMenuContainer.classList.remove('header-banner');
    } // else was horizontal scroll
    lastScrollTop = scrollTop <= 0 ? 0 : scrollTop; // For Mobile or negative scrolling
}