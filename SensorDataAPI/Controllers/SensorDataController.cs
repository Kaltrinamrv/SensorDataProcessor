using Microsoft.AspNetCore.Mvc;
using SensorDataProcessor.Services;

[ApiController]
[Route("api/[controller]")]
public class SensorDataController : ControllerBase
{
    private readonly ISensorDataService _sensorDataService;

    public SensorDataController(ISensorDataService sensorDataService)
    {
        _sensorDataService = sensorDataService;
    }

    [HttpPost]
    public async Task<IActionResult> PostSensorData([FromBody] SensorDataDto sensorData)
    {
        await _sensorDataService.PublishSensorData(sensorData);
        return Ok();
    }
}
