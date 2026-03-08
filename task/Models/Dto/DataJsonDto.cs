using System.Text.Json.Serialization;

namespace task.Models.Dto;

public class DataJsonDto
{
    [JsonPropertyName("city")]
    public List<CityJsonDto> Cities { get; set; } = [];
}