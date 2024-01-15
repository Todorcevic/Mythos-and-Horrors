using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class GainHintGameAction : GameAction
    {
        [Inject] private readonly IHintAnimator _hintAnimator;
        [Inject] private readonly GameActionFactory _gameActionFactory;

        public Investigator Investigator { get; private set; }
        public Stat FromStat { get; private set; }
        public int Amount { get; private set; }

        /*******************************************************************/
        public async Task Run(Investigator investigator, Stat fromStat, int amount)
        {
            Investigator = investigator;
            FromStat = fromStat;
            Amount = fromStat.Value < amount ? fromStat.Value : amount;
            await Start();
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionFactory.Create<UpdateStatGameAction>().Run(FromStat, FromStat.Value - Amount);
            await _hintAnimator.GainHints(Investigator, Amount, FromStat);
            await _gameActionFactory.Create<UpdateStatGameAction>().Run(Investigator.Hints, Investigator.Hints.Value + Amount);
        }
    }
}
