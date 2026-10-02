import {Component, Input, OnInit} from '@angular/core';
import {BaseEdit} from '../../../shared/base/classes/base-edit';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';


@Component({
    selector: 'app-additional-user-prop-detail',
    templateUrl: './additional-user-prop-detail.component.html'
})
export class AdditionalUserPropDetailComponent extends BaseEdit implements OnInit {
    @Input()
    userId: number;
    @Input()
    companyId: number;
    @Input()
    representationId: number;
    userDataSource: any;
    companyDataSource: any;
    representationDataSource: any;

    constructor(private genericDataService: GenericDataService) {
        super(genericDataService);
        this.entityName = 'additionalUserProp';
    }

    ngOnInit(): void {
        super.ngOnInit();
        if (this.userId) {
            this.entity.userId = this.userId;
        }
        if (this.companyId) {
            this.entity.companyId = this.companyId;
        }
        if (this.representationId) {
            this.entity.representationId = this.representationId;
        }

        this.userDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'user');
        this.companyDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'company');
        this.representationDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'representation');
    }
}
