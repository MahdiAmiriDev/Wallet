import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';
import {Component, OnInit, ViewChild} from '@angular/core';
import {DxFileUploaderComponent} from 'devextreme-angular/ui/file-uploader';
import {FactorPaymentFileModel} from './factor-Payment-file-model';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';


@Component({
  selector: 'app-upload-factor-payment-file-list',
  templateUrl: './upload-factor-payment-file.component.html'
})
export class UploadFactorPaymentFileComponent extends Selectable implements OnInit {
  @ViewChild('listForm', {static: true}) listForm: BaseList;
  @ViewChild(DxFileUploaderComponent) factorPaymentFile: DxFileUploaderComponent;
  loadAfterSetFilter: boolean;
  selectedItemId: number;
  showUploadedItems = false;
  showDialog: boolean;
  factorPaymentFileModel: FactorPaymentFileModel;
  customButtons: any;
  isValidUploadedFactorPaymentFile: boolean;
  columns: any[] = [];
  valueFileUploader: any[] = [];
  entityName = 'uploadedFactorPayment';
  customListUrl = `${this.entityName}/GetUploadedFactorPaymentFiles`;
  uploadUrl = `api/${this.entityName}/uploadFactorPaymentFile`;
  // tslint:disable-next-line:variable-name
  _genericDataService: GenericDataService;

  constructor(private genericDataService: GenericDataService) {
    super();
    this._genericDataService = genericDataService;
    this.factorPaymentFileModel = new FactorPaymentFileModel();
  }

  ngOnInit(): void {
    this.bindCustomButtons();
    this.columns.push(
      {
        caption: 'عنوان فایل',
        dataField: 'orginalName'
      },
      {
        caption: 'تاریخ آپلود فایل',
        dataField: 'creationDate',
        cellTemplate: 'jalaliDate',
        width: 100
      }
    );
  }

  bindCustomButtons(): void {
    this.customButtons = [{
      icon: 'file',
      text: 'بارگذاری فایل اکسل ',
      type: 'success',
      key: 1,
    }];
  }

  addFile(files): void {
    const that = this;
    if (files && files[0]) {
      that.factorPaymentFileModel.fileName = files[0].name;
    }
  }

  customButtonClicked(e: any): void {
    this.showDialog = true;
    this.showUploadedItems = false;
    this.resetFileModel();
  }

  resetFileModel(): void {
    this.factorPaymentFileModel = new FactorPaymentFileModel();
    this.factorPaymentFile.instance.reset();
  }

  // بررسی میکند که فایل اپلود شده دارای محتویات صحیحی هست یا خیر
  ChecksInvalidUploadedFactorPaymentFile(): void {
    this.setBusy(true);
    this._genericDataService.getEntitiesWithAction(this.entityName, 'CheckIsValidUploadedFactorPaymentFile', null)
      .subscribe((res: any) => {
          this.setBusy(false);
          if (res.isSuccess) {
            this.isValidUploadedFactorPaymentFile = true;
          } else {
            // اگر ولیدیشن مشکلی داشته باشد باید به کاربر فرم مشکلات را نشان دهیم
            this.showError(res.message);
            this.isValidUploadedFactorPaymentFile = false;
          }
        }
      );
  }

  afterUploadFile(item: any): void {
    this.busyIndicator.setBusy(false);
    // چک میکنیم که آپلود با موفقیت انجام شده است یا خیر
    const response = JSON.parse(item.request.responseText);
    if (response && response.isSuccess) {
      // اگر آپلود با موفقیت انجام شده باشد باید
      // باید اطلاعات فایل بارگذاری شده را صحت سنجی کنیم
      this.ChecksInvalidUploadedFactorPaymentFile();
      this.showUploadedItems = true;
    } else {
      this.showError(response.message);
    }
  }


  // بستن پاپ آپ و رفرش کردن لیست فایل ها
  closePopUpAndRefreshList($event: any): void {
    this.showDialog = false;
    this.listForm.refreshList();
  }
}
