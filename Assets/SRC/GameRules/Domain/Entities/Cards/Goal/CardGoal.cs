using System.Diagnostics.CodeAnalysis;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class CardGoal : Card, IRevelable
    {
        public Stat Hints { get; private set; }

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Used by Injection")]
        private void Init()
        {
            Hints = new Stat(Info.Hints ?? 0, Info.Hints ?? 0);
        }

        /*******************************************************************/
        void IRevelable.Reveal()
        {
            throw new System.NotImplementedException();
        }
    }
}
