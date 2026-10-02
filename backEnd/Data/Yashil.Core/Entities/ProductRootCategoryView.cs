using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class ProductRootCategoryView  
    {
public int ProductId { get; set; }
public int CategoryId { get; set; }
public string Title { get; set; }
public int ApplicationId { get; set; }
    }
 }
