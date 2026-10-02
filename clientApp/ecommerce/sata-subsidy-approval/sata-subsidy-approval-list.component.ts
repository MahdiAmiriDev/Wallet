import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';
import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {SataSubsidyApprovalDetailComponent} from './sata-subsidy-approval-detail.component';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';


@Component({
  selector: 'app-sata-subsidy-approval-list',
  templateUrl: './sata-subsidy-approval-list.component.html'
})
export class SataSubsidyApprovalListComponent extends Selectable implements OnInit {
  @ViewChild('listForm', {static: true}) listForm: BaseList;
  loadAfterSetFilter: boolean;
  selectedItemId: number;
  columns: any[] = [];
  entityName = 'order';
  detailComponent = SataSubsidyApprovalDetailComponent;
  // tslint:disable-next-line:variable-name
  _lastOrderId: number;
  @Input()
  set lastOrderId(val) {
    if (val !== this._lastOrderId) {
      this._lastOrderId = val;
    }
  }

  get lastOrderId(): number {
    return this._lastOrderId;
  }

  // tslint:disable-next-line:variable-name
  _userId: number;
  @Input()
  set userId(val) {
    if (val !== this._userId) {
      this._userId = val;
    }
  }

  get userId(): number {
    return this._userId;
  }

  // tslint:disable-next-line:variable-name
  _membershipType: number;
  @Input()
  set membershipType(val) {
    if (val !== this._membershipType) {
      this._membershipType = val;
    }
  }

  get membershipType(): number {
    return this._membershipType;
  }

  // tslint:disable-next-line:variable-name
  _customerClubId: number;
  @Input()
  set customerClubId(val) {
    if (val !== this._customerClubId) {
      this._customerClubId = val;
    }
  }

  get customerClubId(): number {
    return this._customerClubId;
  }

  // tslint:disable-next-line:variable-name
  _billingAddressId: number;
  @Input()
  set billingAddressId(val) {
    if (val !== this._billingAddressId) {
      this._billingAddressId = val;
    }
  }

  get billingAddressId(): number {
    return this._billingAddressId;
  }

  // tslint:disable-next-line:variable-name
  _shippingAddressId: number;
  @Input()
  set shippingAddressId(val) {
    if (val !== this._shippingAddressId) {
      this._shippingAddressId = val;
    }
  }

  get shippingAddressId(): number {
    return this._shippingAddressId;
  }

  // tslint:disable-next-line:variable-name
  _orderStatusType: number;
  @Input()
  set orderStatusType(val) {
    if (val !== this._orderStatusType) {
      this._orderStatusType = val;
    }
  }

  get orderStatusType(): number {
    return this._orderStatusType;
  }

  // tslint:disable-next-line:variable-name
  _shippingStatusType: number;
  @Input()
  set shippingStatusType(val) {
    if (val !== this._shippingStatusType) {
      this._shippingStatusType = val;
    }
  }

  get shippingStatusType(): number {
    return this._shippingStatusType;
  }

  // tslint:disable-next-line:variable-name
  _rewardPointsHistoryEntryId: number;
  @Input()
  set rewardPointsHistoryEntryId(val) {
    if (val !== this._rewardPointsHistoryEntryId) {
      this._rewardPointsHistoryEntryId = val;
    }
  }

  get rewardPointsHistoryEntryId(): number {
    return this._rewardPointsHistoryEntryId;
  }

  // tslint:disable-next-line:variable-name
  _affiliateId: number;
  @Input()
  set affiliateId(val) {
    if (val !== this._affiliateId) {
      this._affiliateId = val;
    }
  }

  get affiliateId(): number {
    return this._affiliateId;
  }

  // tslint:disable-next-line:variable-name
  _subsidyStatusType: number;
  @Input()
  set subsidyStatusType(val) {
    if (val !== this._subsidyStatusType) {
      this._subsidyStatusType = val;
    }
  }

  get subsidyStatusType(): number {
    return this._subsidyStatusType;
  }

  // tslint:disable-next-line:variable-name
  _subsidyType: number;
  @Input()
  set subsidyType(val) {
    if (val !== this._subsidyType) {
      this._subsidyType = val;
    }
  }

  get subsidyType(): number {
    return this._subsidyType;
  }

  // tslint:disable-next-line:variable-name
  _subsidyId: number;
  @Input()
  set subsidyId(val) {
    if (val !== this._subsidyId) {
      this._subsidyId = val;
    }
  }

  get subsidyId(): number {
    return this._subsidyId;
  }

  private baseListUrl = 'order/GetAwaitingSataSubsidyApproval?';

  constructor(private genericDataService: GenericDataService) {
    super();
    this.verifySataSubsidy = this.verifySataSubsidy.bind(this);
    this.rejectionSataSubsidy = this.rejectionSataSubsidy.bind(this);
    this.verifyBySataService = this.verifyBySataService.bind(this);
  }

  ngOnInit(): void {
    this.bindColumns();
    this.listForm.customListUrl = `${this.baseListUrl}`;
    if (this.bindCustomDataSources()) {
      this.loadAfterSetFilter = true;
    }
  }

  private bindColumns(): void {
    this.columns.push({
      caption: 'کد ملی خریدار',
      dataField: 'nationalCode'
    },{
      caption: 'شماره کارت',
      dataField: 'cardNumber'
    },{
      caption: 'تایید/رد یارانه',
      type: 'buttons',
      buttons: [
        {
          hint: 'تایید یارانه ساتا',
          cssClass: 'customButton',
          icon: 'check',
          onClick: this.verifySataSubsidy,
        },
        {
          hint: 'رد یارانه ساتا',
          cssClass: 'customButton',
          icon: 'close',
          onClick: this.rejectionSataSubsidy,
        }]
    },{
      caption: 'بررسی توسط ساتا',
      type: 'buttons',
      buttons: [
        {
          hint: 'بررسی توسط ساتا',
          cssClass: 'customButton',
          icon: 'sorted',
          onClick: this.verifyBySataService,
        }]
    });
  }

  public bindCustomDataSources(): boolean {
    let customListUrl = `${this.baseListUrl}`;
    if (this.listForm) {
      if (this.lastOrderId) {
        customListUrl += `lastOrderId=${this.lastOrderId}&`;
      }
      if (this.userId) {
        customListUrl += `userId=${this.userId}&`;
      }
      if (this.membershipType) {
        customListUrl += `membershipType=${this.membershipType}&`;
      }
      if (this.customerClubId) {
        customListUrl += `customerClubId=${this.customerClubId}&`;
      }
      if (this.billingAddressId) {
        customListUrl += `billingAddressId=${this.billingAddressId}&`;
      }
      if (this.shippingAddressId) {
        customListUrl += `shippingAddressId=${this.shippingAddressId}&`;
      }
      if (this.orderStatusType) {
        customListUrl += `orderStatusType=${this.orderStatusType}&`;
      }
      if (this.shippingStatusType) {
        customListUrl += `shippingStatusType=${this.shippingStatusType}&`;
      }
      if (this.rewardPointsHistoryEntryId) {
        customListUrl += `rewardPointsHistoryEntryId=${this.rewardPointsHistoryEntryId}&`;
      }
      if (this.affiliateId) {
        customListUrl += `affiliateId=${this.affiliateId}&`;
      }
      if (this.subsidyStatusType) {
        customListUrl += `subsidyStatusType=${this.subsidyStatusType}&`;
      }
      if (this.subsidyType) {
        customListUrl += `subsidyType=${this.subsidyType}&`;
      }
      if (this.subsidyId) {
        customListUrl += `subsidyId=${this.subsidyId}&`;
      }
    }
    let res = false;
    if (this.lastOrderId || this.userId || this.membershipType || this.customerClubId ||
      this.billingAddressId || this.shippingAddressId || this.orderStatusType || this.shippingStatusType ||
      this.rewardPointsHistoryEntryId || this.affiliateId || this.subsidyStatusType || this.subsidyType ||
      this.subsidyId) {

      this.listForm.customListUrl = customListUrl;
      this.listForm.refreshList();
      res = true;
    }
    return res;

  }

  afterInitialDetailComponent(componentInstance: any): void {
    const comp = (componentInstance.instance as SataSubsidyApprovalDetailComponent);

    if (this.lastOrderId) {
      comp.lastOrderId = this.lastOrderId;
    }

    if (this.userId) {
      comp.userId = this.userId;
    }

    if (this.membershipType) {
      comp.membershipType = this.membershipType;
    }

    if (this.customerClubId) {
      comp.customerClubId = this.customerClubId;
    }

    if (this.billingAddressId) {
      comp.billingAddressId = this.billingAddressId;
    }

    if (this.shippingAddressId) {
      comp.shippingAddressId = this.shippingAddressId;
    }

    if (this.orderStatusType) {
      comp.orderStatusType = this.orderStatusType;
    }

    if (this.shippingStatusType) {
      comp.shippingStatusType = this.shippingStatusType;
    }

    if (this.rewardPointsHistoryEntryId) {
      comp.rewardPointsHistoryEntryId = this.rewardPointsHistoryEntryId;
    }

    if (this.affiliateId) {
      comp.affiliateId = this.affiliateId;
    }

    if (this.subsidyStatusType) {
      comp.subsidyStatusType = this.subsidyStatusType;
    }

    if (this.subsidyType) {
      comp.subsidyType = this.subsidyType;
    }

    if (this.subsidyId) {
      comp.subsidyId = this.subsidyId;
    }

  }

  verifySataSubsidy(e): void {
    const orderId = e.row.data.id;
    this.genericDataService.getEntitiesWithAction('Subsidy', 'VerifySataSubsidy?orderId=' + orderId, null)
      .subscribe((response: any) => {
        if (!response.isSuccess) {
          this.showMessage(response.message);
        } else {
          this.listForm.refreshList();
          this.showError(response.message);
        }
      });
  }

  rejectionSataSubsidy(e): void {
    const orderId = e.row.data.id;
    this.genericDataService.getEntitiesWithAction('Subsidy', 'RejectionSataSubsidy?orderId=' + orderId, null)
      .subscribe((response: any) => {
        if (response.isSuccess) {
          this.showMessage(response.message);
        } else {
          this.listForm.refreshList();
          this.showError(response.message);
        }
      });
  }

  verifyBySataService(e): void {
    this.genericDataService.postEntityByUrl('/api/Subsidy/ConfirmSubsidyBySata', {id: e.row.data.id})
      .subscribe(response => {
        if (response.isSuccess) {
          this.showMessage(response.message);
        } else {
          this.listForm.refreshList();
          this.showError(response.message);
        }
      });
  }
}
