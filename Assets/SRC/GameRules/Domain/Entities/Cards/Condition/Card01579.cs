using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01579 : CardConditionReaction
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Fortune };
        protected override GameActionTime FastReactionAtStart => GameActionTime.After;
        protected override Localization Localization => new("OptativeReaction_Card01579");

        /*******************************************************************/
        protected override bool CanPlayFromHandSpecific(GameAction gameAction)
        {
            if (gameAction is not InvestigatePlaceGameAction investigateGameAction) return false;
            if (investigateGameAction.ActiveInvestigator != ControlOwner) return false;
            if (investigateGameAction.IsSucceed) return false;
            if (investigateGameAction.ResultChallenge.TotalDifferenceValue < -2) return false;
            if (ControlOwner.CurrentPlace.Keys.Value < 1) return false;
            return true;
        }

        protected override async Task ExecuteConditionEffect(GameAction gameAction, Investigator investigator)
        {
            await _gameActionsProvider.Create<GainKeyGameAction>().SetWith(investigator, investigator.CurrentPlace.Keys, 2).Execute();
        }
    }
}
