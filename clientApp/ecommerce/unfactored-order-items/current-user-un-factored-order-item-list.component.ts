import {ChangeDetectorRef, Component, EventEmitter, Input, OnInit, Output, ViewChild} from '@angular/core';
import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';

@Component({
  selector: 'app-current-user-un-factored-order-item-list',
  templateUrl: './current-user-un-factored-order-item-list.component.html'
})
export class CurrentUserUnFactoredOrderItemListComponent extends Selectable implements OnInit {
  @ViewChild('listForm', {static: true}) listForm: BaseList;
  showProductSalesPlanningFilter: boolean = false;
  customButtons: any;
  loadAfterSetFilter: boolean;
  selectedItemId: number;
  columns: any[] = [];
  groupList: { value: number, title: string }[] = [];
  private baseListUrl = 'factorItem/GetCurrentUserUnFactoredOrderItems';
  @Input()
  productSalesPlanningIds: number[] = [];
  @Output() productSalesPlanningItem: EventEmitter<any> = new EventEmitter<any>();

  constructor(private changeDetector: ChangeDetectorRef) {
    super();
    this.getGroupTitle = this.getGroupTitle.bind(this);

  }

  ngOnInit(): void {
    this.bindColumns();
    if (this.bindCustomDataSources()) {
      this.loadAfterSetFilter = true;
    }
  }

  private bindColumns(): void {
    this.columns.push({
      caption: 'عنوان محصول',
      dataField: 'productId',
      groupIndex: 1,
      customizeText: this.getGroupTitle
    }, {
      caption: 'کد سفارش',
      dataField: 'orderTitle',
      allowGrouping: 0
    }, {
      caption: 'کد ملی',
      dataField: 'nationalCode',
      allowGrouping: 0
    }, {
      caption: 'تعداد',
      dataField: 'quantity',
      allowGrouping: 0
    }, {
      caption: 'قیمت با تخفیف',
      dataField: 'priceIncDiscount',
      allowGrouping: 0
    }, {
      caption: 'هرینه ارسال',
      dataField: 'additionalShippingCharge',
      allowGrouping: 0
    }, {
      caption: 'قیمت با تخفیف با هزینه ارسال',
      dataField: 'priceIncDiscountWithAdditionalShippingCharge',
      allowGrouping: 0
    }, {
      caption: 'تاریخ',
      dataField: 'creationDate',
      cellTemplate: 'jalaliDate',
      allowGrouping: 0
    });
  }

  bindCustomButtons(): void {
    this.customButtons = [{
      icon: 'file',
      text: 'فیلتر برنامه ریزی دوره',
      type: 'success',
      key: 'showProductSalesPlanningDialog',
      float: 'right'
    }];
  }

  customButtonClicked(key: any): void {
    if (key === 'showProductSalesPlanningDialog') {
      this.showProductSalesPlanningFilter = true;
    }
  }

  bindCustomDataSources(): boolean {
    this.listForm.getCustomDataSourceByPostMethod = true;
    let customListUrl = `${this.baseListUrl}`;
    if (this.listForm) {
      if (this.productSalesPlanningIds.length > 0) {
        this.listForm.customParameterForPostMethod = this.productSalesPlanningIds;
        this.productSalesPlanningItem.emit(this.productSalesPlanningIds);
      }
    }
    this.listForm.customListUrl = customListUrl;
    this.listForm.refreshList();
    return true;
  }

  /*
دریافت عنوان گروه
هنگامی که لیست آیتمهای فاکتور نشده نمایش داده میشود بر اساس کد کالا گروه بندی میشود
و برای آنکه نام کالا را بجای کد کالا نمایش دهیم از این متد استفاده میکنیم  */
  getGroupTitle(cellInfo): string {
    const groups = this.groupList.filter(x => x.value === cellInfo.value);
    if (groups.length > 0) {
      return groups[0].title;
    } else {
      const currentRowTitle = this.listForm.gridDataSource.items().filter(x => x.key === cellInfo.value)[0]?.items[0]?.productTitle;
      if (currentRowTitle) {
        this.groupList.push({value: cellInfo.value, title: currentRowTitle});
        return currentRowTitle;
      }
      return '-';
    }
  }


  /*   دریافت id برنامه ریزی دوره های انتخاب شده از کامپوننت  un-factored-product-sales-planning */
  getSelectedItemsId(value: any): any {
    if (value.length == 0) {
      this.productSalesPlanningIds = value;
      this.changeDetector.detectChanges();
    } else {
      this.productSalesPlanningIds = value;
      this.changeDetector.detectChanges();
      this.showProductSalesPlanningFilter = false;
      this.bindCustomDataSources();
    }
  }

  /* این متد برسی می کند که آیا دکمه فیلر بر اساس برنامه ریزی دوره را نمایش بدهیم یا نه*/
  showFilterByProductSalesPlanningBtn(showButton: boolean): void {
    if(showButton){
      this.bindCustomButtons();
    }
  }
}
