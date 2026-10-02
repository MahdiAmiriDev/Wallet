import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';
import {Component, OnInit, ViewChild} from '@angular/core';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {DxFileUploaderComponent} from 'devextreme-angular/ui/file-uploader';
import {CachedKey} from '../../../shared/enums/enum';
import {CachedDataService} from '../../../shared/services/cached-data.service';
import {VoucherFileModel} from './voucher-file-model';
import {UserDataService} from '../../../_services/user-data.service';


@Component({
  selector: 'app-uploaded-voucher',
  templateUrl: './uploaded-voucher.component.html'
})
export class UploadedVoucherComponent extends Selectable implements OnInit {
  @ViewChild('listForm', {static: true}) listForm: BaseList;
  @ViewChild(DxFileUploaderComponent) tempVoucherFile: DxFileUploaderComponent;
  companyId: any;
  voucherTypeId: any;
  companyDataSource: any;
  additionalUserProp: any;
  showDialog: boolean;
  showList: boolean;
  showSaveButton = false;
  // tslint:disable-next-line:variable-name
  _genericDataService: GenericDataService;
  voucherFileModel: VoucherFileModel;
  customButtons: any;
  columns: any[] = [];
  value: any[] = [];
  invalidItem: boolean;
  uploadUrl: string;
  message: string;

  constructor(private genericDataService: GenericDataService, private cachedDataService: CachedDataService,private userDataService: UserDataService) {
    super();
    this._genericDataService = genericDataService;
    this.voucherFileModel = new VoucherFileModel();
    this.refreshListAndShowMessage = this.refreshListAndShowMessage.bind(this);
    this.afterFinalRegistration = this.afterFinalRegistration.bind(this);
    this.resetVoucherFileModel = this.resetVoucherFileModel.bind(this);
  }


  ngOnInit(): void {
    this.additionalUserProp = this.userDataService.getAdditionalUserProp();
    if (this.additionalUserProp && this.additionalUserProp.companyId) {
      this.companyId = this.additionalUserProp.companyId;
      this.uploadUrl = `api/uploadVoucher/uploadVoucherFile/${this.companyId}&${this.voucherTypeId}`;
    }
    this.companyDataSource = this.genericDataService.createCustomDataSourceForSelect('id', 'company');
    this.bindColumns();
    this.bindCustomButtons();
  }

  bindCustomButtons(): void {
    this.customButtons = [{
      icon: 'file',
      text: 'بارگذاری فایل اکسل کد کوپن',
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


  resetVoucherFileModel(): void {
    this.voucherFileModel = new VoucherFileModel();
    this.tempVoucherFile.instance.reset();
  }

  addFile(files): void {
    const that = this;
    if (files && files[0]) {
      that.voucherFileModel.fileName = files[0].name;
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
      this.message = response.message;
    } else {
      this.invalidItem = true;
      this.showList = true;
      this.listForm.refreshList();
      this.message = response.message;

    }
  }

  public afterFinalRegistration(): void {
    this.showDialog = false;
  }

  /*
  متد زیر در صورت تغییر مرکز اجرا می شود
   */
  selectedCompanyChanged(e): void {
    if (e && e.company) {
      this.companyId = e.company.id;

    } else {
      this.companyId = null;
    }
  }

  /*
متد زیر در صورت تغییر نوع کوپن اجرا می شود
 */
  selectedVoucherTypeChanged(e): void {
    if (e && e.voucherType) {
      this.voucherTypeId = e.voucherType.id;
      this.uploadUrl = `api/uploadedVoucher/uploadedVoucherFile/${this.companyId},${this.voucherTypeId}`;
    } else {
      this.voucherTypeId = null;
    }
  }

  /*
   دانلود نمونه اکسل کد کوپن یک مرکز
   */
  downloadSampleExcel(): void {
    if (!this.voucherTypeId) {
      this.showError('کاربر گرامی لطفا نوع کوپن را انتخاب کنید');
    } else {
      this.setBusy(true);
      const url = `api/uploadedVoucher/ExportCompanyVoucherCodeToExcel?companyId=${this.companyId}&voucherTypeId=${this.voucherTypeId}`;
      if (url != null) {
        window.open(url, '_blank');
        this.setBusy(false);
      } else {
        this.showError('کد کوپن برای مرکز و نوع کوپن انتخابی وجود ندارد');
      }
    }
  }


}
