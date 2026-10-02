import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';
import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {MerchantTerminalInfoDetailComponent} from './merchant-terminal-info-detail.component';


@Component({
    selector: 'app-merchant-terminal-info-list',
    templateUrl: './merchant-terminal-info-list.component.html'
})
export class MerchantTerminalInfoListComponent extends Selectable implements OnInit {
    @ViewChild('listForm', {static: true}) listForm: BaseList;
    loadAfterSetFilter: boolean;
    selectedItemId: number;
    columns: any[] = [];
    entityName = 'merchantTerminalInfo';
    detailComponent = MerchantTerminalInfoDetailComponent;
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

    private baseListUrl = 'merchantTerminalInfo/GetByCustomFilterForList?';

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
            caption: 'نام پذیرنده',
            dataField: 'merchantTitle'
        });
        this.columns.push({
            caption: 'آی پی',
            dataField: 'ip'
        });
        this.columns.push({
            caption: 'پورت',
            dataField: 'port'
        });
        this.columns.push({
            caption: 'آدرس سایت',
            dataField: 'url'
        });
    }

  public bindCustomDataSources() {
        let customListUrl = `${this.baseListUrl}`;
        if (this.listForm) {
            if (this.merchantId) {
                customListUrl += `merchantId=${this.merchantId}&`;
            }
        }
        let res = false;
        if (this.merchantId) {

            this.listForm.customListUrl = customListUrl;
            this.listForm.refreshList();
            res = true;
        }
        return res;

    }

    afterInitialDetailComponent(componentInstance: any) {
        const comp = (<MerchantTerminalInfoDetailComponent>componentInstance.instance);

        if (this.merchantId) {
            comp.merchantId = this.merchantId;
        }

    }
}
