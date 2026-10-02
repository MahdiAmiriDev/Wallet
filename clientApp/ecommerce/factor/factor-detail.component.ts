import {Component, Input, OnInit} from '@angular/core';
import {BaseEdit} from '../../../shared/base/classes/base-edit';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {Observable} from "rxjs/Observable";

@Component({
    selector: 'app-factor-detail',
    templateUrl: './factor-detail.component.html'
})
export class FactorDetailComponent extends BaseEdit implements OnInit {
    @Input()
    taxCategoryId: number;
    @Input()
    companyId: number;
    @Input()
    representationId: number;
    @Input()
    factorReviewStatusType: number;
    taxCategoryDataSource: any;
    companyDataSource: any;
    representationDataSource: any;
    accessLevels: any[] = [];
    contentHeight: number;
    showSaveBtn = true;
    public validationMessage: string;
    showPopup: boolean = true;
    productSalesPlanningIds: any [] = [];

    constructor(private genericDataService: GenericDataService) {
        super(genericDataService);
        this.entityName = 'factor';
    }

    ngOnInit(): void {
        this.contentHeight = window.innerHeight - 110;
        this.customAddMethodName = 'CreateFactor';
        super.ngOnInit();
        if (this.taxCategoryId) {
            this.entity.taxCategoryId = this.taxCategoryId;
        }
        if (this.companyId) {
            this.entity.companyId = this.companyId;
        }
        if (this.representationId) {
            this.entity.representationId = this.representationId;
        }
        if (this.factorReviewStatusType) {
            this.entity.factorReviewStatusType = this.factorReviewStatusType;
        }
        this.checkAllowAddFactor();
    }

    /**
     * Check Exists Factor Items And Not Exists Any Pending Factors
     */
    checkAllowAddFactor(): void {
        this.setBusy(true);
        this._genericDataService.getEntitiesWithAction('factor', 'ValidateShowSaveBtn', null)
            .subscribe((res: any) => {
                    this.setBusy(false);
                    if (res.isSuccess) {
                        this.showSaveBtn = true;
                        this.validationMessage = null;
                    } else {
                        this.showSaveBtn = false;
                        this.validationMessage = res.message;
                    }
                }
            );
    }

   //دریافت شناسه برنامه ریزی محصول های انخاب شده
  onItemSelect(productSalesPlanningId:any){
    this.productSalesPlanningIds = productSalesPlanningId;
  }

  //افزودن شناسه محصول های انخاب شده به انتیتی قبل از ارسال به سرور
  doBeforeSubmit(e): Observable<boolean> {
    this.entity.productSalesPlanningIds = this.productSalesPlanningIds;
    return super.doBeforeSubmit(e);
  }
}
