import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';
import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {CanceledOrderItemDetailComponent} from './canceled-order-item-detail.component';


@Component({
    selector: 'app-canceled-order-item-list',
    templateUrl: './canceled-order-item-list.component.html'
})
export class CanceledOrderItemListComponent extends Selectable implements OnInit {
    @ViewChild('listForm', {static: true}) listForm: BaseList;
    loadAfterSetFilter: boolean;
    selectedItemId: number;
    columns: any[] = [];
    entityName = 'canceledOrderItem';
    detailComponent = CanceledOrderItemDetailComponent;
    _orderItemId: number;
    @Input()
    set orderItemId(val) {
        if (val !== this._orderItemId) {
            this._orderItemId = val;
        }
    }

    get orderItemId(): number {
        return this._orderItemId;
    }

    private baseListUrl = 'canceledOrderItem/GetByCustomFilterForList?';

    constructor() {
        super();
    }

    ngOnInit(): void {
        this.bindColumns();
        if (this.bindCustomDataSources()) {
            this.loadAfterSetFilter = true;
        }
    }

    private bindColumns() {
        this.columns.push({
            caption: 'آیتم سفارش',
            dataField: 'orderItemTitle'
        });
        this.columns.push({
            caption: 'تایید شده',
            dataField: 'verified'
        });
    }

    public bindCustomDataSources() {
        let customListUrl = `${this.baseListUrl}`;
        if (this.listForm) {
            if (this.orderItemId) {
                customListUrl += `orderItemId=${this.orderItemId}&`;
            }
        }
        let res = false;
        if (this.orderItemId) {

            this.listForm.customListUrl = customListUrl;
            this.listForm.refreshList();
            res = true;
        }
        return res;

    }

    afterInitialDetailComponent(componentInstance: any) {
        const comp = (<CanceledOrderItemDetailComponent>componentInstance.instance);

        if (this.orderItemId) {
            comp.orderItemId = this.orderItemId;
        }

    }
}
