import {PaymentConfigurationDetailComponent} from './payment-configuration/payment-configuration-detail.component';
import {PaymentConfigurationListComponent} from './payment-configuration/payment-configuration-list.component';

import {PaymentMethodDetailComponent} from './payment-method/payment-method-detail.component';
import {PaymentMethodListComponent} from './payment-method/payment-method-list.component';

import {TaxRateDetailComponent} from './tax-rate/tax-rate-detail.component';
import {TaxRateListComponent} from './tax-rate/tax-rate-list.component';

import {CardInfoDetailComponent} from './card-info/card-info-detail.component';
import {CardInfoListComponent} from './card-info/card-info-list.component';

import {CreditOrCashPaymentDetailComponent} from './credit-or-cash-payment/credit-or-cash-payment-detail.component';
import {CreditOrCashPaymentListComponent} from './credit-or-cash-payment/credit-or-cash-payment-list.component';

import {InstallmentDetailComponent} from './installment/installment-detail.component';
import {InstallmentListComponent} from './installment/installment-list.component';

import {CardAmountHistoryDetailComponent} from './card-amount-history/card-amount-history-detail.component';
import {CardAmountHistoryListComponent} from './card-amount-history/card-amount-history-list.component';
import {MerchantInfoDetailComponent} from './merchant-info/merchant-info-detail.component';
import {MerchantInfoListComponent} from './merchant-info/merchant-info-list.component';

import {MerchantTerminalInfoDetailComponent} from './merchant-terminal-info/merchant-terminal-info-detail.component';
import {MerchantTerminalInfoListComponent} from './merchant-terminal-info/merchant-terminal-info-list.component';

import {HekmatPaymentGatewayTransactionDetailComponent} from './hekmat-payment-gateway-transaction/hekmat-payment-gateway-transaction-detail.component';
import {HekmatPaymentGatewayTransactionListComponent} from './hekmat-payment-gateway-transaction/hekmat-payment-gateway-transaction-list.component';

import {HekmatTerminalDetailComponent} from './hekmat-terminal/hekmat-terminal-detail.component';
import {HekmatTerminalListComponent} from './hekmat-terminal/hekmat-terminal-list.component';

import {YashilHekmatTerminalDetailComponent} from './yashil-hekmat-terminal/yashil-hekmat-terminal-detail.component';
import {YashilHekmatTerminalListComponent} from './yashil-hekmat-terminal/yashil-hekmat-terminal-list.component';

import {YashilHekmatTerminalDetailTabBasedComponent} from "./yashil-hekmat-terminal/yashil-hekmat-terminal-detail-tab-based.component";
import {HekmatTerminalDetailTabBasedComponent} from "./hekmat-terminal/hekmat-terminal-detail-tab-based.component";
import {HekmatPaymentGatewayTransactionDetailTabBasedComponent} from "./hekmat-payment-gateway-transaction/hekmat-payment-gateway-transaction-detail-tab-based.component";
import {TerminalFilterComponent} from "./terminal-filter/terminal-filter.component";
import {MerchantInfoDetailTabBasedComponent} from "./merchant-info/merchant-info-detail-tab-based.component";
import {Provider} from '@angular/core';
import {CardChargeComponent} from './card-charge/card-charge.component';
import {
  HekmatPaymentGatewayTransactionBulkRequestCategoryListComponent
} from "./hekmat-payment-gateway-transaction-bulk-request-category/hekmat-payment-gateway-transaction-bulk-request-category-list.component";
import {
  HekmatPaymentGatewayTransactionBulkRequestCategoryDetailComponent
} from "./hekmat-payment-gateway-transaction-bulk-request-category/hekmat-payment-gateway-transaction-bulk-request-category-detail.component";
import {
  HekmatPaymentGatewayTransactionBulkRequestComponent
} from "./hekmat-payment-gateway-transaction-bulk-request/hekmat-payment-gateway-transaction-bulk-request.component";
import {
  HekmatPaymentGatewayTransactionBulkRequestListComponent
} from "./hekmat-payment-gateway-transaction-bulk-request/hekmat-payment-gateway-transaction-bulk-request-list.component";
import {
  HekmatPaymentGatewayTransactionBulkRequestValidateComponent
} from "./hekmat-payment-gateway-transaction-bulk-request/hekmat-payment-gateway-transaction-bulk-request-validate.component";
import {
  HekmatPaymentGatewayTransactionBulkRequestDetailListComponent
} from "./hekmat-payment-gateway-transaction-bulk-request-detail/hekmat-payment-gateway-transaction-bulk-request-detail-list.component";
import {
  RefundPaymentTransactionItemRequestDetailComponent
} from "./refund-payment-transaction-item-request/refund-payment-transaction-item-request-detail.component";
import {
  RefundPaymentTransactionItemRequestListComponent
} from "./refund-payment-transaction-item-request/refund-payment-transaction-item-request-list.component";
import {
  YashilHekmatTerminalSupplierDetailComponent
} from "./yashil-hekmat-terminal-supplier/yashil-hekmat-terminal-supplier-detail.component";
import {
  PaymentTransactionItemRefundRequestActionListComponent
} from "./payment-transaction-item-refund-request-action/payment-transaction-item-refund-request-action-list.component";

export const COMPONENTS: Provider[] = [

  PaymentConfigurationListComponent,

  PaymentConfigurationDetailComponent,
  PaymentMethodListComponent,

  PaymentMethodDetailComponent,
  TaxRateListComponent,

  TaxRateDetailComponent,
  CardInfoListComponent,

  CardInfoDetailComponent,
  CreditOrCashPaymentListComponent,

  CreditOrCashPaymentDetailComponent,
  InstallmentListComponent,

  InstallmentDetailComponent,
  CardAmountHistoryListComponent,

  CardAmountHistoryDetailComponent,
  CardChargeComponent,
  MerchantInfoListComponent,

  MerchantInfoDetailComponent,
  MerchantTerminalInfoListComponent,

  MerchantTerminalInfoDetailComponent,
  HekmatPaymentGatewayTransactionListComponent,

  HekmatPaymentGatewayTransactionDetailComponent,
  HekmatTerminalListComponent,

  HekmatTerminalDetailComponent,
  YashilHekmatTerminalListComponent,

  YashilHekmatTerminalDetailComponent,
  YashilHekmatTerminalDetailTabBasedComponent,
  HekmatTerminalDetailTabBasedComponent,
  HekmatPaymentGatewayTransactionDetailTabBasedComponent,
  TerminalFilterComponent,
  MerchantInfoDetailTabBasedComponent,
  HekmatPaymentGatewayTransactionBulkRequestCategoryListComponent,
  HekmatPaymentGatewayTransactionBulkRequestCategoryDetailComponent,
  HekmatPaymentGatewayTransactionBulkRequestComponent,
  HekmatPaymentGatewayTransactionBulkRequestListComponent,
  HekmatPaymentGatewayTransactionBulkRequestValidateComponent,
  HekmatPaymentGatewayTransactionBulkRequestDetailListComponent,
  HekmatPaymentGatewayTransactionBulkRequestValidateComponent,
  RefundPaymentTransactionItemRequestDetailComponent,
  RefundPaymentTransactionItemRequestListComponent,
  YashilHekmatTerminalSupplierDetailComponent,
  PaymentTransactionItemRefundRequestActionListComponent
];
