using System.Diagnostics.CodeAnalysis;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class CardScene : Card, IEffectable
    {
        public Stat ResourceCost { get; private set; }
        public Stat PileAmount { get; private set; }

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Used by Injection")]
        private void Init()
        {
            ResourceCost = new Stat(1);
            PileAmount = new Stat(int.MaxValue);
        }
    }
}
