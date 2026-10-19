const searchInputPlaceholder = document.getElementById('search-component__placeholder');
const closeSearchComponentButton = document.getElementById('close-search-component');

const searchComponentFormElement = document.getElementById('search-component__form');
const searchInputElement = document.getElementById('search-input');
const clearSearchInputButtonElement = document.getElementById('clear-search-input__button');
const searchResultsLoadingContainerElement = document.getElementById('search-results-loader__container');
const searchResultsBannerContainerElement = document.getElementById('search-results-banner__container');
const searchResultsContainerElement = document.getElementById('search-results')

let searchQuery;
let shouldInitiateSearch = false;
const getSearchResultsURL = "/api/ProductSalesPlanning/Search?searchKey=";
const PRODUCT_TITLE = "عنوان محصول";
const COMPANY_NAME = "مراکز آموزشی";
const CATEGORY_NAME = "دسته بندی";

new Swiper(".popular-searches", {
    // Optional parameters
    slidesPerView: 'auto',
    spaceBetween: 8,

    // Navigation arrows
    navigation: {
        nextEl: '.popular-searches__next',
        prevEl: '.popular-searches__previous',
    },
});

// میشود submit هنگامی که فرم جستجو
// کاربر به صفحه جستجو با
// مقدار وارد
searchComponentFormElement.addEventListener('submit', function (e) {
    e.preventDefault();
    const searchInputElement = document.getElementById('search-input');
    const searchQueryValue = searchInputElement.value;

    const queryParams = new URLSearchParams(window.location.search);
    if (queryParams.has('searchTitle')) {
        queryParams.set('searchTitle', searchQuery)
    } else {
        queryParams.append('searchTitle', searchQuery);
    }

    const newParams = new URLSearchParams(queryParams).toString();

    const baseUrl = '/Company';
    const newUrl = `${baseUrl}?${newParams}`
    if (searchQueryValue.length > 1) window.location.href = newUrl;
})

// با کلیک بر روی
// search input
// کامپوننت جستجو باز میشود
searchInputPlaceholder.addEventListener('click', function () {
    searchComponentElement.classList.add('show');
    fadeOverlay.classList.add('show');
    headerEl.classList.add('hovered');
    mainContent.classList.add('overflow-hidden');
    $('.wide-nav-header').css('padding-right', function (index, curValue) {
        return parseInt(curValue, 10) + 14 + 'px';
    });
    setTimeout(function () {
        searchInputElement.focus();
    }, 100);
})

// در مود موبایل با کلیک
// بر روی دکمه بازگشت
// کامپوننت جستجو بسته میشود
closeSearchComponentButton.addEventListener('click', function () {
    searchComponentElement.classList.remove('show');
    fadeOverlay.classList.remove('show');
    mainContent.classList.remove('overflow-hidden')
})

// به تاخیر انداختن و کاهش
// های فرستاده شدهrequest تعداد
// هنگامی که کاربر عنوان مورد نظر
// خود را جهت جستجو وارد میکند
function debounceSearchRequest(func, waitAmount) {
    let timeout;
    return function executedFunction(...argumentss) {
        const later = () => {
            clearTimeout(timeout);
            func(...argumentss);
        };
        clearTimeout(timeout);
        timeout = setTimeout(later, waitAmount);
    };
}

// ارسال مقدار جستجوی وارد شده
// debounce function در فیلد جستجو به
const debouncedSearchHandle = debounceSearchRequest((query) => {
    if (shouldInitiateSearch) {
        handleSearchResquest(query)
    }
}, 500);

// نمایش دکمه خالی کردن فیلد جستجو
// درصورت تایپ متن داخل آن
searchInputElement.addEventListener('input', function (e) {
    searchQuery = e.target.value.trim();

    shouldInitiateSearch = !!searchQuery;

    if (searchQuery.length > 1 && shouldInitiateSearch) {
        clearSearchInputButtonElement.classList.add('show');
        debouncedSearchHandle(searchQuery)
    } else {
        clearSearchInputButtonElement.classList.remove('show');
        hideSearchResultsContainer();
        showSearchResultsBanner();
    }
})

// با کلیک بر روی این دکمه
// فیلد جستجو خالی میشود
clearSearchInputButtonElement.addEventListener('click', function () {
    searchInputElement.value = "";
    clearSearchInputButtonElement.classList.remove('show');
    showSearchResultsBanner();
    hideSearchResultsContainer();
})


// ارسال درخواست جستجو با استفاده از
// وارد شده searchQuery
async function handleSearchResquest(searchQuery) {
    hideSearchResultsBanner();
    showLoadingSpinner();
    try {
        const response = await fetch(getSearchResultsURL + searchQuery);

        // نمایش این خطا به هنگام بروز مشکل در سرور
        if (!response.ok) throw new Error("در بارگیری نتایج جستجو خطایی رخ داد");

        const searchResults = await response.json();
        renderSearchResults(searchResults);
    } catch (error) {
        console.error(error.message);
    } finally {
        hideLoadingSpinner();
    }
}

// Loading Spinner نمایش
function showLoadingSpinner() {
    searchResultsLoadingContainerElement.classList.remove('d-none')
}

// Loading Spinner مخفی کردن
function hideLoadingSpinner() {
    searchResultsLoadingContainerElement.classList.add('d-none')
}

// نمایش عکس موجود در کامپوننت جستجو
function showSearchResultsBanner() {
    searchResultsBannerContainerElement.classList.remove('d-none')
}

// مخفی کردن عکس موجود در کامپوننت جستجو
function hideSearchResultsBanner() {
    searchResultsBannerContainerElement.classList.add('d-none')
}

// پاک و مخفی کردن نتایج جستجو
function hideSearchResultsContainer() {
    searchResultsContainerElement.classList.add('d-none');
    searchResultsContainerElement.innerHTML = ""
}

// نمایش نتایج جستجو
function renderSearchResults(searchResults) {
    hideSearchResultsContainer();
    searchResultsContainerElement.classList.remove('d-none');
    generateSearchResultsTemplate(searchResults);
}

// html ساخت
// نتایج جستجو
function generateSearchResultsTemplate(searchResults) {
    const resultInCategory = searchResults.searchInCategoryResult.slice(0, 4);
    const resultInInCompany = searchResults.searchInCompanyResult.slice(0, 4);
    const resultInProductSalesPlanning = searchResults.searchInProductSalesPlanningResult.slice(0, 4);

    // اضافه کردن دکمه جستجو با عنوان
    // وارد شده در محصولات به کامپوننت سرچ
    searchResultsContainerElement.insertAdjacentHTML('beforeend', generateMakeSearchWithSearchQueryTemplate())

    // درصورتی که حداقل برای یکی از
    // دسته‌ها نتیجه‌ای وجود داشته باشد
    // نتایج نمایش داده خواهند شد
    if (resultInProductSalesPlanning.length > 0 || resultInInCompany.length > 0 || resultInCategory.length > 0) {
        if (resultInProductSalesPlanning.length > 0) {
            searchResultsContainerElement.insertAdjacentHTML('beforeend', generateSearchResultSectionTemplate(PRODUCT_TITLE, resultInProductSalesPlanning))
        }
        if (resultInInCompany.length > 0) {
            searchResultsContainerElement.insertAdjacentHTML('beforeend', generateSearchResultSectionTemplate(COMPANY_NAME, resultInInCompany))
        }
        if (resultInCategory.length > 0) {
            searchResultsContainerElement.insertAdjacentHTML('beforeend', generateSearchResultSectionTemplate(CATEGORY_NAME, resultInCategory))
        }
    } else {
        // زمانی نمایش داده میشود که
        // نتیجه‌ای برای عبارت جستجو شده
        // وجود نداشته باشد
        searchResultsContainerElement.innerHTML = "<p class='no-results-found__message'>برای عبارت وارد شده نتیجه‌ای یافت نشد.</p>"
    }
}

// html ساخت
// بخش‌های مختلف نتایج جستجو
function generateSearchResultSectionTemplate(sectionTitle, results) {
    function renderSectionContent() {
        switch (sectionTitle) {
            case PRODUCT_TITLE:
                return results.map(result => generateSearchResultItemInProductTitleTemplate(result)).join('');
            case COMPANY_NAME:
                return results.map(result => generateSearchResultItemInCompanyTemplate(result)).join('');
            case CATEGORY_NAME:
                return results.map(result => generateSearchResultItemInCategoryTemplate(result)).join('');
        }
    };

    return `
        <div class="search-result-section border-bottom mb-4">
                    <div class="d-flex align-items-center gap-2">
                        <h3 class="search-result-section--title mb-0">${sectionTitle}</h3>
                    </div>
                    <div>
                        ${renderSectionContent()}
                    </div>
                </div>
    `
}

// html ساخت
// جستجو در محصولات با مقدار داده شده
function generateMakeSearchWithSearchQueryTemplate() {
    return `
        <div class="d-flex align-items-center border-bottom mb-4">
            <a href="/Company?searchTitle=${searchQuery}" class="search-item d-flex align-items-center gap-3">
                        <div class="d-flex align-items-center justify-content-center search-item-icon--container">
                            <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor" fill="none" stroke-linecap="round" stroke-linejoin="round">
                              <path stroke="none" d="M0 0h24v24H0z" fill="none"/>
                              <path d="M3 21l18 0" />
                              <path d="M3 7v1a3 3 0 0 0 6 0v-1m0 1a3 3 0 0 0 6 0v-1m0 1a3 3 0 0 0 6 0v-1h-18l2 -4h14l2 4" />
                              <path d="M5 21l0 -10.15" />
                              <path d="M19 21l0 -10.15" />
                              <path d="M9 21v-4a2 2 0 0 1 2 -2h2a2 2 0 0 1 2 2v4" />
                            </svg>
                        </div>
                        <div class="d-flex align-items-center gap-1">
                            <p class="search-item--title mb-0">
                                جستجو در محصولات با عنوان
                            </p>
                            <span class="search-item--category" style="text-decoration: underline;">${searchQuery}</span>
                        </div>
                    </a>
                    <a href="/Company?searchTitle=${searchQuery}">
                        <svg xmlns="http://www.w3.org/2000/svg" width="30" height="30" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor" fill="none" stroke-linecap="round" stroke-linejoin="round">
                            <path stroke="none" d="M0 0h24v24H0z" fill="none" />
                            <path d="M5 12l14 0" />
                            <path d="M5 12l4 4" />
                            <path d="M5 12l4 -4" />
                        </svg>
                    <a/>
        </div>
    `
}

// html ساخت
// بخش نتایج براساس عنوان محصول
function generateSearchResultItemInProductTitleTemplate(result) {
    return `
        <a href="/product?pspId=${result.productSalesPlanningId}" class="search-item d-flex gap-3">
                            <div class="d-flex align-items-center justify-content-center search-item-icon--container">
                                <svg width="22" height="22" stroke="currentColor">
                                    <use href="../images/icons/general.svg#search" />
                                </svg>
                            </div>
                            <div class="d-flex flex-column gap-1">
                                <h3 class="search-item--title mb-0">
                                    ${result.productSalesPlanningTitle}
                                </h3>
                                ${result.companyTitle !== null ? `
                                    <div class="search-item--categories d-flex flex-column flex-sm-row align-items-sm-center gap-2">
                                        <span style="font-size: inherit">در مرکز</span>
                                        <p class="search-item--category mb-0">${result.companyTitle}</p>
                                    </div>
                                ` : ''}
                                ${result.representationTitle !== null ? `
                                    <div class="search-item--categories d-flex flex-column flex-sm-row align-items-sm-center gap-2">
                                        <span style="font-size: inherit">در نمایندگی</span>
                                        <p class="search-item--category mb-0">${result.representationTitle}</p>
                                    </div>
                                `: ''}
                            </div>
                        </a>
    `
}

// html ساخت
// بخش نتایج براساس مراکز آموزشی
function generateSearchResultItemInCompanyTemplate(result) {
    return `
        <a href="/Company?company=${result.id}" class="search-item d-flex flex-column flex-sm-row gap-3">
                            <div class="d-flex align-items-center justify-content-center search-item-icon--container">
                                <svg width="22" height="22" stroke="currentColor">
                                    <use href="../images/icons/general.svg#search" />
                                </svg>
                            </div>
                            <div class="d-flex flex-column gap-1">
                                <h4 class="search-item--title d-flex align-items-center gap-1 mb-0">
                                   ${result.title}
                                </h4>
                                <h5 class="search-query mb-0">
                                    ${searchQuery}
                                </h5>
                            </div>
                        </a>
    `
}

// html ساخت
// بخش نتایج براساس دسته‌بندی
function generateSearchResultItemInCategoryTemplate(result) {
    return `
        <a href="/${result.isLevelOneCategory ? `main/${result.id}` : `Company?categoryId=${result.id}`}" class="search-item d-flex flex-column flex-sm-row gap-3">
                            <div class="d-flex align-items-center justify-content-center search-item-icon--container">
                                <svg width="22" height="22" stroke="currentColor">
                                    <use href="../images/icons/general.svg#search" />
                                </svg>
                            </div>
                            <div class="d-flex flex-column gap-1">
                                <h4 class="search-item--title d-flex align-items-center gap-1 mb-0">
                                 ${result.title}
                                </h4>
                                <h3 class="search-query mb-0">
                                   ${searchQuery}
                                </h3>
                            </div>
                        </a>
    `
}