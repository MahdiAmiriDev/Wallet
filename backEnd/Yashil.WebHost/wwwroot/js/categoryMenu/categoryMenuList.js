(async function () {
    const categories = await fetchMenuList();
    if (!categories || categories.length == 0) {
        return;
    }
    let itemCount = 0;
    const itemCountLimit = 50;
    const menuElement = document.querySelector('.sub-menu-block');
    const headerElement = document.getElementById('header');
    const categoryImage = document.querySelector('.category_image');
    const menuContainer = document.querySelector('#menu_container');
    const subMenuContainers = document.querySelectorAll('.submenu_column');

    if (menuContainer === null) {
        console.log('Not Found!')
    };

    /* دسته‌بندی محصولات اصلی */
    const generateCategoryTemplate = function (category) {
        return `<a href="/Company?categoryId=${category.id}" class="d-flex align-items-center gap-1 p-3 border-bottom menu_item" data-tag=${category.id}><div class="pe-none category_icon"><svg width="24" height="24" stroke="#2c3e50"><use href="../images/icons/mega-menu.svg#${category.icon}" /></svg></div><p class="mb-0 pe-none category_title text-nowrap">${category.title}</p></a>`;
    };

    /* دسته‌بندی Level 2 در منوی سمت چپ */
    const generateLevelTwoSubmenuTemplate = function (category) {
        return `<a href="/Company?categoryId=${category.id}"><p class="content_sub--item" style="width: fit-content">${category.title}</p></a>
                                              `;
    };

    /* زیر دسته بندی Level 1 که تک محصول است */
    const generateSingleSubmenuTemplate = function (category) {
        return `<a href="/Company?categoryId=${category.id}" class="content_item"><div class="d-flex gap-1"><p class="fw-bolder ps-3 mb-4" style="width: fit-content">${category.title}</p><svg xmlns="http://www.w3.org/2000/svg" class="icon icon-tabler icon-tabler-chevron-left mt-1" width="14" height="14" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor" fill="none" stroke-linecap="round" stroke-linejoin="round"><path stroke="none" d="M0 0h24v24H0z" fill="none"/><path d="M15 6l-6 6l6 6"/></svg></div></a>`;
    };

    /* زیر دسته بندی Level 1 که دارای چند محصول است */
    const generateMultipleSubmenuTemplate = function (category) {
        return `
                                                      <div>
                                                          <a href="/Company?categoryId=${category.id}" class="content_item">
                                                            <div class="d-flex gap-1">
                                                                <p class="fw-bolder ps-3 mb-3" style="width: fit-content">${category.title}</p>
                                                                <svg xmlns="http://www.w3.org/2000/svg" class="icon icon-tabler icon-tabler-chevron-left mt-1" width="14" height="14" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor" fill="none" stroke-linecap="round" stroke-linejoin="round">
                                                                    <path stroke="none" d="M0 0h24v24H0z" fill="none"/>
                                                                    <path d="M15 6l-6 6l6 6"/>
                                                                </svg>
                                                            </div>
                                                          </a>
                                                          <div class="mb-4 ms-2">
                                                              ${category.subCategories.map((subMenu) => generateLevelTwoSubmenuTemplate(subMenu)).join("")}
                                                          </div>
                                                      </div>
                                              `;
    };

    /* زیر دسته بندی کلی Level 1 که میتواند تک محصول یا چند محصول باشد */
    const generateLevelOneSubMenuItem = function (category) {
        return category.subCategories
            ? generateMultipleSubmenuTemplate(category)
            : generateSingleSubmenuTemplate(category);
    };

    /* اضافه کردن دسته بندی های Level 1 به منوی سمت راست */
    categories.forEach((category) => {
        menuContainer.insertAdjacentHTML("beforeend", generateCategoryTemplate(category));
    });

    /* اضافه کردن زیردسته های محصولات اصلی به منوی سمت چپ */
    const generateLevelOneSubMenus = (container, category) => {
        container.insertAdjacentHTML(
            "beforeend",
            generateLevelOneSubMenuItem(category)
        )
    };

    const isMenuLargerThanWindow = function () {
        const contentHeight = parseInt(getComputedStyle(menuElement).getPropertyValue('height')) + parseInt(getComputedStyle(headerElement).getPropertyValue('height'));
        const levelTwoMenuContainer = document.getElementById('submenu_container');
        if (contentHeight + 200 >= window.innerHeight) {
            menuElement.classList.add('small-monitor');
            levelTwoMenuContainer.style.maxHeight = "unset";
        } else {
            menuElement.classList.add('large-monitor');
            levelTwoMenuContainer.style.maxHeight = `${parseInt(getComputedStyle(menuContainer).getPropertyValue('height')) - 20}px`;
        }
    }

    /* جمع کردن تعداد زیردسته های یک دسته بندی و تولید ستون زیردسته بندی */
    const addSubcategoryCountAndGenerateColumn = function (category) {
        category.subCategories.forEach(subMenu => {
            if (subMenu.subCategories) {
                itemCount += subMenu.subCategories.length + 1;
                generateColumns(subMenu, itemCount);
            } else {
                itemCount += 1;
                generateColumns(subMenu, itemCount);
            }
        });
    }

    /* اضافه کردن زیردسته بندی ها به ستون های مربوطه در منوی سمت چپ */
    const generateColumns = function (category, count) {
        const columnCount = Math.ceil(count / itemCountLimit);
        if (columnCount === 1) {
            generateLevelOneSubMenus(subMenuContainers[columnCount - 1], category);
        } else if (columnCount === 2) {
            generateLevelOneSubMenus(subMenuContainers[columnCount - 1], category);
        } else {
            generateLevelOneSubMenus(subMenuContainers[columnCount - 1], category);
        }
    }

    /* نمایش زیردسته های اولین دسته بندی به همراه عکس دسته بندی به صورت پیشفرص */
    const levelOneMenuElements = document.querySelectorAll(".menu_item");
    if (levelOneMenuElements && levelOneMenuElements.length > 0) {
        levelOneMenuElements[0].classList.add("active");
    }
    addSubcategoryCountAndGenerateColumn(categories.at(0));
    categoryImage.setAttribute('src', `/api/PublicApi/GetCategoryMenuIcon?categoryId=${categories.at(0).id}&modificationDate=20230919075432`);
    categoryImage.setAttribute('alt', categories.at(0).title);
    window.onload = isMenuLargerThanWindow();


    /* نمایش زیردسته های مربوط به هر دسته بندی  Level 1 به هنگام hover کردن روی آن */
    menuContainer.addEventListener("mouseover", function (e) {
        const currentCategory = e.target;
        levelOneMenuElements.forEach((item) => {
            item.classList.remove("active");
        });

        if (currentCategory.classList.contains("menu_item")) {
            currentCategory.classList.add("active");
            categoryImage.onerror = () => {
                categoryImage.style.display = 'none';
            };
            categoryImage.onload = () => {
                categoryImage.style.display = 'block';
            }
            const selectedCategory =
                categories.find((category) => currentCategory.dataset.tag === category.id) ||
                categories.at(0);
            subMenuContainers.forEach(contentElement => contentElement.innerHTML = "");
            itemCount = 0;
            addSubcategoryCountAndGenerateColumn(selectedCategory);
            categoryImage.setAttribute('src', `/api/PublicApi/GetCategoryMenuIcon?categoryId=${selectedCategory.id}&modificationDate=20230919075432`);
            categoryImage.setAttribute('alt', selectedCategory.title);
        }
    });
})();