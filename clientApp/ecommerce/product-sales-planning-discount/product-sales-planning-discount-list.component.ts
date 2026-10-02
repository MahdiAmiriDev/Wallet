import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';
import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {ProductSalesPlanningDiscountDetailComponent} from './product-sales-planning-discount-detail.component';


@Component({
    selector: 'app-product-sales-planning-discount-list',
    templateUrl: './product-sales-planning-discount-list.component.html'
})
export class ProductSalesPlanningDiscountListComponent extends Selectable implements OnInit {
    @ViewChild('listForm', {static: true}) listForm: BaseList;
    loadAfterSetFilter: boolean;
    selectedItemId: number;
    @Input()
    allowAdd = true;
    @Input()
    allowDeleteInForm = true;
    @Input()
    allowEditInForm = true;
    @Input()
    showAddButton = true;
    columns: any[] = [];
    entityName = 'productSalesPlanningDiscount';
    detailComponent = ProductSalesPlanningDiscountDetailComponent;
    _productSalesPlanningId: number;
    @Input()
    set productSalesPlanningId(val) {
        if (val !== this._productSalesPlanningId) {
            this._productSalesPlanningId = val;
        }
    }

    get productSalesPlanningId(): number {
        return this._productSalesPlanningId;
    }

    _customerClubId: number;
    @Input()
    set customerClubId(val) {
        if (val !== this._customerClubId) {
            this._customerClubId = val;
        }
    }

    get customerClubId(): number {
        return this._customerClubId;
    }

    private baseListUrl = 'productSalesPlanningDiscount/GetByCustomFilterForList?';

    constructor() {
        super();
    }

    ngOnInit(): void {
        this.bindColumns();
        if (this.bindCustomDataSources()) {
            this.loadAfterSetFilter = true;
        }
    }

    private bindColumns(): void {
        this.columns.push(
            {
                caption: 'باشگاه مشتریان',
                dataField: 'customerClubTitle'
            },
            {
                caption: 'درصد تخفیف برنامه ریزی دوره',
                dataField: 'discountPercentage'
            },
            {
                caption: 'درصد تخفیف قرارداد',
                dataField: 'saleConfigurationDiscountPercentage'
            }
        );
    }

    bindCustomDataSources(): boolean {
        let customListUrl = `${this.baseListUrl}`;
        if (this.listForm) {
            if (this.productSalesPlanningId) {
                customListUrl += `productSalesPlanningId=${this.productSalesPlanningId}&`;
            }
            if (this.customerClubId) {
                customListUrl += `customerClubId=${this.customerClubId}&`;
            }
        }
        let res = false;
        if (this.productSalesPlanningId || this.customerClubId) {

            this.listForm.customListUrl = customListUrl;
            this.listForm.refreshList();
            res = true;
        }
        return res;
    }

    afterInitialDetailComponent(componentInstance: any): void {
        const comp = (componentInstance.instance as ProductSalesPlanningDiscountDetailComponent);

        if (this.productSalesPlanningId) {
            comp.productSalesPlanningId = this.productSalesPlanningId;
        }

    }
}
