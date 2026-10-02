import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';
import {Component, OnInit, ViewChild} from '@angular/core';
import {PaymentMethodDetailComponent} from './payment-method-detail.component';


@Component({
    selector: 'app-payment-method-list',
    templateUrl: './payment-method-list.component.html'
})
export class PaymentMethodListComponent extends Selectable implements OnInit {
    @ViewChild('listForm', {static: true}) listForm: BaseList;
    loadAfterSetFilter: boolean;
    selectedItemId: number;
    columns: any[] = [];
    entityName = 'paymentMethod';
    detailComponent = PaymentMethodDetailComponent;
    private baseListUrl = 'paymentMethod/GetByCustomFilterForList?';

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
            caption: 'عنوان',
            dataField: 'title'
        });
    }

    public bindCustomDataSources() {
        const customListUrl = `${this.baseListUrl}`;
        if (this.listForm) {
        }

        this.listForm.customListUrl = customListUrl;
        this.listForm.refreshList();

        return false;

    }

    afterInitialDetailComponent(componentInstance: any) {
        const comp = (<PaymentMethodDetailComponent>componentInstance.instance);

    }
}
