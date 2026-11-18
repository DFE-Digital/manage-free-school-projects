using Dfe.ManageFreeSchoolProjects.Data;
using Microsoft.EntityFrameworkCore;

namespace Dfe.ManageFreeSchoolProjects.API.StartupConfiguration;

public class DatabaseWarmupHostedService(IServiceProvider serviceProvider, ILogger<DatabaseWarmupHostedService> logger)
    : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<MfspContext>();

            _ = await context.Kpi
                .AsNoTracking()
                .Select(kpi => kpi.Rid)
                .Take(1)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Database warmup query failed");
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
