using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class TempMyfile  
    {
public int Id { get; set; }
public string CardNum { get; set; }
public string Nid { get; set; }
public int? Price { get; set; }
public int? Installment { get; set; }
public int? Status { get; set; }
public string Msg { get; set; }
public int? HekmatFileId { get; set; }
public DateTime? Date { get; set; }
public bool? IsDuplicate { get; set; }
    }
 }
