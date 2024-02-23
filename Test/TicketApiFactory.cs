using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RazorClassLib.Data;
using Testcontainers.PostgreSql;

namespace Test;

public class TicketsApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer;
    public TicketsApiFactory()
    {
        var whereAmI = Environment.CurrentDirectory;

        var backupFile = Directory.GetFiles("../../../../", "*.sql", SearchOption.AllDirectories)
            .Select(f => new FileInfo(f))
            .OrderByDescending(fi => fi.LastWriteTime)
            .First();

        _dbContainer = new PostgreSqlBuilder()
            .WithImage("postgres")
            .WithPassword("Strong_password_123!")
            .WithBindMount(backupFile.FullName, "/docker-entrypoint-initdb.d/init.sql")
            .Build();

    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            var connection = _dbContainer.GetConnectionString();
            services.RemoveAll(typeof(DbContextOptions<TicketContext>));
            services.AddDbContextFactory<TicketContext>(options => options.UseNpgsql(_dbContainer.GetConnectionString()));
        });
    }
    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
    }


    public async Task DisposeAsync()
    {
        await _dbContainer.StopAsync();
    }
}
