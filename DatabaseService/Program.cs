using DatabaseService;
using DatabaseService.Integration;
using Microsoft.EntityFrameworkCore;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? throw new InvalidOperationException("DB_PASSWORD not set.");
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Default connection string not set.");
        connectionString = connectionString.Replace("{DB_PASSWORD}", dbPassword);
        builder.Services.AddDbContext<DatabaseContext>(options =>
            options.UseSqlServer(connectionString, b => b.MigrationsAssembly("DatabaseService")));

        ConfigureServices(builder.Services, builder.Configuration);

        var app = builder.Build();

        // Configure Middleware
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseAuthorization();
        app.MapControllers();
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

        services.AddHostedService<KafkaConsumerService>();
    }
}