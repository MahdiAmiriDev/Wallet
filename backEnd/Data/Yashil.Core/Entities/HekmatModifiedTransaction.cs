using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class HekmatModifiedTransaction  :IBaseEntity<int>
    {
public int Id { get; set; }
public int AppDocumentId { get; set; }
public string CartNumber { get; set; }
public string RecieverCode { get; set; }
public string StoreName { get; set; }
public string TerminalNumber { get; set; }
public long? SendTransactionPrice { get; set; }
public long? FinalTransactionPrice { get; set; }
public string TransactionDate { get; set; }
public string ProcessDate { get; set; }
public string ModifiedDate { get; set; }
public int? AnnouncedInstallmentCount { get; set; }
public int? RestOfInstallmentAnnounced { get; set; }
public long? RefundPrice { get; set; }
public int CreateBy { get; set; }
public DateTime CreationDate { get; set; }
public int? RefundDetailId { get; set; }
public int? ModifyBy { get; set; }
public DateTime? ModificationDate { get; set; }
public bool Deleted { get; set; }
    }
 }
