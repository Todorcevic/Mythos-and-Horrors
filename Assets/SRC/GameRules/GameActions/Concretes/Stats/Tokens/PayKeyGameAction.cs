using System.Collections.Generic;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class PayKeyGameAction : GameAction
    {
        public Investigator Investigator { get; private set; }
        public Stat ToStat { get; private set; }
        public int Amount { get; private set; }
        public override bool CanBeExecuted => Investigator.IsInPlay.IsTrue && Amount > 0;

        /*******************************************************************/
        public PayKeyGameAction SetWith(Investigator investigator, Stat toStat, int amount)
        {
            Investigator = investigator;
            ToStat = toStat;
            Amount = Investigator.Keys.Value < amount ? Investigator.Keys.Value : amount; ;
            return this;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            Dictionary<Stat, int> statablesUpdated = new()
            {
                { ToStat, ToStat.Value - Amount },
                { Investigator.Keys, Investigator.Keys.Value - Amount}
            };

            await _gameActionsProvider.Create<UpdateStatGameAction>().SetWith(statablesUpdated).Execute();
        }
    }
}
