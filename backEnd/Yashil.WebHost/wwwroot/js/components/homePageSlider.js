const heroSwiperElement = document.querySelector('.swiper-slider');
const swiperNavigationButtonsElement = document.querySelector('.swiper-navigation-buttons');

new Swiper(".swiper-slider", {
    // Optional parameters
    spaceBetween: 50,
    slidesPerView: 1,
    loop: true,

    // Enabled autoplay mode
    autoplay: {
        delay: 5000,
    },

    // If we need pagination
    pagination: {
        el: ".swiper-pagination",
        clickable: 'true',
    },

    // Navigation arrows
    navigation: {
        nextEl: '.swiper-button-next',
        prevEl: '.swiper-button-prev',
    },
});

heroSwiperElement.addEventListener('mouseenter', function () {
    swiperNavigationButtonsElement.classList.remove('opacity-0');
});
heroSwiperElement.addEventListener('mouseleave', function () {
    swiperNavigationButtonsElement.classList.add('opacity-0');
});