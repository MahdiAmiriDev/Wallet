import {Component, Input, OnInit} from '@angular/core';
import {BaseEdit} from '../../../shared/base/classes/base-edit';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {Entity} from '../../../shared/base/base-data/entity.enum';


@Component({
    selector: 'app-order-detail',
    templateUrl: './order-detail.component.html'
})
export class OrderDetailComponent extends BaseEdit implements OnInit {
    @Input()
    userId: number;
    @Input()
    billingAddressId: number;
    @Input()
    shippingAddressId: number;
    @Input()
    orderStatusType: number;
    @Input()
    shippingStatusType: number;
    @Input()
    paymentStatusType: number;
    @Input()
    paymentMethodType: number;
    @Input()
    rewardPointsHistoryEntryId: number;
    @Input()
    affiliateId: number;
    @Input()
    salesPlanningId: number;
    userDataSource: any;
    billingAddressDataSource: any;
    shippingAddressDataSource: any;
    orderStatusTypes: any;
    shippingStatusTypes: any;
    paymentStatusTypes: any;
    paymentMethodTypes: any;
    rewardPointsHistoryEntryDataSource: any;
    affiliateDataSource: any;
    salesPlanningDataSource: any;
    accessLevels: any[] = [];

    constructor(private genericDataService: GenericDataService) {
        super(genericDataService);
        this.entityName = 'order';
    }

    ngOnInit(): void {
        super.ngOnInit();
        if (this.userId) {
            this.entity.userId = this.userId;
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
        if (this.paymentStatusType) {
            this.entity.paymentStatusType = this.paymentStatusType;
        }
        if (this.paymentMethodType) {
            this.entity.paymentMethodType = this.paymentMethodType;
        }
        if (this.rewardPointsHistoryEntryId) {
            this.entity.rewardPointsHistoryEntryId = this.rewardPointsHistoryEntryId;
        }
        if (this.affiliateId) {
            this.entity.affiliateId = this.affiliateId;
        }
        if (this.salesPlanningId) {
            this.entity.salesPlanningId = this.salesPlanningId;
        }

        this.userDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'user');
        this.billingAddressDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'address');
        this.shippingAddressDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'address');
        this._genericDataService.getCommonBaseDataForSelect('OrderStatusType').subscribe(res => this.orderStatusTypes = res);
        this._genericDataService.getCommonBaseDataForSelect('ShippingStatusType').subscribe(res => this.shippingStatusTypes = res);
        this._genericDataService.getCommonBaseDataForSelect('PaymentStatusType').subscribe(res => this.paymentStatusTypes = res);
        this._genericDataService.getCommonBaseDataForSelect('PaymentMethodType').subscribe(res => this.paymentMethodTypes = res);
        this.rewardPointsHistoryEntryDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'rewardPointsHistory');
        this.affiliateDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'affiliate');
        this.salesPlanningDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'productSalesPlanning');
        this._genericDataService.getEntitiesByEntityNameForSelect(Entity.AccessLevel).subscribe(res => this.accessLevels = res);
    }


}
