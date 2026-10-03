using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yashil.Infrastructure.Extensions
{
    public static class YashilQueryParamExtention
    {
        /// <summary>
        ///  این متد با توجه به کلید ارسالی مقادیر مربوط به آن کلید را از
        ///  Url        
        /// می خواند و سپس تبدیل به 
        /// int
        /// کرده 
        /// و در قالب لیست باز می گرداند
        /// </summary>
        /// <param name="key">کلید مقادیر در url</param>
        /// <returns>List<int?></returns>
        public static List<int?> GetIntValueByKey(this IQueryCollection queryParam , string key)
        {
            if (queryParam.TryGetValue(key, out var queryParams))
            {
                List<int?> ids = new();
                foreach (var item in queryParams.ToString().Split(","))
                {
                    if (int.TryParse(item, out var id))
                        ids.Add(id);
                }
                return ids;
            }

            return null;
        }
    }
}
