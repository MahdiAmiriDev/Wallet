import {Component, Input, OnInit, ViewChild} from "@angular/core";
import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';
import {FactorPaymentDetailComponent} from "../factor-payment/factor-payment-detail.component";

@Component({
    selector: 'app-factor-payment-summary-list',
    templateUrl: './factor-payment-summary-list.component.html'
})

export class FactorPaymentSummaryListComponent extends Selectable implements OnInit {
    @ViewChild('listForm', {static: true}) listForm: BaseList;
    columns: any[] = [];
    private baseListUrl = 'FactorItem/GetFactorItemPaymentSummary?';
    customListUrl: string;
    _factorId: number;
    loadAfterSetFilter: boolean = true;
    entityName = 'factorItem';

    @Input()
    set factorId(val) {
        if (val !== this._factorId) {
            this._factorId = val;
        }
    }

    get factorId(): number {
        return this._factorId;
    }


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
        this.columns.push(
            {
                caption: 'نوع پرداخت',
                dataField: 'paymentMethodType',
                width: '160px'
            },
            {
                caption: 'تعداد اقساط',
                dataField: 'installmentCount',
                width: '160px'
            }
            , {
                caption: 'مبلغ پرداخت',
                dataField: 'price',
                width: '160px',
                cellTemplate: 'number'
            }
            ,
            {
                caption: 'مجموع مبلغ پرداخت',
                dataField: 'factorItemPaymentTotal',
                width: '200px',
                cellTemplate: 'number'
            }
            , {
                caption: 'درصد پرداخت',
                dataField: 'totalPricePercentage',
                width: '160px',
            }
        );
    }

   public bindCustomDataSources(): boolean {
        this.customListUrl = `${this.baseListUrl}`;
        if (this.factorId) {
            this.customListUrl += `factorId=${this.factorId}&`;
        }

        let res = false;
        if (this.factorId) {
            this.listForm.customListUrl = this.customListUrl;
            this.listForm.refreshList();
            res = true;
        }
        return res;
    }

    afterInitialDetailComponent(componentInstance: any): void {
        const comp = (componentInstance.instance as FactorPaymentDetailComponent);

        if (this.factorId) {
            comp.factorId = this.factorId;
        }

    }


}
