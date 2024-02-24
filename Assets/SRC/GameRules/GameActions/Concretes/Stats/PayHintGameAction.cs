using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class PayHintGameAction : GameAction
    {
        [Inject] private readonly IPresenter<PayHintGameAction> _payHintPresenter;
        [Inject] private readonly GameActionFactory _gameActionFactory;

        public Investigator Investigator { get; }
        public Stat ToStat { get; }
        public int Amount { get; }
        protected override bool CanBeExecuted => Amount > 0;

        /*******************************************************************/
        public PayHintGameAction(Investigator investigator, Stat toStat, int amount)
        {
            Investigator = investigator;
            ToStat = toStat;
            Amount = investigator.Hints.Value < amount ? Investigator.Hints.Value : amount;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionFactory.Create(new DecrementStatGameAction(Investigator.Hints, Amount));
            await _payHintPresenter.PlayAnimationWith(this);
            await _gameActionFactory.Create(new IncrementStatGameAction(ToStat, Amount));
        }
    }
}
