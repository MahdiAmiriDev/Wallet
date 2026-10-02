import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';
import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {CardAmountHistoryDetailComponent} from './card-amount-history-detail.component';


@Component({
    selector: 'app-card-amount-history-list',
    templateUrl: './card-amount-history-list.component.html'
})
export class CardAmountHistoryListComponent extends Selectable implements OnInit {
    @ViewChild('listForm', {static: true}) listForm: BaseList;
    loadAfterSetFilter: boolean;
    selectedItemId: number;
    columns: any[] = [];
    entityName = 'cardAmountHistory';
    detailComponent = CardAmountHistoryDetailComponent;
    private baseListUrl = 'cardAmountHistory/GetByCustomFilterForList?';
    amount: number;

    _cardInfoId: number;
    @Input()
    set cardInfoId(val) {
        if (val !== this._cardInfoId) {
            this._cardInfoId = val;
        }
    }

    get cardInfoId(): number {
        return this._cardInfoId;
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

    private bindColumns() {
        this.columns.push({
            caption: 'دارنده',
            dataField: 'ownerName',
            allowHeaderFiltering: false
        });
        this.columns.push({
            caption: 'کد ملی دارنده',
            dataField: 'nationalCode',
            allowHeaderFiltering: false
        });
        this.columns.push({
            caption: 'سازمان',
            dataField: 'organizationTitle'
        });
        this.columns.push({
            caption: 'شماره کارت',
            dataField: 'cardNumber',
            allowHeaderFiltering: false
        });
        this.columns.push({
            caption: 'نوع کارت',
            dataField: 'cardType'
        });
        this.columns.push({
            caption: 'ریالی یا اعتباری',
            dataField: 'creditOrCashString'
        });
        this.columns.push({
            caption: 'موجودی قبل از افزایش',
            dataField: 'currentAmount',
            allowHeaderFiltering: false
        });
        this.columns.push({
            caption: 'موجودی بعد از افزایش',
            dataField: 'newAmount',
            allowHeaderFiltering: false
        });
        this.columns.push({
            caption: 'مبلغ شارژ شده',
            dataField: 'amount',
            allowHeaderFiltering: false
        });
        this.columns.push({
            caption: 'تاریخ',
            dataField: 'creationDate',
            cellTemplate: 'jalaliDate',
            allowHeaderFiltering: false
        });
    }

    public bindCustomDataSources() {
        let customListUrl = `${this.baseListUrl}`;
        if (this.listForm) {
            if (this.cardInfoId) {
                customListUrl += `cardInfoId=${this.cardInfoId}&`;
            }
        }
        let res = false;
        if (this.cardInfoId) {

            this.listForm.customListUrl = customListUrl;
            this.listForm.refreshList();
            res = true;
        }
        return res;

    }

    afterInitialDetailComponent(componentInstance: any) {
        const comp = (<CardAmountHistoryDetailComponent>componentInstance.instance);

        if (this.cardInfoId) {
            comp.cardInfoId = this.cardInfoId;
        }

    }
}
