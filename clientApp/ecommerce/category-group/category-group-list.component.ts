import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';
import {Component, OnInit, ViewChild} from '@angular/core';
import {CategoryGroupDetailTabBased} from "./category-group-detail-tab-based";


@Component({
    selector: 'app-category-group-list',
    templateUrl: './category-group-list.component.html'
})
export class CategoryGroupListComponent extends Selectable implements OnInit {
    @ViewChild('listForm', {static: true}) listForm: BaseList;
    loadAfterSetFilter: boolean;
    selectedItemId: number;
    columns: any[] = [];
    entityName = 'categoryGroup';
    detailComponent = CategoryGroupDetailTabBased;


    constructor() {
        super();
    }

    ngOnInit(): void {
        this.bindColumns();

    }

    private bindColumns(): void {
        this.columns.push(
            {
                caption: 'عنوان',
                dataField: 'title'
            },
        );
    }



    afterInitialDetailComponent(componentInstance: any): void {
        const comp = (componentInstance.instance as CategoryGroupDetailTabBased);

    }
}
