import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';
import {Component, EventEmitter, Input, OnInit, Output, ViewChild} from '@angular/core';
import {UploadedFactorPaymentDetailComponent} from './uploaded-factor-payment-detail.component';
import {YashilCustomButton} from '../../../shared/base/models/yasil-custom-button';
import {custom} from 'devextreme/ui/dialog';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';


@Component({
  selector: 'app-uploaded-factor-payment-list',
  templateUrl: './uploaded-factor-payment-list.component.html'
})
export class UploadedFactorPaymentListComponent extends Selectable implements OnInit {

  @ViewChild('listForm', {static: true}) listForm: BaseList;
  loadAfterSetFilter: boolean;
  selectedItemId: number;
  columns: any[] = [];
  customButtons: YashilCustomButton[];
  entityName = 'uploadedFactorPayment';
  // tslint:disable-next-line:variable-name
  _genericDataService: GenericDataService;
  detailComponent = UploadedFactorPaymentDetailComponent;
  @Output()
  transformToFactorPaymentBtnClicked: EventEmitter<any> = new EventEmitter<any>();
  // tslint:disable-next-line:variable-name
  _appDocumentId: number;
  @Input()
  set appDocumentId(val) {
    if (val !== this._appDocumentId) {
      this._appDocumentId = val;
    }
  }

  get appDocumentId(): number {
    return this._appDocumentId;
  }

  // tslint:disable-next-line:variable-name
  private _isValidUploadedFactorPaymentFile: boolean;

  get isValidUploadedFactorPaymentFile(): boolean {
    return this._isValidUploadedFactorPaymentFile;
  }

  @Input()
  set isValidUploadedFactorPaymentFile(value: boolean) {
    this._isValidUploadedFactorPaymentFile = value;
    this.bindCustomDataSources();
    this.bindCustomButtons();
  }

  private baseListUrl = `${this.entityName}/GetByCustomFilterForList?`;

  constructor(private genericDataService: GenericDataService) {
    super();
    this._genericDataService = genericDataService;
  }

  ngOnInit(): void {
    this.loadAfterSetFilter = true;
    this.bindColumns();
    this.bindCustomDataSources();
    this.bindCustomButtons();
  }

  private bindColumns(): void {
    this.columns.push({
      caption: 'کد فاکتور',
      dataField: 'factorCode'
    },{
      caption: 'آیا کد فاکتور نامعتبر است؟',
      dataField: 'factorCodeIsInvalid'
    },{
      caption: 'مبلغ پرداخت',
      dataField: 'price'
    },{
      caption: 'آیا مبلغ واریزی نامعتبر است؟',
      dataField: 'priceIsInvalid'
    },{
      caption: 'تاریخ پرداخت',
      dataField: 'paymentDate'
    },{
      caption: 'آیا تاریخ پرداخت نامعتبر است؟',
      dataField: 'paymentDateIsInvalid'
    },{
      caption: 'تعداد اقساط',
      dataField: 'installmentCount'
    },{
      caption: 'آیا تعداد اقساط نامعتبر است؟',
      dataField: 'installmentCountIsInvalid'
    },{
      caption: 'شماره حساب',
      dataField: 'bankAccountNumber'
    },{
      caption: 'آیا شماره حساب نامعتبر است؟',
      dataField: 'bankAccountNumberIsInvalid'
    });
  }

  bindCustomDataSources(): boolean {
    let customListUrl = `${this.baseListUrl}`;
    if (this.listForm) {
      if (this.appDocumentId) {
        customListUrl += `appDocumentId=${this.appDocumentId}&`;
      }
    }
    this.listForm.customListUrl = customListUrl;
    this.listForm.refreshList();
    return true;

  }

  afterInitialDetailComponent(componentInstance: any): void {
    const comp = (componentInstance.instance as UploadedFactorPaymentDetailComponent);
    if (this.appDocumentId) {
      comp.appDocumentId = this.appDocumentId;
    }

  }

  private bindCustomButtons(): void {
    if (this.isValidUploadedFactorPaymentFile) {
      this.customButtons = [{
        text: 'ثبت نهایی پرداختی های فاکتور',
        type: 'default',
        icon: 'exportselected',
      }];
    } else {
      this.customButtons = [];
    }
  }


  // نمایش پیام ثبت نهایی فاکتور
  private showFinalConfirmMessageForUpload(message: string): any {
    return custom({
      title: 'تایید نهایی بارگذاری فاکتور',
      messageHtml: message,
      buttons: [{
        text: 'تایید نهایی',
        type: 'success',
        onClick: (e) => {
          return {res: 'ok'};
        }
      },
        {
          text: 'انصراف',
          type: 'default',
          onClick: (e) => {
            return {res: 'cancel'};
          }
        }
      ],
    });
  }


  // ثبت نهایی اطلاعات پرداختی های فاکتور
  // اگر مشکلی بابت فایل آپلود شده وجود نداشته باشد و صحت سنجی انها درست باشد
  // این متد صدا زده میشود و این متد اطلاعات را از uploadedFactorPayment
  // میخواند و در جدول factorPayment بارگذاری میکند
  transferUploadedFileToFactorPayment(item: any): void {
    this.setBusy(true);
    this.showFinalConfirmMessageForUpload('کاربر گرامی آیا تمایل به ثبت نهایی اطلاعات دارید؟').show().then((dialogResult) => {
      if (dialogResult.res === 'ok') {
        this._genericDataService.getEntitiesWithAction(this.entityName,
          `TransferDataFromUploadedFactorPaymentsToFactorPayment`,
          null)
          .subscribe((res: any) => {
              this.setBusy(false);
              if (res.isSuccess) {
                this.showMessage(res.message);
              } else {
                this.showError(res.message);
              }
              this.transformToFactorPaymentBtnClicked.emit();
            }
          );
      } else {
        return;
      }
    });
  }
}
