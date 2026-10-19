const advertisementBannersContainerElement = document.querySelector('.four_skills--container');
const allBackdropElements = document.querySelectorAll('.homepage-banner-description__backdrop');
const allBannerDescriptionElements = document.querySelectorAll('.homepage-banner-description__container');

// مخفی کردن توضیحات و
// هایی که از قبل درحال نمایش هستندbackdrop
function hideAllBackdropsAndDescriptions() {
    for (let index = 0; index < allBackdropElements.length; index++) {
        allBackdropElements[index].classList.remove('show');
        allBannerDescriptionElements[index].classList.remove('show');
    }
}

// نمایش توضیحات و
// بنری که backdrop
// میکند hover موس روی آن
advertisementBannersContainerElement.addEventListener('mouseover', function (e) {
    // اضافه شدن سه نقطه به آخر توضیحات بنرهای تبلیغات که تعداد کاراکتر  توضیحات آن ها بیش از  150 باشد
    shrinkText('.homepage-banner__description', 150);
    const targetElement = e.target;
    if (targetElement.classList.contains('homepage-banner__container')) {
        hideAllBackdropsAndDescriptions();

        const descriptionBackdropElement = targetElement.querySelector('.homepage-banner-description__backdrop');
        const descriptionContainerElement = targetElement.querySelector('.homepage-banner-description__container');
        descriptionBackdropElement.classList.add('show');
        descriptionContainerElement.classList.add('show');
    }
})

// مخفی کردن توضیحات و
// هایی که از قبل درحال نمایش هستندbackdrop
// هنگامی که موس از روی بنرها کنار میرود
advertisementBannersContainerElement.addEventListener('mouseleave', hideAllBackdropsAndDescriptions)