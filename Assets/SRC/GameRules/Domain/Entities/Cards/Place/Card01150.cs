using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01150 : CardPlace
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Woods };

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            CreateForceReaction<MoveInvestigatorToPlaceGameAction>(ChallengeLogic, ChallengeCondition, GameActionTime.After);
        }

        /*******************************************************************/
        private async Task ChallengeCondition(MoveInvestigatorToPlaceGameAction moveInvestigatorToPlaceGameAction)
        {
            await _gameActionsProvider.Create<SafeForeach<Investigator>>().SetWith(InvestigatorsToChallenge, ChallengeLogic).Execute();

            /*******************************************************************/
            IEnumerable<Investigator> InvestigatorsToChallenge() => moveInvestigatorToPlaceGameAction.Investigators
                .Where(investigator => moveInvestigatorToPlaceGameAction.From[investigator] != this);

            async Task ChallengeLogic(Investigator investigator)
            {
                await _gameActionsProvider.Create<ChallengePhaseGameAction>().SetWith(investigator.Power, 4, "Challenge_Card01150", this, failEffect: TakeFearAndmove, localizableArgs: Info.Name).Execute();

                /*******************************************************************/
                async Task TakeFearAndmove() => await _gameActionsProvider.Create<HarmToInvestigatorGameAction>().SetWith(investigator, this, amountDamage: 1, amountFear: 1).Execute();
            }
        }

        private bool ChallengeLogic(MoveInvestigatorToPlaceGameAction moveInvestigatorToPlaceGameAction)
        {
            if (moveInvestigatorToPlaceGameAction.CardPlace != this) return false;
            return true;
        }
    }
}
