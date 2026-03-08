using System.Globalization;
using task.Models.Dto;
using task.Models.Entities;

namespace task.Mapping;

public static class TerminalMapper
{
    public static List<Office> ToOffices(this DataJsonDto data)
    {
        var offices = new List<Office>();

        foreach (var city in data.Cities)
        {
            foreach (var terminal in city.Terminals.Terminal)
            {
                offices.Add(MapOffice(city, terminal));
            }
        }

        return offices;
    }

    private static Office MapOffice(CityJsonDto city, TerminalJsonDto terminal)
    {
        var officeId = int.Parse(terminal.Id);

        return new Office
        {
            Id = officeId,
            CityCode = city.Code,

            Coordinates = new Coordinates
            {
                Latitude = double.Parse(terminal.Latitude, CultureInfo.InvariantCulture),
                Longitude = double.Parse(terminal.Longitude, CultureInfo.InvariantCulture)
            },

            Address = new Address
            {
                City = city.Name,
                ShortAddress = terminal.Address,
                FullAddress = terminal.FullAddress
            },

            WorkTime = terminal.CalcSchedule.Derival,

            Phones = MapPhones(terminal.Phones, officeId)
        };
    }

    private static List<Phone> MapPhones(List<PhoneJsonDto> phones, int officeId)
    {
        return phones.Select(p => new Phone
        {
            OfficeId = officeId,
            Number = p.Number,
            Type = p.Type,
            Comment = p.Comment,
            Primary = p.Primary
        }).ToList();
    }
}
