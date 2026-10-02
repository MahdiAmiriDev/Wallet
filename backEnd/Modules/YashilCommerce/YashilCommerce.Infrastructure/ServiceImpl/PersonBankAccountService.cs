using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Yashil.Common.Core.Classes;
using Yashil.Common.Core.Interfaces;
using Yashil.Common.Infrastructure.Implementations;
using Yashil.Core.Entities;
using Yashil.Common.Core.AppEvents;
using Yashil.Core;
using Yashil.Core.CustomDomainModels.Commerce;
using Yashil.Core.Enums;
using YashilBaseInfo.Core.Services;
using YashilCommerce.Core.Repositories;
using YashilCommerce.Core.Services;

namespace YashilCommerce.Infrastructure.ServiceImpl
{
    public class PersonBankAccountService : GenericService<PersonBankAccount, int>, IPersonBankAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEventPublisher _eventPublisher;
        private readonly IPersonBankAccountRepository _personBankAccountRepository;
        private readonly IUserPrincipal _userPrincipal;
        private readonly ICommonBaseDataService _commonBaseDataService;
        private readonly IHostingEnvironment _hostingEnvironment;

        public PersonBankAccountService(IUnitOfWork unitOfWork,
            IPersonBankAccountRepository personBankAccountRepository, IUserPrincipal userPrincipal,
            IEventPublisher eventPublisher, ICommonBaseDataService commonBaseDataService, IHostingEnvironment iHostingEnvironment)
            : base(unitOfWork, personBankAccountRepository, userPrincipal, eventPublisher)
        {
            _unitOfWork = unitOfWork;
            _personBankAccountRepository = personBankAccountRepository;
            _userPrincipal = userPrincipal;
            _eventPublisher = eventPublisher;
            _commonBaseDataService = commonBaseDataService;
            _hostingEnvironment = iHostingEnvironment;
        }

        public IQueryable<PersonBankAccount> GetByCustomFilter(int? personId, int? bankType, int? representationId)
        {
            return _personBankAccountRepository.GetByCustomFilter(personId, bankType, representationId);
        }

        public async Task<PersonBankAccountCustomDomainModel> GetByCardNumberAsync(string cardNumber)
        {
            var personBankAccount = await _personBankAccountRepository.GetByCardNumber(cardNumber).Select(x =>
                new PersonBankAccountCustomDomainModel
                {
                    AccountNumber = x.AccountNumber,
                    BankType = x.BankType,
                    BranchName = x.BranchName,
                    CardNumber = x.CardNumber,
                    FullName = x.Person.Name + " " + x.Person.LastName,
                    ShebaNumber = x.ShebaNumber,
                    Id = x.Id,
                    PersonId = x.PersonId
                }).FirstOrDefaultAsync();

            if (personBankAccount == null)
            {
                // سعی می کنیم با استفاده از سرویس مربوطه اطلاعات حساب شخص را از روی شماره کارت استخراج کنیم
                personBankAccount = new PersonBankAccountCustomDomainModel();
                var bankAccountInquiry = await BankService.GetBankAccountInformationAsync(cardNumber, _hostingEnvironment.IsDevelopment());
                if (bankAccountInquiry != null && bankAccountInquiry.Status == "DONE")
                {
                    var bankName = bankAccountInquiry.Result.BankName;
                    var bankList = _commonBaseDataService.GetByKeyName(CommonBaseTypeEnum.BankType.ToString());
                    var bankCommonData = await bankList.Select(x => new { x.Id, x.Title }).FirstOrDefaultAsync(x => x.Title == bankName);
                    int bankType;
                    if (bankCommonData == null)
                    {
                        var commonBaseData = new CommonBaseData
                        {
                            CommonBaseTypeId = (int)CommonBaseTypeEnum.BankType,
                            CreateBy = _userPrincipal.Id,
                            CreationDate = DateTime.Now,
                            CreatorOrganizationId = _userPrincipal.OrganizationId,
                            Title = bankName,
                            Value = 1
                        };
                        await _commonBaseDataService.AddAsync(commonBaseData);
                        await SaveChangeAsync();
                        bankType = commonBaseData.Id;
                    }
                    else
                    {
                        bankType = bankCommonData.Id;
                    }

                    if (int.TryParse(bankAccountInquiry.Result.DepositStatus, out int depositStatus))
                    {
                        var depositStatusBaseData = await _commonBaseDataService.GetByValueAsync(CommonBaseTypeEnum.DepositStatus, depositStatus);
                        if (depositStatusBaseData != null)
                        {
                            personBankAccount.DepositStatus = depositStatusBaseData.Value;
                        }
                    }

                    personBankAccount.BankType = bankType;
                    personBankAccount.CardNumber = bankAccountInquiry.Result.Card;
                    personBankAccount.ShebaNumber = bankAccountInquiry.Result.IBAN;
                    personBankAccount.AccountNumber = bankAccountInquiry.Result.Deposit;
                    personBankAccount.CardNumber = bankAccountInquiry.Result.Card;
                    personBankAccount.FullName = bankAccountInquiry.Result.DepositOwners;

                }
            }

            return personBankAccount;
        }

        public IQueryable<PersonBankAccount> GetIncludePerson(int id)
        {
            return _personBankAccountRepository.GetIncludePerson(id);
        }

        public IQueryable<PersonBankAccount> FindById(int id, bool readOnly = false)
        {
            return _personBankAccountRepository.FindById(id, readOnly);
        }
    }
}
