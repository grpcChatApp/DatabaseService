using System.ComponentModel.DataAnnotations.Schema;
using static Common.Constants;
namespace DatabaseService.Data.Models
{
    [Table("scopes", Schema = "auth")]
    public class ApiScope : BaseEntity
    {
        public required PermissionLevel Level { get; set; }
        public virtual ICollection<ApiResource> Resources { get; set; } = [];
    }
}
