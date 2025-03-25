namespace Common.Data.KafkaEvents
{
    public class UserEvent : BaseEvent
        {
        public string? Email { get; set; }
        public DateTime ChangedDate { get; set; }
    }
}
