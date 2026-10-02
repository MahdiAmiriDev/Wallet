import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';
import {AfterViewInit, Component, Input, OnInit, ViewChild} from '@angular/core';
import {GenericDataService} from "../../../shared/base/services/generic-data.service";


@Component({
  selector: 'app-uploaded-factor-installment-payment-list',
  templateUrl: './uploaded-factor-installment-payment-list.component.html'
})
export class UploadedFactorInstallmentPaymentListComponent
  extends Selectable
  implements OnInit, AfterViewInit {

  @ViewChild('listForm', {static: true})
  listForm: BaseList;

  loadAfterSetFilter: boolean;
  selectedItemId: number;

  columns: any[] = [];

  entityName = 'uploadedFactorInstallmentPayment';

  customListUrl =
    'uploadedFactorInstallmentPayment/getUploadedFactorInstallmentPayment?';

  @Input()
  showFinalRegistrationButton = false;

  @Input()
  yearMonth: number;

  @Input()
  message: string;


  constructor(
    private genericDataService: GenericDataService
  ) {
    super();
  }


  ngOnInit(): void {
    this.bindColumns();
  }


  ngAfterViewInit(): void {
    setTimeout(() => {
      this.bindCustomDataSources();
    });
  }


  public bindCustomDataSources(): boolean {

    let customerListUrl = `${this.customListUrl}`;

    if (this.yearMonth) {
      customerListUrl += `yearMonth=${this.yearMonth}&`;
    }

    if (this.listForm && this.yearMonth) {

      this.listForm.customListUrl = customerListUrl;
      this.listForm.refreshList();

      return true;
    }

    return false;
  }


  private bindColumns(): void {

    this.columns.push(
      {
        caption: 'شناسه',
        dataField: 'factorPaymentInstallmentId',
        width: '110px'
      },
      {
        caption: 'شناسه نامعتبر است.',
        dataField: 'factorPaymentInstallmentIdIsInvalid',
        width: '110px'
      },
      {
        caption: 'کدآموزشگاه',
        dataField: 'code',
        width: '100px'
      },
      {
        caption: 'کدآموزشگاه نامعتبر است.',
        dataField: 'codeIsInvalid',
        width: '100px'
      },
      {
        caption: 'موسسه',
        dataField: 'companyOrRepresentationTitle',
        width: '80px'
      },
      {
        caption: 'شماره فاکتور',
        dataField: 'factorCode',
        width: '90px'
      },
      {
        caption: 'شماره فاکتور نامعتبر است.',
        dataField: 'factorCodeIsInvalid',
        width: '100px'
      },
      {
        caption: 'تعداد اقساط پرداختی',
        dataField: 'numberOfInstallmentsPaid',
        width: '90px'
      },
      {
        caption: 'قیمت کل',
        dataField: 'sumAmountPaymentToSupplier',
        cellTemplate: 'number',
        width: '70px'
      },
      {
        caption: 'قیمت کل نامعتبر است.',
        dataField: 'sumAmountPaymentToSupplierIsInvalid',
        width: '100px'
      },
      {
        caption: 'مبلغ هر قسط',
        dataField: 'installmentAmount',
        cellTemplate: 'number',
        width: '80px'
      },
      {
        caption: 'مبلغ هر قسط نامعتبر است.',
        dataField: 'installmentAmountIsInvalid',
        width: '100px'
      },
      {
        caption: 'مبلغ برگشتی',
        dataField: 'refundAmount',
        cellTemplate: 'number',
        width: '80px'
      },
      {
        caption: 'مبلغ برگشتی نامعتبر است.',
        dataField: 'refundAmountIsInvalid',
        width: '100px'
      },
      {
        caption: 'مبلغ پرداخت شده',
        dataField: 'paidAmount',
        cellTemplate: 'number',
        width: '90px'
      },
      {
        caption: 'مبلغ پرداخت شده نامعتبر است.',
        dataField: 'paidAmountIsInvalid',
        width: '100px'
      },
      {
        caption: 'مانده پرداختی به تامین‌کننده',
        dataField: 'remainingPaymentToSupplier',
        cellTemplate: 'number',
        width: '100px'
      },
      {
        caption: 'کل پرداختی به تامین‌کننده',
        dataField: 'totalPaymentToSupplier',
        cellTemplate: 'number',
        width: '100px'
      },
      {
        caption: 'شماره قسط جاری',
        dataField: 'installmentNumber',
        width: '90px'
      },
      {
        caption: 'شماره قسط نامعتبر است.',
        dataField: 'installmentNumberIsInvalid',
        width: '100px'
      },
      {
        caption: 'تعداد اقساط باقی‌مانده',
        dataField: 'numberOfInstallmentsRemaining',
        width: '100px'
      },
      {
        caption: 'اقساط باقی‌مانده نامعتبر است.',
        dataField: 'numberOfInstallmentsRemainingIsInvalid',
        width: '100px'
      },
      {
        caption: 'شماره حساب بانکی',
        dataField: 'accountNumber',
        width: '90px'
      },
      {
        caption: 'شماره حساب بانکی نامعتبر است.',
        dataField: 'accountNumberIsInvalid',
        width: '100px'
      },
      {
        caption: 'شماره شبا',
        dataField: 'shebaNumber',
        width: '80px'
      },
      {
        caption: 'شماره شبا نامعتبر است.',
        dataField: 'shebaNumberIsInvalid',
        width: '100px'
      },
      {
        caption: 'صاحب حساب بانکی',
        dataField: 'bankAccountOwner',
        width: '90px'
      },
      {
        caption: 'آیا نام صاحب حساب نامعتبر است.',
        dataField: 'bankAccountOwnerIsInvalid',
        width: '100px'
      },
      {
        caption: 'نام بانک',
        dataField: 'bankName',
        width: '70px'
      },
      {
        caption: 'پیام خطا',
        dataField: 'errorMessage',
        width: '150px'
      }
    );
  }


  finalRegistrationButtonClicked(): void {

    this.confirmBeforeAction(
      'ثبت نهایی',
      'آیا از ثبت نهایی کد فایل بارگذاری شده در سایت اطمینان دارید؟',
      'تایید'
    )
      .show()
      .then(res => {

        if (res.res === 'ok') {
          this.finalRegistration();
        }

      });
  }


  finalRegistration(): void {

    this.setBusy(true);

    this.genericDataService
      .postEntityByUrl(
        'api/UploadedFactorInstallmentPayment/UpdateFactorInstallmentPaymentFromUploadedFile',
        {}
      )
      .subscribe(res => {

        this.setBusy(false);

        if (!res.isSuccess) {
          return;
        }

        this.showMessage(res.message);
        this.showFinalRegistrationButton = false;
        this.message = null;

        if (this.listForm) {
          this.listForm.refreshList();
        }

      });
  }
}
