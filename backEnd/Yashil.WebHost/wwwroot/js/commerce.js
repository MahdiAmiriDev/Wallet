function findNode(id, currentNode) {
    var i,
        currentChild,
        result;

    if (id == currentNode.id) {
        return currentNode;
    } else {

        // Use a for loop instead of forEach to avoid nested functions
        // Otherwise "return" will not work properly
        if (currentNode.nodes != null) {
            for (i = 0; i < currentNode.nodes.length; i += 1) {
                currentChild = currentNode.nodes[i];

                // Search in the current child
                result = findNode(id, currentChild);

                // Return the result if the node has been found
                if (result != null) {
                    return result;
                }
            }
        }
        // The node has not been found and we have no more options
        return null;
    }
}

function setCategory(categoryId, companyId, cityId, representationId) {
    var params = "";
    if (companyId) {
        params = "company=" + companyId;
    }
    if (cityId) {
        params += ((params != "" ? "&" : "") + "cityId=" + cityId);
    }
    if (representationId) {
        params += ((params != "" ? "&" : "") + "representationId=" + representationId);
    }
    if (categoryId) {
        params += ((params != "" ? "&" : "") + "categoryId=" + categoryId);
    }
    var url = '/Company/Index?' + params;
    window.location = url;
}

function toggleUserMenu(userBox, userAccount) {
    const userBoxEl = $('#' + userBox);
    const userAccountEl = $('#' + userAccount);

    userBoxEl.toggleClass("open");
    if (userAccountEl.hasClass('openAccount')) {
        userAccountEl.removeClass('openAccount');
        userAccountEl.addClass('closeAccount');
    } else if (userAccountEl.hasClass('closeAccount') && !userBoxEl.hasClass('open')) {
        userAccountEl.removeClass('closeAccount');
        userAccountEl.addClass('openAccount');
    }
}

function redirectToUserLogin() {
    location.href = '/user/login';
}

function cancelEditOrder(orderId) {
    var param = {
        orderId: orderId,
        confirm: {
            title: " انصراف از ویرایش سفارش",
            message: "آیا از ویرایش سفارش خود منصرف شده اید؟"
        },
        actionName: 'CancelModifyOrder',
        errorMessage: 'خطا در انصراف از ویرایش سفارش'
    };
    modifyOrderStatus(param);
}

function editOrder(orderId) {
    var param = {
        orderId: orderId,
        confirm: {
            title: "ویرایش سفارش",
            message: "آیا میخواهید سفارش خود را ویرایش کنید؟"
        },
        actionName: 'StartModifyOrder',
        errorMessage: 'خطا در ویرایش سفارش'
    };
    modifyOrderStatus(param);
}

function cancelOrder(orderId, btnName) {

    var param = {
        orderId: orderId,
        confirm: {
            title: "انصراف سفارش",
            message: "آیا میخواهید سفارش خود را انصراف دهید؟"
        },
        actionName: 'CancelOrder',
        errorMessage: 'خطا در انصراف سفارش'
    };
    modifyOrderStatus(param, btnName);
}

function openDeleteModal(orderId, submit) {
    const buttonElement = document.querySelector(submit);
    const spinnerElement = buttonElement.querySelector('.spinner-border');
    spinnerElement.classList.remove('d-none');
    buttonElement.setAttribute('disabled', true);
    const param = {
        orderId: orderId,
        confirm: {
            title: "حذف تحویل گیرنده",
            message: "آیا مطمئنید که این تحویل گیرنده حذف شود؟"
        },
        errorMessage: 'عملیات حذف تحویل گیرنده با خطا مواجه شد'
    };
    modifyAddressStatus(param, submit);

}

function openShoppingCartItemDeleteModal(shoppingCartItemId) {
    const orderDetailElement = document.querySelector(`.cart-item-${shoppingCartItemId}`);
    const submitButton = orderDetailElement.querySelector('.btn-outline-danger');
    const spinnerElement = orderDetailElement.querySelector('.spinner-border');
    spinnerElement.classList.remove('d-none');
    submitButton.setAttribute('disabled', true);

    const param = {
        shoppingCartItemId: shoppingCartItemId,
        confirm: {
            title: "حذف از سبد خرید",
            message: "آیا مطمئنید که این آیتم سبد خرید حذف شود؟"
        },
        errorMessage: 'عملیات حذف آیتم سبد خرید با خطا مواجه شد'
    };
    deleteShoppingCartItem(param, submitButton, spinnerElement);
}

function refundOrder(orderId) {
    const param = {
        orderId: orderId,
        confirm: {
            title: "لغو سفارش",
            message: "آیا میخواهید سفارش خود را لغو کنید؟"
        },
        actionName: 'RefundOrder',
        errorMessage: 'خطا در لغو  سفارش'

    };
    modifyOrderStatus(param);
}



function modifyOrderStatus(param, btnName) {
    bootbox.confirm({
        title: param.confirm.title,
        message: param.confirm.message,
        locale: 'fa',
        centerVertical: true,
        callback: function (result) {
            if (btnName) {
                disableButtonAndShowSnipper(btnName);
            }
            if (result) {
                var dataParams = { OrderId: + param.orderId };
                fetch('/api/Order/' + param.actionName,
                    {
                        method: 'POST',
                        headers: {
                            'Content-Type': 'application/json; charset=utf-8'
                        },
                        body: JSON.stringify(dataParams)
                    })
                    .then(response => response.json())
                    .then(data => {
                        if (data.isSuccess) {
                            window.location = "/Order";
                        } else {
                            bootbox.alert(param.errorMessage);
                        }

                    })
                    .catch(x => {
                        bootbox.alert(param.errorMessage);
                    }).finally(() => {
                        if (btnName) {
                            enableButtonAndHideSnipper(btnName);
                        }
                    });
            } else {
                enableButtonAndHideSnipper(btnName);
            }
        }
    });
}

function modifyAddressStatus(param, submit) {
    const buttonElement = document.querySelector(submit);
    const spinnerElement = buttonElement.querySelector('.spinner-border');
    bootbox.confirm({
        title: param.confirm.title,
        message: param.confirm.message,
        locale: 'fa',
        centerVertical: true,
        callback: function (result) {
            if (result) {
                spinnerElement.classList.remove('d-none');
                buttonElement.setAttribute('disabled', true);
                fetch('/api/Recipient/DeleteEntity/' + param.orderId,
                    {
                        method: 'POST',
                        headers: {
                            'Content-Type': 'application/json; charset=utf-8'
                        },
                        //body: JSON.stringify(id)
                    })
                    .then(response => response.json())
                    .then(data => {
                        if (data.isSuccess === true) {
                            window.location = "/Order/checkout";
                        } else {
                            $('.alert').alert();
                            $('.alert').show();

                        }

                    })
                    .catch(x => {
                        $('.alert').alert();
                        $('.alert').show();
                    });
            } else {
                spinnerElement.classList.add('d-none');
                buttonElement.removeAttribute('disabled');
            }
        }
    });
}

function deleteShoppingCartItem(param, submit, spinner) {
    bootbox.confirm({
        title: param.confirm.title,
        message: param.confirm.message,
        locale: 'fa',
        centerVertical: true,
        callback: function (result) {
            if (result) {
                loadingSpinnerWithModal.showLoadingSpinner();
                spinner.classList.add('d-none');
                submit.removeAttribute('disabled');
                fetch('api/shoppingCartItem/delete/' + param.shoppingCartItemId,
                    {
                        method: 'POST',
                        headers: {
                            'Content-Type': 'application/json; charset=utf-8'
                        }
                    })
                    .then(response => response.json())
                    .then(data => {
                        if (data.isSuccess === true) {
                            window.location = data.returnUrl;
                        }
                    })
                    .catch(x => {
                    });
            } else {
                spinner.classList.add('d-none');
                submit.removeAttribute('disabled');
            }
        }
    });
}

var categoryData = null;

function loadCategoryTree(onSelectedNodeChange, categoryContainer, parentId, cityId, companyId) {
    var params = '';
    if (cityId) {
        params = 'cityId=' + cityId + '&';
    }
    if (categoryData == null) {
        fetch('/api/PublicApi/getCategoryHierarchically?' + params + companyParam)
            .then(response => response.json()).then(data => {
                categoryData = data;
                bindCategoryTreeWithData(onSelectedNodeChange, categoryContainer, data, parentId);
            });
    } else {
        bindCategoryTreeWithData(onSelectedNodeChange, categoryContainer, data, parentId);
    }
}

function bindCategoryTreeWithData(onSelectedNodeChange, categoryContainer, data, parentId = null) {
    var rootNode = [];
    if (parentId) {
        for (let i = 0; i < data.length; i++) {
            const node = findNode(parentId, data[i]);
            if (node != null) {
                rootNode.push(node);
                break;
            }
        }
        if (rootNode.length === 0) {
            rootNode = data;
        }
    } else {
        rootNode = data;
    }
    var imageSize = $(window).width();
    categoryContainer.each(function () {
        $(this).treeview({
            //expandIcon: 'fa fa-chevron-right',
            //collapseIcon: 'fa fa-chevron-down',
            data: rootNode,
            levels: 2,
            backColor: 'white',
            onNodeSelected: onSelectedNodeChange
        });
        if (imageSize < 500) {

        } else if (imageSize > 500) {
            $(this).treeview('expandAll', { silent: true })
        }

    });
    var li_count = 0;
    var j = 0;
    var parentNodeId = false;
    if (imageSize < 500) {

    } else if (imageSize > 500) {
        $('.categoriesTreeCustom ul.list-group li.list-group-item.node-').each(function () {
            var attr = $(this).attr('Id');

            if (typeof attr !== 'undefined' && attr !== false) {
                parentNodeId = true;
            }
            if ($(this).find('span').hasClass('glyphicon-minus')) {
                $(this).attr('Id', 'ul_' + $(this).data('nodeid'));
                $(this).addClass("TreeLiHead");
                // $(this).prepend('<i class="fa fa-chevron-right"></i>')
            } else {
                $(this).attr('Id', 'li_' + $(this).data('nodeid'));
            }

        })
    }

    li_count = $('.categoriesTreeCustom ul.list-group li.list-group-item.node-:last-child').data('nodeid');
    if (parentNodeId == false) {
        for (var i = 0; i < li_count + 1; i++) {
            if ($('#ul_' + i).length) {
                $('#ul_' + i + '> .node-icon')
                    .after('<span class="fa fa-chevron-right" style="margin-left:5px"></span>');
            }
            if (!$('#li_' + i).length) {
                if (j > 5) {
                    $('#li_' + (i - 1))
                        .after(
                            '<div id="showMoreItem" onclick="showMoreLiTreer()" class="showMoreItems"><span class="indent"></span><span class="indent"></span>+ نمایش بیشتر</div>');
                }
                j = 0;
                $('#ul_' + i).addClass('content hideContent')
            }
            if ($('#li_' + i).length) {
                j++;
                $('#li_' + i).addClass('TreeLi');
                if (j > 5) {
                    // $('#li_' + i).after('<div id="showMoreItem" onclick="showMoreLiTreer()" class="showMoreItem"><span class="indent"></span><span class="indent"></span><span class="indent"></span>+ Show more</div>');
                    $('#li_' + i).removeClass('list-group-item');
                    $('#li_' + i).addClass('showLessItem');
                }


            }
        }
        if (j > 5) {
            $('#li_' + li_count)
                .after(
                    '<div id="showMoreItem" onclick="showMoreLiTreer()" class="showMoreItems"><span class="indent"></span><span class="indent"></span>+ نمایش بیشتر</div>');
        }
    }


}

function showMoreLiTreer() {

    if ($('#showMoreItem').text() == '+ نمایش بیشتر') {
        $('.showLessItem').addClass('showMoreItem');
        $('.showLessItem').removeClass('showLessItem');
        $('.showMoreItem').addClass('list-group-item');
        $('.showMoreItems').html("<span class='indent'></span><span class='indent'></span>- نمایش کمتر");
    } else {
        $('.showMoreItem').addClass('showLessItem');
        $('.showMoreItem').removeClass('showMoreItem');
        $('.showLessItem').removeClass('list-group-item');
        $('.showMoreItems').html("<span class='indent'></span><span class='indent'></span>+ نمایش بیشتر");
    }
}

function showMoreLiTree() {
    $('.LiLessItem').addClass('list-group-item');
    $('.LiLessItem').removeClass('showLessItem');
    $('.LiLessItem').addClass('showMoreItem');
    $('#showLessItem').removeClass('showLessItem');
    $('#showLessItem').addClass('showMoreItem');
    $('#showMoreItem').removeClass('showMoreItem');
    $('#showMoreItem').addClass('showLessItem');

}

function showLessLiTree() {
    $('.LiLessItem').removeClass('list-group-item');
    $('.LiLessItem').addClass('showLessItem');
    $('.LiLessItem').removeClass('showMoreItem');
    $('#showLessItem').removeClass('showMoreItem');
    $('#showLessItem').addClass('showLessItem');
    $('#showMoreItem').removeClass('showLessItem');
    $('#showMoreItem').addClass('showMoreItem');
}

function removeParam(key) {
    $('.sidebar-left').addClass('disabledDiv');
    $('.sidebar-left').css('opacity', '0');
    $('.sidebar-left').append(
        '<object id="deleteRecipientLoading" type="image/svg+xml" data="~/images/svg/loading.svg" style="position:absolute;left: 50%; top:50%;  transform: translate(-50%, -50%) scale(0.8); z-index: 1000;"></object>');
    $('.widget-categories.border-f-e6f3ff.loading').addClass('disabledDiv');
    $('.widget-categories.border-f-e6f3ff.loading').css('opacity', '0.5');
    $('.widget-categories.border-f-e6f3ff.loading').append(
        '<object id="deleteRecipientLoading" type="image/svg+xml" data="~/images/svg/loading.svg" style="position:absolute;left: 50%; top:50%;  transform: translate(-50%, -50%) scale(0.8); z-index: 1000;"></object>');
    key = encodeURIComponent(key);
    var kvp = document.location.search.substr(1).split('&');
    let i = 0;
    var res = [];
    for (; i < kvp.length; i++) {
        if (!kvp[i].startsWith(key + '=')) {
            res.push(kvp[i]);
        }
    }
    // can return this or...
    let params = res.join('&');
    // reload page with new params
    document.location.search = params;
}

function insertParam(key, value) {
    $('.sidebar-left').addClass('disabledDiv');
    $('.sidebar-left').css('opacity', '0');
    $('.sidebar-left')
        .append(
            '<object id="deleteRecipientLoading" type="image/svg+xml" data="~/images/svg/loading.svg" style="position:absolute;left: 50%; top:50%;  transform: translate(-50%, -50%) scale(0.8); z-index: 1000;"></object>');


    $('#advanceSearchModal.sidenav').addClass('disabledDiv');
    $('#advanceSearchModal.sidenav').css('opacity', '0');
    $('#advanceSearchModal.sidenav')
        .append(
            '<object id="deleteRecipientLoading" type="image/svg+xml" data="~/images/svg/loading.svg" style="position:absolute;left: 50%; top:50%;  transform: translate(-50%, -50%) scale(0.8); z-index: 1000;"></object>');

    key = encodeURIComponent(key);
    value = encodeURIComponent(value);

    // kvp looks like ['key1=value1', 'key2=value2', ...]
    var kvp = document.location.search.substr(1).split('&');
    let i = 0;

    for (; i < kvp.length; i++) {
        if (kvp[i].startsWith(key + '=')) {
            let pair = kvp[i].split('=');
            pair[1] = value;
            kvp[i] = pair.join('=');
            break;
        }
    }

    if (i >= kvp.length) {
        kvp[kvp.length] = [key, value].join('=');
    }

    // can return this or...
    let params = kvp.join('&');


    document.location.search = params;

}

function openAddShoppingCartItemModal(productSalesPlanningId, representationId, cartType, message) {
    loadingSpinnerWithModal.hideLoadingSpinner();
    const param = {
        productSalesPlanningId: productSalesPlanningId,
        representationId: representationId,
        cartType: cartType,
        confirm: {
            title: "اضافه کردن محصول موجود به سبد خرید",
            message: "کاربر گرامی به تعداد " +
                message +
                " مورد از این محصول قبلا به سبد خرید شما اضافه شده است، آیا مایل به اضافه کردن مجدد این محصول به سبد خرید می باشید؟"
        },
        errorMessage: 'عملیات اضافه کردن محصول به سبد خرید با خطا مواجه شد'
    };
    addDuplicateItemToCart(param);
}

function addDuplicateItemToCart(param) {

    loadingSpinnerWithModal.hideLoadingSpinner();
    bootbox.confirm({
        title: param.confirm.title,
        message: param.confirm.message,
        locale: 'fa',
        centerVertical: true,
        callback: function (result) {
            if (result) {
                addToCart(param.productSalesPlanningId, param.representationId, param.cartType);
            }
        }
    });
}


function confirmBeforeAddToCart(productSalesPlanningId, representationId, cartType, buttonSelector) {
    var modalElement = new bootstrap.Modal(document.getElementById('apiModal'));
    document.getElementById('modalBody').textContent = 'در حال لود ...';

    fetch('/api/publicApi/GetAddProductToBasketNoticeMessage')
        .then(function (res) { return res.json(); })
        .then(function (data) {
            var message = data.message ? data.message.trim() : '';

            if (message) {
                document.getElementById('modalBody').textContent = message;
                modalElement.show();

                var okBtn = document.getElementById('modalOkBtn');
                okBtn.onclick = function () {
                    modalElement.hide();
                    checkDuplicateItemAndAddToCart(productSalesPlanningId, representationId, cartType, buttonSelector);
                };
            } else {
                checkDuplicateItemAndAddToCart(productSalesPlanningId, representationId, cartType, buttonSelector);
            }
        })
        .catch(function (err) {
            document.getElementById('modalBody').textContent = 'خطا در بارگذاری پیام.';
            modalElement.show();

            var okBtn = document.getElementById('modalOkBtn');
            okBtn.onclick = function () {
                modalElement.hide();
                checkDuplicateItemAndAddToCart(productSalesPlanningId, representationId, cartType, buttonSelector);
            };
        });
}





function checkDuplicateItemAndAddToCart(productSalesPlanningId, representationId, cartType, button) {
    const buttonElement = document.querySelector(button);
    const spinnerElement = buttonElement.querySelector('.spinner-border');

    buttonElement.setAttribute('disabled', true);
    spinnerElement.classList.remove('d-none');

    if (!representationId) {
        const representaion = $('#representation-list').select2('data');
        var rId = representaion && representaion[0].id;
        if (!rId || Number(rId) <= 0) {
            buttonElement.removeAttribute('disabled');
            spinnerElement.classList.add('d-none');
            loadingSpinnerWithModal.hideLoadingSpinner();
            bootbox.alert("لطفا شهر و بعد نمایندگی را انتخاب نمائید");
            return;
        }
    }
    var data = {};
    if (representationId && representationId != null) {
        data.RepresentationId = representationId;
    } else {
        data.RepresentationId = +rId;
        loadingSpinnerWithModal.hideLoadingSpinner();
    }
    data.ProductSalesPlanningId = productSalesPlanningId;
    data.CartType = cartType;
    fetch('/api/shoppingCartItem/CheckDuplicateItemsInTheCart',
        {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json; charset=utf-8'
            },
            body: JSON.stringify(data)
        })
        .then(response => response.json())
        .then(data => {
            if (!data.errors) {
                if (data.isSuccess === false) {
                    buttonElement.removeAttribute('disabled');
                    spinnerElement.classList.add('d-none');
                    loadingSpinnerWithModal.hideLoadingSpinner();
                    bootbox.alert("کاربر گرامی این محصول قبلا به سبد خرید شما اضافه شده است. ");
                    return;
                } else if (data.message) {
                    buttonElement.removeAttribute('disabled');
                    spinnerElement.classList.add('d-none');
                    loadingSpinnerWithModal.hideLoadingSpinner();
                    //data.message :تعداد این آیتم که قبلا به سبد خرید اضافه شده است
                    openAddShoppingCartItemModal(productSalesPlanningId, representationId, cartType, data.message);
                } else {
                    addToCart(productSalesPlanningId, representationId, cartType, buttonElement, spinnerElement);
                    loadingSpinnerWithModal.hideLoadingSpinner();
                }
            }
        }).catch(data => {
            buttonElement.removeAttribute('disabled');
            spinnerElement.classList.add('d-none');
            bootbox.alert('لطفا مجددا تلاش کنید.');
            loadingSpinnerWithModal.hideLoadingSpinner();
        });
}




function addToCart(productSalesPlanningId, representationId, cartType, button, spinner) {


    if (!representationId) {
        const representaion = $('#representation-list').select2('data');
        loadingSpinnerWithModal.hideLoadingSpinner();
        var rId = representaion && representaion[0].id;
        if (!rId || Number(rId) <= 0) {
            bootbox.alert("لطفا شهر و بعد نمایندگی را انتخاب نمائید");
            loadingSpinnerWithModal.hideLoadingSpinner();
            return;
        }
    }
    button.setAttribute('disabled', true);
    spinner.classList.remove('d-none');
    loadingSpinnerWithModal.hideLoadingSpinner();

    var data = {};
    if (representationId && representationId != null) {
        data.RepresentationId = representationId;
    } else {
        loadingSpinnerWithModal.hideLoadingSpinner();
        data.RepresentationId = +rId;
    }
    data.ProductSalesPlanningId = productSalesPlanningId;
    data.CartType = cartType;
    loadingSpinnerWithModal.hideLoadingSpinner();
    fetch('/api/shoppingCartItem/addToCart',
        {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json; charset=utf-8'
            },
            body: JSON.stringify(data)
        })
        .then(response => response.json())
        .then(data => {
            if (!data.errors && data.isSuccess === true) {
                loadingSpinnerWithModal.showLoadingSpinner();
                window.location = "/shoppingCartItem";
            } else {
                if (data.message) {
                    loadingSpinnerWithModal.hideLoadingSpinner();
                    button.removeAttribute('disabled');
                    spinner.classList.add('d-none');
                    bootbox.alert(data.message);
                }
                return false;
                loadingSpinnerWithModal.hideLoadingSpinner();
            }
        })
        .catch(data => {
            button.removeAttribute('disabled');
            spinner.classList.add('d-none');
            bootbox.alert('لطفا مجددا تلاش کنید.');
            loadingSpinnerWithModal.hideLoadingSpinner();
        });
}

function addOrderToCart(orderId) {
    if (!orderId) {
        loadingSpinnerWithModal.hideLoadingSpinner();
        $('#Message').html("خطا در ارسال سفارش به سبد خرید");
        $('#alert').removeClass('fade');
        $('#alert').addClass('show');
        return;
    }
    fetch('/api/shoppingCartItem/addOrderToCart/' + orderId,
        {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json; charset=utf-8'
            }
        })
        .then(response => response.json())
        .then(data => {
            if (!data.errors && data.isSuccess === true) {
                loadingSpinnerWithModal.showLoadingSpinner();
                window.location = "/shoppingCartItem";
            } else {
                if (data.message) {
                    loadingSpinnerWithModal.hideLoadingSpinner();
                    bootbox.alert(data.message);
                }
                return false;
            }
            return true;
        });
}

function logout() {
    fetch('/api/authentication/Logout',
        {
            method: 'POST'
        })
        .then(() => {
            localStorage.clear();
        })
        .then(data => {
            window.location = "/";
        })
        .finally(() => {
            window.location = "/";
        });
}

function openPayDuplicateOrderModal(recipientId, message, spinner, submit) {
    const param = {
        recipientId: recipientId,
        confirm: {
            title: "تایید پرداخت سفارش تکراری",
            message: " کاربر گرامی محصول'  " +
                message +
                "  'بااین کد ملی قبلا خریداری شده است آیا مایل به خرید مجدد می باشید؟ (برای مشاهده خرید های قبلی به لیست سفارشات مراجعه نمایید.)"
        },
        errorMessage: 'عملیات تایید پرداخت سفارش تکراری با خطا مواجه شد'
    };
    doPayDuplicateOrder(param, spinner, submit);
}

function doPayDuplicateOrder(param, spinner, submit) {
    bootbox.confirm({
        title: param.confirm.title,
        message: param.confirm.message,
        locale: 'fa',
        centerVertical: true,
        callback: function (result) {
            if (result) {
                document.documentElement.classList.add("overflow-hidden");
                doPay(param.recipientId, spinner, submit);
            } else {
                spinner.classList.add('d-none');
                submit.removeAttribute('disabled');
            }
        }
    });
}

function checkAndPayDuplicateOrder(recipientId, orderId) {
    const buttonElement = document.querySelector('.payment-final-register');
    const spinnerElement = buttonElement.querySelector('.spinner-border');

    var data = {};
    data.recipientId = recipientId;
    if (orderId) {
        data.orderId = +orderId
    }

    spinnerElement.classList.remove('d-none');
    buttonElement.setAttribute('disabled', true);
    fetch('/api/order/checkDuplicateOrderForPay',
        {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json; charset=utf-8'
            },
            body: JSON.stringify(data)           
        })
        .then(response => response.json())
        .then(data => {
            if (!data.errors && data.isSuccess === true) {
                if (data.returnUrl === "#") {
                    loadingSpinnerWithModal.hideLoadingSpinner();
                    bootbox.alert(data.message);
                } else {
                    openPayDuplicateOrderModal(recipientId, data.message, spinnerElement, buttonElement);
                }
            } else {
                doPay(recipientId);
            }
        })
        .catch(data => {
            spinnerElement.classList.add('d-none');
            buttonElement.removeAttribute('disabled');
            bootbox.alert('لطفا مجددا تلاش کنید.');
            loadingSpinnerWithModal.hideLoadingSpinner();
        });
}

//function openExistSubsidyConfirmedInitialRegistrationOrderModal(recipientId, message) {
//    const param = {
//        recipientId: recipientId,
//        confirm: {
//            title: " انصراف سفارش توسط کاربر",
//            message: " کاربر گرامی شما به تعداد'  " + message + "  'سفارش با وضعیت یارانه تایید شده دارید که پرداخت نشده است، آیا میخواهید سفارش فوق را انصراف بزنید و یارانه آن را در سفارش جدید استفاده کنید؟ "
//        },
//        errorMessage: 'عملیات انصراف سفارش با خطا مواجه شد'
//    };
//    doCancelOrder(param);
//}
/**
 * در صورتی که اپلیکیشن مبتنی بر یارانه باشد قبل از نمایش صفحه پیش فاکتور و محاسبه یارانه برای کالاهی انتخابی چک می شود
 * در صورتی که  در لیست سفارشات قبلی کاربر، سفارشی دارای یارانه تایید شده و یا ثبت اولیه با وضعیت سفارش ثبت اولیه باشد
 *ان سفارش  را به وضعیت لغو سیستمی تغییر می دهیم 
 * تا یارانه این گونه سفارش ها برای سفارش جدید لحاظ شود
 * @param {any} recipientId
 */
function doCancelOrder(recipientId, spinner, submit) {
    var data = {};
    data.recipientIdString = recipientId;
    fetch('/api/order/CancelSubsidyConfirmedInitialRegistrationOrder',
        {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json; charset=utf-8'
            },
            body: JSON.stringify(data)
        })
        .then(response => response.json())
        .then(data => {
            if (data.isSuccess) {
                loadOrderSummary(recipientId, spinner, submit);
                document.body.classList.remove('modal-open');
            } else {
                spinner.classList.add('d-none');
                submit.removeAttribute('disabled');
                if (data.message) {
                    bootbox.alert(data.message);
                } else {
                    bootbox.alert('بروز خطا در بررسی یارانه حکمت کارت');
                }
            }
        })
        .catch(x => {
            spinner.classList.add('d-none');
            submit.removeAttribute('disabled');
            bootbox.alert('لطفا مجددا تلاش کنید.');
        });
}



//input validation for preRegister in internal product 
function isNumber(evt) {
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    return true;
}


//copyLicenseSpotPlayer for Order List Items in _SpotPlayerPartial.cshtml
function copySpotPlayerLicense() {
    // Get the text field
    var copyText = document.getElementById("InputLicenseSpotPlayer");

    // Select the text field
    copyText.select();
    copyText.setSelectionRange(0, 99999); // For mobile devices

    // Copy the text inside the text field
    navigator.clipboard.writeText(copyText.value);

    // Alert the copied text
    bootbox.alert({
        title: 'لایسنس کپی شد',
        message: 'کاربر گرامی لایسنس در کلیپبورد کپی شد.',
    });
}



