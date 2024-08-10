using Newtonsoft.Json;

namespace MythosAndHorrors.GameRules
{
    public record InteractableInfo
    {
        [JsonProperty("Title")] public string Title { get; init; } = "TEXT NOT FOUND";
        [JsonProperty("MustShowInCenter")] public bool MustShowInCenter { get; init; }
    }
}
