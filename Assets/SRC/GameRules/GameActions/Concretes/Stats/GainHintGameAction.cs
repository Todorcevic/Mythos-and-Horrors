using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class GainHintGameAction : GameAction
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public Investigator Investigator { get; }
        public Stat FromStat { get; }
        public int Amount { get; }
        public override bool CanBeExecuted => Amount > 0;

        /*******************************************************************/
        public GainHintGameAction(Investigator investigator, Stat fromStat, int amount)
        {
            Investigator = investigator;
            FromStat = fromStat;
            Amount = fromStat.Value < amount ? fromStat.Value : amount;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            Dictionary<Stat, int> statablesUpdated = new()
            {
                { FromStat, FromStat.Value - Amount },
                { Investigator.Hints, Investigator.Hints.Value + Amount}
            };

            await _gameActionsProvider.Create(new UpdateStatGameAction(statablesUpdated));
        }
    }
}
