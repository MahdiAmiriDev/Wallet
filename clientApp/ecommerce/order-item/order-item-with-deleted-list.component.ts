import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';
import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {OrderItemDetailComponent} from './order-item-detail.component';


@Component({
  selector: 'app-order-item-with-deleted-list',
  templateUrl: './order-item-with-deleted-list.component.html'
})
export class OrderItemWithDeletedListComponent extends Selectable implements OnInit {
  @ViewChild('listForm', {static: true}) listForm: BaseList;
  @Input()
  allowGrouping = false;
  @Input()
  fixHeight = true;
  representationId: number;
  companyId: number;
  columns: any[] = [];
  entityName = 'orderItem';
  detailComponent = OrderItemDetailComponent;
  private baseListUrl = 'orderItem/GetAllWithDeletedOrderItems?';
  // tslint:disable-next-line:variable-name
  _orderId: number;
  @Input()
  set orderId(val) {
    if (val !== this.orderId) {
      this._orderId = val;
      this.bindCustomDataSources();
    }
  }

  get orderId(): number {
    return this._orderId;
  }

  constructor() {
    super();
  }

  ngOnInit(): void {
    this.bindColumns();
    if (this.bindCustomDataSources()) {
      this.loadAfterSetFilter = true;
    }
  }

  private bindColumns(): void {
    this.columns.push({
        caption: 'کد آیتم سفارش',
        dataField: 'id',
        allowHeaderFiltering: false,
        width: '100px',
      }, {
        caption: 'کد محصول',
        dataField: 'productCode',
        allowHeaderFiltering: false,
        width: '80px',
      },
      {
        caption: 'شناسه برنامه ریزی دوره',
        dataField: 'productSalesPlanningId',
        allowHeaderFiltering: false,
        width: '80px',
      }, {
        caption: 'نام محصول',
        dataField: 'productTitle',
        allowHeaderFiltering: false,
        width: '150px',
      }, {
        caption: 'نام مرکز',
        dataField: 'companyTitle',
        allowHeaderFiltering: false,
        width: '120px',
      }, {
        caption: 'نام نمایندگی',
        dataField: 'representationTitle',
        allowHeaderFiltering: false,
        width: '150px',
      }, {
        caption: 'کد نمایندگی',
        dataField: 'representationCode',
        allowHeaderFiltering: false,
        width: '80px',
      }, {
        caption: 'قیمت پایه (ریال)',
        dataField: 'price',
        cellTemplate: 'number',
        width: '100px',
        allowHeaderFiltering: false
      }, {
        caption: 'تعداد',
        dataField: 'quantity',
        allowHeaderFiltering: false,
        width: '50px',
      }, {
        caption: 'قیمت با تخفیف (ریال)',
        dataField: 'priceIncDiscount',
        cellTemplate: 'number',
        width: '100px',
        allowHeaderFiltering: false,
      }, {
        caption: 'مبلغ کارمزد (ریال)',
        dataField: 'wageAmountExcTax',
        cellTemplate: 'number',
        width: '100px',
        allowHeaderFiltering: false,
      }, {
        caption: 'هزینه ارسال (ریال)',
        dataField: 'additionalShippingCharge',
        cellTemplate: 'number',
        width: '100px',
        allowHeaderFiltering: false,
      }, {
        caption: 'مبلغ یارانه (ریال)',
        dataField: 'subsidyPrice',
        cellTemplate: 'number',
        width: '100px',
        allowHeaderFiltering: false,
      }, {
        caption: 'مبلغ مالیات (ریال)',
        dataField: 'taxPrice',
        cellTemplate: 'number',
        width: '100px',
        allowHeaderFiltering: false,
      }, {
        caption: 'مبلغ نهایی (ریال)',
        dataField: 'total',
        cellTemplate: 'number',
        width: '100px',
        allowHeaderFiltering: false,
      }, {
        caption: 'روزهای تشکیل جلسه',
        dataField: 'coursePlanningSchedule',
        allowHeaderFiltering: false,
        width: '120px',
      }, {
        caption: 'حذف شده',
        dataField: 'deleted',
        width: '80px',
        allowHeaderFiltering: false,
      }
      , {
        caption: 'نوع کوپن ',
        dataField: 'vocherTypeTitle',
        width: '100px',
        allowHeaderFiltering: false,
      }, {
        caption: 'شناسه نوع کوپن',
        dataField: 'vocherTypeId',
        allowHeaderFiltering: false,
        width: '120px',
      }, {
        caption: 'شناسه کوپن',
        dataField: 'vocherId',
        width: '80px',
        allowHeaderFiltering: false,
      });
  }

  public bindCustomDataSources(): boolean {
    let customListUrl = `${this.baseListUrl}`;
    if (this.listForm) {
      if (this.orderId) {
        customListUrl += `orderId=${this.orderId}&`;
      }
    }
    let res: boolean;
    this.listForm.customListUrl = customListUrl;
    this.listForm.refreshList();
    res = true;
    return res;

  }

  onListRowPrepared(e): void {
    if (e.rowType === 'data') {
      if (e.data.deleted) {
        e.rowElement.bgColor = '#ee6050';
        e.rowElement.className = e.rowElement.className.replace('dx-row-alt', '');
      }
    }
  }
}
