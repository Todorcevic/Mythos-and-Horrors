using System;
using UnityEngine;

namespace GameRules
{
    public class Card
    {
        [SerializeField] public string Id { get; init; }
        [SerializeField] public string Name { get; init; }
        [SerializeField] public CardType Type { get; init; }
        public CardInfo CardInfo { get; init; }
        public Zone CurrentZone { get; private set; }

        /*******************************************************************/
        public void MoveToZone(Zone zone)
        {
            CurrentZone = zone ?? throw new ArgumentNullException(nameof(zone));
        }
    }
}
