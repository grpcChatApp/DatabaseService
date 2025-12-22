using System.ComponentModel.DataAnnotations.Schema;
namespace DatabaseService.Data.Models
{
    [Table("resources", Schema = "auth")]
    public class ApiResource : BaseEntity
    {
        public virtual ICollection<ApiScope> Scopes { get; set; } = [];
        public virtual ICollection<Client> Clients { get; set; } = [];
    }
}
