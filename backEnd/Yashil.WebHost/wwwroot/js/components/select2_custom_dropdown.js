function createSelect2Dropdown(elementId, placeholder) {
    $(document).ready(function () {
        $(elementId).select2({
            dir: 'rtl',
            language: 'fa',
            placeholder,
            selectionCssClass: 'select2-dropdown--tbao',
            dropdownCssClass: 'select2-panel--tbao'
        });

        $(elementId).on('select2:open', function (e) {
            $('input.select2-search__field').prop('placeholder', 'جستجو...');
            $('input.select2-search__field')[0].focus()
        });
    });

}
