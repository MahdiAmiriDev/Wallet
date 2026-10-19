const loaderModalElement = document.getElementById('loaderModal');
const genderRadioButtonsContainer = document.getElementById(
    "gender-select-container"
);
const militaryServiceContainer = document.getElementById(
    "military-service-select"
);

const addSkillButton = document.getElementById("add-skill-button");
const skillsCountElement = document.querySelector(".skills-count");
const skillsListFormElement = document.getElementById("skills-list-form");
const skillsListElement = document.getElementById("skills-list");
const noSkillsMessage = document.querySelector(".no-skills-found");
const skillsElement = document.getElementById("skills");

const isStillWorkingElement = document.getElementById("still-working");
const finishMonthElement = document.getElementById("finish-month");
const finishYearElement = document.getElementById("finish-year");
const jobsListElement = document.getElementById("jobs-list");
const noJobsMessage = document.querySelector(".no-jobs-found");

const isStillEducatingElement = document.getElementById("still-educating");
const graduationYear = document.getElementById("graduation-year");
const educationListElement = document.getElementById("education-list");
const noEducationMessage = document.querySelector(".no-education-found");

const coursesListElement = document.getElementById("courses-list");
const noCoursesMessage = document.querySelector(".no-courses-found");

const certificatesListElement = document.getElementById("certificates-list");
const noCertificatesMessage = document.querySelector(".no-certificates-found");
const certificateTitleInput = document.getElementById('title');
const certificateFileInput = document.getElementById('attachments');

const languagesListElement = document.getElementById("languages-list");
const noLanguagesMessage = document.querySelector(".no-languages-found");

const seniorityLevelElement = document.getElementById("seniority-level");
const contractPreferencesElement = document.getElementById(
    "contract-preferences"
);

const jobTitleContainer = document.querySelector(".job-title");
const jobTitleInputElement = document.getElementById("job-title");

const occupationStatusContainer = document.querySelector(".occupation-status");
const occupationStatusInputElements = document.querySelectorAll(
    "input[name='occupationStatus']"
);

const lastCompanyContainer = document.querySelector(".last-company");
const lastCompanyInput = document.querySelector(".last-company-input");

const lastDegreeContainer = document.querySelector(".last-degree");
const lastDegreeInput = document.querySelector(".last-degree-input");

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
const birthYearInputElement = document.getElementById("birth-year");

const marriageStatusContainer = document.querySelector(".marriage-status");
const marriageStatusInputElements = document.querySelectorAll(
    "input[name='marriageStatus']"
);

const genderContainer = document.querySelector(".gender");
const genderInputElements = document.querySelectorAll("input[name='gender']");

const militaryContainer = document.querySelector(".military-service");
const militaryInputElement = document.getElementById("military-service");

const aboutMeContainer = document.querySelector(".about-me");
const aboutMeTextArea = document.querySelector("textarea#about-me");

const preferredLocationsContainer = document.querySelector(
    ".preferred-locations"
);
const fieldsContainer = document.querySelector(".fields");
const fieldInputElement = document.getElementById("fields");

const minimumWageContainer = document.querySelector(".minimum-wage");
const minimumWageInputElement = document.getElementById("minimum-wage");

const seniorityLevelsContainer = document.querySelector(".seniority-level");
const seniorityLevelInputElements = document.querySelectorAll(
    "input[name='seniorityLevels']"
);

const contractsContainer = document.querySelector(".contract");
const contractInputElements = document.querySelectorAll(
    "input[name='contracts']"
);

/** در نسخه فعلی نمایش داده نشود **/
//const jobBenefitsContainer = document.querySelector(
//    "#job-benefits"
//);
//const jobBenefitsInputElements = document.querySelectorAll(
//    "input[name='jobBenefits']"
//);

const profileItemElement = document.querySelector(".profile-item");
const profileItemButton = document.querySelector(".profile-add-item");

// Dropdown Elements
const provinceSelectElement = document.querySelector("select#province");
const citySelectElement = document.querySelector("select#city");
const militaryServiceSelectElement = document.querySelector(
    "select#military-service"
);
const startMonthSelectElement = document.querySelector("select#start-month");
const finishMonthSelectElement = document.querySelector("select#finish-month");
const languageSelectElement = document.querySelector("select#language");
const masterySelectElement = document.querySelector("select#mastery");
const preferredProvinceSelectElement = document.querySelector(
    "select#preferred-province"
);
const preferredCitySelectElement = document.querySelector(
    "select#preferred-city"
);
const wageSelectElement = document.querySelector("select#minimum-wage");
const addLocationButton = document.getElementById("add-location-button");
const locationsListFormElement = document.getElementById("locations-list-form");
const locationsListElement = document.getElementById("locations-list");
const locationsElement = document.getElementById("preferred-locations");

const fieldSelectElement = document.querySelector("select#job-fields");
const fieldsListElement = document.getElementById("fields-list");

// Score Component Elements
const heroSectionProfileItems = document
    .getElementById("hero")
    .querySelectorAll(".profile-item");
const personalInformationSectionProfileItems = document
    .getElementById("personal-information")
    .querySelectorAll(".profile-item");
const aboutMeSectionProfileItem = document
    .getElementById("about-me")
    .querySelector(".profile-item");
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






let isHeroSectionIncomplete;
let isPersonalInformationSectionIncomplete;
let isAboutMeSectionIncomplete;
let isSkillsSectionIncomplete;
let isJobsSectionIncomplete;
let isEducationSectionIncomplete;
let isCoursesSectionIncomplete;
let isCertificatesSectionIncomplete;
let isLanguagesSectionIncomplete;
let isJobPreferencesSectionIncomplete;

const api = "http://localhost:3000";
const getUrl = "/api/user/GetUserResume";
const postUrl = "/api/user/AddUserResume";
let userResume;
let userCertificates;
let skills = [];
let locations = [];
let fields = [];
let suggestions = [];

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
    2: "آلمانی",
    3: "اسپانیایی",
    4: "ایتالیایی",
    5: "فرانسوی"
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
    4: "مادرزادی"
};

const wages = {
    0: "توافقی",
    1: "پایه (وزارت کار)",
    2: "4.5",
    3: "5",
    4: "5.5",
    5: "6",
    6: "6.5",
    7: "7",
    8: "7.5",
    9: "8",
    10: "8.5",
    11: "9",
    12: "9.5",
    13: "10",
    14: "10.5",
    15: "11",
    16: "11.5",
    17: "12",
    18: "12.5",
    19: "13",
    20: "13.5",
    21: "14",
    22: "14.5",
    23: "15",
    24: "15.5",
    25: "16",
    26: "16.5",
    27: "17",
    28: "17.5",
    29: "18",
    30: "18.5",
    31: "19",
    32: "19.5",
    33: "20"
};

(() => {
    "use strict";

    // Fetch all the forms we want to apply custom Bootstrap validation styles to
    const forms = document.querySelectorAll(".needs-validation");

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
                    saveFormData(form, form.dataset.tag);
                }
            },
            false
        );
    });
})();




/** ساخت رزومه کابر **/
(async () => {
    await generateResume();
})();

/******************** های موجود در فرم‌ها با استفاده از داده‌های برگشته از سمت سرورdropdown ساخت ********************/
(async () => {
    const provinces = await getProvinces();

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
        let selectedProvinceId = e.target.value;
        const cities = await getCities(selectedProvinceId);
        citySelectElement.innerHTML =
            "<option selected disabled>-- انتخاب کنید --</option>";
        cities.map((city) =>
            citySelectElement.insertAdjacentHTML(
                "beforeend",
                generateCitySelectOptionTemplate(city)
            )
        );
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

    /** حداقل حقوق توافقی dropdown به ازای هر مقدار حقوق برای option element ساخت **/
    for (let index in wages) {
        wageSelectElement.insertAdjacentHTML(
            "beforeend",
            generateWageSelectOptionTemplate(index, wages[index])
        );
    }

    /** دسته بندی‌های شغلی dropdown به ازای هر دسته بندی شغلی برای option element ساخت **/
    $('document').ready(function () {
        $('#major').combobox({
            height: "38px",
            width: "auto",
            url: "api/PublicApi/GetFindEducationalField",
            method: "get",
            valueField: 'title',
            textField: 'title',
        });
    })

    /** دسته بندی‌های شغلی dropdown به ازای هر دسته بندی شغلی برای option element ساخت **/
    $('document').ready(function () {
        $('#job-fields').tagbox({
            height: "38px",
            width: "auto",
            hasDownArrow: true,
            data: jobFields,
            valueField: 'id',
            textField: 'title',
            limitToList: true,
            tagStyler: function () {
                return 'float: right';
            }
        });
    })
    /******************************************************** پایان ********************************************************/
})()

async function generateResume() {
    userResume = await getUserResume();
    userCertificates = await getUserCertificates();

    /******************** Show User Information When Exists ********************/
    /** نمایش عنوان شغلی کابر **/
    if (userResume.information.jobTitle) {
        showInformation(jobTitleContainer, userResume.information.jobTitle);
    }

    /** نمایش وضعیت شغلی کابر **/
    if (userResume.information.occupationStatus) {
        let occupationStatus;
        switch (userResume.information.occupationStatus) {
            case "lookingForJob":
                occupationStatus = "جویای کار";
                break;
            case "lookingForBetterJob":
                occupationStatus = "به دنبال شغل بهتر";
                break;
            case "occupied":
                occupationStatus = "شاغل";
                break;
        }
        showInformation(occupationStatusContainer, occupationStatus);
    }

    /** نمایش آخرین محل کار کابر **/
    if (userResume.jobs.length !== 0) {
        showInformation(lastCompanyContainer, userResume.jobs.at(-1).company);
    }

    /** نمایش آخرین مدرک تحصیلی کابر **/
    if (userResume.education.length !== 0) {
        const lastEducation = userResume.education.at(-1);
        lastDegreeContainer
            .querySelector(".profile-item")
            .classList.remove("d-none");
        lastDegreeContainer
            .querySelector(".profile-add-item")
            .classList.add("d-none");
        lastDegreeContainer.querySelector(
            ".profile-item"
        ).innerText = `${translateGrade(lastEducation.grade)} ${lastEducation.major
        }`;
    }

    /** نمایش ایمیل کابر **/
    if (userResume.information.email) {
        showInformation(emailAddressContainer, userResume.information.email);
    }

    /** نمایش شماره تلفن کابر **/
    if (userResume.information.mobile) {
        showInformation(mobileNumberContainer, userResume.information.mobile);
    }

    /** نمایش استان محل سکونت کابر **/
    if (userResume.information.province) {
        const province = await getProvince(userResume.information.province);
        showInformation(provinceContainer, province.title);
    }

    /** نمایش شهر محل سکونت کابر **/
    if (userResume.information.city) {
        const province = userResume.information.province;
        const city = await getCity(province, userResume.information.city);
        showInformation(cityContainer, city?.title);
    }

    /** نمایش کدملی کابر **/
    if (userResume.information.nationalId) {
        showInformation(nationalIdContainer, userResume.information.nationalId);
    }

    /** نمایش سال تولد کابر **/
    if (userResume.information.birthYear) {
        showInformation(birthYearContainer, userResume.information.birthYear);
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
            militaryServiceContainer.classList.remove("d-none");
            militaryServiceContainer
                .querySelector("select")
                .setAttribute("required", true);
        } else {
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

    /** نمایش متن درباره من **/
    if (userResume.information.aboutMe) {
        showInformation(aboutMeContainer, userResume.information.aboutMe);
    }

    /** نمایش مهارت‌های کابر **/
    if (userResume.skills.length === 0) {
        skills = userResume.skills;
        skillsListElement.classList.add("d-none");
        noSkillsMessage.classList.remove("d-none");
    } else {
        skills = userResume.skills;
        skillsElement.value = JSON.stringify(userResume.skills);
        addSkillToList(skillsListFormElement, skills, 'type');
        addSkillToList(skillsListElement, skills);
    }

    /** نمایش سوابق شغلی کابر **/
    if (userResume.jobs.length === 0) {
        jobsListElement.classList.add("d-none");
        noJobsMessage.classList.remove("d-none");
    } else {
        jobsListElement.classList.remove("d-none");
        noJobsMessage.classList.add("d-none");
        jobsListElement.innerHTML = "";
        userResume.jobs.map(async (job) =>
            jobsListElement.insertAdjacentHTML(
                "beforeend",
                await generateJobRecordTemplate(job)
            )
        );
    }

    /** نمایش سوابق تحصیلی کابر **/
    if (userResume.education.length === 0) {
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

    /** نمایش دوره‌های آموزشی کابر **/
    if (userResume.courses?.length === 0) {
        coursesListElement.classList.add("d-none");
        noCoursesMessage.classList.remove("d-none");
    } else {
        coursesListElement.classList.remove("d-none");
        noCoursesMessage.classList.add("d-none");
        coursesListElement.innerHTML = "";
        userResume.courses?.map((course) =>
            coursesListElement.insertAdjacentHTML(
                "beforeend",
                generateCourseRecordTemplate(course)
            )
        );
    }

    /** نمایش مدارک و گواهینامه‌های کابر **/
    if (userCertificates?.length === 0) {
        certificatesListElement.classList.add("d-none");
        noCertificatesMessage.classList.remove("d-none");
    } else {
        certificatesListElement.classList.remove("d-none");
        noCertificatesMessage.classList.add("d-none");
        certificatesListElement.innerHTML = "";
        userCertificates?.map((certificate) =>
            certificatesListElement.insertAdjacentHTML(
                "beforeend",
                generateCertificateRecordTemplate(certificate)
            )
        );
    }

    /** نمایش زبان‌های کابر **/
    if (userResume.languages.length === 0) {
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

    /** نمایش محل‌های ترجیحی کابر جهت کار **/
    if (
        userResume.jobPreferences.preferredLocations &&
        userResume.jobPreferences.preferredLocations?.length !== 0
    ) {
        locationsListElement.classList.remove("d-none");
        preferredLocationsContainer
            .querySelector(".profile-add-item")
            .classList.add("d-none");
        locations = userResume.jobPreferences.preferredLocations;
        await addLocationToList(
            locationsListElement,
            locations
        );
    } else {
        locationsListElement.classList.add("d-none");
        preferredLocationsContainer
            .querySelector(".profile-add-item")
            .classList.remove("d-none");
    }

    /** نمایش زمینه شغلی ترجیحی کابر **/
    if (userResume.jobPreferences.jobFields && userResume.jobPreferences.jobFields?.length !== 0) {
        fieldsListElement.classList.remove("d-none");
        fieldsContainer
            .querySelector(".profile-add-item")
            .classList.add("d-none");
        fields = userResume.jobPreferences.jobFields;
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

    /** نمایش سطوح ارشدیت ترجیحی کابر **/
    if (
        userResume.jobPreferences.seniorityLevels &&
        userResume.jobPreferences.seniorityLevels?.length !== 0
    ) {
        seniorityLevelsContainer
            .querySelector(".profile-item")
            .classList.remove("d-none");
        seniorityLevelsContainer
            .querySelector(".profile-add-item")
            .classList.add("d-none");
        seniorityLevelsContainer.querySelector(".profile-item").innerHTML = "";
        userResume.jobPreferences.seniorityLevels.map((seniorityLevel) => {
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
        userResume.jobPreferences.contracts &&
        userResume.jobPreferences.contracts?.length !== 0
    ) {
        contractsContainer
            .querySelector(".profile-item")
            .classList.remove("d-none");
        contractsContainer
            .querySelector(".profile-add-item")
            .classList.add("d-none");
        contractsContainer.querySelector(".profile-item").innerHTML = "";
        userResume.jobPreferences.contracts.map((contract) => {
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

    /** نمایش حداقل حقوق توافقی ترجیحی کابر **/
    if (userResume.jobPreferences.minimumWage) {
        minimumWageContainer
            .querySelector(".profile-item")
            .classList.remove("d-none");
        minimumWageContainer
            .querySelector(".profile-add-item")
            .classList.add("d-none");
        minimumWageContainer.querySelector(".profile-item").innerHTML =
            userResume.jobPreferences.minimumWage !== "1"
                ? new Intl.NumberFormat("en-US").format(
                    +(getWage(userResume.jobPreferences.minimumWage)) * 1000000
                ) + " تومان"
                : getWage(userResume.jobPreferences.minimumWage);
    }

    /** در نسخه فعلی نمایش داده نشود **/
    /** نمایش مزایای شغلی ترجیحی کابر **/
    //if (userResume.jobPreferences.jobBenefits?.length !== 0) {
    //    const jobBenefitsElements =
    //        jobBenefitsContainer.querySelectorAll("label");
    //    Array.from(jobBenefitsElements).map(jobBenefitsElement => jobBenefitsElement.classList.remove("selected"));
    //    userResume.jobPreferences.jobBenefits?.map((jobBenefits) => {
    //        [...jobBenefitsElements].find((jobBenefitsElement) => {
    //            jobBenefitsElement.classList.contains(jobBenefits)
    //                ? jobBenefitsElement.classList.add("selected")
    //                : "";
    //        });
    //    });
    //}

    /** نمایش امتیاز رزومه کابر **/
    await rateResume(userResume);
}
/******************** End ********************/

/******************** Rate Resume based on Existing Information ********************/
async function rateResume(resume) {
    suggestions = [];
    improvementSuggestionsListElement.innerHTML = "";
    isHeroSectionIncomplete = Array.from(heroSectionProfileItems).some(
        (profileItem) => profileItem.innerHTML === ""
    );
    isPersonalInformationSectionIncomplete = Array.from(
        personalInformationSectionProfileItems
    ).some((profileItem) => profileItem.innerHTML === "");
    isAboutMeSectionIncomplete = aboutMeSectionProfileItem.innerHTML === "";
    isSkillsSectionIncomplete = resume?.skills.length < 3;
    isJobsSectionIncomplete = resume?.jobs.length === 0;
    isEducationSectionIncomplete =
        resume?.education.length === 0;
    isCoursesSectionIncomplete =
        resume?.courses.length === 0;
    isCertificatesSectionIncomplete =
        userCertificates?.length === 0;
    isLanguagesSectionIncomplete =
        resume?.languages.length === 0;
    isJobPreferencesSectionIncomplete = Array.from(
        jobPreferencesSectionProfileItems
    ).some((profileItem) => profileItem.innerHTML === "") && resume?.jobPreferences.preferredLocations.length === 0;

    if (isHeroSectionIncomplete) {
        suggestions.push({ score: 5, suggestion: "عنوان شغلی را کامل کنید." });
    }
    if (isPersonalInformationSectionIncomplete) {
        suggestions.push({ score: 15, suggestion: "تکمیل اطلاعات فردی" });
    }
    if (isAboutMeSectionIncomplete) {
        suggestions.push({ score: 5, suggestion: "متنی درباره خود بنویسید." });
    }
    if (isSkillsSectionIncomplete) {
        suggestions.push({ score: 15, suggestion: "حداقل 3 مهارت وارد کنید." });
    }
    if (isJobsSectionIncomplete) {
        suggestions.push({ score: 10, suggestion: "سوابق شغلی خود را وارد کنید." });
    }
    if (isEducationSectionIncomplete) {
        suggestions.push({
            score: 10,
            suggestion: "سوابق تحصیلی خود را وارد کنید.",
        });
    }
    if (isCoursesSectionIncomplete) {
        suggestions.push({
            score: 5,
            suggestion: "دوره‌های آموزشی خود را وارد کنید.",
        });
    }
    if (isCertificatesSectionIncomplete) {
        suggestions.push({
            score: 5,
            suggestion: "مدارک و گواهینامه دوره های خود را وارد کنید."
        });
    }
    if (isLanguagesSectionIncomplete) {
        suggestions.push({
            score: 10,
            suggestion: "زبان‌هایی که با آنها آشنا هستید را بنویسید.",
        });
    }
    if (isJobPreferencesSectionIncomplete) {
        suggestions.push({
            score: 20,
            suggestion: "ترجیحات شغلی خود را مشخص کنید.",
        });
    }

    suggestions
        .slice(0, 4)
        .map((suggestion) =>
            improvementSuggestionsListElement.insertAdjacentHTML(
                "beforeend",
                generateImprovementSuggestionItemTemplate(suggestion)
            )
        );

    const finalScore =
        100 -
        suggestions.reduce((accumulator, currentValue) => {
            return accumulator + currentValue.score;
        }, 0);
    completionPercentage.querySelector("p").textContent = finalScore;
    completionVisualStatus.style.height = `${finalScore}%`;
    finalScore === 100 &&
        improvementSuggestionsListElement.classList.add("d-none");
    finalScore === 100 && completionPercentageMessage.classList.add("d-none");

    if (finalScore < 50) {
        completionPercentage.style.color = "#e34435";
        completionVisualStatus.style.background = "#e34435";
        completionPercentageLine.style.background = "#e34435";
    } else if (finalScore >= 50 && finalScore < 75) {
        completionPercentage.style.color = "#f9a60b";
        completionVisualStatus.style.background = "#f9a60b";
        completionPercentageLine.style.background = "#f9a60b";
    } else {
        completionPercentage.style.color = "#1abc9c";
        completionVisualStatus.style.background = "#1abc9c";
        completionPercentageLine.style.background = "#1abc9c";
    }
}
/******************** End ********************/

/******************** Requests ********************/
/** دریافت لیست استان‌های ایران **/
async function getProvinces() {
    try {
        const response = await fetch("/api/city/GetProvinces");
        if (!response.ok) throw new Error("در هنگام بارگیری اطلاعات مشکلی رخ داد!");
        const provinces = await response.json();
        return provinces;
    } catch (error) {
        bootbox.alert({ message: error.message, locale: 'fa' })
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
        bootbox.alert({ message: error.message, locale: 'fa' })
    }
}

/** دریافت اطلاعات کاربر **/
async function getUserResume() {
    const resumeTemplate = {
        information: {},
        skills: [],
        jobs: [],
        education: [],
        courses: [],
        languages: [],
        jobPreferences: {
            preferredLocations: [],
            jobFields: [],
            seniorityLevels: [],
            contracts: [],
        },
    };
    try {
        const response = await fetch(getUrl);
        if (!response.ok) throw new Error("در هنگام بارگیری اطلاعات مشکلی رخ داد!");
        const user = await response.json();
        if (user) {
            if (user.userResume === "") {
                return resumeTemplate
            } else {
                return JSON.parse(user.userResume)
            }
        }
    } catch (error) {
        bootbox.alert({ message: error.message, locale: 'fa' })
    }
}

/** دریافت مدارک و گواهینامه‌های کاربر **/
//Modified
async function getUserCertificates() {
    /*    try {*/
    const response = await fetch("home/GetUserCertificates");
    if (!response.ok) throw new Error("در هنگام بارگیری اطلاعات مشکلی رخ داد!");
    const certificates = await response.json();
    if (certificates) {
        //if (certificates === "") {
        if (certificates.length == 0) {
            return []
        } else {
            //return JSON.parse(certificates)
            return certificates
        }
    }
    //} catch (error) {
    //    bootbox.alert({ message: error.message, locale: 'fa' })
    //}
}

/** دریافتی id دریافت اطلاعات مربوط به استان مورد نظر باتوجه به **/
async function getProvince(id) {
    try {
        const response = await fetch("/api/city/GetProvinces");
        if (!response.ok) throw new Error("در هنگام بارگیری اطلاعات مشکلی رخ داد!");
        const provinces = await response.json();
        const province = provinces.find((province) => +province.id === +id);
        if (!province) throw new Error("استان مورد نظر یافت نشد!");
        return province;
    } catch (error) {
        bootbox.alert({ message: error.message, locale: 'fa' })
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
        bootbox.alert({ message: error.message, locale: 'fa' })
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

/** دریافتی id دریافت اطلاعات مربوط به دسته بندی شغلی نظر باتوجه به **/
function getField(id) {
    return fields[id];
}



/** ارسال اطلاعات کاربر به سرور **/
async function postResume(resume) {
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
            /** ساخت رزومه کابر **/
            await generateResume();
        }
    } catch (error) {
        bootbox.alert({ message: error.message, locale: 'fa' })
    }
}

/** ارسال مدارک کاربر به سرور **/
async function postCertificate(certificate) {
    const formData = new FormData(certificate);
    try {
        const response = await fetch('/home/UploadAttachments', {
            method: 'POST',
            body: formData
        });
        const data = await response.json();
        if (!data.isSuccess) throw new Error(data.message);
        if (data.isSuccess) {
            bootbox.alert({ message: data.message, locale: 'fa' })
            /** ساخت رزومه کابر **/
            await generateResume();
        }
    } catch (error) {
        bootbox.alert({ message: error.message, locale: 'fa' })
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

/** اضافه کردن مهارت مورد نظر به لیست مهارت‌های حرفه‌ای **/
function addSkillToList(container, list, type = "list") {
    noSkillsMessage.classList.add("d-none");
    container.innerHTML = "";
    list.map((skill) =>
        container.insertAdjacentHTML(
            "beforeend",
            generateSkillCardTemplate(skill, type)
        )
    );
}

/** اضافه کردن شهر و استان مورد نظر در بخش ترجیحات شغلی به لیست مکان‌های ترجیحی **/
async function addLocationToList(container, list, type = "list") {
    container.innerHTML = "";
    locationsListElement.classList.remove("d-none");
    locationsListFormElement.parentElement.classList.remove("d-none");
    preferredLocationsContainer
        .querySelector(".profile-add-item")
        .classList.add("d-none");
    list.map(async (location) => {
        const province = await getProvince(location.province);
        const city = await getCity(location.province, location.city);
        container.insertAdjacentHTML(
            "beforeend",
            generatePreferredLocationCardTemplate(location.id, province, city, type)
        );
    });
}

/** اضافه کردن شهر و استان مورد نظر در بخش ترجیحات شغلی به لیست مکان‌های ترجیحی **/
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

//$('document').ready(function () {
//    $('#job-fields').tagbox({
//        onChange: function () {
//            console.log($('#job-fields').value())
//        }
//    })
//})

//const preferredProvinceElement =
//    document.getElementById("preferred-province");
//const preferredCityElement = document.getElementById("preferred-city");
//if (!preferredProvinceElement.value || !preferredCityElement.value) return;
//locations.push({
//    id: locations.length,
//    province: preferredProvinceElement.value,
//    city: preferredCityElement.value,
//    isNew: true
//});
//preferredProvinceElement.value = "";
//preferredCityElement.innerHTML =
//    "<option selected disabled>-- انتخاب کنید --</option>";
//addLocationToList(locationsListFormElement, locations, "form");
//addLocationToList(locationsListElement, locations);
//locationsElement.value = JSON.stringify(locations);

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
        case "hero":
            showExistingDataInHeroForm();
            return;
        case "personal-information":
            showExistingDataInPersonalInformationForm();
            return;
        case "about-me":
            showExistingDataInAboutMeForm();
            return;
        case "professional-skills":
            showExistingDataInProfessionalSkillsForm();
            return;
        case "job-preferences":
            showExistingDataInJobPreferencesForm();
            return;
    }
}

/** در صورت وجود hero نمایش اطلاعات مربوط به فرم **/
function showExistingDataInHeroForm() {
    /** نمایش عنوان شغلی کابر **/
    if (userResume.information.jobTitle) {
        jobTitleInputElement.value = userResume.information.jobTitle;
    }

    /** نمایش وضعیت شغلی کابر **/
    if (userResume.information.occupationStatus) {
        const selectedOccupationStatus = Array.from(
            occupationStatusInputElements
        ).find(
            (occupationStatusInputElement) =>
                occupationStatusInputElement.value === userResume.information.occupationStatus
        );
        selectedOccupationStatus.setAttribute("checked", true);
    }

    /** نمایش آخرین محل کار کابر **/
    if (userResume.jobs.length !== 0) {
        lastCompanyInput.textContent = userResume.jobs.at(-1).company;
    }

    /** نمایش آخرین مدرک تحصیلی کابر **/
    if (userResume.education.length !== 0) {
        const lastEducation = userResume.education.at(-1);
        lastDegreeInput.textContent = `${translateGrade(lastEducation.grade)} ${lastEducation.major
            }`;
    }
}

/** در صورت وجود personal information نمایش اطلاعات مربوط به فرم **/
async function showExistingDataInPersonalInformationForm() {
    /** نمایش ایمیل کابر **/
    if (userResume.information.email) {
        emailAddressInputElement.value = userResume.information.email;
    }

    /** نمایش شماره تلفن کابر **/
    if (userResume.information.mobile) {
        mobileNumberInputElement.value = userResume.information.mobile;
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

    /** نمایش سال تولد کابر **/
    if (userResume.information.birthYear) {
        birthYearInputElement.value = userResume.information.birthYear;
    }

    /** نمایش کدملی کابر **/
    if (userResume.information.nationalId) {
        nationalIdInputElement.value = userResume.information.nationalId;
    }

    /** نمایش وضعیت تاهل کابر **/
    if (userResume.information.marriageStatus) {
        const selectedMarriageStatus = Array.from(marriageStatusInputElements).find(
            (marriageStatusInputElement) =>
                marriageStatusInputElement.value === userResume.information.marriageStatus
        );
        selectedMarriageStatus.setAttribute("checked", true);
    }

    /** نمایش جنسیت کابر **/
    if (userResume.information.gender) {
        const selectedGender = Array.from(genderInputElements).find(
            (genderInputElement) =>
                genderInputElement.value === userResume.information.gender
        );
        selectedGender.setAttribute("checked", true);
    }

    /** نمایش وضعیت خدمت سربازی درصورتی که کاربر مرد باشد **/
    if (userResume.information.militaryService) {
        militaryInputElement.value = userResume.information.militaryService;
    }
}

/** در صورت وجود about me نمایش اطلاعات مربوط به فرم **/
function showExistingDataInAboutMeForm() {
    /** نمایش متن درباره من **/
    if (userResume.information.aboutMe) {
        aboutMeTextArea.value = userResume.information.aboutMe;
    } else {
        aboutMeTextArea.value = ""
    }
}

/** در صورت وجود professional skills نمایش اطلاعات مربوط به فرم **/
function showExistingDataInProfessionalSkillsForm() {
    /** نمایش مهارت‌های کابر **/
    if (userResume.skills.length === 0) {
        skillsListFormElement.innerHTML = "<p>هنوز مهارتی اضافه نکرده‌اید.</p>";
    } else {
        skillsCountElement.textContent = userResume.skills.length;
        addSkillToList(skillsListFormElement, skills, "form");
    }
}

/** در صورت وجود job preferences نمایش اطلاعات مربوط به فرم **/
async function showExistingDataInJobPreferencesForm() {
    /** نمایش محل‌های ترجیحی کابر جهت کار **/
    if (
        userResume.jobPreferences.preferredLocations &&
        userResume.jobPreferences.preferredLocations?.length !== 0
    ) {
        locationsListFormElement.parentElement.classList.remove("d-none");
        locations = userResume.jobPreferences.preferredLocations;
        await addLocationToList(
            locationsListFormElement,
            locations,
            "form"
        );
    } else {
        locationsListFormElement.parentElement.classList.add("d-none");
    }

    /** نمایش زمینه شغلی ترجیحی کابر **/
    if (userResume.jobPreferences.jobFields) {
        $('document').ready(function () {
            $('#job-fields').tagbox({
                value: userResume.jobPreferences.jobFields
            });
        })
    }

    /** نمایش سطوح ارشدیت ترجیحی کابر **/
    if (
        userResume.jobPreferences.seniorityLevels &&
        userResume.jobPreferences.seniorityLevels?.length !== 0
    ) {
        userResume.jobPreferences.seniorityLevels.map(async (seniorityLevel) => {
            const selectedSeniorityLevel = Array.from(
                seniorityLevelInputElements
            ).find(
                (seniorityLevelInputElement) =>
                    seniorityLevelInputElement.value === seniorityLevel
            );
            selectedSeniorityLevel.setAttribute("checked", true);
        });
    }

    /** نمایش قراردادهای قابل قبول ترجیحی کابر **/
    if (
        userResume.jobPreferences.contracts &&
        userResume.jobPreferences.contracts?.length !== 0
    ) {
        contractsContainer.querySelector(".profile-item").innerHTML = "";
        userResume.jobPreferences.contracts.map(async (contract) => {
            const selectedContract = Array.from(contractInputElements).find(
                (contractInputElement) => contractInputElement.value === contract
            );
            selectedContract.setAttribute("checked", true);
        });
    }

    /** نمایش حداقل حقوق توافقی ترجیحی کابر **/
    if (userResume.jobPreferences.minimumWage) {
        minimumWageInputElement.value = userResume.jobPreferences.minimumWage;
    }

    /** در نسخه فعلی نمایش داده نشود **/
    /** نمایش مزایای شغلی ترجیحی کابر **/
    //if (userResume.jobPreferences.jobBenefits?.length !== 0) {
    //    const jobBenefitsInputs = Array.from(
    //        jobBenefitsInputElements
    //    );
    //    jobBenefitsInputs.map(jobBenefitsInput => jobBenefitsInput.removeAttribute("checked"));
    //    userResume.jobPreferences.jobBenefits.map((jobBenefit) => {
    //        const selectedJobBenefit = Array.from(
    //            jobBenefitsInputElements
    //        ).find(
    //            (jobBenefitInputElement) =>
    //                jobBenefitInputElement.value === jobBenefit
    //        );
    //        selectedJobBenefit.setAttribute("checked", true);
    //    });
    //}
}

async function deleteSkillItem(id) {
    bootbox.confirm({
        message: "آیا از حذف مهارت مورد نظر اطمینان دارید؟",
        buttons: {
            confirm: {
                label: 'بله',
                className: 'btn-success'
            },
            cancel: {
                label: 'خیر',
                className: 'btn-danger'
            }
        },
        callback: async function (result) {
            if (result) {
                userResume.skills = userResume.skills.filter((skill) => {
                    return +skill.id !== +id
                });
                skillsCountElement.textContent = skills.length - 1;
                if (userResume.skills.length === 0) {
                    skillsListElement.classList.add("d-none");
                    skillsListFormElement.innerHTML = "<p>هنوز مهارتی اضافه نکرده‌اید.</p>";
                    noSkillsMessage.classList.remove("d-none");
                }
                await postResume(userResume);
            }
        }
    });
}

async function deleteJobItem(id) {
    bootbox.confirm({
        message: "آیا از حذف سابقه شغلی مورد نظر اطمینان دارید؟",
        buttons: {
            confirm: {
                label: 'بله',
                className: 'btn-success'
            },
            cancel: {
                label: 'خیر',
                className: 'btn-danger'
            }
        },
        callback: async function (result) {
            if (result) {
                userResume.jobs = userResume.jobs.filter((job) => {
                    return +job.id !== +id
                });
                await postResume(userResume);
            }
        }
    });
}

async function deleteEducationItem(id) {
    bootbox.confirm({
        message: "آیا از حذف سابقه تحصیلی مورد نظر اطمینان دارید؟",
        buttons: {
            confirm: {
                label: 'بله',
                className: 'btn-success'
            },
            cancel: {
                label: 'خیر',
                className: 'btn-danger'
            }
        },
        callback: async function (result) {
            if (result) {
                userResume.education = userResume.education.filter((education) => {
                    return +education.id !== +id
                });
                await postResume(userResume);
            }
        }
    });
}

async function deleteCourseItem(id) {
    bootbox.confirm({
        message: "آیا از حذف دوره مورد نظر اطمینان دارید؟",
        buttons: {
            confirm: {
                label: 'بله',
                className: 'btn-success'
            },
            cancel: {
                label: 'خیر',
                className: 'btn-danger'
            }
        },
        callback: async function (result) {
            if (result) {
                userResume.courses = userResume.courses.filter((course) => {
                    return +course.id !== +id
                });
                await postResume(userResume);
            }
        }
    });
}

async function deleteLanguageItem(id) {
    bootbox.confirm({
        message: "آیا از حذف زبان مورد نظر اطمینان دارید؟",
        buttons: {
            confirm: {
                label: 'بله',
                className: 'btn-success'
            },
            cancel: {
                label: 'خیر',
                className: 'btn-danger'
            }
        },
        callback: async function (result) {
            if (result) {
                userResume.languages = userResume.languages.filter((language) => {
                    return +language.id !== +id
                });
                await postResume(userResume);
            }
        }
    });
}

async function deleteLocationItem(id) {
    bootbox.confirm({
        message: "آیا از حذف مکان مورد نظر اطمینان دارید؟",
        buttons: {
            confirm: {
                label: 'بله',
                className: 'btn-success'
            },
            cancel: {
                label: 'خیر',
                className: 'btn-danger'
            }
        },
        callback: async function (result) {
            if (result) {
                userResume.jobPreferences.preferredLocations = userResume.jobPreferences.preferredLocations.filter((preferredLocation) => {
                    return +preferredLocation.id !== +id
                });
                await postResume(userResume);
            }
        }
    });
}


/** ترجمه مقدار مدرک تحصیلی به فارسی **/
function translateGrade(grade) {
    switch (grade) {
        case "diploma":
            return "دیپلم";
        case "bachelor":
            return "کارشناسی";
        case "master":
            return "کارشناسی ارشد";
        case "phd":
            return "دکترا و بالاتر";
        case "other":
            return "سایر";
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
/******************** End ********************/

/******************** Event Listeners ********************/

/* های مربوط به دکمه های مختلفevent listener */
window.addEventListener('click', function (e) {

    /* دریافت شده section نمایش فرم ویرایش اطلاعات function فراخوانی */
    if (e.target.classList.contains('edit-info') || e.target.classList.contains('add-record')) {
        const section = e.target.dataset.section;
        onEditForm(`${section}`, `${section}-form`, 'showEditForm');
    }

    /* دریافت شده section بستن فرم ویرایش اطلاعات function فراخوانی */
    if (e.target.classList.contains('cancel-form')) {
        const section = e.target.dataset.section;
        onEditForm(`${section}`, `${section}-form`, 'hideEditForm');
    }

    /* hero نمایش فرم ویرایش اطلاعات بخش function فراخوانی */
    if (e.target.classList.contains('add-hero-item')) {
        const section = e.target.dataset.section;
        onEditForm(`${section}`, `${section}-form`, 'showEditForm');
    }

    /* job-records نمایش فرم ویرایش اطلاعات بخش function فراخوانی */
    if (e.target.classList.contains('add-job-item')) {
        const section = e.target.dataset.section;
        const y = (document.getElementById(section).getBoundingClientRect().top + window.scrollY) - 200;
        window.scroll({
            top: y,
            behavior: 'smooth'
        });
    }

    /* job-records نمایش فرم ویرایش اطلاعات بخش function فراخوانی */
    if (e.target.classList.contains('add-education-item')) {
        const section = e.target.dataset.section;
        const y = (document.getElementById(section).getBoundingClientRect().top + window.scrollY) - 200;
        window.scroll({
            top: y,
            behavior: 'smooth'
        });
    }

    /* personal-information نمایش فرم ویرایش اطلاعات بخش function فراخوانی */
    if (e.target.classList.contains('add-information-item')) {
        const section = e.target.dataset.section;
        onEditForm(`${section}`, `${section}-form`, 'showEditForm');
    }

    /* job-preferences نمایش فرم ویرایش اطلاعات بخش function فراخوانی */
    if (e.target.classList.contains('add-preference-item')) {
        const section = e.target.dataset.section;
        onEditForm(`${section}`, `${section}-form`, 'showEditForm');
    }

    /* حذف آیتم مهارت function فراخوانی */
    if (e.target.classList.contains('delete-skill')) {
        const skillId = e.target.dataset.skillId;
        deleteSkillItem(skillId);
    }

    /* حذف آیتم سابقه شغلی function فراخوانی */
    if (e.target.classList.contains('delete-job-record')) {
        const jobRecordId = e.target.dataset.jobRecordId;
        deleteJobItem(jobRecordId);
    }

    /* حذف آیتم سابقه تحصیلی function فراخوانی */
    if (e.target.classList.contains('delete-education-record')) {
        const educationRecordId = e.target.dataset.educationRecordId;
        deleteEducationItem(educationRecordId);
    }

    /* حذف آیتم دوره آموزشی function فراخوانی */
    if (e.target.classList.contains('delete-course-record')) {
        const courseRecordId = e.target.dataset.courseRecordId;
        deleteCourseItem(courseRecordId);
    }

    /* حذف آیتم زبان function فراخوانی */
    if (e.target.classList.contains('delete-language')) {
        const languageId = e.target.dataset.languageId;
        deleteLanguageItem(languageId);
    }

    /* حذف آیتم شهر و استان ترجیحی function فراخوانی */
    if (e.target.classList.contains('delete-location')) {
        const locationId = e.target.dataset.locationId;
        deleteLocationItem(locationId);
    }
});

/** وضعیت خدمت سربازی در صورتی که جنسیت کاربر مرد باشد dropdown نمایش **/
genderRadioButtonsContainer.addEventListener("click", function (e) {
    const radioButtonElement = e.target.name === "gender";
    if (radioButtonElement && e.target.value === "male") {
        militaryServiceContainer.classList.remove("d-none");
        militaryServiceContainer
            .querySelector("select")
            .setAttribute("required", true);
    } else {
        militaryServiceContainer.classList.add("d-none");
        militaryServiceContainer
            .querySelector("select")
            .removeAttribute("required");
    }
});

/** اضافه کردن مهارت مورد نظر به لیست مهارت‌های حرفه‌ای **/
addSkillButton.addEventListener("click", function () {
    const skillTitle = document.getElementById("skill-title");
    if (!skillTitle.value) {
        bootbox.alert('عنوان مهارت نمیتواند خالی باشد.');
        return;
    }
    if (skills.length === 10) {
        bootbox.alert('تعداد مهارت‌ها نمیتواند بیشتر از 10 باشد.');
        return;
    }
    skills.push({ id: skills.length, title: skillTitle.value, isNew: true });
    skillsCountElement.textContent = skills.length;
    skillsListElement.classList.remove("d-none");
    addSkillToList(skillsListFormElement, skills, "form");
    addSkillToList(skillsListElement, skills);
    skillsElement.value = JSON.stringify(skills);
    skillTitle.value = "";
});

/** اضافه کردن شهر و استان مورد نظر در بخش ترجیحات شغلی به لیست مکان‌های ترجیحی **/
addLocationButton.addEventListener("click", async function (e) {
    const preferredProvinceElement =
        document.getElementById("preferred-province");
    const preferredCityElement = document.getElementById("preferred-city");
    if (!preferredProvinceElement.value || !preferredCityElement.value) return;
    locations.push({
        id: locations.length,
        province: preferredProvinceElement.value,
        city: preferredCityElement.value,
        isNew: true
    });
    preferredProvinceElement.value = "";
    preferredCityElement.innerHTML =
        "<option selected disabled>-- انتخاب کنید --</option>";
    addLocationToList(locationsListFormElement, locations, "form");
    addLocationToList(locationsListElement, locations);
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
        finishMonthElement.setAttribute("disabled", true);
        finishMonthElement.removeAttribute("required");
        finishYearElement.setAttribute("disabled", true);
        finishYearElement.removeAttribute("required");
    }
});

/** زمانی که کاربر دیگر در موسسه مذکور تحصیل نمیکند disabled سال پایان تحصیل از حالت input خارج کردن **/
isStillEducatingElement.addEventListener("click", function (e) {
    if (!e.target.checked) {
        graduationYear.removeAttribute("disabled");
        graduationYear.setAttribute("required", true);
    } else {
        graduationYear.setAttribute("disabled", true);
        graduationYear.removeAttribute("required");
    }
});

/** میکند hover نمایش دکمه حذف هنگامی که کاربر روی آیتم مهارت در لیست مهارت‌های حرفه‌ای **/
skillsListElement.addEventListener("mouseover", function (e) {
    showDeleteButton(e, "skill-card");
});

/** میکند hover نمایش دکمه حذف هنگامی که کاربر روی آیتم سابقه شغلی در لیست سوابق شغلی **/
jobsListElement.addEventListener("mouseover", function (e) {
    showDeleteButton(e, "job-card");
});

/** میکند hover نمایش دکمه حذف هنگامی که کاربر روی آیتم سابقه تحصیلی در لیست سوابق تحصیلی **/
educationListElement.addEventListener("mouseover", function (e) {
    showDeleteButton(e, "education-card");
});

/** میکند hover نمایش دکمه حذف هنگامی که کاربر روی آیتم دوره آموزشی در لیست دوره‌های آموزشی **/
coursesListElement.addEventListener("mouseover", function (e) {
    showDeleteButton(e, "course-card");
});

/** میکند hover نمایش دکمه حذف هنگامی که کاربر روی آیتم زبان در لیست زبان‌ها **/
languagesListElement.addEventListener("mouseover", function (e) {
    showDeleteButton(e, "language-card");
});

/** میکند hover نمایش دکمه حذف هنگامی که کاربر روی آیتم مکان‌های ترجیجی در لیست مکان‌های ترجیحی **/
locationsListElement.addEventListener("mouseover", function (e) {
    showDeleteButton(e, "location-card");
});

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
                <h6 class="mb-0 fw-bold">${record.jobTitle}</h6>
            <div class="d-flex flex-column flex-sm-row profile-item gap-2">
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
            <div class="bg-primary py-2 p-3">
                <p class="mb-0 text-white" style="font-size: 14px">${record.jobDescription}</p>
            </div>` : ""}
        </div>
      <button type="button" class="delete-job-record delete-button btn btn-secondary position-absolute d-none" style="left: 20px; top: 20px" data-job-record-id="${record.id}">x</button>
    </li>`;
}

/** آیتم‌های لیست سوابق تحصیلی html ساخت **/
function generateEducationRecordTemplate(record) {
    return `
    <li class="education-card list-group-item p-0 position-relative overflow-hidden d-flex flex-column flex-sm-row gap-3 justify-content-between align-items-center">
        <div class="w-100 pe-none">
            <div class="p-3">
                <h6 class="mb-0 fw-bold">${record.major}${record.grade
            ? `<span class="profile-item"> (${translateGrade(record.grade)})<span>`
            : ""
        }</h6>
            <div class="d-flex flex-column flex-sm-row profile-item gap-2">
              <p class="mb-0" style="font-size: 14px; color: rgb(85, 85, 85)">${record.institution}</p>
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
                <p class="mb-0">${record.educationStartYear}</p>
                ${record.stillEducating
            ? `<p class="mb-0">تا حالا</p>`
            : `
                <div class="d-flex align-items-center gap-1">
                <span>تا</span>
                <p class="mb-0">${record.graduationYear}</p>
                </div>
                `
        }
              </div>
            </div>
            </div>
            ${record.educationDescription ? `
            <div class="bg-primary py-2 p-3">
                <p class="mb-0 text-white" style="font-size: 14px">${record.educationDescription}</p>
            </div>` : ""}
        </div>
      <button type="button" class="delete-education-record delete-button btn btn-secondary position-absolute d-none" style="left: 20px; top: 20px" data-education-record-id="${record.id}">x</button>
    </li>`;
}

/** آیتم‌های لیست دوره‌های آموزشی html ساخت **/
function generateCourseRecordTemplate(record) {
    return `
    <li class="course-card list-group-item p-0 position-relative overflow-hidden d-flex flex-column flex-sm-row gap-3 justify-content-between align-items-center">
        <div class="w-100 pe-none">
            <div class="p-3">
                <h6 class="mb-0 fw-bold">${record.courseTitle}</h6>
            </div>
            ${record.courseDescription ? `
            <div class="bg-primary py-2 p-3">
                <p class="mb-0 text-white" style="font-size: 14px">${record.courseDescription}</p>
            </div>` : ""}
        </div>
      <button type="button" class="delete-course-record delete-button btn btn-secondary position-absolute d-none" style="left: 20px; top: 7px" data-course-record-id="${record.id}">x</button>
    </li>`;
}

/** آیتم‌های لیست مدارک و گواهینامه‌ها html ساخت **/
function generateCertificateRecordTemplate(certificate) {
    return `
    <li class="certificate-card list-group-item p-0 position-relative overflow-hidden d-flex flex-column flex-sm-row gap-3 justify-content-between align-items-center p-2" style="min-width: 250px">
        <div class="d-flex justify-content-between align-items-cneter w-100">
            <p class="my-1" style="font-size: 15px">${certificate.title}</p>
            <div>
                <a href="home/GetCertificateFile?certificateId=${certificate.id}" download="${certificate.title}" class="btn btn-primary" style="font-size: 14px !important; height: 35px; cursor: pointer">دانلود فایل</a>
            </div>
        </div>
    </li>`;
    //<button type="button" class="delete-certificate-record delete-button btn btn-danger" style="font-size: 14px;height: 35px">
    //    <a href="home/DeleteCertificate?certificateId=${certificate.id}">حذف مدرک</a>
    //</button>
}

/** آیتم‌های لیست زبان‌ها html ساخت **/
function generateLanguageCardTemplate(language) {
    const languageWithMastery = getLanguageAndMastery(
        +language.language,
        +language.mastery
    );
    return `
  <li class="language-card d-flex align-items-center list-group-item border rounded-2 py-1 p-3" style="flex: 1; min-width:200px; max-height: 58px">
    <button type="button" class="delete-language delete-button btn btn-secondary me-2 d-none" data-language-id="${language.id}">x</button>  
    <div class="w-100 d-flex justify-content-between align-items-center pe-none">
      <span class="fw-bold my-3">${languageWithMastery.at(0)}</span>
      <span class="text-primary" style="font-size: 14px">${languageWithMastery.at(
        -1
    )}</span>
    </div>
  </li>
  `;
}

/** آیتم‌های لیست سطوح ارشدیت در بخش ترجیحات شغلی html ساخت **/
function generateSeniorityLevelCardTemplate(seniorityLevel) {
    return `<li class="list-group-item d-flex justify-content-between align-items-center" style="color: inherit">${translateSeniorityLevel(
        seniorityLevel
    )}</li>`;
}

/** آیتم‌های لیست قراردادهای قابل قبول در بخش ترجیات شغلی html ساخت **/
function generateContractCardTemplate(contract) {
    return `<li class="list-group-item d-flex justify-content-between align-items-center" style="color: inherit">${translateContract(
        contract
    )}</li>`;
}

/** حداقل حقوق درخواستی در بخش ترجیحات شغلی dropdown مبالغ مختلف option هر html ساخت **/
function generateWageSelectOptionTemplate(index, wage) {
    return `
  <option value="${index}">${index !== "1"
            ? `${new Intl.NumberFormat("en-US").format(+wage * 1000000)} تومان`
            : `${getWage(index)}`
        }</option>
  `;
}

/** آیتم‌های لیست مهارت‌های حرفه‌ای html ساخت **/
function generateSkillCardTemplate(skill, type) {
    return `<li class="skill-card list-group-item d-flex justify-content-between align-items-center py-1"><span class="my-3">${skill.title}</span>
        <button type="button" class="delete-skill delete-button btn btn-secondary d-inline ${type !== "form" ? "d-none" : ""}" data-skill-id="${skill.id}">x</button>
    </li>`;
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
function generateSelectOptionTemplate(index, item) {
    return `
  <option value="${index}">${item}</option>
  `;
}

/** آیتم‌های لیست مکان‌های ترجیحی در بخش ترجیات شغلی html ساخت **/
function generatePreferredLocationCardTemplate(id, province, city, type) {
    return `<li class="location-card list-group-item p-3 py-1 d-flex justify-content-between align-items-center" style="color: inherit"><span class="my-2">${province.title} - ${city.title}</span>
    <button type="button" class="delete-location delete-button btn btn-secondary ${type !== "form" ? "d-none" : ""}" data-location-id="${id}">x</button>
    </li>`;
}


/** آیتم‌های لیست دسته بندی‌های شغلی در بخش ترجیات شغلی html ساخت **/
function generateFieldCardTemplate(id) {
    const field = jobFields.find(jobField => +jobField.id === +id);
    return `<li class="location-card list-group-item p-3 py-1 d-flex justify-content-between align-items-center" style="color: inherit"><span class="my-2">${field.title}</span></li>`;
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
/******************** End ********************/

/******************** Form Handling ********************/
/** جهت ارسال به سرور object ها در فرم مورد نظر و ذخیره آنها در قالبinput دریافت مقادیر **/
async function saveFormData(formElement, tag) {
    const formData = new FormData(formElement);
    const record = {};
    if (tag === "certificates") {
        record.title = certificateTitleInput.value,
            record.attachments = Array.from(certificateFileInput.files);
        await postCertificate(formElement);
    } else {
        if (
            tag &&
            tag !== "information" &&
            tag !== "jobPreferences" &&
            tag !== "skills"
        ) {
            if (!userResume[tag]) {
                userResume[tag] = []
            } else {
                record.id = userResume[tag].length;
            }
            for (var [key, value] of formData.entries()) {
                record[key] = value;
            }
            userResume[tag] = [...userResume[tag], record];
        } else if (tag === "skills") {
            const finalSkills = skills.map(skill => { return { ...skill, isNew: false } });
            skills = finalSkills;
            userResume[tag] = skills;
        } else {
            userResume.jobPreferences.seniorityLevels = [];
            userResume.jobPreferences.contracts = [];
            userResume.jobPreferences.jobFields = [];
            for (var [key, value] of formData.entries()) {
                if (
                    key === "seniorityLevels" ||
                    key === "contracts" ||
                    key === "jobFields"
                ) {
                    userResume.jobPreferences[key].push(value);
                } else if (key === "preferredLocations") {
                    const finalLocations = locations.map(location => { return { ...location, isNew: false } });
                    locations = finalLocations;
                    userResume.jobPreferences[key] = locations;
                } else {
                    userResume[tag][key] = value;
                }
            }
        }
        await postResume(userResume);
    }
    const informationElementId = formElement.id.slice(0, -5);
    const informationElement = document.getElementById(informationElementId);
    formElement.classList.remove("show-information");
    formElement.classList.add("hide-information");
    formElement.classList.remove("was-validated");
    informationElement.classList.remove("hide-information");
    informationElement.classList.add("show-information");
    graduationYear.setAttribute("disabled", true);
    graduationYear.removeAttribute("required");
    finishMonthElement.setAttribute("disabled", true);
    finishMonthElement.removeAttribute("required");
    finishYearElement.setAttribute("disabled", true);
    finishYearElement.removeAttribute("required");
    formElement.reset();

}

/** نمایش فرم ویرایش اطلاعات در بخش مورد نظر **/
function onEditForm(informationElementId, formElementId, type) {
    const informationElement = document.getElementById(informationElementId);
    const formElement = document.getElementById(formElementId);

    if (type === "showEditForm") {
        informationElement.classList.remove("show-information");
        informationElement.classList.add("hide-information");
        formElement.classList.remove("hide-information");
        formElement.classList.add("show-information");
        showExistingDataInForm(informationElementId)
    }
    if (type === "hideEditForm") {
        informationElement.scrollIntoView(true);
        window.scrollBy(0, -150);
        if (informationElementId === "professional-skills") {
            const finalSkills = skills.filter(skill => skill.isNew === false);
            skills = finalSkills;
            if (skills.length !== 0) {
                skillsCountElement.textContent = skills.length;
                addSkillToList(skillsListFormElement, skills, "form");
                addSkillToList(skillsListElement, skills);
                skillsElement.value = JSON.stringify(skills);
            }
        }
        if (informationElementId === "job-preferences") {
            const finalLocations = locations.filter(location => location.isNew === false);
            locations = finalLocations;
            addLocationToList(locationsListFormElement, locations, "form");
            addLocationToList(locationsListElement, locations);
            locationsElement.value = JSON.stringify(locations);
        }
        formElement.reset();
        formElement.classList.remove("was-validated");
        informationElement.classList.remove("hide-information");
        informationElement.classList.add("show-information");
        formElement.classList.remove("show-information");
        formElement.classList.add("hide-information");
    }
}
/******************** End ********************/