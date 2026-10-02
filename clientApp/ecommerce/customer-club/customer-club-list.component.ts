import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';
import {Component, OnInit, ViewChild} from '@angular/core';
import {CustomerClubDetailComponent} from './customer-club-detail.component';


@Component({
    selector: 'app-customer-club-list',
    templateUrl: './customer-club-list.component.html'
})
export class CustomerClubListComponent extends Selectable implements OnInit {
    @ViewChild('listForm', {static: true}) listForm: BaseList;
    loadAfterSetFilter: boolean;
    selectedItemId: number;
    columns: any[] = [];
    entityName = 'customerClub';
    detailComponent = CustomerClubDetailComponent;

    constructor() {
        super();
    }

    ngOnInit(): void {
        this.bindColumns();
    }

    private bindColumns() {
        this.columns.push({
            caption: 'عنوان',
            dataField: 'title',
            width: '200px'
        });
        this.columns.push({
            caption: 'نوغ فروش',
            dataField: 'saleTypeTitle',
            width: '150px'
        });
        this.columns.push({
            caption: 'نوع مدل ثبت نام',
            dataField: 'registrationModeTypeTitle',
            width: '150px'
        });
        this.columns.push({
            caption: 'مشمول یارانه',
            dataField: 'supportSubsidy',
            width: '80px'
        });
        this.columns.push({
            caption: 'پرداخت با کیف پول',
            dataField: 'payWithWallet',
            width: '80px'
        });
        this.columns.push({
            caption: 'مبتنی بر IPG',
            dataField: 'ipgBased',
            width: '80px'
        });
        this.columns.push({
            caption: 'مبتنی بر سرویس',
            dataField: 'serviceBased',
            width: '80px'
        });

        this.columns.push({
            caption: 'مبتنی بر پایگاه داده محلی',
            dataField: 'isLocalClub',
            width: '80px'
        });


    }

    afterInitialDetailComponent(componentInstance: any) {
        const comp = (<CustomerClubDetailComponent> componentInstance.instance);

    }
}
