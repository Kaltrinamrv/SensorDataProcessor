namespace SensorDataAPI.Entities
{
    public class TemperatureEvent
    {
        public string SensorId { get; set; }
        public double Temperature { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
