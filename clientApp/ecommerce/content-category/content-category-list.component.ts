import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';
import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {ContentCategoryDetailTabBasedComponent} from './content-category-detail-tab-based.component';


@Component({
    selector: 'app-content-category-list',
    templateUrl: './content-category-list.component.html'
})
export class ContentCategoryListComponent extends Selectable implements OnInit {
    @ViewChild('listForm', {static: true}) listForm: BaseList;
    loadAfterSetFilter: boolean;
    selectedItemId: number;
    columns: any[] = [];
    entityName = 'contentCategory';
    detailComponent = ContentCategoryDetailTabBasedComponent;
    _parentId: number;
    @Input()
    set parentId(val) {
        if (val !== this._parentId) {
            this._parentId = val;
        }
    }

    get parentId(): number {
        return this._parentId;
    }

    private baseListUrl = 'contentCategory/GetByCustomFilterForList?';

    constructor() {
        super();
    }

    ngOnInit(): void {
        this.bindColumns();
        if (this.bindCustomDataSources()) {
            this.loadAfterSetFilter = true;
        }
    }

    private bindColumns() {
        this.columns.push({
            caption: 'عنوان',
            dataField: 'title'
        });
    }

    public bindCustomDataSources() {
        let customListUrl = `${this.baseListUrl}`;
        if (this.listForm) {
            if (this.parentId) {
                customListUrl += `parentId=${this.parentId}&`;
            }
        }
        let res = false;
        if (this.parentId) {

            this.listForm.customListUrl = customListUrl;
            this.listForm.refreshList();
            res = true;
        }
        return res;

    }

    afterInitialDetailComponent(componentInstance: any) {
        const comp = (<ContentCategoryDetailTabBasedComponent>componentInstance.instance);

        if (this.selectedItemId) {
            comp.parentId = this.selectedItemId;
        }

    }

    selectedContentCategoryChanged(item: any) {
        this.selectedItemId = item.id;
    }
}
