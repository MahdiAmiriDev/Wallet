// تغییر نحوه نمایش فعال برای آکهی‌های شغلی
function toggleActiveDisplayModeButton(event) {
    const allDisplayModeButtonElements = document.querySelectorAll('.display-mode--button');
    const displayModeButtonElement = event.target.closest('.display-mode--button');

    for (let i = 0; i < allDisplayModeButtonElements.length; i++) {
        allDisplayModeButtonElements[i].classList.remove('active')
    }

    if (displayModeButtonElement) {
        const displayMode = displayModeButtonElement.dataset.mode;
        displayModeButtonElement.classList.add('active');

        setOffersDisplayMode(displayMode)
    }
}

// تنظیم نحوه نمایش آگهی‌های شغلی
function setOffersDisplayMode(mode) {
    const jobOffersContainerElement = document.getElementById('job-offers');
    const jobOfferCardElements = jobOffersContainerElement.querySelectorAll('.job-offer--card');
    const isCountEvent = jobOfferCardElements.length % 2 === 0;

    for (let i = 0; i < jobOfferCardElements.length; i++) {
        jobOfferCardElements[i].classList.remove('border-end');
        jobOfferCardElements[i].classList.remove('border-bottom');

        if (i % 2 === 0 && mode === 'gallery') {
            jobOfferCardElements[i].classList.add('border-end')
        } 
        if (i < jobOfferCardElements.length - 1 && mode === 'list') {
            jobOfferCardElements[i].classList.add('border-bottom')
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

    if (mode === 'list') {
        jobOffersContainerElement.classList.add('list')
    } else {
        jobOffersContainerElement.classList.remove('list')
    }
}

// بزرگ کردن حرف اول کلمه مورد نظر
function capitalizeFirstLetter(string) {
    return string.charAt(0).toUpperCase() + string.slice(1);
}

// حذف فیلتر مورد نظر
async function clearFilter(e) {
    const params = new URLSearchParams(window.location.search);
    const deleteFilterButtonElement = e.target;
    const filterTagElement = deleteFilterButtonElement.parentElement;
    if (deleteFilterButtonElement) {
        const filterName = capitalizeFirstLetter(filterTagElement.dataset.category);
        const tagId = filterTagElement.dataset.id;
        deleteSearchParam(params, filterName, tagId);
    }
    updateSearchParams(params);
    updateActiveFilterInputs();
    await updateJobOfferCards()
}

// حذف تمامی فیلترها
async function clearAllFilters() {
    const params = new URLSearchParams(window.location.search);
    // Collect keys to delete
    const keysToDelete = [];
    for (let param of params.keys()) {
        if (param !== 'sort') {
            keysToDelete.push(param);
        }
    }

    // Delete collected keys
    keysToDelete.forEach(key => params.delete(key));

    updateSearchParams(params);
    updateActiveFilterInputs();
    await updateJobOfferCards()
}