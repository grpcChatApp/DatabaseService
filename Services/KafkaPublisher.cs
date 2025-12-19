using System.Text.Json;
using Common.Data.KafkaEvents;
using Confluent.Kafka;
using DatabaseService.Data.KafkaEvents;
using DatabaseService.Contracts.Kalfka;

namespace DatabaseService.Services
{
    public class KafkaPublisher : IKafkaPublisher, IDisposable
    {
        private readonly IProducer<string, string> _producer;
        private readonly ILogger<KafkaPublisher> _logger;
        private readonly JsonSerializerOptions _jsonOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

        public KafkaPublisher(ProducerConfig config, ILogger<KafkaPublisher> logger)
        {
            _producer = new ProducerBuilder<string, string>(config).Build();
            _logger = logger;
        }

        public async Task PublishAsync<T>(string topic, T @event, CancellationToken cancellationToken = default)
        {
            var payload = JsonSerializer.Serialize(@event, _jsonOptions);
            var msg = new Message<string, string>
            {
                Key = GetEventKey(@event),
                Value = payload
            };

            try
            {
                await _producer.ProduceAsync(topic, msg, cancellationToken).ConfigureAwait(false);

                _logger.LogInformation(
                    "Published event {EventType} to topic {Topic}",
                    typeof(T).Name,
                    topic
                );

            }
            catch (ProduceException<string, string> ex)
            {
                _logger.LogError(ex, "Failed to publish event {EventType}", typeof(T).Name);
                throw;
            }
        }

        private static string GetEventKey<T>(T @event)
        {
            return @event switch
            {
                UserEvent e => e.ReferenceId,
                ClientEvent e => e.ReferenceId,
                _ => Guid.NewGuid().ToString()
            };
        }

        public void Dispose()
        {
            try
            {
                _producer.Flush(TimeSpan.FromSeconds(5));
            }
            finally
            {
                _producer.Dispose();
            }
        }
    }
}
