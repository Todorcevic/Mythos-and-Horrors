using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class StatGameAction : GameAction
    {
        [Inject] private readonly IAnimator _animator;

        public Stat Stat { get; private set; }
        public int Value { get; private set; }

        /*******************************************************************/
        public virtual async Task Run(Stat stat, int value)
        {
            Stat = stat;
            Value = value;
            await Start();
        }

        protected override async Task ExecuteThisLogic()
        {
            await _animator.PlayAnimationWith(this);
        }
    }
}
