namespace DatabaseService.Data.KafkaEvents
{
    public class BaseEvent
    {
        public string ReferenceId { get; private set; }
        public string EventName { get; set; } = string.Empty;

        public BaseEvent(string referenceId, string eventName)
        {
            ReferenceId = referenceId;
            EventName = eventName;
        }
    }
}
