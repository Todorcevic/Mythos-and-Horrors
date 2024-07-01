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
            await _gameActionsProvider.Create<SafeForeach<Investigator>>().SetWith(InvestigatorsToChallenge, ChallengeLogic).Start();

            /*******************************************************************/
            IEnumerable<Investigator> InvestigatorsToChallenge() =>
                moveInvestigatorToPlaceGameAction.Investigators
                .Where(investigator => moveInvestigatorToPlaceGameAction.From[investigator] != this);


            async Task ChallengeLogic(Investigator investigator)
            {
                await _gameActionsProvider.Create(new ChallengePhaseGameAction(investigator.Power, 3, "Graveyar", this, failEffect: TakeFearAndmove));

                async Task TakeFearAndmove()
                {
                    CardPlace PlaceToReturn = _cardsProvider.GetCard<Card01125>();
                    await _gameActionsProvider.Create(new HarmToInvestigatorGameAction(investigator, this, amountFear: 2));
                    await _gameActionsProvider.Create(new MoveInvestigatorToPlaceGameAction(investigator, PlaceToReturn));
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
