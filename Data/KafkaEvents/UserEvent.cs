namespace DatabaseService.Data.KafkaEvents
{
    public class UserEvent : BaseEvent
    {
        public string Username { get; }

        public UserEvent(string referenceId, string eventName, string username)
            : base(referenceId, eventName)
        {
            Username = username;
        }
    }
}