using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class TempHekmatFile  
    {
public int Id { get; set; }
public string CardNo { get; set; }
public string Amount { get; set; }
public string Installment { get; set; }
public int? Error { get; set; }
public string Msg { get; set; }
public string Balance { get; set; }
public string TransSeq { get; set; }
public int? CodeIsiran { get; set; }
public DateTime? Date { get; set; }
public string Nid { get; set; }
public bool? IsDone { get; set; }
public string Err { get; set; }
public bool? IsDuplicate { get; set; }
    }
 }
