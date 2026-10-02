import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';
import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {HekmatTransactionFileModel} from "./hekmat-transaction-file-model";
import {DxFileUploaderComponent} from 'devextreme-angular/ui/file-uploader';
import {CachedKey} from '../../../shared/enums/enum';
import {CachedDataService} from '../../../shared/services/cached-data.service';


@Component({
  selector: 'app-upload-hekmat-transaction',
  templateUrl: './upload-hekmat-transaction.component.html'
})
export class UploadHekmatTransactionComponent extends Selectable implements OnInit {
  @ViewChild('listForm', {static: true}) listForm: BaseList;
  @ViewChild(DxFileUploaderComponent) hekmatTransactionFile: DxFileUploaderComponent;
  companyId: any;
  companyDataSource: any;
  additionalUserProp: any;
  showDialog: boolean;
  showList: boolean;
  showSaveButton = false;
  // tslint:disable-next-line:variable-name
  _genericDataService: GenericDataService;
  hekmatTransactionFileModel: HekmatTransactionFileModel;
  customButtons: any;
  columns: any[] = [];
  value: any[] = [];
  invalidItem: boolean;
  uploadUrl: string;

  constructor(private genericDataService: GenericDataService, private cachedDataService: CachedDataService) {
    super();
    this._genericDataService = genericDataService;
    this.hekmatTransactionFileModel = new HekmatTransactionFileModel();
    this.refreshListAndShowMessage = this.refreshListAndShowMessage.bind(this);
    this.afterFinalRegistration = this.afterFinalRegistration.bind(this);
  }


  ngOnInit(): void {
    this.bindColumns();
    this.bindCustomButtons();
  }

  bindCustomButtons(): void {
    this.customButtons = [{
      icon: 'file',
      text: 'بارگذاری فایل نتیجه تراکنش های حکمت',
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


  resetHekmatTransactionFileModel(): void {
    this.hekmatTransactionFileModel = new HekmatTransactionFileModel();
    this.hekmatTransactionFile.instance.reset();
  }

  addFile(files): void {
    const that = this;
    if (files && files[0]) {
      that.hekmatTransactionFileModel.fileName = files[0].name;
    }
  }

  customButtonClicked(key: any): void {
    if (key === 'showDialog') {
      this.showDialog = true;
      this.showList = false;
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
      this.showDialog = false;
      this.listForm.refreshList();
      this.showError(response.message);

    }
  }

  public afterFinalRegistration(): void {
    this.showDialog = false;
  }
}
