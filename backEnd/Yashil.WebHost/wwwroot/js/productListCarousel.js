let breakpoints = {};
const productListCarousel = function (id = undefined, spaceBetween, slidesPerView) {
    breakpoints = slidesPerView;
    new Swiper(`#swiperContainer${id}`, {
        freeMode: {
            enabled: true
        },
        spaceBetween,
        breakpoints,
        direction: getDirection(),
        navigation: {
            nextEl: '.swiper-button-next',
            prevEl: '.swiper-button-prev',
        }
    });


    function getDirection() {
        var direction = window.innerWidth <= 760 ? 'horizontal' : 'horizontal';
        return direction;
    }
}

