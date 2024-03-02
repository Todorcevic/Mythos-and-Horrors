using MythosAndHorrors.GameRules;
using System.Collections.Generic;
using Unity.Plastic.Newtonsoft.Json;

namespace MythosAndHorrors.GameView
{
    public class DataSave
    {
        [JsonProperty("InvestigatorsSelected")] public List<string> InvestigatorsSelected { get; init; }
        [JsonProperty("SceneSelected")] public string SceneSelected { get; init; }
        [JsonProperty("DificultySelected")] public Dificulty DificultySelected { get; init; }
        [JsonProperty("LanguajeSelected")] public Languaje LanguajeSelected { get; init; }
    }
}
