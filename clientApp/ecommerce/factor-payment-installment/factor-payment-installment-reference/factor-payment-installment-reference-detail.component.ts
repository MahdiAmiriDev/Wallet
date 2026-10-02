import { Selectable } from "../../../../shared/base/classes/selectable";
import { Component, Input, OnInit, ViewChild } from "@angular/core";
import { BaseList } from "../../../../shared/base/classes/base-list";
import { YashilCustomButton } from "../../../../shared/base/models/yasil-custom-button";
import { GenericDataService } from '../../../../shared/base/services/generic-data.service';
import { FormControl } from '@angular/forms';
import { IActiveDate } from 'ng-persian-datepicker';

@Component({
  selector: 'app-factor-payment-installment-Reference-Detail',
  templateUrl: './factor-payment-installment-reference-detail.component.html'
})
export class FactorPaymentInstallmentReferenceDetailComponent
  extends Selectable
  implements OnInit {

  @ViewChild('listForm', { static: true })
  listForm: BaseList;

  columns: any[] = [];

  entityName = 'uploadedFactorPaymentInstallment';

  private baseListUrl =
    'UploadedFactorInstallmentPayment/getFactorPaymentInstallmentReference?';

  @Input()
  yearMonth: number;

  @Input()
  categoryGroupId: number;

  @Input()
  creationDate: string;

  @Input()
  paymentReferenceId: string = '';

  @Input()
  paymentReferenceDate: string = '';

  @Input()
  readonlyPaymentReference: boolean = false;

  customButtons: YashilCustomButton[] = [];

  paymentReferenceDateDataValue = new FormControl('');

  constructor(
    private genericDataService: GenericDataService
  ) {
    super();

    this.bindCustomButton =
      this.bindCustomButton.bind(this);
  }

  ngOnInit(): void {

    if (
      this.hasPaymentReference()
    ) {

      this.readonlyPaymentReference = true;

      this.paymentReferenceDateDataValue.setValue(
        this.formatDateForDatePicker(
          this.paymentReferenceDate
        )
      );

    } else {

      this.readonlyPaymentReference = false;

      if (this.paymentReferenceDate) {

        this.paymentReferenceDateDataValue.setValue(
          this.formatDateForDatePicker(
            this.paymentReferenceDate
          )
        );
      }
    }

    this.bindColumns();
    this.bindCustomButton();

    if (this.bindCustomDataSources()) {
      this.loadAfterSetFilter = true;
    }
  }

  /**
   * بررسی می‌کند آیا شناسه و تاریخ
   * هر دو قبلاً ثبت شده‌اند یا خیر.
   */
  private hasPaymentReference(): boolean {
    return !!(
      this.paymentReferenceId &&
      this.paymentReferenceId.trim() !== '' &&
      this.paymentReferenceDate &&
      this.paymentReferenceDate.trim() !== ''
    );
  }

  private formatDateForDatePicker(date: string): string {

    if (!date) {
      return '';
    }

    const value = String(date).replace(/\//g, '');

    if (value.length === 8) {

      return (
        value.substring(0, 4) +
        '/' +
        value.substring(4, 6) +
        '/' +
        value.substring(6, 8)
      );
    }

    return date;
  }

  public bindCustomDataSources(): boolean {

    let customerListUrl = `${this.baseListUrl}`;

    if (this.creationDate) {

      customerListUrl +=
        `creationDate=${encodeURIComponent(this.creationDate)}`;
    }

    let res = false;

    if (this.creationDate || this.categoryGroupId) {

      this.listForm.customListUrl = customerListUrl;

      this.listForm.refreshList();

      res = true;
    }

    return res;
  }

  bindColumns(): void {

    this.columns = [
      {
        caption: 'نام طرح',
        dataField: 'categoryGroupTitle',
        visibleIndex: 0,
        alignment: 'right'
      },
      {
        caption: 'نام مرکز و نمایندگی',
        dataField: 'companyOrRepresentationTitle',
        visibleIndex: 0,
        alignment: 'right'
      },
      {
        caption: 'کد نمایندگی یا مرکز',
        dataField: 'companyOrRepresentationCode',
        visibleIndex: 0,
        alignment: 'right'
      },
      {
        caption: 'مبلغ قسط',
        dataField: 'installmentAmount',
        visibleIndex: 0,
        alignment: 'right'
      },
      {
        caption: 'شماره قسط',
        dataField: 'installmentNumber',
        visibleIndex: 0,
        alignment: 'right'
      }
    ];
  }

  bindCustomButton(): void {

    if (!this.hasPaymentReference()) {

      this.customButtons = [
        {
          text: 'ثبت شناسه مرجع پرداخت',
          type: 'primary',
          key: 'registerPaymentReferenceId'
        }
      ];

    } else {

      this.customButtons = [];
    }
  }

  /**
   * زمانی که کاربر از DatePicker تاریخ انتخاب می‌کند
   */
  onPaymentReferenceDateSelect(
    activeDate: IActiveDate
  ): void {

    if (
      !activeDate ||
      !activeDate.shamsi
    ) {
      return;
    }

    this.paymentReferenceDateDataValue.setValue(
      activeDate.shamsi
    );

    this.paymentReferenceDate =
      activeDate.shamsi.replace(/\//g, '');
  }

  /**
   * ثبت شناسه مرجع پرداخت
   */
  registerPaymentReference(): void {

    if (
      !this.paymentReferenceId ||
      this.paymentReferenceId.trim() === ''
    ) {

      this.showError(
        'لطفاً شناسه مرجع پرداخت را وارد کنید'
      );

      return;
    }

    if (
      !this.paymentReferenceDate ||
      this.paymentReferenceDate.trim() === ''
    ) {

      this.showError(
        'لطفاً تاریخ مرجع پرداخت را وارد کنید'
      );

      return;
    }

    this.setBusy(true);

    const dto = {

      creationDate: this.creationDate,

      yearMonth: this.yearMonth,

      paymentReferenceId:
      this.paymentReferenceId,

      paymentReferenceDate:
      this.paymentReferenceDate
    };

    this.genericDataService
      .postEntityByUrl(
        '/api/FactorPaymentInstallment/RegisterPaymentReference',
        dto
      )
      .subscribe({

        next: (res: any) => {

          this.setBusy(false);

          if (res && res.isSuccess) {

            this.showMessage(
              'شناسه مرجع پرداخت با موفقیت ثبت شد'
            );

            this.readonlyPaymentReference = true;
            this.paymentReferenceDateDataValue.setValue(
              this.formatDateForDatePicker(
                this.paymentReferenceDate
              )
            );

            this.customButtons = [];

          } else {

            this.showError(
              res?.message ||
              'ثبت شناسه مرجع پرداخت با خطا مواجه شد'
            );
          }
        },

        error: () => {

          this.setBusy(false);

          this.showError(
            'ثبت شناسه مرجع پرداخت با خطا مواجه شد'
          );
        }
      });
  }

  customButtonClicked(key: any): void {

    if (
      key === 'registerPaymentReferenceId'
    ) {

      this.registerPaymentReference();
    }
  }
}
