using Cronos;
using Microsoft.Extensions.Options;
using task.Options;
using task.Services;
using TimeZoneConverter;

namespace task;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly CronExpression _cron;
    private readonly TimeZoneInfo _timeZone;
    private readonly IServiceScopeFactory _scopeFactory;

    public Worker(ILogger<Worker> logger, IOptions<SchedulerOptions> options,
        IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;

        var config = options.Value;

        _cron = CronExpression.Parse(config.Cron);
        _timeZone = TZConvert.GetTimeZoneInfo(config.TimeZone);

        _logger.LogInformation("Scheduler configured. Cron: {cron}, TimeZone: {tz}",
            config.Cron, config.TimeZone);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Worker started");

        try
        {
            while(!stoppingToken.IsCancellationRequested)
            {
                var now = DateTime.UtcNow;
                var nextUtc = _cron.GetNextOccurrence(now, _timeZone, true);

                if(nextUtc == null)
                    return;

                var delay = nextUtc.Value - now;
                _logger.LogInformation("Next run at {time}", nextUtc.Value);

                await Task.Delay(delay, stoppingToken);

                try
                {
                    await RunImportAsync(stoppingToken);
                }
                catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Job failed, will retry on next schedule");
                }
            }
        }
        catch(OperationCanceledException)
        {
            _logger.LogInformation("Worker stopping gracefully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled error in Worker");
            throw;
        }
        finally
        {
            _logger.LogInformation("Worker stopped");
        }
    }

    private async Task RunImportAsync(CancellationToken token)
    {
        await using var scope = _scopeFactory.CreateAsyncScope();
        await scope.ServiceProvider
            .GetRequiredService<ITerminalImportService>()
            .ImportAsync(token);
    }
}