using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01522 : CardConditionFast
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Insight, Tag.Criminal };
        protected override GameActionTime FastReactionAtStart => GameActionTime.After;

        /*******************************************************************/
        protected override bool CanPlayFromHandSpecific(GameAction gameAction)
        {
            if (gameAction is not DefeatCardGameAction defeatCardGameAction) return false;
            if (defeatCardGameAction.Card is not CardCreature) return false;
            if (defeatCardGameAction.ByThisInvestigator != ControlOwner) return false;
            if (ControlOwner.CurrentPlace.Hints.Value < 1) return false;
            return true;
        }

        protected override async Task ExecuteConditionEffect(GameAction gameAction, Investigator investigator)
        {
            await _gameActionsProvider.Create<GainHintGameAction>().SetWith(investigator, investigator.CurrentPlace.Hints, amount: 1).Start();
        }
    }
}
