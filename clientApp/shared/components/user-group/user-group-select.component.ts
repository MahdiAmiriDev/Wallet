import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {GenericDataService} from "../../../shared/base/services/generic-data.service";
import {DxTreeViewComponent} from "devextreme-angular/ui/tree-view";
import {Selectable} from "../../../shared/base/classes/selectable";


@Component({
    selector: 'app-user-group-select',
    templateUrl: './user-group-select.component.html'
})

export class UserGroupSelectComponent extends Selectable implements OnInit {
    @ViewChild(DxTreeViewComponent) treeView;
    @Input()
    selectedUserGroupId: any;
    @Input()
    isRootNodeSelectable: boolean = false;
    isVisible = false;
    type = '';
    userGroupDataSource: any;
    constructor(private genericDataService: GenericDataService) {
        super();
    }
    ngOnInit(): void {
        this.userGroupDataSource = this.genericDataService.createCustomDataSourceForSelect('id', 'userGroup');
    }

    //زمانی که کاربر ایتم انتخاب شده را حذف کند این متد فراخوانی می شود
    userGroupValueChanged(selectedItem:any): void {
        if(selectedItem.value == null){
            this.selectedRowChanged.emit(null);
        }
    }

    syncTreeViewSelection(e): void {
        const component = (e && e.component) || (this.treeView && this.treeView.instance);
        if (!component) {
            return;
        }
    }
    treeView_itemSelectionChanged(e): void {
        if (e.itemData.parentId) {
            this.selectedUserGroupId = e.itemData.id;
            this.selectedRowChanged.emit(e);
        }
    }
}
