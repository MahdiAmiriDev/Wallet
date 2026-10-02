using System;
using System.Collections.Generic;
using Yashil.Common.Core.Interfaces;

namespace Yashil.Core.Entities
{
    public partial class CompanyVirtualClassRoomConfig  :IBaseEntity<int>,IApplicationBasedEntity
    {
public CompanyVirtualClassRoomConfig()
{
    VirtualClass = new HashSet<VirtualClass>();
 }

public int Id { get; set; }
public int CompanyId { get; set; }
public int VirtualClassRoomConfigId { get; set; }
public int Capacity { get; set; }
public int CreateBy { get; set; }
public int? ModifyBy { get; set; }
public DateTime CreationDate { get; set; }
public DateTime? ModificationDate { get; set; }
public int ApplicationId { get; set; }
public int AccessLevelId { get; set; }
public bool Deleted { get; set; }
public int CreatorOrganizationId { get; set; }

    public virtual Company Company { get; set; }
    public virtual VirtualClassRoomConfig VirtualClassRoomConfig { get; set; }
public virtual ICollection<VirtualClass>
    VirtualClass { get; set; }
    }
 }
