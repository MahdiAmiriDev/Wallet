"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.ENTRYCOMPONENTS = void 0;
var representation_detail_tab_based_component_1 = require("./representation/representation-detail-tab-based.component");
var representation_detail_component_1 = require("./representation/representation-detail.component");
var representation_person_detail_component_1 = require("./representation-person/representation-person-detail.component");
var additional_user_prop_detail_component_1 = require("./additional-user-prop/additional-user-prop-detail.component");
var representation_established_license_type_detail_component_1 = require("./representation-established-license-type/representation-established-license-type-detail.component");
var person_bank_account_detail_component_1 = require("./person-bank-account/person-bank-account-detail.component");
var contract_detail_component_1 = require("./contract/contract-detail.component");
var product_template_detail_component_1 = require("./product-template/product-template-detail.component");
var gift_card_type_detail_component_1 = require("./gift-card-type/gift-card-type-detail.component");
var representation_category_detail_component_1 = require("./representation-category/representation-category-detail.component");
var tax_category_detail_component_1 = require("./tax-category/tax-category-detail.component");
var product_detail_component_1 = require("./product/product-detail.component");
var order_detail_component_1 = require("./order/order-detail.component");
var product_sales_planning_detail_component_1 = require("./product-sales-planning/product-sales-planning-detail.component");
var company_main_category_detail_component_1 = require("./company-main-category/company-main-category-detail.component");
var dependent_product_sale_detail_component_1 = require("./dependent-product-sale/dependent-product-sale-detail.component");
var product_category_detail_component_1 = require("./product-category/product-category-detail.component");
var warehouse_detail_component_1 = require("./warehouse/warehouse-detail.component");
var category_detail_component_1 = require("./category/category-detail.component");
var company_detail_component_1 = require("./company/company-detail.component");
var contract_payment_method_detail_component_1 = require("./contract-payment-method/contract-payment-method-detail.component");
var sale_configuration_detail_component_1 = require("./sale-configuration/sale-configuration-detail.component");
var specification_detail_component_1 = require("./specification/specification-detail.component");
var specification_option_detail_component_1 = require("./specification-option/specification-option-detail.component");
var simple_product_detail_component_1 = require("./product/simple-product/simple-product-detail.component");
var representation_filter_component_1 = require("./representation-filter/representation-filter.component");
var category_select_component_1 = require("./category/select/category-select.component");
var simple_product_detail_tab_based_component_1 = require("./product/simple-product/simple-product-detail-tab-based.component");
var category_specification_detail_component_1 = require("./category-specification/category-specification-detail.component");
var category_detail_tab_based_component_1 = require("./category/category-detail-tab-based.component");
var product_type_detail_component_1 = require("./product-type/product-type-detail.component");
var product_type_select_1 = require("./product-type/product-type-select/product-type-select");
var product_type_specification_detail_component_1 = require("./product-type-specification/product-type-specification-detail.component");
var product_specification_option_detail_component_1 = require("./product-specification-option/product-specification-option-detail.component");
var product_select_component_1 = require("./product/product-select/product-select.component");
var product_selector_component_1 = require("./product/product-select/product-selector.component");
var company_detail_tab_based_component_1 = require("./company/company-detail-tab-based.component");
var subsidy_detail_component_1 = require("./subsidy/subsidy-detail.component");
var order_item_detail_component_1 = require("./order-item/order-item-detail.component");
var canceled_order_item_detail_component_1 = require("./canceled-order-item/canceled-order-item-detail.component");
var reward_points_history_detail_component_1 = require("./reward-points-history/reward-points-history-detail.component");
var customer_club_detail_component_1 = require("./customer-club/customer-club-detail.component");
var customer_club_payment_method_type_detail_component_1 = require("./customer-club-payment-method-type/customer-club-payment-method-type-detail.component");
var recipient_detail_component_1 = require("./recipient/recipient-detail.component");
var customer_club_card_detail_component_1 = require("./customer-club-card/customer-club-card-detail.component");
var shopping_cart_item_detail_component_1 = require("./shopping-cart-item/shopping-cart-item-detail.component");
var application_payment_method_detail_component_1 = require("./application-payment-method/application-payment-method-detail.component");
var factor_item_detail_component_1 = require("./factor-item/factor-item-detail.component");
var factor_payment_detail_component_1 = require("./factor-payment/factor-payment-detail.component");
var factor_detail_component_1 = require("./factor/factor-detail.component");
var factor_detail_tab_based_component_1 = require("./factor/factor-detail-tab-based.component");
var content_category_detail_component_1 = require("./content-category/content-category-detail.component");
var content_category_detail_tab_based_component_1 = require("./content-category/content-category-detail-tab-based.component");
var factor_verification_detail_tab_based_component_1 = require("./factor-verification/factor-verification-detail-tab-based.component");
var application_customer_club_detail_component_1 = require("./application-customer-club/application-customer-club-detail.component");
var factor_verification_detail_component_1 = require("./factor-verification/factor-verification-detail.component");
var factor_verification_configuration_role_detail_component_1 = require("./factor-verification-configuration-role/factor-verification-configuration-role-detail.component");
var factor_verification_configuration_detail_component_1 = require("./factor-verification-configuration/factor-verification-configuration-detail.component");
var factor_verification_list_for_factor_component_1 = require("./factor-verification/factor-verification-list-for-factor.component");
var factor_verification_description_list_for_factor_component_1 = require("./factor-verification/factor-verification-description-list-for-factor.component");
exports.ENTRYCOMPONENTS = [
    company_detail_tab_based_component_1.CompanyDetailTabBasedComponent,
    product_selector_component_1.ProductSelectorComponent,
    product_select_component_1.ProductSelectComponent,
    product_specification_option_detail_component_1.ProductSpecificationOptionDetailComponent,
    product_type_specification_detail_component_1.ProductTypeSpecificationDetailComponent,
    product_type_detail_component_1.ProductTypeDetailComponent,
    product_type_select_1.ProductTypeSelect,
    category_detail_tab_based_component_1.CategoryDetailTabBasedComponent,
    category_specification_detail_component_1.CategorySpecificationDetailComponent,
    simple_product_detail_tab_based_component_1.SimpleProductDetailTabBasedComponent,
    category_select_component_1.CategorySelectComponent,
    representation_filter_component_1.RepresentationFilterComponent,
    simple_product_detail_component_1.ProductSimpleDetailComponent,
    representation_detail_tab_based_component_1.RepresentationDetailTabBasedComponent,
    representation_detail_component_1.RepresentationDetailComponent,
    representation_person_detail_component_1.RepresentationPersonDetailComponent,
    additional_user_prop_detail_component_1.AdditionalUserPropDetailComponent,
    representation_established_license_type_detail_component_1.RepresentationEstablishedLicenseTypeDetailComponent,
    person_bank_account_detail_component_1.PersonBankAccountDetailComponent,
    contract_detail_component_1.ContractDetailComponent,
    product_template_detail_component_1.ProductTemplateDetailComponent,
    gift_card_type_detail_component_1.GiftCardTypeDetailComponent,
    representation_category_detail_component_1.RepresentationCategoryDetailComponent,
    tax_category_detail_component_1.TaxCategoryDetailComponent,
    product_detail_component_1.ProductDetailComponent,
    order_detail_component_1.OrderDetailComponent,
    product_sales_planning_detail_component_1.ProductSalesPlanningDetailComponent,
    company_main_category_detail_component_1.CompanyMainCategoryDetailComponent,
    dependent_product_sale_detail_component_1.DependentProductSaleDetailComponent,
    product_category_detail_component_1.ProductCategoryDetailComponent,
    warehouse_detail_component_1.WarehouseDetailComponent,
    category_detail_component_1.CategoryDetailComponent,
    company_detail_component_1.CompanyDetailComponent,
    contract_payment_method_detail_component_1.ContractPaymentMethodDetailComponent,
    sale_configuration_detail_component_1.SaleConfigurationDetailComponent,
    specification_detail_component_1.SpecificationDetailComponent,
    specification_option_detail_component_1.SpecificationOptionDetailComponent,
    subsidy_detail_component_1.SubsidyDetailComponent,
    order_item_detail_component_1.OrderItemDetailComponent,
    subsidy_detail_component_1.SubsidyDetailComponent,
    reward_points_history_detail_component_1.RewardPointsHistoryDetailComponent,
    shopping_cart_item_detail_component_1.ShoppingCartItemDetailComponent,
    recipient_detail_component_1.RecipientDetailComponent,
    customer_club_detail_component_1.CustomerClubDetailComponent,
    customer_club_card_detail_component_1.CustomerClubCardDetailComponent,
    customer_club_payment_method_type_detail_component_1.CustomerClubPaymentMethodTypeDetailComponent,
    canceled_order_item_detail_component_1.CanceledOrderItemDetailComponent,
    application_payment_method_detail_component_1.ApplicationPaymentMethodDetailComponent,
    factor_item_detail_component_1.FactorItemDetailComponent,
    factor_detail_component_1.FactorDetailComponent,
    factor_payment_detail_component_1.FactorPaymentDetailComponent,
    factor_detail_tab_based_component_1.FactorDetailTabBasedComponent,
    content_category_detail_component_1.ContentCategoryDetailComponent,
    content_category_detail_tab_based_component_1.ContentCategoryDetailTabBasedComponent,
    application_customer_club_detail_component_1.ApplicationCustomerClubDetailComponent,
    factor_verification_detail_tab_based_component_1.FactorVerificationDetailTabBasedComponent,
    factor_verification_detail_component_1.FactorVerificationDetailComponent,
    factor_verification_configuration_detail_component_1.FactorVerificationConfigurationDetailComponent,
    factor_verification_configuration_role_detail_component_1.FactorVerificationConfigurationRoleDetailComponent,
    factor_verification_list_for_factor_component_1.FactorVerificationListForFactorComponent,
    factor_verification_description_list_for_factor_component_1.FactorVerificationDescriptionListForFactorComponent,
];
//# sourceMappingURL=entryIndex.js.map