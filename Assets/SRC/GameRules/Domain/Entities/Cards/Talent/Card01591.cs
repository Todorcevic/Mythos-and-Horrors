using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01591 : CardTalent
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Practiced };

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
            if (!challengePhaseGameAction.IsSucceed) return false;
            return true;
        }

        public override async Task TalentLogic(ChallengePhaseGameAction challengePhaseGameAction)
        {
            challengePhaseGameAction.SuccesEffects.Add(Draw);
            await Task.CompletedTask;

            async Task Draw() => await _gameActionsProvider.Create<DrawAidGameAction>().SetWith(InvestigatorCommiter).Start();
        }
    }
}
