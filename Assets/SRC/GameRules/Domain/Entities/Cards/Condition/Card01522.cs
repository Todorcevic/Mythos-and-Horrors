using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01522 : CardConditionFast
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Insight, Tag.Criminal };
        protected override bool FastReactionAtStart => false;

        /*******************************************************************/
        protected override async Task ExecuteConditionEffect(Investigator investigator)
        {
            await _gameActionsProvider.Create(new GainHintGameAction(investigator, investigator.CurrentPlace.Hints, amount: 1));
        }

        protected override bool CanPlayFromHandOverride(GameAction gameAction)
        {
            if (gameAction is not DefeatCardGameAction defeatCardGameAction) return false;
            if (defeatCardGameAction.Card is not CardCreature) return false;
            if (defeatCardGameAction.ByThisCard != ControlOwner.InvestigatorCard) return false;
            if (ControlOwner.CurrentPlace.Hints.Value < 1) return false;
            return true;
        }
    }
}
