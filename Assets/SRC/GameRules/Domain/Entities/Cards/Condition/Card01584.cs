using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01584 : CardConditionFast
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Fortune };
        protected override bool IsFast => true;
        protected override GameActionTime FastReactionAtStart => GameActionTime.After;

        /*******************************************************************/
        protected override bool CanPlayFromHandSpecific(GameAction gameAction)
        {
            if (gameAction is not ResultChallengeGameAction resultChallengeGameAction) return false;
            if (resultChallengeGameAction.ChallengePhaseGameAction.ActiveInvestigator != ControlOwner) return false;
            if (resultChallengeGameAction.IsSuccessful ?? true) return false;
            return true;
        }

        protected override async Task ExecuteConditionEffect(GameAction gameAction, Investigator investigator)
        {
            if (gameAction is not ResultChallengeGameAction resultChallengeGameAction) return;
            await _gameActionsProvider.Create(new IncrementStatGameAction(resultChallengeGameAction.ChallengePhaseGameAction.StatModifier, 2));
            await _gameActionsProvider.Create(new IncrementStatGameAction(resultChallengeGameAction.ChallengePhaseGameAction.Stat, 2));
            await _gameActionsProvider.Create(resultChallengeGameAction);
            await _gameActionsProvider.Create(new DrawAidGameAction(investigator));
        }
    }
}
