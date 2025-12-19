namespace DatabaseService.Data.Models
{
    public class BaseEntity
    {
        public int Id { get; set; }

        public Guid ReferenceId { get; set; } = Guid.Empty;

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }
    }
}
