import {Component, Input, OnInit} from '@angular/core';
import {BaseEdit} from '../../../shared/base/classes/base-edit';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {Entity} from '../../../shared/base/base-data/entity.enum';
import {FormControl} from '@angular/forms';


@Component({
  selector: 'app-sata-subsidy-approval-detail',
  templateUrl: './sata-subsidy-approval-detail.component.html'
})
export class SataSubsidyApprovalDetailComponent extends BaseEdit implements OnInit {
  creationDateIntDataValue = new FormControl();
  @Input()
  lastOrderId: number;
  @Input()
  userId: number;
  @Input()
  membershipType: number;
  @Input()
  customerClubId: number;
  @Input()
  billingAddressId: number;
  @Input()
  shippingAddressId: number;
  @Input()
  orderStatusType: number;
  @Input()
  shippingStatusType: number;
  @Input()
  rewardPointsHistoryEntryId: number;
  @Input()
  affiliateId: number;
  @Input()
  subsidyStatusType: number;
  @Input()
  subsidyType: number;
  @Input()
  subsidyId: number;
  lastOrderDataSource: any;
  userDataSource: any;
  membershipTypes: any;
  customerClubDataSource: any;
  billingAddressDataSource: any;
  shippingAddressDataSource: any;
  orderStatusTypes: any;
  shippingStatusTypes: any;
  rewardPointsHistoryEntryDataSource: any;
  affiliateDataSource: any;
  subsidyStatusTypes: any;
  subsidyTypes: any;
  subsidyDataSource: any;
  accessLevels: any[] = [];

  constructor(private genericDataService: GenericDataService) {
    super(genericDataService);
    this.entityName = 'order';
  }

  ngOnInit(): void {
    super.ngOnInit();
    if (this.lastOrderId) {
      this.entity.lastOrderId = this.lastOrderId;
    }
    if (this.userId) {
      this.entity.userId = this.userId;
    }
    if (this.membershipType) {
      this.entity.membershipType = this.membershipType;
    }
    if (this.customerClubId) {
      this.entity.customerClubId = this.customerClubId;
    }
    if (this.billingAddressId) {
      this.entity.billingAddressId = this.billingAddressId;
    }
    if (this.shippingAddressId) {
      this.entity.shippingAddressId = this.shippingAddressId;
    }
    if (this.orderStatusType) {
      this.entity.orderStatusType = this.orderStatusType;
    }
    if (this.shippingStatusType) {
      this.entity.shippingStatusType = this.shippingStatusType;
    }
    if (this.rewardPointsHistoryEntryId) {
      this.entity.rewardPointsHistoryEntryId = this.rewardPointsHistoryEntryId;
    }
    if (this.affiliateId) {
      this.entity.affiliateId = this.affiliateId;
    }
    if (this.subsidyStatusType) {
      this.entity.subsidyStatusType = this.subsidyStatusType;
    }
    if (this.subsidyType) {
      this.entity.subsidyType = this.subsidyType;
    }
    if (this.subsidyId) {
      this.entity.subsidyId = this.subsidyId;
    }

    this.lastOrderDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'order');
    this.userDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'user');
    this._genericDataService.getCommonBaseDataForSelect('MembershipType').subscribe(res => this.membershipTypes = res);
    this.customerClubDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'customerClub');
    this.billingAddressDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'address');
    this.shippingAddressDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'address');
    this._genericDataService.getCommonBaseDataForSelect('OrderStatusType').subscribe(res => this.orderStatusTypes = res);
    this._genericDataService.getCommonBaseDataForSelect('ShippingStatusType').subscribe(res => this.shippingStatusTypes = res);
    this.rewardPointsHistoryEntryDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'rewardPointsHistory');
    this.affiliateDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'affiliate');
    this._genericDataService.getCommonBaseDataForSelect('SubsidyStatusType').subscribe(res => this.subsidyStatusTypes = res);
    this._genericDataService.getCommonBaseDataForSelect('SubsidyType').subscribe(res => this.subsidyTypes = res);
    this.subsidyDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'subsidy');
    this._genericDataService.getEntitiesByEntityNameForSelect(Entity.AccessLevel).subscribe(res => this.accessLevels = res);
  }
}
