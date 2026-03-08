namespace task.Services;

public interface ITerminalImportService
{
    Task ImportAsync(CancellationToken token);
}
