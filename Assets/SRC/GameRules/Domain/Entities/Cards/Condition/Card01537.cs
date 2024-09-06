using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01537 : CardConditionPlayFromHand
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public override bool IsFast => true;
        public override IEnumerable<Tag> Tags => new[] { Tag.Insight };

        /*******************************************************************/
        protected override async Task ExecuteConditionEffect(GameAction gameAction, Investigator investigator)
        {
            await _gameActionsProvider.Create<GainHintGameAction>().SetWith(investigator, investigator.CurrentPlace.Hints, 1).Execute();
        }

        protected override bool CanPlayFromHandSpecific(Investigator investigator)
        {
            if (ControlOwner.CurrentPlace.Hints.Value < 1) return false;
            return true;
        }
    }
}
