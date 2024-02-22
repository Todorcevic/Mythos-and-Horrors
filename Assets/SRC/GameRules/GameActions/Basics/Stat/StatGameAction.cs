using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class StatGameAction : GameAction
    {
        [Inject] private readonly IPresenter<StatGameAction> _StatsPresenter;

        public Stat Stat { get; }
        public int Value { get; }

        /*******************************************************************/
        public StatGameAction(Stat stat, int value)
        {
            Stat = stat;
            Value = value;
        }

        protected override async Task ExecuteThisLogic()
        {
            await _StatsPresenter.PlayAnimationWith(this);
        }
    }
}
