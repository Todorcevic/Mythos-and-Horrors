using Sirenix.OdinInspector;
using System;
using MythsAndHorrors.GameRules;
using Unity.Plastic.Newtonsoft.Json;
using UnityEngine;

namespace MythsAndHorrors.Tools
{
    public class DataCreatorBase
    {
        [HorizontalGroup("Split", 0.5f)]
        [BoxGroup("Split/Left", ShowLabel = false)]
        [ShowInInspector]
        public CardType CardType { get; set; }

        [BoxGroup("Split/Left")]
        [SerializeField]
        public string Code { get; set; }

        [BoxGroup("Split/Left")]
        [SerializeField]
        public string Name { get; set; }

        [JsonIgnore]
        public bool IsIncomplete => string.IsNullOrEmpty(Code) || string.IsNullOrEmpty(Name);

        public bool Contains(string word) => Code.Contains(word, StringComparison.OrdinalIgnoreCase)
                || Name.Contains(word, StringComparison.OrdinalIgnoreCase)
                || CardType.ToString().Contains(word, StringComparison.OrdinalIgnoreCase)
                || (this is CardInfo cardInfo && cardInfo.PackCode.Contains(word, StringComparison.OrdinalIgnoreCase));

        public bool ContainsFilters(string filter, CardType type = CardType.None)
        {
            return (this is CardInfo cardInfo
              && (filter == string.Empty || cardInfo.PackCode == filter)
              && (type == CardType.None || type.HasFlag(cardInfo.CardType)));
        }
    }
}
