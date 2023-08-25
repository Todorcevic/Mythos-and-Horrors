using Sirenix.OdinInspector;
using UnityEngine;

namespace Tools
{
    public record CardData : DataCreatorBase
    {
        [BoxGroup("Split/Right")]
        [SerializeField]
        public string Type { get; init; }
    }

}
