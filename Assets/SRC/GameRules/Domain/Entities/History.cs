using Unity.Plastic.Newtonsoft.Json;

namespace MythsAndHorrors.GameRules
{
    public record History
    {
        [JsonProperty("Code")] public string Code { get; init; }
        [JsonProperty("Title")] public string Title { get; init; }
        [JsonProperty("Description")] public string Description { get; init; }
        [JsonProperty("Image")] public string Image { get; init; }
    }
}
