import {Component, Input, OnInit} from '@angular/core';
import {BaseEdit} from '../../../shared/base/classes/base-edit';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {forkJoin} from 'rxjs';
import {TranslateService} from '@ngx-translate/core';


@Component({
    selector: 'app-product-sales-planning-discount-detail',
    templateUrl: './product-sales-planning-discount-detail.component.html'
})
export class ProductSalesPlanningDiscountDetailComponent extends BaseEdit implements OnInit {
    @Input()
    productSalesPlanningId: number;
    saleConfigurationDiscountPercentage: number;
    productSalesPlanningDataSource: any;
    customerClubDataSource: any;
    accessLevels: any[] = [];
    formLabels: any = {};

    constructor(private genericDataService: GenericDataService, private translate: TranslateService,) {
        super(genericDataService);
        this.entityName = 'productSalesPlanningDiscount';
    }

    ngOnInit(): void {
        super.ngOnInit();
        if (this.productSalesPlanningId) {
            this.entity.productSalesPlanningId = this.productSalesPlanningId;
        }


        let result = forkJoin({
            confirmTitle: this.translate.get('Confirm'),
            finalRegistrationTitle: this.translate.get('FinalRegistration'),
            confirmDiscountPercentageTitle: this.translate.get('tms.Course.Validation.ConfirmDiscountPercentage'),
        });

        result.subscribe(({
                              confirmTitle,
                              finalRegistrationTitle,
                              confirmDiscountPercentageTitle,

                          }) => {

            this.formLabels.description = confirmTitle;
            this.formLabels.finalRegistration = finalRegistrationTitle;
            this.formLabels.confirmDiscountPercentage = confirmDiscountPercentageTitle;
        });
        this.customerClubDataSource =
            this.genericDataService.createCustomDataSourceWithCustomUrl('id', `customerClub/getAllCustomerClubHasActiveContract?productSalesPlanningId=${this.entity.productSalesPlanningId}`);
    }

    afterLoadData(res: any): any {
        this.saleConfigurationDiscountPercentage = res.saleConfigurationDiscountPercentage;
        return super.afterLoadData(res);
    }
}
