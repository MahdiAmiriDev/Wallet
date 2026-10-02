import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {BaseEdit} from '../../../shared/base/classes/base-edit';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {AssignableListComponent} from '../../../shared/base/core/components/assignable-list/assignable-list.component';
import {UserDataService} from '../../../_services/user-data.service';


@Component({
    selector: 'app-company-main-category-detail',
    templateUrl: './company-main-category-detail.component.html'
})
export class CompanyMainCategoryDetailComponent extends BaseEdit implements OnInit {
    @ViewChild(AssignableListComponent, {static: true}) assignableListComponent: AssignableListComponent;
    columns: any[] = [];
    @Input()
    companyId: number = 0;
    @Input()
    categoryId: number;
    companyDataSource: any;
    categoryDataSource: any;
    additionalUserProp: any;

    constructor(private genericDataService: GenericDataService
        , private userDataService: UserDataService) {
        super(genericDataService);
        this.entityName = 'companyMainCategory';
    }

    ngOnInit(): void {
        super.ngOnInit();
        this.additionalUserProp = this.userDataService.getAdditionalUserProp();
        if (this.additionalUserProp && this.additionalUserProp.companyId) {
            this.companyId = this.additionalUserProp.companyId;

        }
        if (this.companyId) {
            this.entity.companyId = this.companyId;
            this.assignableListComponent.notAssignedMethodExtraQueryParam = `companyId=${this.companyId}&`;
            this.assignableListComponent.assignedMethodExtraQueryParam = `companyId=${this.companyId}&`;
        }

        if (this.categoryId) {
            this.entity.categoryId = this.categoryId;
        }
        this.columns.push({
            caption: 'عنوان',
            dataField: 'title',
            allowEditing: false
        });
    }

    onSelectedCompanyChanged(selectedItem: any) {
        if (selectedItem) {
            this.companyId = selectedItem.itemData.id;
            this.assignableListComponent.notAssignedMethodExtraQueryParam = `companyId=${selectedItem.itemData.id}&`;
            this.assignableListComponent.assignedMethodExtraQueryParam = `companyId=${selectedItem.itemData.id}&`;
            this.assignableListComponent.bindLists();
        }
    }
}
