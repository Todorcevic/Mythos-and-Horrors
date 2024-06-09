using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01537 : CardCondition
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Insight };
        protected override bool IsFast => true;

        /*******************************************************************/
        protected override async Task ExecuteConditionEffect(Investigator investigator)
        {
            await _gameActionsProvider.Create(new GainHintGameAction(investigator, investigator.CurrentPlace.Hints, 1));
        }

        protected override bool CanPlayFromHandSpecific(GameAction gameAction)
        {
            if (ControlOwner.CurrentPlace.Hints.Value < 1) return false;
            return true;
        }
    }
}
