using System;

namespace DatabaseService.Data.Models.Auth;

public class ClientResourceMapping : BaseEntity
{
    public required int ClientId { get; set; }
    public required Client Client { get; set; }
    public required int ResourceId { get; set; }
    public required ApiResource Resource { get; set; }
}
