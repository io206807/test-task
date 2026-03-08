namespace task.Models.Dto;

public class PhoneJsonDto
{
    public string Number { get; set; } = default!;

    public string Type { get; set; } = default!;

    public string Comment { get; set; } = default!;

    public bool Primary { get; set; }
}