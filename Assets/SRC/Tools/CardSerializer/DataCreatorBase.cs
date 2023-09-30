using GameRules;
using Sirenix.OdinInspector;
using System;
using Unity.Plastic.Newtonsoft.Json;
using UnityEngine;

namespace Tools
{
    public class DataCreatorBase
    {
        [HorizontalGroup("Split", 0.5f)]
        [BoxGroup("Split/Left", ShowLabel = false)]
        [ShowInInspector]
        public CardType CardType { get; set; }

        [BoxGroup("Split/Left", ShowLabel = false)]
        [SerializeField]
        public string Code { get; set; }

        [BoxGroup("Split/Left", ShowLabel = false)]
        [SerializeField]
        public string Name { get; set; }

        [JsonIgnore]
        public bool IsIncomplete => string.IsNullOrEmpty(Code) || string.IsNullOrEmpty(Name);

        public bool Contains(string word) => Code.Contains(word, StringComparison.OrdinalIgnoreCase)
                || Name.Contains(word, StringComparison.OrdinalIgnoreCase)
                || CardType.ToString().Contains(word, StringComparison.OrdinalIgnoreCase);
    }
}
