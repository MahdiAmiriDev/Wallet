namespace Yashil.Core.CustomViewModels.AnsarPaymentModels
{
    public class GenerateTokenResponseModel
    {
        public string Result { get; set; }
        public long ExpirationDate { get; set; }
        public string Token { get; set; }
        public string ChannelId { get; set; }
        public string UserId { get; set; }
    }
}
