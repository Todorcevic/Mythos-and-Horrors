using System.Collections.Generic;
using Unity.Plastic.Newtonsoft.Json;

namespace MythsAndHorrors.GameRules
{
    public record Chapter
    {
        [JsonProperty("Code")] public string Code { get; init; }
        [JsonProperty("Title")] public string Title { get; init; }
        [JsonProperty("Description")] public History Description { get; init; }
        [JsonProperty("Investigators")] public List<string> Investigators { get; init; }
        [JsonProperty("Scenes")] public List<string> Scenes { get; init; }

        public bool HasThisScene(string sceneCode) => Scenes.Contains(sceneCode);
    }
}
