$("form").submit(function (e) {
    e.preventDefault();
});

function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;
    return true;
}

function checkCardNumber() {
    var cardNumber = $("#cardNumber").val();
    if (!isValidValidateHekmatCard(String(cardNumber))) {
        $("#validationCardNumber").html("کاربر گرامی ، شماره کارت وارد شده نامعتبر می باشد");
        return false;
    } else {
        $("#validationCardNumber").html("");
        return true;
    }
}

function checkNationalCode() {
    var nationalCode = $("#nationalCode").val();
    if (!nationalCodeValidate(nationalCode)) {
        $("#validationNationalCode").html("کاربر گرامی ، کد ملی وارد شده نامعتبر می باشد");
        return false;
    } else {
        $("#validationNationalCode").html("");
        return true;
    }
}

function checkMobile() {
    var mobile = $("#mobile").val();
    if (!isValidateMobileNumber(mobile)) {
        $("#validationMobileNumber").html("کاربر گرامی ، شماره همراه وارد شده نامعتبر می باشد");
        return false;
    } else {
        $("#validationMobileNumber").html("");
        return true;
    }
}

function checkTellNumber() {
    var tell = $("#tell").val();
    if (!isValidateTellNumber(tell)) {
        $("#validationTellNumber").html("کاربر گرامی ، شماره تلفن ثابت وارد شده نامعتبر می باشد");
        return false;
    } else {
        $("#validationTellNumber").html("");
        return true;
    }
}

function isValidateMobileNumber(mobile) {
    var re = /^(\+98|0)?9\d{9}$/;
    return re.test(mobile);
}

function isValidateTellNumber(mobile) {
    var re = /^0[1-9]{2}[0-9]{8}$/;
    return re.test(mobile);
}

function isValidValidateHekmatCard(card) {
    if (card === 'undefined' ||
        card.length !== 16 ||
        !card.startsWith("08920") ||
        card === "0892012345678912") {
        return false;
    }
    let cardTotal = 0;
    for (let i = 0; i < 16; i += 1) {
        const c = Number(card[i]);
        if (i % 2 === 0) {
            cardTotal += ((c * 2 > 9) ? (c * 2) - 9 : (c * 2));
        } else {
            cardTotal += c;
        }
    }
    return (cardTotal % 10 === 0);
}