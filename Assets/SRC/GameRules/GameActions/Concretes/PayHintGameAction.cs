using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class PayHintGameAction : GameAction
    {
        [Inject] private readonly IHintAnimator _hintAnimator;
        [Inject] private readonly GameActionFactory _gameActionFactory;

        public Investigator Investigator { get; private set; }
        public Stat ToStat { get; private set; }
        public int Amount { get; private set; }

        /*******************************************************************/
        public async Task Run(Investigator investigator, Stat toStat, int amount)
        {
            Investigator = investigator;
            ToStat = toStat;
            Amount = investigator.Hints.Value < amount ? Investigator.Hints.Value : amount;
            await Start();
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionFactory.Create<UpdateStatGameAction>().Run(Investigator.Hints, Investigator.Hints.Value - Amount);
            await _hintAnimator.GainHints(Investigator, Amount, ToStat);
            await _gameActionFactory.Create<UpdateStatGameAction>().Run(ToStat, ToStat.Value + Amount);
        }
    }
}
