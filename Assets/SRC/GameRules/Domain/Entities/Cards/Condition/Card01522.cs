using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01522 : CardConditionReaction
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Insight, Tag.Criminal };
        protected override GameActionTime FastReactionAtStart => GameActionTime.After;

        protected override Localization Localization => new("OptativeReaction_Card01522");

        /*******************************************************************/
        protected override bool CanPlayFromHandSpecific(GameAction gameAction)
        {
            if (gameAction is not DefeatCardGameAction defeatCardGameAction) return false;
            if (defeatCardGameAction.Card is not CardCreature) return false;
            if (defeatCardGameAction.ByThisInvestigator != ControlOwner) return false;
            if (ControlOwner.CurrentPlace.Keys.Value < 1) return false;
            return true;
        }

        protected override async Task ExecuteConditionEffect(GameAction gameAction, Investigator investigator)
        {
            await _gameActionsProvider.Create<GainKeyGameAction>().SetWith(investigator, investigator.CurrentPlace.Keys, amount: 1).Execute();
        }
    }
}
