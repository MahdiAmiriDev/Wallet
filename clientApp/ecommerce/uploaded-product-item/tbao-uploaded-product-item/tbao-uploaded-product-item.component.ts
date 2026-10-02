import {Component, OnInit} from '@angular/core';
import {GenericDataService} from '../../../../shared/base/services/generic-data.service';
import {ProductFileModel} from '../product-file-model';
import {UserDataService} from '../../../../_services/user-data.service';
import {BaseUploadedProductItem} from "../base-uploaded-product-item/base-uploaded-product-item";


@Component({
  selector: 'app-tbao-uploaded-product-item',
  templateUrl: '../base-uploaded-product-item/base-uploaded-product-item.component.html'
})
export class TbaoUploadedProductItemComponent extends BaseUploadedProductItem implements OnInit {

  categoryGroupId: number | null;
  categoryGroupDataSource: any;

  constructor(protected genericDataService: GenericDataService, protected userDataService: UserDataService) {
    super(userDataService,genericDataService);
    this.productFileModel = new ProductFileModel();
    this.finalRegistration = this.finalRegistration.bind(this);
    this.refreshListAndShowMessage = this.refreshListAndShowMessage.bind(this);
    this.afterFinalRegistration = this.afterFinalRegistration.bind(this);
  }


  ngOnInit(): void {
    super.ngOnInit();

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


  //ست کردم دیتاسورس مربوط به مراکز و categoryGroup
  setCompanyDataSource(): void {
    super.setCompanyDataSource();
    this.setCategoryGroupDataSource();

  }

  //ست کردم مقدار dataSource مربوط به CategoryGroup
  setCategoryGroupDataSource(): void{
    this.categoryGroupDataSource = this.genericDataService.createCustomDataSourceForSelect('id', 'categoryGroup');
  }


  /*
   در صورتی که فایل خطا نداشته باشد این متد برای بارگذاری محصولات در جداول مربوطه اجرا میشود
     */
  public finalRegistration(): void {
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







  selectedCompanyChanged(e: any): void {
    if (e && e.selectedItem) {
      this.companyId = e.selectedItem.id;
      this.uploadUrl = `api/uploadedProductItem/uploadProductFile/${this.companyId}`;
    } else {
      this.companyId = null;
    }
  }

  selectedCategoryGroupChanged(e): void {
    if (e && e.selectedItem) {
      this.categoryGroupId = e.selectedItem.id;
    } else {
      this.categoryGroupId = null;
    }

  }

  /*
   دانلود نمونه اکسل محصولات یک مرکز
   */
  exportCompanyProductsToExcel(): void {
    if (!this.companyId) {
      this.showError('کاربر گرامی لطفا مرکز را انتخاب کنید');
      return;
    }

    if (!this.categoryGroupId) {
      this.showError('کاربر گرامی لطفا گروه دسته بندی را انتخاب کنید');
      return;
    }


    this.setBusy(true);
    const url = `api/uploadedProductItem/ExportCompanyProductsToExcel?companyId=${this.companyId}&categoryGroupId=${this.categoryGroupId}`;
    if (url != null) {
      window.open(url, '_blank');
      this.setBusy(false);
    } else {
      this.showError('محصول برای مرکز انتخابی وجود ندارد');
    }
  }


}
