namespace Common.Data
{
    public class ApiResource
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<ApiScope> Scopes { get; set; } = new List<ApiScope>();
    }
}
