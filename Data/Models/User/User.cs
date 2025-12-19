using GrpcChat.Database.Users;
namespace DatabaseService.Data.Models
{
    public class User : BaseEntity
    {
        public string Username { get; private set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public bool IsActive { get; set; }

        public static User Create(string username, string email, string name, string passwordHash) => new User
        {
            ReferenceId = Guid.NewGuid(),
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
            Id = ReferenceId.ToString(),
            Username = Username,
            Email = Email,
            IsActive = IsActive,
            Name = Name
        };
    }
}
