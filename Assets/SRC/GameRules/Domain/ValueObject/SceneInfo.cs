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
        [JsonProperty("ChallengeTokensEasy")] public List<ChallengeTokenType> ChallengeTokensEasy { get; init; }
        [JsonProperty("ChallengeTokensNormal")] public List<ChallengeTokenType> ChallengeTokensNormal { get; init; }
        [JsonProperty("ChallengeTokensHard")] public List<ChallengeTokenType> ChallengeTokensHard { get; init; }
        [JsonProperty("ChallengeTokensExpert")] public List<ChallengeTokenType> ChallengeTokensExpert { get; init; }
        [JsonProperty("AncientTokenDescriptionNormal")] public string AncientTokenDescriptionNormal { get; init; }
        [JsonProperty("CreatureTokenDescriptionNormal")] public string CreatureTokenDescriptionNormal { get; init; }
        [JsonProperty("DangerTokenDescriptionNormal")] public string DangerTokenDescriptionNormal { get; init; }
        [JsonProperty("CultistTokenDescriptionNormal")] public string CultistTokenDescriptionNormal { get; init; }
        [JsonProperty("AncientTokenDescriptionHard")] public string AncientTokenDescriptionHard { get; init; }
        [JsonProperty("CreatureTokenDescriptionHard")] public string CreatureTokenDescriptionHard { get; init; }
        [JsonProperty("DangerTokenDescriptionHard")] public string DangerTokenDescriptionHard { get; init; }
        [JsonProperty("CultistTokenDescriptionHard")] public string CultistTokenDescriptionHard { get; init; }

        public IEnumerable<CardPlace> PlaceCards => Cards.OfType<CardPlace>();
        public IEnumerable<CardPlot> PlotCards => Cards.OfType<CardPlot>();
        public IEnumerable<CardGoal> GoalCards => Cards.OfType<CardGoal>();
        public IEnumerable<Card> DangerCards => Cards.FindAll(card => card is CardAdversity || card is CardCreature);

    }
}
