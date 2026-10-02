import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';
import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {YashilCustomButton} from '../../../shared/base/models/yasil-custom-button';

@Component({
  selector: 'app-factor-group-factor-item-list',
  templateUrl: './factor-group-factor-item-list.component.html'
})
export class FactorGroupFactorItemListComponent extends Selectable implements OnInit {
  @ViewChild('listForm', {static: true}) listForm: BaseList;
  loadAfterSetFilter: boolean;
  selectedItemId: number;
  columns: any[] = [];
  entityName = 'OrderItem';
  customButtons: YashilCustomButton[];
  @Input()
  gridHeight: number;
  private baseListUrl = 'factorItem/GetFactorGroupFactorItemForList?';

  constructor() {
    super();
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

  // tslint:disable-next-line:variable-name
  _orderItemId: number;

  get orderItemId(): number {
    return this._orderItemId;
  }

  @Input()
  set orderItemId(val) {
    if (val !== this._orderItemId) {
      this._orderItemId = val;
    }
  }

  ngOnInit(): void {
    if (!this.gridHeight) {
      this.gridHeight = window.innerHeight - 170;
    }
    this.bindColumns();
    this.bindCustomButtons();
    if (this.bindCustomDataSources()) {
      this.loadAfterSetFilter = true;
    }
  }

  public bindCustomDataSources(): boolean {
    let customListUrl = `${this.baseListUrl}`;
    if (this.listForm) {
      if (this.factorGroupId) {
        customListUrl += `factorGroupId=${this.factorGroupId}&`;
      }
    }
    let res = false;
    if (this.factorGroupId || this.orderItemId) {

      this.listForm.customListUrl = customListUrl;
      this.listForm.refreshList();
      res = true;
    }
    return res;

  }

  bindCustomButtons(): void {
    this.customButtons = [{
      text: 'دانلود گزارش',
      type: 'default',
      icon: 'xlsfile',
      key: this.factorGroupId
    }];
  }

  downloadFactorGroupFactorItemReport(factorGroupId: any): void {
    const url = `${window.location.origin}/api/factorGroup/GenerateFactorGroupFactorItemReportFile?factorGroupId=${factorGroupId}`;
    window.open(url, '_blank');
  }

  private bindColumns(): void {
    this.columns.push({
      width: '100px',
      caption: 'سفارش',
      allowHeaderFiltering: false,
      dataField: 'orderNumber',
      allowGrouping: 0
    }, {
      width: '200px',
      caption: 'فاکتور',
      dataField: 'factorCode',
      groupIndex: 1
    }, {
      width: '100px',
      caption: 'کاربر',
      allowHeaderFiltering: false,
      dataField: 'userName',
      allowGrouping: 0
    }, {
      width: '100px',
      caption: 'کدملی',
      allowHeaderFiltering: false,
      dataField: 'nationalCode',
      allowGrouping: 0
    }, {
      width: '150px',
      caption: 'نام محصول',
      allowHeaderFiltering: false,
      dataField: 'productTitle',
      allowGrouping: 0
    }, {
      width: '150px',
      caption: 'مرکز',
      dataField: 'companyTitle',
      allowGrouping: 0
    }, {
      width: '150px',
      caption: 'نمایندگی',
      dataField: 'representationTitle',
      allowGrouping: 0
    }, {
      width: '150px',
      caption: 'نوع پرداخت',
      dataField: 'paymentMethodType',
      allowGrouping: 0
    }, {
      width: '70px',
      caption: 'اقساط',
      allowHeaderFiltering: false,
      dataField: 'installmentCount',
      allowGrouping: 0
    }, {
      width: '90px',
      caption: 'یارانه وزارت',
      allowHeaderFiltering: false,
      dataField: 'vezaratSubsidy',
      cellTemplate: 'number',
      allowGrouping: 0
    }, {
      width: '100px',
      caption: 'یارانه وزارت با کارمزد',
      allowHeaderFiltering: false,
      dataField: 'vezaratSubsidyPriceIncProfit',
      cellTemplate: 'number',
      allowGrouping: 0
    }, {
      width: '100px',
      caption: 'یارانه وزارت با کارمزد و مالیات',
      allowHeaderFiltering: false,
      dataField: 'vezaratSubsidyPriceIncTaxIncProfit',
      cellTemplate: 'number',
      allowGrouping: 0
    }, {
      width: '100px',
      caption: 'یارانه ساتا',
      allowHeaderFiltering: false,
      dataField: 'sataSubsidy',
      cellTemplate: 'number',
      allowGrouping: 0
    }, {
      width: '100px',
      caption: 'یارانه ساتا با کارمزد',
      allowHeaderFiltering: false,
      dataField: 'sataSubsidyPriceIncProfit',
      cellTemplate: 'number',
      allowGrouping: 0
    }, {
      width: '100px',
      caption: 'یارانه ساتا با کارمزد و مالیات',
      allowHeaderFiltering: false,
      dataField: 'sataSubsidyPriceIncTaxIncProfit',
      cellTemplate: 'number',
      allowGrouping: 0
    }, {
      width: '100px',
      caption: 'قیمت کل با مالیات',
      allowHeaderFiltering: false,
      dataField: 'totalIncludeTax',
      cellTemplate: 'number',
      allowGrouping: 0
    }, {
      width: '100px',
      caption: 'قیمت پایه',
      allowHeaderFiltering: false,
      dataField: 'basePrice',
      cellTemplate: 'number',
      allowGrouping: 0
    }, {
      caption: 'تعداد',
      width: '70px',
      allowHeaderFiltering: false,
      dataField: 'quantity',
      allowGrouping: 0
    }, {
      width: '100px',
      caption: 'مبلغ(قیمت پایه در تعداد)',
      allowHeaderFiltering: false,
      dataField: 'price',
      cellTemplate: 'number',
      allowGrouping: 0
    }, {
      width: '100px',
      caption: 'هزینه ارسال',
      allowHeaderFiltering: false,
      dataField: 'additionalShippingCharge',
      cellTemplate: 'number',
      allowGrouping: 0
    });

  }
}
