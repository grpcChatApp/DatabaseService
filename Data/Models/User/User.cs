using GrpcChat.Database.Users;
namespace DatabaseService.Data.Models
{
    public class User : BaseEntity
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;

        public static User Create(string username, string email, string name, string passwordHash) => new()
        {
            ReferenceId = Guid.NewGuid().ToString("N"),
            Username = username,
            Name = name,
            Email = email,
            PasswordHash = passwordHash,
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow,
            IsActive = true
        };

        public UserDto ToDto() => new()
        {
            Id = ReferenceId,
            Username = Username,
            Email = Email,
            IsActive = IsActive,
            Name = Name
        };
    }
}
