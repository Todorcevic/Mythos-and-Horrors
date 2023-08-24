using Sirenix.OdinInspector;
using System;
using System.Linq;
using UnityEngine;

namespace GameRules
{
    public class CardDataModel
    {
        [HorizontalGroup("Split", 0.5f)]
        [BoxGroup("Split/left")]
        [SerializeField] public string Id { get; init; }
        [BoxGroup("Split/left")]
        [SerializeField] public string Name { get; init; }
        [BoxGroup("Split/Right")]
        [SerializeField] public string Type { get; init; }
        [BoxGroup("Split/Right")]
        [SerializeField] public string Image { get; init; }
        [BoxGroup("Split/left")]
        [SerializeField] public bool IsComplete { get; set; }

        public bool Contains(string word)
        {
            return Id.Contains(word, StringComparison.OrdinalIgnoreCase)
                || Name.Contains(word, StringComparison.OrdinalIgnoreCase)
                || Type.Contains(word, StringComparison.OrdinalIgnoreCase);
        }
    }
}
