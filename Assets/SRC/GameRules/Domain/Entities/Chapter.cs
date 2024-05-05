using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace MythosAndHorrors.GameRules
{
    public record Chapter
    {
        [JsonProperty("Code")] public string Code { get; init; }
        [JsonProperty("Title")] public string Title { get; init; }
        [JsonProperty("Description")] public History Description { get; init; }
        [JsonProperty("Investigators")] public List<string> Investigators { get; init; }
        [JsonProperty("Scenes")] public List<string> Scenes { get; init; }
        [JsonProperty("Register")] public Dictionary<int, bool> Register { get; init; } = new();

        /*******************************************************************/
        public Type RegisterEnum => Type.GetType($"{GetType().Namespace}.{Code}{nameof(Register)}");

        public bool HasThisScene(string sceneCode) => Scenes.Contains(sceneCode);

        public void ChapterRegister<T>(T position, bool state) where T : Enum => Register[position.GetHashCode()] = state;

        public bool IsRegistered<T>(T position) where T : Enum
        {
            Register.TryGetValue(position.GetHashCode(), out bool isRegistered);
            return isRegistered;
        }
    }
}
