using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class CardGoal : Card
    {
        [Inject] private readonly List<History> _histories;

        public Stat Hints { get; private set; }
        public History InitialHistory => _histories[0];
        public History FinalHistory => _histories[1];

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Used by Injection")]
        private void Init()
        {
            Hints = new Stat(Info.Hints ?? 0, Info.Hints ?? 0);
        }

        /*******************************************************************/
    }
}
