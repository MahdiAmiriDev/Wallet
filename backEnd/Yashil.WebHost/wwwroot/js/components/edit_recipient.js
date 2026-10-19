import jalaali from 'jalaali-js';

const birthYearSelectElement = document.querySelector(".birth-year--select");
const birthMonthSelectElement = document.querySelector(".birth-month--select");
const birthDaySelectElement = document.querySelector(".birth-day--select");
const birthdayHiddenInputElement = document.getElementById('birthday-hidden--input');

$(document).ready(function () {
    createSelect2Dropdown('#cities-list', 'لطفا شهر را انتخاب کنید');
    $('#cities-list').val(cityId);
    $('#cities-list').trigger('change'); // Notify any JS components that the value changed

    createSelect2Dropdown('#relationshipWithBuyer-list', 'لطفا نسبت خریدار با تحویل گیرنده را انتخاب کنید');
    $('#relationshipWithBuyer-list').val(relationshipWithBuyerType);
    $('#relationshipWithBuyer-list').trigger('change');
})

const monthsName = [
    "فروردین",
    "اردیبهشت",
    "خرداد",
    "تیر",
    "مرداد",
    "شهریور",
    "مهر",
    "آبان",
    "آذر",
    "دی",
    "بهمن",
    "اسفند",
];
const defaultDayCount = 31;
const currentYear = new Date().getFullYear();
const maximumYear = currentYear - 4;
const minimumYear = currentYear - 80;

(() => {
    renderBirthYearOptions();
    renderBirthMonthOptions();
    renderBirthDayOptions(defaultDayCount);
    if (birthdayHiddenInputElement.value) renderBirthDate(birthdayHiddenInputElement.value)
})()

// تبدیل سال میلادی به شمسی
function convertToLocalYear(year) {
    const localYear = jalaali.toJalaali(new Date(year, 1, 1))
    return localYear;
};

// انتخاب سال تولدinput های متناسب باoption ساخت
function renderBirthYearOptions() {
    birthYearSelectElement.innerHTML = '<option value="placeholder" selected>سال</option>';
    for (let i = maximumYear; i >= minimumYear; i--) {
        const localYear = convertToLocalYear(i).jy;
        birthYearSelectElement.insertAdjacentHTML(
            "beforeend",
            generateBirthDayOptionTemplate(localYear, localYear)
        );
    }
}

// انتخاب ماه تولدinput های متناسب باoption ساخت
function renderBirthMonthOptions() {
    birthMonthSelectElement.innerHTML = '<option value="placeholder" selected>ماه</option>';
    for (let i = 0; i < monthsName.length; i++) {
        birthMonthSelectElement.insertAdjacentHTML(
            "beforeend",
            generateBirthDayOptionTemplate(i, monthsName[i])
        );
    }
}

// انتخاب روز تولدinput های متناسب باoption ساخت
function renderBirthDayOptions(count) {
    birthDaySelectElement.innerHTML = '<option value="placeholder" selected>روز</option>';
    for (let i = 1; i <= count; i++) {
        birthDaySelectElement.insertAdjacentHTML(
            "beforeend",
            generateBirthDayOptionTemplate(i, i)
        );
    }
}

// ساخت یک خروجی عددی از تاریخ کامل تولد جهت فرستادن به سمت سرور
function generateIntFromSelectedDate() {
    const birthDate = {
        year: +birthYearSelectElement.value,
        month: +birthMonthSelectElement.value + 1,
        day: +birthDaySelectElement.value,
    };
    if (Object.is(birthDate.year, NaN), Object.is(birthDate.month, NaN), Object.is(birthDate.day, NaN)) {
        birthdayHiddenInputElement.value = '';
        return
    }
    const date = `${birthDate.year}${String(birthDate.month).padStart(2, 0)}${String(birthDate.day).padStart(2, 0)}`;
    birthdayHiddenInputElement.value = date;
}

// انتخاب تاریخ تولد در صورت وجود select option نمایش تاریخ کامل تولد در
function renderBirthDate(birthDate) {
    const birthYear = +birthDate.slice(0, 4);
    const birthMonth = +birthDate.slice(4, 6) - 1;
    const birthDay = +birthDate.slice(6, 8);
    const defaultValue = +birthDate === 0 && 'placeholder';

    birthYearSelectElement.value = Object.is(birthYear, NaN) ? defaultValue : birthYear;
    birthMonthSelectElement.value = Object.is(birthMonth, NaN) ? defaultValue : birthMonth;

    const daysInMonth = jalaali.jalaaliMonthLength(birthYear, birthMonth);
    renderBirthDayOptions(daysInMonth);
    birthDaySelectElement.value = Object.is(birthDay, NaN) ? defaultValue : birthDay
}

// جمع آوری مقادیر وارد شده در
// های موجود برای انتخاب تاریخ تولدinput 
function gatherSelectedDateInputValues(e) {
    const targetElement = e.target;
    let selectedYear = +birthYearSelectElement.value;
    let selectedMonth = +birthMonthSelectElement.value + 1;
    let selectedDay = +birthDaySelectElement.value;

    const daysInMonth = jalaali.jalaaliMonthLength(selectedYear, selectedMonth);

    if (
        (!Object.is(selectedYear, NaN)
            && !Object.is(selectedMonth, NaN))
        && (targetElement.classList.contains('birth-month--select') || targetElement.classList.contains('birth-year--select'))
    ) {
        renderBirthDayOptions(daysInMonth);
        if (selectedDay > daysInMonth || Object.is(selectedDay, NaN)) {
            birthDaySelectElement.value = "placeholder"
        } else {
            birthDaySelectElement.value = selectedDay
        }
    }
    if (Object.is(+e.target.value, NaN)) {
        if (targetElement.classList.contains('birth-month--select') || targetElement.classList.contains('birth-year--select')) {
            renderBirthDayOptions(defaultDayCount)
        } else {
            renderBirthDayOptions(daysInMonth)
        }
        e.target.value = 'placeholder';
    }
    if (!Object.is(selectedYear, NaN) && !Object.is(selectedMonth, NaN) && !Object.is(selectedDay, NaN)) {
        generateIntFromSelectedDate()
    } else {
        birthdayHiddenInputElement.value = ''
    };
}

// هایoption مربوط به html ساخت
// های انتخاب تاریخ تولدselect مورد نیاز در
function generateBirthDayOptionTemplate(index, value) {
    return `
    <option value="${index}">${value}</option>
  `;
}

birthYearSelectElement.addEventListener("change", function (e) {
    gatherSelectedDateInputValues(e)
});

birthMonthSelectElement.addEventListener("change", function (e) {
    gatherSelectedDateInputValues(e)
});

birthDaySelectElement.addEventListener("change", function (e) {
    gatherSelectedDateInputValues(e)
});