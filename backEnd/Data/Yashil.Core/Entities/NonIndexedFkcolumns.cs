using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class NonIndexedFkcolumns  
    {
public string NonIndexedColTableName { get; set; }
public string NonIndexedColColumnName { get; set; }
public string ConstraintName { get; set; }
public string RefSchemaName { get; set; }
public string RefTableName { get; set; }
public string RefColumnName { get; set; }
    }
 }
