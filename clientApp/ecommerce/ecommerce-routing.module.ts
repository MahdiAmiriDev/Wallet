import {RouterModule, Routes} from '@angular/router';
import {AuthGuard} from '../../shared/_guards';
import {RepresentationListComponent} from './representation/representation-list.component';
import {RepresentationPersonListComponent} from './representation-person/representation-person-list.component';
import {AdditionalUserPropListComponent} from './additional-user-prop/additional-user-prop-list.component';
import {
  RepresentationEstablishedLicenseTypeListComponent
} from './representation-established-license-type/representation-established-license-type-list.component';
import {PersonBankAccountListComponent} from './person-bank-account/person-bank-account-list.component';
import {ContractListComponent} from './contract/contract-list.component';
import {ProductTemplateListComponent} from './product-template/product-template-list.component';
import {GiftCardTypeListComponent} from './gift-card-type/gift-card-type-list.component';
import {RepresentationCategoryListComponent} from './representation-category/representation-category-list.component';
import {TaxCategoryListComponent} from './tax-category/tax-category-list.component';
import {ProductListComponent} from './product/product-list.component';
import {OrderListComponent} from './order/order-list.component';
import {CompanyMainCategoryListComponent} from './company-main-category/company-main-category-list.component';
import {DependentProductSaleListComponent} from './dependent-product-sale/dependent-product-sale-list.component';
import {ProductCategoryListComponent} from './product-category/product-category-list.component';
import {WarehouseListComponent} from './warehouse/warehouse-list.component';

import {CompanyListComponent} from './company/company-list.component';
import {ContractPaymentMethodListComponent} from './contract-payment-method/contract-payment-method-list.component';
import {SaleConfigurationListComponent} from './sale-configuration/sale-configuration-list.component';
import {SpecificationListComponent} from './specification/specification-list.component';
import {SpecificationOptionListComponent} from './specification-option/specification-option-list.component';
import {NgModule} from '@angular/core';

import {
  ProductSpecificationOptionListComponent
} from './product-specification-option/product-specification-option-list.component';
import {SubsidyListComponent} from './subsidy/subsidy-list.component';
import {OrderItemListComponent} from './order-item/order-item-list.component';
import {RewardPointsHistoryListComponent} from './reward-points-history/reward-points-history-list.component';
import {ShoppingCartItemListComponent} from './shopping-cart-item/shopping-cart-item-list.component';
import {RecipientListComponent} from './recipient/recipient-list.component';
import {CustomerClubListComponent} from './customer-club/customer-club-list.component';
import {CustomerClubCardListComponent} from './customer-club-card/customer-club-card-list.component';
import {
  CustomerClubPaymentMethodTypeListComponent
} from './customer-club-payment-method-type/customer-club-payment-method-type-list.component';
import {CanceledOrderItemListComponent} from './canceled-order-item/canceled-order-item-list.component';
import {
  ApplicationPaymentMethodListComponent
} from './application-payment-method/application-payment-method-list.component';
import {FactorItemListComponent} from './factor-item/factor-item-list.component';
import {FactorListComponent} from './factor/factor-list.component';
import {ContentCategoryListComponent} from './content-category/content-category-list.component';
import {FactorVerificationListComponent} from './factor-verification/factor-verification-list.component';
import {
  ApplicationCustomerClubListComponent
} from './application-customer-club/application-customer-club-list.component';
import {
  FactorVerificationConfigurationRoleListComponent
} from './factor-verification-configuration-role/factor-verification-configuration-role-list.component';
import {
  FactorVerificationConfigurationListComponent
} from './factor-verification-configuration/factor-verification-configuration-list.component';
import {SupervisorFactorListComponent} from './factor/supervisor-factor-list.component';
import {IssueCommentListComponent} from './issue-comment/issue-comment-list.component';
import {IssueListComponent} from './issue/issue-list.component';
import {IssueParticipantListComponent} from './issue-participant/issue-participant-list.component';
import {IssueLogWorkListComponent} from './issue-log-work/issue-log-work-list.component';
import {IssueTypeItemListComponent} from './issue-type-item/issue-type-item-list.component';
import {IssueSearchComponent} from './issue/issue-search.component';
import {SataSubsidyApprovalListComponent} from './sata-subsidy-approval/sata-subsidy-approval-list.component';
import {FactorGroupItemCustomListComponent} from './factor-group-item/factor-group-item-custom-list.component';

import {CompletedOrderItemsComponent} from './order-item/completed-order-items.component';
import {GroupedFactorListComponent} from './factor/grouped-factor-list.component';
import {CustomFactorPaymentListComponent} from './factor-payment/custom-factor-payment-list.component';
import {PaymentListComponent} from './payment/payment-list.component';
import {OrderItemFullInfoListComponent} from './order-item-full-info/order-item-full-info-list.component';
import {YashilSchedulerListComponent} from './yashil-scheduler/yashil-scheduler-list.component';
import {
  UpdateOrderNationalCodeListComponent
} from './update-order-nationalcode/update-order-national-code-list.component';
import {ModifiedOrderItemListComponent} from './modified-order-item/modified-order-item-list.component';
import {
  ModifiedOrderItemActionListComponent
} from './modified-order-item-action/modified-order-item-action-list.component';
import {TbaoUploadedProductItemComponent} from './uploaded-product-item/tbao-uploaded-product-item/tbao-uploaded-product-item.component';
import {
  UploadedProductItemListComponent
} from './uploaded-product-item/uploaded-product-item-list/uploaded-product-item-list.component';
import {RefundDetailGroupingComponent} from './refund-detail-grouping/refund-detail-grouping.component';
import {
  CustomerClubWalletPaymentGroupingComponent
} from './customer-club-wallet-payment-grouping/customer-club-wallet-payment-grouping.component';
import {SataOrderItemsListComponent} from './sata-order-items/sata-order-items-list.component';
import {FactorDeductionListComponent} from './factor-deduction/factor-deduction-list.component';
import {
  RefundDetailForPaymentGroupingComponent
} from './refund-detail-for-payment-grouping/refund-detail-for-payment-grouping.component';
import {
  RefundDetailSetBankAccountComponent
} from './refund-detail-set-bank-account/refund-detail-set-bank-account .component';
import {
  PersonBankAccountSearchComponent
} from './person-bank-account/person-bank-account-serach/person-bank-account-search.component';
import {
  UploadHekmatTransactionFileComponent
} from './upload-hekmat-transaction-file/upload-hekmat-transaction-file.component';
import {UploadHekmatRefundFileComponent} from './upload-hekmat-refund-file/upload-hekmat-refund-file.component';
import {UnFactoredOrderItemListComponent} from './unfactored-order-items/un-factored-order-item-list.component';
import {OrderFullInfoListComponent} from './order-full-info/order-full-info-list.component';
import {
  CustomerClubPaymentInstallmentGroupingComponent
} from './customer-club-payment-installment-grouping/customer-club-payment-installment-grouping.component';
import {UploadFactorPaymentFileComponent} from './upload-factor-payment-file/upload-factor-payment-file.component';
import {ModifyFactorInfoComponent} from './factor/modify-factor-info.component';
import {UploadedVoucherComponent} from './uploaded-voucher/uploaded-voucher.component';
import {UploadProductDetailPagesComponent} from './upload-product-detail-pages/upload-product-detail-pages.component';
import {RefundDetailAssignListComponent} from './refund-detail-assign/refund-detail-assign-list.component';
import {HekmatTransactionListComponent} from './hekmat-transaction/hekmat-transaction-list.component';
import {
  HekmatTransactionItemComponent
} from './hekmat-transaction/hekmat-transaction-item/hekmat-transaction-item.component';
import {UploadHekmatTransactionComponent} from './hekmat-transaction/upload-hekmat-transaction.component';
import {
  RenamedProductListComponent
} from './uploaded-product-item/renamed-products-list/renamed-products-list.component';
import {RefundDetailFullInfoComponent} from './refund-detail-full-info/refund-detail-full-info.component';
import {CitySubsidyFactorListComponent} from './city-subsidy-factor/city-subsidy-factor-list.component';
import {
  RefundDetailAssignedUsersComponent
} from './refund-detail-assigned-users/refund-detail-assigned-users.component';
import {
  ModifiedOrderItemToItselfListComponent
} from './modified-order-item-to-itself/modified-order-item-to-itself.component';
import {CertificateListComponent} from './certificate/certificate-list.component';
import {
  SaleConfigurationAttributeListComponent
} from './sale-configuration-attribute/sale-configuration-attribute-list.component';
import {CategoryGroupListComponent} from './category-group/category-group-list.component';
import {SubsidyGroupListComponent} from './subsidy-group/subsidy-group-list.component';
import {PreRegistrationListComponent} from './pre-registration/pre-registration-list.component';
import {CoursePlanningRouteEnum} from "./course-planning/course-planning-route-enums";
import {CoursePlanningCustomListComponent} from "./course-planning/course-planning-custom-list.component";
import {CoursePlanningForConfirmListComponent} from "./course-planning/course-planning-for-confirm-list.component";
import {CourseCustomListComponent} from "./course/course-custom-list.component";
import {ProductSalesPlanningDiscountListComponent} from './product-sales-planning-discount/product-sales-planning-discount-list.component';
import {UnFactoredProductSalesPlanningComponent} from './unfactored-product-sales-planning/un-factored-product-sales-planning.component';
import {ProductTagsListComponent} from "./product-tags/product-tags-list.component";
import {
  TaxCategorySaleConfigurationListComponent
} from "./tax-category-sale-configuration/tax-category-sale-configuration-list.component";
import {AdditionalUserPropGroupingComponent} from './additional-user-prop-grouping/additional-user-prop-grouping.component';
import {DetermineUserLevelListComponent} from "./determine-user-level/determine-user-level-list.component";
import {UploadedCertificateComponent} from "./uploaded-certificate/uploaded-certificate.component";
import {ProductGroupListComponent} from "./product-title-group/product-group-list.component";
import {ProductGroupSearchComponent} from "./product-title-group/product-group-search/product-group-search.component";
import {FactorItemPaymentListComponent} from "./factor-item-payment/factor-item-payment-list.component";
import {
  FactorPaymentSummaryListComponent
} from "./factor-item-payment-summary/factor-payment-summary-list.component";
import {
  UpdateOrderRecipientMobileNumberListComponent
} from "./update-order-recipient-mobile-number/update-order-recipient-mobile-number-list.component";
import {ConvertedFactorListComponent} from "./factor/converted-factor/converted-factor-list.component";
import {VocherDetailsListComponent} from "./vocher-type/vochers-detial/vocher-details-list.component";
import {
  RefundedSubsidizedOrderItemsListComponent
} from "./refunded-subsidized-order-item-list/refunded-subsidized-order-items-list.component";
import {VocherTypeListComponent} from "./vocher-type/vocher-type-list.component";
import {UploadedProductItemWrapperComponent} from "./uploaded-product-item/uploaded-product-item-wrapper.component";
import {
  AdditionalUserPropCompanyGroupListComponent
} from "./additional-user-prop-company-group/additional-user-prop-company-group-list.component";
import {FactorPaymentInstallmentComponent} from "./factor-payment-installment/factor-payment-installment.component";

const routes: Routes = [
  {path: 'representations', component: RepresentationListComponent, canActivate: [AuthGuard]},
  {path: 'representationPersons', component: RepresentationPersonListComponent, canActivate: [AuthGuard]},
  {path: 'additionalUserProps', component: AdditionalUserPropListComponent, canActivate: [AuthGuard]},
  {
    path: 'representationEstablishedLicenseTypes',
    component: RepresentationEstablishedLicenseTypeListComponent,
    canActivate: [AuthGuard]
  },
  {path: 'personBankAccounts', component: PersonBankAccountListComponent, canActivate: [AuthGuard]},
  {path: 'contracts', component: ContractListComponent, canActivate: [AuthGuard]},
  {path: 'productTemplates', component: ProductTemplateListComponent, canActivate: [AuthGuard]},
  {path: 'giftCardTypes', component: GiftCardTypeListComponent, canActivate: [AuthGuard]},
  {path: 'representationCategorys', component: RepresentationCategoryListComponent, canActivate: [AuthGuard]},
  {path: 'taxCategorys', component: TaxCategoryListComponent, canActivate: [AuthGuard]},
  {path: 'products', component: ProductListComponent, canActivate: [AuthGuard]},
  {path: 'orders', component: OrderListComponent, canActivate: [AuthGuard]},

  {
    path: CoursePlanningRouteEnum.CoursePlannings.toString(),
    component: CoursePlanningCustomListComponent,
    canActivate: [AuthGuard]
  },
  {
    path: CoursePlanningRouteEnum.CustomCoursePlannings.toString(),
    component: CoursePlanningCustomListComponent,
    canActivate: [AuthGuard]
  },
  {path: 'courseSalePlanningForConfirm', component: CoursePlanningForConfirmListComponent, canActivate: [AuthGuard]},
  {path: 'companyMainCategorys', component: CompanyMainCategoryListComponent, canActivate: [AuthGuard]},
  {path: 'dependentProductSales', component: DependentProductSaleListComponent, canActivate: [AuthGuard]},
  {path: 'productCategorys', component: ProductCategoryListComponent, canActivate: [AuthGuard]},
  {path: 'warehouses', component: WarehouseListComponent, canActivate: [AuthGuard]},


  {path: 'companys', component: CompanyListComponent, canActivate: [AuthGuard]},
  {path: 'canUserSeeOtherCompanies', component: CompanyListComponent, canActivate: [AuthGuard]},
  {path: 'contractPaymentMethods', component: ContractPaymentMethodListComponent, canActivate: [AuthGuard]},
  {path: 'saleConfigurations', component: SaleConfigurationListComponent, canActivate: [AuthGuard]},
  {path: 'specifications', component: SpecificationListComponent, canActivate: [AuthGuard]},
  {path: 'productSpecifications', component: ProductSpecificationOptionListComponent, canActivate: [AuthGuard]},
  {path: 'specificationOptions', component: SpecificationOptionListComponent, canActivate: [AuthGuard]},

  {path: 'subsidys', component: SubsidyListComponent, canActivate: [AuthGuard]},
  {path: 'orderItems', component: OrderItemListComponent, canActivate: [AuthGuard]},
  {path: 'subsidys', component: SubsidyListComponent, canActivate: [AuthGuard]},
  {path: 'rewardPointsHistorys', component: RewardPointsHistoryListComponent, canActivate: [AuthGuard]},
  {path: 'shoppingCartItems', component: ShoppingCartItemListComponent, canActivate: [AuthGuard]},
  {path: 'recipients', component: RecipientListComponent, canActivate: [AuthGuard]},
  {path: 'customerClubs', component: CustomerClubListComponent, canActivate: [AuthGuard]},
  {path: 'customerClubCards', component: CustomerClubCardListComponent, canActivate: [AuthGuard]},
  {
    path: 'customerClubPaymentMethodTypes',
    component: CustomerClubPaymentMethodTypeListComponent,
    canActivate: [AuthGuard]
  },
  {path: 'canceledOrderItems', component: CanceledOrderItemListComponent, canActivate: [AuthGuard]},
  {path: 'applicationPaymentMethods', component: ApplicationPaymentMethodListComponent, canActivate: [AuthGuard]},
  {path: 'factorItems', component: FactorItemListComponent, canActivate: [AuthGuard]},
  {path: 'factors', component: FactorListComponent, canActivate: [AuthGuard]},
  {path: 'groupedFactors', component: GroupedFactorListComponent, canActivate: [AuthGuard]},
  {path: 'factorPayments', component: CustomFactorPaymentListComponent, canActivate: [AuthGuard]},
  {path: 'contentCategory', component: ContentCategoryListComponent, canActivate: [AuthGuard]},
  {path: 'applicationCustomerClubs', component: ApplicationCustomerClubListComponent, canActivate: [AuthGuard]},
  {path: 'unFactoredOrderItems', component: UnFactoredOrderItemListComponent, canActivate: [AuthGuard]},
  {path: 'factorVerifications', component: FactorVerificationListComponent, canActivate: [AuthGuard]},
  {
    path: 'factorVerificationConfigurations',
    component: FactorVerificationConfigurationListComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'factorVerificationConfigurationRoles',
    component: FactorVerificationConfigurationRoleListComponent,
    canActivate: [AuthGuard]
  },
  {path: 'supervisorFactors', component: SupervisorFactorListComponent, canActivate: [AuthGuard]},
  {path: 'factorGroups', component: FactorGroupItemCustomListComponent, canActivate: [AuthGuard]},
  {path: 'issues', component: IssueListComponent, canActivate: [AuthGuard]},
  {path: 'issueLogWorks', component: IssueLogWorkListComponent, canActivate: [AuthGuard]},
  {path: 'issueParticipants', component: IssueParticipantListComponent, canActivate: [AuthGuard]},
  {path: 'issueTypeItems', component: IssueTypeItemListComponent, canActivate: [AuthGuard]},
  {path: 'issueComments', component: IssueCommentListComponent, canActivate: [AuthGuard]},
  {path: 'issueSearch', component: IssueSearchComponent, canActivate: [AuthGuard]},
  {path: 'sataSubsidyApproval', component: SataSubsidyApprovalListComponent, canActivate: [AuthGuard]},

  {path: 'completedOrderItems', component: CompletedOrderItemsComponent, canActivate: [AuthGuard]},
  {path: 'orderItemsFullInfo', component: OrderItemFullInfoListComponent, canActivate: [AuthGuard]},
  {path: 'payments', component: PaymentListComponent, canActivate: [AuthGuard]},
  {path: 'yashilSchedulers', component: YashilSchedulerListComponent, canActivate: [AuthGuard]},
  {path: 'updateOrderNationalCodes', component: UpdateOrderNationalCodeListComponent, canActivate: [AuthGuard]},
  {path: 'modifiedOrderItems', component: ModifiedOrderItemListComponent, canActivate: [AuthGuard]},
  {path: 'modifiedOrderItemAction', component: ModifiedOrderItemActionListComponent, canActivate: [AuthGuard]},
  {path: 'paymentGroupings', component: CustomerClubWalletPaymentGroupingComponent, canActivate: [AuthGuard]},
  {path: 'uploadedProductItem', component: UploadedProductItemWrapperComponent, canActivate: [AuthGuard]},
  {path: 'UploadedProductItemList', component: UploadedProductItemListComponent, canActivate: [AuthGuard]},
  {path: 'factorDeductions', component: FactorDeductionListComponent, canActivate: [AuthGuard]},
  {path: 'refundDetailGrouping', component: RefundDetailGroupingComponent, canActivate: [AuthGuard]},
  {
    path: 'refundDetailForPaymentGrouping',
    component: RefundDetailForPaymentGroupingComponent,
    canActivate: [AuthGuard]
  },
  {path: 'sataOrderItems', component: SataOrderItemsListComponent, canActivate: [AuthGuard]},
  {path: 'personBankAccountSearch', component: PersonBankAccountSearchComponent, canActivate: [AuthGuard]},
  {path: 'refundDetailSetBankAccount', component: RefundDetailSetBankAccountComponent, canActivate: [AuthGuard]},
  {path: 'uploadHekmatTransactionFile', component: UploadHekmatTransactionFileComponent, canActivate: [AuthGuard]},
  {path: 'uploadHekmatRefundFile', component: UploadHekmatRefundFileComponent, canActivate: [AuthGuard]},
  {path: 'orderFullInfo', component: OrderFullInfoListComponent, canActivate: [AuthGuard]},
  {
    path: 'customerClubPaymentInstallmentGroupings',
    component: CustomerClubPaymentInstallmentGroupingComponent,
    canActivate: [AuthGuard]
  },
  {path: 'uploadFactorPayments', component: UploadFactorPaymentFileComponent, canActivate: [AuthGuard]},
  {path: 'modifyFactorInfo', component: ModifyFactorInfoComponent, canActivate: [AuthGuard]},
  {path: 'uploadedVoucherCode', component: UploadedVoucherComponent, canActivate: [AuthGuard]},
  {path: 'uploadProductDetailPages', component: UploadProductDetailPagesComponent, canActivate: [AuthGuard]},
  {path: 'hekmatTransactions', component: HekmatTransactionListComponent, canActivate: [AuthGuard]},
  {path: 'refundDetailsAssignee', component: RefundDetailAssignListComponent, canActivate: [AuthGuard]},
  {path: 'hekmatTransactionItem', component: HekmatTransactionItemComponent, canActivate: [AuthGuard]},
  {path: 'uploadHekmatTransaction', component: UploadHekmatTransactionComponent, canActivate: [AuthGuard]},
  {path: 'renamedProductList', component: RenamedProductListComponent, canActivate: [AuthGuard]},
  {path: 'refundDetailFullInfo', component: RefundDetailFullInfoComponent, canActivate: [AuthGuard]},
  {path: 'refundDetailAssignedUsers', component: RefundDetailAssignedUsersComponent, canActivate: [AuthGuard]},
  {path: 'citySubsidyFactors', component: CitySubsidyFactorListComponent, canActivate: [AuthGuard]},
  {path: 'vocherTypes', component: VocherDetailsListComponent, canActivate: [AuthGuard]},
  {path: 'modifiedOrderItemToItself', component: ModifiedOrderItemToItselfListComponent, canActivate: [AuthGuard]},
  {path: 'certificateList', component: CertificateListComponent, canActivate: [AuthGuard]},
  {path: 'saleConfigurationAttributes', component: SaleConfigurationAttributeListComponent, canActivate: [AuthGuard]},
  {path: 'categoryGroups', component: CategoryGroupListComponent, canActivate: [AuthGuard]},
  {path: 'subsidyGroups', component: SubsidyGroupListComponent, canActivate: [AuthGuard]},
  {path: 'preRegistrations', component: PreRegistrationListComponent, canActivate: [AuthGuard]},
  {path: 'courses', component: CourseCustomListComponent, canActivate: [AuthGuard]},
  {path: 'productSalesPlanningDiscounts', component: ProductSalesPlanningDiscountListComponent, canActivate: [AuthGuard]},
  {path: 'unFactoredProductSalesPlanningComponent', component: UnFactoredProductSalesPlanningComponent, canActivate: [AuthGuard]},
  {path: 'productTags', component: ProductTagsListComponent, canActivate: [AuthGuard]},
  {path: 'taxCategorySaleConfigurations', component: TaxCategorySaleConfigurationListComponent, canActivate: [AuthGuard]},
  {path: 'additionalUserPropGroup', component: AdditionalUserPropGroupingComponent, canActivate: [AuthGuard]},
  {path: 'determineUserLevel', component: DetermineUserLevelListComponent, canActivate: [AuthGuard]},
  {path: 'uploadedCertificate', component: UploadedCertificateComponent, canActivate: [AuthGuard]},
  {path: 'productGroups', component: ProductGroupListComponent, canActivate: [AuthGuard]},
  {path: 'productGroupsTest', component: ProductGroupSearchComponent, canActivate: [AuthGuard]},
  {path: 'factorMicroPayments', component: FactorItemPaymentListComponent, canActivate: [AuthGuard]},
  {path: 'factorPaymentSummary', component: FactorPaymentSummaryListComponent, canActivate: [AuthGuard]},
  {path: 'updateOrderRecipientMobileNumber', component: UpdateOrderRecipientMobileNumberListComponent, canActivate: [AuthGuard]},
  {path: 'vocherTypeVocherListComponent', component: VocherTypeListComponent, canActivate: [AuthGuard]},
  {path: 'factorItemsWithoutImpact', component: ConvertedFactorListComponent, canActivate: [AuthGuard]},
  {path: 'vocherTypeVocherListComponent', component: VocherDetailsListComponent, canActivate: [AuthGuard]},
  {path: 'refundedSubsidizedOrderItemsList', component: RefundedSubsidizedOrderItemsListComponent, canActivate: [AuthGuard]},
  {path: 'additionalUserPropCompanyGroupListComponent',component : AdditionalUserPropCompanyGroupListComponent,canActivate:[AuthGuard]},
  {path: 'factorPaymentInstallment',component : FactorPaymentInstallmentComponent,canActivate:[AuthGuard]},

];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
  }
)
export class ECommerceRoutingModule {

}
