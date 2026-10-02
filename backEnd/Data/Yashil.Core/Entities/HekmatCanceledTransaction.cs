using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class HekmatCanceledTransaction  :IBaseEntity<int>
    {
public int Id { get; set; }
public int AppDocumentId { get; set; }
public string CartNumber { get; set; }
public int? RecieverCode { get; set; }
public string StoreName { get; set; }
public int? TerminalNumber { get; set; }
public long? SendTransactionPrice { get; set; }
public string TransactionDate { get; set; }
public string ProcessDate { get; set; }
public string CancleDate { get; set; }
public long? FullAnnouncedInstallmentPrice { get; set; }
public long? RefundPrice { get; set; }
public int CreateBy { get; set; }
public DateTime CreationDate { get; set; }
public int? RefundDetailId { get; set; }
public int? ModifyBy { get; set; }
public DateTime? ModificationDate { get; set; }
public bool Deleted { get; set; }
    }
 }
