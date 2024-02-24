using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class PayResourceGameAction : GameAction
    {
        [Inject] private readonly IPresenter<PayResourceGameAction> _payResourcePresenter;
        [Inject] private readonly GameActionFactory _gameActionFactory;

        public Investigator Investigator { get; }
        public Stat ToStat { get; }
        public int Amount { get; }
        protected override bool CanBeExecuted => Amount > 0;

        /*******************************************************************/
        public PayResourceGameAction(Investigator investigator, Stat toStat, int amount)
        {
            Investigator = investigator;
            ToStat = toStat;
            Amount = investigator.Resources.Value < amount ? Investigator.Resources.Value : amount;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionFactory.Create(new DecrementStatGameAction(Investigator.Resources, Amount));
            await _payResourcePresenter.PlayAnimationWith(this);
            await _gameActionFactory.Create(new IncrementStatGameAction(ToStat, Amount));
        }
    }
}
