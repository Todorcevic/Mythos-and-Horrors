using Sirenix.OdinInspector;
using System;
using Unity.Plastic.Newtonsoft.Json;
using UnityEngine;

namespace GameRules
{
    public class Card
    {
        [SerializeField] public string Id { get; init; }
        [SerializeField] public string Name { get; init; }
        [SerializeField] public CardType Type { get; init; }
        public Zone CurrentZone { get; private set; }

        [BoxGroup("Split/right")]
        [SerializeField]
        [JsonProperty("is_complete")]
        public bool IsComplete { get; set; }

        /*******************************************************************/
        public void MoveToZone(Zone zone)
        {
            CurrentZone = zone ?? throw new ArgumentNullException(nameof(zone));
        }
    }
}
