import {Component, OnInit} from '@angular/core';
import {BaseEdit} from '../../../shared/base/classes/base-edit';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';


@Component({
    selector: 'app-category-group-detail',
    templateUrl: './category-group-detail.component.html'
})
export class CategoryGroupDetailComponent extends BaseEdit implements OnInit {


    constructor(private genericDataService: GenericDataService) {
        super(genericDataService);
        this.entityName = 'categoryGroup';
    }

    ngOnInit(): void {
        super.ngOnInit();
    }
}
