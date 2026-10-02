import {Selectable} from '../../../shared/base/classes/selectable';
import {Component, OnInit, ViewChild} from '@angular/core';
import {TransactionFileModel} from './transaction-file-model';
import {DxFileUploaderComponent} from 'devextreme-angular/ui/file-uploader';
import {BaseList} from '../../../shared/base/classes/base-list';


@Component({
  selector: 'app-upload-hekmat-transaction-file-list',
  templateUrl: './upload-hekmat-transaction-file.component.html'
})
export class UploadHekmatTransactionFileComponent extends Selectable implements OnInit {
  @ViewChild('listForm', {static: true}) listForm: BaseList;
  @ViewChild(DxFileUploaderComponent) hekmatTransactionFile: DxFileUploaderComponent;
  showDialog: boolean;
  error = '';
  transactionFileModel: TransactionFileModel;
  customButtons: any;
  columns: any[] = [];
  value: any[] = [];


  constructor() {
    super();
    this.transactionFileModel = new TransactionFileModel();
    this.refreshListAndShowMessage = this.refreshListAndShowMessage.bind(this);
  }

  ngOnInit(): void {
    this.bindCustomButtons();
    this.columns.push({
        caption: 'عنوان',
        dataField: 'title'
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
      text: 'بارگذاری فایل اکسل ',
      type: 'success',
      key: 1,
    }];
  }

  customButtonClicked(e: any): void {
    this.resetFileModel();
    this.showDialog = true;
  }

  addFile(files): void {
    const that = this;
    if (files && files[0]) {
      that.transactionFileModel.FileName = files[0].name;
    }
  }

  resetFileModel(): void {
    this.transactionFileModel = new TransactionFileModel();
    this.hekmatTransactionFile.instance.reset();
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
