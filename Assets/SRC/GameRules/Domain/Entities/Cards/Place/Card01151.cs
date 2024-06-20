using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01151 : CardPlace
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Woods };

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            CreateForceReaction<MoveInvestigatorToPlaceGameAction>(ChallengeLogic, ChallengeCondition, GameActionTime.Before);
        }

        /*******************************************************************/
        private async Task ChallengeCondition(MoveInvestigatorToPlaceGameAction moveInvestigatorToPlaceGameAction)
        {
            await _gameActionsProvider.Create(new SafeForeach<Investigator>(InvestigatorsToChallenge, ChallengeLogic));

            /*******************************************************************/
            IEnumerable<Investigator> InvestigatorsToChallenge() => moveInvestigatorToPlaceGameAction.Investigators
                .Where(investigator => moveInvestigatorToPlaceGameAction.From[investigator] != this);

            async Task ChallengeLogic(Investigator investigator)
            {
                await _gameActionsProvider.Create(new ChallengePhaseGameAction(investigator.Intelligence, 3, "Enter Forest", this, failEffect: CantMove));

                async Task CantMove()
                {
                    moveInvestigatorToPlaceGameAction.Cancel();
                    await Task.CompletedTask;
                }
            }
        }

        private bool ChallengeLogic(MoveInvestigatorToPlaceGameAction moveInvestigatorToPlaceGameAction)
        {
            if (moveInvestigatorToPlaceGameAction.CardPlace != this) return false;
            return true;
        }
    }
}
