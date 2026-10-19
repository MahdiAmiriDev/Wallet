
function deleteFilters() {
    $('.sidebar-left').addClass('disabledDiv');
    $('.sidebar-left').css('opacity', '0.5');
    $('.sidebar-left').append('<object id="deleteRecipientLoading" type="image/svg+xml" data="~/images/svg/loading.svg" class="d-block mx-auto text-center"></object>');
    $('.widget-categories.border-f-e6f3ff.loading').addClass('disabledDiv');
    $('.widget-categories.border-f-e6f3ff.loading').css('opacity', '0.5');
    $('.widget-categories.border-f-e6f3ff.loading').append('<object id="deleteRecipientLoading" type="image/svg+xml" data="~/images/svg/loading.svg" class="d-block mx-auto text-center"></object>');
}

    $('#citySelected').html('');
    $('#citySelected').html($('#citySelectedName').html());

