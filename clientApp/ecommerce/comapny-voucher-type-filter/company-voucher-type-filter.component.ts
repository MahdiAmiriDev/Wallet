import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {CachedDataService} from '../../../shared/services/cached-data.service';
import {CachedKey} from '../../../shared/enums/enum';
import {UserDataService} from '../../../_services/user-data.service';


@Component({
  selector: 'app-company-voucher-type-filter',
  templateUrl: './company-voucher-type-filter.component.html'
})
export class CompanyVoucherTypeFilterComponent implements OnInit {
  // tslint:disable-next-line:variable-name
  _selectedVoucherTypeId: number;
  // tslint:disable-next-line:variable-name
  _selectedCompanyId: number;

  @Input()
  set selectedCompanyId(val: number) {
    if (val && val !== this._selectedCompanyId) {
      this._selectedCompanyId = val;
      this._companyId = val;
    }
  }

  @Input()
  set selectedVoucherTypeId(val: number) {
    if (val && val !== this._selectedVoucherTypeId) {
      this._selectedVoucherTypeId = val;
      this.voucherTypeId = val;
      this.firstLoad = true;
    }
  }

  @Input()
  showInOneRow = true;
  @Input()
  isRequired: false;
  @Input()
  postId: number;
  voucherTypeId: number;
  @Input()
  voucherTypeLabel = 'نوع کوپن: ';
  @Input()
  companyLabel: 'مرکز';
  selectedData: any = {};
  @Output()
  selectedVoucherTypeChanged: EventEmitter<any> = new EventEmitter<any>();
  @Output()
  selectedCompanyChanged: EventEmitter<any> = new EventEmitter<any>();
  voucherTypeDataSource: any;
  additionalUserProp: any;
  contentHeight: number;
  selectedItemId: number;
  columns: any[] = [];
  entityName = 'representationPerson';
  companyDataSource: any;
  // tslint:disable-next-line:variable-name
  private _companyId: number;
  firstLoad: boolean;

  set companyId(val: number) {
    if (val && this._companyId !== val) {
      this._companyId = val;
    }
  }

  get companyId(): number {
    return this._companyId;
  }

  constructor(private genericDataService: GenericDataService, private cachedDataService: CachedDataService,private userDataService: UserDataService) {
    this.contentHeight = window.innerHeight - 110;
  }

  ngOnInit(): void {
    if (this._selectedVoucherTypeId) {
      this.firstLoad = true;
    }
    this.additionalUserProp = this.userDataService.getAdditionalUserProp();
    if (this.additionalUserProp && this.additionalUserProp.companyId) {
      this.companyId = this.additionalUserProp.companyId;
      this.selectedCompanyChanged.emit(this.companyId);
    }
    this.companyDataSource = this.genericDataService.createCustomDataSourceWithAction('id', 'company', 'getForSelectHasVoucherType');
    if (this.companyId) {
      this.bindDataSources(this.companyId);
    }
  }

  onSelectedCompanyChanged(item: any): void {
    if (item) {
      this.selectedData.company = item.selectedItem;
      if (!this.firstLoad) {
        this.voucherTypeId = null;
      } else {
        this.firstLoad = false;
      }
    }
    this.selectedCompanyChanged.emit(this.selectedData);
    if (item.selectedItem) {
      this.bindDataSources(item.selectedItem.id);
    } else {
      this.voucherTypeDataSource = null;
    }
  }

  private bindDataSources(companyId): void {
    if (companyId) {
      this.voucherTypeDataSource =
        this.genericDataService.createCustomDataSourceWithAction('id', 'vocherType', `GetByCompanyId?companyId=${companyId}`);
    } else {
      this.voucherTypeDataSource = null;
    }
  }

  onSelectedVoucherTypeChanged(item: any): void {
    if (item) {
      this.selectedData.voucherType = item.selectedItem;
      this.selectedVoucherTypeChanged.emit(this.selectedData);
    }else{
      this.selectedData.voucherType = null;
      this.selectedVoucherTypeChanged.emit(this.selectedData);
    }
  }
}
