import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';
import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {FactorVerificationConfigurationRoleDetailComponent} from './factor-verification-configuration-role-detail.component';


@Component({
    selector: 'app-factor-verification-configuration-role-list',
    templateUrl: './factor-verification-configuration-role-list.component.html'
})
export class FactorVerificationConfigurationRoleListComponent extends Selectable implements OnInit {
    @ViewChild('listForm', {static: true}) listForm: BaseList;
    loadAfterSetFilter: boolean;
    selectedItemId: number;
    columns: any[] = [];
    entityName = 'factorVerificationConfigurationRole';
    detailComponent = FactorVerificationConfigurationRoleDetailComponent;
  // tslint:disable-next-line:variable-name
    _factorVerificationConfigurationId: number;
    @Input()
    set factorVerificationConfigurationId(val) {
        if (val !== this._factorVerificationConfigurationId) {
            this._factorVerificationConfigurationId = val;
        }
    }

    get factorVerificationConfigurationId(): number {
        return this._factorVerificationConfigurationId;
    }

  // tslint:disable-next-line:variable-name
    _roleId: number;
    @Input()
    set roleId(val) {
        if (val !== this._roleId) {
            this._roleId = val;
        }
    }

    get roleId(): number {
        return this._roleId;
    }

    private baseListUrl = 'factorVerificationConfigurationRole/GetByCustomFilterForList?';

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
        this.columns.push({
            caption: 'تنظیم نقش بررسی فاکتور',
            dataField: 'factorVerificationConfigurationTitle'
        });
        this.columns.push({
            caption: 'نقش',
            dataField: 'roleTitle'
        });
        this.columns.push({
            caption: 'ترتیب بررسی',
            dataField: 'verificationOrder'
        });
    }

    public bindCustomDataSources(): boolean {
        let customListUrl = `${this.baseListUrl}`;
        if (this.listForm) {
            if (this.factorVerificationConfigurationId) {
                customListUrl += `factorVerificationConfigurationId=${this.factorVerificationConfigurationId}&`;
            }
            if (this.roleId) {
                customListUrl += `roleId=${this.roleId}&`;
            }
        }
        let res = false;
        if (this.factorVerificationConfigurationId || this.roleId) {

            this.listForm.customListUrl = customListUrl;
            this.listForm.refreshList();
            res = true;
        }
        return res;

    }

    afterInitialDetailComponent(componentInstance: any): void {
        const comp = (componentInstance.instance as FactorVerificationConfigurationRoleDetailComponent);

        if (this.factorVerificationConfigurationId) {
            comp.factorVerificationConfigurationId = this.factorVerificationConfigurationId;
        }

        if (this.roleId) {
            comp.roleId = this.roleId;
        }

    }
}
