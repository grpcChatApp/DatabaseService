using System;
using Google.Protobuf.WellKnownTypes;

namespace DatabaseService.Data.Models.Auth;

public class ResourceScopeMapping : BaseEntity
{
    public required int ResourceId { get; set; }
    public required ApiResource Resource { get; set; }
    public required int ScopeId { get; set; }
    public required ApiScope Scope { get; set; }

}
