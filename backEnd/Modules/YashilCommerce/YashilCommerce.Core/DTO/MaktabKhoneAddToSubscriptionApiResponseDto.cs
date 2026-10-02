

using Newtonsoft.Json;

namespace YashilCommerce.Core.DTO
{
    /// <summary>
    /// مدل پاسخ api
    /// مربوط به افزودن کاربر به مشترک های مکتب خونه
    /// </summary>
    public class MaktabKhoneAddToSubscriptionApiResponseDto
    {
        public bool IsSuccessStatusCode { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("Subscription_id")]
        public int SubscriptionId { get; set; }
        [JsonProperty("error")]
        public bool Error { get; set; }
    }

    /// <summary>
    /// مدل پاسخ Api
    /// مربوط به دریافت توکن از مکتب خونه
    /// </summary>
    public class MaktabKhoneAuthenticationApiResponseDto
    {
        public bool IsSuccess { get; set; }
        [JsonProperty("status")]
        public string Message { get; set; }
        [JsonProperty("token")]
        public string Token { get; set; }

        public string RedirectUrl { get; set; }
    }
}
