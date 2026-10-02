import {Selectable} from '../../../shared/base/classes/selectable';
import {Component, ComponentFactoryResolver, Input, OnInit, ViewChild} from '@angular/core';
import {YashilCustomButtonColumn} from '../../../shared/base/classes/yashil-cutom-button-column';
import {PaymentListComponent} from '../payment/payment-list.component';
import {DetailComponentDirective} from '../../../shared/base/base-list-form/detail-component.directive';
import {OrderStatusTypeEnum} from './order-status-enums';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import * as moment from 'jalali-moment';
import {BaseList} from '../../../shared/base/classes/base-list';
import {FormControl} from '@angular/forms';
import {UserDataService} from "../../../_services/user-data.service";

@Component({
  selector: 'app-order-item-full-info-list',
  templateUrl: './order-item-full-info-list.component.html'
})
export class OrderItemFullInfoListComponent extends Selectable implements OnInit {
  orderCreationDateFilterFromDateDataValue = new FormControl();
  orderCreationDateFilterToDateDataValue = new FormControl();

  @ViewChild('listForm', {static: true}) listForm: BaseList;
  @ViewChild(DetailComponentDirective) componentContainer: DetailComponentDirective;
  /**
   * Load Data After Fill Parameters And Call bindCustomDataSources
   */
  customButtons: any;
  paymentListComponent = PaymentListComponent;
  public showPopUp: boolean;
  public popUpTitle: string;
  representationId: number = null;
  companyId: number = null;
  selectedItemId: number;
  customRowButtons: YashilCustomButtonColumn;
  columns: any[] = [];
  entityName = 'orderItem';
  orderStatusTypes: any;
  // tslint:disable-next-line:variable-name
  _genericDataService: GenericDataService;
  @Input()
  showChangeToItSelf: boolean;
  @Input()
  showAmountColumns: boolean;
  // tslint:disable-next-line:variable-name
  @Input()
  showCompletedOrderStatus: boolean;
  @Input()
  showRepresentationFilter: boolean;
  @Input()
  isForSelectReadOnly = false;
  orderCreationDateFilterFromDate: number;
  orderCreationDateFilterToDate: number;
  additionalUserProp: any;
  datePickerConfig = {
    format: 'jYYYY/jMM/jDD',
    dir: 'rtl',
    locale: moment.locale('fa')
  };
  private baseListUrl = 'orderItem/GetOrderItemFullInfo?';

  constructor(private genericDataService: GenericDataService,
              private userDataService: UserDataService,
              private componentFactoryResolver: ComponentFactoryResolver) {
    super();
    this._genericDataService = genericDataService;
    this.showPaymentsDetail = this.showPaymentsDetail.bind(this);
    this.changeOrderItemToItSelf = this.changeOrderItemToItSelf.bind(this);
    this.exportToExcel = this.exportToExcel.bind(this);
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

  ngOnInit(): void {
    this.additionalUserProp = this.userDataService.getAdditionalUserProp();
    this._genericDataService.getCommonBaseDataForSelect('OrderStatusType').subscribe(res => this.orderStatusTypes = res);
    this.orderStatusType = OrderStatusTypeEnum.Confirm;
    this.orderCreationDateFilterFromDate = this.convertMomentDateToInt(moment().startOf('year'));
    this.orderCreationDateFilterToDate = this.convertMomentDateToInt(moment());
    this.gridHeight = window.innerHeight - 170;
    this.bindColumns();
    if (this.bindCustomDataSources()) {
      this.loadAfterSetFilter = true;
    }
    this.listForm.exportGridToExcel = this.exportToExcel;
  }

  onSelectedRepresentationChanged(item: any): void {
    if (item.company) {
      this.companyId = item.company.id;
    } else {
      this.companyId = null;
    }
    if (item.representation) {
      this.representationId = item.representation.id;
    } else {
      this.representationId = this.additionalUserProp.representationId ?? null;
    }
    this.bindCustomDataSources();
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
      if (this.orderStatusType) {
        customListUrl += `orderStatusType=${this.orderStatusType}&`;
      }
      if (this.orderCreationDateFilterFromDate) {
        customListUrl += `orderCreationDateFromDate=${this.orderCreationDateFilterFromDate}&`;
      }
      if (this.orderCreationDateFilterToDate) {
        customListUrl += `orderCreationDateToDate=${this.orderCreationDateFilterToDate}&`;
      }
    }
    let res = false;
    // tslint:disable-next-line:max-line-length
    if (this.orderId || this.saleType || this.recipientNationalCode || this.mobileNumber || this.orderStatusTypes || this.showCompletedOrderStatus || this.representationId || this.companyId || this.cardNumber) {

      this.listForm.customListUrl = customListUrl;
      this.listForm.refreshList();
      res = true;
    }
    return res;

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

  onListRowPrepared(e): void {
    if (e.rowType === 'data') {
      if (e.data.orderStatusType === OrderStatusTypeEnum.Confirm) {
        e.rowElement.bgColor = '#60d394';
        e.rowElement.className = e.rowElement.className.replace('dx-row-alt', '');
      }
    }
  }

  selectedOrderStatusChanged(item: any): void {
    if (item.selectedItem) {
      this.orderStatusType = item.selectedItem.id;
    } else {
      this.orderStatusType = null;
    }

    this.bindCustomDataSources();
  }

  bindCustomButtons(): void {
    /*
       اگر پارمتر نامعتبر برابر با false بود دکمه ثبت نهایی را به کاربر نمایش دهد
         */
    this.customButtons = [{
      icon: 'file',
      text: 'گزارش عملکرد فروش',
      type: 'success',
      key: 'supplierSalesPerformance',
      float: 'right'
    }];

  }


  /*
  دانلود گزارش عملکرد فروش
     */
  customButtonClicked(key: any): void {
    if (key === 'supplierSalesPerformance') {
      this.exportSupplierSalesPerformanceExcel();
    }
  }

  /*
 این متد عملکرد فروش تامین کنندگان بر اساس فیلتر های فرم به گزارش عملکرد فروش تامین کنندگان فرستاده و گزارش را دانلود می کند
  */
  private exportSupplierSalesPerformanceExcel(): any {
    let url = `${window.location.origin}/api/orderItem/GetSupplierSalesPerformanceExcelFile?`;
    if (this.companyId) {
      url += `companyId=${this.companyId}&`;
    } else {
      this.showError('کاربر گرامی لطفا مرکز را انتخاب نمایید');
      return;
    }
    if (this.orderCreationDateFilterFromDate) {
      url += `orderCreationDateFromDate=${this.orderCreationDateFilterFromDate}&`;
    } else {
      this.showError('کاربر گرامی لطفا فیلتر از تاریخ را انتخاب نمایید');
      return;
    }
    if (this.orderCreationDateFilterToDate) {
      url += `orderCreationDateToDate=${this.orderCreationDateFilterToDate}&`;
    } else {
      this.showError('کاربر گرامی لطفا فیلتر تا تاریخ را انتخاب نمایید');
      return;
    }
    if (this.representationId) {
      url += `representationId=${this.representationId}&`;
    }
    if (this.orderStatusType) {
      url += `orderStatusType=${this.orderStatusType}&`;
    }
    window.open(url, '_blank');
  }

  onSelectedCompanyChanged(item: any): void {
    if (item.company) {
      this.companyId = item.company.id;
      this.representationId = null;
    } else {
      this.companyId = null;
      this.representationId = null;
    }
    this.bindCustomDataSources();
  }

  private bindColumns(): void {


    this.columns.push({
        caption: 'کد پیگیری',
        dataField: 'orderNumber',
        allowHeaderFiltering: false,
        width: '100px',
      }, {
        caption: 'کد آیتم سفارش',
        dataField: 'id',
        allowHeaderFiltering: false,
        width: '80px',
      }, {
        caption: 'شناسه برنامه ریزی دوره',
        dataField: 'productSalesPlanningId',
        allowHeaderFiltering: false,
        width: '80px',
      },
      {
        caption: 'کد ملی تحویل گیرنده',
        dataField: 'nationalCode',
        allowHeaderFiltering: false,
        width: '100px',

      }, {
        caption: 'شماره موبایل دانشجو',
        dataField: 'recipientMobileNumber',
        allowHeaderFiltering: false,
        width: '120px'
      }, {
        caption: 'نام محصول',
        dataField: 'productTitle',
        width: '120px',
      },
      {
        caption: 'گروه محصول',
        dataField: 'productGroup',
        width: '120px',
      },
      {
        caption: 'روزهای تشکیل جلسه',
        dataField: 'coursePlanningSchedule',
        width: '120px',
      },
      {
        caption: 'نام مرکز',
        dataField: 'companyTitle',
        width: '110px',
      });
    if (!this.showCompletedOrderStatus && !this.showChangeToItSelf) {
      this.columns.push({
        caption: 'وضعیت سفارش',
        dataField: 'orderStatus',
        sortOrder: 'desc',
        width: '100px',
      });
    }
    if (!this.showChangeToItSelf) {
      this.columns.push(
        {
          caption: 'کد محصول',
          dataField: 'productCode',
          width: '80px',
        }, {
          caption: 'کد نمایندگی',
          dataField: 'representationCode',
          width: '80px',

        },
        {
          caption: 'نام نمایندگی',
          dataField: 'representationTitle',
          width: '120px',
        }, {
          caption: 'تاریخ ثبت',
          dataField: 'creationDateInt',
          cellTemplate: 'intDate',
          width: '100px'
        }, {
          caption: 'تاریخ تعیین وضعیت',
          dataField: 'orderModificationDate',
          cellTemplate: 'jalaliDate',
          width: '100px',
          allowHeaderFiltering: false,
          hidingPriority: 0,
        });
    }
    if (!this.showChangeToItSelf) {
      this.columns.push({
        caption: 'قیمت پایه (ریال)',
        dataField: 'price',
        cellTemplate: 'number',
        hidingPriority: 0,
        allowHeaderFiltering: false
      }, {
        caption: 'تعداد',
        dataField: 'quantity',
        cellTemplate: 'number',
        hidingPriority: 1,
        allowHeaderFiltering: false
      }, {
        caption: 'قیمت با تخفیف (ریال)',
        dataField: 'priceIncDiscount',
        cellTemplate: 'number',
        hidingPriority: 2,
        allowHeaderFiltering: false,
      }, {
        caption: 'هزینه ارسال (ریال)',
        dataField: 'additionalShippingCharge',
        cellTemplate: 'number',
        hidingPriority: 4,
        allowHeaderFiltering: false,
      });
    }
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
    if (this.showChangeToItSelf) {
      this.customRowButtons = {
        width: '80px',
        customButtons: [
          {
            hint: 'اصلاح پرداخت اضافه سفارش',
            text: 'اصلاح پرداخت اضافه سفارش',
            icon: 'fieldchooser',
            type: 'danger',
            stylingMode: 'contained',
            onClick: this.changeOrderItemToItSelf,
          }]
      };
    }
  }

  /*
  متد خروجی اکسل را اورراید میکنیم
  چون ممکن است رکورد های این جدول زیاد باشد
  لذا خروجی اکسل را در سرور تولید میکنیم
   */
  private exportToExcel(): any {
    let url = `${window.location.origin}/api/orderItem/GetOrderItemFullInfoExcelFile?`;

    if (this.companyId || this.additionalUserProp.companyId) {
      url += `companyId=${this.companyId ?? this.additionalUserProp.companyId}&`;
    } else {
      this.showError('کاربر گرامی لطفا مرکز را انتخاب نمایید');
      return;
    }
    if (this.orderCreationDateFilterFromDate) {
      url += `orderCreationDateFromDate=${this.orderCreationDateFilterFromDate}&`;
    } else {
      this.showError('کاربر گرامی لطفا فیلتر از تاریخ را انتخاب نمایید');
      return;
    }
    if (this.orderCreationDateFilterToDate) {
      url += `orderCreationDateToDate=${this.orderCreationDateFilterToDate}&`;
    } else {
      this.showError('کاربر گرامی لطفا فیلتر تا تاریخ را انتخاب نمایید');
      return;
    }
    if (this.representationId || this.additionalUserProp.representationId) {
      url += `representationId=${this.representationId ?? this.additionalUserProp.representationId}&`;
    }
    if (this.orderStatusType) {
      url += `orderStatusType=${this.orderStatusType}&`;
    }
    window.open(url, '_blank');
  }

  changeOrderItemToItSelf(e): void {
    let orderItemId = e.row.data.id;
    this.confirmBeforeAction('اصلاح پرداخت اضافه سفارش',
      'آیا از اصلاح پرداخت اضافه سفارش اطمینان دارید؟', 'اصلاح پرداخت اضافه').show().then((dialogResult) => {
      if (dialogResult.res === 'ok') {

        this.busyIndicator.setBusy(true);
        this.genericDataService.postEntityByUrl(`api/ModifiedOrderItem/ChangeOrderItemToItSelfAsync?orderItemId=${orderItemId}`, {})
          .subscribe(response => {
            this.busyIndicator.setBusy(false);
            if (!response.isSuccess) {
              this.showError(response.message);
            } else {
              this.showMessage(response.message);
            }
          });
      }
    });
  }
}
