using System.Collections.Concurrent;

namespace YashilCommerce.Infrastructure.Implementations
{
    public class PaymentTransactionUtil
    {
        public PaymentTransactionUtil()
        {
            PaymentTransactionStatus = new ConcurrentDictionary<string, bool>();
        }
        public ConcurrentDictionary<string, bool> PaymentTransactionStatus { get; set; }
    }
}
