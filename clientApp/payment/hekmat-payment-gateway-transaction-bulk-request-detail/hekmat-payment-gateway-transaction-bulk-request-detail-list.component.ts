import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';
import {Component, EventEmitter, Input, OnInit, Output, ViewChild} from '@angular/core';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';

@Component({
  selector: 'app-hekmat-payment-gateway-transaction-bulk-request-detail-list',
  templateUrl: './hekmat-payment-gateway-transaction-bulk-request-detail-list.component.html'
})
export class HekmatPaymentGatewayTransactionBulkRequestDetailListComponent extends Selectable implements OnInit {
  @ViewChild('listForm', {static: true}) listForm: BaseList;
  loadAfterSetFilter: boolean;
  selectedItemId: number;
  columns: any[] = [];
  entityName = 'hekmatPaymentGatewayTransactionBulkRequestDetail';
  customButtons: any;
  @Input()
  hekmatPaymentGatewayTransactionBulkRequestId: number;

  @Output()
  afterFinalRegistration: EventEmitter<any> = new EventEmitter<any>();
  private baseListUrl = 'HekmatPaymentGatewayTransactionBulkRequestDetail/GetByCustomFilterForList?';

  constructor(private genericDataService: GenericDataService) {
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
        caption: 'شماره کارت',
        dataField: 'cardNumber',
        width: '20%'
      },
      {
        caption: 'کد ملی صاحب کارت',
        dataField: 'nationalCode',
        width: '20%'
      },
      {
        caption: 'مبلغ به ریال',
        dataField: 'price',
        width: '15%'
      },
      {
        caption: 'وضعیت',
        dataField: 'status',
        width: '10%'
      },
      {
        caption: 'تعداد اقساط',
        dataField: 'installmentCount',
        width: '10%'
      },
      {
        caption: 'پیغام پرداخت',
        dataField: 'hekmatTransactionResultMessage',
        width: '25%'
      }
    );
  }

  bindCustomDataSources(): boolean {
    let customListUrl = `${this.baseListUrl}`;
    if (this.listForm) {
      customListUrl += `hekmatPaymentGatewayTransactionBulkRequestId=${this.hekmatPaymentGatewayTransactionBulkRequestId}&`;
      this.listForm.customListUrl = customListUrl;
      this.listForm.refreshList();
      return true;
    }
  }

}
