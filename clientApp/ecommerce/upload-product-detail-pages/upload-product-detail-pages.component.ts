import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';
import {Component, OnInit, ViewChild} from '@angular/core';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {DxFileUploaderComponent} from 'devextreme-angular/ui/file-uploader';
import {ProductDetailPagesFileModel} from './product-deail-pages-file-model';


@Component({
  selector: 'app-upload-product-detail-pages',
  templateUrl: './upload-product-detail-pages.component.html'
})
export class UploadProductDetailPagesComponent extends Selectable implements OnInit {
  @ViewChild('listForm', {static: true}) listForm: BaseList;
  @ViewChild(DxFileUploaderComponent) tempVoucherFile: DxFileUploaderComponent;
  additionalUserProp: any;
  showDialog: boolean;
  showSaveButton = false;
  // tslint:disable-next-line:variable-name
  _genericDataService: GenericDataService;
  productDetailPagesFileModel: ProductDetailPagesFileModel;
  customButtons: any;
  columns: any[] = [];
  value: any[] = [];
  message: string;
  acceptRules = false;
  resourceName = 'uploadProductDetailPage';

  constructor(private genericDataService: GenericDataService) {
    super();
    this._genericDataService = genericDataService;
    this.productDetailPagesFileModel = new ProductDetailPagesFileModel();
    this.refreshListAndShowMessage = this.refreshListAndShowMessage.bind(this);
  }


  ngOnInit(): void {
    this.bindColumns();
    this.bindCustomButtons();
  }

  bindColumns(): void {
    this.columns.push({
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

  bindCustomButtons(): void {
    this.customButtons = [{
      icon: 'file',
      text: 'بارگذاری zip صفحات توضیحات محصول',
      type: 'success',
      key: 'showDialog',
      float: 'right'
    }];
  }

  addFile(files): void {
    const that = this;
    if (files && files[0]) {
      that.productDetailPagesFileModel.fileName = files[0].name;
    }
  }

  /*
  نمایش فرم بارگذاری فایل زیپ
   */
  showUploadForm(): void {
    this.showDialog = true;
    this.acceptRules = false;
  }

  /*
     بعد از بارگذاری فایل زیپ متد زیر اجرا می شود
       */
  refreshListAndShowMessage(item: any): void {
    const response = JSON.parse(item.request.responseText);
    if (response && response.isSuccess) {
      this.showMessage(response.message);
    } else {
      this.showError(response.message);
    }
    this.showDialog = false;
    this.listForm.refreshList();
  }


}
