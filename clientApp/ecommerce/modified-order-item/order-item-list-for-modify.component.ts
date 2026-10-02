import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';
import {Component, EventEmitter, Input, OnInit, Output, ViewChild} from '@angular/core';
import {YashilCustomButtonColumn} from '../../../shared/base/classes/yashil-cutom-button-column';
import {DetailComponentDirective} from '../../../shared/base/base-list-form/detail-component.directive';


@Component({
  selector: 'app-order-item-list-for-modify',
  templateUrl: './order-item-list-for-modify.component.html'
})
export class OrderItemListForModifyComponent extends Selectable implements OnInit {

  @Output()
  public orderItemEdited: EventEmitter<any> = new EventEmitter<any>();

  @Output()
  public orderItemCanceled: EventEmitter<any> = new EventEmitter<any>();

  @ViewChild('listForm', {static: true}) listForm: BaseList;
  @ViewChild(DetailComponentDirective) componentContainer: DetailComponentDirective;
  @Input()
  allowGrouping = false;
  representationId: number;
  companyId: number;
  selectedItemId: number;
  customRowButtons: YashilCustomButtonColumn;
  columns: any[] = [];
  entityName = 'orderItem';

  // tslint:disable-next-line:variable-name
  _orderId: number;
  @Input()
  set orderId(val) {
    if (val !== this._orderId) {
      this._orderId = val;
    }
  }

  get orderId(): number {
    return this._orderId;
  }

  // tslint:disable-next-line:variable-name
  _mobileNumber: string;
  @Input()
  set mobileNumber(val) {
    if (val !== this._mobileNumber) {
      this._mobileNumber = val;
      this.bindCustomDataSources();
    }
  }

  get mobileNumber(): string {
    return this._mobileNumber;
  }


  // tslint:disable-next-line:variable-name
  _cardNumber: string;
  @Input()
  set cardNumber(val) {
    if (val !== this._cardNumber) {
      this._cardNumber = val;
      this.bindCustomDataSources();
    }
  }

  get cardNumber(): string {
    return this._cardNumber;
  }


  // tslint:disable-next-line:variable-name
  _recipientNationalCode: string;
  @Input()
  set recipientNationalCode(val) {
    if (val !== this._recipientNationalCode) {
      this._recipientNationalCode = val;
    }
  }

  get recipientNationalCode(): string {
    return this._recipientNationalCode;
  }


  // tslint:disable-next-line:variable-name
  _saleType: number;
  @Input()
  set saleType(val) {
    if (val !== this._saleType) {
      this._saleType = val;
    }
  }

  get saleType(): number {
    return this._saleType;
  }

  @Input()
  showAmountColumns: boolean;

  showCompletedOrderStatus: boolean;
  @Input()
  showRepresentationFilter: boolean;
  private baseListUrl = 'orderItem/GetLimitedInformation?';
  @Input()
  isForSelectReadOnly = false;

  constructor() {
    super();
    this.cancelOrderItem = this.cancelOrderItem.bind(this);
    this.editOrderItem = this.editOrderItem.bind(this);
  }

  ngOnInit(): void {
    this.showCompletedOrderStatus = true;
    this.bindColumns();
    if (this.bindCustomDataSources()) {
      this.loadAfterSetFilter = true;
    }
  }

  onSelectedRepresentationChanged(item: any): void {
    this.companyId = item.company.id;
    this.representationId = item.representation.id;
    this.bindCustomDataSources();
  }

  private bindColumns(): void {

    this.columns.push({
      caption: 'کد پیگیری',
      dataField: 'orderNumber',
      allowHeaderFiltering: false,
      width: '100px',
    },{
      caption: 'کد آیتم سفارش',
      dataField: 'id',
      allowHeaderFiltering: false,
      width: '80px',
    },{
      caption: 'کد ملی تحویل گیرنده',
      dataField: 'nationalCode',
      allowHeaderFiltering: false,
      width: '120px',

    },{
      caption: 'شماره موبایل',
      dataField: 'mobileNumber',
      allowHeaderFiltering: false,
      width: '120px',

    },{
      caption: 'نام محصول',
      dataField: 'productTitle',
      allowHeaderFiltering: false,
      width: '150px',

    },{
      caption: 'کد محصول',
      dataField: 'productCode',
      width: '80px',
    },{
      caption: 'نام مرکز',
      dataField: 'companyTitle',
      width: '120px',
    },{
      caption: 'کد نمایندگی',
      dataField: 'representationCode',
      width: '80px',

    },{
      caption: 'نام نمایندگی',
      dataField: 'representationTitle',
      width: '120px',
    });
    this.createCustomButtons();
  }

  public bindCustomDataSources(): boolean {
    let customListUrl = `${this.baseListUrl}`;
    if (this.listForm) {
      if (this.orderId) {
        customListUrl += `orderId=${this.orderId}&`;
      }
      if (this.saleType) {
        customListUrl += `saleType=${this.saleType}&`;
      }
      if (this.showCompletedOrderStatus) {
        customListUrl += `showCompletedOrderStatus=${this.showCompletedOrderStatus}&`;
      }
      if (this.companyId) {
        customListUrl += `companyId=${this.companyId}&`;
      }
      if (this.representationId) {
        customListUrl += `representationId=${this.representationId}&`;
      }
      if (this.recipientNationalCode) {
        customListUrl += `recipientNationalCode=${this.recipientNationalCode}&`;
      }
      if (this.mobileNumber) {
        customListUrl += `mobileNumber=${this.mobileNumber}&`;
      }
      if (this.cardNumber) {
        customListUrl += `cardNumber=${this.cardNumber}&`;
      }
      customListUrl += `fillNotModified=true`;
    }
    let res = false;
    // tslint:disable-next-line:max-line-length
    this.listForm.customListUrl = customListUrl;
    this.listForm.refreshList();
    res = true;
    return res;

  }

  private createCustomButtons(): void {
    this.customRowButtons = {
      width: '250px',
      customButtons: [
        {
          hint: 'لغو سفارش',
          text: 'لغو سفارش',
          width: '95px',
          type: 'danger',
          onClick: this.cancelOrderItem
        },
        {
          hint: 'تغییر سفارش',
          text: 'تغییر سفارش',
          width: '120px',
          type: 'default',
          onClick: this.editOrderItem
        }
      ]
    };
  }

  editOrderItem(e): void {
    const id = e.row.data.id;
    this.confirmBeforeAction('تغییر سفارش', 'آیا از تغییر سفارش اطمینان دارید؟', 'تغییر سفارش').show().then((dialogResult) => {
      if (dialogResult.res === 'ok') {
        this.orderItemEdited.emit(id);
      }
    });
  }

  cancelOrderItem(e): void {
    const id = e.row.data.id;
    this.confirmBeforeAction('لغو سفارش', 'آیا از لغو سفارش اطمینان دارید؟', 'لغو سفارش').show().then((dialogResult) => {
      if (dialogResult.res === 'ok') {
        this.orderItemCanceled.emit(id);
      }
    });
  }
}


