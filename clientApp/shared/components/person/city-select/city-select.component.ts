import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {Selectable} from '../../../base/classes/selectable';
import {GenericDataService} from '../../../base/services/generic-data.service';
import {DxTreeViewComponent} from 'devextreme-angular/ui/tree-view';


@Component({
    selector: 'app-city-select',
    templateUrl: './city-select.component.html'
})
export class CitySelectComponent extends Selectable implements OnInit {
    @ViewChild(DxTreeViewComponent) treeView;
    @Input()
    selectedItemId: number;
    columns: any[] = [];
    entityName = 'City';
    cityDataSource: any;

    constructor(private genericDataService: GenericDataService) {
        super();
    }

    ngOnInit(): void {
        this.cityDataSource = this.genericDataService.createCustomDataSourceForSelect('id', 'city');
    }

    syncTreeViewSelection(e) {
        const component = (e && e.component) || (this.treeView && this.treeView.instance);

        if (!component) {
            return;
        }
    }

    treeView_itemSelectionChanged(e) {
        if (e.itemData.parentId) {
            this.selectedRowChanged.emit(e.itemData.id);
        }
    }
}
