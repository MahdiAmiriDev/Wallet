import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {BaseEdit} from '../../../shared/base/classes/base-edit';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {Entity} from '../../../shared/base/base-data/entity.enum';
import {DxTreeViewComponent} from 'devextreme-angular/ui/tree-view';


@Component({
    selector: 'app-specification-detail',
    templateUrl: './specification-detail.component.html'
})
export class SpecificationDetailComponent extends BaseEdit implements OnInit {
    @ViewChild(DxTreeViewComponent) treeView;
    @Input()
    specificationCategoryId: number;
    specificationCategoryDataSource: any;
    accessLevels: any[] = [];

    constructor(private genericDataService: GenericDataService) {
        super(genericDataService);
        this.entityName = 'specification';
    }

    ngOnInit(): void {
        super.ngOnInit();
        if (this.specificationCategoryId) {
            this.entity.specificationCategoryId = this.specificationCategoryId;
        }

        this.specificationCategoryDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'specificationCategory');
        this._genericDataService.getEntitiesByEntityNameForSelect(Entity.AccessLevel).subscribe(res => this.accessLevels = res);
    }

    selectedSpecificationCategoryChanged(item: any) {
        this.entity.specificationCategoryId = item;
    }
}
