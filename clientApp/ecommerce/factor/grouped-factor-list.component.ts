import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';
import {Component, EventEmitter, Input, OnInit, Output, ViewChild} from '@angular/core';
import {FactorDetailTabBasedComponent} from './factor-detail-tab-based.component';
import {YashilCustomButtonColumn} from '../../../shared/base/classes/yashil-cutom-button-column';


@Component({
  selector: 'app-grouped-factor-list',
  templateUrl: 'grouped-factor-list.component.html',
})
export class GroupedFactorListComponent extends Selectable implements OnInit {
  @ViewChild('listForm', {static: true}) listForm: BaseList;
  loadAfterSetFilter: boolean;
  selectedItemId: number;
  columns: any[] = [];
  customRowButtons: YashilCustomButtonColumn;
  entityName = 'factor';
  @Output()
  selectedFactorIdChanged: EventEmitter<any> = new EventEmitter<any>();
  groupList: { value: number, title: string }[] = [];
  private baseListUrl = 'factor/GetGroupedFactorList?';

  constructor() {
    super();
    this.selectFactor = this.selectFactor.bind(this);
    this.getGroupParentTitle = this.getGroupParentTitle.bind(this);
    this.getGroupChildTitle = this.getGroupChildTitle.bind(this);
  }

  // tslint:disable-next-line:variable-name
  _companyId: number;

  get companyId(): number {
    return this._companyId;
  }

  @Input()
  set companyId(val) {
    if (val !== this._companyId) {
      this._companyId = val;
    }
  }

  // tslint:disable-next-line:variable-name
  _representationId: number;

  get representationId(): number {
    return this._representationId;
  }

  @Input()
  set representationId(val) {
    if (val !== this._representationId) {
      this._representationId = val;
    }
  }

  ngOnInit(): void {
    this.bindColumns();
    this.createCustomButtons();
    if (this.bindCustomDataSources()) {
      this.loadAfterSetFilter = true;
    }
  }

  public bindCustomDataSources(): boolean {
    let customListUrl = `${this.baseListUrl}`;
    this.listForm.customListUrl = customListUrl;
    if (this.listForm) {
      if (this.companyId) {
        customListUrl += `companyId=${this.companyId}&`;
      }
      if (this.representationId) {
        customListUrl += `representationId=${this.representationId}&`;
      }
    }
    let res = false;
    if (this.companyId || this.representationId) {

      this.listForm.customListUrl = customListUrl;
      this.listForm.refreshList();
      res = true;
    }
    return res;

  }

  afterInitialDetailComponent(componentInstance: any): void {
    const comp = (componentInstance.instance as FactorDetailTabBasedComponent);
    if (this.companyId) {
      comp.companyId = this.companyId;
    }
    if (this.representationId) {
      comp.representationId = this.representationId;
    }
  }

  /*
دریافت عنوان گروه
هنگامی که لیست آیتمهای فاکتور گروه بندی میشود
گروه بندی بر اساس آیدی صورت میگیرد و نام نمایشی گروه بندی با این متد نمایش داده میشود
 */
  getGroupParentTitle(cellInfo): string {
    const groups = this.groupList.filter(x => x.value === cellInfo.value);
    if (groups.length > 0) {
      return groups[0].title;
    } else {
      const currentRowTitle = this.listForm.gridDataSource.items().filter(x => x.key === cellInfo.value)[0]?.items[0]?.items[0]?.factorGroupParentTitle;
      if (currentRowTitle) {
        this.groupList.push({value: cellInfo.value, title: currentRowTitle});
        return currentRowTitle;
      }
      return '-';
    }
  }

  getGroupChildTitle(cellInfo): string {
    const groups = this.groupList.filter(x => x.value === cellInfo.value);
    if (groups.length > 0) {
      return groups[0].title;
    } else {

      for (const levelOneItem of this.listForm.gridDataSource.items()) {
        if (Array.isArray(levelOneItem.items)) {
          const currentRowTitle = levelOneItem.items.filter(x => x.key === cellInfo.value)[0]?.items[0]?.factorGroupTitle;
          if (currentRowTitle) {
            this.groupList.push({value: cellInfo.value, title: currentRowTitle});
            return currentRowTitle;
          }
        }
      }
      return '-';
    }
  }

  private bindColumns(): void {

    this.columns.push({
        caption: 'گروه',
        dataField: 'factorGroupParentId',
        width: '150px',
        groupIndex: 1,
        customizeText: this.getGroupParentTitle
      }, {
        caption: 'گروه',
        dataField: 'factorGroupId',
        width: '150px',
        groupIndex: 2,
        customizeText: this.getGroupChildTitle
      },
      {
        caption: 'کد',
        dataField: 'code',
        width: '150px',
        allowGrouping: 0
      }, {
        caption: 'مرکز',
        dataField: 'companyTitle',
        width: '100px',
        allowGrouping: 0
      }, {
        caption: 'نمایندگی',
        dataField: 'representationTitle',
        width: '150px',
        allowGrouping: 0
      }, {
        caption: 'تاریخ صدور',
        dataField: 'issueDate',
        cellTemplate: 'intDate',
        allowHeaderFiltering: false,
        width: '120px',
        allowGrouping: 0
      });
  }

  private createCustomButtons(): void {
    this.customRowButtons = {
      width: '130px',
      customButtons: [
        {
          hint: 'نمایش لیست پرداختی ها',
          text: 'لیست پرداختی ها',
          icon: 'money',
          width: '130px',
          type: 'default',
          onClick: this.selectFactor
        }
      ]
    };
  }

  private selectFactor(item: any): void {
    this.selectedFactorIdChanged.emit(item);
  }
}
