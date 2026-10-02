using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;
using Yashil.Core.CustomDomainModels.BankServiceModels;

namespace Yashil.Core
{
    public class BankService
    {
        public static async Task<BankAccountInquiry> GetBankAccountInformationAsync(string cardNumber, bool isDevelopment = false)
        {
            //TODO Check And Refactor
            //var responseContent = "{'trackId':'1f63d3a7-e795-4de3-8d49-1f523a98f587','result':{'deposit':'540300483601','bankName':'تست نام بانک','card':'6274121171905375','depositOwners':'تستی نام و نام خانوادگی','IBAN':'IR760150000000540300483601','depositStatus':'02'},'status':'DONE'}";

            //if (!isDevelopment)
            //{
            //    try
            //    {
            //        var client = new RestClient($"https://api.apieco.ir/cardToIban?card={cardNumber}&version=2");


            //        var request = new RestRequest();


            //        request.AddHeader("Content-Type", "application/json");
            //        request.AddHeader("apieco-key", "hzLOL52VMJqC02dVb6DSSZ1dcjL54CdA");
            //        request.AddHeader("x-apieco-host", "vytwks2zav.apieco.ir");
            //        request.AddHeader("host", "api.apieco.ir");
            //        request.AddHeader("protocol", "https");
            //        request.AddHeader("Cookie", "boomrangid=s%3AsyXGEM__4xmxJJ-tNyGeY07E8wxMlsRA.69%2Fos8hdKZXrSSp5WSps4zLywLtYImldVoG4B1REJ8A");


            //        var response = await client.ExecuteAsync(request);
            //        responseContent = response.Content;
            //    }
            //    catch (Exception e)
            //    {
            //        return null;
            //    }
            //}
            //return JsonConvert.DeserializeObject<BankAccountInquiry>(responseContent);
            return null;

        }
    }
}
