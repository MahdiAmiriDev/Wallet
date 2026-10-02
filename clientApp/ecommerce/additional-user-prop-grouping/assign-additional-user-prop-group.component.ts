import {Component, EventEmitter, Input, OnInit, Output, ViewChild} from '@angular/core';
import {BaseEdit} from '../../../shared/base/classes/base-edit';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {AssignableListComponent} from '../../../shared/base/core/components/assignable-list/assignable-list.component';
import {forkJoin} from 'rxjs';
import {TranslateService} from '@ngx-translate/core';
import {UserDataService} from '../../../_services/user-data.service';


@Component({
    selector: 'app-assign-Additional-user-prop-group',
    templateUrl: './assign-additional-user-prop-group.component.html'
})
export class AssignAdditionalUserPropGroupComponent extends BaseEdit implements OnInit {
    @ViewChild(AssignableListComponent, {static: true}) assignableListComponent: AssignableListComponent;
    @Input()
    groupId:number;
    @Input()
    groupTitle:string;
    columns: any[] = [];
    companyId:number;
    representationId: number;
    categoryId: number;
    userGroupId:number;
    @Output()
    assignAffectedChange = new EventEmitter<boolean>();
    formLabels: any = {};
    userGroupDataSource:any;
    additionalUserProp:any;

    constructor(private genericDataService: GenericDataService,private translate: TranslateService,
                private userDataService: UserDataService) {
        super(genericDataService);
        this.entityName = 'additionalUserProp';
    }
    ngOnInit(): void {
        this.genericDataService.getEntitiesWithAction('ProductSalesPlanningStudent', `GetUserGroups`, null)
            .subscribe(res => {
                this.userGroupDataSource = res;
            });
        this.additionalUserProp = this.userDataService.getAdditionalUserProp();
        if (this.additionalUserProp) {
            if (this.additionalUserProp.companyId) {
                this.companyId = this.additionalUserProp.companyId;
                this.setNotAssignedMethodExtraQueryParam();
            }
            if (this.additionalUserProp.representationId) {
                this.representationId = this.additionalUserProp.representationId;
            }
        }
        let result = forkJoin({
            userTitle: this.translate.get('yl.AdditionalUserPropGroup.UserTitle'),
            companyTitle: this.translate.get('yl.AdditionalUserPropGroup.CompanyTitle'),
            representationTitle: this.translate.get('yl.AdditionalUserPropGroup.RepresentationTitle'),
            groupLabel: this.translate.get('yl.AdditionalUserPropGroup.GroupLabel'),
            usersLabel: this.translate.get('yl.AdditionalUserPropGroup.UsersLabel'),

        });
        result.subscribe(({userTitle, companyTitle, representationTitle,groupLabel,usersLabel}) => {

            this.formLabels.userTitle = userTitle;
            this.formLabels.companyTitle = companyTitle;
            this.formLabels.representationTitle = representationTitle;
            this.formLabels.groupLabel = groupLabel;
            this.formLabels.usersLabel = usersLabel;
        });

        this.columns.push({
            caption: this.formLabels.userTitle,
            dataField: 'userTitle',
            allowEditing: false
        });
        this.columns.push({
            caption: this.formLabels.companyTitle,
            dataField: 'companyTitle',
            allowEditing: false
        });
        this.columns.push({
            caption: this.formLabels.representationTitle,
            dataField: 'representationTitle',
            allowEditing: false
        });
    }
    onSelectedCompanyChanged(item: any) {
        this.companyId = null;
        if (item && item.company) {
            this.companyId = item.company.id;
            this.setNotAssignedMethodExtraQueryParam();
            this.assignableListComponent.bindLists();
        }
    }
    onSelectedRepresentationChanged(item: any) {
        this.representationId = null;
        if (item && item.representation) {
            this.representationId = item.representation.id;
        }
        this.setNotAssignedMethodExtraQueryParam();
        this.assignableListComponent.bindLists();
    }
    onSelectedCategoryChanged(item: any): void {
        this.categoryId = null;
        if (item && item.category) {
            this.categoryId = item.category.id;
        }
        this.setNotAssignedMethodExtraQueryParam();
        this.assignableListComponent.bindLists();
    }
    onSelectedUserGroupChanged(item: any) {
        this.userGroupId = null;
        if (item && item.value) {
            this.userGroupId = item.value;
        }
        else {
            this.userGroupId = null;
        }
        this.setNotAssignedMethodExtraQueryParam();
        this.assignableListComponent.bindLists();
    }
    setNotAssignedMethodExtraQueryParam() {
        this.assignableListComponent.notAssignedMethodExtraQueryParam = '';
        if (this.companyId) {
            this.assignableListComponent.notAssignedMethodExtraQueryParam += `companyId=${this.companyId}&`;
        }
        if (this.representationId) {
            this.assignableListComponent.notAssignedMethodExtraQueryParam += `representationId=${this.representationId}&`;
        }
        if (this.categoryId) {
            this.assignableListComponent.notAssignedMethodExtraQueryParam += `categoryId=${this.categoryId}&`;
        }
        if (this.userGroupId) {
            this.assignableListComponent.notAssignedMethodExtraQueryParam += `userGroupId=${this.userGroupId}&`;
        }
    }

    onAssignAffectedChange(item: boolean) {
        this.assignAffectedChange.emit(item);
    }
}
