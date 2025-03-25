namespace Common.Data
{
    public class Client : BaseEntity
    {
        public string ClientId { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;
        public List<ApiScope> AllowedScopes { get; set; } = new List<ApiScope>();

    }
}
