import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';
import {Component, ComponentFactoryResolver, OnInit, ViewChild} from '@angular/core';
import {YashilCustomButtonColumn} from '../../../shared/base/classes/yashil-cutom-button-column';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {IssueTypeEnums} from '../issue/issue-type-enums';
import {DetailComponentDirective} from '../../../shared/base/base-list-form/detail-component.directive';
import {RefundDetailArchiveDescriptionEditComponent} from '../refund-detail/refund-detail-archive-description-edit.component';
import {custom} from 'devextreme/ui/dialog';


@Component({
    selector: 'app-refund-detail-set-bank-account-list',
    templateUrl: './refund-detail-set-bank-account.component.html'
})
export class RefundDetailSetBankAccountComponent extends Selectable implements OnInit {
    @ViewChild('listForm', {static: true}) listForm: BaseList;
    selectedRefundDetailId: number;
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
    private baseListUrl = 'refundDetail/GetGroupedItems?';
    radioButtonItems: any[];
    archiveMode: number;
    requestForArchive = false;
    @ViewChild(DetailComponentDirective) refundDetailArchiveDescriptionEditComponentContainer: DetailComponentDirective;
    refundDetailArchiveDescriptionEditComponent = RefundDetailArchiveDescriptionEditComponent;

    constructor(private genericDataService: GenericDataService, private componentFactoryResolver: ComponentFactoryResolver) {
        super();
        this.setBankAccount = this.setBankAccount.bind(this);
        this.setIssueLogWork = this.setIssueLogWork.bind(this);
        this.selectedRefundDetailChanged = this.selectedRefundDetailChanged.bind(this);
        this.archiveRefundDetail = this.archiveRefundDetail.bind(this);
        this.unArchiveRefundDetail = this.unArchiveRefundDetail.bind(this);
        this.showUnArchiveConfirmation = this.showUnArchiveConfirmation.bind(this);
        this.removeBankAccount = this.removeBankAccount.bind(this);
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

    setBankAccount(selectedItem: any): void {
        this.showSelectBankAccountForm = true;
    }

    removeBankAccount(selectedItem: any): void {
        this.setBusy(true);
        if (selectedItem && selectedItem.data) {
            this.confirmBeforeAction('حذف اطلاعات حساب از عودت وجه انتخابی', 'آیا از حذف اطلاعات حساب از عودت وجه انتخابی اطمینان دارید؟', 'تایید')
                .show().then((dialogResult) => {
                if (dialogResult.res === 'ok') {
                    // `/api/User/SendVerifyCode/${this.userLogin.username}`
                    this.genericDataService.postEntityByUrl(`/api/refundDetail/removePersonBankAccountFromRefundDetail?refundDetailId=${selectedItem.data.id}`, {})
                        .subscribe(response => {
                            this.setBusy(false);
                            if (!response.isSuccess) {
                                this.showError(response.message);
                            } else {
                                this.showMessage(response.message);
                                this.listForm.refreshList();
                            }
                        });
                    return;
                } else {
                    return;
                }
            });
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

    onSelectedPersonBankAccountChanged(item: any): void {
        this.setBusy(true);
        this.showSelectBankAccountForm = false;
        if (this.selectedRefundDetailId && item.data.id) {
            this.genericDataService.postEntityByUrl('/api/RefundDetail/SetRefundDetailPersonBankAccount',
                {
                    RefundDetailId: this.selectedRefundDetailId,
                    PersonBankAccountId: item.data.id
                })
                .subscribe(response => {
                    this.setBusy(false);
                    if (response.isSuccess) {
                        this.listForm.refreshList();
                        this.showMessage(response.message);
                    } else {
                        this.showError(response.message);
                    }
                });
        } else {
            this.showError('هیپچ رکوردی انتخاب نشده است');
        }

    }

    selectedRefundDetailChanged(item: any): void {
        this.selectedRefundDetailId = item.id;
    }

    /*
    بستن فرم ثبت رویداد
     */
    closeIssueLogWorkForm(item: any): void {
        this.showIssueLogWork = false;
        this.listForm.refreshList();
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
            }, {
                caption: 'بانک',
                dataField: 'bankTypeTitle',
                width: '80px',
            }, {
                caption: 'شماره شبا',
                dataField: 'shebaNumber',
                width: '200px',
                allowHeaderFiltering: false
            }, {
                caption: 'شماره حکمت کارت',
                dataField: 'cardNumber',
                hidingPriority: 6,
                allowHeaderFiltering: false
            }, {
                caption: 'شعبه',
                dataField: 'branchName',
                hidingPriority: 5,
                width: '80px',

            }, {
                caption: 'شماره حساب',
                dataField: 'accountNumber',
                width: '150px',
                hidingPriority: 4
            }, {
                caption: 'تاریخ پرداخت',
                dataField: 'paidDateInt',
                width: '80px',
                cellTemplate: 'intDate',
                hidingPriority: 3,
                allowHeaderFiltering: false
            }, {
                caption: 'تاریخ ثبت عودت',
                dataField: 'creationDate',
                cellTemplate: 'jalaliDate',
                width: '80px',
                hidingPriority: 2,
                allowHeaderFiltering: false
            }, {
                caption: 'گروه عودت وجه',
                dataField: 'refundDetailGroupTitle',
                hidingPriority: 1,
                allowHeaderFiltering: false
            }, {
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
        this.customRowButtons = {
            width: '150px',
            customButtons: [
                {
                    hint: 'ثبت اطلاعات حساب',
                    text: 'ثبت اطلاعات حساب',
                    icon: 'fieldchooser',
                    type: 'success',
                    stylingMode: 'contained',
                    onClick: this.setBankAccount
                }, {
                    hint: 'حذف اطلاعات حساب',
                    text: 'حذف اطلاعات حساب',
                    icon: 'trash',
                    type: 'danger',
                    visibleProp: 'allowRemovePersonBankAccount',
                    stylingMode: 'outlined',
                    onClick: this.removeBankAccount

                }, {
                    hint: 'ثبت و نمایش پیگیری',
                    text: 'ثبت و نمایش پیگیری',
                    icon: 'tags',
                    type: 'default',
                    stylingMode: 'contained',
                    onClick: this.setIssueLogWork
                }, {
                    hint: 'ثبت در بایگانی',
                    text: 'ثبت در بایگانی',
                    icon: 'activefolder',
                    type: 'success',
                    stylingMode: 'outlined',
                    visibleProp: 'allowArchive',
                    onClick: this.archiveRefundDetail
                },
                {
                    hint: 'خروج از بایگانی',
                    text: 'خروج از بایگانی',
                    icon: 'return',
                    type: 'danger',
                    stylingMode: 'outlined',
                    visibleProp: 'allowUnArchive',
                    onClick: this.unArchiveRefundDetail
                }]
        };
    }

    archiveRefundDetail(selectedItem: any): void {
        if (selectedItem && selectedItem.data) {
            const componentFactory = this.componentFactoryResolver.resolveComponentFactory(this.refundDetailArchiveDescriptionEditComponent);
            const viewContainerRef = this.refundDetailArchiveDescriptionEditComponentContainer.viewContainerRef;
            viewContainerRef.clear();
            const componentRef = viewContainerRef.createComponent(componentFactory);
            const editComponent = (componentRef.instance as RefundDetailArchiveDescriptionEditComponent);
            editComponent.selectedEntityId = selectedItem.data.id;
            editComponent.showCloseButton = true;
            editComponent.closeFormClick.subscribe(x => {
                this.requestForArchive = false;
                this.listForm.refreshList();
            });
            this.requestForArchive = true;
        }
    }

    unArchiveRefundDetail(selectedItem: any): void {
        if (selectedItem && selectedItem.data) {
            this.showUnArchiveConfirmation().show().then((dialogResult) => {
                if (dialogResult.res === 'ok') {
                    this.genericDataService.postEntityByUrl('/api/refundDetail/UnArchiveRefundDetail', {id: selectedItem.data.id})
                        .subscribe(response => {
                            if (!response.isSuccess) {
                                this.showError(response.message);
                            } else {
                                this.showMessage(response.message);
                                this.listForm.refreshList();
                            }
                        });
                    return;
                } else {
                    return;
                }
            });
        }
    }


    /*
    نشان دادن پیغام تایید جهت اینکه عودت وجه را از بایگانی خارج کنیم
     */
    showUnArchiveConfirmation(): any {
        return custom({
            title: 'خارج کردن از بایگانی',
            messageHtml: 'کاربر گرامی، آیا از خارج کردن از بایگانی عودت وجه اطمینان دارید',
            buttons: [{
                text: 'تایید و خارج کردن از بایگانی',
                type: 'success',
                onClick: (e) => {
                    return {res: 'ok'};
                }
            },
                {
                    text: 'انصراف',
                    type: 'default',
                    onClick: (e) => {
                        return {res: 'cancel'};
                    }
                }
            ],
        });
    }


    showDeletePersonBankButton(): any {

    }
}
