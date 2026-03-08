using task.Mapping;
using task.Persistence;
using task.Sources;

namespace task.Services;

public class TerminalImportService : ITerminalImportService
{
    private readonly ILogger<TerminalImportService> _logger;
    private readonly ITerminalSource _terminalSource;
    private readonly IOfficeRepository _officeRepository;

    public TerminalImportService(ILogger<TerminalImportService> logger, ITerminalSource terminalSource,
        IOfficeRepository officeRepository)
    {
        _logger = logger;
        _terminalSource = terminalSource;
        _officeRepository = officeRepository;
    }

    public async Task ImportAsync(CancellationToken token)
    {
        _logger.LogInformation("Import started at {time}", DateTime.UtcNow);

        var data = await _terminalSource.LoadAsync(token);

        var offices = data.ToOffices();
        var phones = offices.SelectMany(o => o.Phones).ToList();
        offices.ForEach(o => o.Phones.Clear());
        _logger.LogInformation("Loaded {Count} terminals from JSON", offices.Count);

        var oldCount = await _officeRepository.GetOfficeCountAsync(token);
        await _officeRepository.ReplaceAllAsync(offices, phones, token);
        _logger.LogInformation("Deleted {OldCount} old records", oldCount);
        _logger.LogInformation("Saved {NewCount} new terminals", offices.Count);

        _logger.LogInformation("Import finished at {time}", DateTime.UtcNow);
    }
}