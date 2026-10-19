// Initialize City List Dropdown
createSelect2Dropdown('#city-list', 'لطفا شهر را انتخاب کنید');

// Show Representation List of The Selected City
$('#city-list').on('select2:select', retrieveSelection)

// Get City List Value and Load Representation List Based on That
function retrieveSelection(e) {
    const data = e.params.data;
    const cityId = data.id;
    loadRepresentations(cityId)
}

// Load Representation List
function loadRepresentations(cityId) {
    $('#representation-list--container').empty();
    $('#representation-list--container').append($('<div class="spinner-border" role="status"></div>').html('<span class="visually-hidden"></span>'));
    $.get(representionComponentUrl + '?cityId=' + cityId + '&companyId=' + companyId, function (data) {
        // Assuming 'container' is a div element in your main view where you want to place the second ComboBox
        $('#representation-list--container').html(data); // This inserts the partial view returned by the action method into the container

        // Initialize Representation List Dropdown
        createSelect2Dropdown('#representation-list', 'لطفا نمایندگی را انتخاب کنید');
    });
}

// درصورتیکه که تعداد کارکترهای موجود در توضیحات محصول بیشتر از 568 باشد، توضیحات به دو قسمت تقسیم شده و بخش اول آن نمایش داده میشود و با کلیک روی دکمه مشاهده بیشتر میتوان بقیه توضیحات را مشاهده کرد
const productDescriptionElement = document.querySelector(".expertReview");
const showMoreButton = document.querySelector(".expert-showMore");
showMoreButton.addEventListener("click", function () {
    if (!productDescriptionElement.classList.contains("expanded")) {
        showMoreButton.querySelector("span").textContent = "بستن";
        productDescriptionElement.classList.add("expanded");
    } else if (productDescriptionElement.classList.contains("expanded")) {
        showMoreButton.querySelector("span").textContent = "مشاهده بیشتر";
        productDescriptionElement.classList.remove("expanded");
        window.scrollTo({
            top: 0,
            behavior: "smooth",
        });
    }
});
productDescriptionElement.firstElementChild.clientHeight < 500 ? showMoreButton.remove() : '';

if (productDescriptionElement.firstElementChild.clientHeight < 500) {
    productDescriptionElement.style.height = 'max-content';
}

//END. length of description content of product



// بخش تگ های مرتبط از اکشن کنترلر فراخوانی میشود و وقتی حالت دسکتاپ باشد یک بار درخواست ارسال میشود + در حالت موبایل پنهان میشود همچنین در حالت موبایل درخواست جداگانه برای لود کامپوننت ارسال میشود + در حالت دسکتاپ مخفی میشود 
const relatedProductTagDesktop = document.querySelector('#inner-product-tags-desktop');
const relatedProductTagMobile = document.querySelector('#inner-product-tags-mobile');
const isMobileDevice = window.innerWidth <= 992;
async function setRelatedProductTags() {

    const url = generateUrl();
    try {
        if (url) {
            const response = await fetch(url, {
                method: 'GET'
            })
            if (!response.ok) {
                throw new Error('بروز خطا در هنگام دریافت اطلاعات');
            }
            const data = await response.text();
            if (isMobileDevice) {
                relatedProductTagMobile.innerHTML = data;
            }
            else {
                relatedProductTagDesktop.innerHTML = data;
            }
        } else {
            console.warn('آدرس مورد نیاز جهت دریافت اطلاعات فراهم نشده است');
        }

    } catch (error) {
        console.error('بروز خطا:', error);
    }
}
window.addEventListener('DOMContentLoaded', async function () {
    await setRelatedProductTags();
})
//END. load tag component


//کانفیگ سوییپر اسلایدر دوره های مرتبط به محصول
productListCarousel("CategoriesProductSalePlanning", 0, {
    320: {
        slidesPerView: 1.2
    },
    360: {
        slidesPerView: 1.5
    },
    402: {
        slidesPerView: 1.8
    },
    502: {
        slidesPerView: 2.1
    },
    640: {
        slidesPerView: 2.2
    },
    768: {
        slidesPerView: 2.5
    },
    992: {
        slidesPerView: 3.7
    },
    1024: {
        slidesPerView: 3.9
    },
    1200: {
        slidesPerView: 4.5
    },
    1400: {
        slidesPerView: 5.1
    }
})
//END. swiper related product