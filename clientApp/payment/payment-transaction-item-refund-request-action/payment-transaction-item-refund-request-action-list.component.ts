import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';
import {Component, EventEmitter, Input, OnInit, ViewChild} from '@angular/core';
import {YashilCustomButtonColumn} from "../../../shared/base/classes/yashil-cutom-button-column";
import {first} from "rxjs/operators";
import {GenericDataService} from "../../../shared/base/services/generic-data.service";


@Component({
  selector: 'app-payment-transaction-item-refund-request-action-list',
  templateUrl: './payment-transaction-item-refund-request-action-list.component.html'
})
export class PaymentTransactionItemRefundRequestActionListComponent extends Selectable implements OnInit {
  @ViewChild('listForm', {static: true}) listForm: BaseList;
  loadAfterSetFilter: boolean;
  selectedItemId: number;
  columns: any[] = [];
  entityName = 'refundPaymentTransactionItemRequest';
  customRowButtons: YashilCustomButtonColumn;
  applyEvent: EventEmitter<any> = new EventEmitter<any>();

  private baseListUrl = 'RefundPaymentTransactionItemRequest/GetByCustomFilterForActions?';

  constructor(private genericDataService: GenericDataService) {
    super();
    this.rejectRequest = this.rejectRequest.bind(this);
    this.refundTransactionAndTerminalPayment = this.refundTransactionAndTerminalPayment.bind(this);
  }

  ngOnInit(): void {
    this.bindColumns();
    this.bindCustomRowButtons();
    if (this.bindCustomDataSources()) {
      this.loadAfterSetFilter = true;
    }
  }
  bindCustomDataSources(): boolean {
    let customListUrl = `${this.baseListUrl}`;
    let res: boolean;
    this.listForm.customListUrl = customListUrl;
    this.listForm.refreshList();
    res = true;
    return res;

  }
  private bindColumns(): void {
    this.columns.push(
      {
        caption: 'شناسه ترمینال',
        dataField: 'yashilHekmatTerminalTitle',
        width: '15%',
      },

      {
        caption: 'شناسه تراکنش پدیرنده',
        dataField: 'reservationNumber',
        width: '15%',
      },


      {
        caption: 'نوع درخواست',
        dataField: 'refundPaymentTransactionItemRequestTypeTitle',
        width: '20%',
      },

      {
        caption: 'وضعیت درخواست',
        dataField: 'refundPaymentTransactionItemRequestStatusTitle',
        width: '20%',
      },

    );
  }


  bindCustomRowButtons(): void {
    this.customRowButtons = {
      width: '30%',
      customButtons: [
        {
          hint: 'تایید درخواست لغو تراکنش و سفارش',
          text: 'تایید درخواست',
          width: '50%',
          type: 'success',
          onClick: this.refundTransactionAndTerminalPayment,
          visibleProp: 'allowRefundTransactionAndTerminalPayment'
        },
        {
          hint: 'رد کردن درخواست',
          text: 'رد درخواست',
          width: '50%',
          type: 'danger',
          onClick: this.rejectRequest,
          visibleProp: 'allowRejectRequest'
        }
      ]
    };
  }

  /*
   * رد کردن درخواست تغییر سفارش
   */
  rejectRequest(selectedItem: any): void {
    this.confirmBeforeAction(
      'رد کردن درخواست تغییر سفارش',
      'آیا از رد کردن درخواست تغییر یا لغو سفارش اطمینان دارید؟',
      'تایید'
    ).show().then((dialogResult) => {
      if (dialogResult.res === 'ok') {
        // this.applyEvent.pipe(first()).subscribe(() => {
          this.busyIndicator.setBusy(true);
          const selectedEntityId = selectedItem.row.data.id;
          this.genericDataService.updateEntity('RefundPaymentTransactionItemRequest', {
            paymentTransactionItemRefundRequestId: selectedEntityId,
            rejectRequest: true,
          }, 'RefundPaymentTransactionItemRequestAction')
            .subscribe(_ => {
              this.busyIndicator.setBusy(false);
              this.listForm.refreshList();
            });
        // });
      }
    });
  }

  /*
  وقتی درخواست لغو را تایید می کنیم این متد فراخوانی می شود
   */
  refundTransactionAndTerminalPayment(selectedItem: any) {
    const selectedEntityId = selectedItem.row.data.id;
    let title = 'تایید درخواست لغو سفارش';
    let message = 'آیا از تایید درخواست لغو سفارش اطمینان دارید؟';
    let confirmText = 'تایید';
    this.confirmBeforeAction(title, message, confirmText).show().then((dialogResult) => {
      if (dialogResult.res === 'ok') {
        this.busyIndicator.setBusy(true);
        this.genericDataService.updateEntity('RefundPaymentTransactionItemRequest', {
          paymentTransactionItemRefundRequestId: selectedEntityId
        }, 'RefundPaymentTransactionItemRequestAction')
          .subscribe(response => {
            this.showMessage(response.message);
            this.busyIndicator.setBusy(false);
            this.listForm.refreshList();
          });
      }
    });
  }
}

