using Nest;
using DotPulsar;
using DotPulsar.Extensions;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();

        // Register Elasticsearch
        var settings = new ConnectionSettings(new Uri("http://localhost:9200"))
            .DefaultIndex("temperature-events");
        var client = new ElasticClient(settings);
        services.AddSingleton<IElasticClient>(client);

        // Register Pulsar client
        var pulsarClient = PulsarClient.Builder()
            .ServiceUrl(new Uri("pulsar://localhost:6650"))
            .Build();
        services.AddSingleton(pulsarClient);
        services.AddHostedService<PulsarConsumerService>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
