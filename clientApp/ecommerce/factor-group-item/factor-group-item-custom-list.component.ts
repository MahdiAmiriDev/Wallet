import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';
import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {FactorDetailComponent} from '../factor/factor-detail.component';
import {YashilCustomButtonColumn} from '../../../shared/base/classes/yashil-cutom-button-column';
import {custom} from 'devextreme/ui/dialog';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';


@Component({
    selector: 'app-factor-group-custom-list',
    templateUrl: './factor-group-item-custom-list.component.html'
})
export class FactorGroupItemCustomListComponent extends Selectable implements OnInit {
    @ViewChild('factorListForm', {static: true}) factorListForm: BaseList;
    loadAfterSetFilter: boolean;
    selectedItemId: number;
    showPopUp: boolean;
    columns: any[] = [];
    entityName = 'factor';
    detailComponent = FactorDetailComponent;
    contentHeight: number;
    selectedFactorGroup: any;
    popUpTitle: string;
    customRowButtons: YashilCustomButtonColumn;
    comparisonHekmatTransactionListHeight: number;
    private baseListUrl = 'factor/GetByCustomFilterForFactorGroupList?';

    constructor(private genericDataService: GenericDataService) {
        super();
        this.contentHeight = window.innerHeight - 110;
        this.comparisonHekmatTransactionListHeight = window.innerHeight - 350;
        this.factorComparisonHekmatTransactions = this.factorComparisonHekmatTransactions.bind(this);
        this.RemoveFactorFromFactorGroup = this.RemoveFactorFromFactorGroup.bind(this);
        this.showRemoveFromFactorGroupMessageConfirm = this.showRemoveFromFactorGroupMessageConfirm.bind(this);
    }

    // tslint:disable-next-line:variable-name
    _factorId: number;

    get factorId(): number {
        return this._factorId;
    }

    @Input()
    set factorId(val) {
        if (val !== this._factorId) {
            this._factorId = val;
        }
    }

    // tslint:disable-next-line:variable-name
    _factorGroupId: number;

    get factorGroupId(): number {
        return this._factorGroupId;
    }

    @Input()
    set factorGroupId(val) {
        if (val !== this._factorGroupId) {
            this._factorGroupId = val;
        }
    }

    ngOnInit(): void {
        this.bindColumns();
        this.factorListForm.customListUrl = `${this.baseListUrl}`;
        if (this.bindCustomDataSources()) {
            this.loadAfterSetFilter = true;
        }
        this.bindCustomRowButtons();
    }

    public bindCustomDataSources(): boolean {
        let customListUrl = `${this.baseListUrl}`;
        if (this.factorListForm) {
            if (this.factorId) {
                customListUrl += `factorId=${this.factorId}&`;
            }
            if (this.factorGroupId) {
                customListUrl += `factorGroupId=${this.factorGroupId}&`;
            }
        }
        let res = false;
        if (this.factorId || this.factorGroupId) {
            this.factorListForm.customListUrl = customListUrl;
            this.factorListForm.refreshList();
            res = true;
        }
        return res;
    }

    afterInitialDetailComponent(componentInstance: any): void {
        const comp = (componentInstance.instance as FactorDetailComponent);
    }

    /*
    هنگامی که گروه انتخابی عوض شود باید مجددا لیست آیتمهای گروه رفرش شود
     */
    selectedFactorGroupChanged(item: any): void {
        this.selectedFactorGroup = item;
        if (this.selectedFactorGroup) {
            this.factorGroupId = item.id;
        }
        this.bindCustomDataSources();
    }

    bindCustomRowButtons(): void {
        this.customRowButtons = {
            width: '180px',
            setAtFirstColumn: true,
            customButtons: [
                {
                    text: 'اعتبارسنجی تراکنش حکمت',
                    hint: 'اعتبار سنجی تراکنشات حکمت',
                    icon: 'checklist',
                    type: 'default',
                    onClick: this.factorComparisonHekmatTransactions,
                    width: '180px'
                }, {
                    text: 'حذف از گروه',
                    hint: 'حذف از گروه',
                    icon: 'save',
                    type: 'danger',
                    visibleProp: 'allowRemoveFromGroup',
                    onClick: this.RemoveFactorFromFactorGroup,
                    width: '100px'
                }
            ]
        };
    }

    /*
    جهت نمایش فرم اعتبار سنجی تراکنش های حکمت
     */
    factorComparisonHekmatTransactions(item: any): void {// مقداردهی پارامترهای ورودی
        this.factorId = item.row.data.id;
        this.popUpTitle = 'اعتبار سنجی تراکنشات حکمت';
        this.showPopUp = true;
    }


    RemoveFactorFromFactorGroup(item: any): void {// مقداردهی پارامترهای ورودی
        this.factorId = item.row.data.id;
        this.showRemoveFromFactorGroupMessageConfirm().show().then((dialogResult) => {
            if (dialogResult.res === 'ok') {
                this.genericDataService.postEntityByUrl(`/api/factor/RemoveFactorFromFactorGroup/${this.factorId}`, {id: this.factorId})
                    .subscribe(response => {
                        if (!response.isSuccess) {
                            this.showError(response.message);
                        } else {
                            this.showMessage(response.message);
                        }
                        this.factorListForm.refreshList();
                    });
                this.factorListForm.refreshList();
            }
            return;
        });
    }


    /*
    * نماایش پیغام حذف از گروه
    *  */
    private showRemoveFromFactorGroupMessageConfirm(): any {
        return custom({
            title: 'حذف از گروه کانورت',
            messageHtml: 'کاربر گرامی آیا تمایل به حذف گروه از گروه کانورت دارید؟',
            buttons: [{
                text: 'حذف از گروه کانورت',
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

    private bindColumns(): void {
        this.columns.push({
            caption: 'شماره فاکتور',
            dataField: 'code',
            width: '200px',
        }, {
            caption: 'گروه دسته بندی',
            dataField: 'categoryGroupTitle',
            width: '120px',
        }, {
            caption: 'مرکز',
            dataField: 'companyTitle',
            width: '100px',
        }, {
            caption: 'نمایندگی',
            dataField: 'representationTitle',
            width: '120px',
        }, {
            caption: 'دسته بندی مالیات',
            dataField: 'taxCategoryTitle',
            width: '120px',
        }, {
            caption: 'تاریخ صدور',
            dataField: 'issueDate',
            cellTemplate: 'intDate',
            allowHeaderFiltering: false,
            width: '120px',
        });
    }
}
