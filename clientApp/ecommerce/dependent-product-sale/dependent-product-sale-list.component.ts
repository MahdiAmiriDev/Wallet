import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';
import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {DependentProductSaleDetailComponent} from './dependent-product-sale-detail.component';


@Component({
    selector: 'app-dependent-product-sale-list',
    templateUrl: './dependent-product-sale-list.component.html'
})
export class DependentProductSaleListComponent extends Selectable implements OnInit {
    @ViewChild('listForm', {static: true}) listForm: BaseList;
    loadAfterSetFilter: boolean;
    selectedItemId: number;
    columns: any[] = [];
    entityName = 'dependentProductSale';
    detailComponent = DependentProductSaleDetailComponent;
    _mainProductId: number;
    @Input()
    set mainProductId(val) {
        if (val !== this._mainProductId) {
            this._mainProductId = val;
        }
    }

    get mainProductId(): number {
        return this._mainProductId;
    }

    _relatedProductId: number;
    @Input()
    set relatedProductId(val) {
        if (val !== this._relatedProductId) {
            this._relatedProductId = val;
        }
    }

    get relatedProductId(): number {
        return this._relatedProductId;
    }

    private baseListUrl = 'dependentProductSale/GetByCustomFilterForList?';

    constructor() {
        super();
    }

    bindColumns() {
        if (!this.mainProductId) {
            this.columns.push({
                caption: 'محصول اصلی',
                dataField: 'mainProductTitle'
            });
        }
        this.columns.push({
            caption: 'محصول وابسته',
            dataField: 'relatedProductTitle'
        });
    }

    ngOnInit(): void {
        if (this.bindCustomDataSources()) {
            this.loadAfterSetFilter = true;
        }
        this.bindColumns();
    }

    public bindCustomDataSources() {
        let customListUrl = `${this.baseListUrl}`;
        if (this.listForm) {
            if (this.mainProductId) {
                customListUrl += `mainProductId=${this.mainProductId}&`;
            }
            if (this.relatedProductId) {
                customListUrl += `relatedProductId=${this.relatedProductId}&`;
            }
        }
        let res = false;
        if (this.mainProductId || this.relatedProductId) {
            this.listForm.customListUrl = customListUrl;
            this.listForm.refreshList();
            res = true;
        }
        return res;
    }

    afterInitialDetailComponent(componentInstance: any) {
        const comp = (<DependentProductSaleDetailComponent>componentInstance.instance);

        if (this.mainProductId) {
            comp.mainProductId = this.mainProductId;
        }

        if (this.relatedProductId) {
            comp.relatedProductId = this.relatedProductId;
        }

    }
}
