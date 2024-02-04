using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class Investigator
    {
        public string Code => InvestigatorCard.Info.Code;
        public CardInvestigator InvestigatorCard { get; init; }
        public CardAvatar AvatarCard { get; init; }
        public List<Card> Cards { get; init; }
        public List<Card> RequerimentCard { get; init; }
        public List<Card> FullDeck => Cards.Concat(RequerimentCard).ToList();
        public List<Card> CardsWithInvestigator => FullDeck.Concat(new[] { InvestigatorCard }).ToList();
        public Dictionary<Faction, int> DeckBuildingConditions { get; init; }
        public Stat DeckSize { get; } = new Stat(30);
        public Stat Xp { get; } = new Stat(0);
        public Stat Injury { get; } = new Stat(0);
        public Stat Shock { get; } = new Stat(0);
        public Stat Resources { get; } = new Stat(0);
        public Stat Hints { get; } = new Stat(0);
        public Stat InitialHandSize { get; } = new Stat(5);
        public Stat Turns { get; } = new Stat(0, 3);
        public Zone HandZone { get; } = new Zone(ZoneType.Hand);
        public Zone DeckZone { get; } = new Zone(ZoneType.InvestigatorDeck);
        public Zone DiscardZone { get; } = new Zone(ZoneType.InvestigatorDiscard);
        public Zone AidZone { get; } = new Zone(ZoneType.Aid);
        public Zone DangerZone { get; } = new Zone(ZoneType.Danger);
        public Zone InvestigatorZone { get; } = new Zone(ZoneType.Investigator);

        /*******************************************************************/
        public bool HasThisZone(Zone zone) =>
            zone == HandZone || HandZone.Cards.Select(card => card.OwnZone).Contains(zone) ||
            zone == DeckZone || DeckZone.Cards.Select(card => card.OwnZone).Contains(zone) ||
            zone == DiscardZone || DiscardZone.Cards.Select(card => card.OwnZone).Contains(zone) ||
            zone == AidZone || AidZone.Cards.Select(card => card.OwnZone).Contains(zone) ||
            zone == DangerZone || DangerZone.Cards.Select(card => card.OwnZone).Contains(zone) ||
            zone == InvestigatorZone || InvestigatorZone.Cards.Select(card => card.OwnZone).Contains(zone);

        public bool HasThisCard(Card card) => CardsWithInvestigator.Contains(card);
    }
}
