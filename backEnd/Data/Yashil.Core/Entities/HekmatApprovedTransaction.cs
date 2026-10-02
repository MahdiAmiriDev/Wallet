using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class HekmatApprovedTransaction  :IBaseEntity<int>
    {
public int Id { get; set; }
public int AppDocumentId { get; set; }
public string HekmatCardNo { get; set; }
public long? Price { get; set; }
public int? TerminalId { get; set; }
public string StoreName { get; set; }
public int? TransactionDate { get; set; }
public int? ProcessDate { get; set; }
public int? InstallmentCount { get; set; }
public string TransactionState { get; set; }
public string InstallmentGroup { get; set; }
public string InstallmentTrackingCode { get; set; }
public int? TraceNo { get; set; }
public string Token { get; set; }
public int CreateBy { get; set; }
public DateTime CreationDate { get; set; }
public int? ModifyBy { get; set; }
public DateTime? ModificationDate { get; set; }
public bool Deleted { get; set; }
public long? HekmatCardNoInt { get; set; }

    public virtual AppDocument AppDocument { get; set; }
    }
 }
