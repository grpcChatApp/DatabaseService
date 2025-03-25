namespace Common.Data
{
    public class BaseEntity
    {
        public int Id { get; set; }

        public string Guid { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; }
    }
}
