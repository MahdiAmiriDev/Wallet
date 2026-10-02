import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';
import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {SaleConfigurationDetailComponent} from './sale-configuration-detail.component';
import {YashilCustomButtonColumn} from '../../../shared/base/classes/yashil-cutom-button-column';


@Component({
  selector: 'app-sale-configuration-list',
  templateUrl: './sale-configuration-list.component.html'
})
export class SaleConfigurationListComponent extends Selectable implements OnInit {
  @ViewChild('listForm', {static: true}) listForm: BaseList;
  loadAfterSetFilter: boolean;
  selectedItemId: number;
  columns: any[] = [];
  entityName = 'saleConfiguration';
  detailComponent = SaleConfigurationDetailComponent;
  private baseListUrl = 'saleConfiguration/GetByCustomFilterForList?';
  customRowButtons: YashilCustomButtonColumn;
  popUpTitle: string;
  showSaleConfigurationPopup: boolean = false;
  showTaxConfigurationPopup: boolean = false;
  showPopUp: boolean = false;
  saleConfigurationId: number;


  constructor() {
    super();
    this.showSaleConfigurationAttribute = this.showSaleConfigurationAttribute.bind(this);
    this.showTaxConfigurationAttribute = this.showTaxConfigurationAttribute.bind(this);
  }

  // tslint:disable-next-line:variable-name
  _representationId: number;

  get representationId(): number {
    return this._representationId;
  }

  @Input()
  set representationId(val) {
    if (val !== this._representationId) {
      this._representationId = val;
    }
  }

  // tslint:disable-next-line:variable-name
  _companyId: number;

  get companyId(): number {
    return this._companyId;
  }

  @Input()
  set companyId(val) {
    if (val !== this._companyId) {
      this._companyId = val;
    }
  }

  // tslint:disable-next-line:variable-name
  _taxCategoryId: number;

  get taxCategoryId(): number {
    return this._taxCategoryId;
  }

  @Input()
  set taxCategoryId(val) {
    if (val !== this._taxCategoryId) {
      this._taxCategoryId = val;
    }
  }

  ngOnInit(): void {
    this.bindColumns();
    this.BindCustomRowButtons();
    if (this.bindCustomDataSources()) {
      this.loadAfterSetFilter = true;
    }
  }

  bindCustomDataSources(): boolean {
    let customListUrl = `${this.baseListUrl}`;
    if (this.listForm) {
      if (this.representationId) {
        customListUrl += `representationId=${this.representationId}&`;
      }
      if (this.companyId) {
        customListUrl += `companyId=${this.companyId}&`;
      }
      if (this.taxCategoryId) {
        customListUrl += `taxCategoryId=${this.taxCategoryId}&`;
      }
    }
    let res = false;
    if (this.representationId || this.companyId || this.taxCategoryId) {
      this.listForm.customListUrl = customListUrl;
      this.listForm.refreshList();
      res = true;
    }
    return res;
  }

  afterInitialDetailComponent(componentInstance: any): void {
    const comp = (componentInstance.instance as SaleConfigurationDetailComponent);

    if (this.representationId) {
      comp.representationId = this.representationId;
    }

    if (this.companyId) {
      comp.companyId = this.companyId;
    }

    if (this.taxCategoryId) {
      comp.taxCategoryId = this.taxCategoryId;
    }

  }

  private bindColumns(): void {
    this.columns.push(
      {
        caption: 'مرکز',
        dataField: 'companyTitle',
        width: '150px'
      }, {
        caption: 'نمایندگی',
        dataField: 'representationTitle',
        width: '150px'
      },
      {
        caption: 'موبایل نمایندگی',
        dataField: 'founderMobileNumber',
        width: '100px',
        allowHeaderFiltering: false,
      },
      {
        caption: 'تلفن نمایندگی',
        dataField: 'founderPhoneNumber',
        width: '100px'
      },
      {
        caption: 'تاریخ شروع',
        dataField: 'fromDate',
        cellTemplate: 'intDate',
        width: '85px'
      }, {
        caption: 'تاریخ پایان',
        dataField: 'toDate',
        cellTemplate: 'intDate',
        width: '85px'
      },
      {
        caption: 'فعال',
        dataField: 'isActive',
        width: '80px'
      });
  }

  BindCustomRowButtons() {
    this.customRowButtons = {
      width: '150px',
      customButtons: [
        {
          hint: 'ویژگی های تنظیم فروش',
          text: 'ویژگی های تنظیم فروش',
          icon: 'bulletlist',
          type: 'default',
          stylingMode: 'contained',
          onClick: this.showSaleConfigurationAttribute,
        }, {
          hint: 'نوع نمایش فاکتور بر اساس دسته بندی مالیاتی',
          text: 'تنظیم دسته بندی مالیاتی',
          icon: 'money',
          type: 'default',
          stylingMode: 'contained',
          onClick: this.showTaxConfigurationAttribute,
        }]
    };
  }

// متد مربوط به نمایش پاپ آپ در حالتی که دکمه ویژگی های تنظیم فروش کلیک شده باشد
  showSaleConfigurationAttribute(e): void {
    this.saleConfigurationId = e.row.data.id;
    this.popUpTitle = 'ویژگی های تنظیم فروش';
    this.showSaleConfigurationPopup = true;
    this.showTaxConfigurationPopup = false
    this.showPopUp = true;
  }
// متد مربوط به نمایش پاپ آپ در حالتی که دکمه تنظیم دسته بندی مالیاتی کلیک شده باشد
  showTaxConfigurationAttribute(e): void {
    this.saleConfigurationId = e.row.data.id;
    this.popUpTitle = 'نوع نمایش فاکتور بر اساس دسته بندی مالیاتی';
    this.showTaxConfigurationPopup = true;
    this.showSaleConfigurationPopup = false;
    this.showPopUp = true;

  }
}
