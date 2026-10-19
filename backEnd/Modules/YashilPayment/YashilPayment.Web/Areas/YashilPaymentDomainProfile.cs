using AutoMapper;
using Yashil.Common.Core.Interfaces;
using Yashil.Core.CustomDtos.Payment;
using Yashil.Core.Entities;
using YashilPayment.Core.Const;
using YashilPayment.Core.Enums;
using YashilPayment.Web.Areas.PaymentAdminApi.ViewModels;

namespace YashilPayment.Web.Areas
{
    public class YashilPaymentProfile : Profile, IOrderedMapperProfile
    {
        public int Order => 1;

        public YashilPaymentProfile()
        {
            CreateMap<CardInfo, CardInfoEditModel>();

            CreateMap<CardInfo, CardInfoListViewModel>()
                .ForMember(x => x.AccessLevelTitle,
                    b => b.MapFrom(c => c.AccessLevel.Title))
                .ForMember(x => x.CardTypeTitle, b => b.MapFrom(c => c.CardTypeNavigation.Title))
                .ForMember(x => x.PersonTitle,
                    b => b.MapFrom(c => string.Concat(c.Person.Name, " " + c.Person.LastName)))
                .ForMember(x => x.OrganizationTitle,
                    b => b.MapFrom(c => c.Person.User.FirstOrDefault().Organization.Title))
                .ForMember(x => x.NationalCode, b => b.MapFrom(c => c.Person.NationalCode));
            CreateMap<CardInfoEditModel, CardInfo>();

            CreateMap<CardInfo, CardInfoSimpleViewModel>();


            CreateMap<CreditOrCashPayment, CreditOrCashPaymentEditModel>();

            CreateMap<CreditOrCashPayment, CreditOrCashPaymentListViewModel>()
                .ForMember(x => x.AccessLevelTitle,
                    b => b.MapFrom(c => c.AccessLevel.Title))
                .ForMember(x => x.MerchantTitle,
                    b => b.MapFrom(c => c.Merchant.MerchantName))
                .ForMember(x => x.CardTitle,
                    b => b.MapFrom(c => c.Card.CardNumber))
                .ForMember(x => x.StatusTitle, b => b.MapFrom(c => c.StatusNavigation.Title));
            CreateMap<CreditOrCashPaymentEditModel, CreditOrCashPayment>();

            CreateMap<CreditOrCashPayment, CreditOrCashPaymentSimpleViewModel>();


            CreateMap<Installment, InstallmentEditModel>();

            CreateMap<Installment, InstallmentListViewModel>()
                .ForMember(x => x.AccessLevelTitle,
                    b => b.MapFrom(c => c.AccessLevel.Title))
                .ForMember(x => x.PaymentTitle,
                    b => b.MapFrom(c => c.Payment.Id))
                .ForMember(x => x.PaymentStatusTitle, b => b.MapFrom(c => c.PaymentStatusNavigation.Title));
            CreateMap<InstallmentEditModel, Installment>();

            CreateMap<Installment, InstallmentSimpleViewModel>();


            CreateMap<CardAmountHistory, CardAmountHistoryEditModel>();

            CreateMap<CardAmountHistory, CardAmountHistoryListViewModel>()
                .ForMember(x => x.AccessLevelTitle,
                    b => b.MapFrom(c => c.AccessLevel.Title))
                .ForMember(x => x.CardNumber, b => b.MapFrom(c => c.CardInfo.CardNumber))
                .ForMember(x => x.OwnerName,
                    b => b.MapFrom(c => string.Concat(c.CardInfo.Person.Name, " " + c.CardInfo.Person.LastName)))
                .ForMember(x => x.CardType, b => b.MapFrom(c => c.CardInfo.CardTypeNavigation.Title))
                .ForMember(x => x.OrganizationTitle,
                    b => b.MapFrom(c => c.CardInfo.Person.User.FirstOrDefault().Organization.Title))
                .ForMember(x => x.NationalCode, b => b.MapFrom(c => c.CardInfo.Person.NationalCode))
                ;
            CreateMap<CardAmountHistoryEditModel, CardAmountHistory>();

            CreateMap<CardAmountHistory, CardAmountHistorySimpleViewModel>();


            CreateMap<MerchantInfo, MerchantInfoEditModel>();

            CreateMap<MerchantInfo, MerchantInfoListViewModel>()
                .ForMember(x => x.AccessLevelTitle,
                    b => b.MapFrom(c => c.AccessLevel.Title))
                .ForMember(x => x.CityTitle, b =>
                    b.MapFrom(c => c.City.Title))
                .ForMember(x => x.Username, b =>
                    b.MapFrom(c => c.User.UserName));

            CreateMap<MerchantInfoEditModel, MerchantInfo>();

            CreateMap<MerchantInfo, MerchantInfoSimpleViewModel>()
                .ForMember(x => x.Title,
                    b => b.MapFrom(c => c.MerchantName));


            CreateMap<MerchantTerminalInfo, MerchantTerminalInfoEditModel>();

            CreateMap<MerchantTerminalInfo, MerchantTerminalInfoListViewModel>()
                .ForMember(x => x.AccessLevelTitle,
                    b => b.MapFrom(c => c.AccessLevel.Title))
                .ForMember(x => x.MerchantTitle,
                    b => b.MapFrom(c => c.Merchant.MerchantName))
                ;
            CreateMap<MerchantTerminalInfoEditModel, MerchantTerminalInfo>();

            CreateMap<MerchantTerminalInfo, MerchantTerminalInfoSimpleViewModel>();
            CreateMap<HekmatPaymentGatewayTransaction, HekmatPaymentGatewayTransactionEditModel>();

            CreateMap<HekmatPaymentGatewayTransaction, HekmatPaymentGatewayTransactionListViewModel>()
                .ForMember(x => x.HekmatTerminalTitle,
                    b => b.MapFrom(c => c.HekmatTerminal.Title))
                .ForMember(x => x.HekmatReservationNumber,
                    b => b.MapFrom(c => c.HekmatReservationNumber))
                .ForMember(x => x.LocalReservationNumber,
                    b => b.MapFrom(c => c.LocalReservationNumber))
                .ForMember(x => x.MerchantTitle,
                    b => b.MapFrom(c => c.YashilHekmatTerminal.Merchant.MerchantName))
                ;
            CreateMap<HekmatPaymentGatewayTransactionEditModel, HekmatPaymentGatewayTransaction>();

            CreateMap<HekmatPaymentGatewayTransaction, HekmatPaymentGatewayTransactionSimpleViewModel>();

            CreateMap<HekmatPaymentGatewayTransaction, HekmatPaymentGatewayTransactionCustomViewModel>()
                .ForMember(x => x.IsLearn,
                    b =>
                        b.MapFrom(c => c.HekmatTerminal.IsLearn ? 1 : 0))
                ;


            CreateMap<HekmatTerminal, HekmatTerminalEditModel>();

            CreateMap<HekmatTerminal, HekmatTerminalListViewModel>()
                .ForMember(x => x.AccessLevelTitle,
                    b => b.MapFrom(c => c.AccessLevel.Title))
                ;
            CreateMap<HekmatTerminalEditModel, HekmatTerminal>();

            CreateMap<HekmatTerminal, HekmatTerminalSimpleViewModel>();


            CreateMap<YashilHekmatTerminal, YashilHekmatTerminalEditModel>();

            CreateMap<YashilHekmatTerminal, YashilHekmatTerminalListViewModel>()
                .ForMember(x => x.HekmatTerminalTitle,
                    b => b.MapFrom(c => c.HekmatTerminal.Title))
                .ForMember(x => x.MerchantTitle,
                    b => b.MapFrom(c => c.Merchant.MerchantName));


            CreateMap<YashilHekmatTerminal, YashilHekmatTerminalWithMerchantViewModel>()
                .ForMember(x => x.IsActive,
                    b => b.MapFrom(c => c.IsActive))
                .ForMember(x => x.MerchantUrl,
                    b => b.MapFrom(c => c.Url))
                .ForMember(x => x.HekmatTerminalTitle,
                    b => b.MapFrom(c => c.HekmatTerminal.Title))
                .ForMember(x => x.MerchantName,
                    b => b.MapFrom(c => c.Merchant.MerchantName));

            CreateMap<YashilHekmatTerminalEditModel, YashilHekmatTerminal>();

            CreateMap<YashilHekmatTerminal, YashilHekmatTerminalSimpleViewModel>();

            CreateMap<HybridPayment, HybridPaymentEditModel>();

            CreateMap<HybridPayment, HybridPaymentListViewModel>()
                .ForMember(x => x.HybridPaymentRequestTitle,
                    b => b.MapFrom(c => c.HybridPaymentRequest.Title))
                .ForMember(x => x.HybridPaymentStatusTypeTitle, b =>
                    b.MapFrom(c => c.HybridPaymentStatusTypeNavigation.Title))
                .ForMember(x => x.PaymentMethodTypeTitle, b =>
                    b.MapFrom(c => c.PaymentMethodTypeNavigation.Title))
                .ForMember(x => x.AccessLevelTitle,
                    b => b.MapFrom(c => c.AccessLevel.Title))
                ;
            CreateMap<HybridPaymentEditModel, HybridPayment>();

            CreateMap<HybridPayment, HybridPaymentSimpleViewModel>();
            CreateMap<HybridPayment, HybridPaymentIpgListViewModel>()
                .ForMember(x => x.PaidDateInt,
                    b => b.MapFrom(c => c.PaidDateInt))
                .ForMember(x => x.PaymentMethodtype,
                    b => b.MapFrom(c => c.PaymentMethodType))
                .ForMember(x => x.PaymentMethodTypeTitle,
                    b => b.MapFrom(c => c.PaymentMethodTypeNavigation.Title))
                .ForMember(x => x.HybridPaymentStatusType,
                    b => b.MapFrom(c => c.HybridPaymentStatusType))
                .ForMember(x => x.HybridPaymentStatusTypeTitle,
                    b => b.MapFrom(c => c.HybridPaymentStatusTypeNavigation.Title))
                .ForMember(b => b.SaleType,
                    b => b.MapFrom(c => c.SaleType))
                .ForMember(b => b.SaleTypeTitle,
                    b => b.MapFrom(c => c.SaleType))
                .ForMember(x => x.Price,
                    b => b.MapFrom(c => c.Price))
                ;


            CreateMap<HybridPaymentRequest, HybridPaymentRequestEditModel>();

            CreateMap<HybridPaymentRequest, HybridPaymentRequestListViewModel>()
                .ForMember(x => x.MerchantTitle,
                    b => b.MapFrom(c => c.Merchant.Title))
                .ForMember(x => x.YashilHekmatTerminalTitle,
                    b => b.MapFrom(c => c.YashilHekmatTerminal.Title))
                .ForMember(x => x.HybridPaymentRequestStatusTypeTitle,
                    b => b.MapFrom(c => c.HybridPaymentRequestStatusTypeNavigation.Title)).ForMember(
                    x => x.AccessLevelTitle,
                    b => b.MapFrom(c => c.AccessLevel.Title))
                ;
            CreateMap<HybridPaymentRequestEditModel, HybridPaymentRequest>();

            CreateMap<HybridPaymentRequest, HybridPaymentRequestSimpleViewModel>();
            CreateMap<HybridPaymentRequest, HybridPaymentRequestIpgViewModel>()
                .ForMember(x => x.AdditionalData,
                    b => b.MapFrom(c => c.AdditionalDate))
                .ForMember(x => x.CardNumber,
                    b => b.MapFrom(c => c.HekmatCardNumber))
                .ForMember(x => x.HybridPaymentRequestStatusTypeTitle,
                    b => b.MapFrom(c => c.HybridPaymentRequestStatusTypeNavigation.Title))
                .ForMember(x => x.HybridPayments,
                    b => b.MapFrom(c => c.HybridPayment))

                ;
            CreateMap<HybridPayment, HybridPaymentIpgListViewModel>()
                .ForMember(x => x.PaidDateInt,
                    b => b.MapFrom(c => c.PaidDateInt))
                .ForMember(x => x.SaleType,
                    b => b.MapFrom(c => c.SaleType))
                 .ForMember(x => x.SaleTypeTitle,
                     b =>
                         b.MapFrom(c => c.SaleTypeNavigation.Title))
                .ForMember(x => x.PaymentMethodtype,
                    b => b.MapFrom(c => c.PaymentMethodType))
                .ForMember(x => x.PaymentMethodTypeTitle,
                    b => b.MapFrom(c => c.PaymentMethodTypeNavigation.Title))
                .ForMember(x => x.HybridPaymentStatusTypeTitle,
                    b => b.MapFrom(c => c.HybridPaymentStatusTypeNavigation.Title))
                .ForMember(x => x.HybridPaymentStatusType,
                    b => b.MapFrom(c => c.HybridPaymentStatusType))
                .ForMember(x => x.Price,
                    b => b.MapFrom(c => c.Price))
                ;

            CreateMap<HybridPayment, HybridPaymentIpgVerifyViewModel>()
                .ForMember(x => x.Id,
                    b => b.MapFrom(c => c.Id))
                .ForMember(x => x.Price,
                    b => b.MapFrom(c => c.Price))
                .ForMember(x => x.AuthorizationTransactionResult,
                    b => b.MapFrom(c => c.AuthorizationTransactionResult))
                .ForMember(x => x.PaidDateUtc,
                    b => b.MapFrom(c => c.PaidDateUtc))
                .ForMember(x => x.HybridPaymentStatusType,
                    b => b.MapFrom(c => c.HybridPaymentStatusType))
                .ForMember(x => x.HybridPaymentRequestId,
                    b => b.MapFrom(c => c.HybridPaymentRequestId))
                .ForMember(x => x.HybridPaymentRequestTitle,
                    b => b.MapFrom(c => c.HybridPaymentRequest.Title))
                .ForMember(x => x.HybridPaymentRequestCallBackUrl,
                    b => b.MapFrom(c => c.HybridPaymentRequest.CallBackUrl))
                .ForMember(x => x.UseFormula, b => b.MapFrom(c => c.HybridPaymentRequest.UseFormula))
                ;

            CreateMap<HybridPaymentDto, HybridPaymentIpgListViewModel>();


            CreateMap<PaymentGateway, PaymentGatewayEditModel>();

            CreateMap<PaymentGateway, PaymentGatewayListViewModel>()
                .ForMember(x => x.AccessLevelTitle,
                    b => b.MapFrom(c => c.AccessLevel.Title))
                ;
            CreateMap<PaymentGatewayEditModel, PaymentGateway>();

            CreateMap<PaymentGateway, PaymentGatewaySimpleViewModel>();


            CreateMap<PaymentGatewayPaymentMethod, PaymentGatewayPaymentMethodEditModel>();

            CreateMap<PaymentGatewayPaymentMethod, PaymentGatewayPaymentMethodListViewModel>()
                .ForMember(x => x.PaymentGatewayTitle,
                    b => b.MapFrom(c => c.PaymentGateway.Title))
                .ForMember(x => x.PaymentMethodTypeTitle, b => b.MapFrom(c => c.PaymentMethodTypeNavigation.Title)).ForMember(x => x.AccessLevelTitle,
                    b => b.MapFrom(c => c.AccessLevel.Title))
                ;
            CreateMap<PaymentGatewayPaymentMethodEditModel, PaymentGatewayPaymentMethod>();

            CreateMap<PaymentGatewayPaymentMethod, PaymentGatewayPaymentMethodSimpleViewModel>();

            CreateMap<HekmatPaymentGatewayTransactionBulkRequest, HekmatPaymentGatewayTransactionBulkRequestEditModel>();
            CreateMap<HekmatPaymentGatewayTransactionBulkRequest, HekmatPaymentGatewayTransactionBulkRequestListViewModel>()


                    .ForMember(x => x.HekmatPaymentGatewayTransactionBulkRequestCategoryTitle,
                    b => b.MapFrom(c => c.HekmatPaymentGatewayTransactionBulkRequestCategory.Title))


                    .ForMember(x => x.AccessLevelTitle,
                    b => b.MapFrom(c => c.AccessLevel.Title))
                ;
            CreateMap<HekmatPaymentGatewayTransactionBulkRequestEditModel, HekmatPaymentGatewayTransactionBulkRequest>();
            CreateMap<HekmatPaymentGatewayTransactionBulkRequest, HekmatPaymentGatewayTransactionBulkRequestSimpleViewModel>();

            CreateMap<HekmatPaymentGatewayTransactionBulkRequestCategory, HekmatPaymentGatewayTransactionBulkRequestCategoryEditModel>();
            CreateMap<HekmatPaymentGatewayTransactionBulkRequestCategory, HekmatPaymentGatewayTransactionBulkRequestCategoryListViewModel>()


                    .ForMember(x => x.YashilHekmatTerminalTitle,
                    b => b.MapFrom(c =>
                        c.YashilHekmatTerminal.Title))
                    .ForMember(x => x.yashilHekmatTerminalId,
                        b =>
                            b.MapFrom(c => c.YashilHekmatTerminal.Id))

                    .ForMember(x => x.AccessLevelTitle,
                    b => b.MapFrom(c => c.AccessLevel.Title))
                ;
            CreateMap<HekmatPaymentGatewayTransactionBulkRequestCategoryEditModel, HekmatPaymentGatewayTransactionBulkRequestCategory>();
            CreateMap<HekmatPaymentGatewayTransactionBulkRequestCategory, HekmatPaymentGatewayTransactionBulkRequestCategorySimpleViewModel>();

            CreateMap<HekmatPaymentGatewayTransactionBulkRequestDetail, HekmatPaymentGatewayTransactionBulkRequestDetailEditModel>();
            CreateMap<HekmatPaymentGatewayTransactionBulkRequestDetail, HekmatPaymentGatewayTransactionBulkRequestDetailListViewModel>()


                    .ForMember(x => x.AppDocumentTitle,
                    b => b.MapFrom(c => c.AppDocument.Title))


                    .ForMember(x => x.HekmatPaymentGatewayTransactionBulkRequestTitle,
                    b => b.MapFrom(c => c.HekmatPaymentGatewayTransactionBulkRequest.Title))


                    .ForMember(x => x.Status,
                        b =>
                            b.MapFrom(c => 
                                c.StatusCode == (int)HekmatStatusCodeEnum.Success && c.ErrorCode == (int)HekmatErrorCodeEnum.Success
                                    ? HekmatStatusConst.Successful 
                                    : c.StatusCode == (int)HekmatStatusCodeEnum.Unsuccessful && c.ErrorCode == (int)HekmatErrorCodeEnum.Unsuccessful ?
                                    HekmatStatusConst.UnSuccessful
                                    : c.StatusCode == (int)HekmatStatusCodeEnum.Waiting && c.ErrorCode == (int)HekmatErrorCodeEnum.SnedToHekmat ?
                                        HekmatStatusConst.Waiting
                                    : !c.StatusCode.HasValue && !c.ErrorCode.HasValue  ?
                                        HekmatStatusConst.NotStatus  
                                        : string.Empty));
            CreateMap<HekmatPaymentGatewayTransactionBulkRequestDetailEditModel, HekmatPaymentGatewayTransactionBulkRequestDetail>();
            CreateMap<HekmatPaymentGatewayTransactionBulkRequestDetail, HekmatPaymentGatewayTransactionBulkRequestDetailSimpleViewModel>();
            CreateMap<HekmatPaymentGatewayTransactionBulkRequestDetail, HekmatPaymentGatewayTransactionBulkRequestDetailListSimpleViewModel>();


            CreateMap<RefundPaymentTransactionItemRequest, RefundPaymentTransactionItemRequestEditModel>();
            CreateMap<RefundPaymentTransactionItemRequest, RefundPaymentTransactionItemRequestListViewModel>()


                .ForMember(x => x.YashilHekmatTerminalTitle,
                    b => b.MapFrom(c => c.YashilHekmatTerminal.Title))


                .ForMember(x => x.RefundPaymentTransactionItemRequestTypeTitle,
                    b => b.MapFrom(c => c.RefundPaymentTransactionItemRequestTypeNavigation.Title))

                .ForMember(x => x.RefundPaymentTransactionItemRequestStatusTitle,
                    b => b.MapFrom(c => c.RefundPaymentTransactionItemRequestStatusNavigation.Title))

                .ForMember(x => x.AccessLevelTitle,
                    b => b.MapFrom(c => c.AccessLevel.Title))
                ;
            CreateMap<RefundPaymentTransactionItemRequestEditModel, RefundPaymentTransactionItemRequest>();
            CreateMap<RefundPaymentTransactionItemRequest, RefundPaymentTransactionItemRequestSimpleViewModel>();

            CreateMap<YashilHekmatTerminalSupplier, YashilHekmatTerminalSupplierEditModel>();
            CreateMap<YashilHekmatTerminalSupplier, YashilHekmatTerminalSupplierListViewModel>()


                .ForMember(x => x.UserTitle,
                    b => b.MapFrom(c => c.User.UserName))


                .ForMember(x => x.AccessLevelTitle,
                    b => b.MapFrom(c => c.AccessLevel.Title))
                ;
            CreateMap<YashilHekmatTerminalSupplierEditModel, YashilHekmatTerminalSupplier>();
            CreateMap<YashilHekmatTerminalSupplier, YashilHekmatTerminalSupplierSimpleViewModel>()
                .ForMember(x => x.Title, b => b.MapFrom(c => c.SupplierTitle));

            CreateMap<RefundPaymentTransactionItemRequest, PaymentTransactionItemRefundRequestListForModifyActionViewModel>()
                .ForMember(x=>
                    x.RefundPaymentTransactionItemRequestStatusTitle,b=>b.MapFrom(
                    c=>c.RefundPaymentTransactionItemRequestStatusNavigation.Title
                    ))
                .ForMember(x =>
                    x.RefundPaymentTransactionItemRequestTypeTitle, b => b.MapFrom(
                    c => c.RefundPaymentTransactionItemRequestTypeNavigation.Title
                ));
            

        }
    }
}