using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorDataProcessor.Services
{
    public class SensorDataProcessorService : ISensorDataProcessorService
    {
        public SensorDataDto ProcessSensorData(SensorDataDto sensorData)
        {
            // Modify the sensor data as needed
            sensorData.Temperature = ConvertToFahrenheit(sensorData.Temperature);
            return sensorData;
        }

        private double ConvertToFahrenheit(double celsius)
        {
            return (celsius * 9 / 5) + 32;
        }
    }
}
