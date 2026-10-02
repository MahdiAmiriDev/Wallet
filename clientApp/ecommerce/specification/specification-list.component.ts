import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';
import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {SpecificationDetailComponent} from './specification-detail.component';


@Component({
    selector: 'app-specification-list',
    templateUrl: './specification-list.component.html'
})
export class SpecificationListComponent extends Selectable implements OnInit {
    @ViewChild('listForm', {static: true}) listForm: BaseList;
    loadAfterSetFilter: boolean;
    selectedItemId: number;
    columns: any[] = [];
    entityName = 'specification';
    detailComponent = SpecificationDetailComponent;
    private baseListUrl = 'specification/GetByCustomFilterForList?';

    constructor() {
        super();
    }

    _specificationCategoryId: number;

    get specificationCategoryId(): number {
        return this._specificationCategoryId;
    }

    @Input()
    set specificationCategoryId(val) {
        if (val !== this._specificationCategoryId) {
            this._specificationCategoryId = val;
        }
    }

    ngOnInit(): void {
        this.bindColumns();
        if (this.bindCustomDataSources()) {
            this.loadAfterSetFilter = true;
        }
    }

    afterInitialDetailComponent(componentInstance: any) {
        const comp = (<SpecificationDetailComponent>componentInstance.instance);

        if (this.specificationCategoryId) {
            comp.specificationCategoryId = this.specificationCategoryId;
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
            if (this.specificationCategoryId) {
                customListUrl += `specificationCategoryId=${this.specificationCategoryId}&`;
            }
        }
        let res = false;
        if (this.specificationCategoryId) {
            this.listForm.customListUrl = customListUrl;
            this.listForm.refreshList();
            res = true;
        }
        return res;
    }
}
