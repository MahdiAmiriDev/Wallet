import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';
import {Component, ComponentFactoryResolver, Input, OnInit, ViewChild} from '@angular/core';
import {OrderItemDetailComponent} from './order-item-detail.component';
import {YashilCustomButtonColumn} from '../../../shared/base/classes/yashil-cutom-button-column';
import {DetailComponentDirective} from '../../../shared/base/base-list-form/detail-component.directive';
import {OrderStatusTypeEnum} from '../order-item-full-info/order-status-enums';
import {PaymentListComponent} from '../payment/payment-list.component';
import * as moment from 'jalali-moment';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {FormControl} from '@angular/forms';

@Component({
  selector: 'app-order-item-list',
  templateUrl: './order-item-list.component.html'
})
export class OrderItemListComponent extends Selectable implements OnInit {
  orderCreationDateFilterFromDateDataValue = new FormControl();
  orderCreationDateFilterToDateDataValue = new FormControl();
  paymentListComponent = PaymentListComponent;
  @ViewChild('listForm', {static: true}) listForm: BaseList;
  @ViewChild(DetailComponentDirective) componentContainer: DetailComponentDirective;
  @Input()
  allowGrouping = false;
  representationId: number;
  orderStatusTypes: any;
  companyId: number;
  selectedItemId: number;
  orderCreationDateFilterFromDate: number;
  orderCreationDateFilterToDate: number;
  datePickerConfig = {
    format: 'jYYYY/jMM/jDD',
    dir: 'rtl',
    locale: moment.locale('fa')
  };
  customRowButtons: YashilCustomButtonColumn;
  columns: any[] = [];
  entityName = 'orderItem';
  detailComponent = OrderItemDetailComponent;
  public showPopUp: boolean;
  public popUpTitle: string;
  @Input()
  showAmountColumns: boolean;
  @Input()
  showCompletedOrderStatus: boolean;
  @Input()
  showRepresentationFilter: boolean;
  @Input()
  isForSelectReadOnly = false;
  @Input()
  productSalesPlanningId: number;

  private baseListUrl = 'orderItem/GetLimitedInformation?';

  constructor(private genericDataService: GenericDataService, private componentFactoryResolver: ComponentFactoryResolver) {
    super();
    this.showPaymentsDetail = this.showPaymentsDetail.bind(this);
  }

  // tslint:disable-next-line:variable-name
  _orderId: number;

  get orderId(): number {
    return this._orderId;
  }

  @Input()
  set orderId(val) {
    if (val !== this._orderId) {
      this._orderId = val;
    }
  }

  // tslint:disable-next-line:variable-name
  _mobileNumber: string;

  get mobileNumber(): string {
    return this._mobileNumber;
  }

  @Input()
  set mobileNumber(val) {
    if (val !== this._mobileNumber) {
      this._mobileNumber = val;
      this.bindCustomDataSources();
    }
  }

  // tslint:disable-next-line:variable-name
  _cardNumber: string;

  get cardNumber(): string {
    return this._cardNumber;
  }

  @Input()
  set cardNumber(val) {
    if (val !== this._cardNumber) {
      this._cardNumber = val;
      this.bindCustomDataSources();
    }
  }

  // tslint:disable-next-line:variable-name
  _recipientNationalCode: string;

  get recipientNationalCode(): string {
    return this._recipientNationalCode;
  }

  @Input()
  set recipientNationalCode(val) {
    if (val !== this._recipientNationalCode) {
      this._recipientNationalCode = val;
    }
  }

  // tslint:disable-next-line:variable-name
  _saleType: number;

  get saleType(): number {
    return this._saleType;
  }

  @Input()
  set saleType(val) {
    if (val !== this._saleType) {
      this._saleType = val;
    }
  }

// tslint:disable-next-line:variable-name
  _orderStatusType: number;

  get orderStatusType(): number {
    return this._orderStatusType;
  }

  @Input()
  set orderStatusType(val) {
    if (val !== this._orderStatusType) {
      this._orderStatusType = val;
    }
  }

  ngOnInit(): void {
    this.gridHeight = window.innerHeight - 190;
    this.genericDataService.getCommonBaseDataForSelect('OrderStatusType').subscribe(res => this.orderStatusTypes = res);
    this.orderStatusType = OrderStatusTypeEnum.Confirm;

    this.orderCreationDateFilterFromDate = this.convertMomentDateToInt(moment().startOf('year'));

    this.orderCreationDateFilterToDate = this.convertMomentDateToInt(moment());
    this.bindColumns();
    if (this.bindCustomDataSources()) {
      this.loadAfterSetFilter = true;
    }
  }

  onSelectedRepresentationChanged(item: any): void {
    this.companyId = item.company.id;
    if (item.representation) {
      this.representationId = item.representation.id;
    } else {
      this.representationId = null;
    }
    this.bindCustomDataSources();
  }

  public bindCustomDataSources(): boolean {
    let customListUrl = `${this.baseListUrl}`;
    if (this.listForm) {
      if (this.orderStatusType) {
        customListUrl += `orderStatusType=${this.orderStatusType}&`;
      }
      if (this.orderCreationDateFilterFromDate && this.showRepresentationFilter) {
        customListUrl += `orderCreationDateFromDate=${this.orderCreationDateFilterFromDate}&`;
      }
      if (this.orderCreationDateFilterToDate && this.showRepresentationFilter) {
        customListUrl += `orderCreationDateToDate=${this.orderCreationDateFilterToDate}&`;
      }
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
      if (this.productSalesPlanningId) {
        customListUrl += `productSalesPlanningId=${this.productSalesPlanningId}&`;
      }
    }
    let res = false;
    // tslint:disable-next-line:max-line-length
    if (this.orderStatusTypes || this.companyId || this.representationId) {
      this.listForm.customListUrl = customListUrl;
      this.listForm.refreshList();
      res = true;
    }
    return res;

  }

  onListRowPrepared(e): void {
    if (e.rowType === 'data') {
      if (e.data.orderStatusType === OrderStatusTypeEnum.Confirm) {
        e.rowElement.bgColor = '#60d394';
        e.rowElement.className = e.rowElement.className.replace('dx-row-alt', '');
      }
    }
  }

  showPaymentsDetail(e): void {
    const componentFactory = this.componentFactoryResolver.resolveComponentFactory(this.paymentListComponent);
    const viewContainerRef = this.componentContainer.viewContainerRef;
    viewContainerRef.clear();
    const componentRef = viewContainerRef.createComponent(componentFactory);
    const paymentComponent = (componentRef.instance as PaymentListComponent);
    paymentComponent.orderId = e.row.data.orderId;
    this.popUpTitle = 'لسیت پرداخت های سفارش';
    this.showPopUp = true;
  }

  /*
  هنگامی که فیلتر وضعیت سفارش تغییر کند این متد صدا زده میشود
   */
  selectedOrderStatusChanged(item: any): void {
    if (item && item.selectedItem) {
      this.orderStatusType = item.selectedItem.id;
    } else {
      this.orderStatusType = null;
    }
    this.bindCustomDataSources();
  }

  private bindColumns(): void {
    this.columns.push({
        caption: 'کد پیگیری',
        dataField: 'orderNumber',
        allowHeaderFiltering: false,
        width: '100px',
        allowGrouping: 0
      }, {
        caption: 'کد آیتم سفارش',
        dataField: 'id',
        allowHeaderFiltering: false,
        width: '80px',
        allowGrouping: 0
      }, {
        caption: 'تعداد',
        dataField: 'quantity',
        allowHeaderFiltering: false,
        width: '50px',
        allowGrouping: 0
      },
      {
        caption: 'شناسه برنامه ریزی دوره',
        dataField: 'productSalesPlanningId',
        allowHeaderFiltering: false,
        width: '140px',
        allowGrouping: 0
      },
      {
        caption: 'کد ملی تحویل گیرنده',
        dataField: 'nationalCode',
        allowHeaderFiltering: false,
        width: '120px',
        allowGrouping: 0
      }, {
        caption: 'شماره موبایل',
        dataField: 'mobileNumber',
        allowHeaderFiltering: false,
        width: '120px',
        allowGrouping: 0
      }, {
        caption: 'شماره موبایل دانشجو',
        dataField: 'recipientMobileNumber',
        allowHeaderFiltering: false,
        width: '120px'
      }, {
        caption: 'نام محصول',
        dataField: 'productTitle',
        width: '130px',
        allowGrouping: 0
      }, {
        caption: 'کد محصول',
        dataField: 'productCode',
        width: '90px',
        allowGrouping: 0
      }, {
        caption: 'نام مرکز',
        dataField: 'companyTitle',
        width: '120px',
        allowGrouping: 0
      }, {
        caption: 'کد نمایندگی',
        dataField: 'representationCode',
        width: '90px',
        allowGrouping: 0
      }, {
        caption: 'نام نمایندگی',
        dataField: 'representationTitle',
        width: '150px',
        allowGrouping: 0
      }, {
        caption: 'تاریخ ثبت',
        dataField: 'creationDateInt',
        cellTemplate: 'intDate',
        width: '100px',
        allowGrouping: 0
      }, {
        caption: 'تاریخ تعیین وضعیت',
        dataField: 'orderModificationDate',
        cellTemplate: 'jalaliDate',
        width: '100px',
        allowHeaderFiltering: false,
        allowGrouping: 0
      });
    if (!this.showCompletedOrderStatus) {
      this.columns.push({
        caption: 'وضعیت سفارش',
        dataField: 'orderStatus',
        groupIndex: 1,
        sortOrder: 'desc'
      });
    }
    this.columns.push({
        caption: 'قیمت با تخفیف (ریال)',
        dataField: 'priceIncDiscount',
        cellTemplate: 'number',
        width: '100px',
        allowHeaderFiltering: false,
        allowGrouping: 0
      }, {
        caption: 'هزینه ارسال (ریال)',
        dataField: 'additionalShippingCharge',
        cellTemplate: 'number',
        width: '100px',
        allowHeaderFiltering: false,
        allowGrouping: 0
      }, {
        caption: 'مبلغ یارانه (ریال)',
        dataField: 'subsidyPrice',
        cellTemplate: 'number',
        width: '100px',
        allowHeaderFiltering: false,
        allowGrouping: 0
      }, {
        caption: 'وضعیت یارانه',
        dataField: 'subsidyStatusType',
        allowGrouping: 0
      }, {
        caption: 'نسبت تحویل گیرنده با مالک حکمت کارت',
        dataField: 'relationshipWithBuyerTitle',
        allowGrouping: 0
      },
      {
        caption: 'شهر',
        dataField: 'cityTitle',
        allowGrouping: 0
      });
    this.customRowButtons = {
      width: '150px',
      setAtFirstColumn: true,
      customButtons: [
        {
          hint: 'مشاهده جزییات پرداخت های سفارش',
          text: 'جزییات پرداخت',
          icon: 'fieldchooser',
          type: 'danger',
          stylingMode: 'contained',
          onClick: this.showPaymentsDetail,
        }]
    };
  }
}
