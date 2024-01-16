using System.Collections.Generic;
using System.Linq;

namespace MythsAndHorrors.GameRules
{
    public class Investigator
    {
        public CardInvestigator InvestigatorCard { get; init; }
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
        public Zone HandZone { get; } = new Zone();
        public Zone DeckZone { get; } = new Zone();
        public Zone DiscardZone { get; } = new Zone();
        public Zone AidZone { get; } = new Zone();
        public Zone DangerZone { get; } = new Zone();
        public Zone InvestigatorZone { get; } = new Zone();

        /*******************************************************************/
        public bool HasThisZone(Zone zone) =>
            zone == HandZone ||
            zone == DeckZone ||
            zone == DiscardZone ||
            zone == AidZone ||
            zone == DangerZone ||
            zone == InvestigatorZone;

        public bool HasThisCard(Card card) => CardsWithInvestigator.Contains(card);
    }
}
