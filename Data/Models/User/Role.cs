using System.ComponentModel.DataAnnotations.Schema;
namespace DatabaseService.Data.Models
{
    [Table("roles", Schema = "auth")]
    public class Role : BaseEntity
    {
        public string Description { get; set; } = string.Empty;
        public virtual ICollection<User> Users { get; set; } = [];
    }
}