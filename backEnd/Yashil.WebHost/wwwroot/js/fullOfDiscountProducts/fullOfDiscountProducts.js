const swiperContainer = document.querySelector(".bestseller_swiper");
const initializFullOfDiscountProductsSwipper = async function () {
    const productsResponse = await fetch('/api/PublicApi/GetFullOfDiscountsProducts');
    const products = await productsResponse.json();

    const productsContainer = document.querySelector(".products");
    const swiperEl = document.querySelector(".bestseller_swiper--wrapper");

    const convertCurrency = function (price) {
        return new Intl.NumberFormat("fa-IR", {
            compactDisplay: "short",
        }).format(price / 10);
    };


    /* ---------- Generates Product Card Template ---------- */
    const generateProductCardTemplate = function (product, index) {
        return `
        <article class="bestseller border-end border-bottom px-1 py-1">
        <!-- Skeleton Loading Placeholder for Biggest Discount Product Card -->
        <div class="skeleton-loading--container w-100 h-100 d-flex flex-column align-items-center justify-content-between gap-4" style="background: #F8F7FC; padding: 15px">
                                            <div class="skeleton skeleton-rect biggest-discount-product--skeleton"></div>
                                            <div class="d-flex align-items-center gap-3 w-100">
                                                <div class="skeleton skeleton-circle"
                                                     style="--c-s: 50px;--c-p: 0px;"></div>
                                                <div class="skeleton skeleton-line"
                                                     style="
            --lines: 2;
                --l-h: 15px;
        --c-w: 100%;
        --c-p: 0px;
    flex: 1
          "></div>
                                            </div>
                                        </div>
        <!-- Skeleton Loading Placeholder for Biggest Discount Product Card /-->

        <!-- Biggest Discount Product Card -->
           <a href="/Product?pSpId=${product.id}" class="skeleton-content--container d-none ss02 col d-flex justify-content-center text-decoration-none px-1 py-1">
           <div class="d-flex flex-column w-100 mx-2 my-3">
        <div class="text-center d-flex align-items-center">
            <img data-src="/api/PublicApi/GetProductSalesPlanningImage?productSalesPlanningId=${product.id}&modificationDate=${product.modificationDate}" alt="${product.productTitle}" class="w-100" style="aspect-ratio: 1.9; object-fit: cover;" />
        </div>
        <div class="d-flex flex-column justify-content-between pt-4 lh-lg w-100" style="flex:1">
            <div class="mb-4">
                <h6 class="text-dark product-title">${product.productTitle || "عنوان"}</h6>
            </div>
            <div class="d-flex justify-content-between align-items-center">
                <div style="max-width:40px" class="pb-2">
                    <img data-src="/api/PublicApi/GetCompanyIcon?cId=${product.companyId}&modificationDate=${product.modificationDate}" class="w-100"/>
                </div>
                <div>
                    <div class="d-flex align-items-center justify-content-between">
                        <div class="d-flex align-items-center text-dark">
                            <span class="fs-5 text-dark fw-bold" style="line-height: 10px">
                                ${convertCurrency(
            product.price -
            Math.round(product.price * (product.discountPercentage / 100))
        )}
                            </span>
                            <svg width="20" height="20" viewBox="0 0 16 16">
                                <use href="../images/icons/general.svg#currency" />
                            </svg>
                        </div>
                    </div>
                    <div class="d-flex align-items-center justify-content-end gap-1">
                        <span class="text-decoration-line-through"
                              style="color: #c0c2c5">${convertCurrency(product.price)}</span>
                        <div class="d-flex align-items-center px-2 lh-base"
                             style="border-radius:10px; background: var(--dark-primary-yellow-color)">
                            <span class="text-light discount">${product.discountPercentage}%</span>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</a>
<!-- Biggest Discount Product Card /-->
</article>
    `;
    };


    /* ---------- Insert Products to Container For Laptop ---------- */
    products.forEach((product, index) =>
        productsContainer.insertAdjacentHTML("beforeend", generateProductCardTemplate(product, index))
    );


    /* ---------- Generates Swiper Slide Template ---------- */
    const generateSwiperSlideTemplate = function (product, index) {
        return `
        <div class="swiper-slide bestseller border-start px-1 py-1">
        <!-- Skeleton Loading Placeholder for Biggest Discount Product Card -->
        <div class="skeleton-loading--container w-100 h-100 d-flex flex-column align-items-center justify-content-between gap-4" style="background: #F8F7FC; padding: 15px">
                                            <div class="skeleton skeleton-rect biggest-discount-product--skeleton"></div>
                                            <div class="d-flex align-items-center gap-3 w-100">
                                                <div class="skeleton skeleton-circle"
                                                     style="--c-s: 50px;--c-p: 0px;"></div>
                                                <div class="skeleton skeleton-line"
                                                     style="
            --lines: 2;
                --l-h: 15px;
        --c-w: 100%;
        --c-p: 0px;
    flex: 1
          "></div>
                                            </div>
                                        </div>
        <!-- Skeleton Loading Placeholder for Biggest Discount Product Card /-->

        <!-- Biggest Discount Product Card -->
    <div class="skeleton-content--container d-none ss02 d-flex flex-column" style="height:100%">
        <a href="/Product?pSpId=${product.id}"
           class="col d-flex justify-content-center text-decoration-none px-2"
           style="align-items:center">
            <div class="d-flex flex-column w-100" style="height:90%">
                <div class="text-center">
                    <img data-src="/api/PublicApi/GetProductSalesPlanningImage?productSalesPlanningId=${product.id}&modificationDate=${product.modificationDate}" alt="${product.productTitle}" class="w-100" />
                </div>
                <div class="d-flex flex-column justify-content-between pt-4 lh-lg w-100" style="flex:1">
                    <div>
                        <h6 class="text-dark product-title">${product.productTitle || "عنوان"}</h6>
                    </div>
                    <div class="d-flex justify-content-between align-items-center">
                        <div style="max-width:40px" class="pb-2">
                            <img data-src="/api/PublicApi/GetCompanyIcon?cId=${product.companyId}&modificationDate=${product.modificationDate}" class="w-100"/>
                        </div>
                        <div>
                            <div class="d-flex align-items-center justify-content-between">
                                <div class="d-flex align-items-center text-dark">
                                    <span class="fs-5 text-dark fw-bold" style="line-height: 10px">
                                        ${convertCurrency(
                product.price -
                Math.round(product.price * (product.discountPercentage / 100))
            )}
                                    </span>
                                    <svg width="20" height="20" viewBox="0 0 16 16">
                                        <use href="../images/icons/general.svg#currency" />
                                    </svg>
                                </div>
                            </div>
                            <div class="d-flex align-items-center justify-content-end gap-1">
                                <span class="text-decoration-line-through"
                                      style="color: #c0c2c5">${convertCurrency(product.price)}</span>
                                <div class="d-flex align-items-center px-2 lh-base"
                                     style="border-radius:10px; background: var(--dark-primary-yellow-color)">
                                    <span class="text-light discount">${product.discountPercentage}%</span>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </a>
    </div>
    <!-- Biggest Discount Product Card /-->
</div>
    `;
    };


    /* ---------- Insert Products to Container For Swiper ---------- */
    products.forEach((product, index) =>
        swiperEl.insertAdjacentHTML("beforeend", generateSwiperSlideTemplate(product, index))
    );

    /* ---------- Swiper Bug Fix ---------- */
    /* ---------- در هنگام تغییر سایز سوییپر دچار باگ میشود و ستون ها و محتوا را به درستی نشان نمیدهد که با کد زیر این مشکل رفع خواهد شد ---------- */
    window.onresize = () => {
        if (
            window.innerWidth <= 768 &&
            !swiperContainer.classList.contains("swiper-grid")
        ) {
            swiperContainer.classList.add("swiper-grid");
            swiperContainer.classList.add("swiper-grid-column");
        }
    };


    /* ---------- Lazy Load Images ---------- */
    skeletonLoading();
    /* ---------- Lazy Load Images ---------- */

    /* ---------- Limit Long Product Title ---------- */
    shrinkText('.product-title', 40);
    /* ---------- Limit Long Product Title ---------- */

    /* ---------- Swiper Configuration ---------- */
    const bestsellerSwiper = new Swiper(".bestseller_swiper", {
        // Optional parameters
        direction: "horizontal",
        freeMode: {
            enabled: true
        },
        breakpoints: {
            // when window width is >= 320px
            280: {
                slidesPerView: 1,
            },
            // when window width is >= 320px
            320: {
                slidesPerView: 1.5,
            },
            // when window width is >= 550px
            550: {
                slidesPerView: 2,
            },
        },
        // Navigation arrows
        navigation: {
            nextEl: ".swiper-button-next",
            prevEl: ".swiper-button-prev",
        },
        grid: {
            rows: 2,
        },
    });
};

if (swiperContainer) {
    initializFullOfDiscountProductsSwipper();
}
