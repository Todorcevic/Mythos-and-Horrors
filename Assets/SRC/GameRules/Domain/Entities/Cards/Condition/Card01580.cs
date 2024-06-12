using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01580 : CardConditionTrigged
    {
        private ResultChallengeGameAction _resultChallengeGameAction;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Fortune };
        protected override bool IsFast => true;
        protected override bool FastReactionAtStart => false;

        /*******************************************************************/
        protected override async Task ExecuteConditionEffect(Investigator investigator)
        {
            await _gameActionsProvider.Create(new IncrementStatGameAction(_resultChallengeGameAction.ChallengePhaseGameAction.StatModifier, 2));
            await _gameActionsProvider.Create(new IncrementStatGameAction(_resultChallengeGameAction.ChallengePhaseGameAction.Stat, 2));
            await _gameActionsProvider.Create(_resultChallengeGameAction);
        }

        protected override bool CanPlayFromHandSpecific(GameAction gameAction)
        {
            if (gameAction is not ResultChallengeGameAction resultChallengeGameAction) return false;
            if (resultChallengeGameAction.ChallengePhaseGameAction.ActiveInvestigator != ControlOwner) return false;
            if (resultChallengeGameAction.IsSuccessful ?? true) return false;
            _resultChallengeGameAction = resultChallengeGameAction;
            return true;
        }

    }
}
