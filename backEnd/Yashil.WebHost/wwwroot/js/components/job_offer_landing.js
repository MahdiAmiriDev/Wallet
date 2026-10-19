const jobOfferSearchFormElement = document.getElementById('job-offer-search--form');

const resumeStatusCompletionContainerElement = document.querySelector('.resume-status-completion--container');
const resumeStatusCompletionProgressElement = document.querySelector('.resume-status--completion');
const resumeStatusRateElement = document.querySelector('.resume-status-rate');

/***** Job Offer Search *****/
// Initialize Province List Dropdown
createSelect2Dropdown('#province-list--input', 'همه استان‌ها');

// Initialize Category List Dropdown
createSelect2Dropdown('#category-list--input', 'همه دسته بندی‌ها');

jobOfferSearchFormElement.addEventListener('submit', function (event) {
    event.preventDefault();

    // Create a FormData instance
    const formData = new FormData(event.target);
    let allInputsEmptyOrZero = true;

    // Create a FormData instance of params which have a value
    const queryParams = new FormData();
    for (let [key, value] of formData.entries()) {
        if (value.trim() !== '' && value !== '0') {
            queryParams.append(key, value);
            allInputsEmptyOrZero = false;
        }
    }

    // Convert FormData to URLSearchParams
    const params = new URLSearchParams(queryParams).toString();

    // Construct the new URL with query parameters
    const baseUrl = '/jobOffer'; // Replace with your actual base URL
    let newUrl;

    if (allInputsEmptyOrZero) {
        newUrl = baseUrl
    } else {
        newUrl = `${baseUrl}?${params}`
    }

    // Redirect to the new URL
    window.location.href = newUrl;
})
/***** Job Offer Search *****/

/***** Job Offers By Company *****/
new Swiper('.corporation-requirements--swiper', {
    loop: false,
    speed: 600,
    autoplay: {
        delay: 3000,
    },
    slidesPerView: 1.7,
    spaceBetween: 15,
    breakpoints: {
        // when window width is >= 320px
        350: {
            slidesPerView: 2
        },
        // when window width is >= 320px
        425: {
            slidesPerView: 2.4
        },
        // when window width is >= 320px
        500: {
            slidesPerView: 2.7
        },
        // when window width is >= 320px
        600: {
            slidesPerView: 3
        },
        // when window width is >= 320px
        750: {
            slidesPerView: 3.8
        },
        // when window width is >= 320px
        900: {
            slidesPerView: 4.7
        },
        // when window width is >= 320px
        1000: {
            slidesPerView: 5.5
        },
        // when window width is >= 320px
        1200: {
            slidesPerView: 6.5
        },
        // when window width is >= 320px
        1400: {
            slidesPerView: 7.5
        },
        // when window width is >= 320px
        1600: {
            slidesPerView: 8.5
        },
    },
    navigation: {
        nextEl: '.swiper-button-next',
        prevEl: '.swiper-button-prev',
    },
});
/***** Job Offers By Company *****/

/***** User Resume Status *****/
if (!userName) {
    resumeStatusCompletionContainerElement.innerHTML = "<p class='user-not-logged'>لطفا برای مشاهده وضعیت رزومه، وارد حساب کاربری خود شوید.</p>"
}

const completionPercentage = +resumeCompletionPercentage;
let progressCurrentValue = 0;
let resumeCompletionCounter = completionPercentage && setInterval(updatePercentage, 20);

// نمایش درصد تکمیل بودن رزومه
// counter به شکل
function updatePercentage() {
    ++progressCurrentValue;
    resumeStatusRateElement.innerHTML = progressCurrentValue;
    resumeStatusCompletionProgressElement.value = progressCurrentValue;
    const colors = generateColorByPercentage(progressCurrentValue);
    setProgressColor(colors);
    if (progressCurrentValue === completionPercentage) {
        clearInterval(resumeCompletionCounter);
    }
}

// از رنگ‌ها object ساخت یک
// براساس درصد تکمیل بودن رزومه
function generateColorByPercentage(percentage) {
    /* Color Values For Different Percentages
        Red Shade(0 - 50):
            Dark => #c1121f
            Ligth => #c1121f19
    
        Yellow Shade(50 - 70):
            Dark => #fca311
            Light => #fca31119
    
        Green Shade(70 - 100):
            Dark => #55a630
            Light => #55a63019
    */
    if (percentage < 50) {
        return {
            dark: '#c1121f',
            light: '#c1121f19'
        }
    } else if (percentage >= 50 && percentage < 75) {
        return {
            dark: '#fca311',
            light: '#fca31119'
        }
    } else {
        return {
            dark: '#55a630',
            light: '#55a63019'
        }
    }
}

// progress تنظیم رنگ المان
// براساس رنگ‌های دریافت شده
function setProgressColor(colors) {
    document.documentElement.style.setProperty('--progress-bar', colors.light);
    document.documentElement.style.setProperty('--progress-value', colors.dark);
}
/***** User Resume Status *****/