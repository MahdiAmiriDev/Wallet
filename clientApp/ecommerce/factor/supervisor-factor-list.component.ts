import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';
import {Component, ComponentFactoryResolver, EventEmitter, Input, OnInit, ViewChild} from '@angular/core';
import {FactorDetailTabBasedComponent} from './factor-detail-tab-based.component';
import {CachedDataService} from '../../../shared/services/cached-data.service';
import {YashilCustomButtonColumn} from '../../../shared/base/classes/yashil-cutom-button-column';
import {FactorVerifierListComponent} from '../factor-verifier/factor-verifier-list.component';
import {DetailComponentDirective} from '../../../shared/base/base-list-form/detail-component.directive';
import {FactorItemListComponent} from '../factor-item/factor-item-list.component';
import {FactorReviewStatusTypeEnums} from './factor-review-status-type-enums';
import {FactorPaymentTabBasedComponent} from '../factor-payment/factor-payment-tab-based.component';
import {ModifyFactorInfoComponent} from './modify-factor-info.component';
import {UserDataService} from '../../../_services/user-data.service';
import {custom} from 'devextreme/ui/dialog';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {FactorItemPaymentListComponent} from "../factor-item-payment/factor-item-payment-list.component";
import {FactorPaymentSummaryListComponent} from "../factor-item-payment-summary/factor-payment-summary-list.component";
import {FactorPaymentRefundedSubsidyDetailsComponent} from "../factor-payment-refunded-details/factor-payment-refunded-subsidy-details.component";


@Component({
    selector: 'app-factor-list-for-delete',
    templateUrl: './supervisor-factor-list.component.html'
})
export class SupervisorFactorListComponent extends Selectable implements OnInit {
    @ViewChild('listForm', {static: true}) listForm: BaseList;
    @ViewChild(DetailComponentDirective) componentContainer: DetailComponentDirective;
    afterCloseModifyFactorInfoDialogEventEmitter = new EventEmitter<boolean>();
    additionalUserProp: any;
    factorVerifierListComponent = FactorVerifierListComponent;
    modifyFactorInfoComponent = ModifyFactorInfoComponent;
    factorPaymentTabBasedComponent = FactorPaymentTabBasedComponent;
    factorItemsComponent = FactorItemListComponent;
    factorMicroPaymentComponent = FactorItemPaymentListComponent;
    FactorPaymentSummaryComponent = FactorPaymentSummaryListComponent;
    FactorPaymentRefundedSubsidyComponent = FactorPaymentRefundedSubsidyDetailsComponent;
    loadAfterSetFilter: boolean;
    selectedItemId: number;
    public showPopUp: boolean;
    public popUpTitle: string;
    customRowButtons: YashilCustomButtonColumn;
    columns: any[] = [];
    entityName = 'factor';
    detailComponent = FactorDetailTabBasedComponent;
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

    private baseListUrl = 'factor/GetSupervisorFactorList?';

    constructor(private componentFactoryResolver: ComponentFactoryResolver, private cachedDataService: CachedDataService,
                private genericDataService: GenericDataService,
                private userDataService: UserDataService) {
        super();
        this.showFactorVerifiers = this.showFactorVerifiers.bind(this);
        this.viewUploadedReport = this.viewUploadedReport.bind(this);
        this.viewReport = this.viewReport.bind(this);
        this.modifyFactorInfo = this.modifyFactorInfo.bind(this);
        this.factorPayments = this.factorPayments.bind(this);
        this.factorItemDetails = this.factorItemDetails.bind(this);
        this.resolveComponent = this.resolveComponent.bind(this);
        this.changeFactorToProcessingStatus = this.changeFactorToProcessingStatus.bind(this);
        this.confirmChangeFactorToProcessingStatus = this.confirmChangeFactorToProcessingStatus.bind(this);
        this.showFactorItemsMicroPayment=this.showFactorItemsMicroPayment.bind(this);
        this.showFactorPaymentSummaryComponent=this.showFactorPaymentSummaryComponent.bind(this);
        this.showFactorPaymentRefundedFactorItemDetails = this.showFactorPaymentRefundedFactorItemDetails.bind(this);

    }

    ngOnInit(): void {
        this.listForm.customListUrl = `${this.baseListUrl}`;
        this.additionalUserProp = this.userDataService.getAdditionalUserProp();
        if (this.additionalUserProp) {
            if (this.additionalUserProp.companyId) {
                this.companyId = this.additionalUserProp.companyId;
            }
            if (this.additionalUserProp.representationId) {
                this.representationId = this.additionalUserProp.representationId;
            }
        }
        this.bindColumns();
        this.bindCustomButtons();
        this.bindCustomDataSources();
        this.afterCloseModifyFactorInfoDialogEventEmitter.subscribe(() => {
            this.showPopUp = false;
            this.listForm.refreshList();
        });
    }

    private bindColumns(): void {
        this.columns.push({
            caption: 'کد',
            dataField: 'code',
            allowHeaderFiltering: false,
            width: '150px'
        });
        if (!this.companyId) {
            this.columns.push({
                width: '100px',
                caption: 'مرکز',
                dataField: 'companyTitle',
                allowSorting: false
            });
        }
        if (!this.representationId) {
            this.columns.push({
                width: '150px',
                caption: 'نمایندگی',
                dataField: 'representationTitle',
                allowSorting: false
            });
        }
        this.columns.push({
            caption: 'قرارداد',
            dataField: 'saleConfigurationTitle',
            allowSorting: false,
            width: '150px'
        }, {
            caption: 'سود(%)',
            dataField: 'profitPercentage',
            allowSorting: false,
            width: '70px',
        }, {
            caption: 'تخفیف(%)',
            dataField: 'discountPercentage',
            allowSorting: false,
            width: '80px',
        }, {
            width: '100px',
            caption: 'دسته مالیاتی',
            dataField: 'taxCategoryTitle',
            allowSorting: false
        }, {
            width: '100px',
            caption: 'گروه دسته بندی',
            dataField: 'categoryGroupTitle',
            allowSorting: false
        }, {
            dataField: 'factorReviewStatusTypeTitle',
            caption: 'وضعیت',
            width: '90px'
        }, {
            caption: 'تاریخ صدور',
            dataField: 'issueDate',
            cellTemplate: 'intDate',
            sortOrder: 'desc',
            allowHeaderFiltering: false,
            width: '110px'
        }, {
            caption: 'گروه بندی فاکتور',
            dataField: 'factorGroupTitle',
            allowHeaderFiltering: false,
            width: '110px'
        }, {
            caption: 'باشگاه مشتریان',
            dataField: 'customerClubTitle',
            allowHeaderFiltering: false,
            width: '110px'
        });
    }

    public bindCustomDataSources(): boolean {
        let customListUrl = `${this.baseListUrl}`;
        if (this.companyId) {
            customListUrl += `companyId=${this.companyId}&`;
        }
        if (this.representationId) {
            customListUrl += `representationId=${this.representationId}&`;
        }
        if (this.listForm) {
            if (this.taxCategoryId) {
                customListUrl += `taxCategoryId=${this.taxCategoryId}&`;
            }
            if (this.factorReviewStatusType) {
                customListUrl += `factorReviewStatusType=${this.factorReviewStatusType}&`;
            }
        }
        this.listForm.customListUrl = customListUrl;
        this.listForm.refreshList();
        return true;
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

    onSelectedRepresentationChanged(item: any): void {
        if (item && item.company) {
            this.companyId = item.company.id;
        } else {
            this.companyId = null;
        }
        if (item && item.representation) {
            this.representationId = item.representation.id;
        } else {
            this.representationId = null;
        }
        this.bindCustomDataSources();
    }

    /*
    هنگامی که مرکز تغییر میکند این متد صدا زده میشود
     */
    onSelectedCompanyChanged(item: any): void {
        if (item && item.company) {
            this.companyId = item.company.id;
            this.representationId = null;
        } else {
            this.companyId = null;
            this.representationId = null;
        }
        this.bindCustomDataSources();

    }

    private bindCustomButtons(): void {
        this.customRowButtons = {
            width: '200px',
            setAtFirstColumn: true,
            customButtons: [
                {
                    text: 'مشاهده وضعیت بررسی',
                    hint: 'مشاهده وضعیت بررسی فاکتور',
                    icon: 'fieldchooser',
                    type: 'default',
                    onClick: this.showFactorVerifiers,
                    visibleProp: 'allowFactorVerifierBtn',
                },
                {
                    text: 'دانلود فاکتور',
                    hint: 'دانلود فاکتور',
                    icon: 'pdffile',
                    type: 'default',
                    stylingMode: 'outlined',
                    onClick: this.viewReport
                },
                {
                    text: 'مشاهده و تایید اولیه فاکتور',
                    hint: 'مشاهده و تایید اولیه فاکتور',
                    icon: 'fieldchooser',
                    type: 'danger',
                    onClick: this.modifyFactorInfo,
                    visibleProp: 'allowModifyFactorInfo',
                },
                {
                    text: 'فاکتور مهر و امضا شده',
                    hint: 'دانلود فاکتور مهروامضا شده توسط تامین کننده',
                    icon: 'upload',
                    type: 'success',
                    onClick: this.viewUploadedReport,
                    visibleProp: 'allowViewUploadedReport'
                },
                {
                    text: 'آیتم های فاکتور',
                    hint: 'آیتم های فاکتور',
                    icon: 'bulletlist',
                    type: 'default',
                    onClick: this.factorItemDetails,
                },
                {
                    text: 'پرداختی های فاکتور',
                    hint: 'لیست پرداختی های فاکتور',
                    icon: 'money',
                    type: 'success',
                    visibleProp: 'allowPaymentBtn',
                    onClick: this.factorPayments
                },
                {
                    text: 'تغییر به وضعیت در حال بررسی',
                    hint: 'تغییر به وضعیت در حال بررسی',
                    icon: 'columnchooser',
                    type: 'danger',
                    visibleProp: 'allowPaymentBtn',
                    onClick: this.confirmChangeFactorToProcessingStatus
                },
                {
                    text: 'ریزپرداخت های آیتم فاکتور',
                    hint: 'ریزپرداخت های آیتم فاکتور',
                    icon: 'fieldchooser',
                    type: 'default',
                    onClick: this.showFactorItemsMicroPayment,
                },
                {
                    text: 'درصد پرداخت آیتم فاکتور',
                    hint: 'درصد پرداخت آیتم فاکتور',
                    icon: 'fieldchooser',
                    type: 'default',
                    onClick: this.showFactorPaymentSummaryComponent,
                },
                {
                    text: 'جزییات عودت وجه دارای یارانه',
                    hint: 'جزییات عودت وجه داری یارانه',
                    icon: 'fieldchooser',
                    type: 'default',
                    onClick: this.showFactorPaymentRefundedFactorItemDetails,
                }
            ]
        };
    }

    /*
    نمایش تایید کنندگان فاکتور
     */
    showFactorVerifiers(item: any): void {
        const factorVerifierComponent = this.resolveComponent(this.factorVerifierListComponent);
        // دادن پارامترهای ورودی
        factorVerifierComponent.factorId = item.row.data.id;
        this.popUpTitle = 'لیست تایید کنندگان فاکتور';
        this.showPopUp = true;
    }

    /*
    نمایش گزارش فاکتور
    این گزارش بصورت برخط توسط سامانه تهیه و تولید میشود
     */
    viewReport(item: any): void {
        const id = item.row.data.id;
        const url = `${window.location.origin}/api/factor/GenerateFactorFile?factorId=${id}`;
        window.open(url, '_blank');
    }

    /*
  ویرایش اطلاعات فروشنده (تامین کننده) مربوط به هر فاکتور
   */
    modifyFactorInfo(item: any): void {
        const modifyFactorComponent = this.resolveComponent(this.modifyFactorInfoComponent);
        // دادن پارامترهای ورودی
        modifyFactorComponent.selectedEntityId = item.row.data.id;
        modifyFactorComponent.closeFormClick = this.afterCloseModifyFactorInfoDialogEventEmitter;
        this.popUpTitle = 'ویرایش اطلاعات فروشنده فاکتور';
        this.showPopUp = true;
    }

    /*
    نمایش گزارش اپلود شده توسط تامین کننده
     */
    viewUploadedReport(item: any): void {
        const id = item.row.data.id;
        const url = `${window.location.origin}/api/factor/GetFactorPdf?factorId=${id}`;
        window.open(url, '_blank');
    }

    /*
    نمایش پرداختی های فاکتور
    در این فرم پرداخت هایی که برای یک فاکتور به تامین کننده انجام شده است را نمایش میدهد
     */
    factorPayments(item: any): void {
        const editComponent = this.resolveComponent(this.factorPaymentTabBasedComponent);
        // مقداردهی پارامترهای ورودی
        editComponent.factorId = item.row.data.id;
        editComponent.showAddButton = false;
        this.popUpTitle = 'لیست پرداختی های فاکتور';
        this.showPopUp = true;
    }

    /*
   نمایش فرم آیتم های فاکتور
   در این فرم آیتمهایی که در این فاکتور قراردارند بنمایش گذاشته میشود
    */
    factorItemDetails(item: any): void {
        const editComponent = this.resolveComponent(this.factorItemsComponent);
        // مقدار دهی پارامترهای ورودی
        editComponent.factorId = item.row.data.id;
        this.popUpTitle = 'آیتم های فاکتور';
        this.showPopUp = true;
    }

    /*
    جهت ایجاد کامپوننت مورد نظر با توجه به پراپرتی داده شده
    بازای هر دکمه که زده میشود یک کامپوننت مخصوص ایجاد میشود و این کامپوننت توسط این متد ساخته میشود
     */
    resolveComponent(componentName: any): any {
        const componentFactory = this.componentFactoryResolver.resolveComponentFactory(componentName);
        const viewContainerRef = this.componentContainer.viewContainerRef;
        viewContainerRef.clear();
        const componentRef = viewContainerRef.createComponent(componentFactory);
        return (componentRef.instance as typeof componentName);
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


    /*
    نمایش تاییدیه کاربر برای تغییر وضعیت قاکتور
     */
   private confirmChangeFactorToProcessingStatus(item): void {
        this.verifyChangeFactorToProcessingStatusMessage().show().then((dialogResult) => {
            if (dialogResult.res === 'ok') {
                this.changeFactorToProcessingStatus(item);
            } else {
                return;
            }
        });
    }

    /*
    نمایش ریز پرداخت ایتم های فاکتور
    */
    private showFactorItemsMicroPayment(item :any){
        const editComponent = this.resolveComponent(this.factorMicroPaymentComponent);
        // مقدار دهی پارامترهای ورودی
        editComponent.factorId = item.row.data.id;
        this.popUpTitle = 'ریز پرداخت های آیتم فاکتور';
        this.showPopUp = true;
    }
    /*
    نمایش خلاصه درصد پرداخت آیتم های فاکتور
    */
    private showFactorPaymentSummaryComponent(item :any){
        const editComponent = this.resolveComponent(this.FactorPaymentSummaryComponent);
        // مقدار دهی پارامترهای ورودی
        editComponent.factorId = item.row.data.id;
        this.popUpTitle = 'درصد پرداخت های آیتم فاکتور';
        this.showPopUp = true;
    }
    /*
    نمایش جزئیات عودت وجه در مورد فاکترو هایی که از یارانه استفاده کرده اند
     */
    private showFactorPaymentRefundedFactorItemDetails(item :any){
        const editComponent = this.resolveComponent(this.FactorPaymentRefundedSubsidyComponent);
        // مقدار دهی پارامترهای ورودی
        editComponent.factorId = item.row.data.id;
        this.popUpTitle = 'جزییات عودت وجه دارای یارانه';
        this.showPopUp = true;
    }

    /*
        نمایش پیام های تاییدیه کاربر برای تغییر وضعیت قاکتور
         */
    private verifyChangeFactorToProcessingStatusMessage(): any {
        return custom({
            title: 'تغییر وضعیت فاکتور',
            messageHtml: 'کاربر گرامی آیا از تغییر وضعیت فاکتور به وضعیت درحال بررسی اطمینان دارید؟',
            buttons: [{
                text: 'تایید',
                type: 'danger',
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

    /*
        خارج کردن فاکتور های گروه
         */
    changeFactorToProcessingStatus(item): void {
        let factorId = item.row.data.id;
        this.setBusy(true);
        this.genericDataService.postEntityByUrl(`/api/factor/ChangeFactorToProcessingStatus?id=${factorId}`, {})
            .subscribe(response => {
                if (!response.isSuccess) {
                    this.showError(response.message);
                } else {
                    this.listForm.refreshList();
                    this.showMessage(response.message);
                }
                this.setBusy(false);
            }, error => {
                this.setBusy(false);
            });
    }
}
