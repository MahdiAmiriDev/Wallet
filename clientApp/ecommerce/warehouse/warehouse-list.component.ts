import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';
import {Component, ViewChild} from '@angular/core';
import {WarehouseDetailComponent} from './warehouse-detail.component';


@Component({
    selector: 'app-warehouse-list',
    templateUrl: './warehouse-list.component.html'
})
export class WarehouseListComponent extends Selectable  {
    @ViewChild('listForm', {static: true}) listForm: BaseList;
    loadAfterSetFilter: boolean;
    selectedItemId: number;
    columns: any[] = [];
    entityName = 'warehouse';
    detailComponent = WarehouseDetailComponent;
    private baseListUrl = 'warehouse/GetByCustomFilterForList?';

    constructor() {
        super();
        this.columns.push({
            caption: 'عنوان',
            dataField: 'title'
        });

    }

    afterInitialDetailComponent(componentInstance: any) {
        const comp = (<WarehouseDetailComponent>componentInstance.instance);
    }
}
