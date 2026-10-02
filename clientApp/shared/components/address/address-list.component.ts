import {Selectable} from '../../base/classes/selectable';
import {BaseList} from '../../base/classes/base-list';
import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {AddressDetailComponent} from './address-detail.component';


@Component({
    selector: 'app-address-list',
    templateUrl: './address-list.component.html'
})
export class AddressListComponent extends Selectable implements OnInit {
    @ViewChild('listForm', {static: true}) listForm: BaseList;
    loadAfterSetFilter: boolean;
    selectedItemId: number;
    columns: any[] = [];
    entityName = 'address';
    detailComponent = AddressDetailComponent;
    _accessLevelId: number;
    @Input()
    set accessLevelId(val) {
        if (val !== this._accessLevelId) {
            this._accessLevelId = val;
        }
    }

    get accessLevelId(): number {
        return this._accessLevelId;
    }

    private baseListUrl = 'address/GetByCustomFilterForList?';

    constructor() {
        super();
        this.columns.push({
            caption: 'آدرس',
            dataField: 'address'
        });
        this.columns.push({
            caption: 'شهر',
            dataField: 'cityId'
        });
        this.columns.push({
            caption: 'رایانامه (Email)',
            dataField: 'email'
        });
        this.columns.push({
            caption: 'شماره تلفن',
            dataField: 'telephone'
        });
        this.columns.push({
            caption: 'شماره موبایل',
            dataField: 'mobileNumber'
        });
        this.columns.push({
            caption: 'دور نگار',
            dataField: 'faxNumber'
        });
        this.columns.push({
            caption: 'کد پستی',
            dataField: 'postalCode'
        });

    }

    ngOnInit(): void {
        if (this.bindCustomDataSources()) {
            this.loadAfterSetFilter = true;
        }
    }

    public bindCustomDataSources() {
        let customListUrl = `${this.baseListUrl}`;
        if (this.listForm) {
            if (this.accessLevelId) {
                customListUrl += `accessLevelId=${this.accessLevelId}&`;
            }
        }
        let res = false;
        if (this.accessLevelId) {
            this.listForm.customListUrl = customListUrl;
            this.listForm.refreshList();
            res = true;
        }
        return res;
    }

    afterInitialDetailComponent(componentInstance: any) {
        const comp = (<AddressDetailComponent>componentInstance);

        if (this.accessLevelId) {
            comp.accessLevelId = this.accessLevelId;
        }

    }
}
