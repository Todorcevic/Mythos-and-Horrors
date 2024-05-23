using Newtonsoft.Json;
using System.Collections.Generic;

namespace MythosAndHorrors.GameRules
{
    public class Owner
    {
        [JsonProperty("Cards")] public List<Card> Cards { get; init; }
        public List<Zone> Zones { get; init; } = new();

        /*******************************************************************/
        //public bool HasThisZone(Zone zone) => Zones.Contains(zone);

        //public bool HasThisCard(Card card) => Cards.Contains(card);
    }
}
