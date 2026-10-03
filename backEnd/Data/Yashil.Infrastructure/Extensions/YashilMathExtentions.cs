using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yashil.Infrastructure.Extensions
{
    public static class YashilMathExtentions
    {
        /// <summary>
        /// بزرگ ‌ترین مقسوم‌علیه مشترک
        /// بزرگ‌ترین عددی که هر دو عدد بدون باقیمانده بهش تقسیم می‌شوند.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static int Gcd(this int a, int b)
        {
            a = Math.Abs(a);
            b = Math.Abs(b);

            if (a == 0) return b;
            if (b == 0) return a;

            while (b != 0)
            {
                int temp = b;
                b = a % b;
                a = temp;
            }

            return a;
        }
    }
}
