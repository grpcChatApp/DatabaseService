using static Common.Constants;
namespace DatabaseService.Data.Models
{
    public class ApiScope : BaseEntity
    {
        public required PermissionLevel Level { get; set; }
    }
}
