import {Component, OnInit, ViewChild} from '@angular/core';
import {Selectable} from "../../../shared/base/classes/selectable";
import {BaseList} from "../../../shared/base/classes/base-list";
import {DxFileUploaderComponent} from "devextreme-angular/ui/file-uploader";
import {CertificateFileModel} from "./certificate-file-model";

@Component({
  selector: 'app-uploaded-certificate',
  templateUrl: './uploaded-certificate.component.html'
})

export class UploadedCertificateComponent extends Selectable implements OnInit {

  @ViewChild('listFrom', {static: true}) listForm: BaseList;
  @ViewChild('fileUploader', {static: true}) tempUserFile: DxFileUploaderComponent;

  columns: any[] = [];
  customButtons: any[] = [];
  showDialog: boolean;
  showList: boolean;
  certificateFileModel: CertificateFileModel = new CertificateFileModel('');
  value: any[] = [];
  uploadUrl: string;
  invalidItem: boolean;


  constructor() {
    super();
  }

  ngOnInit() {
    this.bindColumns();
    this.bindCustomButtons();
    this.uploadUrl = 'api/uploadedCertificate/UploadUserCertificateFile'
  }

  bindColumns(): void {
    this.columns.push({
      caption: 'عنوان',
      dataField: 'title'
    }, {
      caption: 'عنوان فایل',
      dataField: 'orginalName'
    }, {
      caption: 'نام کاربر',
      dataField: 'createByFullName'
    }, {
      caption: 'تاریخ آپلود فایل',
      dataField: 'creationDate',
      cellTemplate: 'jalaliDate'
    });
  }

  bindCustomButtons(): void {
    this.customButtons.push({
      icon: 'file',
      text: 'بارگذاری فایل اکسل گواهینامه',
      type: 'success',
      key: 'showDialog',
      float: 'right'
    }, {
      icon: 'download',
      text: 'دانلود نمونه فایل بارگذاری گواهینامه',
      type: 'default',
      key: 'download',
      float: 'right'
    });
  }


  customButtonClicked(key: any): void {
    if (key == 'download')
      this.downloadSampleExcel();

    if (key == 'showDialog')
      this.showDialog = true;

  }

  downloadSampleExcel(): void {
    window.open('api/uploadedCertificate/ExportSampleCertificateFileToExcel', '_blank');
  }

  bindCustomDataSources(): boolean {
    this.listForm.customListUrl = 'uploadedCertificate/GeUploadedInfo';
    this.listForm.refreshList();
    return true;
  }

  resetFileModel(): void {
    this.certificateFileModel = new CertificateFileModel('');
    this.showList = false;
    this.tempUserFile.instance.reset();
  }

  addFile(files: any): void {
    if (files && files[0]) {
      this.certificateFileModel.fileName = files[0].name;
    }
  }

  refreshListAndShowMessage(item: any): void {
    const response = JSON.parse(item.request.responseText);
    if (response && response.isSuccess) {
      this.showList = true;
      this.invalidItem = false;
      this.showMessage(response.message);
    } else {
      this.invalidItem = true;
      this.showList = true;

      this.showError(response.message);
    }
  }

  afterFinalRegistration(): void {
    this.showDialog = false;
    this.listForm.refreshList();
  }

}
