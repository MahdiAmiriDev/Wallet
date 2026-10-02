import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';
import {Component, EventEmitter, Input, OnInit, Output, ViewChild} from '@angular/core';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';

@Component({
  selector: 'app-hekmat-payment-gateway-transaction-bulk-request-validate',
  templateUrl: './hekmat-payment-gateway-transaction-bulk-request-validate.component.html'
})
export class HekmatPaymentGatewayTransactionBulkRequestValidateComponent extends Selectable implements OnInit {
  @ViewChild('listForm', {static: true}) listForm: BaseList;
  loadAfterSetFilter: boolean;
  selectedItemId: number;
  columns: any[] = [];
  entityName = 'hekmatPaymentGatewayTransactionBulkRequestDetail';
  customButtons: any;
  @Input()
  hekmatPaymentGatewayTransactionBulkRequestId: number;

  // tslint:disable-next-line:variable-name
  _invalidItem: boolean;

  get invalidItem(): boolean {
    return this._invalidItem;
  }

  @Input()
  set invalidItem(val) {
    if (val !== this._invalidItem) {
      this._invalidItem = val;
      this.bindCustomDataSources();
    }
  }


  _categoryId: number;
  get categoryId(): number {
    return this._categoryId;
  }

  @Input()
  set categoryId(val) {
    if (val !== this._categoryId) {
      this._categoryId = val;
    }
  }


  @Output()
  afterFinalRegistration: EventEmitter<any> = new EventEmitter<any>();
  private baseListUrl = 'HekmatPaymentGatewayTransactionBulkRequestDetail/GetHekmatBulkRequest?';

  constructor(private genericDataService: GenericDataService) {
    super();
  }

  ngOnInit(): void {
    this.bindColumns();
    this.bindCustomButtons();
    if (this.bindCustomDataSources()) {
      this.loadAfterSetFilter = true;
    }
  }

  private bindColumns(): void {
    this.columns.push(
      {
        caption: 'شماره کارت',
        dataField: 'cardNumber',
        width: '20%'
      },
      {
        caption: 'نامعتبر بودن شماره کارت',
        dataField: 'cardNumberIsInValid',
        width: '5%'
      },
      {
        caption: 'کد ملی صاحب کارت',
        dataField: 'nationalCode',
        width: '20%'
      },
      {
        caption: 'نامعتبر بودن کد ملی صاحب کارت',
        dataField: 'nationalCodeIsInValid',
        width: '5%'
      },
      {
        caption: 'مبلغ به ریال',
        dataField: 'price',
        width: '15%'
      },
      {
        caption: 'نامعتبر بودن مبلغ',
        dataField: 'priceIsInValid',
        width: '5%'
      },
      {
        caption: 'تعداد اقساط',
        dataField: 'installmentCount',
        width: '5%'
      },
      {
        caption: 'نامعتبر بودن تعداد اقساط',
        dataField: 'installmentCountIsInValid',
        width: '5%'
      },
      {
        caption: 'پیام اعتبار سنجی',
        dataField: 'validateMessage',
        width: '20%'
      }
    );
  }

  bindCustomDataSources(): boolean {
    let customListUrl = `${this.baseListUrl}`;
    if (this.listForm) {
      customListUrl += `showInvalidItem=${this._invalidItem}&`;
      this.listForm.customListUrl = customListUrl;
      this.listForm.refreshList();
      return true;
    }
  }

  bindCustomButtons(): void {
    /*
       اگر پارمتر نامعتبر برابر با false بود دکمه ثبت نهایی را به کاربر نمایش دهد
         */
    if (!this._invalidItem) {
      this.customButtons = [{
        icon: 'file',
        text: 'ثبت نهایی',
        type: 'success',
        key: 'finalRegistration',
        float: 'right'
      }];
    }
  }

  /*
تایید ثبت نهایی و اعمال کاربران بارگذاری شده از کاربر
   */
  customButtonClicked(key: any): void {
    if (key === 'finalRegistration') {
      this.confirmFinalRegistration();
    }
  }


  /*
 تاییده ارکاربر قبل از ثبت نهایی کاربران بارگذاری شده
 */
  public confirmFinalRegistration() {
    this.confirmBeforeAction('ثبت نهایی', 'آیا از ثبت نهایی اکسل تراکنش ها بارگذاری شده در سایت اطمینان دارید؟', 'تایید')
      .show().then((dialogResult) => {
      if (dialogResult.res === 'ok') {
        this.finalRegistration();
      }
    });

  }

  /*
  درصورتی که فایل اکسل بارگذاری شده دارای خطا نباشد و کاربر دکمه ثبت نهایی را بزند این متد صدا زده میشود
    */
  public finalRegistration(): void {
    this.setBusy(true);
    this.genericDataService.postEntityByUrl(`api/HekmatPaymentGatewayTransactionBulkRequest/AddHekmatBulkRequestDetail?hekmatPaymentGatewayTransactionBulkRequestId=${+this.hekmatPaymentGatewayTransactionBulkRequestId}`, {})
      .subscribe(response => {
        if (!response.isSuccess) {
          this.setBusy(false);
          this.afterFinalRegistration.emit();
          this.showError(response.message);
        } else {
          this.setBusy(false);
          this.afterFinalRegistration.emit();
          this.showMessage(response.message);
        }
      });
  }

  /*این متد برسی می کند که اگر سازمان کاربری تغییر کرده بود ردیف مربوط به آن کاربر را تغییر رنگ بدهد */
  onListRowPrepared(e: any): void {
    if (e && e.rowType === 'data' && e.data.hasInvalidItem == true) {
      e.rowElement.bgColor = '#ffc107';
      e.rowElement.className = e.rowElement.className.replace('dx-row-alt', '');
    }
  }
}
