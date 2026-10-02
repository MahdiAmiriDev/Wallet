using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class LogEvents  
    {
public int Id { get; set; }
public string Message { get; set; }
public string Level { get; set; }
public DateTime? Timestamp { get; set; }
public string Exception { get; set; }
public string LogEvent { get; set; }
public string IpAddress { get; set; }
public string UserName { get; set; }
public string ObjectName { get; set; }
public string Action { get; set; }
public string ShortMessage { get; set; }
public DateTime? CreationDate { get; set; }
public int? ApplicationId { get; set; }
public string UserAgent { get; set; }
    }
 }
