using task.Models.Dto;

namespace task.Sources;

public interface ITerminalSource
{
    Task<DataJsonDto> LoadAsync(CancellationToken token);
}