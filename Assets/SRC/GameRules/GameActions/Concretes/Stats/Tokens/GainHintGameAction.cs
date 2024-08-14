using System.Collections.Generic;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class GainHintGameAction : GameAction
    {
        public Investigator Investigator { get; private set; }
        public Stat FromStat { get; private set; }
        public int Amount { get; private set; }
        public override bool CanBeExecuted => Investigator.IsInPlay && Amount > 0;

        /*******************************************************************/
        public GainHintGameAction SetWith(Investigator investigator, Stat fromStat, int amount)
        {
            Investigator = investigator;
            FromStat = fromStat;
            Amount = fromStat.Value < amount ? fromStat.Value : amount;
            return this;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            Dictionary<Stat, int> statablesUpdated = new()
            {
                { FromStat, FromStat.Value - Amount },
                { Investigator.Hints, Investigator.Hints.Value + Amount}
            };

            await _gameActionsProvider.Create<UpdateStatGameAction>().SetWith(statablesUpdated).Execute();
        }
    }
}
