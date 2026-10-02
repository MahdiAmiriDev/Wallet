import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';
import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {FactorVerificationDetailTabBasedComponent} from '../factor-verification/factor-verification-detail-tab-based.component';


@Component({
  selector: 'app-factor-verifier-description-list',
  templateUrl: './factor-verifier-description-list.component.html'
})
export class FactorVerifierDescriptionListComponent extends Selectable implements OnInit {
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

  private baseListUrl = 'factorVerification/GetFactorVerificationDescriptionForFactor?';

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
      caption: 'بررسی کننده',
      dataField: 'verifierFullName',
      allowHeaderFiltering: false,
      width: '150px'
    }, {
      caption: 'توضیحات',
      dataField: 'description',
      allowHeaderFiltering: false,
    });
  }

  public bindCustomDataSources(): boolean {
    const customListUrl = `${this.baseListUrl}`;
    let res = false;
    if (this.factorId) {

      this.listForm.customListUrl = customListUrl + `factorId=${this.factorId}`;
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
}
