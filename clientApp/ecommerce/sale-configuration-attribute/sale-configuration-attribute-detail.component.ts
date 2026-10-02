import {Component, Input, OnInit} from '@angular/core';
import {BaseEdit} from '../../../shared/base/classes/base-edit';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {Entity} from '../../../shared/base/base-data/entity.enum';
import {FormControl} from '@angular/forms';


@Component({
    selector: 'app-sale-configuration-attribute-detail',
    templateUrl: './sale-configuration-attribute-detail.component.html'
})
export class SaleConfigurationAttributeDetailComponent extends BaseEdit implements OnInit {
    startDateValue = new FormControl();
    endDateValue = new FormControl();
    @Input()
    discountType: number;
    @Input()
    discountLimitationType: number;
    @Input()
    saleConfigurationId: number;
    @Input()
    customerClubId: number;
    discountTypes: any;
    discountLimitationTypes: any;
    accessLevels: any[] = [];
    saleConfigurationDataSource: any;
    customerClubDataSource: any;
    showLimitationTimes: boolean = false;
    showAppliedDiscountToSubCategories: boolean = false;
    saleTypes: any;

    constructor(private genericDataService: GenericDataService) {
        super(genericDataService);
        this.entityName = 'saleConfigurationAttribute';

    }

    ngOnInit(): void {
        super.ngOnInit();

        if (this.discountLimitationType) {
            this.entity.discountLimitationType = this.discountLimitationType;
        }
        if (this.saleConfigurationId) {
            this.entity.saleConfigurationId = this.saleConfigurationId;
        }
        this._genericDataService.getCommonBaseDataForSelect('DiscountType').subscribe(res => this.discountTypes = res);
        this._genericDataService.getCommonBaseDataForSelect('DiscountLimitationType').subscribe(res => this.discountLimitationTypes = res);
        this._genericDataService.getEntitiesByEntityNameForSelect(Entity.AccessLevel).subscribe(res => this.accessLevels = res);
        this.saleConfigurationDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'saleConfiguration');
        this.customerClubDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'customerClub');
        this._genericDataService.getCommonBaseDataForSelect('SaleType').subscribe(res => this.saleTypes = res);

    }


}
