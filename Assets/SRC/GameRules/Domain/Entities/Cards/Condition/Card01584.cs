using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01584 : CardConditionReaction
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Fortune };
        protected override GameActionTime FastReactionAtStart => GameActionTime.Before;
        protected override Localization Localization => new("OptativeReaction_Card01584");

        /*******************************************************************/
        protected override bool CanPlayFromHandSpecific(GameAction gameAction)
        {
            if (gameAction is not FinishChallengeControlGameAction finishChallenge) return false;
            if (finishChallenge.ChallengePhaseGameAction.ActiveInvestigator != ControlOwner) return false;
            if (finishChallenge.ChallengePhaseGameAction.ResultChallenge.IsSuccessful ?? true) return false;
            return true;
        }

        protected override async Task ExecuteConditionEffect(GameAction gameAction, Investigator investigator)
        {
            if (gameAction is not FinishChallengeControlGameAction finishChallenge) return;
            await _gameActionsProvider.Create<IncrementStatGameAction>().SetWith(finishChallenge.ChallengePhaseGameAction.StatModifier, 2).Execute();
            await _gameActionsProvider.Create<IncrementStatGameAction>().SetWith(finishChallenge.ChallengePhaseGameAction.Stat, 2).Execute();
            await finishChallenge.ChallengePhaseGameAction.ResultChallenge.Execute();
            await _gameActionsProvider.Create<DrawAidGameAction>().SetWith(investigator).Execute();
        }
    }
}
