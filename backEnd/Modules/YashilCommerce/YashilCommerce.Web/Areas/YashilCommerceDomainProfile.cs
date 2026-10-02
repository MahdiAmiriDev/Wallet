using AutoMapper;
using System;
using System.Linq;
using Yashil.Common.Core.Interfaces;
using Yashil.Common.SharedKernel.Helpers;
using Yashil.Core.CustomDomainModels;
using Yashil.Core.CustomDomainModels.Commerce;
using Yashil.Core.CustomDtos.Representation;
using Yashil.Core.CustomViewModels;
using Yashil.Core.CustomViewModels.ECommerce;
using Yashil.Core.Entities;
using Yashil.Core.Enums;
using YashilCommerce.Core.DTO;
using YashilCommerce.Core.Enums;
using YashilCommerce.Web.Areas.ECommerce.ViewModels;
using YashilCommerce.Web.Areas.ECommerce.ViewModels.Slider;
using YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels;
using YashilJobJourney.Web.Areas.JobJourneyAdminApi.ViewModels;
using YashilLight.Core.Dto;


namespace YashilCommerce.Web.Areas
{
    public class YashilCommerceProfile : Profile, IOrderedMapperProfile
    {
        public int Order => 1;

        public YashilCommerceProfile()
        {
            string currentUserCompanyCodePath = "";
            int? currentUserCompanyId = default;
            bool isTeacher = false;
            bool isAdmin = false;

            CreateMap<AppDocument, UploadedFileListViewModel>()
            .ForMember(x => x.CreateByFullName, b => b.MapFrom(c => string.Concat(c.CreateByNavigation.PersonNavigation.Name, " " + c.CreateByNavigation.PersonNavigation.LastName)));

            CreateMap<ModifiedOrderItem, ModifiedOrderItemEditModel>();
            CreateMap<UnFactoredOrderItemView, UnFactoredOrderItemViewModel>();
            CreateMap<FactorItemsPurchasedFromSuppliersView, FactorItemDetailStatisticViewModel>();

            CreateMap<ModifiedOrderItem, ModifiedOrderItemListViewModel>()
                .ForMember(x => x.OldOrderItemTitle, b => b.MapFrom(c => c.OldOrderItem.Order.Title))
                .ForMember(x => x.NewOrderTitle, b => b.MapFrom(c => c.NewOrderItem.Order.Title))
                .ForMember(x => x.ModifiedOrderItemTypeTitle, b => b.MapFrom(c => c.ModifiedOrderItemTypeNavigation.Title))
                .ForMember(x => x.ModifiedOrderItemStatusTypeTitle, b => b.MapFrom(c => c.ModifiedOrderItemStatusTypeNavigation.Title))
                .ForMember(x => x.AssigneeTitle, b => b.MapFrom(c => c.AssigneeNavigation.UserName));
            CreateMap<ModifiedOrderItem, ModifiedOrderItemListForModifyViewModel>()
              .ForMember(x => x.ModifiedOrderItemStatusTypeTitle, b => b.MapFrom(c => c.ModifiedOrderItemStatusTypeNavigation.Title))
              .ForMember(x => x.LastOrderItemTitle, b => b.MapFrom(c => c.NewOrderItemId.HasValue ? c.NewOrderItem.ProductSalesPlanning.Product.Title : c.OldOrderItem.ProductSalesPlanning.Product.Title))
              .ForMember(x => x.LastOrderItemId, b => b.MapFrom(c => c.NewOrderItemId.HasValue ? c.NewOrderItem.Id : c.OldOrderItem.Id))
              .ForMember(x => x.LastOrderItemCode, b => b.MapFrom(c => c.NewOrderItemId.HasValue ? c.NewOrderItem.Order.Title : c.OldOrderItem.Order.Title))
              .ForMember(x => x.NationalCode, b => b.MapFrom(c => c.NewOrderItemId.HasValue ? c.NewOrderItem.Order.NationalCode : c.OldOrderItem.Order.NationalCode))
              .ForMember(x => x.MobileNumber, b => b.MapFrom(c => c.NewOrderItemId.HasValue ? c.NewOrderItem.Order.User.UserName : c.OldOrderItem.Order.User.UserName))
              .ForMember(x => x.CompanyTitle, b => b.MapFrom(c => c.NewOrderItemId.HasValue ? c.NewOrderItem.ProductSalesPlanning.Product.Company.Title : c.OldOrderItem.ProductSalesPlanning.Product.Company.Title))
              .ForMember(x => x.RepresentationTitle, b => b.MapFrom(c => c.NewOrderItemId.HasValue ? c.NewOrderItem.Representation.Title : c.OldOrderItem.Representation.Title))
              .ForMember(x => x.Quantity, b => b.MapFrom(c => c.NewOrderItemId.HasValue ? c.NewOrderItem.Quantity : c.OldOrderItem.Quantity))
              .ForMember(x => x.AssigneeTitle, b => b.MapFrom(c => c.AssigneeNavigation.UserName))
              .ForMember(x => x.ModifiedOrderItemTypeTitle, b => b.MapFrom(c => c.ModifiedOrderItemTypeNavigation.Title));

            CreateMap<ModifiedOrderItem, ModifiedOrderItemListForModifyActionViewModel>()
                .ForMember(x => x.AllowCancelModification,
                b => b.MapFrom(c => c.ModifiedOrderItemStatusType == (int)ModifiedOrderItemStatusTypeEnum.Completed
                  && (
                  c.ModifiedOrderItemType.Value == (int)ModifiedOrderItemTypeEnum.CancelOrderItem ?
                   c.RefundDetail.Any(rd => rd.RefundType.Value != (int)RefundTypeEnum.Online && !rd.RefundDetailGroupId.HasValue
                                      ||
                                       rd.RefundType.Value == (int)RefundTypeEnum.Online && !rd.RefundDetailForPaymentGroupId.HasValue
                                       ) : c.OldOrderItem.Order.OrderStatusType == (int)OrderStatusTypesEnum.IncompletePayment
                  )
                //&& c.ModificationDate > DateTime.Now.AddDays(-7)

                ))
                .ForMember(x => x.PriceDifference,
                    b => b.MapFrom(c =>
                        c.OldOrderItem.Order.Refund.SelectMany(r => r.RefundDetail).Sum(rd => rd.RefundAmount)))
                .ForMember(x => x.OldOrderItemTitle, b => b.MapFrom(c => c.OldOrderItem.ProductSalesPlanning.Product.Title))
              .ForMember(x => x.ModifiedOrderItemStatusTypeTitle, b => b.MapFrom(c => c.ModifiedOrderItemStatusTypeNavigation.Title))
              .ForMember(x => x.LastOrderItemTitle, b => b.MapFrom(c => c.NewOrderItemId.HasValue ? c.NewOrderItem.ProductSalesPlanning.Product.Title : c.OldOrderItem.ProductSalesPlanning.Product.Title))
              .ForMember(x => x.LastOrderItemId, b => b.MapFrom(c => c.NewOrderItemId.HasValue ? c.NewOrderItem.Id : c.OldOrderItem.Id))
              .ForMember(x => x.LastOrderItemCode, b => b.MapFrom(c => c.NewOrderItemId.HasValue ? c.NewOrderItem.Order.Title : c.OldOrderItem.Order.Title))
              .ForMember(x => x.NationalCode, b => b.MapFrom(c => c.NewOrderItemId.HasValue ? c.NewOrderItem.Order.NationalCode : c.OldOrderItem.Order.NationalCode))
              .ForMember(x => x.MobileNumber, b => b.MapFrom(c => c.NewOrderItemId.HasValue ? c.NewOrderItem.Order.User.UserName : c.OldOrderItem.Order.User.UserName))
              .ForMember(x => x.CompanyTitle, b => b.MapFrom(c => c.NewOrderItemId.HasValue ? c.NewOrderItem.ProductSalesPlanning.Product.Company.Title : c.OldOrderItem.ProductSalesPlanning.Product.Company.Title))
              .ForMember(x => x.RepresentationTitle, b => b.MapFrom(c => c.NewOrderItemId.HasValue ? c.NewOrderItem.Representation.Title : c.OldOrderItem.Representation.Title))
              .ForMember(x => x.Quantity, b => b.MapFrom(c => c.NewOrderItemId.HasValue ? c.NewOrderItem.Quantity : c.OldOrderItem.Quantity))
              .ForMember(x => x.AssigneeTitle, b => b.MapFrom(c => c.AssigneeNavigation.UserName))
              .ForMember(x => x.ModifiedOrderItemTypeTitle, b => b.MapFrom(c => c.ModifiedOrderItemTypeNavigation.Title))
                .ForMember(x => x.OrderId, b =>
                    b.MapFrom(c => c.OldOrderItem.OrderId));

            CreateMap<ModifiedOrderItemEditModel, ModifiedOrderItem>();

            CreateMap<ModifiedOrderItem, ModifiedOrderItemSimpleViewModel>();

            CreateMap<Company, CompanyViewModel>()
                .ForMember(x => x.ModificationDate, b => b.MapFrom(c => c.ModificationDate.Value.ToString("yyyyMMddhhmmss")))
                ;

            CreateMap<City, CityViewModel>();

            CreateMap<RecipientEditModel, Recipient>()
                .ForMember(x => x.Title,
                    b => b.MapFrom(c => c.NationalCode));

            CreateMap<Recipient, RecipientEditModel>();

            CreateMap<RecipientPresentationEditModel, Recipient>()
                .ForMember(x => x.AddressId,
                    b => b.MapFrom(c => c.AddressId))
                .ForMember(x => x.Address, b => b.MapFrom(c => c.Address))
                .ForMember(x => x.Title, b => b.MapFrom(c => c.NationalCode));

            CreateMap<RecipientPresentationEditModel, Address>()
                .ForMember(x => x.Id,
                    b => b.MapFrom(c => c.AddressId))
                .ForMember(x => x.FullAddress,
                    b => b.MapFrom(c => c.FullAddress))
                .ForMember(x => x.CityId,
                    b => b.MapFrom(c => c.CityId));

            CreateMap<Recipient, OrderAddressViewModel>()
                .ForMember(x => x.CityTitle, b => b.MapFrom(c => c.Address.City.Title))
                .ForMember(x => x.SubsidyStatusTypeTitle, b => b.MapFrom(c => c.SubsidyStatusTypeNavigation.Title))
                .ForMember(x => x.FullAddress, b => b.MapFrom(c => c.Address.FullAddress))
                .ForMember(x => x.IsEncrypted,
                    b => b.MapFrom(c => c.Address.IsEncrypted))
                ;

            CreateMap<Recipient, RecipientPresentationEditModel>()
                .ForMember(x => x.FullAddress,
                    b => b.MapFrom(c => c.Address.FullAddress))
                .ForMember(x => x.AddressId,
                    b => b.MapFrom(c => c.AddressId))
                .ForMember(x => x.IsEncrypted,
                    b => b.MapFrom(c => c.Address.IsEncrypted))
                .ForMember(x => x.CityId,
                    b => b.MapFrom(c => c.Address.CityId));

            CreateMap<Recipient, RecipientSimpleViewModel>();

            CreateMap<Recipient, RecipientListViewModel>()
                .ForMember(x => x.MembershipTypeTitle,
                    b => b.MapFrom(c => c.MembershipTypeNavigation.Title))
                .ForMember(x => x.FullAddress,
                    b => b.MapFrom(c => c.Address.FullAddress))
                .ForMember(x => x.IsEncryptedAddress,
                    b => b.MapFrom(c => c.Address.IsEncrypted))
                .ForMember(x => x.CityTitle,
                    b => b.MapFrom(c => c.Address.City.Title))
                .ForMember(x => x.CityId,
                    b => b.MapFrom(c => c.Address.City.Id));


            CreateMap<Representation, RepresentationEditModel>();

            CreateMap<Representation, RepresentationListViewModel>()
                .ForMember(x => x.Telephone, b => b.MapFrom(c => c.Address.Telephone))
                .ForMember(x => x.CityTitle, b => b.MapFrom(c => c.Address.City.Title))
                .ForMember(x => x.StateTitle, b => b.MapFrom(c => c.Address.City.Parent.Title))
                .ForMember(x => x.CompanyTitle, b => b.MapFrom(c => c.Company.Title))
                .ForMember(x => x.CompanyId, b => b.MapFrom(c => c.Company.Id))
                .ForMember(x => x.FounderFullName, b => b.MapFrom(c => string.Concat(c.Founder.Name, " " + c.Founder.LastName)))
                .ForMember(x => x.LicenseTypeTitle, b => b.MapFrom(c => c.LicenseTypeNavigation.Title))
                .ForMember(x => x.OwnershipTypeTitle, b => b.MapFrom(c => c.OwnershipTypeNavigation.Title))
                .ForMember(x => x.AccessLevelTitle, b => b.MapFrom(c => c.AccessLevel.Title))
                .ForMember(x => x.DisplayOrder, b => b.MapFrom(c => c.DisplayOrder));


            CreateMap<Representation, RepresentationSearchViewModel>()
                .ForMember(x => x.Title, b => b.MapFrom(c => c.Title))
                .ForMember(x => x.CityTitle, b => b.MapFrom(c => c.Address.City.Title))
                .ForMember(x => x.Telephone, b => b.MapFrom(c => c.Address.Telephone))
                .ForMember(x => x.FullAddress, b => b.MapFrom(c => c.Address.FullAddress));


            CreateMap<RepresentationEditModel, Representation>();


            CreateMap<Representation, RepresentationSimpleViewModel>();


            CreateMap<RepresentationPerson, RepresentationPersonEditModel>();

            CreateMap<RepresentationPerson, RepresentationPersonListViewModel>()
                .ForMember(x => x.RepresentationTitle,
                    b => b.MapFrom(c => c.Representation.Title))
                .ForMember(x => x.PersonTitle,
                    b => b.MapFrom(c => c.Person.Name))
                .ForMember(x => x.PostTitle,
                    b => b.MapFrom(c => c.Post.Title))
                .ForMember(x => x.AccessLevelTitle,
                    b => b.MapFrom(c => c.AccessLevel.Title))
                ;
            CreateMap<RepresentationPersonEditModel, RepresentationPerson>();

            CreateMap<RepresentationPerson, RepresentationPersonSimpleViewModel>();


            CreateMap<AdditionalUserProp, AdditionalUserPropEditModel>();
            CreateMap<AdditionalUserProp, CurrentUserAdditionalPropViewModel>()
                .ForMember(x => x.CategoryId, b => b.MapFrom(c => c.RepresentationCategory.CategoryId));

            CreateMap<AdditionalUserProp, AdditionalUserPropInfoViewModel>()
                .ForMember(x => x.CompanyTitle,
                    b => b.MapFrom(c => c.Company.Title))
                .ForMember(x => x.RepresentationTitle,
                    b => b.MapFrom(c => c.Representation.Title))
                .ForMember(x => x.RepresentationCategoryTitle,
                    b => b.MapFrom(c => c.RepresentationCategory.Category.Title))
                .ForMember(x => x.ApplicationTitle,
                    b => b.MapFrom(c => c.Application.Title))
                .ForMember(x => x.OrganizationTitle,
                    b => b.MapFrom(c => c.CreatorOrganization.Title))
                .ForMember(x => x.UserId,
                    b => b.MapFrom(c => c.UserId))
                .ForMember(x => x.CompanyId,
                    b => b.MapFrom(c => c.CompanyId))
                .ForMember(x => x.RepresentationId,
                    b => b.MapFrom(c => c.RepresentationId))
                .ForMember(x => x.RepresentationCategoryId,
                    b => b.MapFrom(c => c.RepresentationCategoryId))
                .ForMember(x => x.UserName,
                    b => b.MapFrom(c => c.User.UserName))
                .ForMember(x => x.FullName,
                    b => b.MapFrom(c => string.Concat(c.User.PersonNavigation.Name, c.User.PersonNavigation.LastName)))
                .ForMember(x => x.OrganizationId,
                    b => b.MapFrom(c => c.CreatorOrganizationId))
                ;

            CreateMap<AdditionalUserProp, AdditionalUserPropListViewModel>()
                .ForMember(x => x.UserTitle,
                    b => b.MapFrom(c => c.User.UserName))
                .ForMember(x => x.CompanyTitle,
                    b => b.MapFrom(c => c.Company.Title))
                .ForMember(x => x.RepresentationTitle,
                    b => b.MapFrom(c => c.Representation.Title));

            CreateMap<AdditionalUserProp, AdditionalUserPropCustomListViewModel>()
                .ForMember(x => x.UserTitle,
                    b =>
                        b.MapFrom(c => string.Concat(c.User.PersonNavigation.Name, "" +
                            c.User.PersonNavigation.LastName + "" + c.User.UserName)))
                .ForMember(x => x.CompanyTitle,
                    b =>
                        b.MapFrom(c => c.CompanyId.HasValue ? c.Company.Title : String.Empty))
                .ForMember(x => x.RepresentationTitle,
                    b =>
                        b.MapFrom(c => c.RepresentationId.HasValue ? c.Representation.Title : String.Empty));


            CreateMap<AdditionalUserPropEditModel, AdditionalUserProp>();

            CreateMap<AdditionalUserProp, AdditionalUserPropSimpleViewModel>();

            CreateMap<AdditionalUserPropCompanyGroup, AdditionalUserPropCompanyGroupEditModel>();
            CreateMap<AdditionalUserPropCompanyGroup, AdditionalUserPropCompanyGroupListViewModel>()
                    .ForMember(x => x.AccessLevelTitle,
                    b => b.MapFrom(c => c.AccessLevel.Title))
                    .ForMember(x => x.CompanyGroupTitle,
                    b => b.MapFrom(c => c.CompanyGroup.Title))
                    .ForMember(c => c.AdditionalUserPropTitle,
                    b => b.MapFrom(c => string.Concat(c.AdditionalUserProp.User.PersonNavigation.Name, " ",
                    c.AdditionalUserProp.User.PersonNavigation.LastName)));

            CreateMap<AdditionalUserPropCompanyGroupEditModel, AdditionalUserPropCompanyGroup>();
            CreateMap<AdditionalUserPropCompanyGroup, AdditionalUserPropCompanyGroupSimpleViewModel>();


            CreateMap<RepresentationEstablishedLicenseType, RepresentationEstablishedLicenseTypeEditModel>();

            CreateMap<RepresentationEstablishedLicenseType, RepresentationEstablishedLicenseTypeListViewModel>()
                .ForMember(x => x.RepresentationTitle,
                    b => b.MapFrom(c => c.Representation.Title))
                .ForMember(x => x.EstablishedLicenseTypeTitle,
                    b => b.MapFrom(c => c.EstablishedLicenseTypeNavigation.Title)).ForMember(x => x.AccessLevelTitle,
                    b => b.MapFrom(c => c.AccessLevel.Title))
                ;
            CreateMap<RepresentationEstablishedLicenseTypeEditModel, RepresentationEstablishedLicenseType>();

            CreateMap<RepresentationEstablishedLicenseType, RepresentationEstablishedLicenseTypeSimpleViewModel>();


            CreateMap<PersonBankAccount, PersonBankAccountEditModel>()
                .ForMember(x => x.Name, b => b.MapFrom(c => c.Person.Name))
                .ForMember(x => x.LastName, b => b.MapFrom(c => c.Person.LastName))
                .ForMember(x => x.NationalCode, b => b.MapFrom(c => c.Person.NationalCode))
                .ForMember(x => x.MobileNumber, b => b.MapFrom(c => c.Person.MobileNumber))
                ;

            CreateMap<PersonBankAccount, PersonBankAccountListViewModel>()

                .ForMember(x => x.PersonTitle, b => b.MapFrom(c => string.Concat(c.Person.Name, " " + c.Person.LastName)))
                .ForMember(x => x.NationalCode, b => b.MapFrom(c => c.Person.NationalCode))
                .ForMember(x => x.MobileNumber, b => b.MapFrom(c => c.Person.MobileNumber))
                .ForMember(x => x.BankTypeTitle, b => b.MapFrom(c => c.BankTypeNavigation.Title));

            CreateMap<PersonBankAccountEditModel, PersonBankAccount>();

            CreateMap<PersonBankAccount, PersonBankAccountSimpleViewModel>()
                .ForMember(x => x.FullName,
                    b => b.MapFrom(c => string.Concat(c.Person.Name, " " + c.Person.LastName)))
                .ForMember(x => x.AccountNumber,
                    b => b.MapFrom(c => c.AccountNumber))
                .ForMember(x => x.BankTypeTitle,
                    b => b.MapFrom(c => c.BankTypeNavigation.Title));


            CreateMap<Contract, ContractEditModel>();

            CreateMap<Contract, ContractListViewModel>()
                .ForMember(x => x.ContractStatusTitle, b => b.MapFrom(c => c.ContractStatusNavigation.Title))
                .ForMember(x => x.FounderMobileNumber, b => b.MapFrom(c => c.SaleConfiguration.Founder.Person.MobileNumber))
                .ForMember(x => x.FounderPhoneNumber, b => b.MapFrom(c => c.SaleConfiguration.Founder.Person.PhoneNumber))
                .ForMember(x => x.IsActive, b => b.MapFrom(c => c.SaleConfiguration.IsActive))
                .ForMember(x => x.CompanyTitle, b => b.MapFrom(c => c.SaleConfiguration.Company.Title))
                .ForMember(x => x.RepresentationTitle, b => b.MapFrom(c => c.SaleConfiguration.Representation.Title))
                .ForMember(x => x.LicenseExpireDate, b => b.MapFrom(c => c.SaleConfiguration.LicenseExpireDate))
                .ForMember(x => x.FromDate, b => b.MapFrom(c => c.SaleConfiguration.FromDate))
                .ForMember(x => x.ToDate, b => b.MapFrom(c => c.SaleConfiguration.ToDate));

            CreateMap<ContractEditModel, Contract>();

            CreateMap<Contract, ContractSimpleViewModel>();


            CreateMap<ProductTemplate, ProductTemplateEditModel>();

            CreateMap<ProductTemplate, ProductTemplateListViewModel>()
                .ForMember(x => x.AccessLevelTitle,
                    b => b.MapFrom(c => c.AccessLevel.Title));
            CreateMap<ProductTemplateEditModel, ProductTemplate>();

            CreateMap<ProductTemplate, ProductTemplateSimpleViewModel>();


            CreateMap<GiftCardType, GiftCardTypeEditModel>();

            CreateMap<GiftCardType, GiftCardTypeListViewModel>()
                .ForMember(x => x.AccessLevelTitle,
                    b => b.MapFrom(c => c.AccessLevel.Title));
            CreateMap<GiftCardTypeEditModel, GiftCardType>();

            CreateMap<GiftCardType, GiftCardTypeSimpleViewModel>();


            CreateMap<RepresentationCategory, RepresentationCategoryEditModel>();

            CreateMap<RepresentationCategory, RepresentationCategoryListViewModel>()
                .ForMember(x => x.RepresentationTitle,
                    b => b.MapFrom(c => c.Representation.Title))
                .ForMember(x => x.CategoryTitle,
                    b => b.MapFrom(c => c.Category.Title))
                .ForMember(x => x.AccessLevelTitle,
                    b => b.MapFrom(c => c.AccessLevel.Title));
            CreateMap<RepresentationCategoryEditModel, RepresentationCategory>();

            CreateMap<RepresentationCategory, RepresentationCategorySimpleViewModel>()
                .ForMember(x => x.Title,
                    b => b.MapFrom(c => c.Category.Title));

            CreateMap<RepresentationCategory, CategoryRepresentationListViewModel>()
                .ForMember(x => x.CategoryTitle,
                    b => b.MapFrom(c => c.Category.Title));


            CreateMap<Product, ProductEditModel>()
                .ForMember(x => x.Course, b => b.MapFrom(c => c.Course))
                .ForMember(x => x.SalesByAllRepresentations, b => b.MapFrom(c => c.SalesModelType == 1085))
                .ForMember(x => x.SalesBySelectedRepresentations, b => b.MapFrom(c => c.SalesModelType == 1086))
                .ForMember(x => x.AllowSelectByRepresentation, b => b.MapFrom(c => c.SalesModelType == 1087))
                .ForMember(x => x.Categories, b => b.MapFrom(c => c.ProductCategory.Select(x => x.CategoryId)))
                ;

            CreateMap<Product, ProductSimpleListViewModel>()
                .ForMember(x => x.CompanyTitle, b => b.MapFrom(c => c.Company.Title))
                .ForMember(x => x.CategoryTitle,
                    b => b.MapFrom(c => c.ProductCategory.FirstOrDefault().Category.Title));
            CreateMap<Product, ProductWithCourseSimpleListViewModel>()
              .ForMember(x => x.CompanyTitle, b => b.MapFrom(c => c.Company.Title))
              .ForMember(x => x.Duration, b => b.MapFrom(c => c.Duration))
              .ForMember(x => x.CategoryTitle, b => b.MapFrom(c => c.ProductCategory.FirstOrDefault().Category.Title));

            CreateMap<Product, ProductListViewModel>()
                .ForMember(x => x.ProductTypeTitle,
                    b => b.MapFrom(c => c.ProductTypeNavigation.Title))
                .ForMember(x => x.CategoryTitle, b => b.MapFrom(c => c.ProductCategory.FirstOrDefault().Category.Title))
                .ForMember(x => x.Duration, b => b.MapFrom(c => c.Duration))
                .ForMember(x => x.CompanyTitle,
                    b => b.MapFrom(c => c.Company.Title))
                .ForMember(x => x.AppEntityTitle,
                    b => b.MapFrom(c => c.AppEntity.Title))
                .ForMember(x => x.ProductTemplateTitle,
                    b => b.MapFrom(c => c.ProductTemplate.Title))
                .ForMember(x => x.GiftCardTypeTitle,
                    b => b.MapFrom(c => c.GiftCardType.Title))
                .ForMember(x => x.DownloadActivationTypeTitle,
                    b => b.MapFrom(c => c.DownloadActivationTypeNavigation.Title))
                .ForMember(x => x.RecurringCyclePeriodTypeTitle,
                    b => b.MapFrom(c => c.RecurringCyclePeriodTypeNavigation.Title))
                .ForMember(x => x.TaxCategoryTitle, b => b.MapFrom(c => c.TaxCategory.Title))
                .ForMember(x => x.BasePriceUnitTypeTitle, b => b.MapFrom(c => c.BasePriceUnitTypeNavigation.Title))
                .ForMember(x => x.BasePriceBaseUnitTypeTitle,
                    b => b.MapFrom(c => c.BasePriceBaseUnitTypeNavigation.Title)).ForMember(x => x.AccessLevelTitle,
                    b => b.MapFrom(c => c.AccessLevel.Title));
            CreateMap<ProductEditModel, Product>();

            CreateMap<Product, ProductSimpleViewModel>();
            CreateMap<Product, ProductInfoViewModel>()
                .ForMember(x => x.CategoryTitle, b => b.MapFrom(c => c.DefaultCategory.Title))
                .ForMember(x => x.Categories, b => b.MapFrom(c => c.DefaultCategoryId))
                .ForMember(x => x.CertificateType, b => b.MapFrom(c => c.Course.CertificateType))
                .ForMember(x => x.CertificateTypeTitle, b => b.MapFrom(c => c.Course.CertificateTypeNavigation.Title))
                .ForMember(x => x.EvaluationMethod, b => b.MapFrom(c => c.Course.EvaluationMethod))
                .ForMember(x => x.EvaluationMethodTitle, b => b.MapFrom(c => c.Course.EvaluationMethodNavigation.Title))
                .ForMember(x => x.SkillType, b => b.MapFrom(c => c.Course.SkillType))
                .ForMember(x => x.SkillTypeTitle, b => b.MapFrom(c => c.Course.SkillTypeNavigation.Title))
                ;


            CreateMap<Order, OrderEditModel>();

            CreateMap<Order, OrderSimplePresentationViewModel>()
                .ForMember(x => x.OrderStatusTypeTitle,
                    b => b.MapFrom(c => c.OrderStatusTypeNavigation.Title))
                .ForMember(x => x.SubsidyStatusTypeTitle,
                    b => b.MapFrom(c => c.SubsidyStatusTypeNavigation.Title))
                .ForMember(x => x.IsDownloadAbleProduct,
                    b =>
                        b.MapFrom(c =>
                            c.OrderItem.Where(oi => oi.OrderId == c.Id)
                                .Select(oi => oi.ProductSalesPlanning.Product.IsDownload).FirstOrDefault()))
                .ForMember(x => x.ProductSalesPlanningDownloadUrl, b => b.MapFrom(c => c.OrderItem
                    .Where(oi => oi.OrderId == c.Id)
                    .Select(oi => oi.ProductSalesPlanning.DownloadUrl).FirstOrDefault()));

            CreateMap<Order, OrderAddressViewModel>()
                .ForMember(x => x.CityTitle, b => b.MapFrom(c => c.ShippingAddress.City.Title))
                .ForMember(x => x.FullAddress, b => b.MapFrom(c => c.ShippingAddress.FullAddress))
                .ForMember(x => x.IsEncrypted, b => b.MapFrom(c => c.ShippingAddress.IsEncrypted))
                ;
            CreateMap<Order, OrderDetailPresentationViewModel>()
                .ForMember(x => x.OrderStatusTypeTitle,
                    b => b.MapFrom(c => c.OrderStatusTypeNavigation.Title))
                .ForMember(x => x.ShippingStatusTypeTitle,
                    b => b.MapFrom(c => c.ShippingStatusTypeNavigation.Title))
                .ForMember(x => x.SubsidyStatusTypeTitle,
                    b => b.MapFrom(c => c.SubsidyStatusTypeNavigation.Title))
                .ForMember(x => x.SubsidyTypeTitle,
                    b => b.MapFrom(c => c.SubsidyTypeNavigation.Title))
                .ForMember(x => x.NationalCode, b => b
                    .MapFrom(c => c.NationalCode))
                .ForMember(x => x.CardNumber, b => b
                    .MapFrom(c => c.CardNumber))
                .ForMember(x => x.MembershipType, b => b
                    .MapFrom(c => c.MembershipType))
                .ForMember(x => x.addressListViewModel, b => b
                    .MapFrom(c => c.ShippingAddress));


            CreateMap<Order, OrderListViewModel>()
                .ForMember(x => x.UserTitle, b => b.MapFrom(c => c.User.UserName))
                .ForMember(x => x.OrderStatusTypeTitle, b => b.MapFrom(c => c.OrderStatusTypeNavigation.Title))
                .ForMember(x => x.SubsidyTitle, b => b.MapFrom(c => c.Subsidy.Title))
                .ForMember(x => x.PaymentMethods, b => b.MapFrom(c => string.Join(",", c.Payment.Select(p => p.PaymentMethodTypeNavigation.Title))))
                .ForMember(x => x.SubsidyStatusTypeTitle, b => b.MapFrom(c => c.SubsidyStatusTypeNavigation.Title))
                .ForMember(x => x.SubsidyTypeTitle, b => b.MapFrom(c => c.SubsidyTypeNavigation.Title))
                .ForMember(x => x.RelationshipWithBuyerTitle,
                 b => b.MapFrom(c => c.RecipientRelationshipWithBuyer.Title))
                .ForMember(x => x.CityTitle, b => b.MapFrom(c => c.City.Title))
                .ForMember(x => x.MembershipTypeTitle,
                b => b.MapFrom(c => (
                c.SubsidyType.HasValue
                ? c.SubsidyTypeNavigation.Title
                : c.MembershipType == (int)MembershipTypesEnum.Vezarat
                ? MembershipType.Vezarat
                : MembershipType.Sata)
                )).ForMember(x => x.GenderTitle, b => b.MapFrom(c => c.RecipientGenderTypeNavigation.Title));

            CreateMap<Order, OrderStatisticViewModel>()
                .ForMember(x => x.UserTitle, b => b.MapFrom(c => c.User.UserName))
                .ForMember(x => x.OrderStatusTypeTitle, b => b.MapFrom(c => c.OrderStatusTypeNavigation.Title))
                .ForMember(x => x.SubsidyTitle,
                    b => b.MapFrom(c => c.Subsidy.Title))
                .ForMember(x => x.SubsidyStatusTypeTitle, b => b.MapFrom(c => c.SubsidyStatusTypeNavigation.Title))
                .ForMember(x => x.SubsidyTypeTitle, b => b.MapFrom(c => c.SubsidyTypeNavigation.Title));



            CreateMap<OrderEditModel, Order>();

            CreateMap<Order, OrderSimpleViewModel>();


            CreateMap<OrderItem, OrderItemEditModel>();


            CreateMap<OrderItem, OrderItemDetailPresentationViewModel>()
                .ForMember(x => x.ProductTemplateId,
                    b => b.MapFrom(c =>
                        c.ProductSalesPlanning.Product.ProductTemplateId))
                .ForMember(x => x.ProductTemplateTitle,
                    b => b.MapFrom(c =>
                        c.ProductSalesPlanning.Product.ProductTemplate.Title))
                .ForMember(x => x.ProductSalePlaningSubTitleFormat, b => b.MapFrom(c => c.ProductSalesPlanning.SubTitleFormat))
                .ForMember(x => x.CompanySubTitleFormat, b => b.MapFrom(c => c.Representation.Company.SubTitleFormat))
                .ForMember(x => x.EndDate, b => b.MapFrom(c => c.ProductSalesPlanning.EndDate))
                .ForMember(x => x.StartDate, b => b.MapFrom(c => c.ProductSalesPlanning.StartDate))
                .ForMember(x => x.CoursePlanningSchedule, b => b.MapFrom(c => c.ProductSalesPlanning.CoursePlanningSchedule))
                .ForMember(x => x.OrderCode, b => b.MapFrom(c => c.Order.Code))
                .ForMember(x => x.ProductCode, b => b.MapFrom(c => c.ProductSalesPlanning.Product.Code))
                .ForMember(x => x.RepresentationTitle, b => b.MapFrom(c => c.Representation.Title))
                .ForMember(x => x.CompanyTitle, b => b.MapFrom(c => c.ProductSalesPlanning.Product.Company.Title))
                .ForMember(x => x.Title, b => b.MapFrom(c => c.ProductSalesPlanning.Product.Title))
                .ForMember(x => x.CityTitle, b => b.MapFrom(c => c.Representation.Address.City.Title))
                 .ForMember(x => x.StateTitle, b => b.MapFrom(c => c.Representation.Address.City.Parent.Title))
                 .ForMember(x => x.RepresentationAddress, b => b.MapFrom(c => c.Representation.Address.FullAddress))
                 .ForMember(x => x.RepresentationAddressIsEncrypted, b => b.MapFrom(c => c.Representation.Address.IsEncrypted))
                .ForMember(x => x.RepresentationTelephone, b => b.MapFrom(c => c.Representation.Address.Telephone))
                .ForMember(x => x.CompanyDescription, b => b.MapFrom(c => c.Representation.Company.Description))
                .ForMember(x => x.ModificationDate, b => b.MapFrom(c => c.ProductSalesPlanning.ModificationDate.Value.ToString("yyyyMMddhhmmss")));


            //CreateMap<OrderItem, OrderItemChildDetailPresentationViewModel>()
            //    .ForMember(x => x.Id, b =>
            //        b.MapFrom(c => c.InverseParent.Select(d => d.Id)))
            //    .ForMember(x => x.Title, b =>
            //        b.MapFrom(c =>
            //            c.InverseParent.Select(d => d.ProductSalesPlanning.Product.Title)))
            //    .ForMember(x => x.Price, b =>
            //        b.MapFrom(c => c.InverseParent.Select(d => d.Price)))
            //    .ForMember(x => x.PriceIncDiscount, b =>
            //        b.MapFrom(c => c.InverseParent.Select(d => d.PriceIncDiscount)));

            CreateMap<OrderItem, OrderItemSimplePresentationViewModel>()
                .ForMember(x => x.OrderCode, b => b.MapFrom(c => c.Order.Code))
                .ForMember(x => x.VoucherString, b => b.MapFrom(c => c.Vocher.Code))
                .ForMember(x => x.VoucherTemplateId,
                    b =>
                        b.MapFrom(c => c.Vocher.VocherType.VoucherTemplateId))
                .ForMember(x => x.VoucherUrlPrefix,
                    b =>
                        b.MapFrom(c => c.Vocher.VocherType.Company.VoucherUrlPrefix))
                .ForMember(x => x.ProductCode, b => b.MapFrom(c => c.ProductSalesPlanning.Product.Code))
                .ForMember(x => x.RepresentationTitle, b => b.MapFrom(c => c.Representation.Title))
                .ForMember(x => x.ProductTitle, b => b.MapFrom(c => c.ProductSalesPlanning.Product.Title))
                .ForMember(x => x.CompanyApiLocalKey,
                    b => b.MapFrom(c => c.ProductSalesPlanning.Product.Company.ApiLocalKey))
                .ForMember(x => x.CompanyApiSupported,
                    b => b.MapFrom(c =>
                        c.ProductSalesPlanning.Product.Company.ApiSupported.HasValue &&
                        c.ProductSalesPlanning.Product.Company.ApiSupported.Value))
                .ForMember(x => x.NeedRedirectToCompanySiteForReceive,
                    b =>
                        b.MapFrom(c => !string.IsNullOrEmpty(c.ProductSalesPlanning.Product.Company.ApiLocalKey) &&
                                       c.ProductSalesPlanning.Product.NeedRedirectToCompanySiteForReceive.HasValue &&
                                       c.ProductSalesPlanning.Product.NeedRedirectToCompanySiteForReceive.Value
                                       ));

            CreateMap<OrderItem, OrderItemListViewModel>()
                .ForMember(x => x.RecipientMobileNumber, b => b.MapFrom(c => c.Order.RecipientMobileNumber))
                .ForMember(x => x.CompanyTitle, b => b.MapFrom(c => c.Representation.Company.Title))
                .ForMember(x => x.SubsidyStatusType, b => b.MapFrom(c => c.Order.SubsidyStatusTypeNavigation.Title))
                .ForMember(x => x.SubsidyType, b => b.MapFrom(c => c.Order.SubsidyTypeNavigation.Title))
                .ForMember(x => x.CardNumber, b => b.MapFrom(c => c.Order.CardNumber))
                .ForMember(x => x.OrderId, b => b.MapFrom(c => c.Order.Id))
                .ForMember(x => x.CreationDateInt, b => b.MapFrom(c => c.Order.CreationDateInt))
                .ForMember(x => x.OrderCode, b => b.MapFrom(c => c.Order.Code))
                .ForMember(x => x.ProductCode, b => b.MapFrom(c => c.ProductSalesPlanning.Product.Code))
                .ForMember(x => x.RepresentationCode, b => b.MapFrom(c => c.Representation.Code))
                .ForMember(x => x.RepresentationTitle, b => b.MapFrom(c => c.Representation.Title))
                .ForMember(x => x.NationalCode, b => b.MapFrom(c => c.Order.NationalCode))
                .ForMember(x => x.MobileNumber, b => b.MapFrom(c => c.Order.User.UserName))
                .ForMember(x => x.ProductTitle, b => b.MapFrom(c => c.ProductSalesPlanning.Product.Title))
                .ForMember(x => x.OrderModificationDate, b => b.MapFrom(c => c.Order.ModificationDate))
                .ForMember(x => x.OrderNumber, b => b.MapFrom(c => c.Order.Title))
                .ForMember(x => x.OrderStatus, b => b.MapFrom(c => c.Order.OrderStatusTypeNavigation.Title))
                .ForMember(x => x.OrderStatusType, b => b.MapFrom(c => c.Order.OrderStatusType))
                .ForMember(x => x.Price, b => b.MapFrom(c => c.Price / c.Quantity))
                .ForMember(x => x.Total,
                    b => b.MapFrom(c =>
                        (long)(
                            c.Order.SubsidyType != null &&
                            c.Order.SubsidyStatusType == (int)SubsidyStatusTypesEnum.Confirmed
                                ? c.TotalInclSubsidy
                                : c.TotalExclSubsidy)))
                .ForMember(x => x.TaxPrice,
                    b => b.MapFrom(c =>
                        (long?)(
                            c.Order.SubsidyType != null &&
                            c.Order.SubsidyStatusType == (int)SubsidyStatusTypesEnum.Confirmed
                                ? c.TaxPriceInclSubsidy
                                : c.TaxPriceExclSubsidy)));

            CreateMap<OrderItem, OrderItemFullInfoListViewModel>()
                .ForMember(x => x.RecipientMobileNumber, b => b.MapFrom(c => c.Order.RecipientMobileNumber))
                .ForMember(x => x.CompanyTitle, b => b.MapFrom(c => c.Representation.Company.Title))
                .ForMember(x => x.ProductSalesPlanningId, b => b.MapFrom(c => c.ProductSalesPlanningId))
                .ForMember(x => x.CoursePlanningSchedule,
                    b => b.MapFrom(c => c.ProductSalesPlanning.CoursePlanningSchedule))
                .ForMember(x => x.OrderId, b => b.MapFrom(c => c.Order.Id))
                .ForMember(x => x.CreationDateInt, b => b.MapFrom(c => c.Order.CreationDateInt))
                .ForMember(x => x.ProductCode, b => b.MapFrom(c => c.ProductSalesPlanning.Product.Code))
                .ForMember(x => x.RepresentationCode, b => b.MapFrom(c => c.Representation.Code))
                .ForMember(x => x.RepresentationTitle, b => b.MapFrom(c => c.Representation.Title))
                .ForMember(x => x.NationalCode, b => b.MapFrom(c => c.Order.NationalCode))
                .ForMember(x => x.ProductTitle, b => b.MapFrom(c => c.ProductSalesPlanning.Product.Title))
                .ForMember(x => x.ProductGroup, b => b.MapFrom(c => c.ProductSalesPlanning.Product.ProductGroup.Title))
                .ForMember(x => x.OrderModificationDate, b => b.MapFrom(c => c.Order.ModificationDate))
                .ForMember(x => x.OrderNumber, b => b.MapFrom(c => c.Order.Title))
                .ForMember(x => x.OrderStatus, b => b.MapFrom(c => c.Order.OrderStatusTypeNavigation.Title))
                .ForMember(x => x.OrderStatusType, b => b.MapFrom(c => c.Order.OrderStatusType))
                .ForMember(x => x.Price, b => b.MapFrom(c => c.Price / c.Quantity));

            CreateMap<OrderItem, OrderItemRefundedViewModel>()
                .ForMember(x => x.Id, b => b.MapFrom(c => c.FactorItem.Factor.Id))
                .ForMember(x => x.IsConverted, b =>
                    b.MapFrom(c => c.FactorItem.Factor.FactorGroup != null))
                .ForMember(x => x.SubsidyPrice, b =>
                    b.MapFrom(c => c.SubsidyPrice))
                .ForMember(x => x.SubsidyPriceIncProfit, b =>
                    b.MapFrom(c => c.SubsidyPriceIncProfit))
                .ForMember(x => x.SubsidyPriceIncTaxIncProfit, b =>
                    b.MapFrom(c => c.SubsidyPriceIncTaxIncProfit))
                .ForMember(x => x.CardNumber, b =>
                    b.MapFrom(c => c.Order.CardNumber))
                .ForMember(x => x.NationalCode, b =>
                    b.MapFrom(c => c.Order.NationalCode))
                .ForMember(x => x.PaidDateInt, b =>
                    b.MapFrom(c => c.Order.Payment.Where(p => p.OrderId == c.OrderId)
                   .Select(x => x.PaidDateInt).FirstOrDefault()))
                .ForMember(x => x.RefundDateInt, b =>
                    b.MapFrom(c => c.Order.Payment.Where(p => p.OrderId == c.OrderId)
                   .SelectMany(p => p.RefundDetail
                   .Where(r => r.PaymentId == p.Id))
                   .Select(r => r.CreationDateInt).FirstOrDefault()))
                .ForMember(x => x.SubsidyType, b =>
                b.MapFrom(c => c.Order.SubsidyTypeNavigation.Title))
                .ForMember(x => x.FactorGroupTitle, b =>
                b.MapFrom(c => c.FactorItem.Factor.FactorGroup.Title));




            CreateMap<OrderItem, OrderItemListDuplicateViewModel>()
              .ForMember(x => x.ProductTitle, b => b.MapFrom(c => c.ProductSalesPlanning.Product.Title))
              .ForMember(x => x.OrderStatusTitle, b => b.MapFrom(c => c.Order.OrderStatusTypeNavigation.Title));

            CreateMap<OrderItem, OrderItemListCustomViewModel>()

                .ForMember(x => x.ProductSalesPlanningId,
                    b => b.MapFrom(c => c.ProductSalesPlanningId))
                .ForMember(x => x.RecipientMobileNumber, b => b.MapFrom(c => c.Order.RecipientMobileNumber))
                .ForMember(x => x.CompanyTitle, b => b.MapFrom(c => c.Representation.Company.Title))
                .ForMember(x => x.CreationDateInt, b => b.MapFrom(c => c.Order.CreationDateInt))
                .ForMember(x => x.ProductCode, b => b.MapFrom(c => c.ProductSalesPlanning.Product.Code))
                .ForMember(x => x.RepresentationCode, b => b.MapFrom(c => c.Representation.Code))
                .ForMember(x => x.RepresentationTitle, b => b.MapFrom(c => c.Representation.Title))
                .ForMember(x => x.NationalCode, b => b.MapFrom(c => c.Order.NationalCode))
                .ForMember(x => x.MobileNumber, b => b.MapFrom(c => c.Order.User.PersonNavigation.MobileNumber))
                .ForMember(x => x.ProductTitle, b => b.MapFrom(c => c.ProductSalesPlanning.Product.Title))
                .ForMember(x => x.OrderNumber, b => b.MapFrom(c => c.Order.Title))
                .ForMember(x => x.OrderStatus, b => b.MapFrom(c => c.Order.OrderStatusTypeNavigation.Title))
                .ForMember(x => x.SubsidyStatusType, b => b.MapFrom(c => c.Order.SubsidyStatusTypeNavigation.Title))
                .ForMember(x => x.OrderModificationDate, b => b.MapFrom(c => c.Order.ModificationDate))
                .ForMember(x => x.OrderStatusType, b => b.MapFrom(c => c.Order.OrderStatusType))
                .ForMember(x => x.RelationshipWithBuyerTitle,
                    b => b.MapFrom(c => c.Order.RecipientRelationshipWithBuyer.Title))
                .ForMember(x => x.CityTitle, b => b.MapFrom(c => c.Order.City.Title));

            CreateMap<OrderItem, OrderItemWithDeletedListViewModel>()
                .ForMember(x => x.Price, b => b.MapFrom(c => c.Price / c.Quantity))
                .ForMember(x => x.ProductSalesPlanningId, b => b.MapFrom(c => c.ProductSalesPlanningId))
                .ForMember(x => x.CoursePlanningSchedule, b => b.MapFrom(c => c.ProductSalesPlanning.CoursePlanningSchedule))
                .ForMember(x => x.Price, b => b.MapFrom(c => c.Price / c.Quantity))
                .ForMember(x => x.CompanyTitle, b => b.MapFrom(c => c.ProductSalesPlanning.SaleConfiguration.Company.Title))
                .ForMember(x => x.ProductCode, b => b.MapFrom(c => c.ProductSalesPlanning.Product.Code))
                .ForMember(x => x.RepresentationCode, b => b.MapFrom(c => c.Representation.Code))
                .ForMember(x => x.RepresentationTitle, b => b.MapFrom(c => c.Representation.Title))
                .ForMember(x => x.ProductTitle, b => b.MapFrom(c => c.ProductSalesPlanning.Product.Title))
                .ForMember(x => x.Total,
                    b => b.MapFrom(c =>
                        (long)(
                            c.Order.SubsidyType != null &&
                            c.Order.SubsidyStatusType == (int)SubsidyStatusTypesEnum.Confirmed
                                ? c.TotalInclSubsidy
                                : c.TotalExclSubsidy)))
                .ForMember(x => x.TaxPrice,
                    b => b.MapFrom(c =>
                        (long?)(
                            c.Order.SubsidyType != null &&
                            c.Order.SubsidyStatusType == (int)SubsidyStatusTypesEnum.Confirmed
                                ? c.TaxPriceInclSubsidy
                                : c.TaxPriceExclSubsidy)))
                .ForMember(x => x.VocherTypeTitle, b => b.MapFrom(
                            c => c.ProductSalesPlanning.Product.VocherType == null ? "ندارد" : c.ProductSalesPlanning.Product.VocherType.Title))
                .ForMember(x => x.VocherTypeId, b => b.MapFrom(
                            c => c.ProductSalesPlanning.Product.VocherType == null ? "ندارد" : c.ProductSalesPlanning.Product.VocherType.Id.ToString()))
                .ForMember(x => x.VocherId, b => b.MapFrom(
                            c => (c.ProductSalesPlanning.Product.VocherType != null && c.Vocher == null) ? "اختصاص داده نشده" :
                                 (c.ProductSalesPlanning.Product.VocherType == null && c.Vocher == null) ? "ندارد" : c.Vocher.Id.ToString()));

            CreateMap<OrderItem, OrderItemCustomListViewModel>()
                .ForMember(x => x.CreationDateInt, b => b.MapFrom(c => c.Order.CreationDateInt))
                .ForMember(x => x.OrderCode, b => b.MapFrom(c => c.Order.Code))
                .ForMember(x => x.ProductCode, b => b.MapFrom(c => c.ProductSalesPlanning.Product.Code))
                .ForMember(x => x.RepresentationCode, b => b.MapFrom(c => c.Representation.Code))
                .ForMember(x => x.RepresentationTitle, b => b.MapFrom(c => c.Representation.Title))
                .ForMember(x => x.MobileNumber, b => b.MapFrom(c => c.Order.User.PersonNavigation.MobileNumber))
                .ForMember(x => x.ProductSalesPlanningTitle, b => b.MapFrom(c => c.ProductSalesPlanning.Product.Title))
                .ForMember(x => x.OrderTitle, b => b.MapFrom(c => c.Order.Title))
                .ForMember(x => x.OrderStatusType, b => b.MapFrom(c => c.Order.OrderStatusType));

            CreateMap<OrderItemEditModel, OrderItem>();

            CreateMap<OrderItem, OrderItemSimpleViewModel>();
            CreateMap<FactorItemsSoldToTheSataAndVezaratAndHekmatView, FactorItemDetailFullViewModel>();
            CreateMap<InvalidFactorItemDomainModel, InvalidFactorItemViewModel>();

            CreateMap<OrderNote, OrderNoteEditModel>();
            CreateMap<FactorItemsSoldToTheSataAndVezaratAndHekmatPaymentsView, FactorItemPaymentFullInfoViewViewModel>();

            CreateMap<OrderNote, OrderNoteListViewModel>()
                .ForMember(x => x.OrderTitle,
                    b => b.MapFrom(c => c.Order.Title))
                .ForMember(x => x.AccessLevelTitle,
                    b => b.MapFrom(c => c.AccessLevel.Title));
            CreateMap<OrderNoteEditModel, OrderNote>();

            CreateMap<OrderNote, OrderNoteSimpleViewModel>();

            CreateMap<ProductSalesPlanning, ProductSalesPlanningInfoListViewModel>()
                .ForMember(x => x.ProductTitle, b => b.MapFrom(psp => psp.Product.Title))
                .ForMember(x => x.RepresentationTitle, b => b.MapFrom(c => c.Representation.Title))
                .ForMember(x => x.CompanyTitle, b => b.MapFrom(psp => psp.Product.Company.Title))
                .ForMember(x => x.Duration, b => b.MapFrom(psp => psp.Duration))
                .ForMember(x => x.CoursePlanningSchedule, b => b.MapFrom(psp => psp.CoursePlanningSchedule))
                .ForMember(x => x.StartDate, b => b.MapFrom(psp => psp.StartDate))
                .ForMember(x => x.EndDate, b => b.MapFrom(psp => psp.EndDate))
                .ForMember(x => x.ImplementationTypeTitle, b => b.MapFrom(psp => psp.ImplementationTypeNavigation.Title))
                .ForMember(x => x.CourseTypeTitle, b => b.MapFrom(psp => psp.CourseTypeNavigation.Title))
                .ForMember(x => x.RunTypeTitle, b => b.MapFrom(psp => psp.RunTypeNavigation.Title))
                .ForMember(x => x.CustomGenderTitle, b => b.MapFrom(psp => psp.CustomGenderNavigation.Title));


            CreateMap<ProductSalesPlanning, ProductSalesPlanningInfoViewModel>()
                .ForMember(x=>x.ProductSalesPlanningTitle , b=>b.MapFrom(psp => psp.Title))
                .ForMember(x => x.ProductTitle, b => b.MapFrom(psp => psp.Product.Title))
                .ForMember(x => x.RepresentationTitle, b => b.MapFrom(c => c.Representation.Title))
                .ForMember(x => x.CompanyTitle, b => b.MapFrom(psp => psp.Product.Company.Title))
                .ForMember(x => x.Duration, b => b.MapFrom(psp => psp.Duration))
                .ForMember(x => x.CoursePlanningSchedule, b => b.MapFrom(psp => psp.CoursePlanningSchedule))
                .ForMember(x => x.StartDate, b => b.MapFrom(psp => psp.StartDate))
                .ForMember(x => x.EndDate, b => b.MapFrom(psp => psp.EndDate))
                .ForMember(x => x.ImplementationTypeTitle, b => b.MapFrom(psp => psp.ImplementationTypeNavigation.Title))
                .ForMember(x => x.CourseTypeTitle, b => b.MapFrom(psp => psp.CourseTypeNavigation.Title))
                .ForMember(x => x.RunTypeTitle, b => b.MapFrom(psp => psp.RunTypeNavigation.Title))
                .ForMember(x => x.CustomGenderTitle, b => b.MapFrom(psp => psp.CustomGenderNavigation.Title))
                 ;

            int customerClubId = 0;
            CreateMap<ProductSalesPlanning, ProductSalePlanningViewModel>()
                .ForMember(x => x.ProductId, b => b.MapFrom(c => c.ProductId))
                .ForMember(x => x.StateTitle, b => b.MapFrom(c => c.Representation.Address.City.Parent.Title))
                .ForMember(x => x.CityTitle, b => b.MapFrom(c => c.Representation.Address.City.Title))
                .ForMember(x => x.ProductSalePlanningId, b => b.MapFrom(c => c.Id))
                .ForMember(x => x.CompanyShortName, b => b.MapFrom(c => c.Product.Company.ShortName))
                .ForMember(x => x.CategoryTitle,
                    b => b.MapFrom(c => c.Product.ProductCategory.FirstOrDefault().Category.Title))
                .ForMember(x => x.BasePrice, b => b.MapFrom(c => c.Price))
                .ForMember(x => x.Title, b => b.MapFrom(c => c.Product.Title))
                .ForMember(x => x.Price,
                    b =>
                        b.MapFrom(c =>
                            c.Price * (1 -
                                       (c.ProductSalesPlanningDiscount
                                            .Where(d =>
                                                d.CustomerClubId == customerClubId)
                                            .Select(e =>
                                                (decimal?)e.DiscountPercentage)
                                            .FirstOrDefault() ??
                                        c.SaleConfiguration.SaleConfigurationAttribute
                                            .FirstOrDefault(d =>
                                                d.CustomerClubId == customerClubId)
                                            .DiscountPercentage) / 100
                            )))

                .ForMember(x => x.CompanyId, b => b.MapFrom(c => c.Product.CompanyId))
                .ForMember(x => x.ShortDescription, b => b.MapFrom(c => c.Product.ShortDescription))
                .ForMember(x => x.CompanyTitle, b => b.MapFrom(c => c.Product.Company.Title))
                .ForMember(x => x.DisableBuyButton, b => b.MapFrom(c => c.DisableBuyButton))
                .ForMember(x => x.ModificationDate,
                    b => b.MapFrom(c => c.ModificationDate.Value.ToString("yyyyMMddhhmmss")))
                .ForMember(x => x.IsEmployment,
                    b => b.MapFrom(c => (c.Product.ProductType == (int)ProductTypeEnum.Employment) ? true : false))
                .ForMember(x => x.RelatedTagCount, b => b.MapFrom(c => c.Product.ProductTags.Count()));

            int takeCount = 0;
            CreateMap<IGrouping<int, ProductSalesPlanning>, ProductSalePlanningGroupedByCompanyViewModel>()
                    .ForMember(x => x.GroupedProductSalesPlanning, b =>
                    b.MapFrom(c => c.Select(p => new ProductSalePlanningViewModel
                    {
                        StartDate = p.StartDate,
                        EndDate = p.EndDate,
                        StartDateString = p.StartDateString,
                        CoursePlanningSchedule = p.CoursePlanningSchedule,
                        EndDateString = p.EndDateString,
                        Duration = p.Duration.ToString(),
                        RepresentationShortName = p.Representation.ShortName,
                        RepresentationTitle = p.Representation.Title,
                        ProductId = p.ProductId,
                        MaxSataSubsidy = p.MaxSataSubsidy,
                        MaxVezaratSubsidy = p.MaxVezaratSubsidy,
                        MaxEmploymentVezaratSubsidy = p.MaxEmploymentVezaratSubsidy,
                        MaxEmploymentSataSubsidy = p.MaxEmploymentSataSubsidy,
                        StateTitle = p.Representation.Address.City.Parent.Title,
                        ImplementationType = p.ImplementationType,
                        CityTitle = p.Representation.Address.City.Title,
                        ProductSalePlanningId = p.Id,
                        CompanyShortName = p.Product.Company.ShortName,
                        CategoryTitle = p.Product.ProductCategory.FirstOrDefault().Category.Title,
                        BasePrice = p.Price.Value,
                        Title = p.Product.Title,
                        CompanyId = p.Product.CompanyId,
                        ShortDescription = p.Product.ShortDescription,
                        CompanyTitle = p.Product.Company.Title,
                        DisableBuyButton = p.DisableBuyButton,
                        ModificationDate = p.ModificationDate.Value.ToString("yyyyMMddhhmmss"),
                        IsEmployment = (p.Product.ProductType == (int)ProductTypeEnum.Employment) ? true : false,
                        RelatedTagCount = p.Product.ProductTags.Count(),
                        Price = p.Price.Value * (1 -
                               (p.ProductSalesPlanningDiscount
                                    .Where(d =>
                                        d.CustomerClubId == customerClubId)
                                    .Select(e =>
                                        (decimal?)e.DiscountPercentage)
                                    .FirstOrDefault() ??
                                p.SaleConfiguration.SaleConfigurationAttribute
                                    .FirstOrDefault(d =>
                                        d.CustomerClubId == customerClubId)
                                    .DiscountPercentage) / 100)
                    }).Take(takeCount).ToList()));


            CreateMap<ProductSalesPlanning, ProductSalesPlanningEditModel>()
                .ForMember(x => x.SalesByAllRepresentations, b => b.MapFrom(c => c.Product.SalesModelType == 1085))
                .ForMember(x => x.SalesBySelectedRepresentations, b => b.MapFrom(c => c.Product.SalesModelType == 1086))
                .ForMember(x => x.AllowSelectByRepresentation, b => b.MapFrom(c => c.Product.SalesModelType == 1087))
                .ForMember(x => x.Product, b => b.MapFrom(c => c.Product));

            CreateMap<ProductSalesPlanning, ProductSalesPlanningSimpleListViewModel>()
                .ForMember(x => x.ProductTitle,
                    b => b.MapFrom(c => c.Product.Title))
                .ForMember(x => x.RepresentationTitle,
                    b => b.MapFrom(c => c.Representation.Title))
                .ForMember(x => x.CategoryTitle,
                    b => b.MapFrom(c => string.Join(",", c.Product.ProductCategory.Select(pc => pc.Category.Title))));


            CreateMap<ProductSalesPlanning, ProductSalesPlanningCustomSimpleListViewModel>()
             .ForMember(x => x.CategoryTitle, b => b.MapFrom(c => c.Product.ProductCategory.FirstOrDefault().Category.Title))
             .ForMember(x => x.CompanyTitle, b => b.MapFrom(c => c.Product.Company.Title))
             .ForMember(x => x.CompanyId, b => b.MapFrom(c => c.Product.CompanyId))
             .ForMember(x => x.ProductTitle, b => b.MapFrom(c => c.Product.Title))
             .ForMember(x => x.ApplicationUrl, b => b.MapFrom(c => c.Product.Application.Url))
             .ForMember(x => x.ApplicationId, b => b.MapFrom(c => c.Product.ApplicationId))
             .ForMember(x => x.RepresentationTitle, b => b.MapFrom(c => c.Representation.Title))
             .ForMember(x => x.CityTitle, b => b.MapFrom(c => c.Representation.Address.City.Title))
             .ForMember(x => x.StateTitle, b => b.MapFrom(c => c.Representation.Address.City.Parent.Title))
             .ForMember(x =>
                 x.CalcedPrice, b =>
                 b.MapFrom(c =>
                 c.Price * (1 - (c.ProductSalesPlanningDiscount
                     .Any(d
                         => d.CustomerClubId == customerClubId)
                     ? c.ProductSalesPlanningDiscount
                         .FirstOrDefault(e
                             => e.CustomerClubId == customerClubId)
                         .DiscountPercentage
                     : c.SaleConfiguration.SaleConfigurationAttribute
                     .FirstOrDefault(a => a.CustomerClubId == customerClubId)
                     .DiscountPercentage) / 100)));
            CreateMap<ProductSalesPlanning, ProductSalesPlanningListViewModel>()

                //.ForMember(x => x.FinalDiscountPercentage, b => b.MapFrom(c => (c.DiscountPercentage.HasValue) ?
                //   c.DiscountPercentage :
                //   (c.RepresentationId.HasValue) ?
                //       c.Representation.SaleConfiguration.FirstOrDefault(s => s.IsActive).DiscountPercentage :
                //       c.Product.Company.SaleConfiguration.FirstOrDefault(s => s.IsActive).DiscountPercentage))
                .ForMember(x => x.CategoryTitle, b => b.MapFrom(c => c.Product.ProductCategory.FirstOrDefault().Category.Title))
                .ForMember(x => x.CompanyTitle, b => b.MapFrom(c => c.Product.Company.Title))
                .ForMember(x => x.ProductSalesPlanningTitle, b => b.MapFrom(c => c.Title))
                .ForMember(x => x.ProductTitle, b => b.MapFrom(c => c.Product.Title))
                .ForMember(x => x.Telephone, b => b.MapFrom(c => c.Representation.Address.Telephone))
                .ForMember(x => x.RepresentationTitle, b => b.MapFrom(c => c.Representation.Title))
                .ForMember(x => x.Duration, b => b.MapFrom(c => c.Duration))
                .ForMember(x => x.CourseStatusTitle, b => b.MapFrom(c => c.CourseStatusNavigation.Title))
                .ForMember(x => x.AgeCategoryTitle, b => b.MapFrom(c => c.AgeCategoryNavigation.Title))
                .ForMember(x => x.ImplementationTypeTitle, b => b.MapFrom(c => c.ImplementationTypeNavigation.Title))
                .ForMember(x => x.CourseTypeTitle, b => b.MapFrom(c => c.CourseTypeNavigation.Title))
                .ForMember(x => x.RunTypeTitle, b => b.MapFrom(c => c.RunTypeNavigation.Title))
                .ForMember(x => x.CourseId, b => b.MapFrom(c => c.Product.CourseId))
                .ForMember(x => x.CustomGenderTitle, b => b.MapFrom(c => c.CustomGenderNavigation.Title))
                .ForMember(x => x.ShowFilterByProductSalesPlanningBtn,
                    b => b.MapFrom(c => c.SaleConfiguration.AllowFilterByProductSalesPlanningIdInGenerateFactor))
                .ForMember(x => x.ProductSalesPlanningAssignedToItemGroup,
                    b => b.MapFrom(c =>
                        c.ProductSalesPlanningItemGroup.Any()))
                .ForMember(x => x.IsAdmin,
               b => b.MapFrom(c => isAdmin))
                ;

            CreateMap<ProductSalesPlanning, ProductSalesPlanningBaseListViewModel>()
                .ForMember(x => x.CategoryTitle,
                    b => b.MapFrom(c => c.Product.ProductCategory.FirstOrDefault().Category.Title))
                .ForMember(x => x.CompanyTitle, b => b.MapFrom(c => c.Product.Company.Title))
                .ForMember(x=>x.ProductSalesPlanningTitle , b => b.MapFrom(c=>c.Title))
                .ForMember(x => x.ProductTitle, b => b.MapFrom(c => c.Product.Title))
                .ForMember(x => x.Telephone, b => b.MapFrom(c => c.Representation.Address.Telephone))
                .ForMember(x => x.RepresentationTitle, b => b.MapFrom(c => c.Representation.Title))
                .ForMember(x => x.Duration, b => b.MapFrom(c => c.Duration))
                .ForMember(x => x.CourseStatusTitle, b => b.MapFrom(c => c.CourseStatusNavigation.Title))
                .ForMember(x => x.ImplementationTypeTitle, b => b.MapFrom(c => c.ImplementationTypeNavigation.Title))
                .ForMember(x => x.CourseTypeTitle, b => b.MapFrom(c => c.CourseTypeNavigation.Title))
                .ForMember(x => x.RunTypeTitle, b => b.MapFrom(c => c.RunTypeNavigation.Title))
                .ForMember(x => x.CustomGenderTitle,
                    b =>
                        b.MapFrom(c => c.CustomGenderNavigation.Title));

            CreateMap<ProductSalesPlanningEditModel, ProductSalesPlanningCustomViewModel>();

            CreateMap<ProductSalesPlanning, ProductSalesPlanningCustomViewModel>()
                .ForMember(x => x.CityTitle, b => b.MapFrom(c => c.Representation.Address.City.Title))
                .ForMember(x => x.StateTitle, b => b.MapFrom(c => c.Representation.Address.City.Parent.Title))
                .ForMember(x => x.CustomGenderTitle, b => b.MapFrom(c => c.CustomGenderNavigation.Title))
                .ForMember(x => x.CustomGender, b => b.MapFrom(c => c.CustomGender))
                .ForMember(x => x.Capacity, b => b.MapFrom(c => c.MaxCapacity));



            CreateMap<ProductSalesPlanningEditModel, ProductSalesPlanning>()
                .ForMember(x => x.Product, b => b.MapFrom(c => c.Product))
                ;

            CreateMap<ProductSalesPlanning, ProductSalesPlanningSimpleViewModel>()
                .ForMember(x => x.Title, b => b.MapFrom(c => c.Product.Title));


            CreateMap<CompanyMainCategory, CompanyMainCategoryEditModel>();

            CreateMap<CompanyMainCategory, CompanyMainCategoryListViewModel>()
                .ForMember(x => x.CompanyTitle,
                    b => b.MapFrom(c => c.Company.Title))
                .ForMember(x => x.MainCategoryTitle,
                    b => b.MapFrom(c => c.Category.Title));
            CreateMap<CompanyMainCategoryEditModel, CompanyMainCategory>();

            CreateMap<CompanyMainCategory, CompanyMainCategorySimpleViewModel>();


            CreateMap<DependentProductSale, DependentProductSaleEditModel>();

            CreateMap<DependentProductSale, DependentProductSaleListViewModel>()
                .ForMember(x => x.MainProductTitle,
                    b => b.MapFrom(c => c.MainProductSalePlanning.Product.Title))
                .ForMember(x => x.RelatedProductTitle,
                    b => b.MapFrom(c => c.RelatedProductSalePlanning.Product.Title))
                .ForMember(x => x.AccessLevelTitle,
                    b => b.MapFrom(c => c.AccessLevel.Title));
            CreateMap<DependentProductSaleEditModel, DependentProductSale>();

            CreateMap<DependentProductSale, DependentProductSaleSimpleViewModel>();


            CreateMap<ProductCategory, ProductCategoryEditModel>();

            CreateMap<ProductCategory, ProductCategoryListViewModel>()
                .ForMember(x => x.CategoryTitle,
                    b => b.MapFrom(c => c.Category.Title))
                .ForMember(x => x.CategoryTitle,
                    b => b.MapFrom(c => c.Category.Title))
                .ForMember(x => x.ProductTitle,
                    b => b.MapFrom(c => c.Product.Title));
            CreateMap<ProductCategoryEditModel, ProductCategory>();

            CreateMap<ProductCategory, ProductCategorySimpleViewModel>();


            CreateMap<Warehouse, WarehouseEditModel>();

            CreateMap<Warehouse, WarehouseListViewModel>()
                .ForMember(x => x.AccessLevelTitle, b => b.MapFrom(c => c.AccessLevel.Title));
            CreateMap<WarehouseEditModel, Warehouse>();

            CreateMap<Warehouse, WarehouseSimpleViewModel>();


            CreateMap<Category, CategoryEditModel>();
            CreateMap<Category, MainCategorySimpleViewModel>();

            CreateMap<Category, CategoryListViewModel>()
                .ForMember(x => x.AccessLevelTitle, b =>
                b.MapFrom(c => c.AccessLevel.Title))
                .ForMember(x => x.AllowEditAndDelete,
                    b =>
                        b.MapFrom(c =>
                            (!currentUserCompanyId.HasValue && !c.CompanyId.HasValue)
                           || (currentUserCompanyId.HasValue && c.CompanyId == currentUserCompanyId.Value)
                        ));

            CreateMap<CategoryEditModel, Category>();

            CreateMap<Category, CategorySimpleViewModel>();

            CreateMap<CategoryCustomModel, CategorySimpleViewModel>()
                 .ForMember(x => x.ModificationDate, b => b.MapFrom(c => c.ModificationDate.Value.ToString("yyyyMMddhhmmss")));

            CreateMap<Category, CategoryCustomViewModelForSelect>();


            CreateMap<Company, CompanyEditModel>();

            CreateMap<Company, CompanyListViewModel>()
                .ForMember(x => x.EstablishedLicenseTypeTitle,
                    b => b.MapFrom(c => c.EstablishedLicenseTypeNavigation.Title))
                .ForMember(x => x.AccessLevelTitle, b => b.MapFrom(c => c.AccessLevel.Title));

            CreateMap<CompanyEditModel, Company>();

            CreateMap<Company, CompanySimpleViewModel>();

            CreateMap<Company, CompanyWithChildrenForSelectViewModel>()
                .ForMember(x => x.HeadId,
                    b => b.MapFrom(c =>
                        !string.IsNullOrEmpty(currentUserCompanyCodePath) && c.CodePath == currentUserCompanyCodePath ? null : c.ParentId
                        ));

            CreateMap<Company, CompanyWithChildrenForListViewModel>()
                .ForMember(x => x.ParentId,
                    b => b.MapFrom(c =>
                        !string.IsNullOrEmpty(currentUserCompanyCodePath) && c.CodePath == currentUserCompanyCodePath
                            ? null
                            : c.ParentId
                    ))
                .ForMember(x => x.ShowCompanyInformationButton,
                    b => b.MapFrom(c => c.ParentId.HasValue ? true : false));

            CreateMap<Company, CompanyCustomSimpleViewModel>()
                 .ForMember(x => x.ApplicationUrl,
                    b => b.MapFrom(c => c.Application.Url))
                 .ForMember(x => x.ApplicationId,
                    b => b.MapFrom(c => c.ApplicationId));

            CreateMap<TaxCategory, TaxCategoryEditModel>();
            CreateMap<TaxCategory, TaxCategoryDefaultValuesViewModel>();

            CreateMap<TaxCategory, TaxCategoryListViewModel>()
                .ForMember(x => x.AccessLevelTitle,
                    b => b.MapFrom(c => c.AccessLevel.Title));
            CreateMap<TaxCategoryEditModel, TaxCategory>();

            CreateMap<TaxCategory, TaxCategorySimpleViewModel>();
            CreateMap<ContractPaymentMethod, ContractPaymentMethodEditModel>();

            CreateMap<ContractPaymentMethod, ContractPaymentMethodListViewModel>()
                .ForMember(x => x.ContractTitle,
                    b => b.MapFrom(c => c.Contract.Title))
                .ForMember(x => x.PaymentMethodTypeTitle,
                    b => b.MapFrom(c => c.PaymentMethodType))
                .ForMember(x => x.AccessLevelTitle,
                    b => b.MapFrom(c => c.AccessLevel.Title));
            CreateMap<ContractPaymentMethodEditModel, ContractPaymentMethod>();

            CreateMap<ContractPaymentMethod, ContractPaymentMethodSimpleViewModel>();

            CreateMap<SaleConfiguration, SaleConfigurationEditModel>();

            CreateMap<SaleConfiguration, SaleConfigurationListViewModel>()
                .ForMember(x => x.RepresentationTitle,
                    b => b.MapFrom(c => c.Representation.Title))
                .ForMember(x => x.CompanyTitle,
                    b => b.MapFrom(c => c.Company.Title))
                .ForMember(x => x.AccessLevelTitle,
                    b => b.MapFrom(c => c.AccessLevel.Title))
                .ForMember(x => x.FactorReportTypeTitle,
                    b => b.MapFrom(c => c.FactorReportTypeNavigation.Title))
                .ForMember(x => x.FounderMobileNumber, b => b.MapFrom(c => c.Founder.Person.MobileNumber))
                .ForMember(x => x.FounderPhoneNumber, b => b.MapFrom(c => c.Founder.Person.PhoneNumber))
                ;
            CreateMap<SaleConfigurationEditModel, SaleConfiguration>();

            CreateMap<SaleConfiguration, SaleConfigurationSimpleViewModel>();
            CreateMap<Specification, SpecificationEditModel>();

            CreateMap<Specification, SpecificationListViewModel>()
                .ForMember(x => x.AccessLevelTitle, b => b.MapFrom(c => c.AccessLevel.Title));
            CreateMap<SpecificationEditModel, Specification>();

            CreateMap<Specification, SpecificationSimpleViewModel>();


            CreateMap<ProductSpecificationOption, ProductSpecificationOptionEditModel>()
                .ForMember(x => x.SpecificationId,
                    b => b.MapFrom(c => c.SpecificationOption.SpecificationId));

            CreateMap<ProductSpecificationOption, ProductSpecificationOptionListViewModel>()
                .ForMember(x => x.ProductTitle,
                    b => b.MapFrom(c => c.Product.Title))
                .ForMember(x => x.SpecificationOptionTitle,
                    b => b.MapFrom(c => c.SpecificationOption.Title))
                .ForMember(x => x.SpecificationTitle,
                    b => b.MapFrom(c => c.SpecificationOption.Specification.Title))
                .ForMember(x => x.AttributeTypeTitle, b => b.MapFrom(c => c.AttributeTypeNavigation.Title)).ForMember(
                    x => x.AccessLevelTitle,
                    b => b.MapFrom(c => c.AccessLevel.Title))
                ;
            CreateMap<ProductSpecificationOptionEditModel, ProductSpecificationOption>();

            CreateMap<ProductSpecificationOption, ProductSpecificationOptionSimpleViewModel>();


            CreateMap<SpecificationOption, SpecificationOptionEditModel>();

            CreateMap<SpecificationOption, SpecificationOptionListViewModel>()
                .ForMember(x => x.SpecificationTitle,
                    b => b.MapFrom(c => c.Specification.Title))
                .ForMember(x => x.AccessLevelTitle,
                    b => b.MapFrom(c => c.AccessLevel.Title));
            CreateMap<SpecificationOptionEditModel, SpecificationOption>();

            CreateMap<SpecificationOption, SpecificationOptionSimpleViewModel>();


            CreateMap<CategorySpecification, CategorySpecificationEditModel>();

            CreateMap<CategorySpecification, CategorySpecificationListViewModel>()
                .ForMember(x => x.CategoryTitle,
                    b => b.MapFrom(c => c.Category.Title))
                .ForMember(x => x.SpecificationTitle,
                    b => b.MapFrom(c => c.Specification.Title))
                .ForMember(x => x.AccessLevelTitle,
                    b => b.MapFrom(c => c.AccessLevel.Title));
            CreateMap<CategorySpecificationEditModel, CategorySpecification>();

            CreateMap<CategorySpecification, CategorySpecificationSimpleViewModel>();


            CreateMap<ProductType, ProductTypeEditModel>();

            CreateMap<ProductType, ProductTypeListViewModel>()
                .ForMember(x => x.AccessLevelTitle, b => b.MapFrom(c => c.AccessLevel.Title))
                .ForMember(x => x.TaxCategoryTitle, b => b.MapFrom(c => c.TaxCategory.Title));
            CreateMap<ProductTypeEditModel, ProductType>();

            CreateMap<ProductType, ProductTypeSimpleViewModel>();


            CreateMap<ProductTypeSpecification, ProductTypeSpecificationEditModel>();

            CreateMap<ProductTypeSpecification, ProductTypeSpecificationListViewModel>()
                .ForMember(x => x.ProductTypeTitle,
                    b => b.MapFrom(c => c.ProductType.Title))
                .ForMember(x => x.SpecificationTitle,
                    b => b.MapFrom(c => c.Specification.Title))
                .ForMember(x => x.AccessLevelTitle,
                    b => b.MapFrom(c => c.AccessLevel.Title))
                ;
            CreateMap<ProductTypeSpecificationEditModel, ProductTypeSpecification>();

            CreateMap<ProductTypeSpecification, ProductTypeSpecificationSimpleViewModel>();


            CreateMap<Subsidy, SubsidyEditModel>();

            CreateMap<Subsidy, SubsidyListViewModel>()
                .ForMember(x => x.AccessLevelTitle,
                    b => b.MapFrom(c => c.AccessLevel.Title))
                .ForMember(x => x.CategoryGroupTitle,
                    b => b.MapFrom(c => c.CategoryGroup.Title))

                ;
            CreateMap<SubsidyEditModel, Subsidy>();

            CreateMap<Subsidy, SubsidySimpleViewModel>();


            CreateMap<Affiliate, AffiliateEditModel>();

            CreateMap<Affiliate, AffiliateListViewModel>()
                .ForMember(x => x.AccessLevelTitle, b => b.MapFrom(c => c.AccessLevel.Title));
            CreateMap<AffiliateEditModel, Affiliate>();

            CreateMap<Affiliate, AffiliateSimpleViewModel>();


            CreateMap<RewardPointsHistory, RewardPointsHistoryEditModel>();

            CreateMap<RewardPointsHistory, RewardPointsHistoryListViewModel>()
                .ForMember(x => x.AccessLevelTitle, b => b.MapFrom(c => c.AccessLevel.Title));
            CreateMap<RewardPointsHistoryEditModel, RewardPointsHistory>();

            CreateMap<RewardPointsHistory, RewardPointsHistorySimpleViewModel>();


            CreateMap<ShoppingCartItem, ShoppingCartItemEditModel>();

            CreateMap<ShoppingCartItem, ShoppingCartItemViewModel>()
                .ForMember(x => x.MaxSataSubsidy, b => b.MapFrom(c => c.ProductSalesPlanning.MaxSataSubsidy))
                .ForMember(x => x.MaxVezaratSubsidy, b => b.MapFrom(c => c.ProductSalesPlanning.MaxVezaratSubsidy))
                .ForMember(x => x.ProductSalesPlanningId, b => b.MapFrom(c => c.ProductSalesPlanning.Id))
                .ForMember(x => x.RepresentationTitle, b => b.MapFrom(c => c.Representation.Title))
                .ForMember(x => x.CompanyTitle, b => b.MapFrom(c => c.ProductSalesPlanning.Product.Company.Title))
                .ForMember(x => x.UnitPrice, b => b.MapFrom(c => c.ProductSalesPlanning.Price * c.Quantity))
                // .ForMember(x => x.CalcPrice, b => b.MapFrom(c => c.ProductSalesPlanning.CalcedPrice * c.Quantity))
                .ForMember(x => x.Title, b => b.MapFrom(c => c.ProductSalesPlanning.Product.Title));


            CreateMap<ShoppingCartItem, OrderItemDetailPresentationViewModel>()

                .ForMember(x => x.ShoppingCartItemId, b => b.MapFrom(c => c.Id))
                .ForMember(x => x.ProductSalesPlanningId, b => b.MapFrom(c => c.ProductSalesPlanning.Id))
                .ForMember(x => x.RepresentationTitle, b => b.MapFrom(c => c.Representation.Title))
                .ForMember(x => x.CompanyTitle, b => b.MapFrom(c => c.ProductSalesPlanning.Product.Company.Title))
                .ForMember(x => x.EndDate, b => b.MapFrom(c => c.ProductSalesPlanning.EndDate))
                .ForMember(x => x.StartDate, b => b.MapFrom(c => c.ProductSalesPlanning.StartDate))
                .ForMember(x => x.CoursePlanningSchedule,
                    b => b.MapFrom(c => c.ProductSalesPlanning.CoursePlanningSchedule))
                .ForMember(x => x.MessageForUserRequireSupplierApprovalBeforePaymentProducts,
                    b =>
                        b.MapFrom(c =>
                            c.ProductSalesPlanning.SaleConfiguration.RequireSupplierApprovalBeforePayment
                                ? c.ProductSalesPlanning.SaleConfiguration
                                    .MessageToUserInRequireSupplierApprovalBeforePayment
                                : c.ProductSalesPlanning.Product.RequireSupplierApprovalBeforePayment
                                    ? c.ProductSalesPlanning.Product.MessageToUserInRequireSupplierApprovalBeforePayment
                                    : string.Empty
                        ))
                .ForMember(x => x.UnitPrice, b => b.MapFrom(c => c.ProductSalesPlanning.Price))
                // .ForMember(x => x.DiscountAmountInclTax, b => b.MapFrom(c => c.ProductSalesPlanning.Price - c.ProductSalesPlanning.CalcedPrice))
                // .ForMember(x => x.CalcPrice, b => b.MapFrom(c => c.ProductSalesPlanning.CalcedPrice))

                .ForMember(x => x.Title, b => b.MapFrom(c => c.ProductSalesPlanning.Product.Title))
                .ForMember(x => x.CityTitle, b => b.MapFrom(c => c.Representation.Address.City.Title))
                .ForMember(x => x.StateTitle, b => b.MapFrom(c => c.Representation.Address.City.Parent.Title))
                .ForMember(x => x.RepresentationAddress, b => b.MapFrom(c => c.Representation.Address.FullAddress))
                .ForMember(x => x.RepresentationAddressIsEncrypted,
                    b => b.MapFrom(c => c.Representation.Address.IsEncrypted))
                .ForMember(x => x.RepresentationTelephone, b => b.MapFrom(c => c.Representation.Address.Telephone))
                .ForMember(x => x.SaleConfigurationId, b => b.MapFrom(c => c.ProductSalesPlanning.SaleConfigurationId)
                ).ForMember(x => x.MinimumPurchasePrice,
                    b => b.MapFrom(c => c.ProductSalesPlanning.SaleConfiguration.MinimumPurchasePrice));

            CreateMap<ShoppingCartItemEditModel, ShoppingCartItem>();

            CreateMap<ShoppingCartItem, ShoppingCartItemListViewModel>()
                .ForMember(x => x.CompanyId, b => b.MapFrom(c => c.ProductSalesPlanning.Product.Company.Id))
                .ForMember(x => x.UnitPrice, b => b.MapFrom(c => c.ProductSalesPlanning.Price))
                .ForMember(x => x.ProductSalesPlanningTitle, b => b.MapFrom(c => c.ProductSalesPlanning.Product.Title))
                .ForMember(x => x.ShoppingCartTypeTitle, b => b.MapFrom(c => c.ShoppingCartTypeNavigation.Title));

            CreateMap<ShoppingCartItemEditModel, ShoppingCartItem>();

            CreateMap<ShoppingCartItem, ShoppingCartItemSimpleViewModel>();


            CreateMap<Subsidy, SubsidyEditModel>();

            CreateMap<Subsidy, SubsidyListViewModel>()
                .ForMember(x => x.AccessLevelTitle,
                    b => b.MapFrom(c => c.AccessLevel.Title));
            CreateMap<SubsidyEditModel, Subsidy>();

            CreateMap<Subsidy, SubsidySimpleViewModel>();

            CreateMap<CommonBaseData, CommonBaseDataListViewModel>();


            CreateMap<ProductSalesPlanning, ProductSalePlanningPresentationViewModel>()
                .ForMember(x => x.CategoryTitle,
                    b => b.MapFrom(c => c.Product.ProductCategory.FirstOrDefault().Category.Title))
                .ForMember(x => x.CategoryId,
                    b => b.MapFrom(c => c.Product.DefaultCategoryId))
                .ForMember(x => x.ParentCategoryTitle,
                    b => b.MapFrom(c => c.Product.ProductCategory.FirstOrDefault().Category.Parent.Title))
                .ForMember(x => x.ShortDescription, b => b.MapFrom(c => c.Product.ShortDescription))
                .ForMember(x => x.FullDescription, b => b.MapFrom(c => c.Product.FullDescription))
                .ForMember(x => x.CompanyId, b => b.MapFrom(c => c.Product.CompanyId))
                .ForMember(x => x.CompanyTitle, b => b.MapFrom(c => c.Product.Company.Title))
                .ForMember(x => x.ProductTitle, b => b.MapFrom(c => c.Product.Title))
                .ForMember(x => x.RepresentationTitle, b => b.MapFrom(c => c.Representation.Title))
                .ForMember(x => x.FullAddress, b => b.MapFrom(c => c.Representation.Address.FullAddress))
                .ForMember(x => x.IsEncryptedAddress, b => b.MapFrom(c => c.Representation.Address.IsEncrypted))
                .ForMember(x => x.ExternalLink, b => b.MapFrom(c => c.Product.ExternalLink))
                .ForMember(x => x.Phone, b => b.MapFrom(c => c.Representation.Address.Telephone))
                .ForMember(x => x.DiscountPercentage,
                    b =>
                        b.MapFrom(c =>
                            c.ProductSalesPlanningDiscount
                                .FirstOrDefault(x =>
                                    x.CustomerClubId == customerClubId)
                                .DiscountPercentage
                        ))
                .ForMember(x => x.ModificationDate,
                    b => b.MapFrom(c => c.ModificationDate.Value.ToString("yyyyMMddhhmmss")))
                .ForMember(x => x.IsEmployement,
                    b => b.MapFrom(c => (c.Product.ProductType == (int)ProductTypeEnum.Employment) ? true : false));

            CreateMap<Payment, PaymentEditModel>();
            CreateMap<Payment, CustomerClubPaymentGroupItemListViewModel>()
                //.ForMember(x => x.ApplicationName, b => b.MapFrom(c => c.Application.Title))
                //.ForMember(x => x.ProductTitle, b => b.MapFrom(c => string.Join(", ", c.Order.OrderItem.Select(oi => oi.ProductSalesPlanning.Product.Title))))
                //.ForMember(x => x.ProvinceTitle, b => b.MapFrom(c => string.Join(", ", c.Order.OrderItem.Select(oi => oi.Representation.Address.City.Parent.Title))))
                //.ForMember(x => x.CityTitle, b => b.MapFrom(c => string.Join(", ", c.Order.OrderItem.Select(oi => oi.Representation.Address.City.Title))))
                .ForMember(x => x.NationalCode, b => b.MapFrom(c => c.Order.NationalCode))
                .ForMember(x => x.CustomerClubCardCode, b => b.MapFrom(c => c.Order.CustomerClubCard.Code))
                .ForMember(x => x.CardNumber, b => b.MapFrom(c => c.Order.CardNumber))
                .ForMember(x => x.InstallmentCount, b => b.MapFrom(c => c.InstallmentCount))
                .ForMember(x => x.Total, b => b.MapFrom(c => c.Total));
            CreateMap<Payment, PaymentListViewModel>()
                .ForMember(x => x.CardTypeTitle, b => b.MapFrom(c => c.CardTypeNavigation.Title))
                .ForMember(x => x.SaleTypeTitle, b => b.MapFrom(c => c.SaleTypeNavigation.Title))
                .ForMember(x => x.PaymentStatusTypeTitle, b => b.MapFrom(c => c.PaymentStatusTypeNavigation.Title))
                .ForMember(x => x.PaymentMethodTypeTitle, b => b.MapFrom(c => c.PaymentMethodTypeNavigation.Title))
                .ForMember(x => x.NewPaymentAmount, b => b.MapFrom(c => c.RefundDetail.Min(rd => rd.NewPaymentAmount)))
                .ForMember(x => x.SumRefundAmount, b => b.MapFrom(c => c.RefundDetail.Sum(rd => rd.RefundAmount)));
            CreateMap<PaymentEditModel, Payment>();
            CreateMap<ApplicationPaymentMethod, ApplicationPaymentMethodListViewModel>()
                .ForMember(x => x.PaymentMethodTypeTitle, b => b.MapFrom(c => c.PaymentMethodTypeNavigation.Title))
                .ForMember(x => x.AccessLevelTitle,
                    b => b.MapFrom(c => c.AccessLevel.Title));
            CreateMap<ApplicationPaymentMethodEditModel, ApplicationPaymentMethod>();

            CreateMap<ApplicationPaymentMethod, ApplicationPaymentMethodSimpleViewModel>()
                .ForMember(x => x.Title, b => b.MapFrom(d => d.PaymentMethodTypeNavigation.Title));

            CreateMap<PaymentMethodType, PaymentSimpleViewModel>()

               .ForMember(x => x.Id, b => b.MapFrom(c => c.Id))
               .ForMember(x => x.Title, b => b.MapFrom(d => d.Title));


            CreateMap<ApplicationPaymentMethod, PaymentSimpleViewModel>()
                .ForMember(x => x.Id, b => b.MapFrom(c => c.PaymentMethodType))
                .ForMember(x => x.Title, b => b.MapFrom(d => d.PaymentMethodTypeNavigation.Title));
            CreateMap<Payment, PaymentSimpleViewModel>();
            CreateMap<Payment, SuccessFulPaymentListViewModel>()
                .ForMember(x => x.Total,
               b =>
                   b.MapFrom(c =>
                       c.RefundDetail.Any() && c.RefundDetail.OrderByDescending(rd => rd.Id).FirstOrDefault().RefundDetailGroupId != null
                           ? c.RefundDetail.OrderByDescending(rd => rd.Id).FirstOrDefault().NewPaymentAmount
                                : c.Total))
                .ForMember(x => x.PaymentMethodTypeTitle, b => b.MapFrom(c => c.PaymentMethodTypeNavigation.Title));

            CreateMap<FactorItem, FactorItemEditModel>();

            CreateMap<FactorItem, FactorItemListViewModel>()
                .ForMember(x => x.FactorTitle,
                    b => b.MapFrom(c => c.Factor.Title))
                .ForMember(x => x.AccessLevelTitle,
                    b => b.MapFrom(c => c.AccessLevel.Title))
                .ForMember(x => x.OrderItemPriceIncDiscount,
                    b => b.MapFrom(c => c.OrderItem.PriceIncDiscount))
                .ForMember(x => x.OrderItemTitle,
                    b => b.MapFrom(c => c.OrderItem.ProductSalesPlanning.Product.Title))
                .ForMember(x => x.OrderId,
                    b => b.MapFrom(c => c.OrderItem.OrderId))
                .ForMember(x => x.OrderDate,
                    b => b.MapFrom(c => c.OrderItem.Order.CreationDateInt))
                .ForMember(x => x.CompanyTitle,
                    b => b.MapFrom(c => c.OrderItem.ProductSalesPlanning.Product.Company.Title))
                .ForMember(x => x.RepresentationTitle,
                    b => b.MapFrom(c => c.OrderItem.Representation.Title))
                .ForMember(x => x.CustomerUserName,
                    b => b.MapFrom(c => c.OrderItem.Order.User.UserName))
                .ForMember(x => x.Title,
                    b => b.MapFrom(c => c.OrderItem.Order.Title))
                .ForMember(x => x.Quantity,
                    b => b.MapFrom(c => c.OrderItem.Quantity));


            //            CreateMap<FactorItemDetailStatistic, FactorItemListViewModel>()
            //                .ForMember(x => x.Quantity,
            //                    b => b.MapFrom(c => c.Quantity));
            CreateMap<FactorItemEditModel, FactorItem>();

            CreateMap<FactorItem, FactorItemSimpleViewModel>();


            CreateMap<Factor, FactorEditModel>();
            CreateMap<FactorStatisticDomainModel, FactorStatisticViewModel>();

            CreateMap<Factor, FactorListViewModel>()
                .ForMember(x => x.TaxCategoryTitle,
                    b => b.MapFrom(c => c.TaxCategory.Title))
                .ForMember(x => x.CompanyTitle,
                    b => b.MapFrom(c => c.Company.Title))
                .ForMember(x => x.RepresentationTitle,
                    b => b.MapFrom(c => c.Representation.Title))
                .ForMember(x => x.FactorReviewStatusTypeTitle,
                    b => b.MapFrom(c => c.FactorReviewStatusTypeNavigation.Title))
                .ForMember(x => x.SaleConfigurationTitle,
                    b => b.MapFrom(c => c.SaleConfiguration.Title))
                .ForMember(x => x.AllowDelete,
                    b => b.MapFrom(c => (c.FactorReviewStatusType == (int)FactorReviewStatusTypeEnum.WaitingForTheFileToUpload)));

            CreateMap<Factor, FactorListForFactorGroupListViewModel>()
                .ForMember(x => x.CategoryGroupTitle,
                    b => b.MapFrom(c => c.CategoryGroup.Title))
                .ForMember(x => x.TaxCategoryTitle,
                    b => b.MapFrom(c => c.TaxCategory.Title))
                .ForMember(x => x.CompanyTitle,
                    b => b.MapFrom(c => c.Company.Title))
                .ForMember(x => x.RepresentationTitle,
                    b => b.MapFrom(c => c.Representation.Title))
                .ForMember(x => x.FactorReviewStatusTypeTitle,
                    b => b.MapFrom(c => c.FactorReviewStatusTypeNavigation.Title))
                .ForMember(x => x.SaleConfigurationTitle,
                    b => b.MapFrom(c => c.SaleConfiguration.Title))
                .ForMember(x => x.AllowRemoveFromGroup,
                    b => b.MapFrom(c => c.FactorGroup.FactorGroupStatus == (int)FactorGroupStatusTypeEnum.Initial));


            CreateMap<Factor, SuperVisorFactorListViewModel>()
                .ForMember(x => x.TaxCategoryTitle,
                    b => b.MapFrom(c => c.TaxCategory.Title))
                .ForMember(x => x.CategoryGroupTitle,
                    b => b.MapFrom(c => c.CategoryGroup.Title))
                .ForMember(x => x.CompanyTitle,
                    b => b.MapFrom(c => c.Company.Title))
                .ForMember(x => x.RepresentationTitle,
                    b => b.MapFrom(c => c.Representation.Title))
                .ForMember(x => x.FactorReviewStatusTypeTitle,
                    b => b.MapFrom(c => c.FactorReviewStatusTypeNavigation.Title))
                .ForMember(x => x.FactorGroupTitle,
                    b => b.MapFrom(c => string.Concat(c.FactorGroup.Title, " " + c.FactorGroup.Parent.Title)))
                .ForMember(x => x.AllowDelete,
                    b => b.MapFrom(c => (c.FactorReviewStatusType == (int)FactorReviewStatusTypeEnum.Rejected)))
                .ForMember(x => x.IsGroupedByFactorGroup,
                    b => b.MapFrom(c => c.FactorGroupId.HasValue))
                .ForMember(x => x.SaleConfigurationTitle,
                    b => b.MapFrom(c => c.SaleConfiguration.Title))
                .ForMember(x => x.CustomerClubTitle,
                    b => b.MapFrom(c =>
                        c.CustomerClubId.HasValue ?
                c.CustomerClub.Title
                : string.Empty));

            CreateMap<FactorItem, FactorItemPaymentsViewModel>()
                 .ForMember(x => x.FactorOrderItemPaymentList, b =>
                    b.MapFrom(c => c.OrderItem.OrderItemPayment));

            CreateMap<OrderItemPayment, FactorOrderItemPaymentViewModel>()
                .ForMember(x => x.FactorCode, b =>
                    b.MapFrom(c => c.OrderItem.FactorItem.Factor.Code))
               .ForMember(x => x.PaymentId, b =>
                    b.MapFrom(c => c.PaymentId))
              .ForMember(x => x.InstallmentCount, b =>
                    b.MapFrom(c => c.Payment.InstallmentCount))
              .ForMember(x => x.OrderItemPayment, b =>
                    b.MapFrom(c => c.Price))
              .ForMember(x => x.Total, b =>
                    b.MapFrom(c => c.Payment.Total))
              .ForMember(x => x.PaidDate, b =>
                    b.MapFrom(c => c.Payment.PaidDateInt))
              .ForMember(x => x.PaymentMethodTitle,
                  b =>
                     b.MapFrom(c => c.Payment.PaymentMethodTypeNavigation.Title));

            CreateMap<ItemPaymentSummaryDto, FactorItemSummaryViewModel>();


            CreateMap<Factor, GroupedFactorListViewModel>()
                .ForMember(x => x.TaxCategoryTitle,
                    b => b.MapFrom(c => c.TaxCategory.Title))
                .ForMember(x => x.CompanyTitle,
                    b => b.MapFrom(c => c.Company.Title))
                .ForMember(x => x.RepresentationTitle,
                    b => b.MapFrom(c => c.Representation.Title))
                .ForMember(x => x.FactorGroupTitle,
                    b => b.MapFrom(c => c.FactorGroup.Title))
                .ForMember(x => x.FactorGroupParentTitle,
                    b =>
                        b.MapFrom(c => c.FactorGroup.Parent.Title))
                .ForMember(x => x.FactorGroupParentId,
                    b =>
                        b.MapFrom(c => c.FactorGroup.ParentId))
                .ForMember(x => x.FactorReviewStatusTypeTitle,
                    b => b.MapFrom(c => c.FactorReviewStatusTypeNavigation.Title))
                .ForMember(x => x.SaleConfigurationTitle,
                    b => b.MapFrom(c => c.SaleConfiguration.Title));


            CreateMap<FactorEditModel, Factor>();

            CreateMap<Factor, FactorSimpleViewModel>();
            CreateMap<Factor, FactorAssignListViewModel>()
                .ForMember(x => x.CategoryGroupTitle,
                    b => b.MapFrom(c => c.CategoryGroup.Title))
                .ForMember(x => x.TaxCategoryTitle,
                    b => b.MapFrom(c => c.TaxCategory.Title))
                .ForMember(x => x.RepresentationTitle, b => b.MapFrom(c => c.Representation.Title))
                .ForMember(x => x.CompanyTitle, b => b.MapFrom(c => c.Company.Title));
            CreateMap<Factor, FactorReportSimpleViewModel>()
                .ForMember(x => x.Code,
                    b => b.MapFrom(c => c.Code))
                .ForMember(x => x.CompanyId,
                    b => b.MapFrom(c => c.CompanyId))
                .ForMember(x => x.CompanyTitle,
                    b => b.MapFrom(c => c.Company.Title))
                .ForMember(x => x.RepresentationId,
                    b => b.MapFrom(c => c.RepresentationId))
                .ForMember(x => x.RepresentationTitle,
                    b => b.MapFrom(c => c.Representation.Title))
                .ForMember(x => x.TaxCategoryTitle,
                    b => b.MapFrom(c => c.TaxCategory.Title))
                .ForMember(x => x.FactorItemCount,
                    b => b.MapFrom(c => c.FactorItem.Sum(d => d.OrderItem.Quantity)))
                .ForMember(x => x.SumTotalInclSubsidy,
                    b => b.MapFrom(c => c.FactorItem.Sum(d => d.OrderItem.TotalInclSubsidy)))
                .ForMember(x => x.DiscountPercentage,
                    b => b.MapFrom(c => c.DiscountPercentage))
                .ForMember(x => x.FirstFactorItemDateInt,
                    b => b.MapFrom(c => c.FactorItem.Min(d => d.OrderItem.Order.CreationDateInt)))
                .ForMember(x => x.LastFactorItemDateInt,
                b => b.MapFrom(c => c.FactorItem.Max(d => d.OrderItem.Order.CreationDateInt)))
                .ForMember(x => x.FactorConfirmDate,
                b => b.MapFrom(c => c.ConfirmDate))
                .ForMember(x => x.FactorGroupConfirmDate,
                b => b.MapFrom(c => c.FactorGroup.ConfirmDate))
                .ForMember(x => x.SumPriceInclDiscount,
                    b => b.MapFrom(c => c.FactorItem.Sum(d => d.OrderItem.PriceIncDiscount)))
                ;





            CreateMap<FactorPayment, FactorPaymentEditModel>();

            CreateMap<FactorPayment, FactorPaymentListViewModel>();
            CreateMap<FactorPaymentEditModel, FactorPayment>();

            CreateMap<FactorPayment, FactorPaymentSimpleViewModel>();


            CreateMap<ContentCategory, ContentCategoryEditModel>();

            CreateMap<ContentCategory, ContentCategoryListViewModel>()
                .ForMember(x => x.AccessLevelTitle,
                    b => b.MapFrom(c => c.AccessLevel.Title));

            CreateMap<ContentCategoryEditModel, ContentCategory>();

            CreateMap<ContentCategory, ContentCategorySimpleViewModel>();

            CreateMap<ApplicationCustomerClub, ApplicationCustomerClubEditModel>();

            CreateMap<ApplicationCustomerClub, ApplicationCustomerClubListViewModel>()
                .ForMember(x => x.AccessLevelTitle,
                    b => b.MapFrom(c => c.AccessLevel.Title));
            CreateMap<ApplicationCustomerClubEditModel, ApplicationCustomerClub>();
            CreateMap<ApplicationCustomerClub, ApplicationCustomerClubSimpleViewModel>();
            CreateMap<ApplicationCustomerClub, ApplicationCustomerClubEditModel>();
            CreateMap<ApplicationCustomerClub, ApplicationCustomerClubListViewModel>()
                .ForMember(x => x.AccessLevelTitle,
                    b => b.MapFrom(c => c.AccessLevel.Title));
            CreateMap<ApplicationCustomerClubEditModel, ApplicationCustomerClub>();
            CreateMap<ApplicationCustomerClub, ApplicationCustomerClubSimpleViewModel>();
            CreateMap<FactorVerification, FactorVerificationEditModel>()
                .ForMember(x => x.VerificationOrder,
                    b => b.MapFrom(c => c.FactorVerificationConfigurationRole.VerificationOrder));
            CreateMap<FactorVerification, FactorVerificationListViewModel>()
                .ForMember(x => x.FactorConfirmationTypeTitle,
                    b => b.MapFrom(c => c.FactorConfirmationTypeNavigation.Title))
                .ForMember(x => x.FactorConfirmationType,
                    b => b.MapFrom(c => c.FactorConfirmationType))
                .ForMember(x => x.AccessLevelTitle,
                    b => b.MapFrom(c => c.AccessLevel.Title))
                .ForMember(x => x.FactorCode,
                    b => b.MapFrom(c => c.Factor.Code))
                .ForMember(x => x.VerificationOrderTitle,
                    b => b.MapFrom(c => string.Concat(@" تایید کننده", c.FactorVerificationConfigurationRole.VerificationOrder.ToString()))).
                ForMember(x => x.VerifierFullName, b => b.MapFrom(c => string.Concat(c.VerifyByNavigation.PersonNavigation.Name, " " + c.VerifyByNavigation.PersonNavigation.LastName)))
                .ForMember(x => x.CompanyTitle,
                    b => b.MapFrom(c => c.Factor.Company.Title))
                .ForMember(x => x.RepresentationTitle,
                    b => b.MapFrom(c => c.Factor.Representation.Title))
                .ForMember(x => x.FactorDate,
                    b => b.MapFrom(c => c.Factor.CreationDate))
                .ForMember(x => x.TaxCategoryTitle,
                    b => b.MapFrom(c => c.Factor.TaxCategory.Title))
                .ForMember(x => x.TryNumber,
                    b => b.MapFrom(c => c.TryNumber))
                .ForMember(x => x.Description,
                    b => b.MapFrom(c => c.FactorConfirmationType == (int)FactorConfirmationTypeEnum.Rejected ? c.Description : string.Empty))
                .ForMember(x => x.SaleConfigurationTitle,
                    b => b.MapFrom(c => c.Factor.SaleConfiguration.Title))
                .ForMember(x => x.DiscountPercentage, b => b.MapFrom(c => c.Factor.DiscountPercentage))
                .ForMember(x => x.CreationDate, b => b.MapFrom(c => c.CreationDate))
                .ForMember(x => x.ProfitPercentage, b => b.MapFrom(c => c.Factor.ProfitPercentage));
            CreateMap<FactorVerification, FactorVerificationDescriptionListViewModel>()
                .ForMember(x => x.Description,
                    b => b.MapFrom(c => c.Description))
                .ForMember(x => x.VerifierFullName, b => b.MapFrom(c => string.Concat(c.VerifyByNavigation.PersonNavigation.Name, " " + c.VerifyByNavigation.PersonNavigation.LastName)));
            CreateMap<FactorVerificationEditModel, FactorVerification>();
            CreateMap<FactorVerification, FactorVerificationSimpleViewModel>();
            CreateMap<FactorVerificationConfiguration, FactorVerificationConfigurationEditModel>();
            CreateMap<FactorVerificationConfiguration, FactorVerificationConfigurationListViewModel>()
                .ForMember(x => x.AccessLevelTitle,
                    b => b.MapFrom(c => c.AccessLevel.Title));
            CreateMap<FactorVerificationConfigurationEditModel, FactorVerificationConfiguration>();
            CreateMap<FactorVerificationConfiguration, FactorVerificationConfigurationSimpleViewModel>();
            CreateMap<FactorVerificationConfigurationRole, FactorVerificationConfigurationRoleEditModel>();
            CreateMap<FactorVerificationConfigurationRole, FactorVerificationConfigurationRoleListViewModel>()
                .ForMember(x => x.FactorVerificationConfigurationTitle,
                    b => b.MapFrom(c => c.FactorVerificationConfiguration.Title))
                .ForMember(x => x.RoleTitle,
                    b => b.MapFrom(c => c.Role.Title))
                .ForMember(x => x.AccessLevelTitle,
                    b => b.MapFrom(c => c.AccessLevel.Title));
            CreateMap<FactorVerificationConfigurationRoleEditModel, FactorVerificationConfigurationRole>();
            CreateMap<FactorVerificationConfigurationRole, FactorVerificationConfigurationRoleSimpleViewModel>();
            CreateMap<CustomerClub, CustomerClubEditModel>();

            CreateMap<CustomerClub, CustomerClubListViewModel>()

                .ForMember(x => x.RegistrationModeTypeTitle,
                    b =>
                        b.MapFrom(c => c.RegistrationModeTypeNavigation.Title));
            CreateMap<CustomerClub, CustomerClubMembershipVerifyViewModel>();
            CreateMap<CustomerClubEditModel, CustomerClub>();
            CreateMap<CustomerClub, CustomerClubSimpleViewModel>();


            CreateMap<FactorGroup, FactorGroupEditModel>();

            CreateMap<FactorGroup, FactorGroupListViewModel>()
                .ForMember(x => x.AccessLevelTitle,
                    b => b.MapFrom(c => c.AccessLevel.Title))
                .ForMember(x => x.FactorGroupStatus,
                    b => b.MapFrom(c => c.FactorGroupStatus))
                .ForMember(x => x.FactorGroupItemCount,
                    b => b.MapFrom(c => c.Factor.Count))
                .ForMember(x => x.AllowEdit, b => b.MapFrom(c => (c.FactorGroupStatus == (int)FactorGroupStatusTypeEnum.Initial)))
                .ForMember(x => x.AllowDelete, b => b.MapFrom(x => x.Factor.Count == 0))
                .ForMember(x => x.ChildsCount, b => b.MapFrom(x => x.InverseParent.Count()));

            CreateMap<FactorGroupEditModel, FactorGroup>();

            CreateMap<FactorGroup, FactorGroupSimpleViewModel>()
                .ForMember(x => x.AllowEdit
                    , b => b.MapFrom(c => c.FactorGroupStatus == (int)FactorGroupStatusTypeEnum.Initial))
                .ForMember(x => x.AllowDelete,
                    b => b.MapFrom(x => x.Factor.Count == 0));

            CreateMap<Issue, IssueEditModel>();

            CreateMap<Issue, IssueListViewModel>()
                .ForMember(x => x.IssueTypeTitle, b => b.MapFrom(c => c.IssueTypeNavigation.Title))
                .ForMember(x => x.ResolutionTypeTitle, b => b.MapFrom(c => c.ResolutionTypeNavigation.Title))
                .ForMember(x => x.PriorityTypeTitle, b => b.MapFrom(c => c.PriorityTypeNavigation.Title))
                .ForMember(x => x.IssueStateTypeTitle, b => b.MapFrom(c => c.IssueStateTypeNavigation.Title))
                .ForMember(x => x.AccessLevelTitle, b => b.MapFrom(c => c.AccessLevel.Title))
                .ForMember(x => x.CreationDateTime, b => b.MapFrom(c => c.CreationDate))
                .ForMember(x => x.AssigneeLastName, b => b.MapFrom(c => c.AssigneeNavigation.PersonNavigation.LastName))

            ;



            CreateMap<IssueEditModel, Issue>();

            CreateMap<Issue, IssueSimpleViewModel>();


            CreateMap<IssueLogWork, IssueLogWorkEditModel>()
                .ForMember(x => x.IssueType, b => b.MapFrom(c => c.Issue.IssueType));

            CreateMap<IssueLogWork, IssueLogWorkListViewModel>()
                .ForMember(x => x.IssueTitle,
                    b => b.MapFrom(c => c.Issue.Title))
                .ForMember(x => x.IssueTypeTitle, b => b.MapFrom(c => c.Issue.IssueTypeNavigation.Title))
                .ForMember(x => x.CreateByFullName,
                    b => b.MapFrom(c => string.Concat(c.CreateByNavigation.PersonNavigation.Name, " " + c.CreateByNavigation.PersonNavigation.LastName)));
            CreateMap<IssueLogWorkEditModel, IssueLogWork>();

            CreateMap<IssueLogWork, IssueLogWorkSimpleViewModel>();


            CreateMap<IssueParticipant, IssueParticipantEditModel>();

            CreateMap<IssueParticipant, IssueParticipantListViewModel>()
                .ForMember(x => x.IssueTitle,
                    b => b.MapFrom(c => c.Issue.Title))
                .ForMember(x => x.UserTitle,
                    b => b.MapFrom(c => c.User.UserName))
                .ForMember(x => x.AccessLevelTitle,
                    b => b.MapFrom(c => c.AccessLevel.Title))
                ;
            CreateMap<IssueParticipantEditModel, IssueParticipant>();

            CreateMap<IssueParticipant, IssueParticipantSimpleViewModel>();


            CreateMap<IssueTypeItem, IssueTypeItemEditModel>();

            CreateMap<IssueTypeItem, IssueTypeItemListViewModel>()
                .ForMember(x => x.IssueTitle,
                    b => b.MapFrom(c => c.Issue.Title))
                .ForMember(x => x.IssueTypeTitle, b => b.MapFrom(c => c.IssueTypeNavigation.Title)).ForMember(
                    x => x.AccessLevelTitle,
                    b => b.MapFrom(c => c.AccessLevel.Title))
                ;
            CreateMap<IssueTypeItemEditModel, IssueTypeItem>();

            CreateMap<IssueTypeItem, IssueTypeItemSimpleViewModel>();


            CreateMap<ValidHekmatCard, ValidHekmatCardEditModel>();

            CreateMap<ValidHekmatCard, ValidHekmatCardListViewModel>()
                .ForMember(x => x.HekmatCardTypeTitle, b => b.MapFrom(c => c.HekmatCardTypeNavigation.Title)).ForMember(
                    x => x.AccessLevelTitle,
                    b => b.MapFrom(c => c.AccessLevel.Title))
                ;
            CreateMap<ValidHekmatCardEditModel, ValidHekmatCard>();

            CreateMap<ValidHekmatCard, ValidHekmatCardSimpleViewModel>();


            CreateMap<IssueComment, IssueCommentEditModel>();

            CreateMap<IssueComment, IssueCommentListViewModel>()
                .ForMember(x => x.IssueTitle,
                    b => b.MapFrom(c => c.Issue.Title))
                .ForMember(x => x.AccessLevelTitle,
                    b => b.MapFrom(c => c.AccessLevel.Title))
                ;
            CreateMap<IssueCommentEditModel, IssueComment>();

            CreateMap<IssueComment, IssueCommentSimpleViewModel>();


            CreateMap<YashilScheduler, YashilSchedulerEditModel>();

            CreateMap<YashilScheduler, YashilSchedulerListViewModel>()
                .ForMember(x => x.OccuranceTypeTitle, b => b.MapFrom(c => c.OccuranceTypeNavigation.Title))
                .ForMember(x => x.AccessLevelTitle,
                    b => b.MapFrom(c => c.AccessLevel.Title))
                ;
            CreateMap<YashilSchedulerEditModel, YashilScheduler>();

            CreateMap<YashilScheduler, YashilSchedulerSimpleViewModel>()
                .ForMember(x => x.OccuranceTypeTitle, b => b.MapFrom(c => c.OccuranceTypeNavigation.Title));

            CreateMap<Vocher, VocherEditModel>();

            CreateMap<Vocher, VocherListViewModel>()
                                .ForMember(x => x.VocherTypeTitle,
                b => b.MapFrom(c => c.VocherType.Title))
                            .ForMember(x => x.AccessLevelTitle,
                b => b.MapFrom(c => c.AccessLevel.Title))
        ;
            CreateMap<VocherEditModel, Vocher>();

            CreateMap<Vocher, VocherSimpleViewModel>();


            CreateMap<VocherType, VocherTypeEditModel>();

            CreateMap<VocherType, VocherTypeListViewModel>()
                                .ForMember(x => x.AccessLevelTitle,
                b => b.MapFrom(c => c.AccessLevel.Title))
                                .ForMember(x => x.CompanyTitle,
                b => b.MapFrom(c => c.Company.Title))
                                .ForMember(x => x.VocherTypeId, b => b.MapFrom(c => c.Id));


            CreateMap<VocherTypeEditModel, VocherType>();

            CreateMap<VocherType, VocherTypeSimpleViewModel>();

            CreateMap<Vocher, VocherTypeVochersListViewModel>()
                .ForMember(x => x.Id,
                        b => b.MapFrom(c => c.Id))
                .ForMember(x => x.OrderId,
                        b => b.MapFrom(c => c.OrderItem
                        .Where(o => o.VocherId == c.Id)
                        .Select(x => x.OrderId).FirstOrDefault()))
                .ForMember(x => x.State,
                        b => b.MapFrom(c => c.OrderItem
                        .Where(o => o.VocherId == c.Id)
                        .Select(x => x.OrderId).FirstOrDefault() != 0 ? true : false));

            CreateMap<ItemGroup, ItemGroupEditModel>();

            CreateMap<ItemGroup, ItemGroupListViewModel>()
                .ForMember(x => x.ChildsCount, b => b.MapFrom(x => x.InverseParent.Count()));
            CreateMap<ItemGroup, WalletPaymentGroupListViewModel>()
                .ForMember(x => x.AllowEdit, b => b.MapFrom(c => (c.ItemGroupStatusType == (int)ItemGroupStatusTypeEnum.Initial) && c.InverseParent.Count == 0))
                .ForMember(x => x.AllowDelete, b => b.MapFrom(x => x.Payment.Count == 0 && x.InverseParent.Count == 0 && x.ItemGroupStatusType == (int)ItemGroupStatusTypeEnum.Initial))
                .ForMember(x => x.ChildsCount, b => b.MapFrom(x => x.InverseParent.Count()))
                .ForMember(x => x.ItemGroupingItemCount, b => b.MapFrom(c => c.Payment.Count));

            CreateMap<ItemGroup, CustomerClubPaymentInstallmentGroupListViewModel>()
                .ForMember(x => x.AllowEdit, b => b.MapFrom(c => (c.ItemGroupStatusType == (int)ItemGroupStatusTypeEnum.Initial) && c.InverseParent.Count == 0))
                .ForMember(x => x.AllowDelete, b => b.MapFrom(x => x.CustomerClubPaymentInstallment.Count == 0 && x.InverseParent.Count == 0 && x.ItemGroupStatusType == (int)ItemGroupStatusTypeEnum.Initial))
                .ForMember(x => x.ChildsCount, b => b.MapFrom(x => x.InverseParent.Count()))
                .ForMember(x => x.ItemGroupingItemCount, b => b.MapFrom(c => c.CustomerClubPaymentInstallment.Count));


            CreateMap<ItemGroup, RefundDetailGroupListViewModel>()
                .ForMember(x => x.AllowEdit, b => b.MapFrom(c => (c.ItemGroupStatusType == (int)ItemGroupStatusTypeEnum.Initial) && c.InverseParent.Count == 0))
                .ForMember(x => x.AllowDelete, b => b.MapFrom(x => x.RefundDetailRefundDetailGroup.Count == 0 && x.InverseParent.Count == 0 && x.ItemGroupStatusType == (int)ItemGroupStatusTypeEnum.Initial))
                .ForMember(x => x.ChildsCount, b => b.MapFrom(x => x.InverseParent.Count))
                .ForMember(x => x.ItemGroupingItemCount, b => b.MapFrom(c => c.RefundDetailRefundDetailGroup.Count));

            CreateMap<ItemGroup, RefundDetailForPaymentPaymentGroupListViewModel>()
                .ForMember(x => x.AllowEdit, b => b.MapFrom(c => (c.ItemGroupStatusType == (int)ItemGroupStatusTypeEnum.Initial) && c.InverseParent.Count == 0))
                .ForMember(x => x.AllowDelete, b => b.MapFrom(x => x.RefundDetailRefundDetailForPaymentGroup.Count == 0 && x.InverseParent.Count == 0 && x.ItemGroupStatusType == (int)ItemGroupStatusTypeEnum.Initial))
                .ForMember(x => x.ChildsCount, b => b.MapFrom(x => x.InverseParent.Count))
                .ForMember(x => x.ItemGroupingItemCount, b => b.MapFrom(c => c.RefundDetailRefundDetailForPaymentGroup.Count));



            CreateMap<ItemGroup, AdditionalUserPropGroupListViewModel>()
                .ForMember(x => x.AllowEdit, b => b.MapFrom(c => (c.ItemGroupStatusType == (int)ItemGroupStatusTypeEnum.Initial) && c.InverseParent.Count == 0))
                .ForMember(x => x.AllowDelete, b => b.MapFrom(x => x.RefundDetailRefundDetailGroup.Count == 0 && x.InverseParent.Count == 0 && x.ItemGroupStatusType == (int)ItemGroupStatusTypeEnum.Initial))
                .ForMember(x => x.ChildsCount, b => b.MapFrom(x => x.InverseParent.Count))
                .ForMember(x => x.ItemGroupingItemCount, b => b.MapFrom(c => c.AdditionalUserProp.Count));



            CreateMap<ItemGroupEditModel, ItemGroup>();

            CreateMap<ItemGroup, ItemGroupSimpleViewModel>();


            CreateMap<Payment, PaymentAssignListViewModel>()
                .ForMember(x => x.ReservationNumber,
                    b => b.MapFrom(c => c.ReservationNumber))
                .ForMember(x => x.OrderTitle,
                    b => b.MapFrom(c => c.Order.Title));

            CreateMap<Refund, RefundEditModel>();

            CreateMap<Refund, RefundListViewModel>()
                                .ForMember(x => x.OrderTitle,
                b => b.MapFrom(c => c.Order.Title))
                            .ForMember(x => x.AccessLevelTitle,
                b => b.MapFrom(c => c.AccessLevel.Title))
        ;
            CreateMap<RefundEditModel, Refund>();

            CreateMap<Refund, RefundSimpleViewModel>();


            CreateMap<RefundDetail, RefundDetailEditModel>();


            CreateMap<RefundDetail, RefundDetailListViewModel>()
                .ForMember(x => x.PaidDateInt,
                    b => b.MapFrom(c => c.Payment.PaidDateInt))
                .ForMember(x => x.InstallmentTrackingCode,
                    b => b.MapFrom(c => c.Payment.InstallmentTrackingCode))
                .ForMember(x => x.TerminalNumber,
                    b => b.MapFrom(c => c.Payment.TerminalNumber))
                .ForMember(x => x.RefundTime,
                    b => b.MapFrom(c => c.RefundTime))
                .ForMember(x => x.OldPaymentAmount,
                    b => b.MapFrom(c => (long)(c.OldPaymentAmount)))
                .ForMember(x => x.NewPaymentAmount,
                    b => b.MapFrom(c => (long)(c.NewPaymentAmount)))
                .ForMember(x => x.RefundAmount,
                    b => b.MapFrom(c => (long?)(c.RefundAmount)))
                .ForMember(x => x.ReservationNumber,
                    b => b.MapFrom(c => c.Payment.ReservationNumber))
                .ForMember(x => x.OrderTitle,
                    b => b.MapFrom(c => c.Refund.Order.Title))
                .ForMember(x => x.ApplicationName,
                    b => b.MapFrom(c => c.Refund.Order.Application.Title))
               .ForMember(x => x.RefundTypeTitle,
                b => b.MapFrom(c => c.RefundTypeNavigation.Title))
            .ForMember(x => x.RefundStatusTypeTitle,
                b => b.MapFrom(c => c.RefundStatusTypeNavigation.Title))
            .ForMember(x => x.NationalCode,
                b => b.MapFrom(c => c.Refund.Order.NationalCode))
            .ForMember(x => x.ProductTitle,
                b => b.MapFrom(c => string.Join(", ", c.Refund.Order.OrderItem.Where(x => !x.Deleted).Select(oi => oi.ProductSalesPlanning.Product.Title))))
            .ForMember(x => x.ProvinceTitle,
                b => b.MapFrom(c => string.Join(", ", c.Refund.Order.OrderItem.Where(x => !x.Deleted).Select(oi => oi.Representation.Address.City.Parent.Title))))
            .ForMember(x => x.CityTitle,
                b => b.MapFrom(c => string.Join(", ", c.Refund.Order.OrderItem.Where(x => !x.Deleted).Select(oi => oi.Representation.Address.City.Title))))
            .ForMember(x => x.CardNumber,
                b => b.MapFrom(c => c.Refund.Order.CardNumber))
                .ForMember(x => x.RefundAmountSetByFullName,
                    b => b.MapFrom(c => string.Concat(c.RefundAmountSetByNavigation.PersonNavigation.Name, " " + c.RefundAmountSetByNavigation.PersonNavigation.LastName)))
                .ForMember(x => x.RefundAmountSetDate,
                    b => b.MapFrom(c => c.RefundAmountSetDate))
                .ForMember(x => x.IsSetPersonBankAccount,
                    b => b.MapFrom(c => c.PersonBankAccountId.HasValue))
                .ForMember(x => x.RefundDetailGroupTitle,
                    b => b.MapFrom(c => string.Concat(c.RefundDetailGroup.Title, " " + c.RefundDetailGroup.Parent.Title)))
                .ForMember(x => x.RefundDetailForPaymentGroupTitle,
                    b => b.MapFrom(c => string.Concat(c.RefundDetailForPaymentGroup.Title, " " + c.RefundDetailForPaymentGroup.Parent.Title)))
                .ForMember(x => x.Confirmed,
                    b => b.MapFrom(c => c.Confirmed))
                .ForMember(x => x.AllowConfirm,
                    b => b.MapFrom(c => (!c.Confirmed.HasValue || !c.Confirmed.Value) && c.RefundType == (int)RefundTypeEnum.Hekmat));


            CreateMap<RefundDetail, RefundDetailFullInfoListViewModel>()
              .ForMember(x => x.PaidDateInt,
                  b => b.MapFrom(c => c.Payment.PaidDateInt))
              .ForMember(x => x.InstallmentTrackingCode,
                  b => b.MapFrom(c => c.Payment.InstallmentTrackingCode))
              .ForMember(x => x.TerminalNumber,
                  b => b.MapFrom(c => c.Payment.TerminalNumber))
                           .ForMember(x => x.OldPaymentAmount,
                  b => b.MapFrom(c => (long)(c.OldPaymentAmount)))
              .ForMember(x => x.NewPaymentAmount,
                  b => b.MapFrom(c => (long)(c.NewPaymentAmount)))
              .ForMember(x => x.RefundAmount,
                  b => b.MapFrom(c => (long?)(c.RefundAmount)))
              .ForMember(x => x.ReservationNumber,
                  b => b.MapFrom(c => c.Payment.ReservationNumber))
              .ForMember(x => x.OrderTitle,
                  b => b.MapFrom(c => c.Refund.Order.Title))
              .ForMember(x => x.ApplicationName,
                  b => b.MapFrom(c => c.Refund.Order.Application.Title))
             .ForMember(x => x.RefundTypeTitle,
              b => b.MapFrom(c => c.RefundTypeNavigation.Title))
          .ForMember(x => x.RefundStatusTypeTitle,
              b => b.MapFrom(c => c.RefundStatusTypeNavigation.Title))
          .ForMember(x => x.NationalCode,
              b => b.MapFrom(c => c.Refund.Order.NationalCode))
          .ForMember(x => x.ProductTitle,
              b => b.MapFrom(c => string.Join(", ", c.Refund.Order.OrderItem.Where(x => !x.Deleted).Select(oi => oi.ProductSalesPlanning.Product.Title))))
           .ForMember(x => x.CityTitle,
              b => b.MapFrom(c => string.Join(", ", c.Refund.Order.OrderItem.Where(x => !x.Deleted).Select(oi => oi.Representation.Address.City.Title))))
          .ForMember(x => x.CardNumber,
              b => b.MapFrom(c => c.Refund.Order.CardNumber))
              .ForMember(x => x.RefundAmountSetByFullName,
                  b => b.MapFrom(c => string.Concat(c.RefundAmountSetByNavigation.PersonNavigation.Name, " " + c.RefundAmountSetByNavigation.PersonNavigation.LastName)))
              .ForMember(x => x.RefundAmountSetDate,
                  b => b.MapFrom(c => c.RefundAmountSetDate))
              .ForMember(x => x.IsSetPersonBankAccount,
                  b => b.MapFrom(c => c.PersonBankAccountId.HasValue))
              .ForMember(x => x.RefundDetailGroupTitle,
                  b => b.MapFrom(c => string.Concat(c.RefundDetailGroup.Title, " " + c.RefundDetailGroup.Parent.Title)))
              .ForMember(x => x.RefundDetailForPaymentGroupTitle,
                  b => b.MapFrom(c => string.Concat(c.RefundDetailForPaymentGroup.Title, " " + c.RefundDetailForPaymentGroup.Parent.Title)))
              .ForMember(x => x.Confirmed,
                  b => b.MapFrom(c => c.Confirmed));




            CreateMap<RefundDetail, RefundDetailStatisticViewModel>()
               .ForMember(x => x.PaidDateInt,
                   b => b.MapFrom(c => c.Payment.PaidDateInt))
               .ForMember(x => x.InstallmentTrackingCode,
                   b => b.MapFrom(c => c.Payment.InstallmentTrackingCode))
               .ForMember(x => x.TerminalNumber,
                   b => b.MapFrom(c => c.Payment.TerminalNumber))
               .ForMember(x => x.OldPaymentAmount,
                   b => b.MapFrom(c => (long)(c.OldPaymentAmount)))
               .ForMember(x => x.NewPaymentAmount,
                   b => b.MapFrom(c => (long)(c.NewPaymentAmount)))
               .ForMember(x => x.RefundAmount,
                   b => b.MapFrom(c => (long?)(c.RefundAmount)))
               .ForMember(x => x.AnnouncedRefundAmount,
                   b => b.MapFrom(c => (long?)(c.AnnouncedRefundAmount)))
               .ForMember(x => x.ReservationNumber,
                   b => b.MapFrom(c => c.Payment.ReservationNumber))
               .ForMember(x => x.OrderTitle,
                   b => b.MapFrom(c => c.Refund.Order.Title))
               .ForMember(x => x.ApplicationName,
                   b => b.MapFrom(c => c.Refund.Order.Application.Title))
               .ForMember(x => x.NationalCode,
               b => b.MapFrom(c => c.Refund.Order.NationalCode))
           .ForMember(x => x.ProductTitle,
               b => b.MapFrom(c => string.Join(", ", c.Refund.Order.OrderItem.Where(x => x.Deleted).Select(oi => oi.ProductSalesPlanning.Product.Title))))
           .ForMember(x => x.ProvinceTitle,
               b => b.MapFrom(c => string.Join(", ", c.Refund.Order.OrderItem.Where(x => x.Deleted).Select(oi => oi.Representation.Address.City.Parent.Title))))
           .ForMember(x => x.CityTitle,
               b => b.MapFrom(c => string.Join(", ", c.Refund.Order.OrderItem.Where(x => x.Deleted).Select(oi => oi.Representation.Address.City.Title))))
           .ForMember(x => x.CardNumber,
               b => b.MapFrom(c => c.Refund.Order.CardNumber))
               .ForMember(x => x.AllowAnnouncedRefundAmount,
                   b => b.MapFrom(c => !c.RefundAmount.HasValue));

            CreateMap<RefundDetail, RefundDetailCustomListViewModel>()
                .ForMember(x => x.CreateByUserName, b
                       => b.MapFrom(c => c.CreateByNavigation.UserName))
                .ForMember(x => x.RefundCreatorName, b => b.MapFrom(c =>
                string.Concat(c.CreateByNavigation.PersonNavigation.Name, " " + c.CreateByNavigation.PersonNavigation.LastName)))
                .ForMember(x => x.AssigneeTitle, b => b.MapFrom(c =>
                    string.Concat(c.AssigneeNavigation.PersonNavigation.Name, " " + c.AssigneeNavigation.PersonNavigation.LastName)))
                .ForMember(x => x.BankAccountOwnerFullName, b => b.MapFrom(c => string.Concat(c.PersonBankAccount.Person.Name, " " + c.PersonBankAccount.Person.LastName)))
                .ForMember(x => x.Name, b => b.MapFrom(c => c.PersonBankAccount.Person.Name))
                .ForMember(x => x.LastName, b => b.MapFrom(c => c.PersonBankAccount.Person.LastName))
               .ForMember(x => x.PaidDateInt,
                   b => b.MapFrom(c => c.Payment.PaidDateInt))
               .ForMember(x => x.InstallmentTrackingCode,
                   b => b.MapFrom(c => c.Payment.InstallmentTrackingCode))
                .ForMember(x => x.OldPaymentAmount,
                   b => b.MapFrom(c => (long?)(c.OldPaymentAmount)))
              .ForMember(x => x.RefundAmount,
                  b => b.MapFrom(c => (long?)(c.RefundAmount)))
              .ForMember(x => x.OrderTitle,
                   b => b.MapFrom(c => c.Refund.Order.Title))
               .ForMember(x => x.ApplicationName,
                   b => b.MapFrom(c => c.Refund.Order.Application.Title))
              .ForMember(x => x.RefundTypeTitle,
               b => b.MapFrom(c => c.RefundTypeNavigation.Title))
           .ForMember(x => x.RefundStatusTypeTitle,
               b => b.MapFrom(c => c.RefundStatusTypeNavigation.Title))
           .ForMember(x => x.NationalCode,
               b => b.MapFrom(c => c.Refund.Order.NationalCode))
            .ForMember(x => x.CardNumber,
               b => b.MapFrom(c =>
                  c.RefundType == (int)RefundTypeEnum.Hekmat ?
                      c.Refund.Order.CardNumber :
                      c.Payment.CustomerRefNum))
             .ForMember(x => x.BankTypeTitle,
               b => b.MapFrom(c => c.PersonBankAccount.BankTypeNavigation.Title))
               .ForMember(x => x.BranchName,
               b => b.MapFrom(c => c.PersonBankAccount.BranchName))
                 .ForMember(x => x.AccountNumber,
               b => b.MapFrom(c => c.PersonBankAccount.AccountNumber))
                   .ForMember(x => x.ShebaNumber,
               b => b.MapFrom(c => c.PersonBankAccount.ShebaNumber))
                     .ForMember(x => x.RefundDetailGroupTitle,
               b => b.MapFrom(c => string.Concat(c.RefundDetailGroup.Parent.Title, "/" + c.RefundDetailGroup.Title)))
            .ForMember(x => x.MobileNumber,
               b => b.MapFrom(c => c.Refund.Order.RecipientMobileNumber ?? c.Refund.Order.User.UserName
               ))
                .ForMember(x => x.OrderId,
                    b => b.MapFrom(c => c.Payment.OrderId))
                .ForMember(x => x.PaymentId,
                    b => b.MapFrom(c => c.PaymentId))
                .ForMember(x => x.PaymentTitle,
                    b => b.MapFrom(c => c.Payment.ReservationNumber))
                .ForMember(x => x.IssueLogWorkCount,
                    b => b.MapFrom(c => c.Payment.Issue.Where(i => i.IssueType == (int)IssueTypeEnum.Refund).SelectMany(i => i.IssueLogWork).Count()))
                .ForMember(x => x.AllowArchive,
                    b => b.MapFrom(c => !c.IsArchived))
                .ForMember(x => x.AllowUnArchive,
                    b => b.MapFrom(c => c.IsArchived))
                .ForMember(x => x.AllowRemovePersonBankAccount,
                    b =>
                        b.MapFrom(c =>
                            c.PersonBankAccountId.HasValue && !c.RefundDetailForPaymentGroupId.HasValue));


            CreateMap<RefundDetailEditModel, RefundDetail>();

            CreateMap<RefundDetail, RefundDetailSimpleViewModel>();
            CreateMap<UploadedProductItem, UploadedProductItemEditModel>();

            CreateMap<ProductDetailDomainModel, ProductDetailViewModel>()
                .ForMember(x => x.TestBasePrice,
                    b => b.MapFrom(c => c.TestBasePrice))
                .ForMember(x => x.BookBasePrice,
                    b => b.MapFrom(c => c.BookBasePrice))
                .ForMember(x => x.TbaoCustomerClubDiscountPercentage,
                    b => b.MapFrom(c => c.TbaoCustomerClubDiscountPercentage))
                .ForMember(x => x.OtherCustomerClubDiscountPercentage,
                    b => b.MapFrom(c => c.OtherCustomerClubDiscountPercentage))
                .ForMember(x => x.DeliveryCost,
                    b => b.MapFrom(c => c.DeliveryCost));

            CreateMap<UploadedProductItem, UploadedProductItemListViewModel>();
            CreateMap<UploadedProductItem, UploadProductListViewModel>();

            CreateMap<UploadedProductItemEditModel, UploadedProductItem>();

            CreateMap<UploadedProductItem, UploadedProductItemSimpleViewModel>()
                .ForMember(x => x.StartDate, b => b.MapFrom(c => c.StartDate.Insert(4, "/").Insert(7, "/")))
                .ForMember(x => x.EndDate, b => b.MapFrom(c => c.EndDate.Insert(4, "/").Insert(7, "/")));

            CreateMap<FactorDeduction, FactorDeductionEditModel>();

            CreateMap<FactorDeduction, FactorDeductionListViewModel>()
                .ForMember(x => x.AccessLevelTitle,
                    b => b.MapFrom(c => c.AccessLevel.Title));

            CreateMap<FactorDeductionEditModel, FactorDeduction>();

            CreateMap<FactorDeduction, FactorDeductionSimpleViewModel>();
            CreateMap<OrderItemFullInfoView, OrderItemFullInfoViewViewModel>()
                .ForMember(x => x.Id, b => b.MapFrom(c => c.OrderItemId));


            CreateMap<HekmatCanceledTransaction, HekmatCanceledTransactionEditModel>();

            CreateMap<HekmatCanceledTransaction, HekmatCanceledTransactionListViewModel>()
                ;
            CreateMap<HekmatCanceledTransactionEditModel, HekmatCanceledTransaction>();

            CreateMap<HekmatCanceledTransaction, HekmatCanceledTransactionSimpleViewModel>();


            CreateMap<HekmatEffectlessCanceledTransaction, HekmatEffectlessCanceledTransactionEditModel>();

            CreateMap<HekmatEffectlessCanceledTransaction, HekmatEffectlessCanceledTransactionListViewModel>()
                ;
            CreateMap<HekmatEffectlessCanceledTransactionEditModel, HekmatEffectlessCanceledTransaction>();

            CreateMap<HekmatEffectlessCanceledTransaction, HekmatEffectlessCanceledTransactionSimpleViewModel>();


            CreateMap<HekmatEffectlessModifiedTransaction, HekmatEffectlessModifiedTransactionEditModel>();

            CreateMap<HekmatEffectlessModifiedTransaction, HekmatEffectlessModifiedTransactionListViewModel>()
                ;
            CreateMap<HekmatEffectlessModifiedTransactionEditModel, HekmatEffectlessModifiedTransaction>();

            CreateMap<HekmatEffectlessModifiedTransaction, HekmatEffectlessModifiedTransactionSimpleViewModel>();


            CreateMap<HekmatModifiedTransaction, HekmatModifiedTransactionEditModel>();

            CreateMap<HekmatModifiedTransaction, HekmatModifiedTransactionListViewModel>()
                ;
            CreateMap<HekmatModifiedTransactionEditModel, HekmatModifiedTransaction>();
            CreateMap<CustomerClubCard, CustomerClubCardListViewModel>();
            CreateMap<CustomerClubCard, CustomerClubCardEditModel>();
            CreateMap<CustomerClubCardEditModel, CustomerClubCard>();
            CreateMap<CustomerClubCard, CustomerClubCardSimpleViewModel>();
            CreateMap<HekmatModifiedTransaction, HekmatModifiedTransactionSimpleViewModel>();
            CreateMap<CustomerClubPaymentInstallment, CustomerClubPaymentInstallmentGroupItemListViewModel>()
                .ForMember(x => x.ReservationNumber,
                    b => b.MapFrom(c => c.Payment.ReservationNumber))
                .ForMember(x => x.ApplicationTitle,
                    b => b.MapFrom(c => c.Payment.Application.Title))
                .ForMember(x => x.CardNumber,
                    b => b.MapFrom(c => c.Payment.Order.CardNumber))
                .ForMember(x => x.CustomerClubCardCode,
                    b => b.MapFrom(c => c.Payment.Order.CustomerClubCard.Code))
                .ForMember(x => x.InstallmentCount,
                    b => b.MapFrom(c => c.Payment.InstallmentCount))
                .ForMember(x => x.YearNumber,
                    b => b.MapFrom(c => c.YearMonthNavigation.YearNumber))
                .ForMember(x => x.MonthTitle,
                    b => b.MapFrom(c => c.YearMonthNavigation.MonthNumberNavigation.Title))
                .ForMember(x => x.PaidDateInt,
                    b => b.MapFrom(c => c.Payment.PaidDateInt))
                ;
            CreateMap<PaymentComparisonWithHekmatApprovedTransactionDomainModel, PaymentComparisonWithHekmatApprovedTransactionViewModel>()
                .ForMember(x => x.PaymentTotalInt, b => b.MapFrom(c => c.PaymentTotalInt))
                .ForMember(x => x.HekmatPrice, b => b.MapFrom(c => c.HekmatPrice.Value))
                .ForMember(x => x.IsNotSameCardNumber, b =>
                b.MapFrom(c => c.PaymentMethodType == (int)PaymentMethodTypesEnum.Hekmat && c.CardNumberInt != c.HekmatCardNumberInt.Value))
                .ForMember(x => x.IsNotSameInstallmentTrackingCode, b =>
                b.MapFrom(c => c.PaymentMethodType == (int)PaymentMethodTypesEnum.Hekmat && c.InstallmentTrackingCode != c.HekmatInstallmentTrackingCode))
                .ForMember(x => x.IsNotSamePrice, b =>
                b.MapFrom(c => c.PaymentMethodType == (int)PaymentMethodTypesEnum.Hekmat && c.PaymentTotalInt != c.HekmatPrice.Value))
                .ForMember(x => x.PaymentMethodTitle, b => b.MapFrom(c => c.PaymentMethodTitle))
                .ForMember(x => x.NotExistInHekmat, b => b.MapFrom(c => c.PaymentMethodType == (int)PaymentMethodTypesEnum.Hekmat && c.HekmatInstallmentTrackingCode == null))
                .ForMember(x => x.AllowShowRefundForPaymentBtn, b => b.MapFrom(c => c.NewPaymentAmount.HasValue))

                ;

            CreateMap<SaleConfiguration, SaleConfigurationCustomViewModel>();
            CreateMap<UploadedFactorPayment, UploadedFactorPaymentEditModel>();

            CreateMap<UploadedFactorPayment, UploadedFactorPaymentListViewModel>()
                .ForMember(x => x.AppDocumentTitle,
                    b => b.MapFrom(c => c.AppDocument.Title))
                ;
            CreateMap<UploadedFactorPaymentEditModel, UploadedFactorPayment>();

            CreateMap<UploadedFactorPayment, UploadedFactorPaymentSimpleViewModel>();
            CreateMap<HekmatCanceledOrModifiedTransactionDomainModel, HekmatCanceledOrModifiedTransactionViewModel>();


            CreateMap<UploadedVoucher, UploadedVoucherEditModel>();

            CreateMap<UploadedVoucher, UploadedVoucherListViewModel>()
          .ForMember(x => x.AppDocumentTitle, b => b.MapFrom(c => c.AppDocument.Title))
        ;
            CreateMap<UploadedVoucherEditModel, UploadedVoucher>();
            CreateMap<UploadedVoucher, UploadedVoucherSimpleViewModel>();


            CreateMap<HekmatTransaction, HekmatTransactionEditModel>();
            CreateMap<HekmatTransaction, HekmatTransactionListViewModel>();
            CreateMap<HekmatTransaction, UploadedHekmatTransactionListViewModel>();
            CreateMap<HekmatTransactionEditModel, HekmatTransaction>();
            CreateMap<HekmatTransaction, HekmatTransactionSimpleViewModel>();
            CreateMap<HekmatTransactionCustomModel, HekmatTransactionListCustomViewModel>();
            CreateMap<CitySubsidyFactor, CitySubsidyFactorEditModel>();

            CreateMap<CitySubsidyFactor, CitySubsidyFactorListViewModel>()
                                .ForMember(x => x.CityTitle,
                b => b.MapFrom(c => c.City.Title))
                            .ForMember(x => x.AccessLevelTitle,
                b => b.MapFrom(c => c.AccessLevel.Title))
        ;
            CreateMap<CitySubsidyFactorEditModel, CitySubsidyFactor>();

            CreateMap<CitySubsidyFactor, CitySubsidyFactorSimpleViewModel>();
            CreateMap<RefundAssignedDomainModel, RefundAssignedUserListViewModel>()
               .ForMember(x => x.AssigneeFullName, b => b.MapFrom(c => c.PersonName))
               ;
            CreateMap<Certificate, CertificateEditModel>();

            CreateMap<Certificate, CertificateListViewModel>()
            .ForMember(x => x.Name, b => b.MapFrom(c =>
            !string.IsNullOrWhiteSpace(c.Name) ? CryptographyHelper.AesDecrypt(c.Name, true) : null))
            .ForMember(x => x.LastName, b => b.MapFrom(c =>
           !string.IsNullOrWhiteSpace(c.LastName) ? CryptographyHelper.AesDecrypt(c.LastName, true) : null))
            .ForMember(x => x.FatherName, b => b.MapFrom(c =>
            !string.IsNullOrWhiteSpace(c.FatherName) ? CryptographyHelper.AesDecrypt(c.FatherName, true) : null))
            .ForMember(x => x.LatinName, b => b.MapFrom(c =>
           !string.IsNullOrWhiteSpace(c.LatinName) ? CryptographyHelper.AesDecrypt(c.LatinName, true) : null))
            .ForMember(x => x.LatinLastName, b => b.MapFrom(c =>
           !string.IsNullOrWhiteSpace(c.LatinLastName) ? CryptographyHelper.AesDecrypt(c.LatinLastName, true) : null))
            .ForMember(x => x.NationalCode, b => b.MapFrom(c =>
           !string.IsNullOrWhiteSpace(c.NationalCode) ? CryptographyHelper.AesDecrypt(c.NationalCode, true) : null))
            .ForMember(x => x.MobileNumber, b => b.MapFrom(c =>
           !string.IsNullOrWhiteSpace(c.MobileNumber) ? CryptographyHelper.AesDecrypt(c.MobileNumber, true) : null))
             .ForMember(x => x.Tell, b => b.MapFrom(c =>
            !string.IsNullOrWhiteSpace(c.Tell) ? CryptographyHelper.AesDecrypt(c.Tell, true) : null))
             .ForMember(x => x.IdentityNumber, b => b.MapFrom(c =>
            !string.IsNullOrWhiteSpace(c.IdentityNumber) ? CryptographyHelper.AesDecrypt(c.IdentityNumber, true) : null))
                ;

            CreateMap<CertificateEditModel, Certificate>();

            CreateMap<Certificate, CertificateSimpleViewModel>();

            CreateMap<Certificate, CertificateStatisticViewModel>()
            .ForMember(x => x.FullName, b => b.MapFrom(c => string.Concat(c.Name, " " + c.LastName)));

            CreateMap<ItemGroup, ProductSalesPlanningGroupListViewModel>();
            CreateMap<SaleConfigurationAttribute, SaleConfigurationAttributeEditModel>();

            CreateMap<SaleConfigurationAttribute, SaleConfigurationAttributeListViewModel>()
                 .ForMember(x => x.SaleTypeTitle,
                    b => b.MapFrom(c => c.SaleTypeNavigation.Title))
                .ForMember(x => x.DiscountTypeTitle,
                    b => b.MapFrom(c => c.DiscountTypeNavigation.Title))
                .ForMember(x => x.CompanyTitle,
                b =>
                    b.MapFrom(c => c.SaleConfiguration.Company.Title))
                .ForMember(x => x.RepresentationTitle,
                    b =>
                        b.MapFrom(c => c.SaleConfiguration.Representation.Title))
                .ForMember(x => x.CustomerClubTitle,
                    b =>
                        b.MapFrom(c => c.CustomerClub.Title))
                .ForMember(x => x.SaleConfigurationTitle,
                    b =>
                        b.MapFrom(c => c.SaleConfiguration.Title));
            CreateMap<SaleConfigurationAttributeEditModel, SaleConfigurationAttribute>();

            CreateMap<SaleConfigurationAttribute, SaleConfigurationAttributeSimpleViewModel>();
            CreateMap<CategoryGroup, CategoryGroupEditModel>();

            CreateMap<CategoryGroup, CategoryGroupListViewModel>()
                .ForMember(x => x.AccessLevelTitle,
                    b => b.MapFrom(c => c.AccessLevel.Title))
                ;
            CreateMap<CategoryGroupEditModel, CategoryGroup>();

            CreateMap<CategoryGroup, CategoryGroupSimpleViewModel>();
            CreateMap<SubsidyGroup, SubsidyGroupEditModel>();

            CreateMap<SubsidyGroup, SubsidyGroupListViewModel>()
                .ForMember(x => x.AccessLevelTitle,
                    b => b.MapFrom(c => c.AccessLevel.Title))
                ;
            CreateMap<SubsidyGroupEditModel, SubsidyGroup>();

            CreateMap<SubsidyGroup, SubsidyGroupSimpleViewModel>();
            CreateMap<PreRegistrationEditModel, PreRegistration>();

            CreateMap<PreRegistration, PreRegistrationSimpleViewModel>();
            CreateMap<PreRegistration, PreRegistrationListViewModel>()
                .ForMember(x => x.UserName,
                    b => b.MapFrom(c => c.User.UserName))
                .ForMember(x => x.CreationDate,
                    b => b.MapFrom(c => c.CreationDate))
                .ForMember(x => x.ProductTitle,
                    b => b.MapFrom(c => c.ProductSalesPlanning.Product.Title))
                .ForMember(x => x.ProductSalesPlanningId,
                    b => b.MapFrom(c => c.ProductSalesPlanning.Id));

            CreateMap<CompanyProductSalesPlanningDomainModel, CompanyProductSalesPlanningViewModel>()
                .ForMember(x => x.ModificationDate,
                    b => b.MapFrom(c => c.ModificationDate.Value.ToString("yyyyMMddhhmmss")))
                .ForMember(x => x.IsEmployment,
                    b => b.MapFrom(c => (c.ProductType == (int)ProductTypeEnum.Employment) ? true : false));



            CreateMap<CategoryCustomerClub, CategoryCustomerClubEditModel>();

            CreateMap<CategoryCustomerClub, CategoryCustomerClubListViewModel>()
                .ForMember(x => x.CategoryTitle,
                    b => b.MapFrom(c => c.Category.Title));

            CreateMap<CategoryCustomerClubEditModel, CategoryCustomerClub>();

            CreateMap<CategoryCustomerClub, CategoryCustomerClubSimpleViewModel>();

            CreateMap<YashilCms, SliderSimpleViewModel>()
                .ForMember(x => x.Url, b => b.MapFrom(c => string.Concat("/api/PublicApi/GetMainPageSliderImage?id=", c.Id)))
                .ForMember(x => x.ModificationDate, b => b.MapFrom(c => c.ModificationDate.Value.ToString("yyyyMMddhhmmss")));

            CreateMap<CategoryCustomerClub, SliderSimpleViewModel>()
                .ForMember(x => x.Url, b => b.MapFrom(c => string.Concat("/api/PublicApi/GetCategoryCustomerClubSlider?id=", c.Id)))
                .ForMember(x => x.ModificationDate, b => b.MapFrom(c => c.ModificationDate.Value.ToString("yyyyMMddhhmmss")));

            CreateMap<ProductSalesPlanningDiscount, ProductSalesPlanningDiscountEditModel>();

            CreateMap<ProductSalesPlanningDiscount, ProductSalesPlanningDiscountListViewModel>()
                .ForMember(x => x.SaleConfigurationDiscountPercentage,
                    b => b.MapFrom(c =>
                        c.ProductSalesPlanning.SaleConfiguration.SaleConfigurationAttribute
                            .FirstOrDefault(
                                sca => sca.CustomerClubId == c.CustomerClubId)
                            .DiscountPercentage))
                          .ForMember(x => x.AccessLevelTitle,
                    b => b.MapFrom(c => c.AccessLevel.Title))
                ;
            CreateMap<ProductSalesPlanningDiscountEditModel, ProductSalesPlanningDiscount>();

            CreateMap<ProductSalesPlanningDiscount, ProductSalesPlanningDiscountSimpleViewModel>();

            CreateMap<AppEntityCustomerClub, AppEntityCustomerClubEditModel>();

            CreateMap<AppEntityCustomerClub, AppEntityCustomerClubListViewModel>()
                .ForMember(x => x.AppEntityTitle,
                    b => b.MapFrom(c => c.AppEntity.Title))
                .ForMember(x => x.AccessLevelTitle,
                    b => b.MapFrom(c => c.AccessLevel.Title))
                ;
            CreateMap<AppEntityCustomerClubEditModel, AppEntityCustomerClub>();

            CreateMap<AppEntityCustomerClub, AppEntityCustomerClubSimpleViewModel>();

            CreateMap<NewsStore, NewsStorePublicPageViewModel>()
                .ForMember(x => x.ModificationDate, b => b.MapFrom(c => c.ModificationDate.Value.ToString("yyyyMMddhhmmss")));

            CreateMap<CompanyGroup, CompanyGroupEditModel>();

            CreateMap<CompanyGroup, CompanyGroupListViewModel>()
                .ForMember(x => x.AccessLevelTitle,
                    b => b.MapFrom(c => c.AccessLevel.Title))
                ;
            CreateMap<CompanyGroupEditModel, CompanyGroup>();

            CreateMap<CompanyGroup, CompanyGroupSimpleViewModel>();

            CreateMap<CommonBaseData, EducationalFieldViewModel>();

            CreateMap<SiteVisit, SiteVisitEditModel>();
            CreateMap<SiteVisit, SiteVisitListViewModel>()


                .ForMember(x => x.AccessLevelTitle,
                    b => b.MapFrom(c => c.AccessLevel.Title))
                ;
            CreateMap<SiteVisitEditModel, SiteVisit>();
            CreateMap<SiteVisit, SiteVisitSimpleViewModel>();

            CreateMap<ProductTags, ProductTagsEditModel>();
            CreateMap<ProductTags, ProductTagsListViewModel>()


                    .ForMember(x => x.ProductTitle,
                    b => b.MapFrom(c => c.Product.Title))


                    .ForMember(x => x.TagTypeTitle,
                    b => b.MapFrom(c => c.TagTypeNavigation.Title))

                    .ForMember(x => x.AccessLevelTitle,
                    b => b.MapFrom(c => c.AccessLevel.Title))
                ;
            CreateMap<ProductTagsEditModel, ProductTags>();
            CreateMap<ProductTags, ProductTagsSimpleViewModel>();



            CreateMap<CorporationJobCo, JobOfferModel>()
                .ForMember(x => x.CorporationJobCoId, b => b.MapFrom(c => c.Id))
                .ForMember(x => x.CorporationId, b => b.MapFrom(c => c.CorporationId))
                .ForMember(x => x.CorporationTitle, b => b.MapFrom(c => c.Corporation.Title))
                .ForMember(x => x.CorporationJobCoTitle, b => b.MapFrom(c => c.Title))
                .ForMember(x => x.ProvienceTitle, b => b.MapFrom(c => c.City.Title))
                .ForMember(x => x.SalaryTypeTitle, b => b.MapFrom(c => c.SalaryTypeNavigation.Title))
                .ForMember(x => x.CooperationTypeTitle, b => b.MapFrom(c => c.CooperationTypeNavigation.Title));




            CreateMap<CorporationJobCo, JobOfferDetailViewModel>()
           .ForMember(x => x.CorporationJobCoId, b => b.MapFrom(c => c.Id))
           .ForMember(x => x.CorporationId, b => b.MapFrom(c => c.CorporationId))
           .ForMember(x => x.CorporationJobCoTitle, b => b.MapFrom(c => c.Title))
           .ForMember(x => x.CorporationJobCoDescription, b => b.MapFrom(c => c.Description))
           .ForMember(x => x.JobCoCategoryTitle, b => b.MapFrom(c => c.JobCo.JobCoCategory.Title))
           .ForMember(x => x.CityTitle, b => b.MapFrom(c => c.City.Title))
           .ForMember(x => x.CooperationTypeTitle, b => b.MapFrom(c => c.CooperationTypeNavigation.Title))
           .ForMember(x => x.MinimumWorkExperience, b => b.MapFrom(c => c.MinimumWorkExperience))
           .ForMember(x => x.SalaryTypeTitle, b => b.MapFrom(c => c.SalaryTypeNavigation.Title))
           .ForMember(x => x.CorporationDescription, b => b.MapFrom(c => c.Corporation.Description))
           .ForMember(x => x.JobCoSkills, b => b.MapFrom(c => c.JobCo.JobCoSkill.Select(skill
           => skill.Title).ToList()))
           .ForMember(x => x.GenderTypeTitle, b => b.MapFrom(c => c.CustomGenderTypeNavigation.Title))
           .ForMember(x => x.MilitaryServiceStatusTypeTitle, b => b.MapFrom(c => c.MilitaryServiceStatusTypeNavigation.Title))
           .ForMember(x => x.EducationGradeTypeTitle, b => b.MapFrom(c => c.EducationGradeTypeNavigation.Title))
           .ForMember(x => x.JobCoTagIds, b => b.MapFrom(c => c.JobCo.JobCoTags.Select(jobCoTag => jobCoTag.TagType).ToList()))
           .ForMember(x => x.JobCoTagTitles, b => b.MapFrom(c => c.JobCo.JobCoTags.Select(jobCoTag => jobCoTag.Title).ToList()));


            var dateTime = DateTime.Now.Date;
            CreateMap<Corporation, CorporationDetailHeaderViewModel>()
                    .ForMember(x => x.ActiveJobCoCount, b => b.MapFrom(c => c.CorporationJobCo.Where(corporationJobCo =>
                    corporationJobCo.IsActive && corporationJobCo.StartDate.Date <= dateTime &&
                                                    corporationJobCo.EndDate.Date >= dateTime).Count()));


            CreateMap<TaxCategorySaleConfiguration, TaxCategorySaleConfigurationEditModel>();
            CreateMap<TaxCategorySaleConfiguration, TaxCategorySaleConfigurationListViewModel>()


                    .ForMember(x => x.TaxCategoryTitle,
                    b => b.MapFrom(c => c.TaxCategory.Title))


                    .ForMember(x => x.SaleConfigurationTitle,
                    b => b.MapFrom(c => c.SaleConfiguration.Title))


                    .ForMember(x => x.FactorReportTypeTitle,
                    b => b.MapFrom(c => c.FactorReportTypeNavigation.Title))

                    .ForMember(x => x.AccessLevelTitle,
                    b => b.MapFrom(c => c.AccessLevel.Title))
                ;
            CreateMap<TaxCategorySaleConfigurationEditModel, TaxCategorySaleConfiguration>();
            CreateMap<TaxCategorySaleConfiguration, TaxCategorySaleConfigurationSimpleViewModel>();
            CreateMap<ProductSalesPlanningItemGroup, ProductSalesPlanningItemGroupEditModel>();

            CreateMap<ProductSalesPlanningItemGroup, ProductSalesPlanningItemGroupListViewModel>()
                .ForMember(x => x.ItemGroupTitle,
                    b => b.MapFrom(c => c.ItemGroup.Title))
                .ForMember(x => x.AccessLevelTitle,
                    b => b.MapFrom(c => c.AccessLevel.Title))
                ;
            CreateMap<ProductSalesPlanningItemGroupEditModel, ProductSalesPlanningItemGroup>();

            CreateMap<ProductSalesPlanningItemGroup, ProductSalesPlanningItemGroupSimpleViewModel>()
                .ForMember(x => x.ItemGroupTitle,
                    b => b.MapFrom(c => c.ItemGroup.Title))
                .ForMember(x => x.ItemGroupTitleWitParent,
                    b => b.MapFrom(c =>
                        string.Concat(c.ItemGroup.Parent.Title, " - " + c.ItemGroup.Title)))
                .ForMember(x => x.ItemGroupId,
                    b => b.MapFrom(c => c.ItemGroup.Id))
                .ForMember(x => x.ParentTitle,
                    b => b.MapFrom(c => c.ItemGroup.Parent.Title));

            CreateMap<ProductSalesPlanningItemGroup, ItemGroupAssignedToProductSalesPlanningViewModel>()
                .ForMember(x => x.Id,
                    b => b.MapFrom(c => c.ItemGroup.Id))
                .ForMember(x => x.Title,
                    b => b.MapFrom(c =>
                        string.Concat(c.ItemGroup.Parent.Title, " - " + c.ItemGroup.Title)));

            CreateMap<Subsidy, EmploymentSubsidyInformationViewModel>()
                .ForMember(x => x.SataCeilingPerHekmatCard,
                    b => b.MapFrom(c => c.SataCeilingPerHekmatCard))
                .ForMember(x => x.VezaratCeilingPerHekmatCard,
                    b => b.MapFrom(c => c.VezaratCeilingPerHekmatCard));

            CreateMap<ProductSalesPlanningBaseEditModel, ProductSalesPlanning>();
            CreateMap<ProductSalesPlanningBaseEditModel, ProductSalesPlanningEditModel>();
            CreateMap<ProductSalesPlanning, ProductSalesPlanningBaseEditModel>()
                .ForMember(x => x.CourseTypeTitle,
                    b =>
                        b.MapFrom(c => c.CourseStatusNavigation.Title));

            CreateMap<Order, OrdersWithWaitingForRequiredSupplierApprovalBeforePaymentViewModel>()
                .ForMember(x => x.Id, b => b.MapFrom(c => c.Id))
                .ForMember(x => x.OrderItemId, b =>
                    b.MapFrom(c => c.OrderItem.Select(oi => oi.Id).FirstOrDefault()))
                .ForMember(x
                        => x.ProductTitle,
                    b =>
                        b.MapFrom(c =>
                            c.OrderItem.Select(oi => oi.ProductSalesPlanning.Product.Title).FirstOrDefault()))
                .ForMember(x => x.RepresentationTitle,
                    b =>
                        b.MapFrom(c => c.OrderItem.Select(oi => oi.Representation.Title).FirstOrDefault()))
                .ForMember(x => x.UserName, b => b.MapFrom(c => c.User.UserName));

            CreateMap<UploadedCertificate, UploadedCertificateEditModel>();
            CreateMap<UploadedCertificate, UploadedCertificateListViewModel>()
                ;
            CreateMap<UploadedCertificateEditModel, UploadedCertificate>();
            CreateMap<UploadedCertificate, UploadedCertificateSimpleViewModel>();

            CreateMap<AppDocument, UploadedCertificateInfoViewModel>()
                .ForMember(x
                        => x.CreateByFullName,
                    b
                        => b.MapFrom(c =>
                            string.Concat(c.CreateByNavigation.PersonNavigation.Name, " "
                                + c.CreateByNavigation.PersonNavigation.LastName)));


            CreateMap<ProductGroup, ProductGroupEditModel>();
            CreateMap<ProductGroup, ProductGroupListViewModel>()


                .ForMember(x => x.AccessLevelTitle,
                    b => b.MapFrom(c => c.AccessLevel.Title))


                .ForMember(x => x.CategoryGroupTitle,
                    b => b.MapFrom(c => c.CategoryGroup.Title))
                ;
            CreateMap<ProductGroupEditModel, ProductGroup>();
            CreateMap<ProductGroup, ProductGroupSimpleViewModel>();

            CreateMap<Representation, RepresentationLocalDto>()
                .ForMember(x => x.Id, b => b.MapFrom(c => c.Id))
                .ForMember(x => x.Province, b => b.MapFrom(c => c.Address.City.Parent.Title))
                .ForMember(x => x.City, b => b.MapFrom(c => c.Address.City.ShortTitle))
                .ForMember(x => x.Code, b => b.MapFrom(c => c.Code))
                .ForMember(x => x.Title, b => b.MapFrom(c => c.Title))
                .ForMember(x => x.Founder, b => b.MapFrom(c => string.Concat(c.Founder.Name, " ", c.Founder.LastName)))
                .ForMember(x => x.LicenseExpireTime, b => b.MapFrom(c => c.LicenseExpireTime));


            CreateMap<Representation, RepresentationDetailDto>()
                .ForMember(x => x.Id, b => b.MapFrom(c => c.Id))
                .ForMember(x => x.Province, b => b.MapFrom(c => c.Address.City.Parent.Title))
                .ForMember(x => x.City, b => b.MapFrom(c => c.Address.City.ShortTitle))
                .ForMember(x => x.Code, b => b.MapFrom(c => c.Code))
                .ForMember(x => x.CompanyTitle, b => b.MapFrom(c => c.Company.Title))
                .ForMember(x => x.Title, b => b.MapFrom(c => c.Title))
                .ForMember(x => x.Founder, b => b.MapFrom(c => string.Concat(c.Founder.Name, " ", c.Founder.LastName)))
                .ForMember(x => x.LicenseExpireTime, b => b.MapFrom(c => c.LicenseExpireTime))
                .ForMember(x => x.FullAddress, b => b.MapFrom(c => c.Address.FullAddress))
                .ForMember(x => x.Email, b => b.MapFrom(c => c.Address.Email))
                .ForMember(x => x.Telephone, b => b.MapFrom(c => c.Address.Telephone))
                .ForMember(x => x.Latitude, b => b.MapFrom(c => c.Address.City.Latitude))
                .ForMember(x => x.Longitude, b => b.MapFrom(c => c.Address.City.Longitude));

            CreateMap<IGrouping<OngoingProductSalesPlanningGroupKey, ProductSalesPlanning>,
                    OngoingProductSalesPlanningViewModel>()
                .ForMember(x => x.Id,
                    b => b.MapFrom(c => c.Select(x => x.Id).FirstOrDefault()))
                .ForMember(x => x.CompanyTitle,
                    b => b.MapFrom(c => c.Select(x => x.Product.Company.Title).FirstOrDefault()))
                .ForMember(x => x.RepresentationTitle,
                    b => b.MapFrom(c => c.Select(x => x.Representation.Title).FirstOrDefault()))
                .ForMember(x => x.OngoingProductSalesPlanningCount,
                    b => b.MapFrom(c => c.Count()))
                .ForMember(x => x.CompanyId, b => b.MapFrom(c => c.Select(s => s.Product.CompanyId).FirstOrDefault()))
                .ForMember(x => x.RepresentationId,
                    b => b.MapFrom(c => c.Select(s => s.RepresentationId).FirstOrDefault()))
                .ForMember(x => x.ExamCount,
                    b =>
                        b.MapFrom(c =>
                            c.SelectMany(s =>
                                s.ExamCategory).SelectMany(s => s.Exam)
                                .Where(e => e.ExamType != (int)ExamTypeEnum.Survey).Count()))
                .ForMember(x => x.ParticipatedUserExamCount, b => b.MapFrom(c =>
                    c.SelectMany(s => s.ProductSalesPlanningStudent)
                        .SelectMany(pspStudent => pspStudent.UserExam)
                        .Count(ue => ue.StudentStartDate.HasValue)))
                .ForMember(x => x.PassedCount, b => b.MapFrom(c =>
                    c.SelectMany(s => s.ProductSalesPlanningStudent)
                        .SelectMany(p => p.UserExam)
                        .Count(ue => ue.UserScore.HasValue && ue.UserScore >= ue.Exam.AcceptScore)))
                .ForMember(x => x.FailedCount, b => b.MapFrom(c =>
                    c.SelectMany(s => s.ProductSalesPlanningStudent)
                        .SelectMany(p => p.UserExam)
                        .Count(ue => ue.UserScore.HasValue && ue.UserScore < ue.Exam.AcceptScore)));


            CreateMap<IGrouping<int, ProductSalesPlanning>, CompanyProductSalesPlanningComprehensiveReportViewModel>()
                .ForMember(x => x.Id,
                    b => b.MapFrom(c => c.Select(x => x.Id).FirstOrDefault()))
                .ForMember(x => x.ProductTitle,
                    b => b.MapFrom(c => c.Select(x => x.Product.Title).FirstOrDefault()))
                .ForMember(x => x.ProductSalesPlanningStudentCount,
                    b => b.MapFrom(c => c.SelectMany(x => x.ProductSalesPlanningStudent).Count()))
                .ForMember(x => x.VirtualClassCount,
                    b => b.MapFrom(c => c.SelectMany(x => x.VirtualClass).Count()))
                .ForMember(x => x.ExamCount,
                    b => b.MapFrom(c =>
                        c.SelectMany(x => x.ExamCategory)
                        .SelectMany(ec => ec.Exam).Where(x => x.ExamType !=
                                                              (int)ExamTypeEnum.Survey)
                        .Count()))
                .ForMember(x => x.HomeWorkCount,
                    b => b.MapFrom(c => c.SelectMany(x => x.HomeWorkCategory)
                        .SelectMany(hc => hc.HomeWork)
                        .Count()));

            CreateMap<ProductSalesPlanningDto, ProductSalesPlanningAssignedViewModel>()
                .ForMember(x => x.Id, b => b.MapFrom(c => c.Id))
                .ForMember(x => x.ProductTitle, b => b.MapFrom(c => c.ProductTitle));

            CreateMap<FactorItem, FactorItemPropertiesThatHaveNotEffectInFactorPriceViewModel>()
                .ForMember(x => x.OrderItemInsurancePercentage,
                    b => b.MapFrom(c => c.OrderItem.InsurancePercentage))
                .ForMember(x => x.OrderItemInsurancePrice,
                    b => b.MapFrom(c => c.OrderItem.InsurancePrice.ToString("N0")))
                .ForMember(x => x.OrderItemOtherDeductionsPrice,
                    b => b.MapFrom(c => c.OrderItem.OtherDeductionsPrice))
                .ForMember(x => x.OrderItemOtherDeductionsPercentage,
                    b => b.MapFrom(c => c.OrderItem.OtherDeductionsPercentage))
                .ForMember(x => x.SaleConfigurationInsurancePercentage,
                    b => b.MapFrom(c => c.OrderItem.SaleConfiguration.InsurancePercentage))
                .ForMember(x => x.IsIncludeInsurance,
                    b =>
                        b.MapFrom(c => c.IsIncludeInsurace));

            CreateMap<ProductSimpleDomainModel, ProductDetailViewModel>()
                .ForMember(x => x.Id, b =>
                    b.MapFrom(c => c.Id.ToString()))
                .ForMember(x => x.Code, b =>
                    b.MapFrom(c => c.Code.ToString()))
                .ForMember(x => x.Title, b =>
                    b.MapFrom(c => c.Title.ToString()))
                .ForMember(x => x.FullDescription, b
                    => b.MapFrom(c => c.FullDescription.ToString()))
                .ForMember(x => x.Published, b =>
                    b.MapFrom(c => c.Published.ToString()))
                .ForMember(x => x.CategoryTitle, b =>
                    b.MapFrom(c => c.CategoryTitle.ToString()))
                .ForMember(x => x.StartDate, b =>
                    b.MapFrom(c => c.StartDate.ToString()))
                .ForMember(x => x.EndDate, b =>
                    b.MapFrom(c => c.EndDate.ToString()))
                .ForMember(x => x.MaxCapacity, b =>
                    b.MapFrom(c => c.MaxCapacity.ToString()));

            CreateMap<ProductSalesPlanning, LastestProductSalesPlaningForShowInAnalyticsDashboard>()
                .ForMember(x => x.Title,
                    b => b.MapFrom(c => c.Product.Title))
                .ForMember(x => x.StartDate, b => b.MapFrom(c => c.StartDate.ToString().Insert(4, "/").Insert(7, "/")))
                .ForMember(x => x.EndDate, b => b.MapFrom(c => c.EndDate.ToString().Insert(4, "/").Insert(7, "/")))
                .ForMember(x => x.CompanyTitle, b => b.MapFrom(c => c.Product.Company.Title))
                .ForMember(x => x.RepresentationTitle, b => b.MapFrom(c => c.Representation.Title))
                .ForMember(x => x.IsTeacher, b => b.MapFrom(c => isTeacher));


            CreateMap<FactorPaymentInstallment, FactorPaymentInstallmentEditModel>();
            CreateMap<FactorPaymentInstallment, FactorPaymentInstallmentListViewModel>()

                .ForMember(x => x.FactorGroupItemTitle,
                    b => b.MapFrom(c => c.FactorGroupItem.Title))
                .ForMember(x => x.InstallmentPaymentStatusTypeTitle,
                    b => b.MapFrom(c => c.InstallmentPaymentStatusTypeNavigation.Title))
                .ForMember(x => x.AccessLevelTitle,
                    b => b.MapFrom(c => c.AccessLevel.Title));
            CreateMap<FactorPaymentInstallmentEditModel, FactorPaymentInstallment>();
            CreateMap<FactorPaymentInstallment, FactorPaymentInstallmentSimpleViewModel>()
                .ForMember(x => x.CategoryGroupTitle,
                    b => b.MapFrom(c => c.FactorGroupItem.Factor.CategoryGroup.Title))
                .ForMember(x => x.CompanyOrRepresentationTitle,
                    b => b.MapFrom(c =>
                        string.IsNullOrEmpty(c.FactorGroupItem.RepresentationTitle) ?
                            c.FactorGroupItem.CompanyTitle
                          : string.Concat(c.FactorGroupItem.CompanyTitle, " - ", c.FactorGroupItem.RepresentationTitle)
                            ))
                .ForMember(x => x.CompanyOrRepresentationCode,
                    b => b.MapFrom(c => c.FactorGroupItem.CompanyOrRepresentationCode))
                ;
            CreateMap<UploadedFactorInstallmentPaymentReferenceGroupDto, UploadedFactorInstallmentPaymentReferenceGroupViewModel>();
            CreateMap<UploadedFactorInstallmentPaymentReferenceDto, UploadedFactorInstallmentPaymentReferenceViewModel>();
            CreateMap<UploadedFactorInstallmentPayment, UploadedFactorInstallmentPaymentListViewModel>();


            //CreateMap<FactorPaymentInstallment, FactorPaymentInstallmentDto.FactorPaymentInstallmentForExportToExcelDto>()
            //    .ForMember(x => x.AccountNumber,
            //        b => b.MapFrom(c => c.FactorGroupItem.AccountNumber))
            //    .ForMember(x => x.BankName,
            //        b => b.MapFrom(c => c.FactorGroupItem.BankTypeTitle))
            //    .ForMember(x => x.ShebaNumber,
            //        b => b.MapFrom(c => c.FactorGroupItem.ShebaNumber))
            //    .ForMember(x => x.AccountOwner,
            //        b => b.MapFrom(c =>
            //            string.Concat(c.FactorGroupItem.PersonBankAccountName, " "
            //                + c.FactorGroupItem.PersonBankAccountLastName)
            //            ))
            //    .ForMember(x => x.FactorCode,
            //        b => b.MapFrom(c => c.FactorGroupItem.FactorCode))

            //    .ForMember(x => x.FactorCode,
            //        b => b.MapFrom(c => c.FactorGroupItem.FactorCode))

            //    .ForMember(x => x.RepresentationTitle,
            //        b => b.MapFrom(c => c.FactorGroupItem.CompanyTitle))

            //    .ForMember(x => x.RepresentationCode,
            //        b => b.MapFrom(c => c.FactorGroupItem.CompanyOrRepresentationCode))
            //    ;
        }
    }
}