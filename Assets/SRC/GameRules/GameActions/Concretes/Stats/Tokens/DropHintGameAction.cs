using System.Collections.Generic;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class DropHintGameAction : GameAction
    {
        public Investigator Investigator { get; private set; }
        public Stat ToStat { get; private set; }
        public int Amount { get; private set; }
        public override bool CanBeExecuted => Amount > 0;

        /*******************************************************************/
        public DropHintGameAction SetWith(Investigator investigator, Stat toStat, int amount)
        {
            Investigator = investigator;
            ToStat = toStat;
            Amount = Investigator.Hints.Value < amount ? Investigator.Hints.Value : amount;
            return this;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            Dictionary<Stat, int> statablesUpdated = new()
            {
                { ToStat, ToStat.Value + Amount },
                { Investigator.Hints, Investigator.Hints.Value - Amount}
            };

            await _gameActionsProvider.Create<UpdateStatGameAction>().SetWith(statablesUpdated).Start();
        }
    }
}
