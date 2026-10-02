import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';
import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {ContractDetailComponent} from './contract-detail.component';

@Component({
  selector: 'app-contract-list',
  templateUrl: './contract-list.component.html'
})
export class ContractListComponent extends Selectable implements OnInit {
  @ViewChild('listForm', {static: true}) listForm: BaseList;
  loadAfterSetFilter: boolean;
  selectedItemId: number;
  columns: any[] = [];
  entityName = 'contract';
  detailComponent = ContractDetailComponent;
  private baseListUrl = 'contract/GetByCustomFilterForList?';

  constructor() {
    super();
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

  // tslint:disable-next-line:variable-name
  _contractType: number;

  get contractType(): number {
    return this._contractType;
  }

  @Input()
  set contractType(val) {
    if (val !== this._contractType) {
      this._contractType = val;
    }
  }

  // tslint:disable-next-line:variable-name
  _contractStatus: number;

  get contractStatus(): number {
    return this._contractStatus;
  }

  @Input()
  set contractStatus(val) {
    if (val !== this._contractStatus) {
      this._contractStatus = val;
    }
  }

  ngOnInit(): void {
    this.bindColumns();
    if (this.bindCustomDataSources()) {
      this.loadAfterSetFilter = true;
    }
  }

  afterInitialDetailComponent(componentInstance: any): void {
    const comp = (componentInstance.instance as ContractDetailComponent);

    if (this.companyId) {
      comp.companyId = this.companyId;
    }

    if (this.representationId) {
      comp.representationId = this.representationId;
    }

    if (this.taxCategoryId) {
      comp.taxCategoryId = this.taxCategoryId;
    }

    if (this.contractType) {
      comp.contractType = this.contractType;
    }

    if (this.contractStatus) {
      comp.contractStatus = this.contractStatus;
    }

  }

  public bindCustomDataSources(): boolean {
    let customListUrl = `${this.baseListUrl}`;
    if (this.listForm) {
      if (this.companyId) {
        customListUrl += `companyId=${this.companyId}&`;
      }
      if (this.representationId) {
        customListUrl += `representationId=${this.representationId}&`;
      }
      if (this.taxCategoryId) {
        customListUrl += `taxCategoryId=${this.taxCategoryId}&`;
      }
      if (this.contractType) {
        customListUrl += `contractType=${this.contractType}&`;
      }
      if (this.contractStatus) {
        customListUrl += `contractStatus=${this.contractStatus}&`;
      }
    }
    let res = false;
    if (this.companyId || this.representationId || this.taxCategoryId || this.contractType || this.contractStatus) {
      this.listForm.customListUrl = customListUrl;
      this.listForm.refreshList();
      res = true;
    }
    return res;
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
        caption: 'تاریخ انقضای مجوز',
        dataField: 'licenseExpireDate',
        cellTemplate: 'intDate',
        width: '85px'
      }, {
        caption: 'فعال',
        dataField: 'isActive',
        width: '60px'
      }, {
        caption: 'تایید شده',
        dataField: 'confirmed',
        width: '60px'
      });
  }
}

