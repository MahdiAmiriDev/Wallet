using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Yashil.Core.Protos;

namespace YashilCommerce.Core.Services
{
    public interface IHekmatService
    {
        /// <summary>
        /// شسناسه پرداخت را به درگاه ارسال می کند و آخرین وضعیت پرداخت حکمت را دریافت می کند.
        /// </summary>
        /// <param name="paymentId">شناسه پرداخت</param>
        /// <returns></returns>
        Task<PaymentStatusResultModel> GetPaymentStatusByPaymentIdAsync(int paymentId);

        /// <summary>
        /// ابتدا از سمت حکمت وضعیت این تراکنش چک می شود و بر اساس آن وضعیت پرداخت و سفارش بروز می گردد
        /// </summary>
        /// <param name="paymentId">شناسه پرداخت</param>
        /// <returns></returns>
        Task<bool> UpdateOrderAndPaymentStatusByPaymentIdAsync(int paymentId, int orderId);

        /// <summary>
        /// پرداخت های حکمت کاربر که در وضعیت پرداخت
        /// ارسال به درگاه یا منتظر تایید پرداخت باشد 
        /// از طریق سرویس درگاه پرداخت حکمت تعیین تکلیف می شود
        /// </summary>
        /// <param name="userId">شناسه کاربر</param>
        /// <returns></returns>
        Task<bool> UpdateUserHekmatTypeOrderAndPaymentStatusAsync(int userId);
        /// <summary>
        ///  این متد برای بارگذاری اطلاعات تراکنش هایی  که در سیستم حکمت مورد تایید قرار گرفته انداستفاده می شود
        /// معمولا روزهای اول و پانزدهم هرماه سی دی تراکنش های تایید شده در سیستم حکمت توسط مسئول بخش ارتباط با حکمت که در حال حاضر
        /// آقای حاجی عزیزی می باشد در قالب سی دی تحویل گرفته شده و این سی دی شامل دو شیت می باشد
        /// شیت اول تراکنش های اعتباری و شیت دوم تراکنش های اقساطی می باشد
        /// شیت تراکنش های اقساطی توسط این متد بارگذاری می شود
        /// برای خواندن فایل اکسل از oledb استفاده شده است
        /// فایل اکسل توسط این متد UploadProductFile گرفته شده و فایل در مسیر فیزیکی
        /// wwwroot/UploadHekmatExcel
        /// ذخیره می شود
        /// فایل اکسل توسط تامین کننده Microsoft.ACE.OLEDB.12.0 خوانده می شود
        /// توسط oledb به فایل اکسل کانکشن زده 
        ///  و فایل اکسل  باز شده و شیت ایندکس 1 آن خوانده شده و در دیتاتیبل ذخیره می شود
        /// سپس این دیتاتیبل توسط sqlBulkCopy با رشته اتصال به پایگاه داده و در جدول معین شده در
        /// DestinationTableName
        /// توسط sqlBulkCopy.DestinationTableName
        /// ذخیره میگردد
        /// </summary>
        /// <returns></returns>
        Task<bool> BulkCopyHekmatApprovedDataAsync(IFormFile hekmatTransactionFile, string title);

        /// <summary>
        /// این متد برای بارگذاری فایل های عودت وجه داده شده اعلامی از سوی حکمت استفاده می شود   
        ///  اکسل از oledb استفاده شده است
        /// فایل اکسل توسط تامین کننده Microsoft.ACE.OLEDB.12.0 خوانده می شود
        /// توسط oledb به فایل اکسل کانکشن زده 
        ///  و فایل اکسل  باز شده و شیت ایندکس 0و1و2و3 آن خوانده شده و در دیتاتیبل ذخیره می شود
        /// سپس این دیتاتیبل توسط sqlBulkCopy با رشته اتصال به پایگاه داده و در جدول معین شده در
        /// DestinationTableName
        /// توسط sqlBulkCopy.DestinationTableName
        /// ذخیره میگردد 
        /// </summary>
        /// <returns></returns>
        Task<bool> BulkCopyHekmatRefundedTransactionsDataAsync(IFormFile hekmatRefundFile);

        /// <summary>
        /// یک instance از سرویس درگاه حکمت را ایجاد میکند و برمیگرداند
        /// </summary>
        /// <returns></returns>
        public HekmatIPG.HekmatIPGClient CreateHekmatIpgService();

        /// <summary>
        /// یک درخواست تراکنش به درگاه حکمت ارسال میکند
        /// و پاسخ را برمیگرداند
        /// </summary>
        /// <param name="reservationNumber">  شناسه پرداخت سایت </param>
        /// <param name="amount">قیمت به ریال</param>
        /// <param name="installmentCount">تعداد اقساط</param>
        /// <param name="cardNumber">شماره کارت</param>
        /// <param name="additionalData">اطلاعات اضافی</param>
        /// <param name="isHybrid">پرداخت ترکیبی (نقدی اقساطی) است؟</param>
        /// <returns></returns>
        Task<PaymentRequestResultModel> RequestPaymentInHekmatIpgAsync(string reservationNumber, int amount,
            int installmentCount, string cardNumber = null , string additionalData = null, bool isHybrid = false);
    }
}
