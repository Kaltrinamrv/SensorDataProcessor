using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorDataProcessor.Services
{
    public interface ISensorDataService
    {
        Task PublishSensorData(SensorDataDto sensorData);
        Task SaveSensorData(SensorDataDto sensorData);
        Task<IEnumerable<SensorDataDto>> GetSensorDataAsync();
    }

}
