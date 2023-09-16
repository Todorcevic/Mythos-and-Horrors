using Sirenix.OdinInspector;
using UnityEngine;

namespace Tools
{
    public class Boss : Tuesday
    {
        [BoxGroup("Split/Left")]
        [SerializeField]
        public int Health { get; set; }

        [BoxGroup("Split/Right")]
        [SerializeField]
        public int Damage { get; set; }
    }

}
