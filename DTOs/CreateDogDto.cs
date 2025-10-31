using System.Text.Json.Serialization;

namespace DogHouse.Api.DTOs
{
    public class CreateDogDto
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("color")]
        public string Color { get; set; }

        [JsonPropertyName("tail_length")]
        public int? TailLength { get; set; }

        [JsonPropertyName("weight")]
        public int? Weight { get; set; }
    }

    public class DogDto
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("color")]
        public string Color { get; set; }

        [JsonPropertyName("tail_length")]
        public int TailLength { get; set; }

        [JsonPropertyName("weight")]
        public int Weight { get; set; }
    }
}