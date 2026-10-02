import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {BaseEdit} from '../../../shared/base/classes/base-edit';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {Entity} from '../../../shared/base/base-data/entity.enum';
import {UserDataService} from '../../../_services/user-data.service';
import {DxTreeViewComponent} from 'devextreme-angular/ui/tree-view';
import {DxDropDownBoxComponent} from 'devextreme-angular/ui/drop-down-box';


@Component({
    selector: 'app-company-detail',
    templateUrl: './company-detail.component.html'
})
export class CompanyDetailComponent extends BaseEdit implements OnInit {
    _selectedCompanyId: number;
    categoryId: number;
    @Input()
    parentId: number;

    @Input()
    set selectedCompanyId(val: number) {
        if (val && val !== this._selectedCompanyId) {
            this._selectedCompanyId = val;

        }
    }

    @Input()
    establishedLicenseType: number;
    parentDataSource: any;
    addressDataSource: any;
    establishedLicenseTypes: any;
    accessLevels: any[] = [];
    companyGroupDataSource: any[] = [];
    additionalUserProp: any;
    readOnly: boolean = false;
    @Input()
    isRootNodeSelectable: boolean = false;
    isVisible = false;
    type = '';
    @Input()
    selectionMode = 'single';
    firstLoad: boolean;
    @ViewChild(DxTreeViewComponent) treeView;

    disabled: boolean;
    companyDataSource: any;

    constructor(private genericDataService: GenericDataService,
                private userDataService: UserDataService) {
        super(genericDataService);
        this.entityName = 'company';
    }

    ngOnInit(): void {
        super.ngOnInit();
        this.bindCompanyDataSource();
        if (this.parentId) {
            this.entity.parentId = this.parentId;
            this.selectedCompanyId = this.parentId;
        }
        if (this.establishedLicenseType) {
            this.entity.establishedLicenseType = this.establishedLicenseType;
        }
        this.additionalUserProp = this.userDataService.getAdditionalUserProp();
        this.parentDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'company');
        this.addressDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'address');
        this._genericDataService.getCommonBaseDataForSelect('EstablishedLicenseType').subscribe(res => this.establishedLicenseTypes = res);
        this._genericDataService.getEntitiesByEntityNameForSelect(Entity.CompanyGroup)
            .subscribe(res => this.companyGroupDataSource = res);

    }

    bindCompanyDataSource(): void {
        // this.companyDataSource = this.genericDataService.createCustomDataSourceForSelect('id', 'company');
        this.genericDataService.getEntitiesWithAction('company', `GetCurrentUserCompanyWithChildrenForSelect`, null)
            .subscribe((res: any) => {
                this.companyDataSource = res;
            });
    }

    syncTreeViewSelection(e): void {
        const component = (e && e.component) || (this.treeView && this.treeView.instance);

        if (!component) {
            return;
        }
    }

    /*
وقتی دراپ دان تری تغییر می کند
 */
    treeView_itemSelectionChanged(e): void {
        if(e && this.selectedEntityId &&  e.itemData.id && e.itemData.id == this.selectedEntityId)
        {
          this.showError('کاربر گرامی، انتخاب مرکز جاری به عنوان پدر خود امکان پذیر نمی باشد.');
          return;
        }

        if (e && e.itemData.id) {
            this.selectedCompanyId = e.itemData.id;
            this.entity.parentId = e.itemData.id;
        } else {
            this.type = 'error';
            this.isVisible = true;
        }
    }

    //زمانی که کاربر ایتم انتخاب شده را حذف کند این متد فراخوانی می شود
    onSelectedItemRemove(selectedItem: any): void {
        if (selectedItem.value == null) {
        }
    }

    onCompanySelectedChange(e: any) {
        if (e.itemData) {
            this.entity.parentId = e.itemData.id;
        }
    }

    /*
        بستن دراپ دان تری
         */
    closeDDl(ddlCompany: DxDropDownBoxComponent): void {
        if (this.selectionMode === 'single') {
            ddlCompany.instance.close();
        }
    }
}
