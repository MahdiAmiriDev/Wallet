import {Selectable} from '../../../shared/base/classes/selectable';
import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {YashilCustomButtonColumn} from '../../../shared/base/classes/yashil-cutom-button-column';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {OrderStatusTypeEnum} from '../order-item-full-info/order-status-enums';
import * as moment from 'jalali-moment';
import {BaseList} from '../../../shared/base/classes/base-list';
import {FormControl} from '@angular/forms';


@Component({
  selector: 'app-order-full-info-list',
  templateUrl: './order-full-info-list.component.html'
})
export class OrderFullInfoListComponent extends Selectable implements OnInit {
  orderCreationDateFilterFromDateDataValue = new FormControl();
  orderCreationDateFilterToDateDataValue = new FormControl();
  @ViewChild('listForm', {static: true}) listForm: BaseList;
  /**
   * Load Data After Fill Parameters And Call bindCustomDataSources
   */
  public showPopUp: boolean;
  public popUpTitle: string;
  selectedItemId: number;
  customRowButtons: YashilCustomButtonColumn;
  columns: any[] = [];
  entityName = 'order';
  orderStatusTypes: any;
  paymentMethodTypes: any;
  paymentMethodType: any;
  orderId: number;
  showIssueLogWork = false;
  orderCreationDateFilterFromDate: number;
  orderCreationDateFilterToDate: number;
  datePickerConfig = {
    format: 'jYYYY/jMM/jDD',
    dir: 'rtl',
    locale: moment.locale('fa')
  };
  @Input()
  companyId: number;
  @Input()
  representationId: number;
  orderTitle: string;
  @Input()
  showRepresentationFilter: boolean;
  @Input()
  isForSelectReadOnly = false;
  private baseListUrl = 'order/GetByCustomFilterForList?';

  constructor(private genericDataService: GenericDataService) {
    super();
    this.showOrderDetail = this.showOrderDetail.bind(this);
    this.exportToExcel = this.exportToExcel.bind(this);
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
    this.genericDataService.getCommonBaseDataForSelect('OrderStatusType').subscribe(res => this.orderStatusTypes = res);
    this.genericDataService.getCommonBaseDataForSelect('PaymentMethodType').subscribe(res => this.paymentMethodTypes = res);
    this.orderStatusType = OrderStatusTypeEnum.Confirm;
    this.orderCreationDateFilterFromDate = this.convertMomentDateToInt(moment().startOf('year'));
    this.orderCreationDateFilterToDate = this.convertMomentDateToInt(moment());
    this.gridHeight = window.innerHeight - 190;
    this.bindColumns();
    this.listForm.exportGridToExcel = this.exportToExcel;
    if (this.bindCustomDataSources()) {
      this.loadAfterSetFilter = true;
    }
  }

  public bindCustomDataSources(): boolean {
    let customListUrl = `${this.baseListUrl}`;
    if (this.listForm) {
      if (this.orderStatusType) {
        customListUrl += `orderStatusType=${this.orderStatusType}&`;
      }
      if (this.companyId) {
        customListUrl += `companyId=${this.companyId}&`;
      }
      if (this.representationId) {
        customListUrl += `representationId=${this.representationId}&`;
      }
      if (this.orderCreationDateFilterFromDate) {
        customListUrl += `orderCreationDateFromDate=${this.orderCreationDateFilterFromDate}&`;
      }
      if (this.orderCreationDateFilterToDate) {
        customListUrl += `orderCreationDateToDate=${this.orderCreationDateFilterToDate}&`;
      }
      if (this.paymentMethodType) {
        customListUrl += `paymentMethodType=${this.paymentMethodType}&`;
      }
    }
    let res = false;
    if (this.orderStatusTypes || this.companyId || this.representationId || this.paymentMethodType) {
      this.listForm.customListUrl = customListUrl;
      this.listForm.refreshList();
      res = true;
    }
    return res;

  }

  /*
  نمایش جزئیات سفارش
   */
  showOrderDetail(e): void {
    this.orderId = e.row.data.id;
    this.orderTitle = e.row.data.title;
    this.popUpTitle = 'جزئیات سفارش' + e.row.data.title;
    this.showPopUp = true;
  }

  onListRowPrepared(e: any): void {
    if (e && e.rowType === 'data') {
      if (e.data.orderStatusType === OrderStatusTypeEnum.Confirm) {
        e.rowElement.bgColor = '#60d394';
        e.rowElement.className = e.rowElement.className.replace('dx-row-alt', '');
      }
    }
  }

  /*
  هنگامی که فیلتر وضعیت سفارش تغییر کند این متد صدا زده میشود
   */
  onSelectedPaymentMethodTypeChanged(item: any): void {
    if (item && item.selectedItem) {
      this.paymentMethodType = item.selectedItem.id;
    } else {
      this.paymentMethodType = null;
    }
    this.bindCustomDataSources();
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

  /*
  هنگامی که فیلتر نمایندگی تغییر کند این متد صدا زده میشود.
   */
  onSelectedRepresentationChanged(item: any): void {
    if (item.company) {
      this.companyId = item.company.id;
    } else {
      this.companyId = null;
    }
    if (item.representation) {
      this.representationId = item.representation.id;
    } else {
      this.representationId = null;
    }
    this.bindCustomDataSources();
  }

  /*
  هنگامی که فیلتر مرکز تغییر میکنند این متد صدا زده میشود.
   */
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
        dataField: 'title',
        allowHeaderFiltering: false,
        width: '120px',
      }, {
        caption: 'شماره کارت',
        dataField: 'cardNumber',
        allowHeaderFiltering: false,
        allowSorting: false,
        width: '130px',
      }, {
        caption: 'کد ملی تحویل گیرنده',
        dataField: 'nationalCode',
        allowHeaderFiltering: false,
        allowSorting: false,
        width: '110px',
      }, {
        caption: 'شماره موبایل',
        dataField: 'userTitle',
        allowHeaderFiltering: false,
        allowSorting: false,
        width: '110px',

      }, {
        caption: 'شماره موبایل دانشجو',
        dataField: 'recipientMobileNumber',
        allowHeaderFiltering: false,
        allowSorting: false,
        width: '110px'
      }, {
        caption: 'وضعیت سفارش',
        dataField: 'orderStatusTypeTitle',
        allowHeaderFiltering: false,
        width: '130px',
      }, {
        caption: 'نوع یارانه',
        dataField: 'subsidyTypeTitle',
        allowHeaderFiltering: false,
        width: '70px',
      }
      , {
        caption: 'وضعیت وزارت و ساتا',
        width: '130px',
        dataField: 'membershipTypeTitle',
        allowHeaderFiltering: false,
        allowSorting: false,
      },
      {
        caption: 'وضعیت یارانه',
        width: '80px',
        dataField: 'subsidyStatusTypeTitle',
        allowHeaderFiltering: false,
      }, {
        caption: 'تاریخ ثبت',
        dataField: 'creationDateInt',
        cellTemplate: 'intDate',
        sortOrder: 'desc',
        width: '95px',
        allowHeaderFiltering: false,
      }, {
        caption: 'روش پرداخت',
        width: '120px',
        dataField: 'paymentMethods',
        allowHeaderFiltering: false,
        allowSorting: false,
      }
      , {
        caption: 'نسبت تحویل گیرنده با مالک حکمت کارت',
        width: '120px',
        dataField: 'relationshipWithBuyerTitle',
        allowHeaderFiltering: false,
        allowSorting: false,
      }
      , {
        caption: 'شهر',
        width: '120px',
        dataField: 'cityTitle',
        allowHeaderFiltering: false,
        allowSorting: false,
      }
      , {
        caption: 'جنسیت',
        width: '70px',
        dataField: 'genderTitle',
        allowHeaderFiltering: false,
        allowSorting: false,
      }
    );
    this.customRowButtons = {
      width: '140px',
      setAtFirstColumn: true,
      allowExporting: false,
      customButtons: [
        {
          hint: 'مشاهده جزییات سفارش',
          text: 'جزییات سفارش',
          icon: 'fieldchooser',
          type: 'danger',
          stylingMode: 'contained',
          onClick: this.showOrderDetail,
        }]
    };
  }

  /*
  متد خروجی اکسل را اورراید میکنیم
  چون ممکن است رکورد های این جدول زیاد باشد
  لذا خروجی اکسل را در سرور تولید میکنیم
   */
  private exportToExcel(): any {
    let url = `${window.location.origin}/api/orderItem/GetOrderItemFullInfoExcelFile?`;

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
}
