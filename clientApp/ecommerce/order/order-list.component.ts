import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';
import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {OrderDetailComponent} from './order-detail.component';


@Component({
    selector: 'app-order-list',
    templateUrl: './order-list.component.html'
})
export class OrderListComponent extends Selectable implements OnInit {
    @ViewChild('listForm', {static: true}) listForm: BaseList;
    loadAfterSetFilter: boolean;
    selectedItemId: number;
    columns: any[] = [];
    entityName = 'order';
    detailComponent = OrderDetailComponent;
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
    _paymentStatusType: number;
    @Input()
    set paymentStatusType(val) {
        if (val !== this._paymentStatusType) {
            this._paymentStatusType = val;
        }
    }

    get paymentStatusType(): number {
        return this._paymentStatusType;
    }

  // tslint:disable-next-line:variable-name
    _paymentMethodType: number;
    @Input()
    set paymentMethodType(val) {
        if (val !== this._paymentMethodType) {
            this._paymentMethodType = val;
        }
    }

    get paymentMethodType(): number {
        return this._paymentMethodType;
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
    _salesPlanningId: number;
    @Input()
    set salesPlanningId(val) {
        if (val !== this._salesPlanningId) {
            this._salesPlanningId = val;
        }
    }

    get salesPlanningId(): number {
        return this._salesPlanningId;
    }

    private baseListUrl = 'order/GetByCustomFilterForList?';

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
            caption: 'شناسه مشتری',
            dataField: 'userTitle',
            width: '10%'
        });
        this.columns.push({
            caption: 'شناسه وضعیت سفارش',
            dataField: 'orderStatusTypeTitle'
        });
        this.columns.push({
            caption: 'شناسه وضعیت پرداخت',
            dataField: 'paymentStatusTypeTitle'
        });
        this.columns.push({
            caption: 'مالیات سفارش',
            dataField: 'orderTax'
        });
        this.columns.push({
            caption: 'مجموع سفارش',
            dataField: 'orderTotal'
        });
        this.columns.push({
            caption: 'نقدی یا اقساطی-نوع پرداخت',
            dataField: 'saleType'
        });
        this.columns.push({
            caption: 'تاریخ و زمان ایجاد سفارش',
            dataField: 'createdOnUtc'
        });
        this.columns.push({
            caption: 'تاریخ و زمان پرداخت شده',
            dataField: 'paidDateUtc'
        });
    }

    public bindCustomDataSources() {
        let customListUrl = `${this.baseListUrl}`;
        if (this.listForm) {
            if (this.userId) {
                customListUrl += `userId=${this.userId}&`;
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
            if (this.paymentStatusType) {
                customListUrl += `paymentStatusType=${this.paymentStatusType}&`;
            }
            if (this.paymentMethodType) {
                customListUrl += `paymentMethodType=${this.paymentMethodType}&`;
            }
            if (this.rewardPointsHistoryEntryId) {
                customListUrl += `rewardPointsHistoryEntryId=${this.rewardPointsHistoryEntryId}&`;
            }
            if (this.affiliateId) {
                customListUrl += `affiliateId=${this.affiliateId}&`;
            }
            if (this.salesPlanningId) {
                customListUrl += `salesPlanningId=${this.salesPlanningId}&`;
            }
        }
        let res = false;
        // tslint:disable-next-line:max-line-length
        if (this.userId || this.billingAddressId || this.shippingAddressId || this.orderStatusType || this.shippingStatusType || this.paymentStatusType || this.paymentMethodType || this.rewardPointsHistoryEntryId || this.affiliateId || this.salesPlanningId) {

            this.listForm.customListUrl = customListUrl;
            this.listForm.refreshList();
            res = true;
        }
        return res;

    }

    afterInitialDetailComponent(componentInstance: any) {
        const comp = (<OrderDetailComponent>componentInstance.instance);

        if (this.userId) {
            comp.userId = this.userId;
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

        if (this.paymentStatusType) {
            comp.paymentStatusType = this.paymentStatusType;
        }

        if (this.paymentMethodType) {
            comp.paymentMethodType = this.paymentMethodType;
        }

        if (this.rewardPointsHistoryEntryId) {
            comp.rewardPointsHistoryEntryId = this.rewardPointsHistoryEntryId;
        }

        if (this.affiliateId) {
            comp.affiliateId = this.affiliateId;
        }

        if (this.salesPlanningId) {
            comp.salesPlanningId = this.salesPlanningId;
        }

    }
}
