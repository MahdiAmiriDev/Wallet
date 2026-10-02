using System;
using Yashil.Core.Entities;
using YashilCommerce.Core.Interfaces;

namespace YashilCommerce.Infrastructure.Implementations
{
    public class PaymentGroup : IItemGroup<Payment>
    {
        public bool AllowAddChild(int itemGroupId)
        {
            throw new NotImplementedException();
        }
    }
}
