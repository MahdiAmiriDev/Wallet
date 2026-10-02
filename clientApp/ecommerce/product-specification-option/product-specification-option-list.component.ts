import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';
import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {ProductSpecificationOptionDetailComponent} from './product-specification-option-detail.component';


@Component({
    selector: 'app-product-specification-option-list',
    templateUrl: './product-specification-option-list.component.html'
})
export class ProductSpecificationOptionListComponent extends Selectable implements OnInit {
    @ViewChild('listForm', {static: true}) listForm: BaseList;
    loadAfterSetFilter: boolean;
    selectedItemId: number;
    columns: any[] = [];
    entityName = 'productSpecificationOption';
    detailComponent = ProductSpecificationOptionDetailComponent;
    firstLoad = true;
    private baseListUrl = 'productSpecificationOption/GetByCustomFilterForList?';

    constructor() {
        super();
    }

    _productId: number;

    get productId(): number {
        return this._productId;
    }

    @Input()
    set productId(val) {
        if (val !== this._productId) {
            this._productId = val;
        }
    }

    _specificationOption: number;

    get specificationOption(): number {
        return this._specificationOption;
    }

    @Input()
    set specificationOption(val) {
        if (val !== this._specificationOption) {
            this._specificationOption = val;
        }
    }

    _attributeType: number;

    get attributeType(): number {
        return this._attributeType;
    }

    @Input()
    set attributeType(val) {
        if (val !== this._attributeType) {
            this._attributeType = val;
        }
    }

    ngOnInit(): void {
        this.bindColumns();
        if (this.bindCustomDataSources()) {
            this.loadAfterSetFilter = true;
        }
    }

    afterInitialDetailComponent(componentInstance: any) {
        if (componentInstance) {
            const comp = (<ProductSpecificationOptionDetailComponent>componentInstance.instance);

            if (this.productId) {
                comp.productId = this.productId;
            }

            if (this.specificationOption) {
                comp.specificationOption = this.specificationOption;
            }

            if (this.attributeType) {
                comp.attributeType = this.attributeType;
            }
        }
    }

    private bindColumns() {
        if (!this.productId) {
            this.columns.push({
                caption: 'محصول',
                dataField: 'productTitle'
            });
        }
        this.columns.push({
            caption: 'مشخصات',
            dataField: 'specificationTitle'
        });
        this.columns.push({
            caption: 'ويژگي مشخصات',
            dataField: 'specificationOptionTitle'
        });
        this.columns.push({
            caption: 'ترتیب نمایش',
            dataField: 'displayOrder'
        });
        this.columns.push({
            caption: 'نوع ویژگی',
            dataField: 'attributeTypeTitle'
        });
        this.columns.push({
            caption: 'اجازه فیلتر',
            dataField: 'allowFiltering'
        });
        this.columns.push({
            caption: 'نمایش در جزییات محصول',
            dataField: 'showOnProductPage'
        });
    }

    public bindCustomDataSources() {
        let customListUrl = `${this.baseListUrl}`;
        if (this.listForm) {
            if (this.productId) {
                customListUrl += `productId=${this.productId}&`;
            }
            if (this.specificationOption) {
                customListUrl += `specificationOption=${this.specificationOption}&`;
            }
            if (this.attributeType) {
                customListUrl += `attributeType=${this.attributeType}&`;
            }
        }
        let res = false;
        if (this.productId || this.specificationOption || this.attributeType) {
            this.listForm.customListUrl = customListUrl;
            this.listForm.refreshList();
            res = true;
        }
        return res;
    }
}
