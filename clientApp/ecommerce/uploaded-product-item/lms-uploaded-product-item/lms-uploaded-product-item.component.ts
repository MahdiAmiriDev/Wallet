import {Component, OnInit} from '@angular/core';
import {BaseUploadedProductItem} from "../base-uploaded-product-item/base-uploaded-product-item";
import {GenericDataService} from "../../../../shared/base/services/generic-data.service";
import {UserDataService} from "../../../../_services/user-data.service";

@Component({
  selector: 'lms-uploaded-product-item',
  templateUrl: '../base-uploaded-product-item/base-uploaded-product-item.component.html'
})

export class LmsUploadedProductItemComponent extends BaseUploadedProductItem implements OnInit {


  constructor(protected genericDataService: GenericDataService, protected userDataService: UserDataService) {
    super(userDataService, genericDataService);
    this.finalRegistration = this.finalRegistration.bind(this);
    this.refreshListAndShowMessage = this.refreshListAndShowMessage.bind(this);
    this.afterFinalRegistration = this.afterFinalRegistration.bind(this);
  }

  ngOnInit() {
    this.showCategoryGroupDxLookUp = false;
    super.ngOnInit();
  }

  selectedCompanyChanged(e: any): void {
    if (e && e.selectedItem) {
      this.companyId = e.selectedItem.id;
      this.uploadUrl = `api/uploadedProductItem/uploadProductFile/${this.companyId}`;
    } else {
      this.companyId = null;
    }
  }

  //دریافت داده های additionalUserProp
  //و ست کردم مقادیر مربوط به مرکز کاربر برای آدرس آپلود اکسل
  setAdditionalUserPropData(): void {
    super.setAdditionalUserPropData();
    if (this.additionalUserProp && this.additionalUserProp.companyId) {
      this.companyId = this.additionalUserProp.companyId;
      this.uploadUrl = `api/uploadedProductItem/uploadProductFile/${this.companyId}`;
    }
  }

  exportCompanyProductsToExcel(): void {
    if (!this.companyId) {
      this.showError('کاربر گرامی لطفا مرکز را انتخاب کنید');
      return;
    }

    this.setBusy(true);
    const url = `api/uploadedProductItem/ExportCompanyProductsToExcel?companyId=${this.companyId}`;
    window.open(url, '_blank');
    this.setBusy(false);
  }

  finalRegistration(): void {
    if (!this.companyId) {
      this.showError('کاربر گرامی لطفا مرکز را انتخاب کنید');
    } else {
      this.genericDataService.postEntityByUrl(`api/uploadedProductItem/AddDataFromUploadedProductItem?companyId=${this.companyId}`, {})
        .subscribe(response => {
          if (!response.isSuccess) {
            this.showError(response.message);
          } else {
            this.showMessage(response.message);
          }
        });
    }
  }

  override bindCustomButtons() {
    this.customButtons = [{
      icon: 'file',
      text: 'بارگذاری فایل اکسل',
      type: 'success',
      key: 'showDialog',
      float: 'right'
    }];
  }


}
