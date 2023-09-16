using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Tools
{
    public class Encounter : Tuesday
    {
        [BoxGroup("Split/Left")]
        [SerializeField]
        public int FreeDraw { get; set; }

        [BoxGroup("Split/Right")]
        [SerializeField]
        public int DiscardCost { get; set; }
    }
}
