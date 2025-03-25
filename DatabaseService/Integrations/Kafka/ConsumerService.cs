
using Common;
using Common.Data;
using Common.Data.KafkaEvents;
using Confluent.Kafka;
using System.Text.Json;

namespace DatabaseService.Integrations.Kafka
{
    public class ConsumerService : BackgroundService
    {
        private readonly string[] _kafkaTopics = [
            KafkaTopics.Client.Create,
            KafkaTopics.Client.Delete,
            KafkaTopics.User.Create,
            KafkaTopics.User.Update,
            KafkaTopics.User.Delete,
        ];
        private readonly ApplicationSettings _configuration;
        private readonly ILogger _logger;
        private readonly IConsumer<string, string> _consumer;
        private ConsumerConfig _consumerConfig;

        public ConsumerService(ApplicationSettings applicationSettings, ILogger logger)
        {
            _configuration = applicationSettings;
            _logger = logger;
            _consumerConfig = new ConsumerConfig()
            {
                BootstrapServers = applicationSettings.KafkaBootstrapServers,
                GroupId = Constants.KafkaDatabaseGroupId,
                AutoOffsetReset = AutoOffsetReset.Earliest,
            };

            _consumer = new ConsumerBuilder<string, string>(_consumerConfig).Build();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _consumer.Subscribe(_kafkaTopics);

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        var result = _consumer.Consume(stoppingToken);
                        if (result != null)
                        {
                            UserEvent userEvent = null;
                            switch (result.Topic)
                            {
                                case KafkaTopics.User.Create:
                                    userEvent = JsonSerializer.Deserialize<UserEvent>(result.Message.Value);
                                    HandleUserCreate(userEvent);
                                    break;
                                case KafkaTopics.User.Delete:
                                    userEvent = JsonSerializer.Deserialize<UserEvent>(result.Message.Value);
                                    HandleUserDelete(userEvent);
                                    break;
                                case KafkaTopics.User.Update:
                                    userEvent = JsonSerializer.Deserialize<UserEvent>(result.Message.Value);
                                    HandleUserUpdate(userEvent);
                                    break;
                                case KafkaTopics.Client.Create:
                                    HandleClientCreate(result.Message.Value);
                                    break;
                                case KafkaTopics.Client.Delete:
                                    HandleClientDelete(result.Message.Value);
                                    break;
                                default:
                                    _logger.LogWarning($"Unhandled topic: {result.Topic}");
                                    break;
                            }
                        }
                    }
                    catch (ConsumeException e)
                    {
                        _logger.LogError($"Kafka Consumer error: {e.Error.Reason}");
                    }
                }
            }
            finally
            {
                _consumer.Close();
            }

            return Task.CompletedTask;
        }

        private void HandleUserCreate(UserEvent message)
        {
            _logger.LogInformation($"Processing user creation event: {message.Name}");
        }

        private void HandleUserDelete(UserEvent message)
        {
            _logger.LogInformation($"Processing user deletion event: {message.Name}");
        }

        private void HandleUserUpdate(UserEvent message)
        {
            _logger.LogInformation($"Processing user updation event: {message.Name}");
        }

        private void HandleClientCreate(string message)
        {
            _logger.LogInformation($"Processing client creation event: {message}");
        }

        private void HandleClientDelete(string message)
        {
            _logger.LogInformation($"Processing client deletion event: {message}");
        }
    }
}
