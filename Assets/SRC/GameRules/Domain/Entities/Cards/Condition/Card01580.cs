using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01580 : CardConditionReaction
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Fortune };
        protected override GameActionTime FastReactionAtStart => GameActionTime.Before;

        protected override Localization Localization => new("OptativeReaction_Card01580");

        /*******************************************************************/
        protected override bool CanPlayFromHandSpecific(GameAction gameAction)
        {
            if (gameAction is not FinishChallengeControlGameAction finishChallengeGameAction) return false;
            if (finishChallengeGameAction.ChallengePhaseGameAction.ActiveInvestigator != ControlOwner) return false;
            if (finishChallengeGameAction.ChallengePhaseGameAction.ResultChallenge.IsSuccessful ?? true) return false;
            return true;
        }

        protected override async Task ExecuteConditionEffect(GameAction gameAction, Investigator investigator)
        {
            if (gameAction is not FinishChallengeControlGameAction finishChallengeGameAction) return;
            await _gameActionsProvider.Create<IncrementStatGameAction>().SetWith(finishChallengeGameAction.ChallengePhaseGameAction.StatModifier, 2).Execute();
            await finishChallengeGameAction.ChallengePhaseGameAction.ResultChallenge.Execute();
        }
    }
}
