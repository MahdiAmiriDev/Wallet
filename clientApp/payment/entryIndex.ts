import {PaymentConfigurationDetailComponent} from './payment-configuration/payment-configuration-detail.component';

import {PaymentMethodDetailComponent} from './payment-method/payment-method-detail.component';

import {TaxRateDetailComponent} from './tax-rate/tax-rate-detail.component';

import {CardInfoDetailComponent} from './card-info/card-info-detail.component';

import {CreditOrCashPaymentDetailComponent} from './credit-or-cash-payment/credit-or-cash-payment-detail.component';

import {InstallmentDetailComponent} from './installment/installment-detail.component';

import {CardAmountHistoryDetailComponent} from './card-amount-history/card-amount-history-detail.component';

import {MerchantInfoDetailComponent} from './merchant-info/merchant-info-detail.component';

import {MerchantTerminalInfoDetailComponent} from './merchant-terminal-info/merchant-terminal-info-detail.component';

import {
  HekmatPaymentGatewayTransactionDetailComponent
} from './hekmat-payment-gateway-transaction/hekmat-payment-gateway-transaction-detail.component';

import {HekmatTerminalDetailComponent} from './hekmat-terminal/hekmat-terminal-detail.component';

import {YashilHekmatTerminalDetailComponent} from './yashil-hekmat-terminal/yashil-hekmat-terminal-detail.component';
import {
  YashilHekmatTerminalDetailTabBasedComponent
} from "./yashil-hekmat-terminal/yashil-hekmat-terminal-detail-tab-based.component";
import {HekmatTerminalDetailTabBasedComponent} from "./hekmat-terminal/hekmat-terminal-detail-tab-based.component";
import {
  HekmatPaymentGatewayTransactionDetailTabBasedComponent
} from "./hekmat-payment-gateway-transaction/hekmat-payment-gateway-transaction-detail-tab-based.component";
import {MerchantInfoListComponent} from "./merchant-info/merchant-info-list.component";
import {MerchantInfoDetailTabBasedComponent} from "./merchant-info/merchant-info-detail-tab-based.component";
import {Provider} from '@angular/core';
import {
  HekmatPaymentGatewayTransactionBulkRequestCategoryDetailComponent
} from './hekmat-payment-gateway-transaction-bulk-request-category/hekmat-payment-gateway-transaction-bulk-request-category-detail.component';

export const ENTRYCOMPONENTS: Provider[] = [
  PaymentConfigurationDetailComponent,
  PaymentMethodDetailComponent,
  TaxRateDetailComponent,
  CardInfoDetailComponent,
  CreditOrCashPaymentDetailComponent,
  InstallmentDetailComponent,
  CardAmountHistoryDetailComponent,
  MerchantInfoDetailComponent,
  MerchantTerminalInfoDetailComponent,
  HekmatPaymentGatewayTransactionDetailComponent,
  HekmatTerminalDetailComponent,
  YashilHekmatTerminalDetailComponent,
  YashilHekmatTerminalDetailTabBasedComponent,
  HekmatTerminalDetailTabBasedComponent,
  HekmatPaymentGatewayTransactionDetailTabBasedComponent,
  MerchantInfoListComponent,
  MerchantInfoDetailTabBasedComponent,
  HekmatPaymentGatewayTransactionBulkRequestCategoryDetailComponent

];
