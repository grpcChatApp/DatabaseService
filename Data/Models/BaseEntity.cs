namespace DatabaseService.Data.Models
{
    public class BaseEntity
    {
        public int Id { get; set; }

        public string Guid { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; }
    }
}
