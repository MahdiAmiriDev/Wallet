import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';
import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {YashilHekmatTerminalDetailComponent} from "./yashil-hekmat-terminal-detail.component";
import {YashilHekmatTerminalDetailTabBasedComponent} from "./yashil-hekmat-terminal-detail-tab-based.component";


@Component({
  selector: 'app-yashil-hekmat-terminal-list',
  templateUrl: './yashil-hekmat-terminal-list.component.html'
})
export class YashilHekmatTerminalListComponent extends Selectable implements OnInit {
  @ViewChild('listForm', {static: true}) listForm: BaseList;
  loadAfterSetFilter: boolean;
  selectedItemId: number;
  columns: any[] = [];
  entityName = 'yashilHekmatTerminal';
  detailComponent = YashilHekmatTerminalDetailTabBasedComponent;
  _hekmatTerminalId: number;
  @Input()
  set hekmatTerminalId(val) {
    if (val !== this._hekmatTerminalId) {
      this._hekmatTerminalId = val;
    }
  }

  get hekmatTerminalId(): number {
    return this._hekmatTerminalId;
  }

  _merchantId: number;
  @Input()
  set merchantId(val) {
    if (val !== this._merchantId) {
      this._merchantId = val;
    }
  }

  get merchantId(): number {
    return this._merchantId;
  }

  private baseListUrl = 'yashilHekmatTerminal/GetByCustomFilterForList?';

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
    this.columns.push(
      {
        caption: 'شناسه ترمینال',
        dataField: 'id'
      },
      {
        caption: 'شناسه پذیرنده',
        dataField: 'merchantId'
      },
      {
        caption: 'عنوان ترمینال',
        dataField: 'title'
      },
      {
        caption: 'نام پذیرنده',
        dataField: 'merchantTitle'
      },
      {
        caption: 'ترمینال حکمت',
        dataField: 'hekmatTerminalTitle'
      },
      {
        caption: 'سقف اقساط',
        dataField: 'maxInstallmentCount'
      },
      {
        caption: 'حداقل مبلغ تراکنش',
        dataField: 'minAmount',
        cellTemplate: 'number',
      },
      {
        caption: 'حداکثر مبلغ تراکتش',
        dataField: 'maxAmount',
        cellTemplate: 'number',
      },

      {
        caption: 'آی پی',
        dataField: 'ipAddress'
      },
      {
        caption: 'فعال',
        dataField: 'isActive'
      }
    );
  }

  bindCustomDataSources(): boolean {
    let customListUrl = `${this.baseListUrl}`;
    if (this.listForm) {
      if (this.hekmatTerminalId) {
        customListUrl += `hekmatTerminalId=${this.hekmatTerminalId}&`;
      }
      if (this.merchantId) {
        customListUrl += `merchantId=${this.merchantId}&`;
      }
    }
    let res = false;
    if (this.hekmatTerminalId || this.merchantId) {
      this.listForm.customListUrl = customListUrl;
      this.listForm.refreshList();
      res = true;
    }
    return res;

  }

  afterInitialDetailComponent(componentInstance: any): void {
    const comp = (componentInstance.instance as YashilHekmatTerminalDetailComponent);

    if (this.hekmatTerminalId) {
      comp.hekmatTerminalId = this.hekmatTerminalId;
    }

    if (this.merchantId) {
      comp.merchantId = this.merchantId;
    }

  }
}
