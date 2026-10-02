using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Reflection;

namespace Yashil.Core.DefaultContractResolvers
{
    /// <summary>
    /// این کلاس جهت استفاده به هنگام تبدیل کردن یک آبجکت به جی سون کاربرد دارد
    /// به این منظور که فیلد _ignoreProps برای دریافت نام پروپرتی های که می خواهیم 
    /// به هنگام تبدیل آبجک به جی سون ایگنور شوند و در فرآیند تبدیل شرکت نکنند استفاده می شود
    /// </summary>
    public class ContractDefaultResolver : DefaultContractResolver
    {
        private readonly HashSet<string> _ignoreProps;

        public ContractDefaultResolver(params string[] propNamesToIgnore)
        {
            _ignoreProps = new HashSet<string>(propNamesToIgnore);
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);

            if (_ignoreProps.Contains(property.PropertyName))
                property.ShouldSerialize = state => false;


            return property;
        }
    }

}
