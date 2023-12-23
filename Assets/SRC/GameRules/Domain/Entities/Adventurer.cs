using System.Collections.Generic;
using System.Linq;

namespace MythsAndHorrors.GameRules
{
    public class Adventurer
    {
        public Card AdventurerCard { get; set; }
        public List<Card> Cards { get; set; }
        public List<Card> RequerimentCard { get; set; }
        public List<Card> AllCards => Cards.Concat(RequerimentCard).Concat(new[] { AdventurerCard }).ToList();
        public Dictionary<Faction, int> DeckBuildingConditions { get; set; }
        public int DeckSize { get; set; }
        public int Xp { get; set; }
        public int Injury { get; set; }
        public int Shock { get; set; }
        public Zone HandZone { get; } = new Zone();
        public Zone DeckZone { get; } = new Zone();
        public Zone DiscardZone { get; } = new Zone();
        public Zone AidZone { get; } = new Zone();
        public Zone DangerZone { get; } = new Zone();
        public Zone AdventurerZone { get; } = new Zone();

        public bool HasThisZone(Zone zone) =>
            zone == HandZone ||
            zone == DeckZone ||
            zone == DiscardZone ||
            zone == AidZone ||
            zone == DangerZone ||
            zone == AdventurerZone;

        public bool HasThisCard(Card card) => AllCards.Contains(card);
    }
}
