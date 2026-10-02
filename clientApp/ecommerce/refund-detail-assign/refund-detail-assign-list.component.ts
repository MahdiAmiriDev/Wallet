import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';
import {Component, ComponentFactoryResolver, OnInit, ViewChild} from '@angular/core';
import {YashilCustomButtonColumn} from '../../../shared/base/classes/yashil-cutom-button-column';
import {IssueTypeEnums} from '../issue/issue-type-enums';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {DetailComponentDirective} from '../../../shared/base/base-list-form/detail-component.directive';
import {RefundDetailAssignEditComponent} from './refund-detail-assign-edit.component';


@Component({
  selector: 'app-refund-detail-assign-list',
  templateUrl: './refund-detail-assign-list.component.html'
})
export class RefundDetailAssignListComponent extends Selectable implements OnInit {
  @ViewChild('listForm', {static: true}) listForm: BaseList;
  @ViewChild(DetailComponentDirective) refundDetailAssignEditComponentContainer: DetailComponentDirective;
  refundDetailAssignEditComponent = RefundDetailAssignEditComponent;

  loadAfterSetFilter: boolean;
  entityName = 'refundDetail';
  selectedItemId: number;
  columns: any[] = [];
  customRowButtons: YashilCustomButtonColumn;
  showSelectBankAccountForm: boolean;
  showIssueLogWork: boolean;
  issueType = IssueTypeEnums.Refund;
  paymentId: number;
  customIssueTitle: string;
  orderId: number;
  private baseListUrl = 'refundDetail/GetAllGroupedItems?';
  radioButtonItems: any[];
  archiveMode: number;
  requestForAssign = false;

  constructor(private genericDataService: GenericDataService, private componentFactoryResolver: ComponentFactoryResolver) {
    super();
    this.assignRefundDetail = this.assignRefundDetail.bind(this);
    this.setIssueLogWork = this.setIssueLogWork.bind(this);


  }

  ngOnInit(): void {
    this.showSelectBankAccountForm = false;
    this.showIssueLogWork = false;
    this.radioButtonItems =
      [{text: 'بایگانی شده', value: 1},
        {text: 'بایگانی نشده', value: 2},
        {text: 'همه موارد', value: 3}];
    this.archiveMode = 2;
    this.bindColumns();
    this.BindCustomRowButtons();
    if (this.bindCustomDataSources()) {
      this.loadAfterSetFilter = true;
    }
  }

  public bindCustomDataSources(): boolean {
    let customListUrl = `${this.baseListUrl}`;
    if (this.archiveMode) {
      customListUrl += `archiveMode=${this.archiveMode}&`;
    }
    this.listForm.customListUrl = customListUrl;
    this.listForm.refreshList();
    return true;
  }

  private bindColumns(): void {
    this.columns.push({
        caption: 'نوع پرداخت',
        dataField: 'refundTypeTitle',
        width: '80px',
      },
      {
        caption: 'سفارش',
        dataField: 'orderTitle',
        width: '100px',
        allowHeaderFiltering: false
      },
      {
        caption: 'مسئول پیگیری',
        dataField: 'assigneeTitle',
        width: '100px'
      },
      {
        caption: 'نام',
        dataField: 'name',
        width: '100px',
        allowHeaderFiltering: false
      },
      {
        caption: 'نام خانوادگی',
        dataField: 'lastName',
        width: '100px',
        allowHeaderFiltering: false
      },
      {
        caption: 'کد ملی',
        dataField: 'nationalCode',
        width: '100px',
        allowHeaderFiltering: false
      },
      {
        caption: 'شماره موبایل',
        dataField: 'mobileNumber',
        width: '100px',
        allowHeaderFiltering: false
      },
      {
        caption: 'مبلغ پرداختی متقاضی',
        dataField: 'oldPaymentAmount',
        cellTemplate: 'number',
        width: '100px',
        allowHeaderFiltering: false
      },
      {
        caption: 'مبلغ عودت به متقاضی',
        dataField: 'refundAmount',
        cellTemplate: 'number',
        width: '100px',
        allowHeaderFiltering: false
      },
      {
        caption: 'بانک',
        dataField: 'bankTypeTitle',
        width: '80px',
      },
      {
        caption: 'شماره شبا',
        dataField: 'shebaNumber',
        width: '200px',
        allowHeaderFiltering: false
      },
      {
        caption: 'شماره حکمت کارت',
        dataField: 'cardNumber',
        hidingPriority: 6,
        allowHeaderFiltering: false
      },
      {
        caption: 'شعبه',
        dataField: 'branchName',
        hidingPriority: 5,
        width: '80px',

      },
      {
        caption: 'شماره حساب',
        dataField: 'accountNumber',
        width: '150px',
        hidingPriority: 4
      },
      {
        caption: 'تاریخ پرداخت',
        dataField: 'paidDateInt',
        width: '80px',
        cellTemplate: 'intDate',
        hidingPriority: 3,
        allowHeaderFiltering: false
      },
      {
        caption: 'تاریخ ثبت عودت',
        dataField: 'creationDate',
        cellTemplate: 'jalaliDate',
        width: '80px',
        hidingPriority: 2,
        allowHeaderFiltering: false
      },
      {
        caption: 'گروه عودت وجه',
        dataField: 'refundDetailGroupTitle',
        hidingPriority: 1,
        allowHeaderFiltering: false
      },
      {
        caption: 'نام کاربری ثبت کننده عودت',
        dataField: 'createByUserName',
        hidingPriority: 0,
        allowHeaderFiltering: false
      },
      {
        caption: 'تعداد پیگیری ثبت شده',
        dataField: 'issueLogWorkCount',
        allowSorting: false,
        width: '70px',
        allowHeaderFiltering: false
      }
    );
  }

  BindCustomRowButtons(): void {
    this.customRowButtons = {
      width: '160px',
      allowExporting: false,
      setAtFirstColumn: true,
      customButtons: [
        {
          hint: 'انتصاب عودت به کاربر',
          text: 'انتصاب به کاربر',
          icon: 'user',
          type: 'success',
          stylingMode: 'outlined',
          onClick: this.assignRefundDetail
        }, {
          hint: 'ثبت و نمایش پیگیری',
          text: 'ثبت و نمایش پیگیری',
          icon: 'tags',
          type: 'default',
          stylingMode: 'contained',
          onClick: this.setIssueLogWork
        }]
    };
  }


  assignRefundDetail(selectedItem: any): void {
    if (selectedItem && selectedItem.data) {
      const componentFactory = this.componentFactoryResolver.resolveComponentFactory(this.refundDetailAssignEditComponent);
      const viewContainerRef = this.refundDetailAssignEditComponentContainer.viewContainerRef;
      viewContainerRef.clear();
      const componentRef = viewContainerRef.createComponent(componentFactory);
      const editComponent = (componentRef.instance as RefundDetailAssignEditComponent);
      editComponent.selectedEntityId = selectedItem.data.id;
      editComponent.showCloseButton = true;
      editComponent.closeFormClick.subscribe(x => {
        this.requestForAssign = false;
        this.listForm.refreshList();
      });
      this.requestForAssign = true;
    }
  }

  /*
  مشاهده و ایجاد رویداد جدید
  اطلاعات مقداردهی میشوند
  و فرم ثبت رویداد جدید نمایش داده میشود
  و پایین فرم لیست رویدادهای ثبت شده تا کنون را نمایش میدهد
   */
  setIssueLogWork(selectedItem: any): void {
    if (selectedItem && selectedItem.data) {
      this.paymentId = selectedItem.data.paymentId;
      this.orderId = selectedItem.data.orderId;
      this.customIssueTitle = ` رفع مشکل عودت وجه برای سفارش ${selectedItem.data.orderTitle} `;
      if (this.paymentId) {
        this.customIssueTitle += ` و پرداخت ${selectedItem.data.paymentTitle} `;
      }
      this.showIssueLogWork = true;
    }
    this.showIssueLogWork = true;
  }

  /*
 بستن فرم ثبت رویداد
  */
  closeIssueLogWorkForm(item: any): void {
    this.showIssueLogWork = false;
    this.listForm.refreshList();
  }
}
