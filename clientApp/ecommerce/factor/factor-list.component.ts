import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';
import {Component, ComponentFactoryResolver, Input, OnInit, ViewChild} from '@angular/core';
import {FactorDetailTabBasedComponent} from './factor-detail-tab-based.component';
import {Router} from '@angular/router';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {FactorDetailComponent} from './factor-detail.component';
import {DetailComponentDirective} from '../../../shared/base/base-list-form/detail-component.directive';
import {FactorUploadFileComponent} from '../factor-upload-file/factor-upload-file.component';
import {FactorItemListComponent} from '../factor-item/factor-item-list.component';
import {YashilCustomButtonColumn} from '../../../shared/base/classes/yashil-cutom-button-column';
import {custom} from 'devextreme/ui/dialog';
import {FactorVerifierListComponent} from '../factor-verifier/factor-verifier-list.component';
import {FactorPaymentTabBasedComponent} from '../factor-payment/factor-payment-tab-based.component';
import {FactorReviewStatusTypeEnums} from './factor-review-status-type-enums';


@Component({
  selector: 'app-factor-list',
  templateUrl: './factor-list.component.html',
})
export class FactorListComponent extends Selectable implements OnInit {
  @ViewChild('listForm', {static: true}) listForm: BaseList;
  isFactorWaitingForTheAdminToApprove: boolean = null;
  loadAfterSetFilter: boolean;
  selectedItemId: number;
  columns: any[] = [];
  customRowButtons: YashilCustomButtonColumn;
  entityName = 'factor';
  detailComponent = FactorDetailComponent;
  // tslint:disable-next-line:variable-name
  _taxCategoryId: number;
  @Input()
  set taxCategoryId(val) {
    if (val !== this._taxCategoryId) {
      this._taxCategoryId = val;
    }
  }

  get taxCategoryId(): number {
    return this._taxCategoryId;
  }

  // tslint:disable-next-line:variable-name
  _companyId: number;
  @Input()
  set companyId(val) {
    if (val !== this._companyId) {
      this._companyId = val;
    }
  }

  get companyId(): number {
    return this._companyId;
  }

  // tslint:disable-next-line:variable-name
  _representationId: number;
  @Input()
  set representationId(val) {
    if (val !== this._representationId) {
      this._representationId = val;
    }
  }

  get representationId(): number {
    return this._representationId;
  }

  // tslint:disable-next-line:variable-name
  _factorReviewStatusType: number;
  @Input()
  set factorReviewStatusType(val) {
    if (val !== this._factorReviewStatusType) {
      this._factorReviewStatusType = val;
    }
  }

  get factorReviewStatusType(): number {
    return this._factorReviewStatusType;
  }

  public showPopUp: boolean;
  public popUpTitle: string;
  @ViewChild(DetailComponentDirective) componentContainer: DetailComponentDirective;
  factorUploadFileComponent = FactorUploadFileComponent;
  factorItemsComponent = FactorItemListComponent;
  factorVerifierListComponent = FactorVerifierListComponent;
  factorPaymentTabBasedComponent = FactorPaymentTabBasedComponent;
  private baseListUrl = 'factor/GetByCustomFilterForList?';

  constructor(private componentFactoryResolver: ComponentFactoryResolver,
              private router: Router, private genericDataService: GenericDataService) {
    super();
    this.finalRegistrationForReview = this.finalRegistrationForReview.bind(this);
    this.uploadFactorDocs = this.uploadFactorDocs.bind(this);
    this.factorItemDetails = this.factorItemDetails.bind(this);
    this.factorVerifiers = this.factorVerifiers.bind(this);
    this.factorPayments = this.factorPayments.bind(this);

  }

  ngOnInit(): void {
    this.bindColumns();
    this.createCustomButtons();
    if (this.bindCustomDataSources()) {
      this.loadAfterSetFilter = true;
    }

  }

  private bindColumns(): void {
    this.columns.push({
      caption: 'کد',
      dataField: 'code',
      allowHeaderFiltering: false,
      width: '150px',
    }, {
      caption: 'دسته مالیاتی',
      dataField: 'taxCategoryTitle',
      allowSorting: false,
      width: '120px',
    }, {
      caption: 'قرارداد',
      dataField: 'saleConfigurationTitle',
      allowHeaderFiltering: false,
      allowSorting: false,
      width: '130px',
    }, {
      caption: 'سود(%)',
      dataField: 'profitPercentage',
      allowHeaderFiltering: false,
      allowSorting: false,
      width: '70px',
    }, {
      caption: 'تخفیف(%)',
      dataField: 'discountPercentage',
      allowSorting: false,
      width: '80px',
    }, {
      caption: 'تاریخ صدور',
      dataField: 'issueDate',
      cellTemplate: 'intDate',
      allowHeaderFiltering: false,
      width: '120px'
    }, {
      caption: 'وضعیت',
      dataField: 'factorReviewStatusTypeTitle',
      width: '95px'
    });
  }

  public bindCustomDataSources(): boolean {
    let customListUrl = `${this.baseListUrl}`;
    if (this.listForm) {
      if (this.taxCategoryId) {
        customListUrl += `taxCategoryId=${this.taxCategoryId}&`;
      }
      if (this.companyId) {
        customListUrl += `companyId=${this.companyId}&`;
      }
      if (this.representationId) {
        customListUrl += `representationId=${this.representationId}&`;
      }
      if (this.factorReviewStatusType) {
        customListUrl += `factorReviewStatusType=${this.factorReviewStatusType}&`;
      }
    }
    let res = false;
    if (this.taxCategoryId || this.companyId || this.representationId || this.factorReviewStatusType) {

      this.listForm.customListUrl = customListUrl;
      this.listForm.refreshList();
      res = true;
    }
    return res;

  }

  afterInitialDetailComponent(componentInstance: any): void {
    const comp = (componentInstance.instance as FactorDetailTabBasedComponent);

    if (this.taxCategoryId) {
      comp.taxCategoryId = this.taxCategoryId;
    }

    if (this.companyId) {
      comp.companyId = this.companyId;
    }

    if (this.representationId) {
      comp.representationId = this.representationId;
    }

    if (this.factorReviewStatusType) {
      comp.factorReviewStatusType = this.factorReviewStatusType;
    }

  }

  /*
  نمایش و دانلود گزارش فاکتور
   */
  viewReport(e): void {
    const id = e.row.data.id;
    const url = `${window.location.origin}/api/factor/GenerateFactorFile?factorId=${id}`;
    window.open(url, '_blank');
  }

  /*
  ثبت نهایی فاکتور جهت ارسال برای بررسی
   */
  finalRegistrationForReview(e): void {
    this.declareFinalRegisterForReview().show().then((dialogResult) => {
      if (dialogResult.res === 'ok') {
        this.genericDataService.postEntityByUrl('/api/factor/FinalFactorRegistration', {id: e.row.data.id})
          .subscribe(response => {
            if (!response.isSuccess) {
              this.showError(response.message);
            } else {
              this.listForm.refreshList();
              this.showMessage(`عملیات بروزرسانی با موفقیت انجام شد`);
            }
          });
      } else {
        return;
      }
    });
  }

  /*
  نمایش فرم آپلود فایل مهر و امضا شده فاکتور
   */
  uploadFactorDocs(e): void {
    const componentFactory = this.componentFactoryResolver.resolveComponentFactory(this.factorUploadFileComponent);
    const viewContainerRef = this.componentContainer.viewContainerRef;
    viewContainerRef.clear();
    const componentRef = viewContainerRef.createComponent(componentFactory);
    const editComponent = (componentRef.instance as FactorUploadFileComponent);
    // مقدار دهی پارامترهای ورودی
    editComponent.selectedEntityId = e.row.data.id;
    this.popUpTitle = 'بارگذاری فایل فاکتور';
    this.showPopUp = true;
  }

  /*
  نمایش فرم آیتم های فاکتور
   */
  factorItemDetails(e): void {
    const componentFactory = this.componentFactoryResolver.resolveComponentFactory(this.factorItemsComponent);
    const viewContainerRef = this.componentContainer.viewContainerRef;
    viewContainerRef.clear();
    const componentRef = viewContainerRef.createComponent(componentFactory);
    const editComponent = (componentRef.instance as FactorItemListComponent);
    // مقدار دهی پارامترهای ورودی
    editComponent.factorId = e.row.data.id;
    editComponent.gridHeight = window.innerHeight - 170;
    this.popUpTitle = 'آیتم های فاکتور';
    this.showPopUp = true;
  }

  /*
  نمایش تایید(بررسی کنندگان) فاکتور
   */
  factorVerifiers(e): void {
    const componentFactory = this.componentFactoryResolver.resolveComponentFactory(this.factorVerifierListComponent);
    const viewContainerRef = this.componentContainer.viewContainerRef;
    viewContainerRef.clear();
    const componentRef = viewContainerRef.createComponent(componentFactory);
    const factorVerifierComponent = (componentRef.instance as FactorVerifierListComponent);
    // مقداردهی پارامترهای ورودی
    factorVerifierComponent.factorId = e.row.data.id;
    this.popUpTitle = 'لیست تایید کنندگان فاکتور';
    this.showPopUp = true;
  }

  /*
  نمایش فرم پرداختی های فاکتور
   */
  factorPayments(e): void {
    const componentFactory = this.componentFactoryResolver.resolveComponentFactory(this.factorPaymentTabBasedComponent);
    const viewContainerRef = this.componentContainer.viewContainerRef;
    viewContainerRef.clear();
    const componentRef = viewContainerRef.createComponent(componentFactory);
    const factorPaymentComponent = (componentRef.instance as FactorPaymentTabBasedComponent);
    // مقدار دهی پارامترهای ورودی
    factorPaymentComponent.factorId = e.row.data.id;
    this.popUpTitle = 'لیست پرداختی های فاکتور';
    this.showPopUp = true;
  }

  /*
  نمایش پرسش از کاربر جهت ثبت نهایی فاکتور جهت بررسی
   */
  private declareFinalRegisterForReview(): any {
    return custom({
      title: 'ثبت نهایی فاکتور جهت بررسی',
      messageHtml: 'کاربر گرامی در صورت ثبت نهایی، فاکتور جهت بررسی به کارشناسان ارسال میشود، در صورت اطمینان دکمه ثبت جهت بررسی را بزنید',
      buttons: [{
        text: 'ثبت جهت برسی',
        type: 'success',
        onClick: () => {
          return {res: 'ok'};
        }
      },
        {
          text: 'انصراف',
          type: 'default',
          onClick: () => {
            return {res: 'cancel'};
          }
        }
      ],
    });
  }

  private createCustomButtons(): void {
    this.customRowButtons = {
      width: '250px',
      customButtons: [
        {
          hint: 'دانلود فاکتور',
          text: 'دانلود فاکتور',
          icon: 'pdffile',
          width: '95px',
          type: 'default',
          onClick: this.viewReport
        },
        {
          hint: 'بارگذاری فاکتور',
          text: 'بارگذاری فاکتور',
          icon: 'upload',
          type: 'success',
          stylingMode: 'outlined',
          onClick: this.uploadFactorDocs,
          visibleProp: 'allowInitialOrRejectState'
        },
        {
          hint: 'آیتم های فاکتور',
          text: 'آیتم های فاکتور',
          icon: 'bulletlist',
          type: 'default',
          stylingMode: 'outlined',
          onClick: this.factorItemDetails,
        },
        {
          hint: 'ثبت نهایی فاکتور جهت بررسی ',
          text: 'ثبت نهایی',
          icon: 'checklist',
          type: 'success',
          onClick: this.finalRegistrationForReview,
          visibleProp: 'allowInitialOrRejectState',
        },
        {
          hint: 'مشاهده وضعیت بررسی فاکتور',
          text: 'مشاهده وضعیت بررسی',
          icon: 'fieldchooser',
          type: 'default',
          stylingMode: 'success',
          onClick: this.factorVerifiers,
          visibleProp: 'allowFactorVerifier',
        },
        {
          hint: 'لیست پرداختی های فاکتور',
          text: 'پرداختی های فاکتور',
          icon: 'money',
          type: 'success',
          stylingMode: 'outlined',
          visibleProp: 'allowPayment',
          onClick: this.factorPayments
        }
      ]
    };
  }


  onListRowPrepared(e): void {
    if (e.rowType === 'data') {
      if (e.data.factorReviewStatusType === FactorReviewStatusTypeEnums.Confirm) {
        e.rowElement.bgColor = '#60d394';
        e.rowElement.className = e.rowElement.className.replace('dx-row-alt', '');
      } else if (e.data.factorReviewStatusType === FactorReviewStatusTypeEnums.Reject) {
        e.rowElement.bgColor = '#ee6050';
        e.rowElement.className = e.rowElement.className.replace('dx-row-alt', '');
      }
    }
  }
}
