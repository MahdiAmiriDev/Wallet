import {Component, Input, OnInit, ViewChild} from "@angular/core";
import {BaseList} from '../../../shared/base/classes/base-list';
import {FactorPaymentDetailComponent} from "../factor-payment/factor-payment-detail.component";
import {BaseEdit} from "../../../shared/base/classes/base-edit";
import {GenericDataService} from "../../../shared/base/services/generic-data.service";

@Component({
    selector: 'app-factor-micro-payment-list',
    templateUrl: './factor-item-payment-list.component.html'
})


export class FactorItemPaymentListComponent extends BaseEdit implements OnInit {
    @ViewChild('listForm', {static: true}) listForm: BaseList;
    loadAfterSetFilter: boolean;
    selectedItemId: number;
    columns: any[] = [];
    _factorId: number;
    private baseListUrl = 'api/factorItem/getFactorItemPayment?';
    customListUrl: string;

    constructor(private genericDataService: GenericDataService) {
        super(genericDataService);
        this.entityName = 'factorItem';
    }

    @Input()
    set factorId(val) {
        if (val !== this._factorId) {
            this._factorId = val;
        }
    }

    get factorId(): number {
        return this._factorId;
    }


    ngOnInit(): void {
        this.bindColumns();
        if (this.bindCustomDataSources()) {
            this.loadAfterSetFilter = true;
        }
    }

    private bindColumns(): void {
        this.columns.push({
                caption: 'شماره پرداخت',
                dataField: 'paymentId',
                width: '160px'
            }
            , {
                caption: 'کد فاکتور',
                dataField: 'factorCode',
                width: '200px'
            }
            , {
                caption: 'تاریخ پرداخت',
                dataField: 'paidDate',
                width: '160px',
                cellTemplate: 'intDate',
            }
            , {
                caption: 'سهم پرداخت',
                dataField: 'orderItemPayment',
                width: '160px',
                cellTemplate: 'number',
            }
            , {
                caption: 'مبلغ کل پرداخت',
                dataField: 'total',
                width: '160px',
                cellTemplate: 'number',
            }
            , {
                caption: 'تعداد اقساط',
                dataField: 'installmentCount',
                width: '160px'
            }
            , {
                caption: 'نوع پرداخت',
                dataField: 'paymentMethodTitle',
                width: '160px'
            },
        );
    }

    bindCustomDataSources(): any {
        this.genericDataService.getCustomEntitiesByUrl(this.baseListUrl + `factorId=${this.factorId}`)
            .subscribe(
                (res: any) => {
                    if (res) {
                        this.listForm.gridDataSource = res.data;
                    }
                }
            );
    }

    afterInitialDetailComponent(componentInstance: any): void {
        const comp = (componentInstance.instance as FactorPaymentDetailComponent);
        if (this.factorId) {
            comp.factorId = this.factorId;
        }
    }

}
