using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class TabletData  
    {
public int Id { get; set; }
public string Nid { get; set; }
public string HekmatCardNo { get; set; }
public string MobileNumber { get; set; }
public long? Price { get; set; }
public string InstallmentCount { get; set; }
public string Sublier { get; set; }
public string Owner { get; set; }
    }
 }
