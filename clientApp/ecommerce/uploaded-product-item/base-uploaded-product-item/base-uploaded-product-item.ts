import {Directive, OnInit, ViewChild} from "@angular/core";
import {BaseList} from "../../../../shared/base/classes/base-list";
import {DxFileUploaderComponent} from "devextreme-angular/ui/file-uploader";
import {Selectable} from "../../../../shared/base/classes/selectable";
import {ProductFileModel} from "../product-file-model";
import {UserDataService} from "../../../../_services/user-data.service";
import {GenericDataService} from "../../../../shared/base/services/generic-data.service";

@Directive()
export abstract class BaseUploadedProductItem extends Selectable implements OnInit {


  @ViewChild('listForm', {static: true}) listForm: BaseList;
  @ViewChild(DxFileUploaderComponent) tempProductFile: DxFileUploaderComponent;
  companyId: any;
  companyDataSource: any;
  additionalUserProp: any;
  showDialog: boolean;
  showList: boolean;
  showSaveButton = false;
  productFileModel: ProductFileModel;
  customButtons: any;
  columns: any[] = [];
  value: any[] = [];
  invalidItem: boolean;
  uploadUrl: string;
  showCategoryGroupDxLookUp: boolean = true;
  isLmsMode: boolean = false;

  protected constructor(protected userDataService: UserDataService, protected genericDataService: GenericDataService) {
    super();
  }


  ngOnInit() {

    if(this.userDataService.getSiteConfigurationLmsMode())
      this.isLmsMode = true;

    this.setAdditionalUserPropData();
    this.setCompanyDataSource();
    this.bindColumns();
    this.bindCustomButtons();
  }

  abstract finalRegistration(): void;

  abstract selectedCompanyChanged(e: any): void;

  abstract exportCompanyProductsToExcel(): void;

  //دریافت داده های additionalUserProp
  //و ست کردم مقادیر مربوط به مرکز کاربر برای آدرس آپلود اکسل
  setAdditionalUserPropData(): void {
    this.additionalUserProp = this.userDataService.getAdditionalUserProp();
  }

  //ست کردم دیتاسورس مربوط به مراکز و categoryGroup
  setCompanyDataSource(): void {
    this.companyDataSource = this.genericDataService.createCustomDataSourceForSelect('id', 'company');

  }

  bindCustomButtons(): void {
    this.customButtons = [{
      icon: 'file',
      text: 'بارگذاری فایل اکسل محصولات',
      type: 'success',
      key: 'showDialog',
      float: 'right'
    }];
  }


  bindColumns(): void {
    this.columns.push({
        caption: 'عنوان',
        dataField: 'title'
      },
      {
        caption: 'عنوان فایل',
        dataField: 'orginalName'
      },
      {
        caption: 'نام کاربر',
        dataField: 'createByFullName'
      },
      {
        caption: 'تاریخ آپلود فایل',
        dataField: 'creationDate',
        cellTemplate: 'jalaliDate'
      }
    );
  }

  resetProductFileModel(): void {
    this.productFileModel = new ProductFileModel();
    this.tempProductFile.instance.reset();
  }

  addFile(files): void {
    const that = this;
    if (files && files[0]) {
      that.productFileModel.fileName = files[0].name;
    }
  }

  customButtonClicked(key: any): void {
    if (key === 'showDialog') {
      if (!this.companyId) {
        this.showError('کاربر گرامی لطفا مرکز را انتخاب کنید');
      } else {
        this.showDialog = true;
        this.showList = false;
      }
    }
  }

  /*
   بعد از بارگذاری فایل اکسل متد زیر اجرا می شود
     */
  refreshListAndShowMessage(item: any): void {
    const response = JSON.parse(item.request.responseText);
    if (response && response.isSuccess) {
      this.showList = true;
      this.invalidItem = false;
      this.listForm.refreshList();
      this.showMessage(response.message);
    } else {
      this.invalidItem = true;
      this.showList = true;
      this.listForm.refreshList();
      this.showError(response.message);

    }
  }

  afterFinalRegistration(): void {
    this.showDialog = false;
  }

}
