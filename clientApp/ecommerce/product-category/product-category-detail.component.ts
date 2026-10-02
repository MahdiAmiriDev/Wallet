import {Component, Input, OnInit} from '@angular/core';
import {BaseEdit} from '../../../shared/base/classes/base-edit';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {Entity} from '../../../shared/base/base-data/entity.enum';


@Component({
    selector: 'app-product-category-detail',
    templateUrl: './product-category-detail.component.html'
})
export class ProductCategoryDetailComponent extends BaseEdit implements OnInit {
    @Input()
    productId: number;
    @Input()
    categoryId: number;
    productDataSource: any;
    categoryDataSource: any;
    accessLevels: any[] = [];

    constructor(private genericDataService: GenericDataService) {
        super(genericDataService);
        this.entityName = 'productCategory';
    }

    ngOnInit(): void {
        super.ngOnInit();
        if (this.productId) {
            this.entity.productId = this.productId;
        }
        if (this.categoryId) {
            this.entity.categoryId = this.categoryId;
        }

        this.productDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'product');
        this.categoryDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'category');
        this._genericDataService.getEntitiesByEntityNameForSelect(Entity.AccessLevel).subscribe(res => this.accessLevels = res);
    }
}
