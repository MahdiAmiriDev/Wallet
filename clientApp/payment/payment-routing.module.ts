import {RouterModule, Routes} from '@angular/router';
import {AuthGuard} from '../../shared/_guards';
import {PaymentConfigurationListComponent} from './payment-configuration/payment-configuration-list.component';
import {PaymentMethodListComponent} from './payment-method/payment-method-list.component';
import {TaxRateListComponent} from './tax-rate/tax-rate-list.component';
import {CardInfoListComponent} from './card-info/card-info-list.component';
import {CreditOrCashPaymentListComponent} from './credit-or-cash-payment/credit-or-cash-payment-list.component';
import {InstallmentListComponent} from './installment/installment-list.component';
import {CardAmountHistoryListComponent} from './card-amount-history/card-amount-history-list.component';
import {MerchantInfoListComponent} from './merchant-info/merchant-info-list.component';
import {MerchantTerminalInfoListComponent} from './merchant-terminal-info/merchant-terminal-info-list.component';
import {NgModule} from '@angular/core';
import {CardChargeComponent} from './card-charge/card-charge.component';
import {HekmatPaymentGatewayTransactionListComponent}
  from './hekmat-payment-gateway-transaction/hekmat-payment-gateway-transaction-list.component';
import {HekmatTerminalListComponent} from './hekmat-terminal/hekmat-terminal-list.component';
import {YashilHekmatTerminalListComponent} from './yashil-hekmat-terminal/yashil-hekmat-terminal-list.component';
import {
  HekmatPaymentGatewayTransactionBulkRequestComponent
} from "./hekmat-payment-gateway-transaction-bulk-request/hekmat-payment-gateway-transaction-bulk-request.component";
import {
  RefundPaymentTransactionItemRequestListComponent
} from "./refund-payment-transaction-item-request/refund-payment-transaction-item-request-list.component";
import {
  PaymentTransactionItemRefundRequestActionListComponent
} from "./payment-transaction-item-refund-request-action/payment-transaction-item-refund-request-action-list.component";


const routes: Routes = [
  {path: 'paymentConfigurations', component: PaymentConfigurationListComponent, canActivate: [AuthGuard]},
  {path: 'paymentMethods', component: PaymentMethodListComponent, canActivate: [AuthGuard]},
  {path: 'taxRates', component: TaxRateListComponent, canActivate: [AuthGuard]},
  {path: 'cardInfos', component: CardInfoListComponent, canActivate: [AuthGuard]},
  {path: 'creditOrCashPayments', component: CreditOrCashPaymentListComponent, canActivate: [AuthGuard]},
  {path: 'installments', component: InstallmentListComponent, canActivate: [AuthGuard]},
  {path: 'cardAmountHistorys', component: CardAmountHistoryListComponent, canActivate: [AuthGuard]},
  {path: 'cardCharges', component: CardChargeComponent, canActivate: [AuthGuard]},
  {path: 'merchantInfos', component: MerchantInfoListComponent, canActivate: [AuthGuard]},
  {path: 'merchantTerminalInfos', component: MerchantTerminalInfoListComponent, canActivate: [AuthGuard]},
  {path: 'hekmatPaymentGatewayTransactions', component: HekmatPaymentGatewayTransactionListComponent, canActivate: [AuthGuard]},
  {path: 'hekmatTerminals', component: HekmatTerminalListComponent, canActivate: [AuthGuard]},
  {path: 'yashilHekmatTerminals', component: YashilHekmatTerminalListComponent, canActivate: [AuthGuard]},
  {path: 'hekmatPaymentGatewayTransactionBulkRequest', component: HekmatPaymentGatewayTransactionBulkRequestComponent, canActivate: [AuthGuard]},
  {path: 'refundPaymentTransactionsItemRequests', component: RefundPaymentTransactionItemRequestListComponent, canActivate: [AuthGuard]},
  {path: 'paymentTransactionItemRefundRequestActionList', component: PaymentTransactionItemRefundRequestActionListComponent, canActivate: [AuthGuard]}

];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
  }
)
export class PaymentRoutingModule {

}
