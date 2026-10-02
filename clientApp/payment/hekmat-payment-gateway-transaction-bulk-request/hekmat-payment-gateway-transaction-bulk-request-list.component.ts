import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';
import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {YashilCustomButtonColumn} from "../../../shared/base/classes/yashil-cutom-button-column";
import {HekmatTransactionBulkFileModel} from "./hekmat-transaction-bulk-file-model";
import {DxFileUploaderComponent} from "devextreme-angular/ui/file-uploader";

@Component({
  selector: 'app-hekmat-payment-gateway-transaction-bulk-request-list',
  templateUrl: './hekmat-payment-gateway-transaction-bulk-request-list.component.html'
})
export class HekmatPaymentGatewayTransactionBulkRequestListComponent extends Selectable implements OnInit {
  @ViewChild('listForm', {static: true}) listForm: BaseList;
  @ViewChild('temTransactionFile', {static: true}) transactionFile: DxFileUploaderComponent;
  loadAfterSetFilter: boolean;
  columns: any[] = [];
  entityName = 'hekmatPaymentGatewayTransactionBulkRequest';
  customButtons: any;
  customRowButton: YashilCustomButtonColumn;
  showDialog: boolean;
  showList: boolean;
  hekmatPaymentGatewayTransactionBulkRequestId: number;
  showTransaction: boolean;
  hekmatTransactionBulkFileModel: HekmatTransactionBulkFileModel = new HekmatTransactionBulkFileModel('');
  value: any[] = [];
  uploadUrl: string;
  invalidItem: boolean;

  _hekmatPaymentGatewayTransactionBulkRequestCategoryId: number;
  @Input()
  set hekmatPaymentGatewayTransactionBulkRequestCategoryId(val) {
    if (val !== this._hekmatPaymentGatewayTransactionBulkRequestCategoryId) {
      this._hekmatPaymentGatewayTransactionBulkRequestCategoryId = val;
    }
  }

  get hekmatPaymentGatewayTransactionBulkRequestCategoryId(): number {
    return this._hekmatPaymentGatewayTransactionBulkRequestCategoryId;
  }

  private baseListUrl = 'HekmatPaymentGatewayTransactionBulkRequest/GetByCustomFilterForList?';

  constructor() {
    super();
  this.displayTransactionsDetail = this.displayTransactionsDetail.bind(this);
  }

  ngOnInit(): void {

    this.bindColumns();
    this.bindCustomButtons();
    this.bindCustomRowButtons();
    if (this.bindCustomDataSources()) {
      this.loadAfterSetFilter = true;
    }
  }

  private bindColumns(): void {
    this.columns.push(
      {
        caption: 'عنوان فایل',
        dataField: 'title',
        width : '30%'
      },
      {
        caption: 'دسته بندی',
        dataField: 'hekmatPaymentGatewayTransactionBulkRequestCategoryTitle',
        width : '20%'
      },
      {
        caption: 'نام کاربر',
        dataField: 'createByFullName',
        width : '20%'
      },
      {
        caption: 'تاریخ آپلود فایل',
        dataField: 'creationDate',
        cellTemplate: 'jalaliDate',
        width : '10%'
      }
    );
  }

  bindCustomButtons(): void {
    this.customButtons = [{
      icon: 'file',
      text: 'بارگذاری فایل اکسل گروهی تراکنش ها',
      type: 'success',
      key: 'showDialog',
      float: 'right'
    }, {
      icon: 'download',
      text: 'دانلود نمونه فایل اکسل گروهی تراکنش ها',
      type: 'default',
      key: 'download',
      float: 'right'
    }];
  }

  private bindCustomRowButtons() {
    this.customRowButton = {
      width: '20%',
      customButtons: [
        {
          hint: 'مشاهده تراکنش ها',
          text: 'مشاهده تراکنش ها',
          icon: 'bulletlist',
          type: 'default',
          stylingMode:'contained',
          onClick:this.displayTransactionsDetail
        }
      ]
    };
  }
  displayTransactionsDetail(e){
    this.showTransaction = true;
    this.hekmatPaymentGatewayTransactionBulkRequestId =  e.row.data.id;
  }
  customButtonClicked(key: any): void {
    if(key === 'showDialog' &&( this.hekmatPaymentGatewayTransactionBulkRequestCategoryId === undefined || !this.hekmatPaymentGatewayTransactionBulkRequestCategoryId ))
    {
      this.showError('کاربر گرامی لطفا دسته یندی را انتخاب کنید.');
    }
   else if (key === 'showDialog') {

      this.showDialog = true;
      this.showList = false;
    }
    if (key === 'download') {
      this.downloadSampleExcel();
    }
  }

  /*
 دانلود نمونه فایل اکسل گروهی تراکنش ها
 */
  downloadSampleExcel(): void {
    this.setBusy(true);
    const url = `api/HekmatPaymentGatewayTransactionBulkRequest/ExportSampleTransactionBulkRequestFileExcel`;
    window.open(url, '_blank');
    this.setBusy(false);

  }

  resetFileModel(): void {
    this.transactionFile.instance.reset();
  }

  bindCustomDataSources(): boolean {
    let customListUrl = `${this.baseListUrl}`;
    if (this.listForm) {
      if (this.hekmatPaymentGatewayTransactionBulkRequestCategoryId) {
        customListUrl += `hekmatPaymentGatewayTransactionBulkRequestCategoryId=${this.hekmatPaymentGatewayTransactionBulkRequestCategoryId}&`;
      }
    }
    let res = false;
    if (this.hekmatPaymentGatewayTransactionBulkRequestCategoryId) {
      this.listForm.customListUrl = customListUrl;
      this.listForm.refreshList();
      res = true;
    }
    return res;

  }

  /*
  وقتی که فایل انتخاب می شود این متد فراخوانی می شود
   */
  addFile(files): void {

    if (files && files[0]) {
      this.hekmatTransactionBulkFileModel.fileName = files[0].name;
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
      this.hekmatPaymentGatewayTransactionBulkRequestId = response.entityId;
      this.showMessage(response.message);
    } else {
      this.invalidItem = true;
      this.showList = true;

      this.showError(response.message);
    }
  }

  setUploadUrl(hekmatPaymentGatewayTransactionBulkRequestCategoryId: number) {
    this.uploadUrl = `api/HekmatPaymentGatewayTransactionBulkRequest/UploadHekmatTransactionFile/${hekmatPaymentGatewayTransactionBulkRequestCategoryId}`;
  }

  /*
  بعد از اینکه فایل بارگذاری شده خطایی نداشت و کاربر دکمه ثبت نهایی را می زند این متد فراخوانی می شود
   */

  afterFinalRegistration() {
    this.showDialog = false;
  }
}

