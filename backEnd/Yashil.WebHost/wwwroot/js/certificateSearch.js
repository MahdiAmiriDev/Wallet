const submitButtonContent = '<span class="text-white fw-600 fs-15">جستجوی گواهینامه</span><svg viewBox="0 0 24 24" width="20" heigth="20"><use href="../images/icons/general.svg#search" /></svg>'
const submitButtonLoading = '<span class="spinner-border spinner-border-sm text-light" role="status" aria-hidden="true" style="margin-top:3px"></span><span class="sr-only text-white fw-bold">درحال دریافت اطلاعات...</span>'
const errorContainerElement = document.getElementById('error-container');
const certificateFormElement = document.getElementById('certificate-form');
const certificateContainerElement = document.getElementById('certificate-container');
const nationalCode = certificateFormElement.querySelector('#nationalId');
const captchaInputElement = certificateFormElement.querySelector('#captcha');
const certificateModal = new bootstrap.Modal(document.getElementById('certificate-modal'), {
    keyboard: false
});


// Add event listener to input field and check input length
//پیام معتبر بودن و نا معتبر بودن رو اینجا بررسی می کند
document.getElementById('nationalId').addEventListener('input', function (e) {
    var value = e.target.value.replace(/[^0-9]/g, '');
    e.target.value = value;

    var resultDiv = document.getElementById('result');
    if (value.length === 10) {
        resultDiv.classList.remove('d-none');
        if (nationalCodeValidate(value)) {
            resultDiv.textContent = 'کد ملی معتبر است.';
            resultDiv.className = 'valid';
        } else {
            resultDiv.textContent = 'کد ملی نامعتبر است.';
            resultDiv.className = 'invalid';
        }
    } else {
        resultDiv.textContent = '';
        resultDiv.className = '';
    }
});


//متد برای جستجوی گواهی نامه های صادر شده (نمایش لیست گواهی نامه ها)
certificateFormElement.addEventListener('submit', function (e) {
    var resultDiv = document.getElementById('result');
    resultDiv.textContent = '';
    resultDiv.className = '';
    const container = document.getElementById('listContainer');
    e.preventDefault();
    try {
        errorContainerElement.textContent = "";
        if (nationalCode.value.length == 10 && captchaInputElement.value.length == 4) {
            certificateFormElement.querySelector('button[type="submit"]').setAttribute('disabled', true);
            certificateFormElement.querySelector('button[type="submit"]').innerHTML = submitButtonLoading;

            const certificateObject = Object.fromEntries(new FormData(e.target));
            certificateObject.NationalCode = nationalCode.value;

            const formData = new FormData();
            for (let key in certificateObject) {
                formData.append(key, certificateObject[key]);
            }

            fetch('/api/Certificate/GetCertificateListByNationalCode', {
                method: 'POST',
                body: formData
            })
                .then(response => response.json())
                .then(data => {
                    if (data.isSuccess) {
                        // اگر کد ملی درست باشد ولی  در لیست گواهی نامه ها نباشد
                        if (data.certificatesStatistic.length == 0) {
                            certificateFormElement.querySelector('button[type="submit"]').removeAttribute('disabled', true);
                            certificateFormElement.querySelector('button[type="submit"]').innerHTML = submitButtonContent;
                            container.innerHTML = ''
                            const listItem = document.createElement('div');
                            listItem.className = 'transparent-div-search-certificate p-4 my-3 rounded-3 row';

                            container.style.display = 'block';
                            container.className += ' transparent-div-search-certificate p-4 my-3 rounded-3 row';
                            container.innerHTML += data.message;
                        }
                        // اگر کد ملی درست باشد و در لیست گواهی نامه ها هم موجود باشد
                        else {
                            container.innerHTML = ''
                            data.certificatesStatistic.forEach(item => {
                                const listItem = document.createElement('div');
                                listItem.innerHTML = '';
                                listItem.className = 'transparent-div-search-certificate p-4 my-3 rounded-3 row';


                                Object.entries(item).forEach(([key, value]) => {
                                    //filter api parameters
                                    if (key !== 'fullName' && key !== 'id' && key !== 'nationalCode' && key !== 'courseType') {
                                        const column = document.createElement('div');
                                        column.className = 'col-6 col-sm-6 col-md-6 col-lg-2 pb-3 pb-lg-0';

                                        const label = document.createElement('p');
                                        label.className = 'fw-bold pb-1';
                                        label.style.borderBottom = '1px solid gray';
                                        label.textContent = getkeyForChangeLable(key);

                                        //taranslate to persian api parameters
                                        function getkeyForChangeLable(key) {
                                            switch (key) {
                                                case 'number':
                                                    return 'کد پیگیری';
                                                case 'issueDateInt':
                                                    return 'تاریخ صدور';
                                                case 'courseTitle':
                                                    return 'دوره آموزشی';
                                                case 'courseDuration':
                                                    return 'مدت دوره';
                                                case 'score':
                                                    return 'نمره';
                                                default:
                                                    return '';
                                            }
                                        }

                                        const valueSpan = document.createElement('span');
                                        valueSpan.className = 'text-dark detail-certificate-search';
                                        valueSpan.textContent = value;

                                        column.appendChild(label);
                                        column.appendChild(valueSpan);
                                        listItem.appendChild(column);
                                    }
                                });
                                var certificateNumber = item.number;

                                const buttonColumn = document.createElement('div');
                                buttonColumn.className = 'd-flex align-items-center col-12 col-sm-6 col-md-6 col-lg-2 pt-2 pt-md-0';

                                const button = document.createElement('button');
                                button.type = 'button';
                                button.className = 'button-86 fs-14';
                                button.id = 'showingCertificateButton';
                                button.role = 'button';
                                button.addEventListener('click', () => {
                                    showCertificateAndEncriptNationalcode(certificateNumber, e);
                                });
                                button.innerHTML = `
                            نمایش گواهینامه
                            <svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="icon icon-tabler icons-tabler-outline icon-tabler-eye ms-1">
                                <path stroke="none" d="M0 0h24v24H0z" fill="none"/>
                                <path d="M10 12a2 2 0 1 0 4 0a2 2 0 0 0 -4 0"/>
                                <path d="M21 12c-2.4 4 -5.4 6 -9 6c-3.6 0 -6.6 -2 -9 -6c2.4 -4 5.4 -6 9 -6c3.6 0 6.6 2 9 6"/>
                            </svg>
                        `;

                                buttonColumn.appendChild(button);
                                listItem.appendChild(buttonColumn);
                                container.appendChild(listItem);

                            });

                            certificateFormElement.querySelector('button[type="submit"]').removeAttribute('disabled', true);
                            certificateFormElement.querySelector('button[type="submit"]').innerHTML = submitButtonContent;
                            doRefreshCaptcha();

                        }

                    }
                    // اگر کد ملی درست نباشد 
                    else {

                        container.innerHTML = ''
                        container.style.display = 'block';
                        container.className += ' transparent-div-search-certificate p-4 my-3 rounded-3 row';
                        container.innerHTML += data.message;

                        const listItem = document.createElement('div');
                        listItem.className = 'transparent-div-search-certificate p-4 my-3 rounded-3 row';

                        certificateFormElement.querySelector('button[type="submit"]').removeAttribute('disabled', true);
                        certificateFormElement.querySelector('button[type="submit"]').innerHTML = submitButtonContent;
                        doRefreshCaptcha();
                        captchaInputElement.value = '';
                    }
                });
        }
        // اگر اطلاعات فرم مثل کد ملی و کپچا اشتباه باشد
        else {
            errorContainerElement.textContent = 'کاربر گرامی لطفا اطلاعات را بررسی فرمایید.';
            certificateFormElement.querySelector('button[type="submit"]').removeAttribute('disabled', true);
            certificateFormElement.querySelector('button[type="submit"]').innerHTML = submitButtonContent;
            nationalCode.value = '';
            captchaInputElement.value = '';

        }
    } catch (e) {
        errorContainerElement.textContent = "";
        certificateFormElement.querySelector('button[type="submit"]').removeAttribute('disabled', true);
        certificateFormElement.querySelector('button[type="submit"]').innerHTML = submitButtonContent;
        nationalCode.value = '';
        captchaInputElement.value = '';
        doRefreshCaptcha();
        console.error(e);
    }
});



//وقتی روی دکمه نمایش گواهی نامه کلیک میشود rsakey ارسال می کند برای هر کد ملی و متد دریافت گواهی نامه کال میشود
function showCertificateAndEncriptNationalcode(certificateNumber, e) {
    fetch('/api/User/GetKeyForEncryption',
        {
            method: 'GET'
        }).then(response => response.json()).then(rsaKey => {
            getCertificate(certificateNumber, rsaKey, e);
        })
}


//متد برای نمایش گواهی نامه بر اساس کد پیگیری
const getCertificate = function (certificateNumber, rsaKey, e) {
    const certificateObject = Object.fromEntries(new FormData(e.target));
    certificateObject.CertificateNumber = certificateNumber;
    certificateObject.Key = rsaKey.entryKey;

    const formData = new FormData();
    for (let key in certificateObject) {
        formData.append(key, certificateObject[key]);
    }

    try {
        fetch('/api/certificate/SearchByCertificateNumberAsync',
            {
                method: 'POST',
                body: formData
            })
            .then(response => response.json())
            .then(data => {
                if (!data.isSuccess) {
                    certificateContainerElement.textContent = '';
                    errorContainerElement.textContent = data.message;
                }
                data.isSuccess && certificateModal.show();
                certificateContainerElement.innerHTML = certificateTemplate(data.certificatesStatistic[0]);
                certificateFormElement.querySelector('button[type="submit"]').removeAttribute('disabled');
                certificateFormElement.querySelector('button[type="submit"]').innerHTML = submitButtonContent;
                certificateFormElement.reset();
            });
    } catch (e) {
        console.error(e);
    }

}
//generate full screen modal
const certificateTemplate = (response) => {
    return `

                                                                                                                                                    <div class=" position-relative w-100 h-100 d-block"  >
                                                                                                                                                            <div id="print-preview">

                                                                                                                                <div class="d-flex justify-content-center">
                                                                                                                                            <img src="/images/certificate-search/certificate.webp" alt="Certificate" width="98%" />
                                                                                                </div>
                                                                                                        <div class="w-100 d-flex flex-column align-items-center position-absolute "
                                                                                                     style="top: 15%">
                                                                                                    <div style="max-width: 25%">
                                                                                                                <div class="d-flex justify-content-center mb-2 ">
                                                                                                            <img src="/images/iran.png" alt="Logo Iran" width="20%" class="print-image-2"/>
                                                                                                        </div>
                                                                                                        <div>
                                                                                                            <p class="text-center mb-0"
                                                                                                               style="font-size: 0.9rem;">
                                                                                                                <span class="d-block text-dark" style="font-size:1.05vw !important">دارنده گواهی‌نامه نظام مديريت</span>
                                                                                                                <span class="d-block fw-bold text-dark" style="font-size: 1.1vw !important">ISO 9001:2008</span>
                                                                                                                <span class="d-block text-dark" style="font-size:1.05vw !important">
                                                                                                                    در حوزه‌ی ارائه خدمات آموزشی حضوری و مجازی انفورماتيک، مديريت
                                                                                                                    و زبان
                                                                                                                </span>
                                                                                                            </p>
                                                                                                        </div>
                                                                                                    </div>
                                                                                                            <certificate-detail-component certificateDetailInformation='${JSON.stringify(response)}'></certificate-detail-component>
                                                                                                </div>
                                                                                            </div>
                                                                                                    </div>
                                                                                                    </div>
                                                                                            `
}









