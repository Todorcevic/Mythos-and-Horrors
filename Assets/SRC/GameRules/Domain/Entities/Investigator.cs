using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Unity.Plastic.Newtonsoft.Json;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class Investigator
    {
        [Inject] private readonly ZonesProvider _zonesProvider;

        [JsonProperty("InvestigatorCard")] public CardInvestigator InvestigatorCard { get; init; }
        [JsonProperty("AvatarCard")] public CardAvatar AvatarCard { get; init; }
        [JsonProperty("Cards")] public List<Card> Cards { get; init; }
        [JsonProperty("RequerimentCard")] public List<Card> RequerimentCard { get; init; }
        [JsonProperty("DeckBuildingConditions")] public Dictionary<Faction, int> DeckBuildingConditions { get; init; }
        public string Code => InvestigatorCard.Info.Code;
        public List<Card> FullDeck => Cards.Concat(RequerimentCard).ToList();
        public List<Card> AllCards => FullDeck.Concat(new[] { InvestigatorCard }).Concat(new[] { AvatarCard }).ToList();
        public Zone HandZone { get; private set; }
        public Zone DeckZone { get; private set; }
        public Zone DiscardZone { get; private set; }
        public Zone AidZone { get; private set; }
        public Zone DangerZone { get; private set; }
        public Zone InvestigatorZone { get; private set; }
        public SlotsManager SlotsManager { get; private set; } = new();

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Zenject injects this method")]
        private void Init()
        {
            HandZone = _zonesProvider.Create();
            DeckZone = _zonesProvider.Create();
            DiscardZone = _zonesProvider.Create();
            AidZone = _zonesProvider.Create();
            DangerZone = _zonesProvider.Create();
            InvestigatorZone = _zonesProvider.Create();

        }

        /*******************************************************************/
        public bool HasThisZone(Zone zone) =>
            zone == HandZone || HandZone.Cards.Select(card => card.OwnZone).Contains(zone) ||
            zone == DeckZone || DeckZone.Cards.Select(card => card.OwnZone).Contains(zone) ||
            zone == DiscardZone || DiscardZone.Cards.Select(card => card.OwnZone).Contains(zone) ||
            zone == AidZone || AidZone.Cards.Select(card => card.OwnZone).Contains(zone) ||
            zone == DangerZone || DangerZone.Cards.Select(card => card.OwnZone).Contains(zone) ||
            zone == InvestigatorZone || InvestigatorZone.Cards.Select(card => card.OwnZone).Contains(zone);

        public bool HasThisCard(Card card) => AllCards.Contains(card);
    }
}
