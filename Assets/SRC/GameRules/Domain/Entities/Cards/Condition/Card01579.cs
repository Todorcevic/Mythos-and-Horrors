using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01579 : CardConditionFast
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Fortune };
        protected override GameActionTime FastReactionAtStart => GameActionTime.After;

        /*******************************************************************/
        protected override bool CanPlayFromHandSpecific(GameAction gameAction)
        {
            if (gameAction is not InvestigatePlaceGameAction investigateGameAction) return false;
            if (investigateGameAction.ActiveInvestigator != ControlOwner) return false;
            if (investigateGameAction.IsSucceed) return false;
            if (investigateGameAction.ResultChallenge.TotalDifferenceValue < -2) return false;
            if (ControlOwner.CurrentPlace.Hints.Value < 1) return false;
            return true;
        }

        protected override async Task ExecuteConditionEffect(GameAction gameAction, Investigator investigator)
        {
            await _gameActionsProvider.Create<GainHintGameAction>().SetWith(investigator, investigator.CurrentPlace.Hints, 2).Execute();
        }
    }
}
