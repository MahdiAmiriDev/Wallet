import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';
import {Component, OnInit, ViewChild} from '@angular/core';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {YashilCustomButtonColumn} from "../../../shared/base/classes/yashil-cutom-button-column";
import {FactorPaymentInstallmentListComponent} from "./factor-payment-installment-list.component";
import {DxFileUploaderComponent} from "devextreme-angular/ui/file-uploader";

@Component({
  selector: 'app-factor-payment-installment',
  templateUrl: './factor-payment-installment.component.html'
})
export class FactorPaymentInstallmentComponent extends Selectable implements OnInit {

  @ViewChild('listForm', {static: true}) listForm: BaseList;
  @ViewChild('factorPaymentInstallmentList', {static: false}) factorPaymentInstallmentList: FactorPaymentInstallmentListComponent;
  @ViewChild(DxFileUploaderComponent) tempFileUploader: DxFileUploaderComponent;

  customRowButtons: YashilCustomButtonColumn;
  popupTitle = 'لیست اقساط ماه'
  showPopUp = false;
  yearMonth: number;
  categoryGroupId: number;
  showUploadDialog = false;
  fileModel: FactorPaymentInstallmentFileModel;
  value: any[] = [];
  uploadUrl: string = 'api/uploadedFactorInstallmentPayment/uploadFactorInstallmentPaymentFile';
  showFactorPaymentInstallmentList: boolean;
  showPaymentReferenceList = false;
  message: string;
  hasErrorOnUpload: boolean;

  constructor(private genericDataService: GenericDataService) {
    super();
    this.exportFactorPaymentInstallment = this.exportFactorPaymentInstallment.bind(this);
    this.uploadFactorPaymentInstallment = this.uploadFactorPaymentInstallment.bind(this);
    this.showFactorPaymentInstallmentReferenceList = this.showFactorPaymentInstallmentReferenceList.bind(this);
    this.resetFileModel = this.resetFileModel.bind(this);
  }

  ngOnInit(): void {
    this.showPopUp = false;
    this.bindCustomRowButtons();
  }

  bindCustomRowButtons(): void {
    this.customRowButtons = {
      customButtons: [
        {
          text: 'انتخاب اقساط فاکتور',
          icon: 'check',
          type: 'default',
          hint: 'دریافت گزارش اقساط',
          onClick: this.exportFactorPaymentInstallment
        },
        {
          icon: 'upload',
          text: 'بارگذاری اقساط پرداختی',
          hint: 'بارگذاری اقساط پرداختی',
          type: 'default',
          onClick: this.uploadFactorPaymentInstallment,
          visibleProp: 'allowUploadFactorPaymentInstallment'
        },
        {
          icon: 'columnchooser',
          text: 'ثبت شماره مرجع',
          hint: 'ثبت شماره مرجع',
          type: 'default',
          onClick: this.showFactorPaymentInstallmentReferenceList,
          visibleProp: 'allowUploadFactorPaymentInstallment'
        }]
    }
  }

  exportFactorPaymentInstallment(item: any) {
    this.yearMonth = item.row.data.id;
    this.showPopUp = true;
  }

  popupShown() {
    this.factorPaymentInstallmentList.yearMonth = this.yearMonth;
    this.factorPaymentInstallmentList.bindCustomDataSources();
  }

  uploadFactorPaymentInstallment(item: any) {
    this.value = [];
    this.showFactorPaymentInstallmentList = false;
    this.yearMonth = item.row.data.id;
    this.showUploadDialog = true;
  }

  showFactorPaymentInstallmentReferenceList(item: any) {
    this.yearMonth = item.row.data.id;
    this.showPaymentReferenceList = true;
  }

  closePopUpBtn(): void {
    this.showPopUp = false;
    this.listForm.refreshList();
  }

  resetFileModel(): void {
    this.fileModel = new FactorPaymentInstallmentFileModel();
    this.tempFileUploader.instance.reset();
  }

  addFile(files): void {
    const that = this;
    if (files && files[0]) {
      that.fileModel.fileName = files[0].name;
    }
  }

  /*
وقتی در پاپ آپ ا گروه دسته بندی تغییر می کند
 */
  onCategoryGroupChanged(item: any): void {
    if (item) {
      this.factorPaymentInstallmentList.categoryGroupId = item.id;
      this.factorPaymentInstallmentList.bindCustomDataSources();
    }
  }

  /*
    بعد از بارگذاری فایل اکسل متد زیر اجرا می شود
      */
  refreshListAndShowMessage(item: any): void {
    const response = JSON.parse(item.request.responseText);
    if (response && response.isSuccess) {
      this.hasErrorOnUpload = false;
      this.showFactorPaymentInstallmentList = true;
      this.listForm.refreshList();
    } else {
      this.hasErrorOnUpload = true;
      this.message = response.message;
      this.showFactorPaymentInstallmentList = true;
      this.listForm.refreshList();
      this.yearMonth = null;
    }
    this.value = [];
  }

}

export class FactorPaymentInstallmentFileModel {
  fileName: string;
}

