using DatabaseService.Contexts;
using DatabaseService.Contracts.Grpc;
using DatabaseService.Contracts.Kalfka;
using DatabaseService.Data;
using DatabaseService.Services;
using DatabaseService;
using Microsoft.EntityFrameworkCore;
using DatabaseService.Services.AuthenticationService;
using Confluent.Kafka;
using DatabaseService.Application.Users;

var builder = WebApplication.CreateBuilder(args);

var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? throw new InvalidOperationException("DB_PASSWORD not set.");
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Default connection string not set.");
connectionString = connectionString.Replace("{DB_PASSWORD}", dbPassword);

// Register dependencies
ConfigureServices(builder.Services, builder.Configuration);

var app = builder.Build();
var _logger = app.Services.GetRequiredService<ILogger<Program>>();

SeedDatabase(app.Services);

// Configure Middleware
app.UseRouting();
// app.UseAuthorization();
MapEndpoints(app);

app.Run();

void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    var applicationSettings = new ApplicationSettings();
    configuration.GetSection("Configurations").Bind(applicationSettings);
    services.AddSingleton(applicationSettings);
    services.AddGrpc();
    var producerConfig = new ProducerConfig();
    configuration.GetSection("Kafka:Producer").Bind(producerConfig);
    services.AddSingleton(producerConfig);
    services.AddScoped<IKafkaPublisher, KafkaPublisher>();
    services.AddScoped<IUserRequestsHandler, UserRequestHandler>();
    services.AddScoped<IClientRequestsHandler, ClientRequestsHandler>();
    // Register concrete handlers as well so services that request the concrete type resolve
    services.AddScoped<UserRequestHandler>();
    services.AddScoped<ClientRequestsHandler>();
    services.AddLogging();

    services.AddDbContext<CoreContext>(options =>
        options.UseNpgsql(connectionString, b => b.MigrationsAssembly("DatabaseService")));
}

void MapEndpoints(WebApplication app)
{
    app.MapGrpcService<AuthenticationService>();
    app.MapGrpcService<ClientGrpcService>();
    app.MapGrpcService<UserGrpcService>();
}

void SeedDatabase(IServiceProvider serviceProvider)
{
    using (var scope = serviceProvider.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            Initializer.Seed(services);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
        }
    }

}
