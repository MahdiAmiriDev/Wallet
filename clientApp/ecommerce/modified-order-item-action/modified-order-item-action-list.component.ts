import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';
import {Component, EventEmitter, Input, OnInit, ViewChild} from '@angular/core';
import {ModifiedOrderItemActionDetailComponent} from './modified-order-item-action-detail.component';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {YashilCustomButtonColumn} from '../../../shared/base/classes/yashil-cutom-button-column';
import {first} from "rxjs/operators";


@Component({
    selector: 'app-modified-order-item-action-list',
    templateUrl: './modified-order-item-action-list.component.html'
})
export class ModifiedOrderItemActionListComponent extends Selectable implements OnInit {
    @ViewChild('listForm', {static: true}) listForm: BaseList;
    loadAfterSetFilter: boolean;
    selectedItemData: any;
    columns: any[] = [];
    entityName = 'modifiedOrderItem';
    detailComponent = ModifiedOrderItemActionDetailComponent;
    customRowButtons: YashilCustomButtonColumn;
    // tslint:disable-next-line:variable-name
    _oldOrderItemId: number;
    applyEvent: EventEmitter<any> = new EventEmitter<any>();


    @Input()
    set oldOrderItemId(val) {
        if (val !== this._oldOrderItemId) {
            this._oldOrderItemId = val;
        }
    }

    get oldOrderItemId(): number {
        return this._oldOrderItemId;
    }

    // tslint:disable-next-line:variable-name
    _newOrderId: number;
    @Input()
    set newOrderId(val) {
        if (val !== this._newOrderId) {
            this._newOrderId = val;
        }
    }

    get newOrderId(): number {
        return this._newOrderId;
    }

    // tslint:disable-next-line:variable-name
    _modifiedOrderItemType: number;
    @Input()
    set modifiedOrderItemType(val) {
        if (val !== this._modifiedOrderItemType) {
            this._modifiedOrderItemType = val;
        }
    }

    get modifiedOrderItemType(): number {
        return this._modifiedOrderItemType;
    }

    // tslint:disable-next-line:variable-name
    _modifiedOrderItemStatusType: number;
    @Input()
    set modifiedOrderItemStatusType(val) {
        if (val !== this._modifiedOrderItemStatusType) {
            this._modifiedOrderItemStatusType = val;
        }
    }

    get modifiedOrderItemStatusType(): number {
        return this._modifiedOrderItemStatusType;
    }

    // tslint:disable-next-line:variable-name
    _assignee: number;
    @Input()
    set assignee(val) {
        if (val !== this._assignee) {
            this._assignee = val;
        }
    }

    get assignee(): number {
        return this._assignee;
    }

    showDescriptionPopUp: boolean = false;
    expertDescription: string;

    private baseListUrl = 'modifiedOrderItem/GetByCustomFilterForModifyAction?';

    constructor(private genericDataService: GenericDataService) {
        super();
        this.refundOrderAndPayment = this.refundOrderAndPayment.bind(this);
        this.editOrder = this.editOrder.bind(this);
        this.rejectRequest = this.rejectRequest.bind(this);
        this.cancelModification = this.cancelModification.bind(this);
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
            caption: 'تاریخ ثبت',
            dataField: 'creationDate',
            cellTemplate: 'jalaliDate',
            allowHeaderFiltering: false,
            width: '80px',
            allowGrouping: 0
        }, {
            caption: 'نوع',
            dataField: 'modifiedOrderItemTypeTitle',
            width: '100px',
            groupIndex: 1
        }, {
            caption: 'کد پیگیری',
            dataField: 'lastOrderItemCode',
            allowHeaderFiltering: false,
            width: '100px',
            allowGrouping: 0
        }, {
            caption: 'کد آیتم سفارش',
            dataField: 'lastOrderItemId',
            allowHeaderFiltering: false,
            width: '60px',
            allowGrouping: 0
        }, {
            caption: 'تعداد',
            dataField: 'quantity',
            allowHeaderFiltering: false,
            width: '50px',
            allowGrouping: 0
        }, {
            caption: 'کد ملی تحویل گیرنده',
            dataField: 'nationalCode',
            allowHeaderFiltering: false,
            width: '100px',
            allowGrouping: 0
        }, {
            caption: 'شماره موبایل',
            dataField: 'mobileNumber',
            allowHeaderFiltering: false,
            width: '100px',
            allowGrouping: 0
        }, {
            caption: 'نام محصول',
            dataField: 'lastOrderItemTitle',
            allowHeaderFiltering: false,
            minWidth: '150px',
            width: '150px',
            allowGrouping: 0
        }, {
            caption: 'مرکز',
            dataField: 'companyTitle',
            width: '80px',
            allowGrouping: 0
        }, {
            caption: 'نمایندگی',
            dataField: 'representationTitle',
            width: '80px',
            allowGrouping: 0
        }, {
            caption: 'وضعیت ',
            dataField: 'modifiedOrderItemStatusTypeTitle',
            width: '80px',
            allowGrouping: 0
        }, {
            caption: 'عنوان آیتم قبلی سفارش',
            dataField: 'oldOrderItemTitle',
            allowHeaderFiltering: false,
            hidingPriority: 1,
            width: '100px',
            allowGrouping: 0
        }, {
            caption: 'توضیحات',
            dataField: 'description',
            allowHeaderFiltering: false,
            hidingPriority: 0,
            allowGrouping: 0
        });
    }

    bindCustomDataSources(): boolean {
        let customListUrl = `${this.baseListUrl}`;
        if (this.listForm) {
            if (this.oldOrderItemId) {
                customListUrl += `oldOrderItemId=${this.oldOrderItemId}&`;
            }
            if (this.newOrderId) {
                customListUrl += `newOrderId=${this.newOrderId}&`;
            }
            if (this.modifiedOrderItemType) {
                customListUrl += `modifiedOrderItemType=${this.modifiedOrderItemType}&`;
            }
            if (this.modifiedOrderItemStatusType) {
                customListUrl += `modifiedOrderItemStatusType=${this.modifiedOrderItemStatusType}&`;
            }
            if (this.assignee) {
                customListUrl += `assignee=${this.assignee}&`;
            }
        }
        let res: boolean;
        this.listForm.customListUrl = customListUrl;
        this.listForm.refreshList();
        res = true;
        return res;

    }

    afterInitialDetailComponent(componentInstance: any): void {
        const comp = (componentInstance.instance as ModifiedOrderItemActionDetailComponent);

        if (this.oldOrderItemId) {
            comp.oldOrderItemId = this.oldOrderItemId;
        }
        comp.oldItemForChange = componentInstance.selectedItem;
        if (this.newOrderId) {
            comp.newOrderId = this.newOrderId;
        }

        if (this.modifiedOrderItemType) {
            comp.modifiedOrderItemType = this.modifiedOrderItemType;
        }

        if (this.modifiedOrderItemStatusType) {
            comp.modifiedOrderItemStatusType = this.modifiedOrderItemStatusType;
        }

        if (this.assignee) {
            comp.assignee = this.assignee;
        }
        if (componentInstance.selectedItem.orderId) {
            comp.orderId = componentInstance.selectedItem.orderId;
        }

        comp.expertDescription = this.expertDescription;
    }

    private createCustomButtons(): void {
        this.customRowButtons = {
            width: '150px',
            customButtons: [
                {
                    hint: 'لغو سفارش',
                    text: 'لغو',
                    width: '70px',
                    type: 'danger',
                    onClick: this.refundOrderAndPayment,
                    visibleProp: 'allowRefundOrderAndPayment'
                },
                {
                    hint: 'تغییر سفارش',
                    text: 'تغییر',
                    width: '70px',
                    type: 'default',
                    onClick: this.editOrder,
                    visibleProp: 'allowEdiModifiedOrder'
                },
                {
                    hint: 'رد کردن درخواست تغییر یا لغو سفارش',
                    text: 'رد',
                    width: '70px',
                    type: 'success',
                    onClick: this.rejectRequest,
                    visibleProp: 'allowRejectRequest'
                },
                {
                    hint: 'آیتم سفارش تغییر داده شده یا لغو شده را به آیتم قبل از تغییر یا لغو برمی گرداند',
                    text: 'بازگشت به قبل تغییر',
                    width: '150px',
                    type: 'danger',
                    onClick: this.cancelModification,
                    visibleProp: 'allowCancelModification'
                }
            ]
        };
    }


    /*
        لغو سفارش
        CancelOrderItem = 1195,
        تغییر سفارش
        ChangeOrderItem = 1196
    */
    rowPrepared(e:any): void {
    }

    /**
     * Open Modify OrderItem Window
     * @param selectedItem selected Grid Row
     */
    editOrder(selectedItem: any): void {
        const selectedEntityId = selectedItem.row.data.id;
        this.genericDataService.getEntitiesWithAction(
            'modifiedOrderItem',
            `checkOrderItemHasBeenFactored?modifiedOrderItemId=${selectedEntityId}`,
            null
        ).subscribe((res: any) => {
            if (res.isSuccess) {
                this.showMessageOnModifyOrderItem(
                    'کاربر گرامی سفارش انتخاب شده فاکتور شده می باشد، آیا از تغییر سفارش اطمینان دارید؟',
                    'تغییر سفارش'
                );
            } else {
                this.listForm.editEntity(selectedEntityId);
            }
        });
    }

    showMessageOnModifyOrderItem(message: string, title: string) {
        this.confirmBeforeAction(title, message, title).show().then((dialogResult) => {
            if (dialogResult.res === 'ok') {
                this.showDescriptionPopUp = true;
                this.applyEvent.pipe(first()).subscribe(() => {
                    this.listForm.editEntity(this.selectedItemData.id);
                });
            } else {
                this.showDescriptionPopUp = false;
            }
        });
    }


    apply() {
        this.applyEvent.emit();
        this.expertDescription = '';
        this.showDescriptionPopUp = false;
    }

    /**
     * رد کردن درخواست تغییر سفارش
     * @param selectedItem selected Grid Row
     */
    rejectRequest(selectedItem: any): void {
        this.confirmBeforeAction(
            'رد کردن درخواست تغییر سفارش',
            'آیا از رد کردن درخواست تغییر یا لغو سفارش اطمینان دارید؟',
            'رد درخواست'
        ).show().then((dialogResult) => {
            if (dialogResult.res === 'ok') {
                this.showDescriptionPopUp = true;
                this.applyEvent.pipe(first()).subscribe(() => {
                    this.busyIndicator.setBusy(true);
                    const selectedEntityId = selectedItem.row.data.id;
                    this.genericDataService.updateEntity('ModifiedOrderItem', {
                        id: selectedEntityId,
                        rejectRequest: true,
                        expertDescription: this.expertDescription,
                    }, 'UpdateNewOrderItem')
                        .subscribe(_ => {
                            this.busyIndicator.setBusy(false);
                            this.listForm.refreshList();
                        });
                });
            }
        });
    }

    /**
     * لغو سفارش
     * @param selectedItem selected Grid Row
     */
    refundOrderAndPayment(selectedItem: any): void {
        const selectedEntityId = selectedItem.row.data.id;
        this.genericDataService.getEntitiesWithAction('modifiedOrderItem', `checkOrderItemHasBeenFactored?modifiedOrderItemId=${selectedEntityId}`, null)
            .subscribe((res: any) => {
                if (res.isSuccess) {
                    this.cancelOrderItem(selectedEntityId, 'کاربر گرامی سفارش انتخاب شده فاکتور شده می باشد، آیا از لغو سفارش اطمینان دارید؟', 'لغو سفارش');
                } else {
                    this.cancelOrderItem(selectedEntityId, 'آیا از لغو سفارش اطمینان دارید؟', 'لغو سفارش');
                }
            });
    }

    cancelOrderItem(modifiedOrderItem: number, message: string, title: string) {
        this.confirmBeforeAction(title, message, title).show().then((dialogResult) => {
            if (dialogResult.res === 'ok') {
                this.busyIndicator.setBusy(true);
                this.genericDataService.updateEntity('ModifiedOrderItem', {
                    id: modifiedOrderItem
                }, 'UpdateNewOrderItem')
                    .subscribe(response => {
                        this.showMessage(response.message);
                        this.busyIndicator.setBusy(false);
                        this.listForm.refreshList();
                    });
            }
        });
    }

    setSelectedRow(item: any): void {
        this.selectedRowChanged.emit(item);
        this.selectedItemData = item;
    }


    /*
       آیتم سفارش اصلاح شده را به قبل برمی گرداند
   */
    cancelModification(selectedItem: any): void {
        this.confirmBeforeAction('برگشت آیتم سفارش به قبل از تغییر یا لغو',
            'آیا از برگشت آیتم سفارش تغییر داده شده یا لغو شده به آیتم سفارش قبل از تغییر یا لغو، اطمینان دارید؟', 'تایید').show().then((dialogResult) => {
            if (dialogResult.res === 'ok') {
                this.busyIndicator.setBusy(true);
                const selectedEntityId = selectedItem.row.data.id;
                this.genericDataService.postEntityByUrl(`/api/ModifiedOrderItem/CancelModification`, {id: selectedEntityId})
                    .subscribe(response => {
                        this.busyIndicator.setBusy(false);
                        if (response.isSuccess) {
                            this.showMessage(response.message);
                            this.listForm.refreshList();
                        } else {
                            this.showError(response.message);
                            this.listForm.refreshList();
                        }

                    });
            }
        });
    }
}
