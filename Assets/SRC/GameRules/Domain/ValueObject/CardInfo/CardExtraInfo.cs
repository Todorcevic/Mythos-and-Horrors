using System.Collections.Generic;
using Newtonsoft.Json;

namespace MythosAndHorrors.GameRules
{
    public record CardExtraInfo
    {
        [JsonProperty("Code")] public string Code { get; init; }
        [JsonProperty("Histories")] public List<History> Histories { get; init; }
        [JsonProperty("ConnectedPlaces")] public string[] ConnectedPlaces { get; init; }
    }
}
