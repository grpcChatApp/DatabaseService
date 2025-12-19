namespace DatabaseService.Contracts.Kalfka
{
    public interface IKafkaPublisher
    {
        Task PublishAsync<T>(string topic, T @event, CancellationToken cancellationToken = default);
    }
}