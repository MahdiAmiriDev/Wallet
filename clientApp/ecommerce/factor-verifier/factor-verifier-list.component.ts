import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';
import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {FactorVerificationDetailTabBasedComponent} from '../factor-verification/factor-verification-detail-tab-based.component';
import {JalaliPipe} from '../../../shared/base/core/pipes/jalali.pipe ';


@Component({
  selector: 'app-factor-verifier-list',
  templateUrl: './factor-verifier-list.component.html'
})
export class FactorVerifierListComponent extends Selectable implements OnInit {
  @ViewChild('listForm', {static: true}) listForm: BaseList;
  loadAfterSetFilter: boolean;
  selectedItemId: number;
  columns: any[] = [];
  groupList: { value: number, title: string }[] = [];
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

  private baseListUrl = 'factorVerification/GetFactorVerificationsForFactor?';

  constructor(private pipe: JalaliPipe) {
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
        width: '150px',
        caption: 'تایید کننده',
        dataField: 'verificationOrderTitle',
        sortOrder: 'asc',
        allowHeaderFiltering: false,
        allowSorting: false,
      }, {
        width: '150px',
        caption: 'بررسی کننده',
        dataField: 'verifierFullName',
        allowHeaderFiltering: false,
        allowSorting: false,
      }, {
        width: '150px',
        caption: 'وضعیت',
        dataField: 'factorConfirmationTypeTitle',
        allowHeaderFiltering: false,
        allowSorting: false,
      },
      {
        caption: 'تاریخ بررسی',
        dataField: 'validationDate',
        cellTemplate: 'jalaliDate',
        allowHeaderFiltering: false,
        allowSorting: false,
        width: '150px',
      },
      {
        caption: 'توضیحات',
        dataField: 'description',
        allowHeaderFiltering: false,
        allowSorting: false,
      },
      {
        caption: 'شماره گردش جهت بررسی فاکتور',
        dataField: 'tryNumber',
        groupIndex: 1,
        customizeText: this.getGroupTitle
      }
    );
  }

  public bindCustomDataSources(): boolean {
    const customListUrl = `${this.baseListUrl}` + `factorId=${this.factorId}&`;
    let res = false;
    if (this.factorId) {
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

  getGroupTitle(cellInfo): string {
    const groups = this.groupList.filter(x => x.value === cellInfo.value);
    if (groups.length > 0) {
      return groups[0].title;
    } else {
      const currentRow = this.listForm.gridDataSource.items().filter(x => x.key === cellInfo.value)[0]?.items[0];
      const currentRowTitle = `${currentRow?.tryNumber} - تاریخ ارسال فاکتور جهت بررسی ${this.pipe.transform(currentRow?.creationDate)}`;
      if (currentRowTitle) {
        this.groupList.push({value: cellInfo.value, title: currentRowTitle});
        return currentRowTitle;
      }
      return '-';
    }
  }
}
