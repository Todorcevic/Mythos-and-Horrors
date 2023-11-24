using MythsAndHorrors.GameRules;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class AreaAdventurerView : MonoBehaviour
    {
        public Adventurer Adventurer { get; private set; }

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Used by Injection")]
        private void Init(Adventurer adventurer)
        {
            Adventurer = adventurer;
        }
    }
}
