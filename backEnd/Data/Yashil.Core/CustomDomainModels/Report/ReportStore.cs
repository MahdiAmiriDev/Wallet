using System.Collections.Generic;

namespace Yashil.Core.CustomDomainModels.Report
{
    public class ReportStoreWithParam
    {
        public Dictionary<string, string> Params { get; set; }
        public string Format { get; set; }
    }
}
