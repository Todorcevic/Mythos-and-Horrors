using Newtonsoft.Json;

namespace MythosAndHorrors.GameView
{
    public record InteractableText
    {
        [JsonProperty("Title")] public string Title { get; init; } = "TEXT NOT FOUND";
        [JsonProperty("MustShowInCenter")] public bool MustShowInCenter { get; init; }
    }
}
