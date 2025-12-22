namespace DatabaseService.Data.KafkaEvents
{
    public class UserEvent : BaseEvent
    {
        public string Username { get; }

        public UserEvent(Guid referenceId, string eventName, string username)
            : base(referenceId.ToString(), eventName)
        {
            Username = username;
        }
    }
}