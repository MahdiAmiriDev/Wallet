
var select = '';
for (i = 0; i <= 100; i++) {
    select += '<option val=' + i + '>' + i + '</option>';
}
$('#CashPaymentPercentage').html(select);


function showHideDiv(ele, secondEle) {
    var srcInsatllmentCalcDiv = document.getElementById(ele);
    var srcPriceCalcDiv = document.getElementById(secondEle);

    if (srcInsatllmentCalcDiv != null) {
        if (srcInsatllmentCalcDiv.style.display == 'none') {
            srcInsatllmentCalcDiv.classList = 'd-block';
            srcPriceCalcDiv.classList = 'd-none';
        }

        return false;
    }
}

function showCalcPriceDiv(ele, secondEle) {
    var srcInsatllmentCalcDiv = document.getElementById(ele);
    var srcPriceCalcDiv = document.getElementById(secondEle);

    if (srcInsatllmentCalcDiv != null) {
        if (srcPriceCalcDiv.classList[0] == 'd-none') {
            srcPriceCalcDiv.classList = 'd-block w-100';
            srcInsatllmentCalcDiv.classList = 'd-none';
        }

        return false;
    }
}


$('.tp-parallax-wrap').css('top', '0px !important');
$('#mainnav-mobi').css('display', 'block');
$('#rev_slider_wrapper').css('height', '575px');
$(".dropdown").hover(
    function () {
        $('.dropdown-menu', this).stop().fadeIn("fast");
    },
    function () {
        $('.dropdown-menu', this).stop().fadeOut("fast");
    });


function scrolldown(id) {
    $('html, body').animate({
        scrollTop: $('#' + id).offset().top
    }, 1000);
}

function openNav() {
    var treeEl = $('#tree');
    var sideNavEl = $('#mySidenav');
    treeEl.css('display', '');
    treeEl.css('visibility', 'visible');
    sideNavEl.css('visibility', 'visible');
    sideNavEl.css('display', '');
}

function closeNav() {
    $('#tree').css('display', 'none');
    $('#mySidenav').css('display', 'none');
}
function closeModalNav() {
    $('#fsModalSort').modal('hide');
}



function encryptRsa(txt, key) {
    if (txt) {
        const rsa = forge.pki.publicKeyFromPem(key.publicKey);
        const res = window.btoa(rsa.encrypt(txt));
        return res;
    }
    return txt;
}

String.prototype.toEnglishDigit = function () {
    var find = ['۰', '۱', '۲', '۳', '۴', '۵', '۶', '۷', '۸', '۹'];
    var replace = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9'];
    var replaceString = this; var regex;
    for (var i = 0; i < find.length; i++) {
        regex = new RegExp(find[i], "g");
        replaceString = replaceString.replace(regex, replace[i]);
    }
    return replaceString;
};

function CalculatePriceForPay(e) {
    e.preventDefault();
    CalculatePrice();
}

function CalculatePrice() {

    let cashPaymentPercentage = $('#CashPaymentPercentage').val();
    cashPaymentPercentage = cashPaymentPercentage.toEnglishDigit();
    cashPaymentPercentage = + cashPaymentPercentage;
    let numberOfInstallment = $('#InstallmentCount').val();


    let token = $('#PaymentRequestToken').val();

    if (cashPaymentPercentage || cashPaymentPercentage === 0) {
        let dataParams = { paymentRequestToken: token, cashPaymentPercentage: cashPaymentPercentage };
        fetch('/api/HybridPaymentRequest/CalculatePriceForPayAsync',
            {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json; charset=utf-8'
                },
                body: JSON.stringify(dataParams)
            })
            .then(response => response.json())
            .then(result => {
                if (result.succeed) {
                    $('#resultMessage').css('display', 'none');
                    $('#RemainingCashPaymentPriceHint').css('display', 'none');
                    $('#RemainingHekmatPaymentPriceHint').css('display', 'none');
                    $('#RemainingCashPaymentPrice').css('display', 'block');
                    $('#RemainingHekmatPaymentPrice').css('display', 'block');
                    $('#CashPaymentPricePrecentage').val(cashPaymentPercentage);
                    $('#RemainingCashPaymentPrice').val(numberWithCommas(rialToTomanConvert(result.cashPaymentPrice)));
                    $('#RemainingHekmatPaymentPrice').val(numberWithCommas(rialToTomanConvert(result.hekmatPaymentPrice)));
                    $('#amountOfEachInstallment').val(numberWithCommas(rialToTomanConvert(result.hekmatPaymentPrice / numberOfInstallment)));
                    let input = document.createElement("input");
                    input.setAttribute("type", "hidden");
                    input.setAttribute("id", "cashPaymentPercentage");
                    input.setAttribute("name", "cashPaymentPercentage");
                    input.setAttribute("value", cashPaymentPercentage.toString());
                    input.setAttribute("style", "display: block");
                    input.setAttribute("class", "form-control");


                    let purchaseCashForm = $('#purchaseCashForm');
                    let purchaseHekmatForm = $('#purchaseHekmatForm');
                    $('#CashPaymentPrecentageAlert').css('display', 'none');

                    if (!result.isValidHekmatPrice) {
                        $("#invalidHekmatPriceModal").modal('show');
                        $('#CashPaymentPrecentageAlert').css('display', 'block');
                        $('#CashPaymentPrecentageAlert').text(result.message);


                    }
                    if (cashPaymentPercentage >= 0 && cashPaymentPercentage < 100) {

                        // نمایش فرم پرداخت اقساطی
                        if (!result.isValidHekmatPrice) {
                            purchaseHekmatForm.css('display', 'none');
                        } else {
                            purchaseHekmatForm.css('display', 'inline-block');
                        }

                        purchaseCashForm.css('display', 'none');
                        if (purchaseHekmatForm.get(0).querySelector('#cashPaymentPercentage')) {
                            purchaseHekmatForm.get(0).querySelector('#cashPaymentPercentage').remove();
                        }
                        purchaseHekmatForm.get(0).appendChild(input);
                    }
                    else if (cashPaymentPercentage === 100) {
                        // نمایش فرم پرداخت نقدی
                        purchaseHekmatForm.css('display', 'none');
                        purchaseCashForm.css('display', 'inline-block');
                        if (purchaseCashForm.get(0).querySelector('#cashPaymentPercentage')) {
                            purchaseCashForm.get(0).querySelector('#cashPaymentPercentage').remove();
                        }
                        purchaseCashForm.get(0).appendChild(input);
                    }
                } else {
                    $('#resultMessage').css('display', 'block');
                    $('#resultMessage').text(result.message);
                }
            });
    } else {

    };
}
function numberWithCommas(x) {
    return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
};


function rialToTomanConvert(val) {
    if (Number(val) === 0) return 0;
    return Math.round(val / 10);
};