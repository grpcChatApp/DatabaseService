using DatabaseService.Data.Models.Auth;
using GrpcChat.Database.Clients;
using static Common.Constants;
using System.Security.Cryptography;
namespace DatabaseService.Data.Models
{
    public class Client : BaseEntity
    {
        public string ClientId { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;
        public ClientType Type { get; set; }
        public ICollection<ClientResourceMapping> AllowedScopes { get; set; } = new List<ClientResourceMapping>();
        public ICollection<ClientScopeMapping> AllowedResources { get; set; } = new List<ClientScopeMapping>();

        public static Client Create(string name, string clientId = "", ClientType type = ClientType.Confidential) => new Client
        {
            ReferenceId = Guid.NewGuid().ToString("N"),
            Name = name,
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow,
            IsActive = true,
            ClientId = string.IsNullOrEmpty(clientId) ? Guid.NewGuid().ToString("N") : clientId,
            ClientSecret = GenerateClientSecret(),
            Type = type
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
