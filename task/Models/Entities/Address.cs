namespace task.Models.Entities;

public class Address
{
    public string City { get; set; } = default!;

    public string ShortAddress { get; set; } = default!;

    public string FullAddress { get; set; } = default!;
}