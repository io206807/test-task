namespace task.Models.Entities;

public class Office
{
    public int Id { get; set; }

    public string CityCode { get; set; } = default!;

    public Coordinates Coordinates { get; set; } = default!;

    public Address Address { get; set; } = default!;

    public string WorkTime { get; set; } = default!;

    public List<Phone> Phones { get; set; } = new();
}