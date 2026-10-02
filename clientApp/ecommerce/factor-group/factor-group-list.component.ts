import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';
import {Component, ComponentFactoryResolver, EventEmitter, Input, OnInit, Output, ViewChild} from '@angular/core';
import {FactorGroupDetailComponent} from './factor-group-detail.component';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {FactorGroupStatusEnumType} from './factor-group-enums';
import {FactorGroupDetailTabBasedComponent} from './factor-group-detail-tab-based.component';
import {AssignableListComponent} from '../../../shared/base/core/components/assignable-list/assignable-list.component';
import {DetailComponentDirective} from '../../../shared/base/base-list-form/detail-component.directive';
import {custom} from 'devextreme/ui/dialog';
import {FactorGroupReportTabBasedComponent} from '../factor-group-report/factor-group-report-tab-based.component';
import {YashilCustomButtonColumn} from '../../../shared/base/classes/yashil-cutom-button-column';


@Component({
    selector: 'app-factor-group-list',
    templateUrl: './factor-group-list.component.html'
})
export class FactorGroupListComponent extends Selectable implements OnInit {
    @ViewChild('listForm', {static: true}) listForm: BaseList;
    loadAfterSetFilter: boolean;
    selectedItemId: number;
    columns: any[] = [];
    customRowButtons: YashilCustomButtonColumn;
    entityName = 'factorGroup';
    popupTitle: string;
    detailComponent = FactorGroupDetailTabBasedComponent;
    @Input()
    gridHeight: number;
    @Output()
    listRefreshed = new EventEmitter();
    @ViewChild(DetailComponentDirective) componentContainer: DetailComponentDirective;
    factorGroupItemComponent = AssignableListComponent;
    factorGroupReportTabBasedComponent = FactorGroupReportTabBasedComponent;
    showPopUp: boolean;
    showCategoryGroupDropDownLis: boolean;
    @Input()
    showAddButton = true;
    private baseListUrl = 'factorGroup/GetByCustomFilterForList?';
    showConvertValidationPopUp = false;
    factorGroupId: number;
    factorGroupTitle: string;
    factorColumns: any[] = [];
    assignableListComponent: any;
    @Output()
    confirmFactorStart = new EventEmitter();
    @Output()
    confirmFactorEnd = new EventEmitter();

    constructor(private componentFactoryResolver: ComponentFactoryResolver, private genericDataService: GenericDataService) {
        super();
        this.confirmFactorGroup = this.confirmFactorGroup.bind(this);
        this.showConfirmFactorGroupBtn = this.showConfirmFactorGroupBtn.bind(this);
        this.modifyFactorGroupItems = this.modifyFactorGroupItems.bind(this);
        this.showFactorGroupItemBtn = this.showFactorGroupItemBtn.bind(this);
        this.reportFactorGroupBtn = this.reportFactorGroupBtn.bind(this);
        this.showReportFactorGroupBtn = this.showReportFactorGroupBtn.bind(this);
        this.changeFactorGroupStateToInitial = this.changeFactorGroupStateToInitial.bind(this);
        this.confirmChangeFactorGroupToInitialStatus = this.confirmChangeFactorGroupToInitialStatus.bind(this)
    }

    // tslint:disable-next-line:variable-name
    _parentId: number;
    convertValidationMessage: string;

    get parentId(): number {
        return this._parentId;
    }

    @Input()
    set parentId(val) {
        if (val !== this._parentId) {
            this._parentId = val;
        }
    }

    ngOnInit(): void {
        if (!this.gridHeight) {
            this.gridHeight = window.innerHeight - 150;
        }
        this.bindColumns();
        if (this.bindCustomDataSources()) {
            this.loadAfterSetFilter = true;
        }
    }

    public bindCustomDataSources(): boolean {
        let customListUrl = `${this.baseListUrl}`;
        if (this.listForm) {
            if (this.parentId) {
                customListUrl += `parentId=${this.parentId}&`;
            }
        }
        let res = false;
        if (this.parentId) {
            this.listForm.customListUrl = customListUrl;
            this.refreshList();
            res = true;
        }
        return res;
    }

    afterInitialDetailComponent(componentInstance: any): void {
        const comp = (componentInstance.instance as FactorGroupDetailComponent);
        if (this.parentId) {
            comp.parentId = this.parentId;
        }
    }

    /**
     * تایید نهایی گروه بندی فاکتور
     */
    confirmFactorGroup(item): void {
        this.verifyConfirmMessageGroup().show().then((dialogResult) => {
            if (dialogResult.res === 'ok') {
                this.confirmFactorGroupStatus(item);
            } else {
                return;
            }
        });
    }

    /*
    نمایش تاییدیه کاربر برای تغییر وضعیت گروه قاکتور
     */
    confirmChangeFactorGroupToInitialStatus(item): void {
        this.verifyChangeFactorGroupToInitialStatusMessage().show().then((dialogResult) => {
            if (dialogResult.res === 'ok') {
                this.changeFactorGroupStateToInitial(item);
            } else {
                return;
            }
        });
    }
    /*
        نمایش پیام های تاییدیه کاربر برای تغییر وضعیت گروه قاکتور
         */
    private verifyChangeFactorGroupToInitialStatusMessage(): any {
        return custom({
            title: 'تغییر وضعیت گروه فاکتور',
            messageHtml: 'کاربر گرامی در صورت شروع و انجام فرآیند مالی شما مجاز به تغییر نمی باشید، با زدن دکمه تایید کلیه مسولیت های مالی این گروه فاکتور به عهده شما می باشد، آیا از تغییر وضعیت گروه فاکتور به ثبت اولیه اطمینان دارید؟',
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
    /**
     * اضافه یا کم کردن آیتم های یک گروه
     */
    modifyFactorGroupItems(item): void {
        this.factorColumns = [
            {
                caption: 'شماره فاکتور',
                dataField: 'code'
            },
            {
                caption: 'گروه دسته بندی',
                dataField: 'categoryGroupTitle'
            },
            {
                caption: 'عنوان',
                dataField: 'title'
            },
            {
                caption: 'دسته بندی مالیاتی',
                dataField: 'taxCategoryTitle'
            },

            {
                caption: 'سود(%)',
                dataField: 'profitPercentage',
                width: '35px',
            },
            {
                caption: 'تخفیف(%)',
                dataField: 'discountPercentage',
                width: '35px',
            },
          {
            caption: 'مرکز',
            dataField: 'companyTitle',
          },
          {
            caption: 'نمایندگی',
            dataField: 'representationTitle',
          }
        ];
        this.popupTitle = 'آیتم های گروه بندی فاکتور';
        const componentFactory = this.componentFactoryResolver.resolveComponentFactory(this.factorGroupItemComponent);
        const viewContainerRef = this.componentContainer.viewContainerRef;
        viewContainerRef.clear();
        const componentRef = viewContainerRef.createComponent(componentFactory);
        this.assignableListComponent = (componentRef.instance as AssignableListComponent);
        this.assignableListComponent.selectedGroupItemId = +(item.row.data.id);
        this.assignableListComponent.showGroupItemComponent = false;
        this.assignableListComponent.groupEntityLabel = 'گروه فاکتور';
        this.assignableListComponent.assignableListName = 'Factor';
        this.assignableListComponent.groupEntityName = 'FactorGroup';
        this.assignableListComponent.assignableListLabel = 'فاکتور';
        this.assignableListComponent.columns = this.factorColumns;
        this.assignableListComponent.assignedColumns = this.factorColumns;
        this.assignableListComponent.selectedGroupItemTitle = (item.row.data.title);
        this.showPopUp = true;
        this.showCategoryGroupDropDownLis = true;
    }

    showFactorGroupItemBtn(item): boolean {
        if (item.row.node.hasChildren || item.row.data.factorGroupStatus === FactorGroupStatusEnumType.Confirm) {
            return false;
        } else if (item.row.data.factorGroupStatus === FactorGroupStatusEnumType.Initial && item.row.data.factorGroupItemCount >= 0) {
            return true;
        }
        return false;
    }

    showConfirmFactorGroupBtn(item): boolean {
        if (item.row.node.hasChildren || item.row.data.factorGroupStatus === FactorGroupStatusEnumType.Confirm) {
            return false;
        } else if (item.row.data.factorGroupStatus === FactorGroupStatusEnumType.Initial && item.row.data.factorGroupItemCount > 0) {
            return true;
        }
        return false;
    }

    reportFactorGroupBtn(item): void {
        this.popupTitle = 'گزارش های کانورت مالی';
        const componentFactory = this.componentFactoryResolver.resolveComponentFactory(this.factorGroupReportTabBasedComponent);
        const viewContainerRef = this.componentContainer.viewContainerRef;
        viewContainerRef.clear();
        const componentRef = viewContainerRef.createComponent(componentFactory);
        const factorGroupReportTabBasedComponent = (componentRef.instance as FactorGroupReportTabBasedComponent);
        factorGroupReportTabBasedComponent.factorGroupId = (item.row.data.id);
        this.showPopUp = true;
        this.showCategoryGroupDropDownLis = false;
    }

    showReportFactorGroupBtn(item): boolean {
        return item.row.data.factorGroupStatus === FactorGroupStatusEnumType.Confirm;
    }

    onListRowPrepared(item): void {
        if (item.rowType === 'data') {
            item.data.allowEdit = this.allowEditOrDeleteItem(item);
            item.data.allowDelete = this.allowEditOrDeleteItem(item);
        }
    }

    allowEditOrDeleteItem(item): boolean {
        if (item.node.hasChildren || item.data.factorGroupStatus === FactorGroupStatusEnumType.Confirm) {
            return false;
        } else if (item.data.factorGroupStatus === FactorGroupStatusEnumType.Initial) {
            return true;
        }
        return false;
    }

    closePopUpBtn(): void {
        this.showPopUp = false;
        this.refreshList();
    }

    rowSelected(item: any): void {
        this.selectedRowChanged.emit(item);
        /*
           نمایش دکمه اضافه کردن را تغییری نمیدهیم
          تا در هر صورت کاربر بتواند گروه ایجاد کند تنها گار گروه مناسبی انتخاب
           شده باشد این گروه در فرم دیتیل نیز بعنوان گروه پدر انتخاب شده است
          */
        if (item.factorGroupStatus === FactorGroupStatusEnumType.Initial
            && item.childsCount >= 0
            && !item.allowConfirmFactorGroupBtn
            && !item.allowGenerateFactorReport) {
            this.parentId = item.id;
        } else {
            this.parentId = null;
        }
    }

    downloadFullReportFactorGroupBtn(item: any): void {
        const url = `${window.location.origin}/api/factorGroup/GenerateFactorGroupConvertFullReportFile?factorGroupId=${item.row.data.id}`;
        window.open(url, '_blank');
    }

    refreshList(): void {
        this.listForm.refreshList();
        this.listRefreshed.emit();
    }

    private bindColumns(): void {
        this.columns.push({
            caption: 'گروه کانورت',
            dataField: 'title',
            width: '150px',
        });
        this.customRowButtons = {
            customButtons: [
                {
                    text: 'ثبت نهایی',
                    type: 'danger',
                    hint: 'ثبت نهایی گروه جهت دریافت گزارش',
                    icon: 'save',
                    onClick: this.confirmFactorGroup,
                    visibleProp: 'allowConfirmFactorGroupBtn'
                },
                {
                    icon: 'bulletlist',
                    text: 'آیتم های گروه',
                    hint: 'آیتم های گروه',
                    type: 'default',
                    onClick: this.modifyFactorGroupItems,
                    visibleProp: 'allowModifyFactorGroupItems'
                },
                {
                    icon: 'columnchooser',
                    text: 'جزییات کانورت',
                    hint: 'مشاهده و دانلود گزارش به تفکیک',
                    type: 'success',
                    onClick: this.reportFactorGroupBtn,
                    visibleProp: 'allowGenerateFactorReport'
                },
                {
                    icon: 'columnchooser',
                    text: 'دانلود جامع گزارش',
                    hint: 'مشاهده و دانلود گزارش به صورت کلی',
                    type: 'default',
                    onClick: this.downloadFullReportFactorGroupBtn,
                    visibleProp: 'allowGenerateFactorReport'
                },
                {
                    icon: 'columnchooser',
                    text: 'تغییر به ثبت اولیه',
                    hint: 'تغییر به ثبت اولیه',
                    type: 'danger',
                    onClick: this.confirmChangeFactorGroupToInitialStatus,
                    visibleProp: 'allowGenerateFactorReport'
                }
            ]
        };

    }

    private verifyConfirmMessageGroup(): any {
        return custom({
            title: 'ثبت نهایی گروه بندی',
            messageHtml: 'کاربر گرامی پس از ثبت نهایی گروه امکان ویرایش آن وجود ندارد. آیا مایل به تایید می باشید؟',
            buttons: [{
                text: 'ثبت نهایی',
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



    /***
     *در این متد وضعیت فاکتور به حالت ثبت نهایی در می اید
     */
    private confirmFactorGroupStatus(item): void {
        this.factorGroupId = item.row.data.id;
        this.confirmFactorStart.emit();
        this.genericDataService.postEntityByUrl('/api/factorGroup/confirmFactorGroup', {id: this.factorGroupId})
            .subscribe(response => {
                    if (!response.isSuccess) {
                        this.convertValidationMessage = response.message;
                        this.showConvertValidationPopUp = true;
                    } else {
                        this.refreshList();
                        this.showMessage('عملیات بروزرسانی با موفقیت انجام شد');

                    }
                    this.confirmFactorEnd.emit();
                },
                error => {
                    this.confirmFactorEnd.emit();
                    this.showError('خط در ثبت نهایی گروه بندی فاکتور');
                });
    }

    /*
    وقتی در پاپ آپ انتخاب فاکتور گروه دسته بندی تغییر می کند
     */
    onCategoryGroupChanged(item: any): void {
        if (item) {
            this.assignableListComponent.notAssignedMethodExtraQueryParam = `categoryGroupId=${item.id}`;
        } else {
            this.assignableListComponent.notAssignedMethodExtraQueryParam = null;
        }
        this.assignableListComponent.bindLists();
    }

    /*
    خارج کردن فاکتور های گروه
     */
    private changeFactorGroupStateToInitial(item): void {
        this.factorGroupId = item.row.data.id;
        this.genericDataService.postEntityByUrl('/api/factorGroup/ChangeFactorGroupStateToInitial', {id: this.factorGroupId})
            .subscribe(response => {
                if (!response.isSuccess) {
                    this.showError(response.message);
                } else {
                    this.refreshList();
                    this.showMessage(response.message);
                }
            });
    }
}
