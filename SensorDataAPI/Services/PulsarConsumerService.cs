using DotPulsar;
using DotPulsar.Abstractions;
using DotPulsar.Extensions;
using Microsoft.Extensions.Hosting;
using Nest;
using SensorDataAPI.Entities;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public class PulsarConsumerService : IHostedService
{
    private readonly IElasticClient _elasticClient;
    private readonly IPulsarClient _pulsarClient;
    private IConsumer<byte[]> _consumer;

    public PulsarConsumerService(IElasticClient elasticClient, IPulsarClient pulsarClient)
    {
        _elasticClient = elasticClient;
        _pulsarClient = pulsarClient;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var pulsarConsumer = _pulsarClient.NewConsumer()   
           .SubscriptionName("temperature-subscription")
           .SubscriptionType(SubscriptionType.KeyShared)
           .Topic("persistent://public/default/temperature-sensors")
           .Create();

        await Task.Factory.StartNew(async () =>
        {
            await foreach (var message in _consumer.Messages(cancellationToken))
            {
                var content = Encoding.UTF8.GetString(message.Data);
                var parts = content.Split(':');
                var sensorId = parts[0];
                var temperature = parts[1];

                
                var modifiedEvent = new TemperatureEvent
                {
                    SensorId = sensorId,
                    Temperature = double.Parse(temperature),
                    Timestamp = DateTime.UtcNow
                };

                // Save to Elasticsearch
                await _elasticClient.IndexDocumentAsync(modifiedEvent);

                await _consumer.Acknowledge(message, cancellationToken);
            }
        }, cancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Default);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return _consumer.DisposeAsync().AsTask();
    }
}
