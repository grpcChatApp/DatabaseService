using System.ComponentModel.DataAnnotations.Schema;
using GrpcChat.Database.Users;
namespace DatabaseService.Data.Models
{
    [Table("users", Schema = "auth")]
    public class User : BaseEntity
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public virtual ICollection<Role> Roles { get; set; } = [];

        public static User Create(string username, string email, string name, string passwordHash) => new()
        {
            ReferenceId = Guid.NewGuid(),
            Username = username,
            Name = name,
            Email = email,
            PasswordHash = passwordHash,
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow,
            IsActive = true,
            Roles = new List<Role>()
        };

        public UserDto ToDto() => new()
        {
            Id = ReferenceId.ToString(),
            Username = Username,
            Email = Email,
            IsActive = IsActive,
            Name = Name
        };
    }
}
