using GrpcChat.Database.Clients;
using System.Security.Cryptography;
namespace DatabaseService.Data.Models
{
    public class Client : BaseEntity
    {
        public string ClientId { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;
        public List<ApiScope> AllowedScopes { get; set; } = new List<ApiScope>();

        public static Client Create(string name) => new Client
        {
            ReferenceId = Guid.NewGuid().ToString("N"),
            Name = name,
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow,
            IsActive = true,
            ClientId = Guid.NewGuid().ToString("N"),
            ClientSecret = GenerateClientSecret()
        };

        public ClientResponseDto ToDto()
        {
            return new ClientResponseDto
            {
                Id = ReferenceId,
                ClientId = ClientId,
                ClientSecret = ClientSecret,
                IsActive = IsActive,
                Name = Name,
                ReferenceId = ReferenceId
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
