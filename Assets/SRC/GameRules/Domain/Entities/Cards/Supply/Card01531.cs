using ModestTree;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01531 : CardSupply
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Tome, Tag.Item };

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            CreateFastActivation(Logic, Condition, PlayActionType.Activate);
        }

        /*******************************************************************/

        private bool Condition(Investigator investigator)
        {
            if (!IsInPlay) return false;
            if (Exausted.IsActive) return false;
            return true;
        }

        private async Task Logic(Investigator investigator)
        {
            InteractableGameAction interactableGameAction = new(canBackToThisInteractable: false, mustShowInCenter: true, "Choose Investigator");
            interactableGameAction.CreateCancelMainButton();
            foreach (Investigator inv in _investigatorsProvider.GetInvestigatorsInThisPlace(investigator.CurrentPlace))
            {
                interactableGameAction.CreateEffect(inv.InvestigatorCard, new Stat(0, false), SelecteInvestigator, PlayActionType.Choose, investigator);

                /*******************************************************************/
                async Task SelecteInvestigator()
                {
                    InteractableGameAction interactableGameAction2 = new(canBackToThisInteractable: false, mustShowInCenter: true, "Choose Card");
                    List<Card> cardsToShow = inv.DeckZone.Cards.TakeLast(3).ToList();

                    foreach (Card card in cardsToShow)
                    {
                        interactableGameAction2.CreateEffect(card, new Stat(0, false), Draw, PlayActionType.Choose, inv);

                        /*******************************************************************/
                        async Task Draw()
                        {
                            await _gameActionsProvider.Create(new DrawGameAction(inv, card));
                            await _gameActionsProvider.Create(new HideCardsGameAction(cardsToShow.Except(new[] { card })));
                        }
                    }

                    await _gameActionsProvider.Create(new ShowCardsGameAction(cardsToShow));
                    await _gameActionsProvider.Create(interactableGameAction2);
                    await _gameActionsProvider.Create(new ShuffleGameAction(inv.DeckZone));
                    await _gameActionsProvider.Create(new UpdateStatesGameAction(Exausted, true));
                }
            }

            await _gameActionsProvider.Create(interactableGameAction);
        }

        /*******************************************************************/
    }
}
