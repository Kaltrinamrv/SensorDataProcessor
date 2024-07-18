using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotPulsar;

namespace SensorDataProcessor.Services
{
    public class SensorDataService : ISensorDataService
    {
        private readonly IElasticsearchClient _elasticsearchClient;
        private readonly IPulsarClient _pulsarClient;
        private readonly ISensorDataProcessorService _processorService;

        public SensorDataService(IElasticsearchClient elasticsearchClient, IPulsarClient pulsarClient, ISensorDataProcessorService processorService)
        {
            _elasticsearchClient = elasticsearchClient;
            _pulsarClient = pulsarClient;
            _processorService = processorService;
        }

        public async Task PublishSensorData(SensorDataDto sensorData)
        {
            var processedData = _processorService.ProcessSensorData(sensorData);
            await _pulsarClient.PublishAsync("sensor-data-topic", processedData);
        }

        public async Task SaveSensorData(SensorDataDto sensorData)
        {
            await _elasticsearchClient.IndexDocumentAsync("sensor_data", sensorData);
        }

        public async Task<IEnumerable<SensorDataDto>> GetSensorDataAsync()
        {
            var response = await _elasticsearchClient.SearchAsync<SensorDataDto>("sensor_data");
            return response.Documents;
        }
    }

}
