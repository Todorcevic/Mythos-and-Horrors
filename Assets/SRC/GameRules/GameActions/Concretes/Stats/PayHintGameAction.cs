using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{

    public class PayHintGameAction : GameAction
    {
        private readonly int _amount;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public Investigator Investigator { get; }
        public Stat ToStat { get; }
        public int Amount => Investigator.Hints.Value < _amount ? Investigator.Hints.Value : _amount;
        public override bool CanBeExecuted => Amount > 0;

        /*******************************************************************/
        public PayHintGameAction(Investigator investigator, Stat toStat, int amount)
        {
            Investigator = investigator;
            ToStat = toStat;
            _amount = amount;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            Dictionary<Stat, int> statablesUpdated = new()
            {
                { ToStat, ToStat.Value - Amount },
                { Investigator.Hints, Investigator.Hints.Value - Amount}
            };

            await _gameActionsProvider.Create(new UpdateStatGameAction(statablesUpdated));
        }
    }
}
