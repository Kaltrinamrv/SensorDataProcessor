using Microsoft.AspNetCore.Mvc;
using Nest;
using SensorDataAPI.Entities;
using System.Threading.Tasks;

[ApiController]
[Route("[controller]")]
public class TemperatureController : ControllerBase
{
    private readonly IElasticClient _elasticClient;

    public TemperatureController(IElasticClient elasticClient)
    {
        _elasticClient = elasticClient;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var searchResponse = await _elasticClient.SearchAsync<TemperatureEvent>(s => s
            .Size(1000)
            .Query(q => q.MatchAll()));

        return Ok(searchResponse.Documents);
    }
}

