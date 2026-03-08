using System.Text.Json;
using Microsoft.Extensions.Options;
using task.Models.Dto;
using task.Options;

namespace task.Sources;

public class FileTerminalSource : ITerminalSource
{
    private readonly ILogger<FileTerminalSource> _logger;
    private readonly string _filePath;

    public FileTerminalSource(ILogger<FileTerminalSource> logger, IOptions<TerminalSourceOptions> options)
    {
        _logger = logger;
        _filePath = Path.Combine(AppContext.BaseDirectory, options.Value.FilePath);
    }

    public async Task<DataJsonDto> LoadAsync(CancellationToken token)
    {
        if (!File.Exists(_filePath))
        {
            _logger.LogWarning("Terminal file not found: {path}", _filePath);
            return new DataJsonDto();
        }

        await using var stream = File.OpenRead(_filePath);
        var terminals = await JsonSerializer.DeserializeAsync<DataJsonDto>(stream, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }, token);
        return terminals ?? new DataJsonDto();
    }
}