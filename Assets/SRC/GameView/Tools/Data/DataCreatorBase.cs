using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Tools
{
    public abstract record DataCreatorBase
    {
        [HorizontalGroup("Split", 0.5f)]
        [BoxGroup("Split/left")]
        [SerializeField]
        public string Code { get; init; }

        [BoxGroup("Split/left")]
        [SerializeField]
        public string Name { get; init; }

        public bool IsEditable { get; set; }
        public bool IsComplete { get; set; }

        public bool Contains(string word)
        {
            return Code.Contains(word, StringComparison.OrdinalIgnoreCase)
                || Name.Contains(word, StringComparison.OrdinalIgnoreCase);
        }
    }
}
