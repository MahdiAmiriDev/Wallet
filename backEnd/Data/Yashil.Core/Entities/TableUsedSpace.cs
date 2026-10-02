using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class TableUsedSpace  
    {
public string TableName { get; set; }
public string SchemaName { get; set; }
public long? Rows { get; set; }
public long? TotalSpaceKb { get; set; }
public decimal? TotalSpaceMb { get; set; }
public long? UsedSpaceKb { get; set; }
public decimal? UsedSpaceMb { get; set; }
public long? UnusedSpaceKb { get; set; }
public decimal? UnusedSpaceMb { get; set; }
    }
 }
