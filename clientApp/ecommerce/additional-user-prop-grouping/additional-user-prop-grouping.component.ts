import {Component, OnInit, ViewChild} from '@angular/core';
import {customAnimations} from '../../../shared/animations/custom-animations';
import {AppEntityGroupingListComponent} from '../../tms/app-entity-grouping/app-entity-grouping-list.component';
import {YashilCustomButton} from '../../../shared/base/models/yasil-custom-button';
import {forkJoin} from 'rxjs';
import {TranslateService} from '@ngx-translate/core';
import {BaseEdit} from '../../../shared/base/classes/base-edit';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';

@Component({
    selector: 'app-additional-user-prop-grouping',
    templateUrl: './additional-user-prop-grouping.component.html',
    animations: customAnimations
})
export class AdditionalUserPropGroupingComponent extends BaseEdit implements OnInit {
    @ViewChild('entityGroupingList', {static: true}) entityGroupingList: AppEntityGroupingListComponent;
    columns: any[] = [];
    showAssignUserToGroup = false;
    rowButtons: YashilCustomButton[] = [];
    groupTitle: string;
    groupId: number;
    formLabels: any = {};
    showItemGroupList = false;

    constructor(private genericDataService: GenericDataService,private translate: TranslateService) {
        super(genericDataService);
        this.assignUsers = this.assignUsers.bind(this);

    }

    ngOnInit(): void {
        let result = forkJoin({
            userTitle: this.translate.get('yl.AdditionalUserPropGroup.UserTitle'),
            companyTitle: this.translate.get('yl.AdditionalUserPropGroup.CompanyTitle'),
            representationTitle: this.translate.get('yl.AdditionalUserPropGroup.RepresentationTitle'),
            additionalUserPropItemGroup: this.translate.get('yl.AdditionalUserPropGroup.AdditionalUserPropItemGroup'),
            additionalUserPropAssignToItemGroup: this.translate.get('yl.AdditionalUserPropGroup.AdditionalUserPropAssignToItemGroupBtn')

        });
        result.subscribe(({userTitle, companyTitle, representationTitle,additionalUserPropItemGroup,additionalUserPropAssignToItemGroup}) => {

            this.formLabels.userTitle = userTitle;
            this.formLabels.companyTitle = companyTitle;
            this.formLabels.representationTitle = representationTitle;
            this.formLabels.additionalUserPropItemGroup = additionalUserPropItemGroup;
            this.formLabels.additionalUserPropAssignToItemGroup = additionalUserPropAssignToItemGroup;

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

            this.showItemGroupList = true;
        });
        if(this.entityGroupingList)
        {
            this.entityGroupingList.itemGroupListForm.parentId = 0;
            this.entityGroupingList.itemGroupListForm.bindCustomDataSources();
        }
        this.bindCustomRowButtons();
    }
     bindCustomRowButtons(): void {

        this.rowButtons.push({
            icon: 'bulletlist',
            text: 'انتخاب کاربر',
            hint: 'انتخاب کاربر',
            width: '100px',
            visibleProp: 'allowEdit',
            type: 'default',
            onClick: this.assignUsers
        });
    }
    /*
      اختصاص کاربر به گروه
   */
    private assignUsers(item: any): void {
        if (item && item.data) {
            this.showAssignUserToGroup = true;
            this.groupTitle = item.data.title;
            this.groupId = item.data.id;
        }
    }


    onAssignAffectedChange() {
            this.entityGroupingList.appEntityListForm.refreshList();
    }
}
