import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {Selectable} from '../../../../shared/base/classes/selectable';
import {GenericDataService} from '../../../../shared/base/services/generic-data.service';


@Component({
    selector: 'app-category-group-search',
    templateUrl: './category-group-search.component.html'
})
export class CategoryGroupSearchComponent extends Selectable implements OnInit {
    @Input()
    isRequired: boolean;
    @Input()
    requiredMessage: string;
    @Input()
    selectedItemId: number;
    columns: any[] = [];
    entityName = 'categoryGroup';
    categoryGroupDataSource: any;
    @Input()
    customListUrl: string;
    @Output()
    categoryGroupChanged: EventEmitter<any> = new EventEmitter<any>();

    constructor(private genericDataService: GenericDataService) {
        super();
        this.onSelectedCategoryGroupChanged = this.onSelectedCategoryGroupChanged.bind(this);
    }

    ngOnInit(): void {
        this.categoryGroupDataSource = this.genericDataService.createCustomDataSourceForSelect('id', 'categoryGroup');
    }

    onSelectedCategoryGroupChanged(item: any): void {
        if (item) {
            this.categoryGroupChanged.emit(item.selectedItem);
        }
    }

}
