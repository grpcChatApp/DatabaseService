using GrpcChat.Database.Clients;
using static Common.Constants;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
namespace DatabaseService.Data.Models
{
    [Table("clients", Schema = "auth")]
    public class Client : BaseEntity
    {
        public string ClientId { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;
        public ClientType Type { get; set; }
        public ICollection<ApiResource> Resources { get; set; } = new List<ApiResource>();
        public ICollection<ApiScope> Scopes { get; set; } = new List<ApiScope>();

        public static Client Create(string name) => new Client
        {
            ReferenceId = Guid.NewGuid(),
            Name = name,
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow,
            IsActive = true,
            ClientId = Guid.NewGuid().ToString("N"),
            ClientSecret = GenerateClientSecret(),
            Type = ClientType.Public
        };

        public ClientResponseDto ToDto()
        {
            return new ClientResponseDto
            {
                Id = ReferenceId.ToString(),
                ClientId = ClientId,
                ClientSecret = ClientSecret,
                IsActive = IsActive,
                Name = Name,
                ReferenceId = ReferenceId.ToString()
            };
        }

        public static string GenerateClientSecret(int length = 32)
        {
            var bytes = new byte[length];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(bytes);
            }
            return Convert.ToBase64String(bytes);
        }
    }
}
