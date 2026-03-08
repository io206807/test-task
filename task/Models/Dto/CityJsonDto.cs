namespace task.Models.Dto;

public class CityJsonDto
{
    public string Name { get; set; } = default!;

    public string Code { get; set; } = default!;

    public TerminalsJsonDto Terminals { get; set; } = default!;
}