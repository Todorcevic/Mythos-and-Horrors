using Sirenix.OdinInspector;
using System;
using Unity.Plastic.Newtonsoft.Json;
using UnityEngine;

namespace Tools
{
    public abstract class DataCreatorBase
    {
        [HorizontalGroup("Split", 0.5f)]
        [BoxGroup("Split/left", ShowLabel = false)]
        [SerializeField]
        public string Code { get; set; }

        [BoxGroup("Split/left")]
        [SerializeField]
        public string Name { get; set; }

        [BoxGroup("Split/Right", ShowLabel = false)]
        [SerializeField]
        public string Type { get; set; }

        [JsonIgnore]
        public bool IsIncomplete => string.IsNullOrEmpty(Code) || string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Type);

        public bool Contains(string word) => Code.Contains(word, StringComparison.OrdinalIgnoreCase)
                || Name.Contains(word, StringComparison.OrdinalIgnoreCase)
                || Type.Contains(word, StringComparison.OrdinalIgnoreCase);
    }
}
