import {Selectable} from '../../../../shared/base/classes/selectable';
import {BaseList} from '../../../../shared/base/classes/base-list';
import {Component, EventEmitter, Input, OnInit, Output, ViewChild} from '@angular/core';
import {PersonEconomicProfileDetailComponent} from './person-economic-profile-detail.component';
import {YashilCustomButtonColumn} from '../../../base/classes/yashil-cutom-button-column';


@Component({
    selector: 'app-person-economic-profile-list',
    templateUrl: './person-economic-profile-list.component.html'
})
export class PersonEconomicProfileListComponent extends Selectable implements OnInit {
    @ViewChild('listForm', {static: true}) listForm: BaseList;
    loadAfterSetFilter: boolean;
    selectedItemId: number;
    columns: any[] = [];
    entityName = 'personEconomicProfile';
    detailComponent = PersonEconomicProfileDetailComponent;
    _personId: number;
    @Input()
    showAddButton = true;
    @Input()
    showEditForm = true;
    @Input()
    allowDelete = true;
    @Input()
    showCustomButton = false;
    customRowButtons: YashilCustomButtonColumn;
    @Output()
    selectedPerson: EventEmitter<any> = new EventEmitter<any>();
    @Input()
    showFilter = false;
    radioButtonItems: any[];
    isLegalPerson: boolean;
    @Input()
    personFullName: string;


    @Input()
    set personId(val) {
        if (val !== this._personId) {
            this._personId = val;
        }
    }

    get personId(): number {
        return this._personId;
    }

    private baseListUrl = 'personEconomicProfile/GetByCustomFilterForList?';
    isSelectedLegalPersonStatus: boolean;

    constructor() {
        super();
        this.selectPerson = this.selectPerson.bind(this);
        this.bindCustomRowButtons = this.bindCustomRowButtons.bind(this);
    }

    ngOnInit(): void {
        this.bindColumns();
        this.bindCustomRowButtons();
        if (this.bindCustomDataSources()) {
            this.loadAfterSetFilter = true;
        }
        this.radioButtonItems = [
            {
                text: 'حقوقی',
                value: true
            },
            {
                text: 'حقیقی',
                value: false
            }
        ];
    }

    private bindColumns(): void {
        this.columns.push(
            {
                caption: 'نام حقوقی',
                dataField: 'name'
            },
            {
                caption: 'کد اقتصادی',
                dataField: 'economicCode'
            },
            {
                caption: 'کد ملی',
                dataField: 'nationalCode'
            },
            {
                caption: 'شماره تلفن',
                dataField: 'telephone'
            },
            {
                caption: 'رایانامه (Email)',
                dataField: 'email'
            },
            {
                caption: 'حقوقی است',
                dataField: 'isLegalPerson'
            },
        );
    }

    bindCustomDataSources(): boolean {
        let customListUrl = `${this.baseListUrl}`;
        if (this.listForm) {
            if (this.personId) {
                customListUrl += `personId=${this.personId}&`;
            }
        }
        if (this.isSelectedLegalPersonStatus) {
            customListUrl += `isLegalPerson=${this.isLegalPerson}&`;
        }
        let res = false;
        if (this.personId || this.isSelectedLegalPersonStatus) {
            this.listForm.customListUrl = customListUrl;
            this.listForm.refreshList();
            res = true;
        }
        return res;

    }

    bindCustomRowButtons(): void {
        if (this.showCustomButton) {
            this.customRowButtons = {
                width: '150px',
                customButtons: [
                    {
                        hint: 'انتخاب',
                        text: 'انتخاب',
                        icon: 'bulletlist',
                        type: 'default',
                        stylingMode: 'contained',
                        onClick: this.selectPerson,
                    }]
            };
        }
    }

    /*
    وقتی که اطلاعات حساب شخص انتخاب می شود
     */
    selectPerson(e) {
        this.selectedPerson.emit(e.row.data);
    }

    afterInitialDetailComponent(componentInstance: any): void {
        const comp = (componentInstance.instance as PersonEconomicProfileDetailComponent);
        if (this.personId) {
            comp.personId = this.personId;
        }
        if (this.personFullName) {
            comp.personFullName = this.personFullName;
        }

    }

    /*
    وقتی کاربر وضعیت حقوقی شخص را انتخاب می کند
     */
    setLegalPersonStatus() {
        this.isSelectedLegalPersonStatus = true;
        this.bindCustomDataSources();
    }
}
