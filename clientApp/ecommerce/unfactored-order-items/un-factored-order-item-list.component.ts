import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';

@Component({
  selector: 'app-un-factored-order-item-list',
  templateUrl: './un-factored-order-item-list.component.html'
})
export class UnFactoredOrderItemListComponent extends Selectable implements OnInit {
  @ViewChild('listForm', {static: true}) listForm: BaseList;
  selectedItemId: number;
  columns: any[] = [];
  entityName = 'unFactoredOrderItem';
  groupList: { value: number, title: string }[] = [];
  baseListUrl = 'factorItem/getUnFactoredOrderItems?';
  @Input()
  companyId: number;
  @Input()
  representationId: number;


  constructor() {
    super();
    this.getGroupTitle = this.getGroupTitle.bind(this);
  }

  ngOnInit(): void {
    this.bindColumns();
  }

  private bindColumns(): void {
    this.columns.push({
      caption: 'عنوان محصول',
      dataField: 'productId',
      groupIndex: 1,
      customizeText: this.getGroupTitle
    }, {
      caption: 'کد ملی',
      dataField: 'nationalCode',
      allowGrouping: 0
    }, {
      caption: 'نمایندگی(بر اساس قرارداد)',
      dataField: 'representationTitle',
      allowGrouping: 0
    },{
      caption: 'شناسه محصول',
      dataField: 'productId',
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
    },{
      caption: 'استفاده از گروه محصول برای صدور فاکتور',
      dataField: 'useProductGroupForFactor',
      allowGrouping: 0
    },{
      caption: 'استفاده از گروه محصول برای صدور کانورت',
      dataField: 'useProductGroupForConvert',
      allowGrouping: 0
    },{
      caption: 'عنوان گروه',
      dataField: 'productGroupTitle',
      allowGrouping: 0
    },{
      caption: 'پیام فاقد مقدار بودن شناسه گروه',
      dataField: 'productGroupIdIsNullMessage',
      allowGrouping: 0
    });
  }

  public bindCustomDataSources(): boolean {
    let customListUrl = `${this.baseListUrl}`;
    if (this.companyId) {
      customListUrl += `companyId=${this.companyId}&`;
    } else {
      return false;
    }
    if (this.representationId) {
      customListUrl += `representationId=${this.representationId}&`;
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

  /*
  هنگامی که نمایندگی تغییر کند این متد صدا زده میشود
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
  هنگامی که مرکز تغییر میکند این متد صدا زده میشود
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


}
