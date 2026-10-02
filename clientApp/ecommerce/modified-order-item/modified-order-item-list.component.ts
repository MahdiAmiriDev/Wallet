import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';
import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {ModifiedOrderItemDetailComponent} from './modified-order-item-detail.component';
import {JalaliPipe} from '../../../shared/base/core/pipes/jalali.pipe ';

@Component({
  selector: 'app-modified-order-item-list',
  templateUrl: './modified-order-item-list.component.html'
})
export class ModifiedOrderItemListComponent extends Selectable implements OnInit {
  @ViewChild('listForm', {static: true}) listForm: BaseList;
  loadAfterSetFilter: boolean;
  selectedItemId: number;
  modifiedOrderItemTypeId: number;
  columns: any[] = [];
  typeGroupList: { value: number, title: string }[] = [];
  statusTypeGroupList: { value: number, title: string }[] = [];
  entityName = 'modifiedOrderItem';
  detailComponent = ModifiedOrderItemDetailComponent;
  // tslint:disable-next-line:variable-name
  _oldOrderItemId: number;
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

  private baseListUrl = 'modifiedOrderItem/GetByCustomFilterForModify?';

  constructor(private pipe: JalaliPipe) {
    super();
    this.getModifiedOrderItemTypeTitle = this.getModifiedOrderItemTypeTitle.bind(this);
    this.getModifiedOrderItemStatusTypeTitle = this.getModifiedOrderItemStatusTypeTitle.bind(this);

  }

  ngOnInit(): void {
    this.bindColumns();
    if (this.bindCustomDataSources()) {
      this.loadAfterSetFilter = true;
    }
  }

  private bindColumns(): void {
    this.columns.push({
      caption: 'تاریخ ثبت',
      dataField: 'creationDate',
      cellTemplate: 'jalaliDate',
      width: '80px',
      allowGrouping: 0
    }, {
      caption: 'نوع',
      dataField: 'modifiedOrderItemType',
      groupIndex: 1,
      customizeText: this.getModifiedOrderItemTypeTitle
    }, {
      caption: 'کد پیگیری',
      dataField: 'lastOrderItemCode',
      allowHeaderFiltering: false,
      width: 100,
      allowGrouping: 0
    }, {
      caption: 'کد آیتم سفارش',
      dataField: 'lastOrderItemId',
      allowHeaderFiltering: false,
      width: 70,
      allowGrouping: 0
    }, {
      caption: 'تعداد',
      dataField: 'quantity',
      allowHeaderFiltering: false,
      width: 50,
      allowGrouping: 0
    }, {
      caption: 'کد ملی تحویل گیرنده',
      dataField: 'nationalCode',
      allowHeaderFiltering: false,
      width: 100,
      allowGrouping: 0
    }, {
      caption: 'شماره موبایل',
      dataField: 'mobileNumber',
      allowHeaderFiltering: false,
      width: 100,
      allowGrouping: 0
    }, {
      caption: 'نام محصول',
      dataField: 'lastOrderItemTitle',
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
      width: '100px',
      allowGrouping: 0
    }, {
      caption: 'وضعیت ',
      dataField: 'ModifiedOrderItemStatusType',
      groupIndex: 2,
      customizeText: this.getModifiedOrderItemStatusTypeTitle
    }, {
      caption: 'توضیحات',
      dataField: 'description',
      allowHeaderFiltering: false,
      with: 300,
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
    let res = false;
    this.listForm.customListUrl = customListUrl;
    this.listForm.refreshList();
    res = true;
    return res;

  }

  afterInitialDetailComponent(componentInstance: any): void {
    const comp = (componentInstance.instance as ModifiedOrderItemDetailComponent);

    if (this.oldOrderItemId) {
      comp.oldOrderItemId = this.oldOrderItemId;
    }

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

  }

  getModifiedOrderItemTypeTitle(cellInfo): string {
    this.modifiedOrderItemTypeId = cellInfo.value;
    const typeGroups = this.typeGroupList.filter(x => x.value === cellInfo.value);
    if (typeGroups.length > 0) {
      return typeGroups[0].title;
    } else {
      const currentRowTitle = this.listForm.gridDataSource.items().filter(x => x.key === cellInfo.value)[0]?.items[0]?.items[0]?.modifiedOrderItemTypeTitle;
      if (currentRowTitle) {
        this.typeGroupList.push({value: cellInfo.value, title: currentRowTitle});
        return currentRowTitle;
      }
      return '-';
    }
  }
  getModifiedOrderItemStatusTypeTitle(cellInfo): string {
    const statusGroups = this.statusTypeGroupList.filter(x => x.value === cellInfo.value);
    if (statusGroups.length > 0) {
      return statusGroups[0].title;
    } else {
      const currentRowTitle = this.listForm.gridDataSource.items().filter(x => x.key === this.modifiedOrderItemTypeId)[0]?.items.filter(x => x.key === cellInfo.value)[0]?.items[0]?.modifiedOrderItemStatusTypeTitle;
      if (currentRowTitle) {
        this.statusTypeGroupList.push({value: cellInfo.value, title: currentRowTitle});
        return currentRowTitle;
      }
      return '-';
    }
  }
}
