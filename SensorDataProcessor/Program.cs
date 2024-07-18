using DotPulsar;
using DotPulsar.Abstractions;
using DotPulsar.Extensions;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SensorDataProcessor
{
    class Program
    {
        private static async Task Main(string[] args)
        {
            var serviceUrl = new Uri("pulsar://localhost:6650");
            var topic = "persistent://public/default/temperature-sensors";

            await using var client = PulsarClient.Builder().ServiceUrl(serviceUrl).Build();
            await using var producer = client.NewProducer().Topic(topic).Create();

            while (true)
            {
                Console.Write("Enter sensor ID: ");
                var sensorId = Console.ReadLine();

                Console.Write("Enter temperature: ");
                var temperature = Console.ReadLine();

                var message = $"{sensorId}:{temperature}";
                await producer.Send(Encoding.UTF8.GetBytes(message));

                Console.WriteLine("Message sent to Pulsar");
                Thread.Sleep(1000); 
            }
        }
    }
}
