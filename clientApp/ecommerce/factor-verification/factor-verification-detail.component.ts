import {Component, ComponentFactoryResolver, EventEmitter, Input, OnInit, ViewChild} from '@angular/core';
import {BaseEdit} from '../../../shared/base/classes/base-edit';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {PDFSource} from 'pdfjs-dist';
import {custom} from 'devextreme/ui/dialog';
import {DetailComponentDirective} from '../../../shared/base/base-list-form/detail-component.directive';
import {FactorVerificationDescriptionEditComponent} from './factor-verification-description-edit.component';
import {FactorVerificationConfirmationType} from './factor-verification-enums';
import {YashilCustomButton} from '../../../shared/base/models/yasil-custom-button';
import {UserDataService} from '../../../_services/user-data.service';


@Component({
    selector: 'app-factor-verification-detail',
    templateUrl: './factor-verification-detail.component.html'
})
export class FactorVerificationDetailComponent extends BaseEdit implements OnInit {
    @Input()
    factorConfirmationType: number;
    @Input()
    factorVerificationConfigurationRoleId: number;
    closeActionEmitter = new EventEmitter<boolean>();
    @ViewChild(DetailComponentDirective) factorVerificationDescriptionEditComponentContainer: DetailComponentDirective;
    factorVerificationDescriptionEditComponent = FactorVerificationDescriptionEditComponent;
    uploadReport: PDFSource;
    systemReport: PDFSource;
    currentUser: any;
    authorizationHeader: any;
    showLargeUploadReport: boolean;
    showLargeSystemReport: boolean;
    customButtons: YashilCustomButton[] = [];
    requestForReject: boolean;
    allowConfirmOrRejectFactor = false;
    columns: any = [];
    invalidFactorItemDataSource: any;

    constructor(private componentFactoryResolver: ComponentFactoryResolver, private genericDataService: GenericDataService, private userDataService: UserDataService) {
        super(genericDataService);
        this.entityName = 'factorVerification';
        this.currentUser = this.userDataService.getCurrentUser();
        this.authorizationHeader = {
            Authorization: `Bearer ${this.currentUser.token}`
        };
        (window as any).pdfWorkerSrc = './assets/scripts/pdf.worker.min.js';
        // tslint:disable-next-line:max-line-length
        this.checkAllowConfirmOrRejectFactorVerificationAndBindCustomButtons = this.checkAllowConfirmOrRejectFactorVerificationAndBindCustomButtons.bind(this);
    }

    ngOnInit(): void {
        super.ngOnInit();
        this.showLargeSystemReport = false;
        this.showLargeUploadReport = false;
        this.requestForReject = false;
        if (this.factorConfirmationType) {
            this.entity.factorConfirmationType = this.factorConfirmationType;
        }
        if (this.factorVerificationConfigurationRoleId) {
            this.entity.factorVerificationConfigurationRoleId = this.factorVerificationConfigurationRoleId;
        }
        this.checkAllowConfirmOrRejectFactorVerificationAndBindCustomButtons();
    }

    /*
    برای بررسی فاکتور داده شده
    بررسی میکند که فاکتور آن
    آیتم های فاکتور با محاسبات اشتباهی نداشته باشد
    اگر آیتم فاکتور اشتباه داشته باشد اجازه رد یا تایید نمیدهد
    و آیتم های فاکتوری که ایراد دارد را نشان می دهد.
     */
    checkAllowConfirmOrRejectFactorVerificationAndBindCustomButtons(): void {
        // tslint:disable-next-line:max-line-length
        this.genericDataService.getEntitiesWithAction('factorItem', `GetInvalidFactorItemList?factorVerificationId=${this.selectedEntityId}`, null)
            .subscribe(response => {
                if (response && response.length) {
                    this.allowConfirmOrRejectFactor = false;
                    this.invalidFactorItemDataSource = response;
                    this.showError('کاربر گرامی، برخی از آیتم های فاکتور دارای محاسبه صحیحی نمی باشند و شما امکان تایید یا رد فاکتور را ندارید. لطفا آیتم های فاکتوری که مشکل دارد را بررسی کنید');
                    this.bindCustomButtons();
                    this.bindColumns();
                } else {
                    this.allowConfirmOrRejectFactor = true;
                    this.bindCustomButtons();
                    return;
                }
            });
    }

    afterLoadData(res: any): any {
        this.uploadReport = {
            url: `${window.location.origin}/api/factor/GetFactorPdf?factorId=${this.entity.factorId}`,
            withCredentials: true,
            httpHeaders: this.authorizationHeader
        };
        this.closeActionEmitter.subscribe(x => {
            this.showPopUp(false);
        });
        return super.afterLoadData(res);
    }

    /*
    تایید بررسی فاکتور
     */
    private confirmFactorVerification(): any {
        let message = 'خطا در بررسی تایید فاکتور';
        this.genericDataService.postEntityByUrl('/api/factorVerification/CheckExistenceSubsequentRecord', this.entity)
            .subscribe(response => {
                if (response === false) {
                    message = 'کاربر گرامی درصورتی که فاکتور را تایید نمایید، امکان ویرایش مجدد آن وجود ندارد، آیا از تایید خود اطمینان دارید؟';
                } else if (response === true) {
                    message = 'کاربر گرامی آیا مایل به تایید فاکتور میباشید؟';
                } else {
                    this.showError(message);
                    return;
                }
                this.declareFinalConfirm(message).show().then((dialogResult) => {
                    if (dialogResult.res === 'ok') {
                        this.confirmFactor();
                    } else {
                        return;
                    }
                });
            });
    }

    /*
    تایید نهایی فاکتور و تغییر آن به حالت تایید شده
     */
    private confirmFactor(): any {
        this.genericDataService.postEntityByUrl('/api/factorVerification/ConfirmFactorVerification', this.entity)
            .subscribe(response => {
                if (!response.isSuccess) {
                    this.showError(response.message);
                } else {
                    this.updateOrInsertAffected = true;
                    this.showMessage(response.message);
                }
            });
        return;
    }

    /*
    نشان دادن پیغام نهایی تایید فاکتور برای اخرین نفر
     */
    private declareFinalConfirm(message: string): any {
        return custom({
            title: 'تایید فاکتور',
            messageHtml: message,
            buttons: [{
                text: 'تایید',
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

    bindCustomButtons(): void {
        if (this.allowConfirmOrRejectFactor) {
            this.customButtons.push({
                icon: 'check',
                text: 'تایید فاکتور ',
                type: 'success',
                key: FactorVerificationConfirmationType.Confirm,
            }, {
                icon: 'clear',
                text: 'رد فاکتور ',
                type: 'danger',
                key: FactorVerificationConfirmationType.Reject,
            });
        }
        this.customButtons.push({
            icon: 'pdffile',
            text: 'دانلود گزارش سامانه',
            hint: 'ذپدانلود گزارش تولید شده توسط سامانه',
            type: 'default',
            key: 'download',
        });
    }

    showPopUp(status: boolean): void {
        this.requestForReject = status;
    }

    /*
    متد دکمه های تایید یا رد فاکتور و دانلود گزارش فاکتور
    این متد بر اساس key که به ان پاس داده میشود تصمیم میگیرد که چه دکمه ای کلیک شده و عملیات مربوط به ان را انجام میدهد
     */
    customButtonClicked(key: any): void {
        // دکمه تایید فاکتور
        if (key === FactorVerificationConfirmationType.Confirm) {
            this.confirmFactorVerification();
            // دکمه رد فاکتور
        } else if (key === FactorVerificationConfirmationType.Reject) {
            const componentFactory = this.componentFactoryResolver.resolveComponentFactory(this.factorVerificationDescriptionEditComponent);
            const viewContainerRef = this.factorVerificationDescriptionEditComponentContainer.viewContainerRef;
            viewContainerRef.clear();
            const componentRef = viewContainerRef.createComponent(componentFactory);
            const editComponent = (componentRef.instance as FactorVerificationDescriptionEditComponent);
            editComponent.selectedEntityId = this.entity.id;
            editComponent.showCloseButton = true;
            editComponent.closeFormClick = this.closeActionEmitter;
            editComponent.onFactorVerificationRejected.subscribe(x => {
                this.updateOrInsertAffected = true;
            });
            this.showPopUp(true);
            // دکمه دانلود فاکتور
        } else if (key === 'download') {
            const id = this.entity.id;
            const url = `${window.location.origin}/api/factor/GenerateVerifierFactorFile?factorVerificationId=${id}`;
            window.open(url, '_blank');
        }
    }

    downloadUploadedReport(e: any): void {
        const url = `${window.location.origin}/api/factor/GetFactorPdf?factorId=${this.entity.factorId}`;
        window.open(url, '_blank');
    }

    bindColumns(): void {
        this.columns.push(
            {
                caption: 'آیتم سفارش',
                dataField: 'id',
                allowSorting: false,
                allowHeaderFiltering: false
            }, {
                caption: 'شناسه سفارش',
                dataField: 'orderTitle',
                allowSorting: false,
                allowHeaderFiltering: false
            }, {
                caption: 'قیمت با تخفیف',
                dataField: 'orderItemPriceIncDiscount',
                allowSorting: false,
                allowHeaderFiltering: false,
                cellTemplate: 'number'
            },
            {
                caption: 'قیمت با تخفیف صحیح',
                dataField: 'correctOrderItemPriceIncDiscount',
                allowSorting: false,
                allowHeaderFiltering: false,
                cellTemplate: 'number'
            },
            {
                caption: 'تخفیف',
                dataField: 'profitPrice',
                allowSorting: false,
                allowHeaderFiltering: false,
                cellTemplate: 'number'
            },
            {
                caption: 'تخفیف صحیح',
                allowSorting: false,
                dataField: 'correctProfitPrice',
                allowHeaderFiltering: false,
                cellTemplate: 'number'
            }, {
                caption: 'تخفیف بدون مالیات',
                dataField: 'discountAmountExclTax',
                allowSorting: false,
                allowHeaderFiltering: false,
                cellTemplate: 'number'
            }, {
                caption: 'تخفیف بدون مالیات صحیح',
                dataField: 'correctDiscountAmountExclTax',
                allowSorting: false,
                allowHeaderFiltering: false,
                cellTemplate: 'number'
            }, {
                caption: 'کارمزد بدون مالیات',
                dataField: 'wageAmountExcTax',
                allowSorting: false,
                allowHeaderFiltering: false,
                cellTemplate: 'number'
            },
            {
                caption: 'کارمزد بدون مالیات صحیح',
                dataField: 'correctWageAmountExcTax',
                allowSorting: false,
                allowHeaderFiltering: false,
                cellTemplate: 'number'
            });
    }
}
