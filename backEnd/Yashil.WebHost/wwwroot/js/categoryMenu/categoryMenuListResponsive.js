(async function () {
    const categories = await fetchMenuList();

    const menuContainerResponsive = document.getElementById("menu_container--responsive");
    const subMenuContainerResponsive = document.getElementById("submenu_responsive");
    const subMenuContainerResponsiveTitle = document.getElementById('submenu_title');
    const subMenuImageApi = "/api/PublicApi/GetCategoryIcon?categoryId="

    /* دسته‌بندی محصولات اصلی */
    const generateCategoryTemplateResponsive = function (category) {
        return `<div class="d-flex flex-column align-items-center p-2 py-3 menu_item--responsive" data-tag=${category.id}><div class="pe-none"><svg width="20" height="20" stroke="#2c3e50"><use href="../images/icons/mega-menu.svg#${category.icon}" /></svg></div><span class="pb-0 pt-2 pe-none" style="font-size: 11px !important">${category.title}</span></div>`;
    };

    /* دسته‌بندی Level 2 در منوی سمت چپ */
    const generateLevelTwoSubmenuTemplateResponsive = function (category) {
        return `
                      <div class="col-6 col-sm-4 d-flex justify-content-center text-center">
                        <a href="/Company?categoryId=${category.id}" class="d-flex flex-column align-items-center gap-2 my-2 w-50">
                          <div class="subcategory_responsive">
                            <img src=${subMenuImageApi + category.id} alt=${category.title} class="subcategory_image"/>
                          </div>
                                  <h4 style="font-size:10px">${category.title}</h4>
                        </a>
                      </div>
                      `;
    };

    /* زیر دسته بندی Level 1 که تک محصول است */
    const generateSingleSubmenuTemplateResponsive = function (category) {
        return `<a href="/Company?categoryId=${category.id}" class="d-block border-bottom submenu_item--responsive"><div><h3 class="fw-bold py-4 mb-0" style="font-size: 12px">${category.title}</h3></div></a>`;
    };

    /* زیر دسته بندی Level 1 که دارای چند محصول است */
    const generateMultipleSubmenuTemplateResponsive = function (category, index) {
        return `
            <div class="accordion-item border-0 border-bottom">
               <h2 class="accordion-header" id="heading-${index}">
                    <button class="accordion-button fw-bold px-0 py-4 ${index !== 0 ? 'collapsed' : ''}" style="font-size: 12px" type="button" data-bs-toggle="collapse" data-bs-target="#collapse-${index}" aria-expanded="${index === 0 ? true : false}" aria-controls="collapse-${index}">
                        ${category.title}
                    </button>
               </h2>
               <div id="collapse-${index}" class="accordion-collapsed collapse ${index === 0 ? 'show' : ''}" aria-labelledby="heading-${index}" data-bs-parent="#submenu_responsive">
                    <div class="accordion-body row">${category.subCategories.map(subMenu => generateLevelTwoSubmenuTemplateResponsive(subMenu)).join("")}</div>
               </div>
            </div>
        `
    };

    /* زیر دسته بندی کلی Level 1 که میتواند تک محصول یا چند محصول باشد */
    const generateLevelOneSubMenuItemResponsive = function (category, index) {
        return category.subCategories
            ? generateMultipleSubmenuTemplateResponsive(category, index)
            : generateSingleSubmenuTemplateResponsive(category);
    };

    /* اضافه کردن دسته بندی های Level 1 به منوی سمت راست */
    categories.forEach((category) => {
        menuContainerResponsive.insertAdjacentHTML("beforeend", generateCategoryTemplateResponsive(category));
    });

    /* اضافه کردن زیردسته های محصولات اصلی به منوی سمت چپ */
    const generateLevelOneSubCategoriesResponsive = (category) => {
        subMenuContainerResponsiveTitle.querySelector('h4').textContent = `همه محصولات ${category.title}`;
        subMenuContainerResponsiveTitle.setAttribute('href', `/Company?categoryId=${category.id}`);
        subMenuContainerResponsive.innerHTML = "";
        category.subCategories.forEach((subCategory, index) =>
            subMenuContainerResponsive.insertAdjacentHTML(
                "beforeend",
                generateLevelOneSubMenuItemResponsive(subCategory, index)
            )
        );
    };

    // All Menu Items
    const allMenuItems = document.querySelectorAll(".menu_item--responsive");

    /* نمایش زیردسته های اولین دسته بندی به صورت پیشفرص */
    generateLevelOneSubCategoriesResponsive(categories.at(0));
    allMenuItems[0].classList.add("active");

    /* نمایش زیردسته های مربوط به هر دسته بندی اصلی به هنگام کلیک کردن روی آن */
    menuContainerResponsive.addEventListener("click", function (e) {
        const currentItem = e.target;
        allMenuItems.forEach((item) => {
            item.classList.remove("active");
        });
        if (currentItem.classList.contains("menu_item--responsive")) {
            currentItem.classList.add("active");

            const selectedCategory =
                categories.find((category) => currentItem.dataset.tag === category.id) ||
                categories.at(0);
            generateLevelOneSubCategoriesResponsive(selectedCategory);
        }
    });
})();