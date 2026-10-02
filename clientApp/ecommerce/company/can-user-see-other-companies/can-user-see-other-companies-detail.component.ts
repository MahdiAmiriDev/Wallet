import {Component,OnInit} from '@angular/core';
import {BaseEdit} from "../../../../shared/base/classes/base-edit";
import {GenericDataService} from "../../../../shared/base/services/generic-data.service";

@Component({
  selector: 'can-user-see-other-companies-detail',
  templateUrl: './can-user-see-other-companies-detail.component.html'
})
/*
* این کامپوننت جهت ویرایش فیلد مربوط به این که کاربر بتواند سایر مراکز را ببیند یا نه کاربرد دارد
* در هنگام بارگذاری گروهی کاربران اکسل نمونه ای که دریافت می شود دارای ستون مرکز بوده که در صورتی که کاربر این چک باکس را true ست کند
تمامی کمپانی ها برای کاربر نمایش داده می شود و در صورتی که false باشد فقط کمپانی مربوط به خود را می تواند ببیند
* این کامپوننت مقدار فیلد canUserSeeOtherCompany را ست می کند
* */
export class CanUserSeeOtherCompaniesDetailComponent extends BaseEdit implements OnInit {


  updateCanUserSeeOtherCompanyEditModel: any = {};


  constructor(private genericDataService: GenericDataService,) {
    super(genericDataService);
    this.entityName = 'company';
  }

  ngOnInit() {
    super.ngOnInit();
  }


  //زمانی که کاربر فرم را submit می کند این متد فراخوانی می شود
  onFormSubmit(): void {
    if (this.selectedEntityId) {
      this.updateCanUserSeeOtherCompanyEditModel.id = this.selectedEntityId;
      this.updateCanUserSeeOtherCompanyEditModel.canUserSeeOtherCompany = this.entity.canUserSeeOtherCompany;
      this.genericDataService.postEntityByUrl(`/api/company/updateCanUserSeeOtherCompany`, this.updateCanUserSeeOtherCompanyEditModel)
        .subscribe((res: any) => {
          if (res.isSuccess) {
            this.showMessage(res.message);
            this.onCloseFormClick();
          } else {
            this.showError(res.message);
          }
        })
    }
  }

  //متد لود کردن انتیتی را overRide کردیم تا از ای پی ای که برای دریافت canUserSeeOtherCompany نوشته ایم استفاده کند
  loadEntityData(): void {
    this.genericDataService.getEntitiesWithAction(`company`,`GetCanUserSeeOtherCompany?companyId=${this.selectedEntityId}`,null)
      .subscribe((res:any) => {
        this.entity.canUserSeeOtherCompany = res;
      })
  }

}
