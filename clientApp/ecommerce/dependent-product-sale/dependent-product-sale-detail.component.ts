import {Component, Input, OnInit} from '@angular/core';
import {BaseEdit} from '../../../shared/base/classes/base-edit';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {Entity} from '../../../shared/base/base-data/entity.enum';


@Component({
    selector: 'app-dependent-product-sale-detail',
    templateUrl: './dependent-product-sale-detail.component.html'
})
export class DependentProductSaleDetailComponent extends BaseEdit implements OnInit {
    @Input()
    mainProductId: number;
    @Input()
    relatedProductId: number;
    mainProductDataSource: any;
    relatedProductDataSource: any;
    accessLevels: any[] = [];

    constructor(private genericDataService: GenericDataService) {
        super(genericDataService);
        this.entityName = 'dependentProductSale';
    }

    ngOnInit(): void {
        super.ngOnInit();
        if (this.mainProductId) {
            this.entity.mainProductId = this.mainProductId;
        }
        if (this.relatedProductId) {
            this.entity.relatedProductId = this.relatedProductId;
        }

        this.mainProductDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'product');
        this.relatedProductDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'product');
        this._genericDataService.getEntitiesByEntityNameForSelect(Entity.AccessLevel).subscribe(res => this.accessLevels = res);
    }
}
