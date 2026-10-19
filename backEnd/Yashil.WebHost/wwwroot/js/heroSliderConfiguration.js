swiper = new Swiper(".swiper-slider", {
    // Optional parameters
    centeredSlides: true,
    spaceBetween: 50,
    slidesPerView: 1,
    grabCursor: true,
    freeMode: false,
    loop: false,
    mousewheel: false,
    keyboard: {
        enabled: true
    },

    // Enabled autoplay mode
    autoplay: {
        delay: 5000,
        disableOnInteraction: false
    },

    // If we need pagination
    pagination: {
        el: ".swiper-pagination",
        dynamicBullets: false,
        clickable: true
    },

    // If we need navigation
    navigation: {
        nextEl: ".swiper-button-next",
        prevEl: ".swiper-button-prev"
    },


});