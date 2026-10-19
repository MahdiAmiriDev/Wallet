$(document).ready(function () {
    getData();
});

function formatShamsiIntDate(intDate) {
    if (!intDate || intDate.toString().length !== 8) return intDate;

    const str = intDate.toString();
    const year = str.substring(0, 4);
    const month = str.substring(4, 6);
    const day = str.substring(6, 8);

    return `${year}/${month}/${day}`;
}

async function getData() {
    try {
        const response = await fetch('api/publicApi/GetListOfLocalCompanies');
        const data = await response.json();
        // Initialize DataTable after data is ready
        $('#userTable').DataTable({
            data: data,
            responsive: true,
            columns: [
                {
                    data: null,
                    title: "ردیف",
                    className: "text-center",
                    render: function (data, type, row, meta) {
                        return meta.row + 1;
                    }
                },
                { data: 'title', className: 'text-center' },
                { data: 'province', className: 'text-center' },
                { data: 'city', className: 'text-center' },
                { data: 'founder', className: 'text-center' },
                { data: 'code', className: 'text-center' },
                {
                    data: 'licenseExpireTime', className: 'text-center', render: function (data) {
                        return formatShamsiIntDate(data);
                    } },
                {
                    data: null,
                    className: 'text-center',
                    render: function (data, type, row) {
                        return `
                        <a class="text-white" href="/Representation/GetDetailOfLocalCompanies?id=${row.id}">
                            <button class="list-representation-show-more--button rounded-3 fs-14 ">نمایش
                            <svg xmlns="http://www.w3.org/2000/svg" width="22" height="22" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                                <path stroke="none" d="M0 0h24v24H0z" fill="none"/>
                                <path d="M10 12a2 2 0 1 0 4 0a2 2 0 0 0 -4 0" />
                                <path d="M12 18c-.328 0 -.652 -.017 -.97 -.05c-3.172 -.332 -5.85 -2.315 -8.03 -5.95c2.4 -4 5.4 -6 9 -6c3.465 0 6.374 1.853 8.727 5.558" />
                                <path d="M18 18m-3 0a3 3 0 1 0 6 0a3 3 0 1 0 -6 0" />
                                <path d="M20.2 20.2l1.8 1.8" />
                            </svg>
                            </a>
                        `;
                    }
                }
            ],
            language: {
                "search": "جستجو:",
                "lengthMenu": "نمایش _MENU_ رکورد",
                "info": "نمایش _START_ تا _END_ از کل ورودی ها (_TOTAL_)",
                "infoEmpty": "رکوردی موجود نیست",
                "infoFiltered": "(فیلتر شده از _MAX_ رکورد)",
                "zeroRecords": "رکوردی یافت نشد",
                "loadingRecords": "بارگزاری...",
                "paginate": {
                    "first": "اولین",
                    "last": "آخرین",
                    "next": "بعدی",
                    "previous": "قبلی"
                },
                "emptyTable": "هیچ داده‌ای در جدول وجود ندارد"
            }
        });

        // Hide spinner, show table
        document.getElementById('loadingSpinner').classList.remove("d-block");
        document.getElementById('loadingSpinner').classList.add("d-none");
        document.getElementById('userTable').classList.remove("d-none");

    } catch (error) {
        console.error("Error loading data:", error);
        document.getElementById('loadingSpinner').innerHTML = `
            <div class="text-danger text-center">خطا در دریافت اطلاعات. لطفا مجددا تلاش کنید.</div>
        `;
    }


}
