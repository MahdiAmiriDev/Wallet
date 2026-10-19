const jobOfferSearchFormElement = document.getElementById('job-offer-search--form');
const jobOfferFiltersContainerElement = document.querySelector('.job-offer-filters--container');
const modalJobOfferFiltersContainerElement = document.querySelector('.modal-job-offer-filters--container');
const filtersSectionElement = document.getElementById('filters-section');
const filterTagsContainerElement = document.getElementById('filter-tags--container');
const allSortOptionElements = document.querySelectorAll('.offers-sort--option');
const deleteAllFilterTagsButtonElement = document.getElementById('delete-filters--button');
const jobOffersContainerElement = document.getElementById('job-offers--container');
const loadingSpinnerContainerElement = document.querySelector('.loading-spinner--container');

//const pagination = new Pagination(
//    document.querySelector('#job-offers--container'),
//    document.querySelector('.pagination-wrapper'),
//);

/***** Job Offer Search *****/
// Initialize Province List Dropdown
createSelect2Dropdown('#province-list--input', 'همه استان‌ها');

// Initialize Category List Dropdown
createSelect2Dropdown('#category-list--input', 'همه دسته بندی‌ها');

jobOfferSearchFormElement.addEventListener('submit', async function (event) {
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
    if (allInputsEmptyOrZero) {
        bootbox.alert('کاربر گرامی، حداقل یکی از فیلترها باید مقدار داشته باشد');
    } else {
        const params = new URLSearchParams(queryParams).toString();

        updateSearchParams(params);
        updateActiveFilterInputs();
        await updateJobOfferCards(params);

        $("#search-query--input").val('');
        $('#province-list--input').combobox('setValue', '0');
        $('#category-list--input').combobox('setValue', '0');
    }
})
/***** Job Offer Search *****/

// url مورد نظر به param اضافه کردن مقدار
function addSearchParam(params, param, value) {
    let currentParams = [];
    if (params.has(param)) {
        currentParams.push(params.get(param));
        currentParams.push(value);
        params.delete(param);
        params.append(param, currentParams.join(','))
    } else {
        params.append(param, value);
    }
}

// url مورد نظر از param حذف کردن مقدار
function deleteSearchParam(params, param, value) {
    if (params.has(param)) {
        let searchParams = params.get(param).split(',');
        if (searchParams.length === 1) {
            params.delete(param)
        } else {
            searchParams = searchParams.filter(tag => tag !== value);
            params.delete(param);
            params.append(param, searchParams.join(','))
        }
    }
}

// sort افزودن مقدار
// searchParams به
async function addSortValueToSearchParam(event) {
    const order = event.target.value;
    const urlObj = new URL(window.location.href);
    urlObj.searchParams.set('OrderBySalary', order);
    history.replaceState({}, '', urlObj.toString());

    const params = new URLSearchParams(window.location.search);
    await updateJobOfferCards(params);
}

// filtersSectionElement پاک کردن محتوای موجود در
function resetFiltersSectionElement() {
    filterTagsContainerElement.innerHTML = '';
}

// filtersSectionElement نمایش المان
function showFiltersSectionElement() {
    filtersSectionElement.classList.add('d-flex');
    filtersSectionElement.classList.remove('d-none');
}

// filtersSectionElement مخفی کردن المان
function hideFiltersSectionElement() {
    filtersSectionElement.classList.add('d-none');
    filtersSectionElement.classList.remove('d-flex');
}

// loadingSpinner نمایش المان
function showLoadingSpinner() {
    loadingSpinnerContainerElement.classList.remove('d-none')
}

// loadingSpinner مخفی کردن المان
function hideLoadingSpinner() {
    loadingSpinnerContainerElement.classList.add('d-none')
}

// نمایش مقدار موجود مرتب سازی براساس
// url در queryParams درصورت وجود به شکل
function renderOrderActiveValue() {
    const sortOffersBySelectElement = document.getElementById('sort-offers-by');
    const params = new URLSearchParams(window.location.search);
    const orderBySalary = params.get('OrderBySalary');
    if (orderBySalary) {
        sortOffersBySelectElement.value = orderBySalary
    }
}

// searchParams آپدیت کردن مقادیر
// بعد از آخرین تغییرات
function updateSearchParams(params) {
    if (params.size === 0) {
        // Create a new URL object based on the current location
        const baseUrl = new URL(window.location.href);

        // Reset the search and hash properties to empty strings
        baseUrl.search = '';
        baseUrl.hash = '';

        // Use replaceState to update the URL without reloading the page
        history.replaceState({}, '', baseUrl.toString());
    } else {
        // Update the URL without reloading the page
        history.pushState({}, '', '?' + params.toString());
    }
}

// مرتبط با option نمایش
// url موجود در sort مقدار
function updateActiveSortOption() {
    const activeFilters = getUrlParams();
    if (activeFilters?.sort) {
        for (let i = 0; i < allSortOptionElements.length; i++) {
            const filterOptionElement = allSortOptionElements[i];
            if (filterOptionElement.value === activeFilters.sort) {
                filterOptionElement.checked = true
            }
        }
    }
}

// های فعالinput بروزرسانی
// searchParams های موجود درparam براساس
function updateActiveFilterInputs() {
    const activeFilters = getUrlParams();
    const allFilterCheckboxElements = document.querySelectorAll('.filter-checkbox');
    const checkedInputs = [];

    if (activeFilters) {
        for (let category in activeFilters) {
            for (let i = 0; i < allFilterCheckboxElements.length; i++) {
                const inputElement = allFilterCheckboxElements[i];
                inputElement.checked = false;
                if (inputElement.name === category) {
                    const filters = activeFilters[category].split(',');
                    filters.forEach(filter => {
                        if (inputElement.value === filter) {
                            checkedInputs.push(inputElement)
                        }
                    });
                }
            }
        }
        checkedInputs.forEach(input => input.checked = true);
    } else {
        for (let i = 0; i < allFilterCheckboxElements.length; i++) {
            const inputElement = allFilterCheckboxElements[i];
            inputElement.checked = false;
        }
    }
}

// بروزرسانی آگهی‌های شغلی و تعداد آنها
// searchParams های موجود درparam براساس
async function updateJobOfferCards() {
    function generateUrl() {
        if (queryString) {
            return jobOffersCardsUrl + '?' + queryString
        } else {
            return jobOffersCardsUrl
        }
    }

    const urlObj = new URL(window.location.href);
    const queryParams = urlObj.searchParams;
    const queryString = queryParams.toString().replace(/%2C/g, ',');
    const url = generateUrl();
    showLoadingSpinner();
    try {
        if (jobOffersCardsUrl) {
            const response = await fetch(url, {
                method: 'GET'
            })
            if (!response.ok) {
                throw new Error('بروز خطا در هنگام دریافت اطلاعات');
            }
            const data = await response.text();
            jobOffersContainerElement.innerHTML = data;
            renderOfferCardBorders();
            renderOrderActiveValue();
        } else {
            console.warn('آدرس مورد نیاز جهت دریافت اطلاعات فراهم نشده است');
        }
    } catch (error) {
        console.error('بروز خطا:', error);
    } finally {
        hideLoadingSpinner();
    }

    lazyLoad()
}

// نمایش تمام فیلترهای یک دسته یا برخی از آنها
function toggleFiltersCountLimit(e) {
    const targetElement = e.target;
    const parentElement = targetElement.parentElement;
    const filtersContainerElement = parentElement.querySelector('.filters-container');
    const showMoreFiltersButtonElement = parentElement.querySelector('.show-more-filters--button');
    const showLessFiltersButtonElement = parentElement.querySelector('.show-less-filters--button');
    if (targetElement.closest('.show-more-filters--button')) {
        filtersContainerElement.classList.add('extended');
        showMoreFiltersButtonElement.classList.add('d-none');
        showLessFiltersButtonElement.classList.remove('d-none')
    }
    if (targetElement.closest('.show-less-filters--button')) {
        filtersContainerElement.classList.remove('extended');
        showMoreFiltersButtonElement.classList.remove('d-none');
        showLessFiltersButtonElement.classList.add('d-none')
    }
}

// مدیریت کردن نمایش فیلترهای بیشتر یا کمتر
// اضافه یا حذف کردن یک فیلتر در صورت انتخاب یا حذف انتخاب آن
async function handleFilterInputToggle(e) {
    const targetElement = e.target;
    // جهت باز و بسته کردن هر بخش فیلتر
    if (targetElement.classList.contains('filter-section--checkbox')) {
        const detailsElement = targetElement.parentElement.parentElement;
        const chevronIconElement = targetElement.parentElement.querySelector('.filter-section--icon');
        detailsElement.classList.toggle('open');
        chevronIconElement.classList.toggle('rotate')
    }

    // جهت اضافه کردن فیلترها به
    // searchParams به عنوان url
    // filterTagsContainerElement و به عنوان تگ به
    if (targetElement.classList.contains('filter-checkbox')) {
        const params = new URLSearchParams(window.location.search);
        const targetElement = e.target;
        const filterName = targetElement.name;

        // Add the newly selected category
        if (targetElement.checked) {
            addSearchParam(params, filterName, targetElement.value)
        } else {
            deleteSearchParam(params, filterName, targetElement.value)
        }
        updateSearchParams(params);
        updateActiveFilterInputs();
        await updateJobOfferCards(params);
    }
}

// هایparam دریافت تمامی
// searchParams موجود در
function getUrlParams() {
    const urlObj = new URL(window.location.href);
    const queryParams = urlObj.searchParams;
    let params = {};
    for (let [key, value] of queryParams.entries()) {
        params[key] = value;
    }

    if (Object.keys(params).length === 0) return undefined;
    return params
}

// به کارت آگهی شغلی border اضافه کردن
function renderOfferCardBorders() {
    const jobOfferCardElements = jobOffersContainerElement.querySelectorAll('.job-offer--card');
    const isCountEvent = jobOfferCardElements.length % 2 === 0;

    for (let i = 0; i < jobOfferCardElements.length; i++) {
        if (i % 2 === 0 && window.innerWidth > 768) {
            jobOfferCardElements[i].classList.add('border-end')
        }
        if (isCountEvent) {
            if (i < jobOfferCardElements.length - 2) {
                jobOfferCardElements[i].classList.add('border-bottom')
            }
        } else {
            if (i < jobOfferCardElements.length - 1) {
                jobOfferCardElements[i].classList.add('border-bottom')
            }
        }
    }
}

// filterTag مربوط به html ساخت
function generateFilterTagTemplate(category, filter) {
    return `
        <div class="filter-tag border" data-category="${category}" data-id="${filter.id}">
                            <button class="delete-filter--button border-end p-1">
                                <svg class="pe-none" xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24" stroke-width="1.8" stroke="currentColor" fill="none" stroke-linecap="round" stroke-linejoin="round">
                                    <path stroke="none" d="M0 0h24v24H0z" fill="none" />
                                    <path d="M18 6l-12 12" />
                                    <path d="M6 6l12 12" />
                                </svg>
                            </button>
                            <p class="filter-tag--title p-2 py-1 mb-0">${filter.title}</p>
                        </div>
    `
}

window.addEventListener('DOMContentLoaded', async function () {
    updateActiveFilterInputs();
    updateActiveSortOption();
    await updateJobOfferCards();
})


jobOfferFiltersContainerElement.addEventListener('change', async function (e) {
    await handleFilterInputToggle(e)
})

jobOfferFiltersContainerElement.addEventListener('click', function (e) {
    toggleFiltersCountLimit(e);
})

modalJobOfferFiltersContainerElement.addEventListener('change', async function (e) {
    await handleFilterInputToggle(e)
})

modalJobOfferFiltersContainerElement.addEventListener('click', function (e) {
    toggleFiltersCountLimit(e)
})