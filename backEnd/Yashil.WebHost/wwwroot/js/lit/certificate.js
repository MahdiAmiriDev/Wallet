import { LitElement, html } from "lit";

/*
  تعریف کامپوننت اسلاعات گواهینامه
*/
class CertificateDetailComponent extends LitElement {
    static properties = {
        certificateDetailInformation: { type: Object },
    };

    constructor() {
        super();
    }

    /*
      برای استفاده از external css
    */
    createRenderRoot() {
        return this;
    }


    /*
      ساخت html اطلاعات گواهینامه
    */
    generateDetailTemplate() {
        return html`
      <div
        class="d-flex flex-column justify-content-between fw-medium"
        style="
        -moz-font-feature-settings: 'ss02';
        -webkit-font-feature-settings: 'ss02';
        font-feature-settings: 'ss02';
        margin: 0 10%;
        "
      >
        <p
          class="text-center certificate-information-container"
        >
          <span class="d-block fw-bold text-start text-dark" style="font-size:1.5vw !important">گواهی می‌شود:</span>
          <span class="mb-0 fw-bold text-body certificate-information"
            >${this.certificateDetailInformation?.fullName}</span
          >
          دارای کد ملی
          <span class="mb-0 fw-bold text-body certificate-information"
            >${this.certificateDetailInformation?.nationalCode}</span
          >
          دوره آموزشی
          <span class="mb-0 fw-bold text-body certificate-information">
            ${this.certificateDetailInformation?.courseTitle}
          </span>
          را به مدت
          <span class="mb-0 fw-bold text-body certificate-information"
            >${this.certificateDetailInformation?.courseDuration}</span
          >
          ساعت مطابق با آیین نامه آموزشی به صورت
          <span class="mb-0 fw-bold text-body certificate-information"
            >${this.certificateDetailInformation?.courseType}</span
          >
          با کسب نمره
          <span class="mb-0 fw-bold text-body certificate-information"
            >${this.certificateDetailInformation?.score}</span
          >
          با موفقیت به پایان رسانده است.
        </p>
        <div class="d-flex justify-content-between mx-5">
          <div>
            <p class="certificate-information mb-0">
              کد پیگیری:
              <span class="mb-0 fw-bold text-body certificate-information"
                >${this.certificateDetailInformation?.number}</span
              >
            </p>
            <p class="certificate-information">
              تاریخ صدور:
              <span class="mb-0 fw-bold text-body certificate-information"
                >${this.certificateDetailInformation?.issueDateInt}</span
              >
            </p>
          </div>
          <div class="position-relative text-center" style="width:30%">
            <p class="certificate-information mb-0">ومن ا... التوفيق</p>
            <p class="certificate-information">مدير عامل انستيتو ايزايران</p>
            <div class="position-absolute" style="top:-25%">
              <img
                src="/images/signature.jpg"
                width="100%"
                style="mix-blend-mode: multiply"
              />
            </div>
          </div>
        </div>
      </div>
    `;
    }

    render() {
        return html`${this.generateDetailTemplate()}`;
    }
}

/*
  Defines a new custom element
*/
customElements.define(
    "certificate-detail-component",
    CertificateDetailComponent
);
