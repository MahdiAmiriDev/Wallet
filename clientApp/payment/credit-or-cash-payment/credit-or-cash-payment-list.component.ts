import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';
import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {CreditOrCashPaymentDetailComponent} from './credit-or-cash-payment-detail.component';


@Component({
    selector: 'app-credit-or-cash-payment-list',
    templateUrl: './credit-or-cash-payment-list.component.html'
})
export class CreditOrCashPaymentListComponent extends Selectable implements OnInit {
    @ViewChild('listForm', {static: true}) listForm: BaseList;
    loadAfterSetFilter: boolean;
    selectedItemId: number;
    columns: any[] = [];
    entityName = 'creditOrCashPayment';
    detailComponent = CreditOrCashPaymentDetailComponent;
    _cardId: number;
    @Input()
    set cardId(val) {
        if (val !== this._cardId) {
            this._cardId = val;
        }
    }

    get cardId(): number {
        return this._cardId;
    }

    _merchantId: number;
    @Input()
    set merchantId(val) {
        if (val !== this._merchantId) {
            this._merchantId = val;
        }
    }

    get merchantId(): number {
        return this._merchantId;
    }

    _status: number;
    @Input()
    set status(val) {
        if (val !== this._status) {
            this._status = val;
        }
    }

    get status(): number {
        return this._status;
    }

    private baseListUrl = 'creditOrCashPayment/GetByCustomFilterForList?';

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
            caption: 'شماره پیگیری',
            dataField: 'id',
            allowHeaderFiltering: false
        });
        this.columns.push({
            caption: 'اطلاعات کارت',
            dataField: 'cardTitle',
            allowHeaderFiltering: false
        });
        this.columns.push({
            caption: 'پذیرنده',
            dataField: 'merchantTitle'
        });
        this.columns.push({
            caption: 'شماره سفارش',
            dataField: 'orderId',
            allowHeaderFiltering: false
        });
        this.columns.push({
            caption: 'تاریخ خرید',
            dataField: 'buyDate',
            cellTemplate: 'jalaliDate'
        });
        this.columns.push({
            caption: 'مبلغ کل',
            dataField: 'totalAmount',
            allowHeaderFiltering: false
        });
        this.columns.push({
            caption: 'تعداد اقساط',
            dataField: 'installmentNumber',
            allowHeaderFiltering: false
        });
        this.columns.push({
            caption: 'وضعیت',
            dataField: 'statusTitle'
        });
        this.columns.push({
            caption: 'اعتباری یا ریالی',
            dataField: 'creditOrCashString'
        });
        this.columns.push({
            caption: 'تاریخ بازگشت مبلغ/اعتبار',
            dataField: 'returnedDate',
            cellTemplate: 'jalaliDate'
        });
        this.columns.push({
            caption: 'مبلغ/اعتبار بازگشت داده شده',
            dataField: 'returnedAmount',
            allowHeaderFiltering: false
        });
    }

    public bindCustomDataSources() {
        let customListUrl = `${this.baseListUrl}`;
        if (this.listForm) {
            if (this.cardId) {
                customListUrl += `cardId=${this.cardId}&`;
            }
            if (this.merchantId) {
                customListUrl += `merchantId=${this.merchantId}&`;
            }
            if (this.status) {
                customListUrl += `status=${this.status}&`;
            }
        }
        let res = false;
        if (this.cardId || this.merchantId || this.status) {

            this.listForm.customListUrl = customListUrl;
            this.listForm.refreshList();
            res = true;
        }
        return res;

    }

    afterInitialDetailComponent(componentInstance: any) {
        const comp = (<CreditOrCashPaymentDetailComponent>componentInstance.instance);

        if (this.cardId) {
            comp.cardId = this.cardId;
        }

        if (this.merchantId) {
            comp.merchantId = this.merchantId;
        }

        if (this.status) {
            comp.status = this.status;
        }

    }
}
