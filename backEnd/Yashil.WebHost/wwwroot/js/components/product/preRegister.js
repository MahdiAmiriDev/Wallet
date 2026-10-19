
/* وقتی روی دکمه پیش ثبت نام کلیک می شود ، مدال نمایش داده می شود */

if (document.getElementById("preRegisterModal")) {
    const preRegisterModal = new bootstrap.Modal(document.getElementById("preRegisterModal"), {
        keyboard: false
    });
    const preRegisterButton = document.getElementById("preRegister");
    if (preRegisterButton) {
        preRegisterButton.addEventListener("click", function () {
            preRegisterModal.show();
        });
    }



    //canceling PreRegister for preRegister in internal product

    function cancelingPreRegister() {
        preRegisterModal.hide();
        $("#error-text-national-code").addClass("d-none");
        $("#national_code").css("border-color", "rgba(255 , 0 , 0 , 0)");

    }



    //successPreRegister for preRegister in internal product

    function preRegister(productSalesPlanningId) {
        var nationalCode = $("#national_code").val();
        if (nationalCodeValidate(nationalCode)) {
            $("#error-text-national-code").addClass("d-none");
            $("#national_code").css("border-color", "var(--dark-primary-yellow-color)");

            disableButtonAndShowSnipper('preRegister');
            fetch('/api/preRegistration/PublicUserPreRegister/' + productSalesPlanningId + '/' + nationalCode,
                {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json; charset=utf-8'
                    }
                })
                .then(data => {
                    enableButtonAndHideSnipper('preRegister');
                    if (data.status === 401) {
                        $('#frmConfirm').submit();
                        return;
                    }
                    return data;
                })
                .then(response => response.json())
                .then(data => {

                    if (data.isSuccess === true) {
                        bootbox.alert('کاربر گرامی، پیش ثبت نام با موفقیت انجام شد!');
                        return;
                    } else {
                        if (data.message) {
                            bootbox.alert(data.message);
                        } else {
                            bootbox.alert('!لطفا مجددا تلاش کنید.');
                        }
                        return;
                    }
                });
        } else {

            $("#error-text-national-code").removeClass("d-none");
            $("#national_code").css("border-color", "#ff0000");
            bootbox.alert('!لطفا مجددا تلاش کنید.');
            enableButtonAndHideSnipper('preRegister');

        }
    }
}