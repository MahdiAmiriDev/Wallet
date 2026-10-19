const loginFormElement = document.getElementById('loginForm');
const submitButton = loginFormElement.querySelector('#loginButton');
const spinnerElement = loginFormElement.querySelector('.spinner-border');

loginFormElement.addEventListener('submit', function (e) {
    e.preventDefault();
    spinnerElement.classList.remove('d-none');
    submitButton.setAttribute('disabled', true);
    let mobileNumber = $("#mobileNumber").val();
    mobileNumber = mobileNumber.toEnglishDigit();
    if (!mobileNumberValidator(mobileNumber)) {
        spinnerElement.classList.add('d-none');
        submitButton.removeAttribute('disabled');
        bootbox.alert('لطفا شماره موبایل معتبر وارد نمایید');
        return false;
    }
    if ($("#loginForm").valid()) {
        fetch('/api/user/SendVerifyCode/' + mobileNumber,
            {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json; charset=utf-8'
                },
            })
            .then(response => response.json())
            .then(data => {
                if (data.isSuccess === true) {
                    const urlParams = new URLSearchParams(window.location.search);
                    const returnUlr = urlParams.get('returnUrl');
                    if (returnUlr) {
                        $('#frmConfirm').attr('action', data.returnUrl + "&returnUrl=" + returnUlr);
                        $('#frmConfirm').submit();
                    } else {
                        $('#frmConfirm').attr('action', data.returnUrl);
                        $('#frmConfirm').submit();
                    }
                }
                else if (data.isSuccess === false || data.isSuccess == null) {
                    spinnerElement.classList.add('d-none');
                    submitButton.removeAttribute('disabled');
                    bootbox.alert('لطفا مجددا تلاش کنید.');
                }
                else {
                    spinnerElement.classList.add('d-none');
                    submitButton.removeAttribute('disabled');
                    bootbox.alert('لطفا مجددا تلاش کنید.');
                }
            })
            .catch(error => {
                spinnerElement.classList.add('d-none');
                submitButton.removeAttribute('disabled');
                bootbox.alert('لطفا مجددا تلاش کنید.');

            });
    } else {
        spinnerElement.classList.add('d-none');
        submitButton.removeAttribute('disabled');
        bootbox.alert('ورودی ها را چک نمایید.');
    }
})
/*
 اعتبار سنجی شماره موبایل
*/
function mobileNumberValidator(mobileNumber) {
    var mobileNumberExperssion = /09(1[0-9]|3[0-9]|2[0-9]|0[0-9]|)-?[0-9]{3}-?[0-9]{4}/;
    return mobileNumberExperssion.test(mobileNumber)

}
function loginByPassword(e) {
    $('#frmConfirm').attr('action', '/user/loginByPassword');
    $('#frmConfirm').submit();
}