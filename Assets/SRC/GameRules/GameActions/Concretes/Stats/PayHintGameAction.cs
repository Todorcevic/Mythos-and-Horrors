using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class PayHintGameAction : GameAction
    {
        [Inject] private readonly GameActionProvider _gameActionFactory;

        public Investigator Investigator { get; }
        public Stat ToStat { get; }
        public int Amount { get; }

        /*******************************************************************/
        public PayHintGameAction(Investigator investigator, Stat toStat, int amount)
        {
            Investigator = investigator;
            ToStat = toStat;
            Amount = investigator.Hints.Value < amount ? Investigator.Hints.Value : amount;
            CanBeExecuted = Amount > 0;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            Dictionary<Stat, int> statablesUpdated = new()
            {
                { ToStat, ToStat.Value - Amount },
                { Investigator.Hints, Investigator.Hints.Value - Amount}
            };

            await _gameActionFactory.Create(new UpdateStatGameAction(statablesUpdated));
        }
    }
}
