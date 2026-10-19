import Swal from 'sweetalert2';

const resumeContainerElement = document.getElementById('resume');
const loaderContainerElement = document.getElementById('loader-container');
const stepsContainerElement = document.getElementById('steps-wrapper');
const forms = document.querySelectorAll(".needs-validation");
const resumeSidebar = document.getElementById('resume-sidebar');
const resumeSidebarLoadingContainerElement = document.querySelector('.resume-rate-loading--container');
const genderRadioButtonsContainer = document.getElementById(
    "gender-select-container"
);
const careerRadioButtonsContainer = document.getElementById(
    "career-select-container"
);
const militaryServiceStatusContainer = document.getElementById(
    "military-service-container"
); const militaryServiceContainer = document.getElementById(
    "military-service-select"
);

const isStillWorkingElement = document.getElementById("still-working");
const finishMonthElement = document.getElementById("finish-month");
const finishYearElement = document.getElementById("finish-year");
const jobsListElement = document.getElementById("jobs-list");
const noJobsMessage = document.querySelector(".no-jobs-found");

const institutionNameInputElement = document.getElementById("institution");
const institutionTypeSelectElement = document.getElementById('institution-type');
const isStillEducatingElement = document.getElementById("still-educating");
const educationStartYearSelectElement = document.getElementById('education-start-year');
const graduationYearSelectElement = document.getElementById("graduation-year");
const educationListElement = document.getElementById("education-list");
const noEducationMessage = document.querySelector(".no-education-found");

const certificatesListElement = document.getElementById("certificates-list");
const noCertificatesMessage = document.querySelector(".no-certificates-found");

const languagesListElement = document.getElementById("languages-list");
const noLanguagesMessage = document.querySelector(".no-languages-found");

const emailAddressContainer = document.querySelector(".email-address");
const emailAddressInputElement = document.getElementById("email");

const mobileNumberContainer = document.querySelector(".mobile-number");
const mobileNumberInputElement = document.getElementById("mobile");

const provinceContainer = document.querySelector(".province");
const provinceInputElement = document.getElementById("province");

const cityContainer = document.querySelector(".city");

const nationalIdContainer = document.querySelector(".national-id");
const nationalIdInputElement = document.getElementById("national-id");

const birthYearContainer = document.querySelector(".birth-year");
const birthYearSelectElement = document.getElementById("birth-year");

const guardianNationalIdContainer = document.querySelector(".guardian-national-id");
const guardianNationalIdInputElement = document.getElementById("guardian-national-id");

const guardianMobileNumberContainer = document.querySelector(".guardian-mobile-number");
const guardianMobileNumberInputElement = document.getElementById("guardian-mobile");

const physicalStatusContainer = document.querySelector(".physical-status");
const physicalStatusInputElements = document.querySelectorAll(
    "input[name='physicalStatus']"
);
const physicalStatusRadioButtonsContainer = document.getElementById(
    "physical-select-container"
);

const disabilityType = document.querySelector(".disability-type");
const disabilityTypeContainer = document.getElementById("disability-type-container");
const disabilityTypeInputElement = document.getElementById("disability-type");

const marriageStatusContainer = document.querySelector(".marriage-status");
const marriageStatusInputElements = document.querySelectorAll(
    "input[name='marriageStatus']"
);
const marriageStatusRadioButtonsContainer = document.getElementById(
    "marriage-select-container"
);

const childrenCount = document.querySelector(".children-count");
const childrenCountContainer = document.getElementById("children-count-container");
const childrenCountSelectElement = document.getElementById("children-count-select");

const genderContainer = document.querySelector(".gender");
const genderInputElements = document.querySelectorAll("input[name='gender']");

const militaryContainer = document.querySelector(".military-service");
const militaryInputElement = document.getElementById("military-service");

const jobRecordStartYearSelectElement = document.getElementById('start-year');
const jobRecordFinishYearSelectElement = document.getElementById("finish-year");

const careerStatusInputElements = document.querySelectorAll(
    "input[name='careerStatus']"
);

const jobRecordsSection = document.getElementById('job-records');
const jobRecordsFormElement = document.getElementById('job-records-form');

const preferredLocationsContainer = document.querySelector(
    ".preferred-locations"
);
const fieldsContainer = document.querySelector(".fields");

const exactJobTitleContainer = document.querySelector('.exact-job-title');
const exactJobTitleInputElement = document.getElementById('exact-job-title');

const seniorityLevelsContainer = document.querySelector(".seniority-level");
const seniorityLevelInputElements = document.querySelectorAll(
    "input[name='seniorityLevels']"
);
const seniorityLevelsDescriptionInputElement = document.getElementById('seniority-levels-description');

const contractsContainer = document.querySelector(".contract");
const contractInputElements = document.querySelectorAll(
    "input[name='contracts']"
);
const contractsDescriptionInputElement = document.getElementById('contracts-description');

const majorSelectElement = document.querySelector("select#major");
const educationCertificateButton = document.getElementById('education-certificate');
const educationCertificateTitleElement = document.getElementById('education-certificate--title');
const noEducationCertificateElement = document.getElementById('no-education--certificate');
const allowedEducationDocTypes = document.getElementById('allowed-education--doctypes');

const certificateButton = document.getElementById('attachments');
const certificateTitleElement = document.getElementById('certificate-title');
const noCertificateElement = document.getElementById('no-certificate');
const allowedCertificateDocTypes = document.getElementById('allowed-certificate--doctypes');

const languageCertificateButton = document.getElementById('language-certificate');
const languageCertificateTitleElement = document.getElementById('language-certificate--title');
const noLanguageCertificateElement = document.getElementById('no-language--certificate');
const allowedLanguageDocTypes = document.getElementById('allowed-language--doctypes');

// Dropdown Elements
const provinceSelectElement = document.querySelector("select#province");
const citySelectElement = document.querySelector("select#city");
const militaryServiceSelectElement = document.querySelector(
    "select#military-service"
);
const startMonthSelectElement = document.querySelector("select#start-month");
const languageSelectElement = document.querySelector("select#language");
const masterySelectElement = document.querySelector("select#mastery");
const preferredProvinceSelectElement = document.querySelector(
    "select#preferred-province"
);
const preferredCitySelectElement = document.querySelector(
    "select#preferred-city"
);
const addLocationButton = document.getElementById("add-location-button");
const locationsListFormElement = document.getElementById("locations-list-form");
const locationsListElement = document.getElementById("locations-list");
const locationsElement = document.getElementById("preferred-locations");
const jobFieldsSelectElement = document.querySelector("select#job-fields");

const fieldsListElement = document.getElementById("fields-list");

// Score Component Elements
const personalInformationSectionProfileItems = document
    .getElementById("personal-information")
    .querySelectorAll(".profile-item");
const jobPreferencesSectionProfileItems = document
    .getElementById("job-preferences")
    .querySelectorAll(".profile-item");
const improvementSuggestionsListElement = document.getElementById(
    "improvement-suggestions-list"
);
const completionPercentage = document.querySelector(".completion-percentage");
const completionPercentageLine = document.querySelector(
    ".completion-percentage-line"
);
const completionVisualStatus = document.querySelector(
    ".completion-visual-status"
);
const completionPercentageMessage = document.querySelector(
    ".completion-percentage-message"
);

const resumeSteps = document.querySelectorAll(".resume-step");
const stepsContainer = document.getElementById('steps-container');
const continueButtonElement = document.getElementById('continue-button');

const showResumeButtonElement = document.getElementById('show-resume');

let isPersonalInformationSectionIncomplete;
let isJobsSectionIncomplete;
let isEducationSectionIncomplete;
let isCertificatesSectionIncomplete;
let isLanguagesSectionIncomplete;
let isJobPreferencesSectionIncomplete;

let currentStep = 0; // Current step is set to be the first step (0)
const getUrl = "/api/additionalUserProp/GetUserResume";
const postUrl = "/api/additionalUserProp/AddUserResume";
const getFormatsUrl = "/api/docFormat/GetDocFormatExtensionsByDocTypeId?docTypeId=";
const deleteDocumentUrl = "/api/AppDocument/DeleteCertificate?"
const resumeCompletionPercentageUrl = "/api/additionalUserProp/GetResumeCompletionPercentage";
const certificateDocTypeId = 1076;
const educationCertificateDocTypeId = 1077;
const languageCertificateDocTypeId = 1078;
let userResume;
let userCertificates = [];
let userLanguageCertificates = [];
let userEducationCertificates = [];
let studyFields = [];
let provinces = [];
let locations = [];
let fields = [];
let suggestions = [];
let educationCertificateFormats;
let certificateFormats;
let languageCertificateFormats;
let record = {};

const months = {
    1: "فروردین",
    2: "اردیبهشت",
    3: "خرداد",
    4: "تیر",
    5: "مرداد",
    6: "شهریور",
    7: "مهر",
    8: "آبان",
    9: "آذر",
    10: "دی",
    11: "بهمن",
    12: "اسفند"
};

const militaryService = {
    1: "پایان خدمت",
    2: "معافیت دائم",
    3: "معافیت تحصیلی",
    4: "درحال انجام",
    5: "مشمول"
};

const languages = {
    1: "انگلیسی",
    2: "عربی",
    3: "آلمانی",
    4: "اسپانیایی",
    5: "ایتالیایی",
    6: "فرانسوی",
    7: "روسی",
    8: "چینی",
    9: "ترکی استانبولی"

};

const jobFields = [
    {
        id: 1,
        title: "وب، برنامه‌نویسی و نرم‌افزار"
    },
    {
        id: 2,
        title: "IT / DevOps / Server"
    },
    {
        id: 3,
        title: "آموزش"
    },
    {
        id: 4,
        title: "انبارداری"
    },
    {
        id: 5,
        title: "پزشکی، پرستاری و دارویی"
    },
    {
        id: 6,
        title: "پشتیبانی و امور مشتریان"
    },
    {
        id: 7,
        title: "تحقیق بازار و تحلیل اقتصادی"
    },
    {
        id: 8,
        title: "تربیت بدنی"
    },
    {
        id: 9,
        title: "ترجمه"
    },
    {
        id: 10,
        title: "تکنسین فنی، تعمیرکار"
    },
    {
        id: 11,
        title: "تولید و مدیریت محتوا"
    },
    {
        id: 12,
        title: "حمل و نقل"
    },
    {
        id: 13,
        title: "حوزه سینما و تصویر"
    },
    {
        id: 14,
        title: "حوزه موسیقی و صدا"
    },
    {
        id: 15,
        title: "خبرنگاری"
    },
    {
        id: 16,
        title: "خرید و بازرگانی"
    },
    {
        id: 17,
        title: "دیجیتال مارکتینگ"
    },
    {
        id: 18,
        title: "راننده، پیک موتوری"
    },
    {
        id: 19,
        title: "روابط عمومی"
    },
    {
        id: 20,
        title: "شیمی، داروسازی"
    },
    {
        id: 21,
        title: "صنایع غذایی"
    },
    {
        id: 22,
        title: "طراحی"
    },
    {
        id: 23,
        title: "علوم زیستی و آزمایشگاهی"
    },
    {
        id: 24,
        title: "فروش و بازاریابی"
    },
    {
        id: 25,
        title: "کارشناس حقوقی، وکالت"
    },
    {
        id: 26,
        title: "کارگر ساده، نیروی خدماتی"
    },
    {
        id: 27,
        title: "کارگر ماهر، کارگر صنعتی"
    },
    {
        id: 28,
        title: "گردشگری"
    },
    {
        id: 29,
        title: "مالی و حسابداری"
    },
    {
        id: 30,
        title: "مدیر محصول"
    },
    {
        id: 31,
        title: "مدیریت بیمه"
    },
    {
        id: 32,
        title: "مسئول دفتر، اجرائی و اداری"
    },
    {
        id: 33,
        title: "منابع انسانی و کارگزینی"
    },
    {
        id: 34,
        title: "مهندسی پزشکی"
    },
    {
        id: 35,
        title: "مهندسی پلیمر"
    },
    {
        id: 36,
        title: "مهندسی شیمی و نفت"
    },
    {
        id: 37,
        title: "مهندسی صنایع و مدیریت صنعتی"
    },
    {
        id: 38,
        title: "مهندسی عمران و معماری"
    },
    {
        id: 39,
        title: "مهندسی کشاورزی"
    },
    {
        id: 40,
        title: "مهندسی معدن و متالوژی"
    },
    {
        id: 41,
        title: "مهندسی مکانیک و هوافضا"
    },
    {
        id: 42,
        title: "مهندسی نساجی، طراحی پارچه و لباس"
    },
    {
        id: 43,
        title: "نگهبان"
    },
    {
        id: 44,
        title: "هتلداری"
    },
    {
        id: 45,
        title: "CEO"
    },
    {
        id: 46,
        title: "HSE"
    },
    {
        id: 47,
        title: "تحقیق و توسعه"
    },
    {
        id: 48,
        title: "مهندسی برق و الکترونیک"
    }
];

const masteryLevels = {
    1: "مقدماتی",
    2: "متوسط",
    3: "پیشرفته",
    4: "زبان مادری"
};

//const wages = {
//    0: "توافقی",
//    1: "پایه (وزارت کار)",
//    2: "4.5",
//    3: "5",
//    4: "5.5",
//    5: "6",
//    6: "6.5",
//    7: "7",
//    8: "7.5",
//    9: "8",
//    10: "8.5",
//    11: "9",
//    12: "9.5",
//    13: "10",
//    14: "10.5",
//    15: "11",
//    16: "11.5",
//    17: "12",
//    18: "12.5",
//    19: "13",
//    20: "13.5",
//    21: "14",
//    22: "14.5",
//    23: "15",
//    24: "15.5",
//    25: "16",
//    26: "16.5",
//    27: "17",
//    28: "17.5",
//    29: "18",
//    30: "18.5",
//    31: "19",
//    32: "19.5",
//    33: "20"
//};



document.documentElement.classList.add('overflow-hidden');
(() => {
    // Loop over them and prevent submission
    Array.from(forms).forEach((form) => {
        form.addEventListener(
            "submit",
            (event) => {
                event.preventDefault();
                form.classList.add("was-validated");
                if (!form.checkValidity()) {
                    event.preventDefault();
                    event.stopPropagation();
                }
                if (form.checkValidity()) {
                    saveFormData(form, form.dataset.tag, true);
                }
            },
            false
        );
    });
})();

window.addEventListener('load', async function () {
    await getAllAcceptableFormats();
    await getProvinces();
    await getStudyFields();
    await getUserResume();
    await getUserCertificates(userCertificates, certificateDocTypeId);
    await getUserCertificates(userEducationCertificates, educationCertificateDocTypeId);
    await getUserCertificates(userLanguageCertificates, languageCertificateDocTypeId);
    await generateDropdowns();


    allowedEducationDocTypes.textContent = educationCertificateFormats.replaceAll(".", "");
    educationCertificateButton.setAttribute('accept', educationCertificateFormats);
    allowedCertificateDocTypes.textContent = certificateFormats.replaceAll(".", "");
    certificateButton.setAttribute('accept', certificateFormats);
    allowedLanguageDocTypes.textContent = languageCertificateFormats.replaceAll(".", "");
    languageCertificateButton.setAttribute('accept', languageCertificateFormats)

    /** ساخت رزومه کابر **/
    await generateResume();
});


/******************** های موجود در فرم‌ها با استفاده از داده‌های برگشته از سمت سرورdropdown ساخت ********************/
async function generateDropdowns() {
    /** استان‌ها dropdown به ازای هر استان برای option element ساخت **/
    for (let index in provinces) {
        provinceSelectElement.insertAdjacentHTML(
            "beforeend",
            generateProvinceSelectOptionTemplate(provinces[index])
        );
        preferredProvinceSelectElement.insertAdjacentHTML(
            "beforeend",
            generateProvinceSelectOptionTemplate(provinces[index])
        );
    }

    /** شهرها dropdown به ازای هر شهر استان انتخاب شده برای option element ساخت **/
    provinceSelectElement.addEventListener("change", async function (e) {
        let selectedProvinceId = +e.target.value;
        const cities = await getCities(selectedProvinceId);
        citySelectElement.innerHTML =
            "<option selected disabled>-- انتخاب کنید --</option>";
        cities?.map((city) =>
            citySelectElement.insertAdjacentHTML(
                "beforeend",
                generateCitySelectOptionTemplate(city)
            )
        );
    });


    /** رشته تحصیلی dropdown به ازای هر رشته تحصیلی برای option element ساخت **/
    for (let index in studyFields) {
        const optionValue = studyFields[index].id;
        const optionTitle = studyFields[index].title;
        majorSelectElement.insertAdjacentHTML(
            "beforeend",
            generateSelectOptionTemplate(optionValue, optionTitle)
        );
    }
    createSelect2Dropdown('#major', '--انتخاب کنید--');

    /** دسته بندی شغلی dropdown به ازای هر دسته شغلی برای option element ساخت **/
    for (let index in jobFields) {
        const optionValue = jobFields[index].id;
        const optionTitle = jobFields[index].title;
        jobFieldsSelectElement.insertAdjacentHTML(
            "beforeend",
            generateSelectOptionTemplate(optionValue, optionTitle)
        );
    }
    $(document).ready(function () {
        $('#job-fields').select2({
            placeholder: '--انتخاب کنید--',
            dir: 'rtl',
            language: 'fa',
            selectionCssClass: 'select2-multiselect--tbao',
            dropdownCssClass: 'select2-panel--tbao'
        });
    });

    /** شهرها در ترجیحات شغلی dropdown به ازای هر شهر استان انتخاب شده برای option element ساخت **/
    preferredProvinceSelectElement.addEventListener("change", async function (e) {
        let selectedProvinceId = +e.target.value;
        const cities = await getCities(selectedProvinceId);
        preferredCitySelectElement.innerHTML =
            "<option selected disabled>-- انتخاب کنید --</option>";
        cities?.map((city) => {
            preferredCitySelectElement.insertAdjacentHTML(
                "beforeend",
                generateCitySelectOptionTemplate(city)
            );
        });
    });

    /** سال تولد dropdown به ازای سال های 1350 به بعد برای option element ساخت **/
    const currentYear = convertToEnglishDigits(new Intl.DateTimeFormat('fa-IR', { year: 'numeric' }).format(new Date()));
    for (let index = 1350; index <= currentYear - 15 && index >= 1350; index++) {
        birthYearSelectElement.insertAdjacentHTML(
            "beforeend",
            generateYearSelectOptionTemplate(index, index, currentYear, true)
        );
    }

    for (let index = 1350; index <= currentYear && index >= 1350; index++) {
        educationStartYearSelectElement.insertAdjacentHTML(
            "beforeend",
            generateYearSelectOptionTemplate(index, index, currentYear, false)
        );
    }

    for (let index = 1350; index <= currentYear && index >= 1350; index++) {
        graduationYearSelectElement.insertAdjacentHTML(
            "beforeend",
            generateYearSelectOptionTemplate(index, index, currentYear, false)
        );
    }

    for (let index = 1350; index <= currentYear && index >= 1350; index++) {
        jobRecordStartYearSelectElement.insertAdjacentHTML(
            "beforeend",
            generateYearSelectOptionTemplate(index, index, currentYear, false)
        );
    }

    for (let index = 1350; index <= currentYear && index >= 1350; index++) {
        jobRecordFinishYearSelectElement.insertAdjacentHTML(
            "beforeend",
            generateYearSelectOptionTemplate(index, index, currentYear, false)
        );
    }

    /** وضعیت خدمت سربازی dropdown به ازای وضعیت های مختلف خدمت سربازی آقایان برای option element ساخت **/
    for (let index in militaryService) {
        militaryServiceSelectElement.insertAdjacentHTML(
            "beforeend",
            generateSelectOptionTemplate(index, militaryService[index])
        );
    }

    /** ماه شروع و پایان همکاری در کامپوننت سوابق شغلی dropdown به ازای هر ماه برای option element ساخت **/
    for (let index in months) {
        startMonthSelectElement.insertAdjacentHTML(
            "beforeend",
            generateSelectOptionTemplate(index, months[index])
        );
        finishMonthElement.insertAdjacentHTML(
            "beforeend",
            generateSelectOptionTemplate(index, months[index])
        );
    }

    /** انتخاب زبان‌های مورد نظر dropdown به ازای هر زبان برای option element ساخت **/
    for (let index in languages) {
        languageSelectElement.insertAdjacentHTML(
            "beforeend",
            generateSelectOptionTemplate(index, languages[index])
        );
    }

    /** میزان تسلط بر زبان مورد نظر dropdown به ازای سطوح تسلط زبان برای option element ساخت **/
    for (let index in masteryLevels) {
        masterySelectElement.insertAdjacentHTML(
            "beforeend",
            generateSelectOptionTemplate(index, masteryLevels[index])
        );
    }
    /******************************************************** پایان ********************************************************/
}

async function generateResume() {
    /******************** Show User Information When Exists ********************/
    /** نمایش کدملی کابر **/
    if (userResume.information.nationalId) {
        showInformation(nationalIdContainer, userResume.information.nationalId);
    }

    /** نمایش ایمیل کابر **/
    if (userResume.information.email) {
        showInformation(emailAddressContainer, userResume.information.email);
    }

    /** نمایش استان محل سکونت کابر **/
    if (userResume.information.province) {
        const province = getProvince(userResume.information.province);
        showInformation(provinceContainer, province?.title);
    }

    /** نمایش شهر محل سکونت کابر **/
    if (userResume.information.city) {
        const province = userResume.information.province;
        const city = await getCity(province, userResume.information.city);
        showInformation(cityContainer, city?.title);
    }

    /** نمایش شماره تلفن کابر **/
    if (userResume.information.mobile) {
        showInformation(mobileNumberContainer, userResume.information.mobile);
    }

    /** نمایش سال تولد کابر **/
    if (userResume.information.birthYear) {
        showInformation(birthYearContainer, userResume.information.birthYear);
    }

    /** نمایش کدملی سرپرست کاربر **/
    if (userResume.information.guardianNationalId) {
        showInformation(guardianNationalIdContainer, userResume.information.guardianNationalId);
    }

    /** نمایش شماره تلفن سرپرست کاربر **/
    if (userResume.information.guardianMobile) {
        showInformation(guardianMobileNumberContainer, userResume.information.guardianMobile);
    }

    /** نمایش وضعیت جسمانی کابر **/
    if (userResume.information.physicalStatus) {
        let physicalStatus;
        switch (userResume.information.physicalStatus) {
            case "normal":
                physicalStatus = "سالم";
                break;
            case "disabled":
                physicalStatus = "دارای معلولیت";
                break;
        }
        showInformation(physicalStatusContainer, physicalStatus);
        const selectedPhysicalStatus = Array.from(physicalStatusInputElements).find(
            (physicalStatusInputElement) =>
                physicalStatusInputElement.value === userResume.information.physicalStatus
        );
        if (selectedPhysicalStatus.value === "disabled") {
            disabilityTypeContainer.classList.remove('d-none');
            disabilityTypeInputElement.required = true;
            disabilityTypeInputElement.disabled = false;
        } else {
            disabilityTypeContainer.classList.add('d-none');
            disabilityTypeInputElement.required = false;
            disabilityTypeInputElement.disabled = true;
        }
    }

    /** نمایش نوع معلولیت کاربر **/
    if (userResume.information.disabilityType) {
        showInformation(disabilityType, userResume.information.disabilityType);
    }

    /** نمایش وضعیت تاهل کابر **/
    if (userResume.information.marriageStatus) {
        let marriageStatus;
        switch (userResume.information.marriageStatus) {
            case "bachelor":
                marriageStatus = "مجرد";
                break;
            case "married":
                marriageStatus = "متاهل";
                break;
        }
        showInformation(marriageStatusContainer, marriageStatus);
        const selectedMarriageStatus = Array.from(marriageStatusInputElements).find(
            (marriageStatusInputElement) =>
                marriageStatusInputElement.value === userResume.information.marriageStatus
        );

        if (selectedMarriageStatus.value === "married") {
            childrenCountContainer.classList.remove('d-none');
            childrenCountSelectElement.querySelector("select").required = true;
            childrenCountSelectElement.querySelector("select").disabled = false;
        } else {
            childrenCountContainer.classList.add('d-none');
            childrenCountSelectElement.querySelector("select").required = false;
            childrenCountSelectElement.querySelector("select").disabled = true;
        }
    }

    /** نمایش تعداد فرزندان کاربر **/
    if (userResume.information.childrenCount) {
        showInformation(childrenCount, userResume.information.childrenCount);
    }

    /** نمایش جنسیت کابر **/
    if (userResume.information.gender) {
        let gender;
        switch (userResume.information.gender) {
            case "male":
                gender = "مرد";
                break;
            case "female":
                gender = "زن";
                break;
        }
        showInformation(genderContainer, gender);
        const selectedGender = Array.from(genderInputElements).find(
            (genderInputElement) =>
                genderInputElement.value === userResume.information.gender
        );

        if (selectedGender.value === "male") {
            militaryServiceStatusContainer.classList.remove('d-none');
            militaryServiceContainer.classList.remove("d-none");
            militaryServiceContainer
                .querySelector("select")
                .setAttribute("required", true);
        } else {
            militaryServiceStatusContainer.classList.add('d-none');
            militaryServiceContainer.classList.add("d-none");
            militaryServiceContainer
                .querySelector("select")
                .removeAttribute("required");
        }
    }

    /** نمایش وضعیت خدمت سربازی درصورتی که کاربر مرد باشد **/
    if (userResume.information.militaryService) {
        const militaryService = getMilitaryService(
            userResume.information.militaryService
        );
        showInformation(militaryContainer, militaryService);
    }

    /** نمایش سوابق شغلی کابر **/
    if (userResume.careerProfile.jobs?.length === 0) {
        jobsListElement.classList.add("d-none");
        noJobsMessage.classList.remove("d-none");
    } else {
        jobsListElement.classList.remove("d-none");
        noJobsMessage.classList.add("d-none");
        jobsListElement.innerHTML = "";
        userResume.careerProfile.jobs?.map(async (job) =>
            jobsListElement.insertAdjacentHTML(
                "beforeend",
                await generateJobRecordTemplate(job)
            )
        );
    }

    /** نمایش سوابق تحصیلی کابر **/
    if (userResume.education?.length === 0) {
        educationListElement.classList.add("d-none");
        noEducationMessage.classList.remove("d-none");
    } else {
        educationListElement.classList.remove("d-none");
        noEducationMessage.classList.add("d-none");
        educationListElement.innerHTML = "";
        userResume.education.map((education) =>
            educationListElement.insertAdjacentHTML(
                "beforeend",
                generateEducationRecordTemplate(education)
            )
        );
    }

    /** نمایش مدارک و گواهینامه‌های کابر **/
    if (!userResume.certificates || userResume.certificates?.length === 0) {
        certificatesListElement.classList.add("d-none");
        noCertificatesMessage.classList.remove("d-none");
    } else {
        certificatesListElement.classList.remove("d-none");
        noCertificatesMessage.classList.add("d-none");
        certificatesListElement.innerHTML = "";
        userResume.certificates?.map((certificate) =>
            certificatesListElement.insertAdjacentHTML(
                "beforeend",
                generateCertificateRecordTemplate(certificate)
            )
        );
    }

    /** نمایش زبان‌های کابر **/
    if (userResume.languages?.length === 0) {
        languagesListElement.classList.add("d-none");
        noLanguagesMessage.classList.remove("d-none");
    } else {
        languagesListElement.classList.remove("d-none");
        noLanguagesMessage.classList.add("d-none");
        languagesListElement.innerHTML = "";
        userResume.languages.map((language) =>
            languagesListElement.insertAdjacentHTML(
                "beforeend",
                generateLanguageCardTemplate(language)
            )
        );
    }

    /** نمایش وضعیت شغلی کابر **/
    if (userResume.careerProfile.careerStatus) {
        const selectedCareerStatus = Array.from(careerStatusInputElements).find(
            (careerStatusInputElement) => careerStatusInputElement.value === userResume.careerProfile.careerStatus
        );
        selectedCareerStatus.checked = true;
        showOrHideJobRecordsForm(userResume.careerProfile.careerStatus);
    }

    /** نمایش محل‌های ترجیحی کابر جهت کار **/
    if (
        userResume.careerProfile.jobPreferences?.preferredLocations &&
        userResume.careerProfile.jobPreferences?.preferredLocations?.length !== 0
    ) {
        locationsListElement.classList.remove("d-none");
        preferredLocationsContainer
            .querySelector(".profile-add-item")
            .classList.add("d-none");
        locations = userResume.careerProfile.jobPreferences?.preferredLocations;
        await addLocationToList(
            locationsListElement,
            locations
        );
        await addLocationToList(
            locationsListFormElement,
            locations,
            "form"
        );
    } else {
        locationsListElement.classList.add("d-none");
        preferredLocationsContainer
            .querySelector(".profile-add-item")
            .classList.remove("d-none");
    }

    /** نمایش زمینه شغلی ترجیحی کابر **/
    if (userResume.careerProfile.jobPreferences?.jobFields && userResume.careerProfile.jobPreferences.jobFields?.length !== 0) {
        fieldsListElement.classList.remove("d-none");
        fieldsContainer
            .querySelector(".profile-add-item")
            .classList.add("d-none");
        fields = userResume.careerProfile.jobPreferences.jobFields;
        await addFieldToList(
            fieldsListElement,
            fields
        );
    } else {
        fieldsListElement.classList.add("d-none");
        fieldsContainer
            .querySelector(".profile-add-item")
            .classList.remove("d-none");
    }

    /** نمایش نوع معلولیت کاربر **/
    if (userResume.information.disabilityType) {
        showInformation(disabilityType, userResume.information.disabilityType);
    }

    /** نمایش عنوان دقیق شغل درخواستی کاربر **/
    if (userResume.careerProfile.jobPreferences?.exactJobTitle) {
        showInformation(exactJobTitleContainer, userResume.careerProfile.jobPreferences?.exactJobTitle);
    }

    /** نمایش سطوح ارشدیت ترجیحی کابر **/
    if (
        userResume.careerProfile.jobPreferences?.seniorityLevels &&
        userResume.careerProfile.jobPreferences?.seniorityLevels?.length !== 0
    ) {
        seniorityLevelsContainer
            .querySelector(".profile-item")
            .classList.remove("d-none");
        seniorityLevelsContainer
            .querySelector(".profile-add-item")
            .classList.add("d-none");
        seniorityLevelsContainer.querySelector(".profile-item").innerHTML = "";
        userResume.careerProfile.jobPreferences?.seniorityLevels.map((seniorityLevel) => {
            seniorityLevelsContainer
                .querySelector(".profile-item")
                .insertAdjacentHTML(
                    "beforeend",
                    generateSeniorityLevelCardTemplate(seniorityLevel)
                );
        });
    } else {
        seniorityLevelsContainer
            .querySelector(".profile-item")
            .classList.add("d-none");
        seniorityLevelsContainer
            .querySelector(".profile-add-item")
            .classList.remove("d-none");
    }

    /** نمایش قراردادهای قابل قبول ترجیحی کابر **/
    if (
        userResume.careerProfile.jobPreferences?.contracts &&
        userResume.careerProfile.jobPreferences?.contracts?.length !== 0
    ) {
        contractsContainer
            .querySelector(".profile-item")
            .classList.remove("d-none");
        contractsContainer
            .querySelector(".profile-add-item")
            .classList.add("d-none");
        contractsContainer.querySelector(".profile-item").innerHTML = "";
        userResume.careerProfile.jobPreferences?.contracts.map((contract) => {
            contractsContainer
                .querySelector(".profile-item")
                .insertAdjacentHTML(
                    "beforeend",
                    generateContractCardTemplate(contract)
                );
        });
    } else {
        contractsContainer.querySelector(".profile-item").classList.add("d-none");
        contractsContainer
            .querySelector(".profile-add-item")
            .classList.remove("d-none");
    }


    /** نمایش امتیاز رزومه کابر **/
    await rateResume(userResume);
    loaderContainerElement.classList.add('d-none');
    resumeContainerElement.classList.remove('d-none');
    document.documentElement.classList.remove('overflow-hidden');
}
/******************** End ********************/

/******************** Rate Resume based on Existing Information ********************/
async function rateResume(resume) {
    suggestions = [];
    resumeSidebarLoadingContainerElement.classList.remove('d-none');
    improvementSuggestionsListElement.innerHTML = "";
    improvementSuggestionsListElement.classList.remove("d-none");

    try {
        const response = await fetch(resumeCompletionPercentageUrl);
        if (!response.ok) throw new Error('خطا در برقراری ارتباط با سرور')
        const resumeCompletionPercentage = await response.json();

        completionPercentage.querySelector("p").textContent = resumeCompletionPercentage;
        completionVisualStatus.style.height = `${resumeCompletionPercentage}%`;
        resumeCompletionPercentage === 100 &&
            improvementSuggestionsListElement.classList.add("d-none");
        if (resumeCompletionPercentage === 100) {
            resumeCompletionPercentage === 100 && completionPercentageMessage.classList.add("d-none");
        } else {
            completionPercentageMessage.classList.remove("d-none");
        }

        if (+resumeCompletionPercentage < 50) {
            completionPercentage.style.color = "#e34435";
            completionVisualStatus.style.background = "#e34435";
            completionPercentageLine.style.background = "#e34435";
        } else if (+resumeCompletionPercentage >= 50 && +resumeCompletionPercentage < 75) {
            completionPercentage.style.color = "#f9a60b";
            completionVisualStatus.style.background = "#f9a60b";
            completionPercentageLine.style.background = "#f9a60b";
        } else {
            completionPercentage.style.color = "#1abc9c";
            completionVisualStatus.style.background = "#1abc9c";
            completionPercentageLine.style.background = "#1abc9c";
        }
    } catch (error) {
        const Toast = Swal.mixin({
            toast: true,
            position: "top-end",
            showConfirmButton: false,
            timer: 3000,
            timerProgressBar: true,
            didOpen: (toast) => {
                toast.onmouseenter = Swal.stopTimer;
                toast.onmouseleave = Swal.resumeTimer;
            }
        });
        Toast.fire({
            icon: "error",
            title: error.message
        });
    } finally {
        resumeSidebarLoadingContainerElement.classList.add('d-none');
    }

    isPersonalInformationSectionIncomplete = Array.from(personalInformationSectionProfileItems).every(information => {
        if (information.classList.contains('show-information')) {
            return false
        } else {
            return true
        }
    });
    isEducationSectionIncomplete =
        resume?.education?.length === 0;
    isCertificatesSectionIncomplete =
        resume?.certificates?.length === 0;
    isLanguagesSectionIncomplete =
        resume?.languages?.length === 0;
    isJobsSectionIncomplete = resume?.careerProfile.jobs?.length === 0;
    isJobPreferencesSectionIncomplete = Array.from(
        jobPreferencesSectionProfileItems
    ).some((profileItem) => profileItem.innerHTML === "") && resume?.careerProfile.jobPreferences.preferredLocations?.length === 0;

    if (isPersonalInformationSectionIncomplete) {
        suggestions.push({ score: 25, suggestion: "تکمیل اطلاعات فردی" });
    }
    if (isEducationSectionIncomplete) {
        suggestions.push({
            score: 15,
            suggestion: "سوابق تحصیلی خود را وارد کنید.",
        });
    }
    if (isCertificatesSectionIncomplete) {
        suggestions.push({
            score: 15,
            suggestion: "مدارک و گواهینامه دوره های خود را وارد کنید."
        });
    }
    if (isLanguagesSectionIncomplete) {
        suggestions.push({
            score: 10,
            suggestion: "زبان‌هایی که با آنها آشنا هستید را بنویسید.",
        });
    }
    if (isJobsSectionIncomplete) {
        suggestions.push({ score: 10, suggestion: "سوابق شغلی خود را وارد کنید." });
    }
    if (isJobPreferencesSectionIncomplete) {
        suggestions.push({
            score: 25,
            suggestion: "علاقه مندی‌شغلی خود را مشخص کنید.",
        });
    }

    suggestions
        .map((suggestion) =>
            improvementSuggestionsListElement.insertAdjacentHTML(
                "beforeend",
                generateImprovementSuggestionItemTemplate(suggestion)
            )
        );
}
/******************** End ********************/

/******************** Stepped Forms ********************/
// Generate all step tabs
Array.from(resumeSteps).forEach((section, index) => {
    const sectionTitle = section.querySelector('h6')
    stepsContainer.insertAdjacentHTML('beforeend', generateStepTabTemplate(index, sectionTitle))
})

showSection(currentStep); // Display the current tab

// This function will display the specified tab of the form ...
function showSection(index) {
    resumeSteps[index].classList.remove('d-none');
    fixStepIndicator(index);
}

// This function will figure out which tab to display
function nextPreviousStep(number) {
    // Hide the current tab:
    resumeSteps[currentStep].classList.add('d-none');
    // Set the current tab to given number:
    currentStep = +number;
    // Display the correct tab:
    showSection(currentStep);
    if (currentStep >= resumeSteps.length - 1) {
        continueButtonElement.classList.add('d-none');
        if (window.innerWidth < 992) {
            resumeSidebar.style.marginTop = "0";
        }
    } else {
        continueButtonElement.classList.remove('d-none');
        if (window.innerWidth < 992) {
            resumeSidebar.style.marginTop = "64px";
        }
    }
}

// This function removes the "step-visited" class of all steps...
function fixStepIndicator(index) {
    const steps = document.getElementsByClassName("step-circle");
    const stepTitles = document.getElementsByClassName("step-title");
    // Remove the "step-visited", "step-current" class from all steps and "step-title-current" class from all step titles:
    for (let i = 0; i < steps.length; i++) {
        steps[i].classList.remove("step-visited");
        steps[i].classList.remove("step-current");
        stepTitles[i].classList.remove("step-title-current");
        steps[i].innerHTML = i + 1;
    }
    //... and adds the "step-visited" class to the current step:
    if (index > 0) {
        const visitedSteps = Array.from(steps).splice(0, index);
        visitedSteps.forEach(visitedStep => {
            visitedStep.classList.add("step-visited");
            visitedStep.innerHTML = `<svg xmlns="http://www.w3.org/2000/svg" class="icon icon-tabler icon-tabler-check" width="30" height="30" viewBox="0 0 24 24" stroke-width="1.5" stroke="#fff" fill="none" stroke-linecap="round" stroke-linejoin="round">
                                            <path stroke="none" d="M0 0h24v24H0z" fill="none" />
                                            <path d="M5 12l5 5l10 -10" />
                                        </svg>`
        })
    }
    steps[index].classList.add("step-visited");
    steps[index].classList.add("step-current");
    stepTitles[index].classList.add("step-title-current");
}
/******************** End ********************/

/******************** Requests ********************/
/** دریافت لیست استان‌های ایران **/
async function getProvinces() {
    try {
        const response = await fetch("/api/city/GetProvinces");
        if (!response.ok) throw new Error("در هنگام بارگیری اطلاعات مشکلی رخ داد!");
        const data = await response.json();
        provinces = data
    } catch (error) {
        const Toast = createToast("top-end", 4000)
        Toast.fire({
            icon: "error",
            title: `<h6 style="font-weight: 800">بروز خطا</h6>`,
            html: `<p style="margin-top: -10px">${error.message}</p>`,
        });
    }
}

/** دریافتی id دریافت لیست شهرهای هر استان باتوجه به **/
async function getCities(id) {
    try {
        const response = await fetch(`/api/city/GetCitiesByProvinceId?provinceId=${id}`);
        if (!response.ok) throw new Error("شهرهای استان مورد نظر پیدا نشد!");
        const cities = await response.json();
        return cities;
    } catch (error) {
        const Toast = createToast("top-end", 4000)
        Toast.fire({
            icon: "error",
            title: `<h6 style="font-weight: 800">بروز خطا</h6>`,
            html: `<p style="margin-top: -10px">${error.message}</p>`,
        });
    }
}

/** دریافت لیست رشته‌های تحصیلی **/
async function getStudyFields() {
    try {
        const response = await fetch("/api/PublicApi/GetFindEducationalField");
        if (!response.ok) throw new Error("در هنگام بارگیری اطلاعات مشکلی رخ داد!");
        const data = await response.json();
        studyFields = data
    } catch (error) {
        const Toast = createToast("top-end", 4000)
        Toast.fire({
            icon: "error",
            title: `<h6 style="font-weight: 800">بروز خطا</h6>`,
            html: `<p style="margin-top: -10px">${error.message}</p>`,
        });
    }
}

/*
title
courseLevel
courseLocation
attachments
*/
/** دریافت اطلاعات کاربر **/
async function getUserResume() {
    const resumeTemplate = {
        information: {},
        education: [],
        certificates: [],
        languages: [],
        careerProfile: {
            jobs: [],
            jobPreferences: {
                preferredLocations: [],
                jobFields: [],
                seniorityLevels: [],
                contracts: [],
            },
        }
    };

    const jobRecordAllowedProperties = ["id", "jobTitle", "company", "role", "startMonth", "startYear", "finishMonth", "finishYear", "jobDescription", "stillWorking"];
    const certificateRecordAllowedProperties = ["id", "docTypeId", "title", "courseLevel", "courseLocation", "attachment"];
    const languageRecordAllowedProperties = ["id", "docTypeId", "language", "mastery", "attachment"];
    function checkAllowance(allowedProperties, property) {
        const isAllowed = allowedProperties.includes(property);
        return isAllowed
    }
    try {
        const response = await fetch(getUrl);
        if (!response.ok) throw new Error("در هنگام بارگیری اطلاعات مشکلی رخ داد!");
        const user = await response.json();
        if (user) {
            if (user.userResume === "" || user.userResume === "[]") {
                userResume = resumeTemplate;
            } else {
                userResume = JSON.parse(user.userResume);
                // To be removed...
                if (!userResume.careerProfile) {
                    userResume = {
                        ...userResume, careerProfile: {
                            careerStatus: userResume.careerStatus,
                            jobs: userResume.jobs,
                            jobPreferences: userResume.jobPreferences
                        }
                    };
                }
                delete userResume.jobs;
                delete userResume.jobPreferences;
                delete userResume.careerProfile.jobPreferences.preferredProvince;
                delete userResume.careerProfile.jobPreferences.preferredCity;
                delete userResume.careerStatus;
                // درصورتیکه که رزومه قدیمی باشد، مشکل وجود داده اضافه در رکورد سابقه شغلی توسط این کد حل میشود
                if (userResume.careerProfile.jobs) {
                    userResume.careerProfile.jobs.forEach(job => {
                        for (let property in job) {
                            if (!checkAllowance(jobRecordAllowedProperties, property)) {
                                delete job[property];
                            }
                        }
                    })
                }
                // درصورتیکه که رزومه قدیمی باشد، مشکل وجود داده اضافه در رکورد مدارک و گواهینامه‌ها توسط این کد حل میشود
                if (userResume.certificates) {
                    userResume.certificates.forEach(certificate => {
                        for (let property in certificate) {
                            if (!checkAllowance(certificateRecordAllowedProperties, property)) {
                                delete certificate[property];
                            }
                        }
                    })
                }
                // درصورتیکه که رزومه قدیمی باشد، مشکل وجود داده اضافه در رکورد زبان توسط این کد حل میشود
                if (userResume.languages) {
                    userResume.languages.forEach(language => {
                        for (let property in language) {
                            if (!checkAllowance(languageRecordAllowedProperties, property)) {
                                delete language[property];
                            }
                        }
                    })
                }
            }
        }
    } catch (error) {
        const Toast = createToast("top-end", 4000)
        Toast.fire({
            icon: "error",
            title: `<h6 style="font-weight: 800">بروز خطا</h6>`,
            html: `<p style="margin-top: -10px">${error.message}</p>`,
        });
    }
}

/** دریافت مدارک و گواهینامه‌های کاربر **/
async function getUserCertificates(list, docTypeId) {
    try {
        const response = await fetch(`home/GetUserCertificates?docTypeId=${docTypeId}`);
        if (!response.ok) throw new Error("در هنگام بارگیری اطلاعات مشکلی رخ داد!");
        const data = await response.json();
        if (data) {
            if (data?.length == 0) {
                list = []
            } else {
                list = data
            }
        }
    } catch (error) {
        const Toast = createToast("top-end", 4000)
        Toast.fire({
            icon: "error",
            title: `<h6 style="font-weight: 800">بروز خطا</h6>`,
            html: `<p style="margin-top: -10px">${error.message}</p>`,
        });
    }
}

/** دریافتی id دریافت اطلاعات مربوط به استان مورد نظر باتوجه به **/
function getProvince(id) {
    try {
        const province = provinces.find((province) => +province.id === +id);
        if (!province) throw new Error("استان مورد نظر یافت نشد!");
        return province;
    } catch (error) {
        const Toast = createToast("top-end", 4000)
        Toast.fire({
            icon: "error",
            title: `<h6 style="font-weight: 800">بروز خطا</h6>`,
            html: `<p style="margin-top: -10px">${error.message}</p>`,
        })
    }
}

/** دریافتی id دریافت اطلاعات مربوط به شهر مورد نظر باتوجه به **/
async function getCity(provinceId, id) {
    try {
        const response = await fetch(`/api/city/GetCitiesByProvinceId?provinceId=${provinceId}`);
        if (!response.ok) throw new Error("در هنگام بارگیری اطلاعات مشکلی رخ داد!");
        const cities = await response.json();
        const city = cities.find((city) => +city.id === +id);
        if (!city) throw new Error("شهر مورد نظر یافت نشد!");
        return city;
    } catch (error) {
        const Toast = createToast("top-end", 4000)
        Toast.fire({
            icon: "error",
            title: `<h6 style="font-weight: 800">بروز خطا</h6>`,
            html: `<p style="margin-top: -10px">${error.message}</p>`,
        });
    }
}

/** دریافتی id دریافت اطلاعات مربوط به وضعیت خدمت سربازی مورد نظر باتوجه به **/
function getMilitaryService(id) {
    return militaryService[id];
}

/** دریافتی level و id دریافت اطلاعات مربوط به زبان مورد نظر به همراه سطح تسلط کاربر بر آن باتوجه به **/
function getLanguageAndMastery(id, level) {
    return [languages[id], masteryLevels[level]];
}

/** دریافتی id دریافت اطلاعات مربوط به مبلغ حقوق مورد نظر باتوجه به **/
function getWage(id) {
    return wages[id];
}



/** ارسال اطلاعات کاربر به سرور **/
async function postResume(resume, operation = "post", shouldNavigate = true) {
    const param = JSON.stringify(resume);
    const formData = new FormData();
    formData.userResume = param;
    formData.id = 0;
    try {
        const response = await fetch(postUrl, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(formData),
        });
        if (!response.ok) throw new Error("در هنگام دخیره اطلاعات مشکلی رخ داد!");
        const data = await response.json();
        if (data.isSuccess) {
            if (shouldNavigate) {
                const Toast = createToast("top-end", 4000, true, false, operation);
                Toast.fire({
                    icon: "success",
                    title: operation === "post" ? `<h6 style="font-weight: 800">${data.message}</h6>` : "",
                    html: operation === "post" ? currentStep < Array.from(resumeSteps).length - 1 ? `<p style="margin-top: -10px">تا چند لحظه دیگر به مرحله بعدی منتقل خواهید شد.</p>` : "" : "اطلاعات مورد نظر با موفقیت حذف شد.",
                });
            } else {
                const Toast = createToast("top-end", 4000);
                Toast.fire({
                    icon: "success",
                    html: operation === "post" ? `${data.message}` : "اطلاعات مورد نظر با موفقیت حذف شد.",
                });
            };
        } else {
            const Toast = createToast("top-end", 4000)
            if (data.message.length > 1) {
                Toast.fire({
                    icon: "error",
                    title: `<h6 style="font-weight: 800">بروز خطا</h6>`,
                    html: `<ul style="margin-top: -10px">${data.message.map(error => {
                        return `
                            <li>${error}</li>
                        `
                    }).join('')}</ul>`,
                });
            } else {
                Toast.fire({
                    icon: "error",
                    title: `<h6 style="font-weight: 800">بروز خطا</h6>`,
                    html: `<p style="margin-top: -10px">${data.message.at(0)}</p>`,
                });
            }
        }
    } catch (error) {
        const Toast = createToast("top-end", 4000)
        Toast.fire({
            icon: "error",
            title: `<h6 style="font-weight: 800">بروز خطا</h6>`,
            html: `<p style="margin-top: -10px">${error.message}</p>`,
        });
    } finally {
        /** دریافت رزومه کاربر از سرور **/
        await getUserResume();
        /** ساخت رزومه کابر **/
        await generateResume();
    }
}

/** ارسال مدارک کاربر به سرور **/
async function postCertificate(formElement, tag, shouldNavigate) {
    const formData = new FormData(formElement);
    const Toast = createToast("top-end", 4000);
    record = {}

    if (tag === "education") {
        if (graduationYearSelectElement.value && +educationStartYearSelectElement.value > +graduationYearSelectElement.value) {
            Toast.fire({
                icon: "error",
                title: `<h6 style="font-weight: 800">بروز خطا</h6>`,
                html: `<p style="margin-top: -10px">کاربر گرامی، سال پایان تحصیل نمیتواند قبل از سال شروع تحصیل باشد.</p>`,
            });
            return
        }
    }

    for (let [key, value] of formData.entries()) {
        if (key === "attachments" && value.size === 0) {
            addCertificateToResume();
            if (shouldNavigate) {
                formElement.classList.remove("show-information");
                formElement.classList.add("hide-information");
            }
            formElement.classList.remove("was-validated");
            if (tag === "education") {
                $('#major').val('').trigger('change');
                graduationYearSelectElement.setAttribute("disabled", true);
                graduationYearSelectElement.removeAttribute("required");
            }
            formElement.reset();
            /** ارسال رزومه با مقادیر جدید به سرور **/
            await postResume(userResume, "post", shouldNavigate);

            /** دریافت مدارک کاربر از سرور **/
            await getUserCertificates(userCertificates, certificateDocTypeId);
            await getUserCertificates(userEducationCertificates, educationCertificateDocTypeId);
            await getUserCertificates(userLanguageCertificates, languageCertificateDocTypeId);
            return;
        }
    }

    function addCertificateToResume(data) {
        if (!userResume[tag]) {
            userResume[tag] = []
        } else if (tag !== "education") {
            record.id = userResume[tag].length;
        }
        for (let [key, value] of formData.entries()) {
            if (key === "attachments" && value.size === 0) {
                delete record.attachment;
                continue;
            } else if (key === "attachments" && value.size > 0) {
                record.attachment = data;
            } else if (key === "major") {
                const studyFieldSelectValue = $('#major').select2('data');
                isNaN(value) ? record.id = userResume[tag].length : record.id = +value;
                record[key] = studyFieldSelectValue[0].text
            } else {
                record[key] = value
            };
        }
        userResume[tag] = [...userResume[tag], record];
    }


    try {
        const response = await fetch('/home/UploadAttachments', {
            method: 'POST',
            body: formData
        });
        const data = await response.json();
        const successfulUpload = data.at(0).isSuccess;
        const failedUpload = !data.at(0).isSuccess;

        (function () {
            if (successfulUpload) {
                // All files uploaded successfully
                addCertificateToResume(data, true);
                if (shouldNavigate) {
                    formElement.classList.remove("show-information");
                    formElement.classList.add("hide-information");
                }
                formElement.classList.remove("was-validated");
                if (tag === "education") {
                    $('#major').val('').trigger('change');
                    graduationYearSelectElement.setAttribute("disabled", true);
                    graduationYearSelectElement.removeAttribute("required");
                }
                formElement.reset();

            }
            if (failedUpload) {
                // All files failed to upload
                throw new Error(data.at(0).message);
            }
        })();


        /** ارسال رزومه با مقادیر جدید به سرور **/
        await postResume(userResume, "post", shouldNavigate);

        /** دریافت مدارک کاربر از سرور **/
        await getUserCertificates(userCertificates, certificateDocTypeId);
        await getUserCertificates(userEducationCertificates, educationCertificateDocTypeId);
        await getUserCertificates(userLanguageCertificates, languageCertificateDocTypeId);

    } catch (error) {
        Toast.fire({
            icon: "error",
            title: `<h6 style="font-weight: 800">بروز خطا</h6>`,
            html: `<p style="margin-top: -10px">${error.message}</p>`,
        });
    }
}
/******************** End ********************/

/******************** Functions ********************/
/** نمایش اطلاعات دریافتی از سرور در بخش مورد نظر **/
function showInformation(container, information) {
    container.querySelector(".profile-item").classList.remove("d-none");
    container.querySelector(".profile-item").classList.remove("hide-information");
    container.querySelector(".profile-item").classList.add("show-information");
    container.querySelector(".profile-add-item").classList.add("d-none");
    container.querySelector(".profile-item").innerText = information;
}

/** بررسی صحت کدملی مورد نظر **/
function checkNationalId(nationalId) {
    if (nationalId.length == 10) {
        if (nationalId == '1111111111' ||
            nationalId == '0000000000' ||
            nationalId == '2222222222' ||
            nationalId == '3333333333' ||
            nationalId == '4444444444' ||
            nationalId == '5555555555' ||
            nationalId == '6666666666' ||
            nationalId == '7777777777' ||
            nationalId == '8888888888' ||
            nationalId == '9999999999') {
            return false;
        }
        const lastDigit = parseInt(nationalId.charAt(9));
        const digitsSum = parseInt(nationalId.charAt(0)) * 10 +
            parseInt(nationalId.charAt(1)) * 9 +
            parseInt(nationalId.charAt(2)) * 8 +
            parseInt(nationalId.charAt(3)) * 7 +
            parseInt(nationalId.charAt(4)) * 6 +
            parseInt(nationalId.charAt(5)) * 5 +
            parseInt(nationalId.charAt(6)) * 4 +
            parseInt(nationalId.charAt(7)) * 3 +
            parseInt(nationalId.charAt(8)) * 2;
        const result = digitsSum - parseInt(digitsSum / 11) * 11;
        if ((result == 0 && result == lastDigit) || (result == 1 && lastDigit == 1) || (result > 1 && lastDigit == 11 - result)) {
            return true;
        } else {
            return false;
        }
    } else {
        return false;
    }
}

/** اضافه کردن شهر و استان مورد نظر در بخش ترجیحات شغلی به لیست مکان‌های ترجیحی **/
async function addLocationToList(container, list, type = "list") {
    container.innerHTML = "";
    if (list.length === 0) {
        locationsListElement.classList.add("d-none");
        locationsListFormElement.parentElement.classList.add("d-none");
        preferredLocationsContainer
            .querySelector(".profile-add-item")
            .classList.remove("d-none");
    } else {
        locationsListElement.classList.remove("d-none");
        locationsListFormElement.parentElement.classList.remove("d-none");
        preferredLocationsContainer
            .querySelector(".profile-add-item")
            .classList.add("d-none");
    }
    list.map(async (location) => {
        const province = getProvince(location.province);
        const city = await getCity(location.province, location.city);
        container.insertAdjacentHTML(
            "beforeend",
            generatePreferredLocationCardTemplate(location.id, province, city, type)
        );
    });
}

/** اضافه کردن دسته بندی شغلی در بخش ترجیحات شغلی به لیست دسته بندی شغلی و زمینه کاری **/
async function addFieldToList(container, list) {
    container.innerHTML = "";
    fieldsListElement.classList.remove("d-none");
    fieldsContainer
        .querySelector(".profile-add-item")
        .classList.add("d-none");
    list.map((field) => {
        container.insertAdjacentHTML(
            "beforeend",
            generateFieldCardTemplate(field)
        );
    });
}

/** میکند hover نمایش دکمه حذف هنگامی که کاربر روی آیتمی از لیست مورد نظر **/
function showDeleteButton(e, className) {
    if (e.target.classList.contains(className)) {
        e.target.querySelector(".delete-button").classList.remove("d-none");
        e.target.addEventListener("mouseleave", function () {
            e.target.querySelector(".delete-button").classList.add("d-none");
        });
    }
}

/** داده شده id نمایش اطلاعات موجود در فرم مورد نظر بر اساس **/
function showExistingDataInForm(id) {
    switch (id) {
        case "personal-information":
            showExistingDataInPersonalInformationForm();
            return;
        case "job-preferences":
            showExistingDataInJobPreferencesForm();
            return;
    }
}

/** در صورت وجود personal information نمایش اطلاعات مربوط به فرم **/
async function showExistingDataInPersonalInformationForm() {
    /** نمایش کدملی کابر **/
    if (userResume.information.nationalId) {
        nationalIdInputElement.value = userResume.information.nationalId;
    }

    /** نمایش ایمیل کابر **/
    if (userResume.information.email) {
        emailAddressInputElement.value = userResume.information.email;
    }

    /** نمایش استان محل سکونت کابر **/
    if (userResume.information.province) {
        provinceInputElement.value = userResume.information.province;
        const cities = await getCities(userResume.information.province);
        cities.map((city) => {
            citySelectElement.insertAdjacentHTML(
                "beforeend",
                generateCitySelectOptionTemplate(city)
            );
        });
    }

    /** نمایش شهر محل سکونت کابر **/
    if (userResume.information.city) {
        citySelectElement.value = userResume.information.city;
    }

    /** نمایش شماره تلفن کابر **/
    if (userResume.information.mobile) {
        mobileNumberInputElement.value = userResume.information.mobile;
    }

    /** نمایش سال تولد کابر **/
    if (userResume.information.birthYear) {
        birthYearSelectElement.value = userResume.information.birthYear;
    }

    /** نمایش کدملی سرپرست کاربر **/
    if (userResume.information.guardianNationalId) {
        guardianNationalIdInputElement.value = userResume.information.guardianNationalId
    }

    /** نمایش شماره تلفن سرپرست کاربر **/
    if (userResume.information.guardianMobile) {
        guardianMobileNumberInputElement.value = userResume.information.guardianMobile
    }

    /** نمایش وضعیت جسمانی کابر **/
    if (userResume.information.physicalStatus) {
        const selectedPhysicalStatus = Array.from(physicalStatusInputElements).find(
            (physicalStatusInputElement) =>
                physicalStatusInputElement.value === userResume.information.physicalStatus
        );
        selectedPhysicalStatus.checked = true;
    }

    /** نمایش نوع معلولیت کاربر **/
    if (userResume.information.disabilityType) {
        disabilityTypeInputElement.value = userResume.information.disabilityType;
        disabilityTypeInputElement.required = true;
        disabilityTypeInputElement.disabled = false;
    }

    /** نمایش وضعیت تاهل کابر **/
    if (userResume.information.marriageStatus) {
        const selectedMarriageStatus = Array.from(marriageStatusInputElements).find(
            (marriageStatusInputElement) =>
                marriageStatusInputElement.value === userResume.information.marriageStatus
        );
        selectedMarriageStatus.checked = true;
    }

    /** نمایش تعداد فرزندان کاربر **/
    if (userResume.information.childrenCount) {
        childrenCountSelectElement.querySelector("select").value = userResume.information.childrenCount;
        childrenCountSelectElement.querySelector("select").required = true;
        childrenCountSelectElement.querySelector("select").disabled = false;
    }

    /** نمایش جنسیت کابر **/
    if (userResume.information.gender) {
        const selectedGender = Array.from(genderInputElements).find(
            (genderInputElement) =>
                genderInputElement.value === userResume.information.gender
        );
        selectedGender.checked = true;
    }

    /** نمایش وضعیت خدمت سربازی درصورتی که کاربر مرد باشد **/
    if (userResume.information.militaryService) {
        militaryInputElement.value = userResume.information.militaryService;
        militaryServiceContainer.querySelector("select").required = true;
        militaryServiceContainer.querySelector("select").disabled = false;
    }
}

/** در صورت وجود job preferences نمایش اطلاعات مربوط به فرم **/
async function showExistingDataInJobPreferencesForm() {
    /** نمایش محل‌های ترجیحی کابر جهت کار **/
    if (
        userResume.careerProfile.jobPreferences?.preferredLocations &&
        userResume.careerProfile.jobPreferences?.preferredLocations?.length !== 0
    ) {
        locationsListFormElement.parentElement.classList.remove("d-none");
        await addLocationToList(
            locationsListFormElement,
            locations,
            "form"
        );
    } else {
        locationsListFormElement.parentElement.classList.add("d-none");
    }

    /** نمایش زمینه شغلی ترجیحی کابر **/
    if (userResume.careerProfile.jobPreferences.jobFields) {
        $('document').ready(function () {
            const existingJobFieldValues = userResume.careerProfile.jobPreferences.jobFields.map(jobField => {
                return jobField.id
            });
            $('#job-fields').val(existingJobFieldValues);
            $('#job-fields').trigger('change'); // Notify any JS components that the value changed
        })
    }

    /** نمایش عنوان دقیق شغل درخواستی کاربر **/
    if (userResume.careerProfile.jobPreferences?.exactJobTitle) {
        exactJobTitleInputElement.value = userResume.careerProfile.jobPreferences?.exactJobTitle;
    }

    /** نمایش سطوح ارشدیت ترجیحی کابر **/
    if (
        userResume.careerProfile.jobPreferences?.seniorityLevels &&
        userResume.careerProfile.jobPreferences?.seniorityLevels?.length !== 0
    ) {
        userResume.careerProfile.jobPreferences?.seniorityLevels.map(async (seniorityLevel) => {
            const selectedSeniorityLevel = Array.from(
                seniorityLevelInputElements
            ).find(
                (seniorityLevelInputElement) =>
                    seniorityLevelInputElement.value === seniorityLevel
            );
            selectedSeniorityLevel.checked = true;
        });
        if (userResume.careerProfile.jobPreferences?.seniorityLevels.includes("otherSeniorityLevel")) {
            seniorityLevelsDescriptionInputElement.classList.remove('d-none');
            seniorityLevelsDescriptionInputElement.required = true;
            seniorityLevelsDescriptionInputElement.value = userResume.careerProfile.jobPreferences?.seniorityLevelsDescription;
        }
    }

    /** نمایش قراردادهای قابل قبول ترجیحی کابر **/
    if (
        userResume.careerProfile.jobPreferences?.contracts &&
        userResume.careerProfile.jobPreferences?.contracts?.length !== 0
    ) {
        userResume.careerProfile.jobPreferences?.contracts.map(async (contract) => {
            const selectedContract = Array.from(contractInputElements).find(
                (contractInputElement) => contractInputElement.value === contract
            );
            selectedContract.checked = true;
        });
        if (userResume.careerProfile.jobPreferences?.contracts.includes("otherContract")) {
            contractsDescriptionInputElement.classList.remove('d-none');
            contractsDescriptionInputElement.required = true;
            contractsDescriptionInputElement.value = userResume.careerProfile.jobPreferences?.contractsDescription;
        }
    }
}

async function deleteJobItem(id) {
    Swal.fire({
        title: `<h5 style="font-weight: 800">آیا از حذف سابقه شغلی مورد نظر اطمینان دارید؟</h5>`,
        showCancelButton: true,
        confirmButtonText: "بله",
        cancelButtonText: "انصراف"
    }).then(async (result) => {
        if (result.isConfirmed) {
            userResume.careerProfile.jobs = userResume.careerProfile.jobs.filter((job) => {
                return +job.id !== +id
            });
            await postResume(userResume, "delete", false);
        }
    });
}


async function deleteEducationItem(id, entityId) {
    Swal.fire({
        title: `<h5 style="font-weight: 800">آیا از حذف سابقه تحصیلی مورد نظر اطمینان دارید؟</h5>`,
        showCancelButton: true,
        confirmButtonText: "بله",
        cancelButtonText: "انصراف"
    }).then(async (result) => {
        if (result.isConfirmed) {
            if (entityId !== 'undefined') {
                try {
                    const response = await fetch(deleteDocumentUrl + `certificateId=${entityId}&docTypeId=${educationCertificateDocTypeId}`);
                    const data = await response.json();
                    if (data.isSuccess) {
                        userResume.education = userResume.education.filter((education) => {
                            return +education.id !== +id
                        });
                        await postResume(userResume, "delete", false);
                    } else {
                        throw new Error(data.message)
                    }
                } catch (error) {
                    const Toast = createToast("top-end", 4000)
                    Toast.fire({
                        icon: "error",
                        title: `<h6 style="font-weight: 800">بروز خطا</h6>`,
                        html: `<p style="margin-top: -10px">${error.message}</p>`,
                    });
                }
            } else {
                userResume.education = userResume.education.filter((education) => {
                    return +education.id !== +id
                });
                await postResume(userResume, "delete", false);
            }
        }
    });
}

async function deleteCourseItem(id, entityId) {
    Swal.fire({
        title: `<h5 style="font-weight: 800">آیا از حذف دوره مورد نظر اطمینان دارید؟</h5>`,
        showCancelButton: true,
        confirmButtonText: "بله",
        cancelButtonText: "انصراف"
    }).then(async (result) => {
        if (result.isConfirmed) {
            if (entityId !== 'undefined') {
                try {
                    const response = await fetch(deleteDocumentUrl + `certificateId=${entityId}&docTypeId=${certificateDocTypeId}`);
                    const data = await response.json();
                    if (data.isSuccess) {
                        userResume.certificates = userResume.certificates.filter((certificate) => {
                            return +certificate.id !== +id
                        });
                        await postResume(userResume, 'delete', false);
                    } else {
                        throw new Error(data.message)
                    }
                } catch (error) {
                    const Toast = createToast("top-end", 4000)
                    Toast.fire({
                        icon: "error",
                        title: `<h6 style="font-weight: 800">بروز خطا</h6>`,
                        html: `<p style="margin-top: -10px">${error.message}</p>`,
                    });
                }
            } else {
                userResume.certificates = userResume.certificates.filter((certificate) => {
                    return +certificate.id !== +id
                });
                await postResume(userResume, 'delete', false);
            }
        }
    });
}

async function deleteLanguageItem(id, entityId) {
    Swal.fire({
        title: `<h5 style="font-weight: 800">آیا از حذف زبان مورد نظر اطمینان دارید؟</h5>`,
        showCancelButton: true,
        confirmButtonText: "بله",
        cancelButtonText: "انصراف"
    }).then(async (result) => {
        if (result.isConfirmed) {
            if (entityId !== 'undefined') {
                try {
                    const response = await fetch(deleteDocumentUrl + `certificateId=${entityId}&docTypeId=${educationCertificateDocTypeId}`);
                    const data = await response.json();
                    if (data.isSuccess) {
                        userResume.languages = userResume.languages.filter((language) => {
                            return +language.id !== +id
                        });
                        await postResume(userResume, 'delete', false);
                    } else {
                        throw new Error(data.message)
                    }
                } catch (error) {
                    const Toast = createToast("top-end", 4000)
                    Toast.fire({
                        icon: "error",
                        title: `<h6 style="font-weight: 800">بروز خطا</h6>`,
                        html: `<p style="margin-top: -10px">${error.message}</p>`,
                    });
                }
            } else {
                userResume.languages = userResume.languages.filter((language) => {
                    return +language.id !== +id
                });
                await postResume(userResume, 'delete', false);
            }
        }
    });
}

async function deleteLocationItem(id) {
    Swal.fire({
        title: `<h5 style="font-weight: 800">آیا از حذف مکان مورد نظر اطمینان دارید؟</h5>`,
        showCancelButton: true,
        confirmButtonText: "بله",
        cancelButtonText: "انصراف"
    }).then(async (result) => {
        if (result.isConfirmed) {
            if (userResume.careerProfile.jobPreferences.preferredLocations.length > 0) {
                userResume.careerProfile.jobPreferences.preferredLocations = userResume.careerProfile.jobPreferences?.preferredLocations.filter((preferredLocation) => {
                    return +preferredLocation.id !== +id
                });
                locations = userResume.careerProfile.jobPreferences?.preferredLocations;
            } else {
                locations = locations.filter((preferredLocation) => {
                    return +preferredLocation.id !== +id
                });
            }
            addLocationToList(locationsListFormElement, locations, "form");
            await postResume(userResume, "delete", false);
        }
    });
}


/** ترجمه مقدار مدرک تحصیلی به فارسی **/
function translateGrade(grade) {
    switch (grade) {
        case "diploma":
            return "دیپلم";
        case "foghDiploma":
            return "فوق دیپلم";
        case "bachelors":
            return "کارشناسی";
        case "master":
            return "کارشناسی ارشد";
        case "phd":
            return "دکترا و بالاتر";
        case "other":
            return "سایر";
    }
}

/** ترجمه نوع دانشگاه به فارسی **/
function translateInstitutionType(type) {
    switch (type) {
        case "1":
            return "دانشگاه سراسری (دولتی)";
        case "2":
            return "دانشگاه غیر انتفاعی";
        case "3":
            return "دانشگاه آزاد";
        case "4":
            return "دانشگاه علمی – کاربردی";
        case "5":
            return "دانشگاه پیام‌ نور";
    }
}

/** ترجمه مقدار سطح ارشدیت در شغل به فارسی **/
function translateSeniorityLevel(seniorityLevel) {
    switch (seniorityLevel) {
        case "junior":
            return "تازه کار";
        case "expert":
            return "متخصص";
        case "manager":
            return "مدیر";
        case "chief":
            return "مدیر ارشد";
    }
};

/** ترجمه مقدار قرارداد قابل قبول به فارسی **/
function translateContract(contract) {
    switch (contract) {
        case "full-time":
            return "تمام وقت";
        case "part-time":
            return "پاره وقت";
        case "remote":
            return "دورکاری";
        case "intern":
            return "کارآموز";
    }
}

/** ترجمه مقدار قرارداد قابل قبول به فارسی **/
function translateMonth(month) {
    switch (month) {
        case "1":
            return "فروردین";
        case "2":
            return "اردیبهشت";
        case "3":
            return "خرداد";
        case "4":
            return "تیر";
        case "5":
            return "مرداد";
        case "6":
            return "شهریور";
        case "7":
            return "مهر";
        case "8":
            return "آبان";
        case "9":
            return "آذر";
        case "10":
            return "دی";
        case "11":
            return "بهمن";
        case "12":
            return "اسفند";
    }
}


function createToast(position, timer, nextStep = false, certificate = false, operation) {
    return Swal.mixin({
        toast: true,
        position,
        showConfirmButton: false,
        width: "30em",
        timer,
        timerProgressBar: true,
        didOpen: (toast) => {
            toast.onmouseenter = Swal.stopTimer;
            toast.onmouseleave = Swal.resumeTimer;
        },
        didDestroy: () => {
            if (nextStep && certificate === false) {
                operation === "post" && currentStep < Array.from(resumeSteps).length - 1 ? nextPreviousStep(currentStep + 1) : ""
            }
            if (nextStep && certificate) {
                nextPreviousStep(currentStep + 1);
                scrollStepsContainerLeft();
            }
        }
    })
}

/** تبدیل اعداد فارسی به انگلیسی **/
function convertToEnglishDigits(number) {
    const englishDigits = number.replace(/[٠-٩]/g, d => "٠١٢٣٤٥٦٧٨٩".indexOf(d)).replace(/[۰-۹]/g, d => "۰۱۲۳۴۵۶۷۸۹".indexOf(d));
    return Number(englishDigits)
}

/** دریافت فرمت‌های قابل قبول برای آپلود فایل‌های مدارک **/
async function getAllAcceptableFormats() {
    async function getFormats(id) {
        const response = await fetch(getFormatsUrl + id);
        const formats = await response.text();
        return formats.replaceAll(" ", "").replaceAll("'", "").replaceAll(",", ", ");
    }

    educationCertificateFormats = await getFormats(educationCertificateDocTypeId);
    certificateFormats = await getFormats(certificateDocTypeId);
    languageCertificateFormats = await getFormats(languageCertificateDocTypeId);
}

/** بررسی مجاز بودن فرمت فایل انتخاب شده در بخش بارگذاری مدرک **/
function certificateFormatValidator(element, section) {
    const selectedFileName = element.target.files[0].name;
    const fileFormat = selectedFileName.split('.').at(-1);
    let isValidFormat;
    switch (section) {
        case "education":
            isValidFormat = educationCertificateFormats.replaceAll(".", "").split(", ").includes(fileFormat);
            break;
        case "language":
            isValidFormat = languageCertificateFormats.replaceAll(".", "").split(", ").includes(fileFormat);
            break;
        case "certificate":
            isValidFormat = certificateFormats.replaceAll(".", "").split(", ").includes(fileFormat);
            break;
    }
    try {
        if (!isValidFormat) throw new Error("فرمت فایل انتخاب شده مجاز نیست.")
        switch (section) {
            case 'education':
                noEducationCertificateElement.classList.add('d-none');
                educationCertificateTitleElement.classList.remove('d-none');
                educationCertificateTitleElement.textContent = selectedFileName;
                break;
            case "language":
                noLanguageCertificateElement.classList.add('d-none');
                languageCertificateTitleElement.classList.remove('d-none');
                languageCertificateTitleElement.textContent = selectedFileName;
                break;
            case "certificate":
                noCertificateElement.classList.add('d-none');
                certificateTitleElement.classList.remove('d-none');
                certificateTitleElement.textContent = selectedFileName;
                break;
        }
    } catch (error) {
        const Toast = createToast("top-end", 4000)
        Toast.fire({
            icon: "error",
            title: `<h6 style="font-weight: 800">بروز خطا</h6>`,
            html: `<p style="margin-top: -10px">${error.message}</p>`,
        });
    }
}

/** نمایش یا بستن فرم سوابق شغلی براساس مقدار وضعیت شغلی فعلی **/
function showOrHideJobRecordsForm(value) {
    if (value === "lookingForJob") {
        jobRecordsSection.parentElement.classList.add('d-none');
        jobRecordsFormElement.classList.remove("show-information");
        jobRecordsFormElement.classList.add("hide-information");
        finishMonthElement.disabled = true;
        finishMonthElement.required = false;
        finishYearElement.disabled = true;
        finishYearElement.required = false;
        jobRecordsFormElement.classList.remove("was-validated");
        jobRecordsFormElement.reset();
    } else {
        jobRecordsSection.parentElement.classList.remove('d-none');
        jobRecordsFormElement.classList.remove("hide-information");
        jobRecordsFormElement.classList.add("show-information");
    }
}

/** نمایش فرم علاقه‌مندی های شغلی **/
function showJobPreferencesForm() {
    const jobPreferencesSection = document.getElementById('job-preferences');
    const jobPreferencesForm = document.getElementById('job-preferences-form');
    jobPreferencesSection.classList.remove("show-information");
    jobPreferencesSection.classList.add("hide-information");
    jobPreferencesForm.classList.remove("hide-information");
    jobPreferencesForm.classList.add("show-information");
    window.scroll({
        top: 550
    })
}

/******************** End ********************/

/******************** Event Listeners ********************/

/* های مربوط به دکمه های مختلفevent listener */
window.addEventListener('click', function (e) {
    /* دریافت شده section نمایش فرم ویرایش اطلاعات function فراخوانی */
    if (e.target.classList.contains('edit-info') || e.target.classList.contains('add-record')) {
        const section = e.target.dataset.section;
        onEditForm(section, `${section}-form`, 'showEditForm');
    }

    /* دریافت شده section بستن فرم ویرایش اطلاعات function فراخوانی */
    if (e.target.classList.contains('submit-add-button')) {
        const form = e.target.closest('form');
        e.preventDefault();
        form.classList.add("was-validated");
        if (!form.checkValidity()) {
            e.preventDefault();
            e.stopPropagation();
        }
        if (form.checkValidity()) {
            saveFormData(form, form.dataset.tag, false);
        }
    }

    /* دریافت شده section بستن فرم ویرایش اطلاعات function فراخوانی */
    if (e.target.classList.contains('cancel-form-button')) {
        const section = e.target.dataset.section;
        onEditForm(section, `${section}-form`, 'hideEditForm');
    }

    /* personal-information نمایش فرم ویرایش اطلاعات بخش function فراخوانی */
    if (e.target.classList.contains('add-information-item')) {
        const section = e.target.dataset.section;
        onEditForm(section, `${section}-form`, 'showEditForm');
    }

    /* job-preferences نمایش فرم ویرایش اطلاعات بخش function فراخوانی */
    if (e.target.classList.contains('add-preference-item')) {
        const section = e.target.dataset.section;
        onEditForm(section, `${section}-form`, 'showEditForm');
    }

    /* حذف آیتم سابقه شغلی function فراخوانی */
    if (e.target.classList.contains('delete-job-record')) {
        const jobRecordId = e.target.dataset.jobRecordId;
        deleteJobItem(jobRecordId);
    }

    /* غیراجباری کردن فیلدهای نام و نوع دانشگاه درصورتی که تحصیلات کاربر دیپلم باشد */
    if (e.target.name === "grade") {
        if (e.target.value === "diploma") {
            institutionNameInputElement.required = false;
            institutionTypeSelectElement.required = false;
        } else {
            institutionNameInputElement.required = true;
            institutionTypeSelectElement.required = true
        }
    }

    /* حذف آیتم سابقه تحصیلی function فراخوانی */
    if (e.target.classList.contains('delete-education-record')) {
        const educationRecordId = e.target.dataset.educationRecordId;
        const attachmentEntityId = e.target.dataset.entityId;
        deleteEducationItem(educationRecordId, attachmentEntityId);
    }

    /* حذف آیتم دوره آموزشی function فراخوانی */
    if (e.target.classList.contains('delete-course-record')) {
        const courseRecordId = e.target.dataset.courseRecordId;
        const attachmentEntityId = e.target.dataset.entityId;
        deleteCourseItem(courseRecordId, attachmentEntityId);
    }

    /* حذف آیتم زبان function فراخوانی */
    if (e.target.classList.contains('delete-language')) {
        const languageId = e.target.dataset.languageId;
        const attachmentEntityId = e.target.dataset.entityId;
        deleteLanguageItem(languageId, attachmentEntityId);
    }

    /* حذف آیتم شهر و استان ترجیحی function فراخوانی */
    if (e.target.classList.contains('delete-location')) {
        const locationId = e.target.dataset.locationId;
        deleteLocationItem(locationId);
    }

    /* نمایش فیلد توضیحات در بخش سطوح سازمانی درصورت انتخاب گزینه سایر */
    if (e.target.name === "seniorityLevels") {
        if (e.target.value === "otherSeniorityLevel" && e.target.checked) {
            seniorityLevelsDescriptionInputElement.classList.remove('d-none');
            seniorityLevelsDescriptionInputElement.focus();
            seniorityLevelsDescriptionInputElement.required = true;
        } else {
            seniorityLevelsDescriptionInputElement.classList.add('d-none');
            seniorityLevelsDescriptionInputElement.value = "";
            seniorityLevelsDescriptionInputElement.required = false;
        }
    }

    /* نمایش فیلد توضیحات در بخش نوع همکاری درصورت انتخاب گزینه سایر */
    if (e.target.name === "contracts") {
        if (e.target.value === "otherContract" && e.target.checked) {
            contractsDescriptionInputElement.classList.remove('d-none');
            contractsDescriptionInputElement.focus();
            contractsDescriptionInputElement.required = true;
        } else {
            contractsDescriptionInputElement.classList.add('d-none');
            contractsDescriptionInputElement.value = "";
            contractsDescriptionInputElement.required = false;
        }
    }

    /* رفتن به مرحله مورد نظر */
    if (e.target.classList.contains('step-item')) {
        const stepIndex = e.target.dataset.step;
        nextPreviousStep(+stepIndex)
    }
});

/** وضعیت خدمت سربازی در صورتی که جنسیت کاربر مرد باشد dropdown فعال کردن **/
genderRadioButtonsContainer.addEventListener("click", function (e) {
    const radioButtonElement = e.target.name === "gender";
    if (radioButtonElement && e.target.value === "male") {
        militaryServiceStatusContainer.classList.remove('d-none');
        militaryServiceContainer.querySelector("select").required = true;
        militaryServiceContainer.querySelector("select").disabled = false;
    } else {
        militaryServiceStatusContainer.classList.add('d-none');
        militaryServiceContainer.querySelector("select").required = false;
        militaryServiceContainer.querySelector("select").disabled = true;
    }
});

/** بررسی مقدار وضعیت شغلی فعلی انتخاب شده **/
careerRadioButtonsContainer.addEventListener("input", function (e) {
    showOrHideJobRecordsForm(e.target.value);
    userResume.careerProfile[e.target.name] = e.target.value;
});

/** تعداد فرزند در صورتی که کاربر متاهل باشد dropdown فعال کردن **/
marriageStatusRadioButtonsContainer.addEventListener("click", function (e) {
    const radioButtonElement = e.target.name === "marriageStatus";
    if (radioButtonElement && e.target.value === "married") {
        childrenCountContainer.classList.remove('d-none');
        childrenCountSelectElement.querySelector("select").required = true;
        childrenCountSelectElement.querySelector("select").disabled = false;
    } else {
        childrenCountContainer.classList.add('d-none');
        childrenCountSelectElement.querySelector("select").required = false;
        childrenCountSelectElement.querySelector("select").disabled = true;
    }
});

/** نوع معلولیت در صورتی که کاربر دارای معلولیت باشد input فعال کردن **/
physicalStatusRadioButtonsContainer.addEventListener("click", function (e) {
    const radioButtonElement = e.target.name === "physicalStatus";
    if (radioButtonElement && e.target.value === "disabled") {
        disabilityTypeContainer.classList.remove('d-none');
        disabilityTypeInputElement.required = true;
        disabilityTypeInputElement.disabled = false;
    } else {
        disabilityTypeContainer.classList.add('d-none');
        disabilityTypeInputElement.required = false;
        disabilityTypeInputElement.disabled = true;
    }
});

/** اضافه کردن شهر و استان مورد نظر در بخش ترجیحات شغلی به لیست مکان‌های ترجیحی **/
addLocationButton.addEventListener("click", async function (e) {
    const preferredProvinceElement =
        document.getElementById("preferred-province");
    const preferredCityElement = document.getElementById("preferred-city");
    if ((isNaN(+preferredProvinceElement.value) || +preferredProvinceElement.value <= 0) || (isNaN(+preferredCityElement.value) || +preferredCityElement.value <= 0)) {
        Swal.fire({
            icon: "error",
            title: "خطایی رخ داد",
            text: "کاربر گرامی، مقدار شهر یا استان نمیتواند خالی باشد.",
            confirmButtonText: 'بستن',
        });
        preferredProvinceElement.value = '';
        preferredCityElement.innerHTML =
            "<option selected disabled value='placeholder'>-- انتخاب کنید --</option>";
        preferredCityElement.value = 'placeholder';
        return
    };
    locations.push({
        id: locations.length,
        province: preferredProvinceElement.value,
        city: preferredCityElement.value,
        isNew: true
    });
    preferredProvinceElement.value = "";
    preferredCityElement.innerHTML =
        "<option selected disabled>-- انتخاب کنید --</option>";
    await addLocationToList(locationsListFormElement, locations, "form");
    await addLocationToList(locationsListElement, locations);
    locationsElement.value = JSON.stringify(locations);
});

/** زمانی که کاربر دیگر در شرکت مذکور فعالیت نمیکند disabled های ماه و سال پایان همکاری از حالتinput خارج کردن **/
isStillWorkingElement.addEventListener("click", function (e) {
    if (!e.target.checked) {
        finishMonthElement.removeAttribute("disabled");
        finishMonthElement.setAttribute("required", true);
        finishYearElement.removeAttribute("disabled");
        finishYearElement.setAttribute("required", true);
    } else {
        finishMonthElement.value = "";
        finishMonthElement.setAttribute("disabled", true);
        finishMonthElement.removeAttribute("required");
        finishYearElement.value = "";
        finishYearElement.setAttribute("disabled", true);
        finishYearElement.removeAttribute("required");
    }
});

/** زمانی که کاربر دیگر در موسسه مذکور تحصیل نمیکند disabled سال پایان تحصیل از حالت input خارج کردن **/
isStillEducatingElement.addEventListener("click", function (e) {
    if (!e.target.checked) {
        graduationYearSelectElement.removeAttribute("disabled");
        graduationYearSelectElement.setAttribute("required", true);
    } else {
        graduationYearSelectElement.value = "";
        graduationYearSelectElement.setAttribute("disabled", true);
        graduationYearSelectElement.removeAttribute("required");
    }
});

/** میکند hover نمایش دکمه حذف هنگامی که کاربر روی آیتم سابقه شغلی در لیست سوابق شغلی **/
jobsListElement.addEventListener("mouseover", function (e) {
    showDeleteButton(e, "job-card");
});

/** میکند hover نمایش دکمه حذف هنگامی که کاربر روی آیتم مکان‌های ترجیجی در لیست مکان‌های ترجیحی **/
locationsListElement.addEventListener("mouseover", function (e) {
    showDeleteButton(e, "location-card");
});

continueButtonElement.addEventListener('click', function () {
    const currentForm = forms[currentStep];
    const isCurrentFormOpen = currentForm.classList.contains('show-information');
    switch (isCurrentFormOpen) {
        case true:
            currentForm.classList.add("was-validated");
            if (currentForm.checkValidity()) {
                saveFormData(currentForm, currentForm.dataset.tag, true);
            }
            break;
        case false:
            nextPreviousStep(currentStep + 1);
            scrollStepsContainerLeft();
            break;
    }
    function scrollStepsContainerLeft() {
        stepsContainerElement.scrollLeft -= (currentStep + 1) * 35
        if (window.innerWidth < 992) {
            stepsContainerElement.scrollLeft -= (currentStep + 1) * 40
        }
    }
})

educationCertificateButton.addEventListener('change', (e) => certificateFormatValidator(e, 'education'));
languageCertificateButton.addEventListener('change', (e) => certificateFormatValidator(e, 'language'));
certificateButton.addEventListener('change', (e) => certificateFormatValidator(e, 'certificate'));

showResumeButtonElement.addEventListener('click', function () {
    isPersonalInformationSectionIncomplete = Array.from(personalInformationSectionProfileItems).every(information => {
        if (information.classList.contains('show-information')) {
            return false
        } else {
            return true
        }
    });
    switch (isPersonalInformationSectionIncomplete) {
        case false:
            window.location.href = '/home/showResume';
            break;
        case true:
            Swal.fire({
                icon: "error",
                title: "خطایی رخ داد",
                text: "برای مشاهده رزومه خود، باید حداقل بخش اطلاعات فردی را تکمیل کنید.",
                confirmButtonText: 'بستن',
            });
            break;
    }
})

/******************** End ********************/

/******************** Template Generator Functions ********************/
/** آیتم‌های لیست سوابق شغلی html ساخت **/
async function generateJobRecordTemplate(record) {
    const startMonth = translateMonth(record.startMonth)
    const finishMonth = translateMonth(record.finishMonth)
    return `
    <li class="job-card list-group-item p-0 position-relative overflow-hidden d-flex flex-column flex-sm-row gap-3 justify-content-between align-items-center">
        <div class="w-100 pe-none">
            <div class="p-3">
                <h6 class="mb-0 fw-bold">${record.jobTitle}<span class="profile-item"> (${record.role})<span></h6>
            <div class="d-flex flex-column flex-sm-row profile-item gap-sm-2">
              <p class="mb-0" style="font-size: 14px; color: rgb(85, 85, 85)">${record.company}</p>
              <div>
                <svg xmlns="http://www.w3.org/2000/svg" class="icon icon-tabler icon-tabler-separator" width="24" height="24" viewBox="0 0 24 24" stroke-width="1.5" stroke="#000000" fill="none" stroke-linecap="round" stroke-linejoin="round">
                  <path stroke="none" d="M0 0h24v24H0z" fill="none"/>
                  <path d="M3 12l0 .01" />
                  <path d="M7 12l10 0" />
                  <path d="M21 12l0 .01" />
                </svg>
              </div>
              <div class="d-flex gap-2">
                <span>از</span>
                <div class="d-flex text-center gap-1">
                    <p class="mb-0">${startMonth}</p>
                    <p class="mb-0">${record.startYear}</p>
                </div>
                ${record.stillWorking
            ? `<p class="mb-0">تا حالا</p>`
            : `
                <div class="d-flex align-items-center gap-1">
                <span>تا</span>
                ${record.finishMonth
                ? `<div class="d-flex text-center gap-1">
                    <p class="mb-0">${finishMonth}</p>
                    <p class="mb-0">${record.finishYear}</p>
                </div>`
                : `<p class="mb-0">${record.finishYear}</p>`
            }
                </div>
                `
        }
              </div>
            </div>
            </div>
            ${record.jobDescription ? `
            <div class="bg-dark py-2 p-3">
                <p class="mb-0 text-white" style="font-size: 14px">${record.jobDescription}</p>
            </div>` : ""}
        </div>
      <button type="button" class="delete-job-record delete-button btn btn-secondary position-absolute d-none" style="left: 20px; top: 20px" data-job-record-id="${record.id}">x</button>
    </li>`;
}

/** آیتم‌های لیست سوابق تحصیلی html ساخت **/
function generateEducationRecordTemplate(record) {
    return `
    <li class="education-card list-group-item p-0 position-relative overflow-hidden d-flex flex-column justify-content-between" style="min-width: 250px">
        <div class="d-flex flex-column flex-md-row gap-3 justify-content-between p-2">
        <div>
            <div>
                <h6 class="mb-0 fw-bold">${record.major}${record.grade
            ? `<span class="profile-item"> (${translateGrade(record.grade)})<span>`
            : ""
        }</h6>
        <div class="d-flex align-items-center gap-1"><p class="mb-0" style="font-size: 14px; color: rgb(85, 85, 85)">با معدل</p><p class="mb-0" style="font-size: 14px; color: rgb(119, 119, 119)">${record.average}</p></div>
            <div class="d-flex flex-column flex-sm-row profile-item gap-sm-2">
              ${record.institution ? `<p class="mb-0" style="font-size: 14px; color: rgb(85, 85, 85)">${record.institution}</p>` : ''}
              <div>
                <svg xmlns="http://www.w3.org/2000/svg" class="icon icon-tabler icon-tabler-separator" width="24" height="24" viewBox="0 0 24 24" stroke-width="1.5" stroke="#000000" fill="none" stroke-linecap="round" stroke-linejoin="round">
                  <path stroke="none" d="M0 0h24v24H0z" fill="none"/>
                  <path d="M3 12l0 .01" />
                  <path d="M7 12l10 0" />
                  <path d="M21 12l0 .01" />
                </svg>
              </div>
              ${record.institutionType ? `<p class="mb-0" style="font-size: 14px; color: rgb(119, 119, 119)">${translateInstitutionType(record.institutionType)}</p>` : ''}
            </div>
            <div class="d-flex gap-2">
                <span>از</span>
                <p class="mb-0" style="font-size: 14px; color: rgb(119, 119, 119)">${record.educationStartYear}</p>
                ${record.stillEducating
            ? `<p class="mb-0" style="font-size: 14px; color: rgb(119, 119, 119)">تا حالا</p>`
            : `
                <div class="d-flex align-items-center gap-1">
                <span>تا</span>
                <p class="mb-0" style="font-size: 14px; color: rgb(119, 119, 119)">${record.graduationYear}</p>
                </div>
                `
        }
              </div>
            </div>
        </div>
        <div class="d-flex flex-column gap-1 ms-auto" style="max-width: 250px">
                ${record.attachment?.length > 0 ? `
                    <a href="home/GetCertificateFile?certificateId=${record.attachment.at(0).entityId}&docTypeId=${educationCertificateDocTypeId}" download="مدرک رشته ${record.major}" class="btn btn-primary" style="font-size: 13px !important; height: 35px; cursor: pointer; width: 90px; align-self: end">دانلود فایل</a>
                `
            : ''}
        <button type="button" class="delete-education-record btn btn-secondary" data-entity-id="${record.attachment?.at(0).entityId}" data-education-record-id="${record.id}" style="font-size: 14px !important; height: 35px; width: 90px; cursor: pointer; align-self: end">حذف</button>
                </div>
        </div>
                ${record.educationDescription ? `
            <div class="bg-dark py-2 p-3">
                <p class="mb-0 text-white" style="font-size: 14px">${record.educationDescription}</p>
            </div>` : ""}
    </li>`;
}

/** آیتم‌های لیست مدارک و گواهینامه‌ها html ساخت **/
function generateCertificateRecordTemplate(certificate) {
    return `
    <li class="certificate-card list-group-item p-0 position-relative overflow-hidden d-flex flex-column flex-sm-row gap-3 justify-content-between align-items-center p-2" style="min-width: 250px">
        <div class="d-flex justify-content-between align-items-cneter w-100">
            <div>
                <div class="d-flex align-items-center gap-1">
                    <div style="margin-bottom: 5px">
                        <svg width="20" height="20">
                            <use href="../images/icons/header.svg#certificate" />
                        </svg>
                    </div>
                    <p class="my-1" style="font-size: 15px; font-weight: 600">${certificate.title} - <span style="font-weight: normal">${certificate.courseLevel}</span></p>
                </div>
                <div class="d-flex align-items-center gap-1">
                    <div style="margin-right: -3px">
                        <svg viewBox="0 0 24 24" width="20" height="20" stroke="currentColor" stroke-width="1.5">
                            <use href="../images/icons/general.svg#map-pin" />
                        </svg>
                    </div>
                    <p class="mb-0" style="font-size: 14px">${certificate.courseLocation}</p>
                </div>
            </div>
            <div class="d-flex flex-column gap-1" style="max-width: 250px">
                ${certificate.attachment?.length > 0 ? `
                    <a href="home/GetCertificateFile?certificateId=${certificate.attachment.at(0).entityId}&docTypeId=${certificateDocTypeId}" download="${certificate.title}" class="btn btn-primary" style="font-size: 14px !important; height: 35px; cursor: pointer; width: 90px; align-self: end">دانلود فایل</a>
                ` : ''}
                <button type="button" class="delete-course-record btn btn-secondary" data-entity-id="${certificate.attachment?.at(0).entityId}" data-course-record-id="${certificate.id}" style="font-size: 14px !important; height: 35px; width: 90px; cursor: pointer; align-self: end">حذف</button>
            </div>
        </div>
    </li>`;
}

/** آیتم‌های لیست زبان‌ها html ساخت **/
function generateLanguageCardTemplate(language) {
    const languageWithMastery = getLanguageAndMastery(
        +language.language,
        +language.mastery
    );
    return `
  <li class="language-card d-flex align-items-center justify-content-between list-group-item border rounded-2 p-2" style="flex: 1; min-width:200px;">
    <div class="p-3"><h6 class="mb-0 fw-bold">${languageWithMastery.at(0)}<span class="profile-item"> (${languageWithMastery.at(1)})<span></h6></div>
    <div class="d-flex flex-column gap-1" style="max-width: 250px">
                ${language.attachment?.length > 0 ? `
                    <a href="home/GetCertificateFile?certificateId=${language.attachment.at(0).entityId}&docTypeId=${languageCertificateDocTypeId}" download="مدرک زبان ${languageWithMastery.at(0)}" class="btn btn-primary" style="font-size: 14px !important; height: 35px; cursor: pointer; width: 90px; align-self: end">دانلود فایل</a>
                ` : ''}
        <button type="button" class="delete-language btn btn-secondary" data-entity-id="${language.attachment?.at(0).entityId}" data-language-id="${language.id}" style="font-size: 14px !important; height: 35px; width: 90px; cursor: pointer; align-self: end">حذف</button>
                </div>
  </li>
  `;
}

/** آیتم‌های لیست سطوح ارشدیت در بخش ترجیحات شغلی html ساخت **/
function generateSeniorityLevelCardTemplate(seniorityLevel) {
    return seniorityLevel === "otherSeniorityLevel" ? `<li class="list-group-item d-flex justify-content-between align-items-center" style="color: inherit">${userResume.careerProfile.jobPreferences.seniorityLevelsDescription}</li>` : `<li class="list-group-item d-flex justify-content-between align-items-center" style="color: inherit">${translateSeniorityLevel(
        seniorityLevel
    )}</li>`;
}

/** آیتم‌های لیست قراردادهای قابل قبول در بخش ترجیات شغلی html ساخت **/
function generateContractCardTemplate(contract) {
    return contract === "otherContract" ? `<li class="list-group-item d-flex justify-content-between align-items-center" style="color: inherit">${userResume.careerProfile.jobPreferences.contractsDescription}</li>` : `<li class="list-group-item d-flex justify-content-between align-items-center" style="color: inherit">${translateContract(
        contract
    )}</li>`;
}

/** حداقل حقوق درخواستی در بخش ترجیحات شغلی dropdown مبالغ مختلف option هر html ساخت **/
function generateWageSelectOptionTemplate(index, wage) {
    return `
  <option value="${index}">${index !== "0" && index !== "1"
            ? `${new Intl.NumberFormat("en-US").format(+wage * 1000000)} تومان`
            : `${getWage(index)}`
        }</option>
  `;
}

/** لیست استان‌ها dropdown در option هر html ساخت **/
function generateProvinceSelectOptionTemplate(item) {
    return `
  <option value="${item.id}">${item.title}</option>
  `;
}

/** لیست شهرها dropdown در option هر html ساخت **/
function generateCitySelectOptionTemplate(item) {
    return `
  <option value="${item.id}">${item.title}</option>
  `;
}

/** مورد نظر dropdown در option هر html ساخت **/
function generateYearSelectOptionTemplate(index, item, currentYear, isBirthYear) {
    const age = currentYear - index;
    return `
        ${isBirthYear ? `<option value="${index}">${item} - ${age} سال</option>` : `<option value="${index}">${item}</option>`}
  `;
}

/** مورد نظر dropdown در option هر html ساخت **/
function generateSelectOptionTemplate(index, item) {
    return `
  <option value="${index}">${item}</option>
  `;
}

/** آیتم‌های لیست مکان‌های ترجیحی در بخش ترجیات شغلی html ساخت **/
function generatePreferredLocationCardTemplate(id, province, city, type) {
    return `<li class="location-card list-group-item p-3 py-1 d-flex justify-content-between align-items-center" style="color: inherit"><span class="my-2">${province?.title} - ${city?.title}</span>
    <button type="button" class="delete-location delete-button btn btn-secondary ${type !== "form" ? "d-none" : ""}" data-location-id="${id}">x</button>
    </li>`;
}


/** آیتم‌های لیست دسته بندی‌های شغلی در بخش ترجیات شغلی html ساخت **/
function generateFieldCardTemplate(field) {
    return `<li class="location-card list-group-item p-3 py-1 d-flex justify-content-between align-items-center" style="color: inherit"><span class="my-1">${field.title}</span></li>`;
}

/** آیتم‌های لیست توصیه‌های مربوط به بهبود امتیاز رزومه در بخش امتیازدهی html ساخت **/
function generateImprovementSuggestionItemTemplate(suggestion) {
    return `
  <div
    class="text-decoration-none text-right"
    style="font-size: 12px"
  >
    <div
      class="border rounded-2 fw-normal text-black d-inline-block text-center"
      style="width: 45px"
    >
      <span style="color: #1abc9c">+</span>
      <span>${suggestion.score}</span>
      %
    </div>
     <span>${suggestion.suggestion}</span>
  </div>
  `;
}

/** هر مرحله رزومه در بخش مراحل رزومه html ساخت **/
function generateStepTabTemplate(index, title) {
    return `
        <li class="step-item position-relative d-flex px-3" data-step="${index}" style="cursor: pointer">
          <div class="d-flex w-100 align-items-center gap-3 pe-none">
            <div class="d-flex align-items-center justify-content-center step-circle">
              <p class="mb-0">
               ${index + 1}
              </p>
            </div>
            <span class="text-dark fs-6 text-nowrap step-title">${title?.textContent || "اطلاعات کلی"}</span>
          </div>
          ${index !== resumeSteps.length - 1 ? `<svg class="ms-3" style="transform: scale(-1)" viewBox="0 0 22 80" fill="none" preserveAspectRatio="none"><path d="M0 -2L20 40L0 82" vector-effect="non-scaling-stroke" stroke="#dee2e6" stroke-linejoin="round"></path></svg>` : ""}
        </li>
    `
}
/******************** End ********************/

/******************** Form Handling ********************/
/** جهت ارسال به سرور object ها در فرم مورد نظر و ذخیره آنها در قالبinput دریافت مقادیر **/
async function saveFormData(formElement, tag, shouldHide) {
    const formData = new FormData(formElement);
    const Toast = createToast("top-end", 4000);
    record = {};
    if (tag === "certificates" || tag === "languages" || tag === "education") {
        await postCertificate(formElement, tag, shouldHide);
    } else {
        if (
            tag &&
            tag !== "jobs" &&
            tag !== "information" &&
            tag !== "jobPreferences"
        ) {
            if (!userResume[tag]) {
                userResume[tag] = []
            } else {
                record.id = userResume[tag]?.length;
            }
            for (let [key, value] of formData.entries()) {
                record[key] = value;
            }
            userResume[tag] = [...userResume[tag], record];
        } else if (tag === "jobs") {
            if (!userResume.careerProfile[tag]) {
                userResume.careerProfile[tag] = []
            } else if (finishMonthElement.value && jobRecordFinishYearSelectElement.value) {
                if (+jobRecordStartYearSelectElement.value === +jobRecordFinishYearSelectElement.value && +finishMonthElement.value < +startMonthSelectElement.value) {
                    Toast.fire({
                        icon: "error",
                        title: `<h6 style="font-weight: 800">بروز خطا</h6>`,
                        html: `<p style="margin-top: -10px">کاربر گرامی، ماه پایان همکاری نمیتواند قبل از ماه شروع همکاری باشد.</p>`,
                    });
                    return
                }
                if (+jobRecordFinishYearSelectElement.value < +jobRecordStartYearSelectElement.value || (+jobRecordFinishYearSelectElement.value === +jobRecordStartYearSelectElement.value && +finishMonthElement.value === +startMonthSelectElement.value)) {
                    Toast.fire({
                        icon: "error",
                        title: `<h6 style="font-weight: 800">بروز خطا</h6>`,
                        html: `<p style="margin-top: -10px">کاربر گرامی، تاریخ پایان همکاری نمیتواند قبل از تاریخ شروع همکاری باشد.</p>`,
                    });
                    return
                }
            }
            record.id = userResume.careerProfile[tag]?.length;
            for (let [key, value] of formData.entries()) {
                record[key] = value;
            }
            userResume.careerProfile[tag] = [...userResume.careerProfile[tag], record];
        } else if (tag === "jobPreferences") {
            userResume.careerProfile.jobPreferences.seniorityLevels = [];
            userResume.careerProfile.jobPreferences.contracts = [];
            userResume.careerProfile.jobPreferences.jobFields = [];
            for (let [key, value] of formData.entries()) {
                if (
                    key === "seniorityLevels" ||
                    key === "contracts"
                ) {
                    userResume.careerProfile.jobPreferences[key].push(value);
                } else if (key === "jobFields") {
                    function getJobFields() {
                        const values = $('#job-fields').select2('data'); // Get the selected values
                        const selectedTags = values.map(function (value) {
                            const item = jobFields.find(function (field) {
                                return field.id === +value.id;
                            });
                            return {
                                id: item.id,
                                title: item.title
                            };
                        });
                        return selectedTags;
                    }
                    userResume.careerProfile.jobPreferences[key] = getJobFields();
                } else if (key === "preferredLocations") {
                    const finalLocations = locations.map(location => { return { ...location, isNew: false } });
                    locations = finalLocations;
                    userResume.careerProfile.jobPreferences[key] = locations;
                } else {
                    userResume.careerProfile[tag][key] = value;
                }
            }
        } else {
            for (let [key, value] of formData.entries()) {
                if (tag === "information") {
                    const nationalIdValidity = checkNationalId(nationalIdInputElement.value);
                    const guardianNationalIdValidity = checkNationalId(guardianNationalIdInputElement.value);
                    const Toast = createToast("top-end", 4000);
                    if (!nationalIdValidity) {
                        Toast.fire({
                            icon: "error",
                            title: `<h6 style="font-weight: 800">بروز خطا</h6>`,
                            html: `<p style="margin-top: -10px">کدملی وارد شده معتبر نیست!</p>`,
                        });
                        return
                    } else if (!guardianNationalIdValidity) {
                        Toast.fire({
                            icon: "error",
                            title: `<h6 style="font-weight: 800">بروز خطا</h6>`,
                            html: `<p style="margin-top: -10px">کدملی سرپرست معتبر نیست!</p>`,
                        });
                        return
                    } else {
                        userResume[tag][key] = value;
                    }
                } else {
                    userResume[tag][key] = value;
                }
            }
        }
        await postResume(userResume, "post", shouldHide);

        if (tag === "jobs" && shouldHide) {
            showJobPreferencesForm();
        };

        if (tag === "jobs" && !shouldHide) {
            finishMonthElement.disabled = true;
            finishMonthElement.required = false;
            finishYearElement.disabled = true;
            finishYearElement.required = false;
            formElement.classList.remove("was-validated");
            formElement.reset();
        };

        const informationElementId = formElement.id.slice(0, -5);
        const informationElement = document.getElementById(informationElementId);
        if (shouldHide) {
            formElement.classList.remove("show-information");
            formElement.classList.add("hide-information");
            informationElement.classList.remove("hide-information");
            informationElement.classList.add("show-information");
            graduationYearSelectElement.disabled = true;
            graduationYearSelectElement.required = false;
            finishMonthElement.disabled = true;
            finishMonthElement.required = false;
            finishYearElement.disabled = true;
            finishYearElement.required = false;
            formElement.classList.remove("was-validated");
            formElement.reset();
        };
    }

    institutionNameInputElement.required = true;
    institutionTypeSelectElement.required = true;
    educationCertificateTitleElement.classList.add('d-none');
    noEducationCertificateElement.classList.remove('d-none');
    certificateTitleElement.classList.add('d-none');
    noCertificateElement.classList.remove('d-none');
    languageCertificateTitleElement.classList.add('d-none');
    noLanguageCertificateElement.classList.remove('d-none');

    if (tag !== 'jobs') {
        window.scrollTo({
            top: 0
        });
    }
}

/** نمایش فرم ویرایش اطلاعات در بخش مورد نظر **/
function onEditForm(informationElementId, formElementId, type) {
    const informationElement = document.getElementById(informationElementId);
    const formElement = document.getElementById(formElementId);

    switch (type) {
        case "showEditForm":
            if (informationElementId !== "certificate-records" &&
                informationElementId !== "education-records" &&
                informationElementId !== "languages" &&
                informationElementId !== "job-records") {
                informationElement.classList.remove("show-information");
                informationElement.classList.add("hide-information");
            }
            formElement.classList.remove("hide-information");
            formElement.classList.add("show-information");
            showExistingDataInForm(informationElementId);
            break;
        case "hideEditForm":
            window.scrollTo({
                top: 0
            });
            if (informationElementId === "job-preferences") {
                const finalLocations = locations.filter(location => location.isNew === false);
                locations = finalLocations;
                addLocationToList(locationsListFormElement, locations, "form");
                addLocationToList(locationsListElement, locations);
                locationsElement.value = JSON.stringify(locations);
            }
            formElement.reset();
            $('#major').val('').trigger('change');
            formElement.classList.remove("was-validated");
            informationElement.classList.remove("hide-information");
            informationElement.classList.add("show-information");
            formElement.classList.remove("show-information");
            formElement.classList.add("hide-information");
            institutionNameInputElement.required = true;
            institutionTypeSelectElement.required = true;
            noEducationCertificateElement.classList.remove('d-none');
            educationCertificateTitleElement.classList.add('d-none');
            noCertificateElement.classList.remove('d-none');
            certificateTitleElement.classList.add('d-none');
            noLanguageCertificateElement.classList.remove('d-none');
            languageCertificateTitleElement.classList.add('d-none');
            seniorityLevelsDescriptionInputElement.classList.add('d-none');
            seniorityLevelsDescriptionInputElement.value = "";
            seniorityLevelsDescriptionInputElement.required = false;
            contractsDescriptionInputElement.classList.add('d-none');
            contractsDescriptionInputElement.value = "";
            contractsDescriptionInputElement.required = false;
    }
}
/******************** End ********************/