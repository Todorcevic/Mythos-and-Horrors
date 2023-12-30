using System;
using System.Collections.Generic;
using System.Linq;

namespace MythsAndHorrors.GameRules
{
    public class Investigator
    {
        public Card InvestigatorCard { get; set; }
        public List<Card> Cards { get; set; }
        public List<Card> RequerimentCard { get; set; }
        public List<Card> FullDeck => Cards.Concat(RequerimentCard).ToList();
        public List<Card> AllCards => FullDeck.Concat(new[] { InvestigatorCard }).ToList();
        public Dictionary<Faction, int> DeckBuildingConditions { get; set; }
        public int DeckSize { get; set; }
        public int Xp { get; set; }
        public int Injury { get; set; }
        public int Shock { get; set; }
        public int Resources { get; private set; }
        public int Hints { get; set; }
        public Zone HandZone { get; } = new Zone();
        public Zone DeckZone { get; } = new Zone();
        public Zone DiscardZone { get; } = new Zone();
        public Zone AidZone { get; } = new Zone();
        public Zone DangerZone { get; } = new Zone();
        public Zone InvestigatorZone { get; } = new Zone();

        public bool HasThisZone(Zone zone) =>
            zone == HandZone ||
            zone == DeckZone ||
            zone == DiscardZone ||
            zone == AidZone ||
            zone == DangerZone ||
            zone == InvestigatorZone;

        public bool HasThisCard(Card card) => AllCards.Contains(card);

        public void RemoveResources(int amount)
        {
            if (amount > Resources) throw new InvalidOperationException("Not enough resources");
            Resources -= amount;
        }
    }
}
