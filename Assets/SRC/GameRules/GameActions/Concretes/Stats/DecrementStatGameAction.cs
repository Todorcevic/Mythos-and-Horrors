using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class DecrementStatGameAction : GameAction
    {
        [Inject] private readonly IStatAnimator _statAnimator;

        public Stat Stat { get; private set; }
        public int Value { get; private set; }

        /*******************************************************************/
        public async Task Run(Stat stat, int value)
        {
            if (value == 0) return;
            Stat = stat;
            Value = value;
            await Start();
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            Stat.Decrease(Value);
            await _statAnimator.DecreaseStat(Stat);
        }
    }
}
