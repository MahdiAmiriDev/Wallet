import {Component, OnInit, ViewChild} from '@angular/core';
import {GenericDataService} from "../../../shared/base/services/generic-data.service";
import {BaseList} from "../../../shared/base/classes/base-list";
import {YashilCustomButtonColumn} from "../../../shared/base/classes/yashil-cutom-button-column";
import {YashilComponent} from "../../../core/baseClasses/yashilComponent";
import {custom} from "devextreme/ui/dialog";

@Component({
  selector: 'app-determine-user-level',
  templateUrl: './determine-user-level-list.component.html',
})

export class DetermineUserLevelListComponent extends YashilComponent implements OnInit {

  @ViewChild('listForm', {static: true}) listForm: BaseList;
  productSalesPlanningDataSource: any;
  productSalesPlanningWithRequireSupplierApprovalBeforePaymentDataSource: any;
  companyId: number | null = null;
  representationId: number | null = null;
  columns: any [] = [];
  customRowButton: YashilCustomButtonColumn;
  showChangeProductSalesPlanningPopup: boolean;
  description: string;
  selectedOrderItemId: number;
  selectedNewProductSalesPlanningId: number;
  disableConfirmChangeProductSalesPlanningButton: boolean;
  selectedProductSalesPlanningId: number;


  constructor(private genericDataService: GenericDataService) {
    super();
    this.changeOrderStatusFromWaitingForSupplierApprovalToWaitingForPayment = this.changeOrderStatusFromWaitingForSupplierApprovalToWaitingForPayment.bind(this);
    this.changeOrderStatusFromWaitingForSupplierApprovalToCancel = this.changeOrderStatusFromWaitingForSupplierApprovalToCancel.bind(this);
    this.showChangeProductSalesPlanning = this.showChangeProductSalesPlanning.bind(this);
    this.showConfirmMessageForChangeOrderStatusFromWaitingForSupplierApprovalToWaitingForPayment = this.showConfirmMessageForChangeOrderStatusFromWaitingForSupplierApprovalToWaitingForPayment.bind(this);
    this.showConfirmMessageForChangeOrderStatusFromWaitingForSupplierApprovalToCancel = this.showConfirmMessageForChangeOrderStatusFromWaitingForSupplierApprovalToCancel.bind(this);
  }


  ngOnInit() {
    this.setProductSalesPlanningDataSource();
    this.bindColumns();
    this.bindCustomRowButton();
  }

  //این متد جهت بایند کردن ستون های لیست کاربرد دارد
  bindColumns(): void {
    this.columns.push({
        caption: 'نام دوره',
        dataField: 'productTitle',
        width: '200px'
      }, {
        caption: 'نام کاربری',
        dataField: 'userName',
        width: '200px'
      }, {
        caption: 'نمایندگی',
        dataField: 'representationTitle',
        width: '200px'
      },
      {
        caption: 'کد ملی',
        dataField: 'nationalCode',
        width: '150px'
      })
  }

  //این متد جهت ست کردن دیتا سورس مربوط به برنامه ریزی های دوره ها کاربرد دارد.
  //برنامه ریزی دوره های که مربوط به مرکز و نمایندگی انتخابی است را برمیگرداند
  //در صورتی که کاربر به آن مرکز یا نمایندگی دسترسی نداشته باشد exception می دهد
  //برنامه ریزی دوره های که حداقل یک سفارش داشته باشند و از نوع نیاز به تایید
  //تایمن کننده قبل از پراخت باشند
  setProductSalesPlanningDataSource(): void {

    let actionName: string = 'GetProductSalesPlanningThatHasAnyOrderWithWaitingForSupplierApproval';

    if (this.companyId)
      actionName += '?companyId=' + this.companyId + '&';


    if (this.representationId) {

      if (!actionName.includes('?companyId='))
        actionName += '?';

      actionName += '&representationId=' + this.representationId;
    }

    this.productSalesPlanningDataSource =
      this.genericDataService.createCustomDataSourceWithAction(`id`, `productSalesPlanning`,
        actionName);

    this.setProductSalesPlanningWithRequireSupplierApprovalBeforePaymentDataSource();
  }

  //این متد جهت ست کردن دیتا سورس مربوط به برنامه ریزی های دوره ها کاربرد دارد.
  //برنامه ریزی دوره های که مربوط به مرکز و نمایندگی انتخابی است را برمیگرداند
  //در صورتی که کاربر به آن مرکز یا نمایندگی دسترسی نداشته باشد exception می دهد
  //برنامه ریزی دوره های مرکز و نمایدنگی که دارای که گزینه نیاز به تایید تامین کننده قبل
  //پرداخت آن ها فعال باشد.
  setProductSalesPlanningWithRequireSupplierApprovalBeforePaymentDataSource(): void {

    let actionName: string = 'GetProductSalesPlanningWithRequireSupplierApprovalBeforePayment';

    if (this.companyId)
      actionName += '?companyId=' + this.companyId + '&';

    if (this.representationId) {

      if (!actionName.includes('?companyId='))
        actionName += '?';

      actionName += 'representationId=' + this.representationId;
    }

    this.productSalesPlanningWithRequireSupplierApprovalBeforePaymentDataSource =
      this.genericDataService.createCustomDataSourceWithAction(`id`, `productSalesPlanning`,
        actionName);
  }

  //زمانی که کاربر مرکز جدیدی را انتخاب کند این متد فراخوانی می شود
  onSelectedCompanyChange(item: any): void {

    const companyId = item?.company?.id ? item.company.id : item ? item : null;

    //اگر مرکز مقدار داشت
    if (companyId) {
      //اگر مقدار مرکز تغییر کرده بود
      if (companyId != this.companyId) {
        this.companyId = companyId;
        this.setProductSalesPlanningDataSource();
      }
    } else {//اگر مرکز مقدار نداشت
      this.companyId = null;
    }
  }

  //زمانی که کاربر نمایندگی جدیدی را انتخاب کند این متد فراخوانی می شود
  onSelectedRepresentationChange(item: any): void {
    const representationId = item?.representation?.id ?? null;

    //اگر مقدار داشت
    if (representationId) {
      //اگر مقدار جدید انتخاب کرده بود
      if (representationId != this.representationId) {
        this.representationId = representationId;
        this.setProductSalesPlanningDataSource();
        this.refreshFormListData();
      }
    } else { //مقدار نداشت خالی کن نمایندگی رو
      this.representationId = null;
      this.setProductSalesPlanningDataSource();
      this.refreshFormListData();
    }
  }

  //زمانی که کاربر یک برنامه ریزی دوره را انتخاب می کند
  //این متد فراخوانی می شود و شناسه برناهه ریزی دوره را می دهد و فرم لیست را با توجه
  //به برنامه ریزی دوره جدید انتخابی رفرش می کند
  onSelectedProductSalesPlanningChanged(item: any): void {
    if (item.value) {
      this.selectedProductSalesPlanningId = item.value;
      this.refreshFormListData();
    }else{
      this.selectedProductSalesPlanningId = null;
      this.refreshFormListData();
    }
  }

  //رفرش کردن دیتای لیست با داده های جدید
  refreshFormListData(): void {

    let url = 'Order/GetOrdersWithWaitingForRequiredSupplierApprovalBeforePaymentStatus?';

    if (this.selectedProductSalesPlanningId)
      url += 'productSalesPlanningId=' + this.selectedProductSalesPlanningId;

    if (this.representationId) {

      if (url.includes('productSalesPlanningId'))
        url += '&representationId=' + this.representationId;
      else
        url += 'representationId=' + this.representationId;
    }

    this.listForm.customListUrl = url;
    this.listForm.refreshList();

  }

  //این متد جهت بایند کردن دکمه های مربوط به تایید رد و تغییر بسته برای هر سفارش کاربرد دارد
  bindCustomRowButton(): void {
    this.customRowButton = {
      width: '150px',
      customButtons: [{
        text: 'تایید',
        type: 'success',
        onClick: this.showConfirmMessageForChangeOrderStatusFromWaitingForSupplierApprovalToWaitingForPayment
      },
        {
          text: 'رد',
          type: 'danger',
          onClick: this.showConfirmMessageForChangeOrderStatusFromWaitingForSupplierApprovalToCancel
        },
        {
          text: 'تغییر بسته',
          type: 'warning',
          onClick: this.showChangeProductSalesPlanning
        }]
    }
  }

  //نمایش پیام تایید بعد از کلیک کاربر بر روی دکمه ی تایید بسته
  showConfirmMessageForChangeOrderStatusFromWaitingForSupplierApprovalToWaitingForPayment(item): any {
    this.confirmDialog('تایید بسته', 'آیا از تایید بسته اطمینان دارید؟')
      .show().then((dialogResult) => {
      if (dialogResult.res === 'ok') {
        this.changeOrderStatusFromWaitingForSupplierApprovalToWaitingForPayment(item.data.id)
      }
    });
  }

  //نمایش پیام تایید بعد از کلیک کاربر بر روی دکمه ی رد بسته
  showConfirmMessageForChangeOrderStatusFromWaitingForSupplierApprovalToCancel(item): any {
    this.confirmDialog('رد بسته', 'آیا از رد بسته اطمینان دارید؟')
      .show().then((dialogResult) => {
      if (dialogResult.res === 'ok') {
        this.changeOrderStatusFromWaitingForSupplierApprovalToCancel(item.data.id)
      }
    });
  }

  //نمایش پیام قبل از انجام یک عملیات برای گرفتن تایید کاربر
  confirmDialog(title: string, confirmText: string): any {
    return custom({
      title,
      messageHtml: `${confirmText}`,
      buttons: [{
        text: title,
        onClick: (e) => {
          return {res: 'ok'};
        }
      }, {
        text: 'انصراف',
        onClick: (e) => {
          return {res: 'cancel'};
        }
      }]
    });
  }

  // این مد جهت بروزرسانی وضعیت یک سفارش از حالت
  // نیاز به تایید تامین کننده به حالت منتظر پرداخت کاربرد دارد
  changeOrderStatusFromWaitingForSupplierApprovalToWaitingForPayment(orderId: any): void {
    this.setBusy(true);
    this.genericDataService.getEntitiesWithAction('order',
      'ChangeOrderStatusFromWaitingForSupplierApprovalToWaitingForPayment?orderId=' + orderId, null)
      .subscribe({
        next: (result: any) => {
          this.setBusy(false);

          if (!result.isSuccess) {
            this.showError(result.message);
            return;
          }

          this.showMessage(result.message);
          this.listForm.refreshList();
        },
        error: error => {
          this.setBusy(false);
          this.showError('بروز خطا به هنگام انجام عملیات لطفا با مدیر سیستم تماس بگیرید.');
        }
      });
  }

  // این مد جهت بروزرسانی وضعیت یک سفارش از حالت
  // نیاز به تایید تامین کننده به حالت انصراف کاربرد دارد
  changeOrderStatusFromWaitingForSupplierApprovalToCancel(orderId: any): void {
    this.setBusy(true);
    this.genericDataService.getEntitiesWithAction('order',
      'changeOrderStatusFromWaitingForSupplierApprovalToCancel?orderId=' + orderId, null)
      .subscribe({
        next: (result: any) => {
          this.setBusy(false);

          if (!result.isSuccess) {
            this.showError(result.message);
            return;
          }

          this.showMessage(result.message);
          this.listForm.refreshList();
        },
        error: error => {
          this.setBusy(false);
          this.showError('بروز خطا به هنگام انجام عملیات لطفا با مدیر سیستم تماس بگیرید.');
        }
      });
  }

  //زمانی که کاربر بر روی دکمه ی تغییر بسته کلیک می کند این متد
  //فراخوانی می شود و پاپ آپ مربوط به تغییر بسته را نمایش می دهد
  //و شناسه ایتم سفارش که قصد تغییر آن را
  //داریم را ست می کند
  showChangeProductSalesPlanning(selectedRow: any): void {
    this.selectedOrderItemId = selectedRow.data.orderItemId;
    this.showChangeProductSalesPlanningPopup = true;
  }

  //زمانی که کاربر یک دوره جدید را برای تغییر دوره جاری کاربر انتخاب می کند
  //این متد فراخوانی می شود و شناسه دوره جدید را ست می کند
  onSelectedNewProductSalesPlanningChanged(item: any) {
    if (item.value) {
      this.selectedNewProductSalesPlanningId = item.value;
    }
  }

  //زمانی که کاربر دوره جدید را انتخاب می کند و بر روی دکمه ی تایید
  //کلیک می کند این متد جهت تغییر بسته کاربر فراخوانی می شود
  changeOrderItemProductSalesPlanningForWaitingForSupplierApprovalStatus(): void {
    this.setBusy(true);
    this.disableConfirmChangeProductSalesPlanningButton = true;
    this.genericDataService.getEntitiesWithAction('order',
      'ChangeOrderItemProductSalesPlanningForWaitingForSupplierApprovalStatus?orderItemId=' + this.selectedOrderItemId +
      '&newProductSalesPlanningId=' + this.selectedNewProductSalesPlanningId + '&description=' + this.description, null)
      .subscribe({
        next: (result: any) => {
          this.setBusy(false);
          this.disableConfirmChangeProductSalesPlanningButton = false;

          if (!result.isSuccess) {
            this.showError(result.message);
            return;
          }

          this.showMessage(result.message);
          this.showChangeProductSalesPlanningPopup = false;
          this.description = null;
          this.listForm.refreshList();
        },
        error: error => {
          this.disableConfirmChangeProductSalesPlanningButton = false;
          this.setBusy(false);
          this.showError('بروز خطا به هنگام انجام عملیات لطفا با مدیر سیستم تماس بگیرید.');
        }
      });
  }

}
