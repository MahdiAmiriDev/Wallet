import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';
import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {InstallmentDetailComponent} from './installment-detail.component';


@Component({
    selector: 'app-installment-list',
    templateUrl: './installment-list.component.html'
})
export class InstallmentListComponent extends Selectable implements OnInit {
    @ViewChild('listForm', {static: true}) listForm: BaseList;
    loadAfterSetFilter: boolean;
    selectedItemId: number;
    columns: any[] = [];
    entityName = 'installment';
    detailComponent = InstallmentDetailComponent;
    _paymentId: number;
    @Input()
    set paymentId(val) {
        if (val !== this._paymentId) {
            this._paymentId = val;
        }
    }

    get paymentId(): number {
        return this._paymentId;
    }

    _paymentStatus: number;
    @Input()
    set paymentStatus(val) {
        if (val !== this._paymentStatus) {
            this._paymentStatus = val;
        }
    }

    get paymentStatus(): number {
        return this._paymentStatus;
    }

    private baseListUrl = 'installment/GetByCustomFilterForList?';

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
            dataField: 'paymentTitle',
            allowHeaderFiltering: false
        });
        this.columns.push({
            caption: 'مبلغ قسط',
            dataField: 'installmentAmount',
            allowHeaderFiltering: false
        });
        this.columns.push({
            caption: 'تاریخ قسط',
            dataField: 'dueDate',
            cellTemplate: 'intDate'
        });
        this.columns.push({
            caption: 'وضعیت پرداخت',
            dataField: 'paymentStatusTitle'
        });
    }

    public bindCustomDataSources() {
        let customListUrl = `${this.baseListUrl}`;
        if (this.listForm) {
            if (this.paymentId) {
                customListUrl += `paymentId=${this.paymentId}&`;
            }
            if (this.paymentStatus) {
                customListUrl += `paymentStatus=${this.paymentStatus}&`;
            }
        }
        let res = false;
        if (this.paymentId || this.paymentStatus) {

            this.listForm.customListUrl = customListUrl;
            this.listForm.refreshList();
            res = true;
        }
        return res;

    }

    afterInitialDetailComponent(componentInstance: any) {
        const comp = (<InstallmentDetailComponent>componentInstance.instance);

        if (this.paymentId) {
            comp.paymentId = this.paymentId;
        }

        if (this.paymentStatus) {
            comp.paymentStatus = this.paymentStatus;
        }

    }
}
