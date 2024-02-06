using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class Investigator
    {
        [Inject] private readonly ZonesProvider _zonesProvider;

        public string Code => InvestigatorCard.Info.Code;
        public CardInvestigator InvestigatorCard { get; init; }
        public CardAvatar AvatarCard { get; init; }
        public List<Card> Cards { get; init; }
        public List<Card> RequerimentCard { get; init; }
        public List<Card> FullDeck => Cards.Concat(RequerimentCard).ToList();
        public List<Card> AllCards => FullDeck.Concat(new[] { InvestigatorCard }).Concat(new[] { AvatarCard }).ToList();
        public Dictionary<Faction, int> DeckBuildingConditions { get; init; }
        public Zone HandZone { get; private set; }
        public Zone DeckZone { get; private set; }
        public Zone DiscardZone { get; private set; }
        public Zone AidZone { get; private set; }
        public Zone DangerZone { get; private set; }
        public Zone InvestigatorZone { get; private set; }

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
