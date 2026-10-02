import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';
import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {FactorItemDetailComponent} from './factor-item-detail.component';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';


@Component({
  selector: 'app-factor-item-list',
  templateUrl: './factor-item-list.component.html'
})
export class FactorItemListComponent extends Selectable implements OnInit {
  @ViewChild('listForm', {static: true}) listForm: BaseList;
  areOtherDeductionsIncludeInSupplierInvoice: boolean = null;
  loadAfterSetFilter: boolean;
  selectedItemId: number;
  columns: any[] = [];
  entityName = 'factorItem';
  groupList: { value: number, title: string }[] = [];
  detailComponent = FactorItemDetailComponent;
  @Input()
  gridHeight: number;
  private baseListUrl = 'factorItem/GetByCustomFilterForList?';

  constructor(private genericDataService: GenericDataService) {
    super();
    this.getGroupTitle = this.getGroupTitle.bind(this);
  }

  // tslint:disable-next-line:variable-name
  _factorId: number;

  get factorId(): number {
    return this._factorId;
  }

  @Input()
  set factorId(val) {
    if (val !== this._factorId) {
      this._factorId = val;
    }
  }

  // tslint:disable-next-line:variable-name
  _orderItemId: number;

  get orderItemId(): number {
    return this._orderItemId;
  }

  @Input()
  set orderItemId(val) {
    if (val !== this._orderItemId) {
      this._orderItemId = val;
    }
  }

  ngOnInit(): void {
    this.checkOtherDeductionsAreIncludedInTheSupplierInvoice();
    if (!this.gridHeight) {
      this.gridHeight = window.innerHeight - 170;
    }
    this.bindColumns();
    if (this.bindCustomDataSources()) {
      this.loadAfterSetFilter = true;
    }
  }

  public bindCustomDataSources(): boolean {
    let customListUrl = `${this.baseListUrl}`;
    if (this.listForm) {
      if (this.factorId) {
        customListUrl += `factorId=${this.factorId}&`;
      }
      if (this.orderItemId) {
        customListUrl += `orderItemId=${this.orderItemId}&`;
      }
    }
    let res = false;
    if (this.factorId || this.orderItemId) {

      this.listForm.customListUrl = customListUrl;
      this.listForm.refreshList();
      res = true;
    }
    return res;

  }

  afterInitialDetailComponent(componentInstance: any): void {
    const comp = (componentInstance.instance as FactorItemDetailComponent);

    if (this.factorId) {
      comp.factorId = this.factorId;
    }

    if (this.orderItemId) {
      comp.orderItemId = this.orderItemId;
    }

  }

  /*
 دریافت عنوان گروه
 هنگامی که لیست آیتمهای فاکتور گروه بندی میشود
 گروه بندی بر اساس آیدی صورت میگیرد و نام نمایشی گروه بندی با این متد نمایش داده میشود
   */
  getGroupTitle(cellInfo): string {
    const groups = this.groupList.filter(x => x.value === cellInfo.value);
    if (groups.length > 0) {
      return groups[0].title;
    } else {
      const currentRowTitle = this.listForm.gridDataSource.items().filter(x => x.key === cellInfo.value)[0]?.items[0]?.title;
      if (currentRowTitle) {
        this.groupList.push({value: cellInfo.value, title: currentRowTitle});
        return currentRowTitle;
      }
      return '-';
    }
  }

  /*
بررسی می کند که آیا سایر کسورات در فاکتور تامین کننده لحاظ شده است یا خیر؟
   */
  checkOtherDeductionsAreIncludedInTheSupplierInvoice(): void {
    this.genericDataService.getCustomEntitiesByUrl('/api/FactorItem/CheckOtherDeductionsAreIncludedInTheSupplierInvoice?factorId=' + this.factorId)
      .subscribe((res: any) => {
        this.areOtherDeductionsIncludeInSupplierInvoice = res;
      });
  }

  private bindColumns(): void {
    this.columns.push({
      width: '150px',
      caption: 'سفارش',
      dataField: 'orderTitle',
      allowGrouping: 0
    }, {
      width: '150px',
      caption: 'کاربر',
      dataField: 'userName',
      allowGrouping: 0
    }, {
      width: '150px',
      caption: 'کد ملی',
      dataField: 'nationalCode',
      allowGrouping: 0
    }, {
      caption: 'نام محصول',
      dataField: 'productId',
      groupIndex: 1,
      customizeText: this.getGroupTitle
    }, {
      caption: 'کد محصول',
      width: '150px',
      dataField: 'productCode',
      allowGrouping: 0
    }, {
      caption: 'کد نمایندگی',
      width: '150px',
      dataField: 'representationCode',
      allowGrouping: 0
    }, {
      caption: 'نام نمایندگی',
      width: '150px',
      dataField: 'representationTitle',
      allowGrouping: 0
    }, {
      caption: 'تعداد',
      width: '150px',
      dataField: 'quantity',
      allowGrouping: 0
    }, {
      width: '150px',
      caption: 'مبلغ پایه(ریال)',
      dataField: 'price',
      cellTemplate: 'number',
      allowGrouping: 0
    }, {
      width: '150px',
      caption: 'تخفیف(ریال)',
      dataField: 'discountAmountExclTax',
      cellTemplate: 'number',
      allowGrouping: 0
    }, {
      width: '150px',
      caption: 'سود(ریال)',
      dataField: 'profitPrice',
      cellTemplate: 'number',
      allowGrouping: 0
    }, {
      width: '150px',
      caption: 'هزینه تبلیغات(ریال)',
      dataField: 'otherDeductionsPrice',
      cellTemplate: 'number',
      allowGrouping: 0
    }, {
      width: '150px',
      caption: 'قیمت با تخفیف(ریال)',
      dataField: 'priceExclDiscountExclProfitExclOtherDeductionsPrice',
      cellTemplate: 'number',
      allowGrouping: 0
    }, {
      width: '150px',
      caption: 'هزینه ارسال(ریال)',
      dataField: 'additionalShippingCharge',
      cellTemplate: 'number',
      allowGrouping: 0
    }, {
      width: '150px',
      caption: 'قیمت با تخفیف بعلاوه هزینه ارسال(ریال)',
      dataField: 'netPriceExclOtherDeductionsPrice',
      cellTemplate: 'number',
      allowGrouping: 0
    }, {
      width: '150px',
      caption: 'مبلغ کل با تخفیف بعلاوه جمع مالیات وعوارض و هزینه ارسال(ریال)',
      dataField: 'discountedPriceSumTaxAmount',
      cellTemplate: 'number',
      allowGrouping: 0
    }, {
      caption: 'تاریخ خرید',
      dataField: 'creationDateInt',
      cellTemplate: 'intDate',
      allowHeaderFiltering: false,
      width: '120px',
      allowGrouping: 0
    }, {
      caption: 'تاریخ تایید',
      dataField: 'paidDateInt',
      cellTemplate: 'intDate',
      allowHeaderFiltering: false,
      width: '120px',
      allowGrouping: 0
    });

  }

}
