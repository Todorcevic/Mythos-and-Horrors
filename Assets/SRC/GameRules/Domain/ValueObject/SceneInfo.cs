using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace MythosAndHorrors.GameRules
{
    public record SceneInfo
    {
        [JsonProperty("Code")] public string Code { get; init; }
        [JsonProperty("Position")] public int Position { get; init; }
        [JsonProperty("Name")] public string Name { get; init; }
        [JsonProperty("Description")] public History Description { get; init; }
        [JsonProperty("Resolutions")] public List<History> Resolutions { get; init; }
        [JsonProperty("Cards")] public List<Card> Cards { get; init; }
        [JsonProperty("NextScene")] public string NextScene { get; init; }

        public List<CardPlace> PlaceCards => Cards.OfType<CardPlace>().ToList();
        public List<CardPlot> PlotCards => Cards.OfType<CardPlot>().ToList();
        public List<CardGoal> GoalCards => Cards.OfType<CardGoal>().ToList();
        public List<Card> DangerCards => Cards.FindAll(card => card is CardAdversity || card is CardCreature);

    }
}
