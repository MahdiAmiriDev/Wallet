const bodyElement = document.body;
const serviceSelectionButtonElement = document.getElementById('service-selection--button');
const serviceSelectionModalElement = document.getElementById('service-selection__modal');
const closeModalButton = document.querySelector('.close-modal__button');
const tbaoButtonElements = document.querySelectorAll('.tbao-button');

serviceSelectionButtonElement.addEventListener('click', function () {
    openServiceSelectionModal()
})

// بستن مودال function فراخوانی
// به هنگام کلیک بر روی دکمه بستن
closeModalButton.addEventListener('click', closeServiceSelectionModal);

// بستن مودال به هنگام function فراخوانی
// کلیک بر روی دکمه ورود به طرح باقرالعلوم
tbaoButtonElements.forEach(button => button.addEventListener('click', closeServiceSelectionModal))

// باز کردن مودال و مخفی کردن
// body در overflow مقدار
function openServiceSelectionModal() {
    serviceSelectionModalElement.classList.add('show');
    bodyElement.classList.add('overflow-hidden')
}

// بستن مودال و ریست کردن
// body در overflow مقدار
function closeServiceSelectionModal() {
    serviceSelectionModalElement.classList.remove('show');
    bodyElement.classList.remove('overflow-hidden')
}