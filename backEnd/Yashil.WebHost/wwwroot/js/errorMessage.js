function showErrorMessage(message, lblMessage) {
    lblMessage.html(' <strong>خطا!</strong> ' + message);
    lblMessage.removeClass('hide');
    lblMessage.removeClass('fade');
    lblMessage.addClass('show');
}
function showSuccessfulMessage(message, lblMessage) {
    lblMessage.html(message);
    lblMessage.removeClass('hide');
    lblMessage.removeClass('fade');
    lblMessage.addClass('show');
}
function hideErrorMessage(lblMessage) {
    lblMessage.html('');
    lblMessage.removeClass('show');
    lblMessage.addClass('hide');
    lblMessage.addClass('fade');

}
function disableButtonAndShowSnipper(btn, showSpin = true) {
    var btn = $('#' + btn);
    disableButton(btn);
    
    if (showSpin) {
        var spin = btn.find(".fa-spin");
        if (spin && spin.length > 0) {
            showSpinner(spin);
        }
        else {
            var newSpinnerElement = $('<i id="spinner" class="fas fa-spinner fa-spin"></i>');
            btn.append(newSpinnerElement);
        }
    }

}
function enableButtonAndHideSnipper(btn) {
    var btn = $('#' + btn);
    enableButton(btn);
    var spin = btn.find(".fa-spin");
    if (spin) {
        hideSpinner(spin);
    }
}

function showSpinner(spinner) {
    spinner.css('visibility', 'visible');
    spinner.css('display', '');
}
function hideSpinner(spinner) {
    spinner.css('display', 'none');
}

function disableButton(btn) {
    btn.prop("disabled", true);
}

function enableButton(btn) {
    btn.prop("disabled", false);
}