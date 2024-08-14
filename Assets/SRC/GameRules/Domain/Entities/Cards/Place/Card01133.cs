using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01133 : CardPlace
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly CardsProvider _cardsProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Arkham };

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            CreateForceReaction<MoveInvestigatorToPlaceGameAction>(Condition, Logic, GameActionTime.After);
        }

        /*******************************************************************/
        private async Task Logic(MoveInvestigatorToPlaceGameAction moveInvestigatorToPlaceGameAction)
        {
            await _gameActionsProvider.Create<SafeForeach<Investigator>>().SetWith(InvestigatorsToChallenge, ChallengeLogic).Execute();

            /*******************************************************************/
            IEnumerable<Investigator> InvestigatorsToChallenge() =>
                moveInvestigatorToPlaceGameAction.Investigators
                .Where(investigator => moveInvestigatorToPlaceGameAction.From[investigator] != this);


            async Task ChallengeLogic(Investigator investigator)
            {
                await _gameActionsProvider.Create<ChallengePhaseGameAction>().SetWith(investigator.Power, 3, "Challenge_Card01133", this, failEffect: TakeFearAndmove, localizableArgs: Info.Name).Execute();

                async Task TakeFearAndmove()
                {
                    CardPlace PlaceToReturn = _cardsProvider.GetCard<Card01125>();
                    await _gameActionsProvider.Create<HarmToInvestigatorGameAction>().SetWith(investigator, this, amountFear: 2).Execute();
                    await _gameActionsProvider.Create<MoveInvestigatorToPlaceGameAction>().SetWith(investigator, PlaceToReturn).Execute();
                }
            }
        }

        private bool Condition(MoveInvestigatorToPlaceGameAction moveInvestigatorToPlaceGameAction)
        {
            if (moveInvestigatorToPlaceGameAction.CardPlace != this) return false;
            return true;
        }
    }
}
