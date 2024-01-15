using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class UpdateStatGameAction : GameAction
    {
        [Inject] private readonly IStatAnimator _statAnimator;

        public Stat Stat { get; private set; }
        public int Value { get; private set; }

        /*******************************************************************/
        public async Task Run(Stat stat, int value)
        {
            Stat = stat;
            Value = value;
            await Start();
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            Stat.UpdateValue(Value);
            await _statAnimator.UpdateStat(Stat);
        }
    }
}
