import {Component, Input, OnInit} from '@angular/core';
import {BaseEdit} from '../../../shared/base/classes/base-edit';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';


@Component({
    selector: 'app-subsidy-detail',
    templateUrl: './subsidy-detail.component.html'
})
export class SubsidyDetailComponent extends BaseEdit implements OnInit {
    accessLevels: any[] = [];
    categoryGroupDataSource: any;
    @Input()
    subsidyGroupId: number;

    constructor(private genericDataService: GenericDataService) {
        super(genericDataService);
        this.entityName = 'subsidy';

    }

    ngOnInit(): void {
        super.ngOnInit();
        if (this.subsidyGroupId) {
            this.entity.subsidyGroupId = this.subsidyGroupId;
        }
        this.categoryGroupDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'categoryGroup');
    }
}
