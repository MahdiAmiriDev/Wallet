//کلاس مربوط به نمایش لودینگ به همراه مدال بوت استرپ 
//در فایل لی اوت از این کلاس شی ساخته شده
class ModalLoadingSpinner {
    bodyElement = document.body;
    modalElement;

    constructor(modalId) {
        this.modalId = modalId;
        this._renderTemplate();
        this._createModalInstance();
    }

    // غیر فعال کردن اسکرول و تعامل کاربر
    _disableBodyInteraction() {
        this.bodyElement.classList.add('overflow-hidden');
    }

    //ایجاد شی جدید از مدال بوت استرپ  و غیر فعال کردن کیبورد در خارج از مدال
    _createModalInstance() {

        const modalDomElement = document.getElementById(this.modalId);

        this.modalElement = new bootstrap.Modal(modalDomElement, {
            keyboard: false,
            backdrop: 'static'
        });

        // جلوگیری از کلیک روی عناصر زیر Overlay
        modalDomElement.addEventListener('click', function (event) {
            event.preventDefault();
            event.stopPropagation();
        }, true);
    }

    // نمایش Loading
    showLoadingSpinner() {

        this._disableBodyInteraction();

        this.modalElement.show();
    }

    // مخفی کردن Loading
    hideLoadingSpinner() {

        this.modalElement.hide();

        this.bodyElement.classList.remove('overflow-hidden');
    }

    // Template
    _generateTemplate() {

        return `
            <div
                class="modal fade"
                id="${this.modalId}"
                tabindex="-1"
                data-bs-backdrop="static"
                data-bs-keyboard="false"
                style="
                    background: rgba(240,240,240,0.62);
                    backdrop-filter: blur(1px);
                    pointer-events: auto;
                    z-index: 99999;
                "
            >

                <div class="modal-dialog modal-dialog-centered">

                    <div class="d-block">

                        <div
                            class="modal-content"
                            style="
                                background: transparent !important;
                                border: 0;
                                position: absolute;
                                left: 50%;
                                top: 50%;
                                width: 200px;
                                height: 200px;
                                transform: translate(-50%, -50%) scale(0.8);
                                pointer-events: none;
                            "
                        >
                            <span
                                class="loader"
                                style="z-index: 100000 !important;"
                            ></span>
                        </div>

                        <span
                            class="text-center"
                            style="
                                transform: translate(-50%, 350%) scale(0.8);
                                position: absolute;
                                left: 50%;
                                top: 50%;
                                color: #0a58ca !important;
                                pointer-events: none;
                            "
                        >
                            لطفا منتظر بمانید...
                        </span>

                    </div>

                </div>

            </div>
        `;
    }
    //رندر شدن تمپلیت  اسپینر لودینگ
    _renderTemplate() {

        this.bodyElement.insertAdjacentHTML(
            'beforeend',
            this._generateTemplate()
        );
    }
}