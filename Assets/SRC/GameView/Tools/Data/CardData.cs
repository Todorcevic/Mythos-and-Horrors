using Sirenix.OdinInspector;
using UnityEngine;

namespace Tools
{
    public class CardData : DataCreatorBase
    {
        [BoxGroup("Split/Right")]
        [SerializeField]
        public int Cost { get; set; }
    }

}
