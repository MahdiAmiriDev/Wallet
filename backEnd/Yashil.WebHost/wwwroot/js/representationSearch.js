loadCitiesByCompanyId();
loadCompanyByCityId(0);
loadRepresentation(0, 0);

// Get City List Value and Load Representation List Based on That
function retrieveCityListValue(e) {
    const data = e.params.data;
    const cityId = data.id;
    const company = $('#representation-list').select2('data');
    const companyId = company[0].id || 0;
    loadCompanyByCityId(cityId);
    loadRepresentation(companyId, cityId);
}

// Get Company List Value and Load Representation List Based on That
function retrieveCompanyListValue(e) {
    const data = e.params.data;
    const companyId = data.id;
    const city = $('#cities-province--list').select2('data');
    const cityId = city[0].id || 0;
    loadRepresentation(companyId, cityId);
}

// Load Company List Based on City Id
function loadCompanyByCityId(cityId) {
    let url = '/Company/CityCompanies';
    if (cityId !== 0) {
        url += "?cityId=" + cityId;
    }
    $("#company-list").load(url, function () {
        // Initialize Representation List Dropdown
        createSelect2Dropdown('#representation-list', 'لطفا مرکز را انتخاب کنید');
        $('#representation-list').on('select2:select', retrieveCompanyListValue)
    });
}

// Load City List Based on Company Id
function loadCitiesByCompanyId() {
    const url = '/Company/GetAllCities';
    $("#cities-province").load(url, function () {
        // Initialize City List Dropdown
        createSelect2Dropdown('#cities-province--list', 'لطفا شهر را انتخاب کنید');

        // Show Representations List of The Selected City
        $('#cities-province--list').on('select2:select', retrieveCityListValue)
    });
}

// Load Representation List Based on City Id and Company Id
function loadRepresentation(companyId, cityId) {
    if (Number(cityId) === 0) {
        cityId = null;
    }
    if (Number(companyId) === 0) {
        companyId = null;
    }

    const loadingDataTemplate = `
        <div class="w-100 d-flex justify-content-center">
            <div class="d-flex flex-column align-items-center gap-4">
                <span class="spinner-border text-dark" role="status" aria-hidden="true"></span>
                <span class="sr-only text-dark">درحال دریافت اطلاعات، لطفا منتظر بمانید...</span>
            </div>
        </div>
    `

    document.getElementById('representationTableContainer').innerHTML = loadingDataTemplate;

    $.ajax({
        type: 'Post',
        url: '/Representation/GetRepresentationsByCityAndCompanyId' + '?cityId=' + cityId + '&companyId=' + companyId,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: (function (data) {
            $("#representationTableContainer").html("");
            var setData = '<tbody>';
            var head =
                "<thead class='bg-primary text-white'><tr id='a' style='background-color:#028BFF' ><th class='thColumns' style='text-align: center;'>نام نمایندگی</th><th class='' style='text-align: center;'>شهر</th><th class='desktop' style='text-align: center;'>تلفن</th><th class='desktop' style='text-align: center;'>آدرس</th></thead>";
            for (var i = 0; i < data.length; i++) {

                setData = setData + "<tr>" +
                    "<td style='text-align: center;'>" + "<span id='lblTitle'>" + (data[i].title) + "</span>" + "</td>"
                    + "<td style='text-align: center;'>" + "<span id='lblCityTitle'>" + (data[i].cityTitle) + "</span>" + "</td>" +
                    "<td style='text-align: center;'>" + "<span id='lblTelephone'>" + (data[i].telephone) + "</span>" + "</td>" +
                    "<td style='text-align: center;'>" + "<span id='lblFullAddress'>" + (data[i].fullAddress) + "</span>" + "</td>" +
                    "</tr>";
            }


            $("#representationTableContainer").append("<div id='ooo' style='width: 100%;float: right;margin-right:1px;'><table id='representationRes' class='table table-bordered pt-3 table-hover dt-responsive' cellspacing='0' width='99.8%'>" + head + setData + "</tbody></table></div>");
            $("#representationRes").DataTable({
                "responsive": true,
                "language": {
                    "decimal": "",
                    "emptyTable": "داده ای مطابق با عبارت جستجو شده یافت نشد",
                    "info": "نمایش _START_ تا _END_ از _TOTAL_ کل ورودی ها",
                    "infoEmpty": "نمایش 0 تا 0 از 0 کل رکورد",
                    "infoFiltered": "(فیلتر شده از _MAX_ تعداد رکورد)",
                    "infoPostFix": "",
                    "thousands": ",",
                    "lengthMenu": "نمایش _MENU_ تا از کل ورودی ها",
                    "loadingRecords": "بارگزاری...",
                    "processing": "",
                    "search": "جستجو: ",
                    "zeroRecords": "داده ای مطابق با عبارت جستجو شده یافت نشد",
                    "paginate": {
                        "first": "اول",
                        "last": "آخر",
                        "next": "بعدی",
                        "previous": "قبلی",
                    },
                    "zeroRecords": "داده ای مطابق با عبارت جستجو شده یافت نشد",
                    "infoFiltered": "(فیلتر شده از _MAX_ تعداد رکورد)"
                },
            });
        })
    });

}