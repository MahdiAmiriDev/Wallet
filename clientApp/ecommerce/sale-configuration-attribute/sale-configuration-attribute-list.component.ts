import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';
import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {SaleConfigurationAttributeDetailComponent} from './sale-configuration-attribute-detail.component';

@Component({
  selector: 'app-sale-configuration-attribute-list',
  templateUrl: './sale-configuration-attribute-list.component.html'
})
export class SaleConfigurationAttributeListComponent extends Selectable implements OnInit {
  @ViewChild('listForm', {static: true}) listForm: BaseList;
  loadAfterSetFilter: boolean;
  selectedItemId: number;
  columns: any[] = [];
  entityName = 'saleConfigurationAttribute';
  detailComponent = SaleConfigurationAttributeDetailComponent;
  _discountType: number;
  @Input()
  set discountType(val) {
    if (val !== this._discountType) {
      this._discountType = val;
    }
  }

  get discountType(): number {
    return this._discountType;
  }

  _discountLimitationType: number;
  @Input()
  set discountLimitationType(val) {
    if (val !== this._discountLimitationType) {
      this._discountLimitationType = val;
    }
  }

  get discountLimitationType(): number {
    return this._discountLimitationType;
  }

  _saleConfigurationId: number;
  @Input()
  set saleConfigurationId(val) {
    if (val !== this._saleConfigurationId) {
      this._saleConfigurationId = val;
    }
  }

  get saleConfigurationId(): number {
    return this._saleConfigurationId;
  }

  private baseListUrl = 'saleConfigurationAttribute/GetByCustomFilterForList?';

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
    this.columns.push(
      {
        caption: 'عنوان',
        dataField: 'title',
        width: '150px'
      },
      {
        caption: 'عنوان قراردارد',
        dataField: 'saleConfigurationTitle',
        width: '150px'
      },
      {
        caption: 'فعال',
        dataField: 'isActive',
        width: '50px'
      },
      {
        caption: 'مرکز',
        dataField: 'companyTitle',
        width: '100px'
      },
      {
        caption: 'نمایندگی',
        dataField: 'representationTitle',
        width: '100px'
      },
      {
        caption: 'باشگاه مشتریان',
        dataField: 'customerClubTitle',
        width: '150px'
      },
      {
        caption: 'محاسبه اقساط بر اساس فرمول',
        dataField: 'expressionBasedInstallmentCount',
        width: '80px'
      },
      {
        caption: 'تعداد اقساط پیش فرض',
        dataField: 'installmentCount',
        width: '80px'
      },
      {
        caption: 'نوع فروش',
        dataField: 'saleTypeTitle',
        width: '80px'
      },
      {
        caption: 'نوع تخفیف',
        dataField: 'discountTypeTitle',
        width: '150px'
      },
      {
        caption: 'درصد تخفیف',
        dataField: 'discountPercentage',
        width: '80px'
      },
      {
        caption: 'درصد سود از تامین کننده',
        dataField: 'profitPercentage',
        width: '80px'
      },
      {
        caption: 'درصد سایر کسورات',
        dataField: 'otherDeductionsPercentage',
        width: '80px'
      },
      {
        caption: 'درصد کارمزد',
        dataField: 'wagePercentage',
        width: '80px'
      },
      {
        caption : 'تعداد اقساط تسویه',
        dataField: 'factorInstallmentCount',
        width: '80px'
      }
      );
  }

  bindCustomDataSources(): boolean {
    let customListUrl = `${this.baseListUrl}`;
    if (this.listForm) {
      if (this.discountType) {
        customListUrl += `discountType=${this.discountType}&`;
      }
      if (this.discountLimitationType) {
        customListUrl += `discountLimitationType=${this.discountLimitationType}&`;
      }
      if (this.saleConfigurationId) {
        customListUrl += `saleConfigurationId=${this.saleConfigurationId}&`;
      }
    }
    let res = false;
    if (this.discountType || this.discountLimitationType || this.saleConfigurationId) {

      this.listForm.customListUrl = customListUrl;
      this.listForm.refreshList();
      res = true;
    }
    return res;

  }

  afterInitialDetailComponent(componentInstance: any): void {
    const comp = (componentInstance.instance as SaleConfigurationAttributeDetailComponent);

    if (this.discountType) {
      comp.discountType = this.discountType;
    }

    if (this.discountLimitationType) {
      comp.discountLimitationType = this.discountLimitationType;
    }

    if (this.saleConfigurationId) {
      comp.saleConfigurationId = this.saleConfigurationId;
    }
  }
}
