using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Yashil.Common.Core.Classes;
using Yashil.Common.SharedKernel.Helpers;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;
using Yashil.Common.Web.Infrastructure.Filters;
using Yashil.Core;
using Yashil.Core.CustomViewModels;
using Yashil.Core.Entities;
using Yashil.Core.Enums;
using Yashil.Infrastructure.Extensions;
using YashilBaseInfo.Core.Services;
using YashilCommerce.Core.Classes;
using YashilCommerce.Core.Services;
using YashilCommerce.Web.Areas.ECommerce.ViewModels;
using YashilUserManagement.Core.Services;

namespace YashilCommerce.Web.Areas.ECommerce.Controllers
{
    [Authorize]
    [ServiceFilter(typeof(SessionFixationFilterAsync))]
    public class RecipientController : YashilMvcBaseController<Recipient>
    {
        private readonly IRecipientService _recipientService;
        private readonly IMapper _mapper;
        private readonly StoreConfiguration _storeConfiguration;
        private readonly ICityService _cityService;
        private readonly IUserPrincipal _userPrincipal;
        private readonly IAdditionalUserPropService _additionalUserPropService;
        private readonly IAddressService _addressService;
        private readonly ICommonBaseDataService _commonBaseDataService;

        public RecipientController(
            ApplicationConfiguration applicationConfiguration,
            IMapper mapper,
            IYashilLogger<Recipient> logger,
            IPermissionService permissionService,
            IRecipientService recipientService,
            ICityService cityService,
            IUserPrincipal userPrincipal,
            StoreConfiguration storeConfiguration,
             IAdditionalUserPropService additionalUserPropService, IAddressService addressService, ICommonBaseDataService commonBaseDataService) : base(applicationConfiguration, mapper, logger, permissionService)
        {
            _mapper = mapper;
            _recipientService = recipientService;
            _cityService = cityService;
            _userPrincipal = userPrincipal;
            _storeConfiguration = storeConfiguration;
            _additionalUserPropService = additionalUserPropService;
            _addressService = addressService;
            _commonBaseDataService = commonBaseDataService;
        }
        [AllowAnonymous]
        public async Task<ActionResult> EditRecipient(int? recipientId)
        {
            if (_userPrincipal == null || _userPrincipal.Id == 0)
            {
                return Accepted(string.Empty);
            }
            var recipientPresentationEditModel = new RecipientPresentationEditModel();

            var userAdditionalUserProp = await _additionalUserPropService.GetUserAdditionalUserPropAsync(_userPrincipal.Id);
            var membershipTypes =
                _recipientService.GetMembershipTypes(userAdditionalUserProp.HekmatBased, userAdditionalUserProp.SupportSubsidy);
            var membershipTypeList = membershipTypes.ProjectTo<CommonBaseDataSimpleViewModel>(_mapper.ConfigurationProvider).ToList();

            var genderTypes = _recipientService.GetGenderTypes();
            var genderTypeList = genderTypes.ProjectTo<CommonBaseDataSimpleViewModel>(_mapper.ConfigurationProvider).ToList();


            if (recipientId != null)
            {
                var recipient = _recipientService.GetUserRecipient(recipientId.Value, true);
                if (recipient != null)
                {
                    recipientPresentationEditModel = recipient.ProjectTo<RecipientPresentationEditModel>(_mapper.ConfigurationProvider).FirstOrDefault();
                    if (recipientPresentationEditModel != null)
                    {
                        recipientPresentationEditModel.Name = !string.IsNullOrEmpty(recipientPresentationEditModel.Name)
                            ? CryptographyHelper.AesDecrypt(recipientPresentationEditModel.Name)
                            : recipientPresentationEditModel.Name;
                        recipientPresentationEditModel.LastName =
                            !string.IsNullOrEmpty(recipientPresentationEditModel.LastName)
                                ? CryptographyHelper.AesDecrypt(recipientPresentationEditModel.LastName)
                                : recipientPresentationEditModel.LastName;
                        recipientPresentationEditModel.FullAddress = _addressService.GetFullAddress(recipientPresentationEditModel.FullAddress,
                            recipientPresentationEditModel.IsEncrypted);
                        recipientPresentationEditModel.BirthDateStr =
                            recipientPresentationEditModel.BirthDate.HasValue ?
                            recipientPresentationEditModel.BirthDate.Value.ToString() : "";

                    }
                }
            }
            else
            {

                if (userAdditionalUserProp.HekmatBased || userAdditionalUserProp.SupportSubsidy)
                {
                    recipientPresentationEditModel.MembershipType = (int)MembershipTypesEnum.Vezarat;
                }
            }
            // get all cities
            var cities = _cityService.GetCities();
            var citiesViewModel = cities.ProjectTo<CityViewModel>(_mapper.ConfigurationProvider).ToList();

            // fill some property of viewmodel
            if (recipientPresentationEditModel != null)
            {
                recipientPresentationEditModel.HekmatBased = userAdditionalUserProp.HekmatBased;
                recipientPresentationEditModel.SubsidyBased = userAdditionalUserProp.SupportSubsidy;
                recipientPresentationEditModel.AddressBased = userAdditionalUserProp.AddressBased;
                recipientPresentationEditModel.CityListModelViewModel = citiesViewModel;
                recipientPresentationEditModel.MembershipTypes = membershipTypeList;
                recipientPresentationEditModel.GenderTypes = genderTypeList;
                recipientPresentationEditModel.RequiredRecipientCardNumber =
                    _storeConfiguration.RequireRecipientCardNumber;
                recipientPresentationEditModel.ShowRecipientCardNumber = !_storeConfiguration.LimitedToCustomerClub;
                recipientPresentationEditModel.RelationshipWithBuyerList = await _commonBaseDataService
                    .GetByKeyName(CommonBaseTypeEnum.RelationshipWithBuyer.ToString())
                    .ProjectTo<CommonBaseDataSimpleViewModel>(_mapper.ConfigurationProvider).ToListAsync();
            }
            return View(recipientPresentationEditModel);
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult VerifyCardNumber(int? MembershipType, string cardNumber)
        {
            cardNumber = cardNumber.ToEnglishDigit();
            if (MembershipType.HasValue)
            {
                if (MembershipType.Value == (int)MembershipTypesEnum.Sata || MembershipType.Value == (int)MembershipTypesEnum.Vezarat)
                {
                    if (!string.IsNullOrWhiteSpace(cardNumber))
                        return Json(true);
                    else
                        return Json(false);
                }
                else
                {
                    if (_storeConfiguration.RequireRecipientCardNumber)
                    {
                        if (!string.IsNullOrWhiteSpace(cardNumber))
                            return Json(true);
                        else
                            return Json(false);
                    }
                    else
                    {
                        return Json(true);
                    }

                }
            }
            return Json(false);

        }
    }
}
