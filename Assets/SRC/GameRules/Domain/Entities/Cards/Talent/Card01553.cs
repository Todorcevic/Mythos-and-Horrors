using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01553 : CardTalent
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Innate };

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            CreateForceReaction<CommitCardsChallengeGameAction>(OwnChallengeCondition, OwnChallengeLogic, GameActionTime.Before);
        }

        /*******************************************************************/
        private async Task OwnChallengeLogic(CommitCardsChallengeGameAction commitCardsChallenge)
        {
            CardEffect effectToRemove = commitCardsChallenge.AllPlayableEffects.FirstOrDefault(effect => effect.CardOwner == this);
            if (effectToRemove == null) return;
            commitCardsChallenge.RemoveEffect(effectToRemove);
            await Task.CompletedTask;
        }

        private bool OwnChallengeCondition(CommitCardsChallengeGameAction commitCardChallengeGameAction)
        {
            if (commitCardChallengeGameAction.CurrentChallenge.ActiveInvestigator == ControlOwner) return false;
            if (CurrentZone.ZoneType != ZoneType.Hand) return false;
            return true;
        }

        /*******************************************************************/
        public override bool TalentCondition(ChallengePhaseGameAction challengePhaseGameAction)
        {
            if (challengePhaseGameAction.ResultChallenge.TotalDifferenceValue < 3) return false;
            return true;
        }

        public override async Task TalentLogic(ChallengePhaseGameAction challengePhaseGameAction)
        {
            challengePhaseGameAction.SuccesEffects.Add(ReturnCardToHand);
            await Task.CompletedTask;

            async Task ReturnCardToHand() =>
                await _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(this, challengePhaseGameAction.ActiveInvestigator.HandZone).Execute();
        }
    }
}
