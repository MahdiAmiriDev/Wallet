import {Component, Input, OnInit} from '@angular/core';
import {BaseEdit} from '../../../shared/base/classes/base-edit';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {Entity} from '../../../shared/base/base-data/entity.enum';


@Component({
    selector: 'app-specification-option-detail',
    templateUrl: './specification-option-detail.component.html'
})
export class SpecificationOptionDetailComponent extends BaseEdit implements OnInit {
    @Input()
    specificationId: number;
    specificationDataSource: any;
    accessLevels: any[] = [];

    constructor(private genericDataService: GenericDataService) {
        super(genericDataService);
        this.entityName = 'specificationOption';
    }

    ngOnInit(): void {
        super.ngOnInit();
        if (this.specificationId) {
            this.entity.specificationId = this.specificationId;
        }

        this.specificationDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'specification');
        this._genericDataService.getEntitiesByEntityNameForSelect(Entity.AccessLevel).subscribe(res => this.accessLevels = res);
    }
}
