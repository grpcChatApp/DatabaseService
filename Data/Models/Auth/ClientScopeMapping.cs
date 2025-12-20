using System;

namespace DatabaseService.Data.Models.Auth;

public class ClientScopeMapping
{
    public required int ClientId { get; set; }
    public required Client Client { get; set; }
    public required int ScopeId { get; set; }
    public required ApiScope Scope { get; set; }
}
