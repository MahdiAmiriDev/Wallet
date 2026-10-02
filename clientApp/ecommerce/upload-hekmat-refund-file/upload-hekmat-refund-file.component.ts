import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';
import {Component, OnInit, ViewChild} from '@angular/core';
import {RefundFileModel} from './refund-file-model';
import {DxFileUploaderComponent} from 'devextreme-angular/ui/file-uploader';


@Component({
  selector: 'app-upload-hekmat-refund-file-list',
  templateUrl: './upload-hekmat-refund-file.component.html'
})
export class UploadHekmatRefundFileComponent extends Selectable implements OnInit {
  @ViewChild('listForm', {static: true}) listForm: BaseList;
  @ViewChild(DxFileUploaderComponent) hekmatRefundFile: DxFileUploaderComponent;
  loadAfterSetFilter: boolean;
  selectedItemId: number;
  showDialog: boolean;
  error = '';
  refundFileModel: RefundFileModel;
  customButtons: any;
  columns: any[] = [];
  value: any[] = [];


  constructor() {
    super();
    this.refundFileModel = new RefundFileModel();
  }

  ngOnInit(): void {
    this.bindCustomButtons();
    this.columns.push({
        caption: 'عنوان',
        dataField: 'title',
        width: 200
      },
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
      that.refundFileModel.fileName = files[0].name;
    }
  }

  customButtonClicked(e: any): void {
    this.showDialog = true;
    this.resetFileModel();
  }

  resetFileModel(): void {
    this.refundFileModel = new RefundFileModel();
    this.hekmatRefundFile.instance.reset();
  }

  refreshListAndShowMessage(item: any): void {
    const response = JSON.parse(item.request.responseText);
    if (response && response.isSuccess) {
      this.listForm.refreshList();
      this.showMessage(response.message);
    } else {
      this.showError(response.message);
    }
  }
}
