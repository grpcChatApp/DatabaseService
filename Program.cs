using DatabaseService.Contexts;
using DatabaseService.Data;
using DatabaseService.Services;
using DatabaseService;
using Microsoft.EntityFrameworkCore;
using DatabaseService.Services.AuthenticationService;


var builder = WebApplication.CreateBuilder(args);

var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? throw new InvalidOperationException("DB_PASSWORD not set.");
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Default connection string not set.");
connectionString = connectionString.Replace("{DB_PASSWORD}", dbPassword);

builder.Services.AddDbContext<CoreContext>(options =>
    options.UseSqlServer(connectionString, b => b.MigrationsAssembly("DatabaseService")));

builder.Services.AddGrpc();
builder.Services.AddLogging();
// Register dependencies
ConfigureServices(builder.Services, builder.Configuration);
var app = builder.Build();
var _logger = app.Services.GetRequiredService<ILogger<Program>>();

SeedDatabase(app.Services);

// Configure Middleware
app.UseSwagger();
app.UseSwaggerUI();

app.UseRouting();
app.UseAuthorization();
app.UseEndpoints(endpoints => endpoints.MapGrpcService<AuthenticationService>());
app.Run();

void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    services.AddControllers();
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();

    var applicationSettings = new ApplicationSettings();
    configuration.GetSection("Configurations").Bind(applicationSettings);
    services.AddSingleton(applicationSettings);

    services.AddScoped<IKafkaPublisher, KafkaPublisher>();
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
