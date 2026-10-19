import Swal from 'sweetalert2';

const printResumeButtonElement = document.getElementById('print-resume--button');
const resumeContainerElement = document.getElementById('resume');

printResumeButtonElement.addEventListener('click', function () {
    jQuery.print(resumeContainerElement);
})

///** ترجمه نوع دانشگاه به فارسی **/
//function translateInstitutionType(type) {
//    switch (type) {
//        case "1":
//            return "دانشگاه سراسری (دولتی)";
//        case "2":
//            return "دانشگاه غیر انتفاعی";
//        case "3":
//            return "دانشگاه آزاد";
//        case "4":
//            return "دانشگاه علمی – کاربردی";
//        case "5":
//            return "دانشگاه پیام‌ نور";
//    }
//}

///** ترجمه مقدار سطح ارشدیت در شغل به فارسی **/
//function translateSeniorityLevel(seniorityLevel) {
//    switch (seniorityLevel) {
//        case "junior":
//            return "تازه کار";
//        case "expert":
//            return "متخصص";
//        case "manager":
//            return "مدیر";
//        case "chief":
//            return "مدیر ارشد";
//    }
//};

///** ترجمه مقدار قرارداد قابل قبول به فارسی **/
//function translateContract(contract) {
//    switch (contract) {
//        case "full-time":
//            return "تمام وقت";
//        case "part-time":
//            return "پاره وقت";
//        case "remote":
//            return "دورکاری";
//        case "intern":
//            return "کارآموز";
//    }
//}

///** ترجمه مقدار قرارداد قابل قبول به فارسی **/
//function translateMonth(month) {
//    switch (month) {
//        case "1":
//            return "فروردین";
//        case "2":
//            return "اردیبهشت";
//        case "3":
//            return "خرداد";
//        case "4":
//            return "تیر";
//        case "5":
//            return "مرداد";
//        case "6":
//            return "شهریور";
//        case "7":
//            return "مهر";
//        case "8":
//            return "آبان";
//        case "9":
//            return "آذر";
//        case "10":
//            return "دی";
//        case "11":
//            return "بهمن";
//        case "12":
//            return "اسفند";
//    }
//}

//const months = {
//    1: "فروردین",
//    2: "اردیبهشت",
//    3: "خرداد",
//    4: "تیر",
//    5: "مرداد",
//    6: "شهریور",
//    7: "مهر",
//    8: "آبان",
//    9: "آذر",
//    10: "دی",
//    11: "بهمن",
//    12: "اسفند"
//};



class Configuration {
    getUrl = "/api/AdditionalUserProp/GetUserResume";
    userResume;
    constructor(personalInformation, educationRecordsInformation, languageRecordsInformation, jobRecordsInformation, certificateRecordsInformation) {
        this.personalInformation = personalInformation;
        this.educationRecordsInformation = educationRecordsInformation;
        this.languageRecordsInformation = languageRecordsInformation;
        this.jobRecordsInformation = jobRecordsInformation;
        this.certificateRecordsInformation = certificateRecordsInformation;
        this.resumeContainerElement = document.getElementById('resume');
        this.loaderContainerElement = document.getElementById('loader-container');
    }

    /** دریافت اطلاعات کاربر **/
    async getUserResume() {
        try {
            this.showSpinner();
            const response = await fetch(this.getUrl);
            if (!response.ok) throw new Error("در هنگام بارگیری اطلاعات مشکلی رخ داد!");
            const user = await response.json();
            if (user) {
                if (user.userResume === "") {
                    throw Error('رزومه ناقص')
                } else {
                    this.userResume = JSON.parse(user.userResume);
                    // To be removed...
                    if (!this.userResume.careerProfile) {
                        this.userResume = {
                            ...this.userResume, careerProfile: {
                                careerStatus: this.userResume.careerStatus,
                                jobs: this.userResume.jobs,
                                jobPreferences: this.userResume.jobPreferences
                            }
                        };
                        delete this.userResume.jobs;
                        delete this.userResume.jobPreferences;
                        delete this.userResume.careerStatus;
                    }
                    this.bindResumeToDescendants(this.userResume);
                }
            }
        } catch (error) {
            const Toast = this.createToast("top-end", 4000)
            Toast.fire({
                icon: "error",
                title: `<h6 style="font-weight: 800">بروز خطا</h6>`,
                html: `<p style="margin-top: -10px">${error.message}</p>`,
            });
        }
    }

    createToast(position, timer) {
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
            }
        })
    }

    showSpinner() {
        document.documentElement.classList.add('overflow-hidden');
    }

    hideSpinner() {
        document.documentElement.classList.remove('overflow-hidden');
        this.resumeContainerElement.classList.remove('d-none');
        this.loaderContainerElement.classList.add('d-none');
    }

    bindResumeToDescendants(resume) {
        this.personalInformation?.init(resume);
        this.educationRecordsInformation?.init(resume.education);
        this.languageRecordsInformation?.init(resume.languages);
        this.jobRecordsInformation?.init(resume.careerProfile.jobs);
        this.certificateRecordsInformation?.init(resume.certificates);
    }
}

class PersonalInformation extends Configuration {
    militaryService = {
        1: "پایان خدمت",
        2: "معافیت دائم",
        3: "معافیت تحصیلی",
        4: "درحال انجام",
        5: "مشمول"
    };
    resume = null;
    jobTitle;
    provinces = [];
    province;
    city;
    email;
    mobile;
    gender;
    birthYear;
    militaryServiceStatus;
    marriageStatus;
    childCount;

    constructor() {
        super();
        this.jobTitleContainer = document.querySelector('.job-title--container');
        this.addressInformation = document.getElementById('address-information');
        this.emailInformation = document.getElementById('email-information');
        this.mobileInformation = document.getElementById('mobile-information');
        this.genderInformation = document.getElementById('gender-information');
        this.birthYearInformation = document.getElementById('birth-year--information');
        this.militaryServiceInformation = document.getElementById('military-service--information');
        this.marriageStatusInformation = document.getElementById('marriage-status--information');
        this.childCountInformation = document.getElementById('child-count--information');
        this.careerStatusInformation = document.getElementById('career-status--information');
    }

    async init(resume) {
        this.resume = resume;
        await this.getProvinces();
        await this.getProvince(this.resume.information.province);
        await this.getCity(this.resume.information.province, this.resume.information.city);
        this.jobTitle = this.resume.careerProfile.jobPreferences.exactJobTitle;
        this.email = this.resume.information.email;
        this.mobile = this.resume.information.mobile;
        this.gender = this.resume.information.gender;
        this.birthYear = this.resume.information.birthYear;
        this.militaryServiceStatus = this.resume.information.militaryService;
        this.marriageStatus = this.resume.information.marriageStatus;
        this.childCount = this.resume.information.childrenCount;
        this.careerStatus = this.resume.careerProfile.careerStatus;
        this.renderInformation();
    }

    /** دریافت لیست استان‌های ایران **/
    async getProvinces() {
        try {
            const response = await fetch("/api/city/GetProvinces");
            if (!response.ok) throw new Error("در هنگام بارگیری اطلاعات مشکلی رخ داد!");
            const data = await response.json();
            this.provinces = data;
        } catch (error) {
            const Toast = this.createToast("top-end", 4000)
            Toast.fire({
                icon: "error",
                title: `<h6 style="font-weight: 800">بروز خطا</h6>`,
                html: `<p style="margin-top: -10px">${error.message}</p>`,
            });
        }
    }

    /** دریافتی id دریافت اطلاعات مربوط به استان مورد نظر باتوجه به **/
    getProvince(id) {
        try {
            const province = this.provinces.find((province) => +province.id === +id);
            if (!province) throw new Error("استان مورد نظر یافت نشد!");
            this.province = province;
        } catch (error) {
            const Toast = this.createToast("top-end", 4000)
            Toast.fire({
                icon: "error",
                title: `<h6 style="font-weight: 800">بروز خطا</h6>`,
                html: `<p style="margin-top: -10px">${error.message}</p>`,
            });
        }
    }

    /** دریافتی id دریافت اطلاعات مربوط به شهر مورد نظر باتوجه به **/
    async getCity(provinceId, id) {
        try {
            const response = await fetch(`/api/city/GetCitiesByProvinceId?provinceId=${provinceId}`);
            if (!response.ok) throw new Error("در هنگام بارگیری اطلاعات مشکلی رخ داد!");
            const cities = await response.json();
            const city = cities.find((city) => +city.id === +id);
            if (!city) throw new Error("شهر مورد نظر یافت نشد!");
            this.city = city;
        } catch (error) {
            const Toast = this.createToast("top-end", 4000)
            Toast.fire({
                icon: "error",
                title: `<h6 style="font-weight: 800">بروز خطا</h6>`,
                html: `<p style="margin-top: -10px">${error.message}</p>`,
            });
        } finally {
            this.hideSpinner()
        }
    }

    renderInformation() {
        this.jobTitleContainer.textContent = this.jobTitle;
        this.addressInformation.textContent = `استان ${this.province.title}، ${this.city.title}`;
        this.email !== "" ? this.emailInformation.textContent = this.email : this.emailInformation.parentElement.classList.add('d-none');
        this.mobileInformation.textContent = this.mobile;
        this.genderInformation.textContent = this.gender === "male" ? "مرد" : "زن";
        this.birthYearInformation.textContent = this.birthYear;
        this.militaryServiceInformation.textContent = this.militaryService[this.militaryServiceStatus];
        this.marriageStatusInformation.textContent = this.marriageStatus === "bachelor" ? "مجرد" : "متاهل";
        this.childCountInformation.textContent = this.childCount ? this.childCount : this.childCountInformation.parentElement.classList.add('d-none');
        this.careerStatusInformation.textContent = this.careerStatus === "lookingForBetter" ? "مشغول به کار هستم و به دنبال شرایط بهتر کاری میگردم" : "جویای کار هستم"
    }
}

class EducationRecordsInformation extends Configuration {
    education = [];

    constructor() {
        super();
        this.educationRecordsSectionElement = document.getElementById('education-records--section');
        this.educationRecordsContainerElement = document.querySelector('.education-records');
    }

    async init(education) {
        this.education = education;
        this.renderInformation();
    }

    /** ترجمه مقدار مدرک تحصیلی به فارسی **/
    translateGrade(grade) {
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

    generateTemplate(record) {
        return `
            <li class="education-record d-flex flex-column pb-3">
                <h3 class="education-record--title">${this.translateGrade(record.grade)} ${record.major}</h3>
                <div class="d-flex flex-column flex-sm-row align-items-start align-items-sm-center gap-0 gap-sm-2">
                    ${record.institution ?
                `<p class="mb-0">${record.institution}</p> -` : ''}
                    <div class="d-flex gap-2">
                        <p class="mb-0">از ${record.educationStartYear}</p>
                        <p class="mb-0">تا ${record.graduationYear ? record.graduationYear : "حالا"}</p>
                    </div>
                    -
                    <p class="mb-0">معدل ${record.average}</p>
                </div>
                ${record.educationDescription ? `
                    <div class="education-description mt-1 p-2 ps-3">
                        ${record.educationDescription}
                    </div>
                `: ""}
            </li>
        `
    }

    renderInformation() {
        if (!this.education || this.education?.length === 0) {
            this.educationRecordsSectionElement.classList.add('d-none');
            return
        }
        this.education.map(record => {
            this.educationRecordsContainerElement.insertAdjacentHTML('beforeend', this.generateTemplate(record))
        })
    }
}

class LanguageRecordsInformation extends Configuration {
    languages = [];
    languagesTranslation = {
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

    masteryLevels = {
        1: "مقدماتی",
        2: "متوسط",
        3: "پیشرفته",
        4: "زبان مادری"
    };


    constructor() {
        super();
        this.languageRecordsSectionElement = document.getElementById('language-records--section');
        this.languageRecordsContainerElement = document.querySelector('.language-records')
    }

    async init(languages) {
        this.languages = languages;
        this.renderInformation();
    }

    getLanguageAndMastery(id, level) {
        return [this.languagesTranslation[id], this.masteryLevels[level]];
    }

    generateTemplate(record) {
        const languageLevelRate = new Array(4).fill(0);
        const language = this.getLanguageAndMastery(record.language, record.mastery);

        return `
            <li class="language-record col-12 d-flex align-items-center justify-content-between pb-3">
                <h3 class="language-record--title mb-0">${language[0]}</h3>
                <div class="d-flex flex-column align-items-center">
                    <p class="mb-0">${language[1]}</p>
                    <div class="language-level--rate d-flex align-items-center gap-1">
                        ${languageLevelRate.map((_, index) => index + 1 <= record.mastery ? '<div class="level-rate--box rate-box--active"></div>' : '<div class="level-rate--box"></div>').join('')}
                    </div>
                </div>
            </li>
        `
    }

    renderInformation() {
        if (!this.languages || this.languages.length === 0) {
            this.languageRecordsSectionElement.classList.add('d-none');
            return
        }
        this.languages.map(record => {
            this.languageRecordsContainerElement.insertAdjacentHTML('beforeend', this.generateTemplate(record))
        })
    }
}

class JobRecordsInformation extends Configuration {
    jobs = [];

    constructor() {
        super();
        this.jobRecordsSectionElement = document.getElementById('job-records--section');
        this.jobRecordsContainerElement = document.querySelector('.job-records')
    }

    async init(jobs) {
        this.jobs = jobs;
        this.renderInformation();
    }

    generateTemplate(record) {
        return `
           <li class="job-record d-flex flex-column pb-3 pt-2">
                <div class="d-flex flex-column flex-sm-row align-items-start align-items-sm-center justify-content-between mb-2 mb-sm-0">
                    <h3 class="job-record--title d-flex align-items-center gap-1 mb-0"><span style="font-size: inherit !important; color: inherit">${record.company}</span> - <span>${record.jobTitle}</span></h3>
                    <div class="d-flex align-items-center gap-2">
                        <div>از <span>${record.startYear}</span> تا <span>${record.finishYear ? record.finishYear : "حالا"}</span></div>
                    </div>
                </div>
                ${record.jobDescription ? `
                    <div class="job-description p-2 ps-3">
                        ${record.jobDescription}
                    </div>
                ` : ''}
            </li>
        `
    }

    renderInformation() {
        if (!this.jobs || this.jobs?.length === 0) {
            this.jobRecordsSectionElement.classList.add('d-none');
            return
        }
        this.jobs.map(record => {
            this.jobRecordsContainerElement.insertAdjacentHTML('beforeend', this.generateTemplate(record))
        })
    }
}

class CertificatesInformation extends Configuration {
    certificates = [];

    constructor() {
        super();
        this.certificateRecordsSectionElement = document.getElementById('certificate-records--section');
        this.certificateRecordsContainerElement = document.querySelector('.certificate-records')
    }

    async init(certificates) {
        this.certificates = certificates;
        this.renderInformation();
    }

    generateTemplate(record) {
        return `
           <li class="certificate-record d-flex flex-column pb-3">
<div class="d-flex flex-column flex-sm-row align-items-start align-items-sm-center justify-content-between mb-2 mb-sm-0">
    <h3 class="job-record--title d-flex align-items-center gap-1 mb-0"><span style="font-size: inherit !important; color: inherit">${record.title}</span></h3>
    <div class="d-flex align-items-center gap-2">
        <div>${record.courseLevel}</div>
    </div>
</div>
                <div class="d-flex align-items-center gap-2">
                    ${record.courseLocation}
                </div>
            </li>
        `
    }

    renderInformation() {
        if (!this.certificates || this.certificates?.length === 0) {
            this.certificateRecordsSectionElement.classList.add('d-none');
            return
        }
        this.certificates.map(record => {
            this.certificateRecordsContainerElement.insertAdjacentHTML('beforeend', this.generateTemplate(record))
        })
    }
}

const personalInformation = new PersonalInformation();
const educationRecordsInformation = new EducationRecordsInformation();
const languageRecordsInformation = new LanguageRecordsInformation();
const jobRecordsInformation = new JobRecordsInformation();
const certificateRecordsInformation = new CertificatesInformation();
const showResume = new Configuration(personalInformation, educationRecordsInformation, languageRecordsInformation, jobRecordsInformation, certificateRecordsInformation);
showResume.getUserResume();