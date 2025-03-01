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
        [Inject] private readonly ChaptersProvider _chaptersProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Tome, Tag.Item };

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            CreateActivation(1, Logic, Condition, PlayActionType.Activate, new Localization("Activation_Card01531"));
        }

        /*******************************************************************/
        private bool Condition(Investigator investigator)
        {
            if (IsInPlay.IsFalse) return false;
            if (ControlOwner != investigator) return false;
            if (Exausted.IsActive) return false;
            return true;
        }

        private async Task Logic(Investigator investigator)
        {
            InteractableGameAction interactableGameAction = _gameActionsProvider.Create<InteractableGameAction>()
                .SetWith(canBackToThisInteractable: false, mustShowInCenter: true, new Localization("Interactable_Card01531"));
            foreach (Investigator inv in _investigatorsProvider.GetInvestigatorsInThisPlace(investigator.CurrentPlace))
            {
                interactableGameAction.CreateCardEffect(inv.InvestigatorCard, new Stat(0, false), SelecteInvestigator, PlayActionType.Choose,
                    investigator, new Localization("CardEffect_Card01531"));

                /*******************************************************************/
                async Task SelecteInvestigator()
                {
                    InteractableGameAction interactableGameAction2 = _gameActionsProvider.Create<InteractableGameAction>()
                        .SetWith(canBackToThisInteractable: false, mustShowInCenter: true, new Localization("Interactable_Card01531-2"));
                    List<Card> cardsToShow = inv.DeckZone.Cards.TakeLast(3).ToList();

                    Card cardSelected = null;
                    foreach (Card card in cardsToShow)
                    {
                        interactableGameAction2.CreateCardEffect(card, new Stat(0, false), Draw, PlayActionType.Choose, inv, new Localization("CardEffect_Card01531-1"));

                        /*******************************************************************/
                        async Task Draw()
                        {
                            cardSelected = card;
                            await Task.CompletedTask;
                        }
                    }

                    await _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(Exausted, true).Execute();
                    await _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(cardsToShow, _chaptersProvider.CurrentScene.LimboZone).Execute();
                    await interactableGameAction2.Execute();
                    await _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(cardsToShow.Except(cardSelected), investigator.DeckZone, isFaceDown: true).Execute();
                    await _gameActionsProvider.Create<ShuffleGameAction>().SetWith(inv.DeckZone).Execute();
                    if (cardSelected != null) await _gameActionsProvider.Create<DrawGameAction>().SetWith(inv, cardSelected).Execute();
                }
            }

            await interactableGameAction.Execute();
        }
    }
}
