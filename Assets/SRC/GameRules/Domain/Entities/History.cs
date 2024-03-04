using Newtonsoft.Json;

namespace MythosAndHorrors.GameRules
{
    public record History
    {
        [JsonProperty("Code")] public string Code { get; init; }
        [JsonProperty("Title")] public string Title { get; init; }
        [JsonProperty("Description")] public string Description { get; init; }
        [JsonProperty("Image")] public string Image { get; init; }
    }
}
