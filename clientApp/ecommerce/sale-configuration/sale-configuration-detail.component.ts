import {Component, Input, OnInit} from '@angular/core';
import {BaseEdit} from '../../../shared/base/classes/base-edit';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {FactorReportTypeEnums} from '../factor/factor-report-type-enums';
import {Observable} from 'rxjs';
import {FormControl} from '@angular/forms';


@Component({
    selector: 'app-sale-configuration-detail',
    templateUrl: './sale-configuration-detail.component.html'
})
export class SaleConfigurationDetailComponent extends BaseEdit implements OnInit {
    fromDateDataValue = new FormControl();
    toDateDataValue = new FormControl();
    @Input()
    representationId: number;
    @Input()
    companyId: number;
    @Input()
    taxCategoryId: number;
    representationDataSource: any;
    companyDataSource: any;
    taxCategoryDataSource: any;
    accessLevels: any[] = [];
    factorReportTypes: any;
    withoutDetailFactorReportTypeSelected: boolean;

    constructor(private genericDataService: GenericDataService) {
        super(genericDataService);
        this.entityName = 'saleConfiguration';
        this.checkValidationByFactorReportType = this.checkValidationByFactorReportType.bind(this);
    }

    ngOnInit(): void {
        super.ngOnInit();
        if (this.representationId) {
            this.entity.representationId = this.representationId;
        }
        if (this.companyId) {
            this.entity.companyId = this.companyId;
        }
        if (this.taxCategoryId) {
            this.entity.taxCategoryId = this.taxCategoryId;
        }

        this.representationDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'representation');
        this.companyDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'company');
        this.taxCategoryDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'taxCategory');
        this._genericDataService.getCommonBaseDataForSelect('FactorReportType').subscribe(res => this.factorReportTypes = res);
        if (!this.selectedEntityId) {
            this._genericDataService.getCustomEntitiesByUrl('/api/SaleConfiguration/GetApplicationDefaultIsGroupedByTaxCategory')
                .subscribe(res => {
                    this.entity.isGroupedByTaxCategory = res;
                });
            this._genericDataService.getEntitiesWithAction('TaxCategory', 'GetDefaultTaxValuesForFactor', null)
                .subscribe((res: any) => {
                    if (res && res.data) {
                        const tax = res.data.find(e => e.percentage > 0);
                        const taxExempt = res.data.find(e => e.percentage === 0);
                        this.entity.taxTitleForFactor = tax.titleForConvert;
                        this.entity.taxCodeForFactor = tax.code;
                        this.entity.taxUnitOfMeasurementForFactor = tax.unitOfMeasureMent;
                        this.entity.taxExemptTitleForFactor = taxExempt.titleForConvert;
                        this.entity.taxExemptCodeForFactor = taxExempt.code;
                        this.entity.taxExemptUnitOfMeasurementForFactor = taxExempt.unitOfMeasureMent;
                    }
                });
        }

    }

    onSelectedCompanyChanged(selectedItem: any): void {
        if (selectedItem.company) {
            this.entity.companyId = selectedItem.company.id;
            this.entity.representationId = null;
        } else {
            this.entity.companyId = null;
            this.entity.representationId = null;
        }
    }

    onSelectedRepresentationChanged(selectedItem: any): void {
        if (selectedItem.representation) {
            this.entity.representationId = selectedItem.representation.id;
        } else {
            this.entity.representationId = null;
        }
    }

    doBeforeSubmit(e): Observable<boolean> {
        if (!this.entity.installmentCount) {
            this.entity.installmentCount = 1;
        }
        if (!this.entity.founderId || this.entity.founderId == 0) {
            this.showError('کاربر گرامی، لطفا موسس را انتخاب نمایید.');
            return new Observable<boolean>(c => c.next(false));
        }
        return super.doBeforeSubmit(e);
    }

    selectedSchedulerChanged(selectedYashilSchedulerId: any): void {
        this.entity.yashilSchedulerId = selectedYashilSchedulerId;
    }

    selectedPersonChanged(selectedPersonEconomicProfile: any): void {
        this.entity.founderId = selectedPersonEconomicProfile.id;
    }

    selectedPersonBankAccountChanged(selectedPersonBankAccountId: any): void {
        this.entity.personBankAccountId = selectedPersonBankAccountId;
    }

    checkValidationByFactorReportType(item: any): void {
        this.withoutDetailFactorReportTypeSelected = false;
        if (item && item.value && item.value === FactorReportTypeEnums.WithoutDetail) {
            this.withoutDetailFactorReportTypeSelected = true;
        }
    }
}
