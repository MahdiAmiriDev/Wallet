import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {BaseEdit} from '../../../shared/base/classes/base-edit';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {DxTreeViewComponent} from 'devextreme-angular/ui/tree-view';
import {TranslateService} from '@ngx-translate/core';
import {forkJoin} from 'rxjs';
import {Entity} from '../../../shared/base/base-data/entity.enum';
import {UserDataService} from '../../../_services/user-data.service';


@Component({
    selector: 'app-representation-detail',
    templateUrl: './representation-detail.component.html'
})
export class RepresentationDetailComponent extends BaseEdit implements OnInit {
    @Input()
    companyId: number;
    @Input()
    founderId: number;
    @Input()
    licenseType: number;
    @Input()
    ownershipType: number;
    addressDataSource: any;
    companyDataSource: any;
    founderDataSource: any;
    licenseTypes: any;
    ownershipTypes: any;
    accessLevels: any[] = [];
    personDataSource: any;
    columns: any[] = [];
    @ViewChild(DxTreeViewComponent) treeView;
    cityDataSource: any;
    additionalUserProp: any;
    lmsMode: boolean = false;


    constructor(private genericDataService: GenericDataService,
                private userDataService: UserDataService,
                private translate: TranslateService) {
        super(genericDataService);
        this.entityName = 'representation';
        this.allowSetCompany = this.allowSetCompany.bind(this);
    }

    ngOnInit(): void {
        super.ngOnInit();
        this.lmsMode = this.userDataService.getSiteConfigurationLmsMode();
        this.entity.address = {};
        if (!this.selectedEntityId && this.companyId) {
            this.entity.companyId = this.companyId;
        }
        if (this.companyId) {
            this.entity.companyId = this.companyId;
        }

        if (this.founderId && this.founderId !== 0) {
            this.entity.founderId = this.founderId;
        }
        if (this.licenseType) {
            this.entity.licenseType = this.licenseType;
        }
        if (this.ownershipType) {
            this.entity.ownershipType = this.ownershipType;
        }
        this.additionalUserProp = this.userDataService.getAdditionalUserProp();
        if (this.additionalUserProp && this.additionalUserProp.companyId) {
            this.companyId = this.additionalUserProp.companyId;
            this.entity.companyId = this.additionalUserProp.companyId;
        }
        this.personDataSource = this.genericDataService.createCustomDataSourceForSelect('id', 'person');
        this.cityDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'city');
        this.addressDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'address');
        this.companyDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'company');
        this.founderDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'person');
        this._genericDataService.getCommonBaseDataForSelect('LicenseType').subscribe(res => this.licenseTypes = res);
        this._genericDataService.getCommonBaseDataForSelect('OwnershipType').subscribe(res => this.ownershipTypes = res);

        let result = forkJoin({
            nameAndLastNameTitle: this.translate.get('NameAndLastName'),
            nationalCodeTitle: this.translate.get('NationalCode'),
        });

        result.subscribe(({nameAndLastNameTitle, nationalCodeTitle}) => {
            this.columns.push({
                caption: nameAndLastNameTitle,
                dataField: 'title'
            });
            this.columns.push({
                caption: nationalCodeTitle,
                dataField: 'nationalCode'
            });
        });
    }

    allowSetCompany(): boolean {
        return this.companyId != null && this.companyId !== undefined || (this.additionalUserProp &&  this.additionalUserProp.companyId);
    }

    syncTreeViewSelection(e): void {
        const component = (e && e.component) || (this.treeView && this.treeView.instance);

        if (!component) {
            return;
        }
    }

    treeView_itemSelectionChanged(e): void {
        if (e.itemData.parentId) {
            this.entity.address.cityId = e.itemData.id;
        }
    }

    selectedPersonChanged(e: any): void {
        // this.entity.founderId = selectedPersonId;
        if (e && e.selectedRowsData && e.selectedRowsData.length > 0) {
            this.entity.founderId = e.selectedRowsData[0].id;
        }
    }

    afterLoadData(res: any): any {
        if (!this.entity.address) {
            this.entity.address = {};
        }
        return super.afterLoadData(res);
    }

    companySelectionChanged(e: any) {
        if (e.itemData) {
            this.entity.companyId = e.itemData.id;
        }
    }
}
