using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class StatGameAction : GameAction
    {
        [Inject] private readonly IViewLayer _animator;

        public Stat Stat { get; private set; }
        public int Value { get; private set; }

        /*******************************************************************/
        public StatGameAction(Stat stat, int value)
        {
            Stat = stat;
            Value = value;  
        }

        protected override async Task ExecuteThisLogic()
        {
            await _animator.PlayAnimationWith(this);
        }
    }
}
