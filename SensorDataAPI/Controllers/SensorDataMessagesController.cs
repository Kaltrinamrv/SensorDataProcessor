using Microsoft.AspNetCore.Mvc;
using 

[ApiController]
[Route("api/[controller]")]
public class SensorDataMessagesController : ControllerBase
{
    private readonly ISensorDataService _sensorDataService;

    public SensorDataMessagesController(ISensorDataService sensorDataService)
    {
        _sensorDataService = sensorDataService;
    }

    [HttpGet]
    public async Task<IActionResult> GetSensorData()
    {
        var data = await _sensorDataService.GetSensorDataAsync();
        return Ok(data);
    }
}