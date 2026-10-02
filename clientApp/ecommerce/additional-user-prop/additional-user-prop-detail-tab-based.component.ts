import {Component, OnInit} from '@angular/core';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {Editable} from '../../../shared/base/classes/editable';


@Component({
    selector: 'app-additional-user-prop-detail-tab-based',
    templateUrl: './additional-user-prop-detail-tab-based.component.html'
})
export class AdditionalUserPropDetailTabBasedComponent extends Editable implements OnInit {
    userId: number;
    companyId: number;
    representationId: number;

    allowEditDesc: boolean;
    tabs: any[] = [];

    constructor(private genericDataService: GenericDataService) {
        super();
        this.entityName = 'additionalUserProp';
    }

    ngOnInit(): void {
        if (this.selectedEntityId && this.selectedEntityId !== 0) {
            this.allowEditDesc = true;
        } else {
            this.allowEditDesc = false;
        }
        this.bindTabs();
    }

    rowInserted(insertedRowId: any) {
        this.selectedEntityId = insertedRowId;
        this.allowEditDesc = true;
        this.bindTabs();
    }

    bindTabs() {

        this.tabs = [
            {id: 1, title: 'ويژگي هاي کاربر در سامانه tms', template: 'additionalUserProp'},
        ];

    }


}


