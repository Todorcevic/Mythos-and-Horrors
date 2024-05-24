using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace MythosAndHorrors.GameRules
{
    public class Owner
    {
        [JsonProperty("Cards")] public List<Card> Cards { get; init; }
        public List<Zone> Zones { get; init; } = new();

        public IEnumerable<Zone> FullZones => Zones.Concat(Cards.Select(card => card.OwnZone));

        /*******************************************************************/
        public bool HasThisZone(Zone zone) => FullZones.Contains(zone);

        public bool HasThisCard(Card card) => Cards.Contains(card);
    }
}
