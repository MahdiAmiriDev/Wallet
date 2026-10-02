import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';
import {Component, OnInit, ViewChild} from '@angular/core';
import {ProductTemplateDetailComponent} from './product-template-detail.component';


@Component({
    selector: 'app-product-template-list',
    templateUrl: './product-template-list.component.html'
})
export class ProductTemplateListComponent extends Selectable implements OnInit {
    @ViewChild('listForm', {static: true}) listForm: BaseList;
    loadAfterSetFilter: boolean;
    selectedItemId: number;
    columns: any[] = [];
    entityName = 'productTemplate';
    detailComponent = ProductTemplateDetailComponent;
    private baseListUrl = 'productTemplate/GetByCustomFilterForList?';

    constructor() {
        super();
        this.columns.push({
            caption: 'عنوان',
            dataField: 'title'
        });

    }

    ngOnInit(): void {
    }

    afterInitialDetailComponent(componentInstance: any) {
        const comp = (<ProductTemplateDetailComponent>componentInstance.instance);

    }
}
