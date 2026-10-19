export default class GeneralVariables {
    isPersonalInformationSectionIncomplete;
    isJobsSectionIncomplete;
    isEducationSectionIncomplete;
    isCertificatesSectionIncomplete;
    isLanguagesSectionIncomplete;
    isJobPreferencesSectionIncomplete;

    currentStep = 0;
    suggestions = [];
    userResume;
    getUrl = "/api/user/GetUserResume";

    // Score Component Elements
    forms = document.querySelectorAll(".needs-validation");
    resumeSteps = document.querySelectorAll(".resume-step");
    stepsContainer = document.getElementById('steps-container');
    stepsContainerElement = document.getElementById('steps-wrapper');
    resumeSidebar = document.getElementById('resume-sidebar');
    personalInformationSectionProfileItems = document.getElementById("personal-information").querySelectorAll(".profile-item");
    jobPreferencesSectionProfileItems = document.getElementById("job-preferences").querySelectorAll(".profile-item");
    completionPercentage = document.querySelector(".completion-percentage");
    completionPercentageLine = document.querySelector(".completion-percentage-line");
    completionVisualStatus = document.querySelector(".completion-visual-status");
    completionPercentageMessage = document.querySelector(".completion-percentage-message");
    improvementSuggestionsListElement = document.getElementById("improvement-suggestions-list");
    continueButtonElement = document.getElementById('continue-button');

    constructor(Swal) {
        this.Swal = Swal;
        this.continueButtonElement.addEventListener('click', () => this.goNextStep())
    }

    /** جهت ارسال به سرور object ها در فرم مورد نظر و ذخیره آنها در قالبinput دریافت مقادیر **/
    async saveFormData(formElement, tag, shouldHide) {
        const formData = new FormData(formElement);
        if (tag === "certificates" || tag === "languages" || tag === "education") {
            await postCertificate(formElement, tag, shouldHide);
        } else {
            if (
                tag &&
                tag !== "information" &&
                tag !== "jobPreferences"
            ) {
                if (!this.userResume[tag]) {
                    this.userResume[tag] = []
                } else {
                    record.id = this.userResume[tag]?.length;
                }
                for (let [key, value] of formData.entries()) {
                    record[key] = value;
                }
                if (tag === 'jobs') {
                    this.userResume.careerProfile[tag] = [...this.userResume.careerProfile[tag], record];
                } else {
                    this.userResume[tag] = [...this.userResume[tag], record];
                }
            } else if (tag === "jobPreferences") {
                this.userResume.careerProfile.jobPreferences.seniorityLevels = [];
                this.userResume.careerProfile.jobPreferences.contracts = [];
                this.userResume.careerProfile.jobPreferences.jobFields = [];
                for (let [key, value] of formData.entries()) {
                    if (
                        key === "seniorityLevels" ||
                        key === "contracts" ||
                        key === "jobFields"
                    ) {
                        this.userResume.careerProfile.jobPreferences[key].push(value);
                    } else if (key === "preferredLocations") {
                        const finalLocations = locations.map(location => { return { ...location, isNew: false } });
                        locations = finalLocations;
                        this.userResume.careerProfile.jobPreferences[key] = locations;
                    } else {
                        this.userResume.careerProfile[tag][key] = value;
                    }
                }
            } else {
                for (let [key, value] of formData.entries()) {
                    if (tag === "information") {
                        const nationalIdValidity = checkNationalId(nationalIdInputElement.value);
                        const guardianNationalIdValidity = checkNationalId(guardianNationalIdInputElement.value);
                        const Toast = this.createToast("top-end", 4000);
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
                            this.userResume[tag][key] = value;
                        }
                    } else {
                        this.userResume[tag][key] = value;
                    }
                }
            }
            await postResume(this.userResume, "post", shouldHide);

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

    /** دریافت اطلاعات کاربر **/
    async getUserResume() {
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
        try {
            const response = await fetch(this.getUrl);
            if (!response.ok) throw new Error("در هنگام بارگیری اطلاعات مشکلی رخ داد!");
            const user = await response.json();
            if (user) {
                if (user.userResume === "") {
                    this.userResume = resumeTemplate;
                } else {
                    this.userResume = JSON.parse(user.userResume);
                }

                /** نمایش امتیاز رزومه کابر **/
                this.rateResume(this.userResume);
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

    /** سیستم امتیازدهی رزومه **/
    rateResume() {
        this.suggestions = [];
        this.improvementSuggestionsListElement.innerHTML = "";
        this.isPersonalInformationSectionIncomplete = Array.from(
            this.personalInformationSectionProfileItems
        ).some((profileItem) => profileItem.classList.contains('show-information'));
        this.isEducationSectionIncomplete =
            this.userResume?.education?.length === 0;
        this.isCertificatesSectionIncomplete =
            this.userResume?.certificates?.length === 0;
        this.isLanguagesSectionIncomplete =
            this.userResume?.languages?.length === 0;
        this.isJobsSectionIncomplete = this.userResume?.jobs?.length === 0;
        this.isJobPreferencesSectionIncomplete = Array.from(
            this.jobPreferencesSectionProfileItems
        ).some((profileItem) => profileItem.innerHTML === "") && this.userResume?.careerProfile.jobPreferences.preferredLocations?.length === 0;

        if (this.isPersonalInformationSectionIncomplete) {
            this.suggestions.push({ score: 25, suggestion: "تکمیل اطلاعات فردی" });
        }
        if (this.isEducationSectionIncomplete) {
            this.suggestions.push({
                score: 15,
                suggestion: "سوابق تحصیلی خود را وارد کنید.",
            });
        }
        if (this.isCertificatesSectionIncomplete) {
            this.suggestions.push({
                score: 15,
                suggestion: "مدارک و گواهینامه دوره های خود را وارد کنید."
            });
        }
        if (this.isLanguagesSectionIncomplete) {
            this.suggestions.push({
                score: 10,
                suggestion: "زبان‌هایی که با آنها آشنا هستید را بنویسید.",
            });
        }
        if (this.isJobsSectionIncomplete) {
            this.suggestions.push({ score: 10, suggestion: "سوابق شغلی خود را وارد کنید." });
        }
        if (this.isJobPreferencesSectionIncomplete) {
            this.suggestions.push({
                score: 25,
                suggestion: "علاقه مندی‌شغلی خود را مشخص کنید.",
            });
        }

        this.suggestions
            .slice(0, 4)
            .map((suggestion) =>
                this.improvementSuggestionsListElement.insertAdjacentHTML(
                    "beforeend",
                    this.generateImprovementSuggestionItemTemplate(suggestion)
                )
            );

        const finalScore =
            100 -
            this.suggestions.reduce((accumulator, currentValue) => {
                return accumulator + currentValue.score;
            }, 0);
        this.completionPercentage.querySelector("p").textContent = finalScore;
        this.completionVisualStatus.style.height = `${finalScore}%`;
        finalScore === 100 &&
            improvementSuggestionsListElement.classList.add("d-none");
        finalScore === 100 && completionPercentageMessage.classList.add("d-none");

        if (finalScore < 50) {
            this.completionPercentage.style.color = "#e34435";
            this.completionVisualStatus.style.background = "#e34435";
            this.completionPercentageLine.style.background = "#e34435";
        } else if (finalScore >= 50 && finalScore < 75) {
            this.completionPercentage.style.color = "#f9a60b";
            this.completionVisualStatus.style.background = "#f9a60b";
            this.completionPercentageLine.style.background = "#f9a60b";
        } else {
            this.completionPercentage.style.color = "#1abc9c";
            this.completionVisualStatus.style.background = "#1abc9c";
            this.completionPercentageLine.style.background = "#1abc9c";
        }
    }

    /** دریافت مدارک و گواهینامه‌های کاربر **/
    async getUserCertificates(list, docTypeId) {
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
            const Toast = this.createToast("top-end", 4000)
            Toast.fire({
                icon: "error",
                title: `<h6 style="font-weight: 800">بروز خطا</h6>`,
                html: `<p style="margin-top: -10px">${error.message}</p>`,
            });
        }
    }

    /** نمایش فرم ویرایش اطلاعات در بخش مورد نظر **/
    onEditForm(informationElementId, formElementId, type) {
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
                formElement.classList.remove("was-validated");
                informationElement.classList.remove("hide-information");
                informationElement.classList.add("show-information");
                formElement.classList.remove("show-information");
                formElement.classList.add("hide-information");
        }
    }

    /** داده شده id نمایش اطلاعات موجود در فرم مورد نظر بر اساس **/
    showExistingDataInForm(id) {
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
    async showExistingDataInPersonalInformationForm() {
        /** نمایش کدملی کابر **/
        if (this.userResume.information.nationalId) {
            nationalIdInputElement.value = this.userResume.information.nationalId;
        }

        /** نمایش ایمیل کابر **/
        if (this.userResume.information.email) {
            emailAddressInputElement.value = this.userResume.information.email;
        }

        /** نمایش استان محل سکونت کابر **/
        if (this.userResume.information.province) {
            provinceInputElement.value = this.userResume.information.province;
            const cities = await getCities(this.userResume.information.province);
            cities.map((city) => {
                citySelectElement.insertAdjacentHTML(
                    "beforeend",
                    generateCitySelectOptionTemplate(city)
                );
            });
        }

        /** نمایش شهر محل سکونت کابر **/
        if (this.userResume.information.city) {
            citySelectElement.value = this.userResume.information.city;
        }

        /** نمایش شماره تلفن کابر **/
        if (this.userResume.information.mobile) {
            mobileNumberInputElement.value = this.userResume.information.mobile;
        }

        /** نمایش سال تولد کابر **/
        if (this.userResume.information.birthYear) {
            birthYearSelectElement.value = this.userResume.information.birthYear;
        }

        /** نمایش کدملی سرپرست کاربر **/
        if (this.userResume.information.guardianNationalId) {
            guardianNationalIdInputElement.value = this.userResume.information.guardianNationalId
        }

        /** نمایش شماره تلفن سرپرست کاربر **/
        if (this.userResume.information.guardianMobile) {
            guardianMobileNumberInputElement.value = this.userResume.information.guardianMobile
        }

        /** نمایش وضعیت جسمانی کابر **/
        if (this.userResume.information.physicalStatus) {
            const selectedPhysicalStatus = Array.from(physicalStatusInputElements).find(
                (physicalStatusInputElement) =>
                    physicalStatusInputElement.value === this.userResume.information.physicalStatus
            );
            selectedPhysicalStatus.checked = true;
        }

        /** نمایش نوع معلولیت کاربر **/
        if (this.userResume.information.disabilityType) {
            disabilityTypeInputElement.value = this.userResume.information.disabilityType;
        }

        /** نمایش وضعیت تاهل کابر **/
        if (this.userResume.information.marriageStatus) {
            const selectedMarriageStatus = Array.from(marriageStatusInputElements).find(
                (marriageStatusInputElement) =>
                    marriageStatusInputElement.value === this.userResume.information.marriageStatus
            );
            selectedMarriageStatus.checked = true;
        }

        /** نمایش تعداد فرزندان کاربر **/
        if (this.userResume.information.childrenCount) {
            childrenCountSelectElement.querySelector("select").value = this.userResume.information.childrenCount;
        }

        /** نمایش جنسیت کابر **/
        if (this.userResume.information.gender) {
            const selectedGender = Array.from(genderInputElements).find(
                (genderInputElement) =>
                    genderInputElement.value === this.userResume.information.gender
            );
            selectedGender.checked = true;
        }

        /** نمایش وضعیت خدمت سربازی درصورتی که کاربر مرد باشد **/
        if (this.userResume.information.militaryService) {
            militaryInputElement.value = this.userResume.information.militaryService;
        }
    }

    /** در صورت وجود job preferences نمایش اطلاعات مربوط به فرم **/
    async showExistingDataInJobPreferencesForm() {
        /** نمایش محل‌های ترجیحی کابر جهت کار **/
        if (
            this.userResume.careerProfile.jobPreferences?.preferredLocations &&
            this.userResume.careerProfile.jobPreferences?.preferredLocations?.length !== 0
        ) {
            locationsListFormElement.parentElement.classList.remove("d-none");
            locations = this.userResume.careerProfile.jobPreferences?.preferredLocations;
            await addLocationToList(
                locationsListFormElement,
                locations,
                "form"
            );
        } else {
            locationsListFormElement.parentElement.classList.add("d-none");
        }

        /** نمایش زمینه شغلی ترجیحی کابر **/
        if (this.userResume.careerProfile.jobPreferences.jobFields) {
            $('document').ready(function () {
                $('#job-fields').tagbox({
                    value: this.userResume.careerProfile.jobPreferences.jobFields
                });
            })
        }

        /** نمایش عنوان دقیق شغل درخواستی کاربر **/
        if (this.userResume.careerProfile.jobPreferences?.exactJobTitle) {
            exactJobTitleInputElement.value = this.userResume.careerProfile.jobPreferences?.exactJobTitle;
        }

        /** نمایش سطوح ارشدیت ترجیحی کابر **/
        if (
            this.userResume.careerProfile.jobPreferences?.seniorityLevels &&
            this.userResume.careerProfile.jobPreferences?.seniorityLevels?.length !== 0
        ) {
            this.userResume.careerProfile.jobPreferences?.seniorityLevels.map(async (seniorityLevel) => {
                const selectedSeniorityLevel = Array.from(
                    seniorityLevelInputElements
                ).find(
                    (seniorityLevelInputElement) =>
                        seniorityLevelInputElement.value === seniorityLevel
                );
                selectedSeniorityLevel.checked = true;
            });
        }

        /** نمایش قراردادهای قابل قبول ترجیحی کابر **/
        if (
            this.userResume.careerProfile.jobPreferences?.contracts &&
            this.userResume.careerProfile.jobPreferences?.contracts?.length !== 0
        ) {
            contractsContainer.querySelector(".profile-item").innerHTML = "";
            this.userResume.careerProfile.jobPreferences?.contracts.map(async (contract) => {
                const selectedContract = Array.from(contractInputElements).find(
                    (contractInputElement) => contractInputElement.value === contract
                );
                selectedContract.checked = true;
            });
        }
    }


    goNextStep() {
        const currentForm = this.forms[this.currentStep];
        const isCurrentFormOpen = currentForm.classList.contains('show-information');
        switch (isCurrentFormOpen) {
            case true:
                currentForm.classList.add("was-validated");
                if (currentForm.checkValidity()) {
                    GeneralFunctions.saveFormData(currentForm, currentForm.dataset.tag, true);
                }
                break;
            case false:
                this.nextPreviousStep(this.currentStep + 1);
                this.scrollStepsContainerLeft();
                break;
        }
    }

    // This function will figure out which tab to display
    nextPreviousStep(number) {
        // Hide the current tab:
        this.resumeSteps[this.currentStep].classList.add('d-none');
        // Set the current tab to given number:
        this.currentStep = +number;
        // Display the correct tab:
        showSection(this.currentStep);
        if (this.currentStep >= this.resumeSteps.length - 1) {
            this.continueButtonElement.classList.add('d-none');
            if (window.innerWidth < 992) {
                this.resumeSidebar.style.marginTop = "0";
            }
        } else {
            this.continueButtonElement.classList.remove('d-none');
            if (window.innerWidth < 992) {
                this.resumeSidebar.style.marginTop = "64px";
            }
        }
    }

    scrollStepsContainerLeft() {
    stepsContainerElement.scrollLeft -= (this.currentStep + 1) * 35
    if (window.innerWidth < 992) {
        stepsContainerElement.scrollLeft -= (this.currentStep + 1) * 40
    }
}

    /* براساس پارامترهای دلخواه toast ساخت */
    createToast(position, timer, nextStep = false, certificate = false, operation) {
        return this.Swal.mixin({
            toast: true,
            position,
            showConfirmButton: false,
            width: "30em",
            timer,
            timerProgressBar: true,
            didOpen: (toast) => {
                toast.onmouseenter = this.Swal.stopTimer;
                toast.onmouseleave = this.Swal.resumeTimer;
            },
            didDestroy: () => {
                if (nextStep && certificate === false) {
                    operation === "post" && this.currentStep < Array.from(resumeSteps).length - 1 ? nextPreviousStep(this.currentStep + 1) : ""
                }
                if (nextStep && certificate) {
                    nextPreviousStep(this.currentStep + 1);
                    scrollStepsContainerLeft();
                }
            }
        })
    }

    /** آیتم‌های لیست توصیه‌های مربوط به بهبود امتیاز رزومه در بخش امتیازدهی html ساخت **/
    generateImprovementSuggestionItemTemplate(suggestion) {
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
}
