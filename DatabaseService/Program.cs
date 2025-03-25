using DatabaseService;
using DatabaseService.Initializer;
using DatabaseService.Integrations.Grpc.AuthenticationServer;
using DatabaseService.Integrations.Kafka;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

internal class Program
{
    private static ILogger<Program> _logger;

    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? throw new InvalidOperationException("DB_PASSWORD not set.");
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Default connection string not set.");
        connectionString = connectionString.Replace("{DB_PASSWORD}", dbPassword);
        builder.Services.AddDbContext<DatabaseContext>(options =>
            options.UseSqlServer(connectionString, b => b.MigrationsAssembly("DatabaseService")));

        // Register dependencies
        ConfigureServices(builder.Services, builder.Configuration);
        var app = builder.Build();
        _logger = app.Services.GetRequiredService<ILogger<Program>>();

        SeedDatabase(app.Services);

        // Configure Middleware
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseRouting();
        app.UseAuthorization();
        app.UseEndpoints(endpoints => endpoints.MapGrpcService<AuthenticationService>());
        app.Run();
    }

    private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        var applicationSettings = new ApplicationSettings();
        configuration.GetSection("Configurations").Bind(applicationSettings);
        services.AddSingleton(applicationSettings);

        services.AddHostedService<ConsumerService>();
    }

    private static void SeedDatabase(IServiceProvider serviceProvider)
    {
        using (var scope = serviceProvider.CreateScope())
        {
            var services = scope.ServiceProvider;
            try
            {
                DatabaseInitializer.Seed(services);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while seeding the database.");
            }
        }

    }
}