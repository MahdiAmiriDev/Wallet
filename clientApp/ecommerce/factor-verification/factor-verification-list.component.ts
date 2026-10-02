import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';
import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {FactorVerificationDetailTabBasedComponent} from './factor-verification-detail-tab-based.component';
import {FactorVerificationConfirmationType} from './factor-verification-enums';

@Component({
  selector: 'app-factor-verification-list',
  templateUrl: './factor-verification-list.component.html'
})
export class FactorVerificationListComponent extends Selectable implements OnInit {
  @ViewChild('listForm', {static: true}) listForm: BaseList;
  loadAfterSetFilter: boolean;
  selectedItemId: number;
  columns: any[] = [];
  entityName = 'factorVerification';
  detailComponent = FactorVerificationDetailTabBasedComponent;
  // tslint:disable-next-line:variable-name
  _factorId: number;
  @Input()
  set factorId(val) {
    if (val !== this._factorId) {
      this._factorId = val;
    }
  }

  get factorId(): number {
    return this._factorId;
  }

  // tslint:disable-next-line:variable-name
  _factorConfirmationType: number;
  @Input()
  set factorConfirmationType(val) {
    if (val !== this._factorConfirmationType) {
      this._factorConfirmationType = val;
    }
  }

  get factorConfirmationType(): number {
    return this._factorConfirmationType;
  }

  // tslint:disable-next-line:variable-name
  _factorVerificationConfigurationRoleId: number;
  @Input()
  set factorVerificationConfigurationRoleId(val) {
    if (val !== this._factorVerificationConfigurationRoleId) {
      this._factorVerificationConfigurationRoleId = val;
    }
  }

  get factorVerificationConfigurationRoleId(): number {
    return this._factorVerificationConfigurationRoleId;
  }

  private baseListUrl = 'factorVerification/GetByCustomFilterForList?';

  constructor() {
    super();
  }

  ngOnInit(): void {
    this.listForm.customListUrl = 'factorVerification/GetForConfirmList?';
    this.bindColumns();
    if (this.bindCustomDataSources()) {
      this.loadAfterSetFilter = true;
    }
  }

  private bindColumns(): void {
    this.columns.push({
      caption: 'فاکتور',
      dataField: 'factorCode',
      allowHeaderFiltering: false,
    },{
      caption: 'سود(%)',
      dataField: 'profitPercentage',
      width: '70px',
    },{
      caption: 'تخفیف(%)',
      dataField: 'discountPercentage',
      width: '80px',
    },{
      width: '100px',
      caption: 'مرکز',
      allowHeaderFiltering: false,
      dataField: 'companyTitle'
    },{
      width: '150px',
      caption: 'نمایندگی',
      allowHeaderFiltering: false,
      dataField: 'representationTitle'
    },{
      caption: 'قرارداد',
      dataField: 'saleConfigurationTitle'
    },{
      width: '150px',
      caption: 'دسته مالیاتی',
      dataField: 'taxCategoryTitle'
    },{
      width: '90px',
      caption: 'وضعیت',
      dataField: 'factorConfirmationTypeTitle'
    },{
      caption: 'تاریخ',
      dataField: 'factorDate',
      cellTemplate: 'jalaliDate',
      allowHeaderFiltering: false,
      width: '120px'
    });

  }

  public bindCustomDataSources(): boolean {
    let customListUrl = `${this.baseListUrl}`;
    if (this.listForm) {
      if (this.factorId) {
        customListUrl += `factorId=${this.factorId}&`;
      }
      if (this.factorConfirmationType) {
        customListUrl += `factorConfirmationType=${this.factorConfirmationType}&`;
      }
      if (this.factorVerificationConfigurationRoleId) {
        customListUrl += `factorVerificationConfigurationRoleId=${this.factorVerificationConfigurationRoleId}&`;
      }
    }
    let res = false;
    if (this.factorId || this.factorConfirmationType || this.factorVerificationConfigurationRoleId) {
      this.listForm.customListUrl = customListUrl;
      this.listForm.refreshList();
      res = true;
    }
    return res;

  }

  afterInitialDetailComponent(componentInstance: any): void {
    const comp = (componentInstance.instance as FactorVerificationDetailTabBasedComponent);
    if (this.factorId) {
      comp.factorId = this.factorId;
    }

    if (this.factorConfirmationType) {
      comp.factorConfirmationType = this.factorConfirmationType;
    }

    if (this.factorVerificationConfigurationRoleId) {
      comp.factorVerificationConfigurationRoleId = this.factorVerificationConfigurationRoleId;
    }

  }

  /*
  متدی که هنگام لود کردن هر رکورد در جدول صدا زده میشود
  برای رنگی کردن هر رکورد مورد استفاده قرار میگیرد
  بررسی های فاکتور تایید شده سبز و رد شده قرمز میشوند
   */
  onListRowPrepared(e): void {
    if (e.rowType === 'data') {
      if (e.data.factorConfirmationType === FactorVerificationConfirmationType.Confirm) {
        e.rowElement.bgColor = '#60d394';
        e.rowElement.className = e.rowElement.className.replace('dx-row-alt', '');
      } else if (e.data.factorConfirmationType === FactorVerificationConfirmationType.Reject) {
        e.rowElement.bgColor = '#ee6050';
        e.rowElement.className = e.rowElement.className.replace('dx-row-alt', '');
      }
    }
  }
}
