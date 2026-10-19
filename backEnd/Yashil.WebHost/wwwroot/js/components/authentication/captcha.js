function getCaptchaInfo() {

    fetch('/CreateDntCaptchaParams',
            {
                method: 'GET'
            })
        .then(response => response.json())
        .then(data => {
            $("#dntCaptchaImg").attr("src", data.dntCaptchaImgUrl);
            $("#DNTCaptchaText").attr("value", data.dntCaptchaTextValue);
            $("#DNTCaptchaToken").attr("value", data.dntCaptchaTokenValue);
            $("div.dntCaptcha").attr("id", data.dntCaptchaId);

        }).catch((error) => {
            $("#validationErrorMessage").html('خطا در تولید کپچا');
            $("#validationErrorMessage").show();

        });
};

getCaptchaInfo();

function doRefreshCaptcha() {
    //$("#validationErrorMessage").show();
    getCaptchaInfo();
};