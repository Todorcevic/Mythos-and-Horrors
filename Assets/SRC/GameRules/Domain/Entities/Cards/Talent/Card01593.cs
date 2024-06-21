using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01593 : CardTalent
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Innate };

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            CreateForceReaction<CommitCardsChallengeGameAction>(CheckMaxCondition, CheckMaxLogic, GameActionTime.Before);
        }

        /*******************************************************************/
        private async Task CheckMaxLogic(CommitCardsChallengeGameAction commitCardsChallengeGameAction)
        {
            CardEffect effectToRemove = commitCardsChallengeGameAction.AllEffects.FirstOrDefault(effect => effect.CardOwner == this);
            if (effectToRemove == null) return;
            commitCardsChallengeGameAction.RemoveEffect(effectToRemove);
            await Task.CompletedTask;
        }

        private bool CheckMaxCondition(CommitCardsChallengeGameAction commitCardsChallengeGameAction)
        {
            if (commitCardsChallengeGameAction.CurrentChallenge.CurrentCommitsCards.All(commitableCard => commitableCard.Info.Code != Info.Code)) return false;
            return true;
        }

        /*******************************************************************/
        public override bool TalentCondition(ChallengePhaseGameAction challengePhaseGameAction)
        {
            return false;
        }

        public override async Task TalentLogic(ChallengePhaseGameAction challengePhaseGameAction)
        {
            await Task.CompletedTask;
        }
    }
}
