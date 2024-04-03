using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Data;

public class Program
{
    public static void Main(string[] args)
        => CreateHostBuilder(args).Build().Run();

    // EF Core uses this method at design time to access the DbContext
    public static IHostBuilder CreateHostBuilder(string[] args)
        => Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(
                webBuilder => webBuilder.UseStartup<Startup>());
}

public class Startup
{
    private const string conn = "Server=localhost;Port=5432;Database=BookSwapHub;Username=postgres;Password=4";

    public void ConfigureServices(IServiceCollection services)
        => services.AddDbContext<PostgresContext>(options => options.UseNpgsql(conn));

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
    }
}
