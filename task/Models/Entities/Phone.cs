namespace task.Models.Entities;

public class Phone
{
    public int Id { get; set; }

    public int OfficeId { get; set; }

    public string Number { get; set; } = default!;

    public string Type { get; set; } = default!;

    public string Comment { get; set; } = default!;

    public bool Primary { get; set; }

    public Office Office { get; set; } = default!;
}