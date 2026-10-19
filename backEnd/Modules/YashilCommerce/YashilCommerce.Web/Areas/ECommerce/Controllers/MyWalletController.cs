using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Yashil.Common.Core.Classes;
using Yashil.Common.Web.Infrastructure.Filters;
using YashilCommerce.Core.Services;

namespace YashilCommerce.Web.Areas.ECommerce.Controllers
{
    [Authorize]
    [ServiceFilter(typeof(SessionFixationFilterAsync))]
    public class MyWalletController : Controller
    {
        private readonly ICustomerWalletBalanceService _balances;
        private readonly IWalletBenefitsService _benefits;
        private readonly IRecipientService _recipients;
        private readonly IUserPrincipal _principal;
        private readonly ApplicationConfiguration _application;

        public MyWalletController(ICustomerWalletBalanceService balances, IWalletBenefitsService benefits,
            IRecipientService recipients, IUserPrincipal principal, ApplicationConfiguration application)
        {
            _balances = balances;
            _benefits = benefits;
            _recipients = recipients;
            _principal = principal;
            _application = application;
        }

        /// <summary>مانده کیف پول کاربر واردشده را نمایش می‌دهد.</summary>
        public async Task<IActionResult> Index()
        {
            var recipients = await _recipients.GetUserRecipients(_principal.Id)
                .Where(x => x.ApplicationId == _application.ApplicationId && !x.Deleted
                    && x.CardNumber != null && x.CardNumber != "")
                .Select(x => new { x.Id, x.Name, x.LastName }).ToListAsync();
            ViewBag.CardRecipients = recipients.Select(x => new SelectListItem
                { Value = x.Id.ToString(), Text = (x.Name + " " + x.LastName).Trim() }).ToList();
            return View(await _balances.GetCurrentUserBalanceAsync());
        }

        /// <summary>Programهای دارای اعتبار کاربر را برای انتخاب پرداخت ترکیبی برمی‌گرداند.</summary>
        [HttpGet]
        public async Task<IActionResult> Programs()
            => Json((await _balances.GetCurrentUserBalanceAsync()).Items);

        /// <summary>کارت گیرنده منتخب Checkout را تطبیق می‌دهد و اعتبار تازه را برمی‌گرداند.</summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MatchCheckoutRecipientCard(int recipientId)
        {
            var result = await _benefits.MatchRecipientCardAsync(recipientId);
            var programs = (await _balances.GetCurrentUserBalanceAsync()).Items;
            return Json(new { result.IsSuccess, result.IsReplay, result.Message, Programs = programs });
        }

        /// <summary>کارت گیرنده متعلق به خریدار را برای عضویت یا Grant برنامه تطبیق می‌دهد.</summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MatchRecipientCard(int recipientId)
        {
            var result = await _benefits.MatchRecipientCardAsync(recipientId);
            TempData["WalletMessage"] = result.Message ?? (result.IsSuccess
                ? "تطبیق کارت انجام شد." : "تطبیق کارت انجام نشد.");
            return RedirectToAction(nameof(Index));
        }

        /// <summary>Claim و Grant قطعی کوپن را برای کاربر واردشده اجرا می‌کند.</summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ClaimCoupon(string code)
        {
            var result = await _benefits.ClaimCouponAsync(code);
            TempData["WalletMessage"] = result.Message ?? (result.IsSuccess
                ? "کوپن ثبت شد." : "کوپن قابل استفاده نیست.");
            return RedirectToAction(nameof(Index));
        }
    }
}
