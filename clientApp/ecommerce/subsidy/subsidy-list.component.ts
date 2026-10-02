import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';
import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {SubsidyDetailComponent} from './subsidy-detail.component';


@Component({
    selector: 'app-subsidy-list',
    templateUrl: './subsidy-list.component.html'
})
export class SubsidyListComponent extends Selectable implements OnInit {
    @ViewChild('listForm', {static: true}) listForm: BaseList;
    loadAfterSetFilter: boolean;
    selectedItemId: number;
    columns: any[] = [];
    entityName = 'subsidy';
    detailComponent = SubsidyDetailComponent;
    private baseListUrl = 'subsidy/GetByCustomFilterForList?';

    _subsidyGroupId: number;
    @Input()
    set subsidyGroupId(val) {
        if (val !== this._subsidyGroupId) {
            this._subsidyGroupId = val;
        }
    }

    get subsidyGroupId(): number {
        return this._subsidyGroupId;
    }

    _categoryGroupId: number;
    @Input()
    set categoryGroupId(val) {
        if (val !== this._categoryGroupId) {
            this._categoryGroupId = val;
        }
    }

    get categoryGroupId(): number {
        return this._categoryGroupId;
    }

    constructor() {
        super();
    }

    ngOnInit(): void {
        this.bindColumns();
        if (this.bindCustomDataSources()) {
            this.loadAfterSetFilter = true;
        }
    }

    private bindColumns(): void {
        this.columns.push({
                caption: 'عنوان',
                dataField: 'title'
            }, {
                caption: 'گروه دسته بندی',
                dataField: 'categoryGroupTitle'
            },
            {
                caption: 'سقف یارانه وزارت',
                dataField: 'vezaratCeiling',
                cellTemplate: 'number'
            }, {
                caption: 'سقف یارانه ساتا',
                dataField: 'sataCeiling',
                cellTemplate: 'number'
            }, {
                caption: 'سقف یارانه وزارت به ازای هر شخص',
                dataField: 'vezaratCeilingPerHekmatCard',
                cellTemplate: 'number',
            }, {
                caption: 'سقف یارانه ساتا به ازای هر شخص',
                dataField: 'sataCeilingPerHekmatCard',
                cellTemplate: 'number',
            }, {
                caption: 'سقف تعداد یارانه وزارت به ازای هر شخص',
                dataField: 'vezaratSubsidyMaxCountPerHekmatCard',
                cellTemplate: 'number',
            }, {
                caption: 'سقف تعداد یارانه ساتا به ازای هر شخص',
                dataField: 'sataSubsidyMaxCountPerHekmatCard',
                cellTemplate: 'number',
            }, {
                caption: 'فعال',
                dataField: 'isActive',
                width: '50px'
            }, {
                caption: 'فعال بودن یارانه ساتا',
                dataField: 'isSataActive',
                width: '50px'
            }, {
                caption: 'فعال بودن یارانه وزارت',
                dataField: 'isVezaratActive',
                width: '50px'
            }, {
            caption: 'آیا ضرایب اعمال بشود یا نه',
            dataField: 'applySubsidyFactor',
            width: '60px'
          }
            );
    }

    bindCustomDataSources(): boolean {
        let customListUrl = `${this.baseListUrl}`;
        if (this.listForm) {
            if (this.categoryGroupId) {
                customListUrl += `categoryGroupId=${this.categoryGroupId}&`;
            }
            if (this.subsidyGroupId) {
                customListUrl += `subsidyGroupId=${this.subsidyGroupId}&`;
            }

        }
        let res = false;
        if (this.categoryGroupId || this.subsidyGroupId) {
            this.listForm.customListUrl = customListUrl;
            this.listForm.refreshList();
            res = true;
        }
        return res;

    }

    afterInitialDetailComponent(componentInstance: any): void {
        const comp = (componentInstance.instance as SubsidyDetailComponent);
        if (this.subsidyGroupId) {
            comp.subsidyGroupId = this.subsidyGroupId;
        }
    }
}
