namespace task.Models.Dto;

public class TerminalJsonDto
{
    public string Id { get; set; } = default!;

    public string Address { get; set; } = default!;

    public string FullAddress { get; set; } = default!;

    public string Latitude { get; set; } = default!;

    public string Longitude { get; set; } = default!;

    public List<PhoneJsonDto> Phones { get; set; } = [];

    public CalcScheduleJsonDto CalcSchedule { get; set; } = default!;
}