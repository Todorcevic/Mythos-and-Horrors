using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01686 : CardSupply, IChargeable
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Tome, Tag.Item };

        public Charge Charge { get; private set; }

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            Charge = new Charge(2, ChargeType.Secret);
            CreateActivation(1, Logic, Condition, PlayActionType.Activate, "Activation_Card01686");
        }

        /*******************************************************************/
        private bool Condition(Investigator investigator)
        {
            if (!IsInPlay.IsTrue) return false;
            if (ControlOwner != investigator) return false;
            if (Exausted.IsActive) return false;
            return true;
        }

        private async Task Logic(Investigator investigator)
        {
            InteractableGameAction interactableGameAction = _gameActionsProvider.Create<InteractableGameAction>()
                .SetWith(canBackToThisInteractable: false, mustShowInCenter: true, "Interactable_Card01686");
            foreach (Investigator inv in _investigatorsProvider.GetInvestigatorsInThisPlace(investigator.CurrentPlace))
            {
                interactableGameAction.CreateCardEffect(inv.InvestigatorCard, new Stat(0, false), SelecteInvestigator,
                    PlayActionType.Choose, investigator, "CardEffect_Card01686");

                /*******************************************************************/
                async Task SelecteInvestigator()
                {
                    List<Card> cardsToShow = inv.DeckZone.Cards.TakeLast(3).ToList();
                    await _gameActionsProvider.Create<ShowCardsGameAction>().SetWith(cardsToShow).Execute();
                    InteractableGameAction interactableGameAction2 = _gameActionsProvider.Create<InteractableGameAction>()
                        .SetWith(canBackToThisInteractable: false, mustShowInCenter: true, "Interactable_Card01686-1");

                    foreach (Card card in cardsToShow)
                    {
                        interactableGameAction2.CreateCardEffect(card, new Stat(0, false), Draw, PlayActionType.Choose, inv, "CardEffect_Card01686-1");

                        /*******************************************************************/
                        async Task Draw()
                        {
                            await _gameActionsProvider.Create<HideCardsGameAction>().SetWith(cardsToShow.Except(new[] { card })).Execute();
                            await _gameActionsProvider.Create<DrawGameAction>().SetWith(inv, card).Execute();
                            await _gameActionsProvider.Create<ShuffleGameAction>().SetWith(inv.DeckZone).Execute();
                            await _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(Exausted, true).Execute();
                            await DecrementCost(card, inv);
                        }
                    }

                    await interactableGameAction2.Execute();
                }
            }

            await interactableGameAction.Execute();
        }

        private async Task DecrementCost(Card card, Investigator investigator)
        {
            if (Charge.IsEmpty) return;
            if (card is not IPlayableFromHand playableFromHand) return;
            if (!playableFromHand.PlayFromHandCondition.IsTrueWith(card.ControlOwner)) return;

            await _gameActionsProvider.Create<DecrementStatGameAction>().SetWith(playableFromHand.ResourceCost, 2).Execute();

            if (playableFromHand.PlayFromHandCondition.IsTrueWith(investigator))
            {
                InteractableGameAction interactableGameAction = _gameActionsProvider.Create<InteractableGameAction>()
                    .SetWith(canBackToThisInteractable: true, mustShowInCenter: true, "Interactable_Card01686-2");
                interactableGameAction.CreateContinueMainButton();
                interactableGameAction.CreateCardEffect(card, new Stat(0, false), DecrementLogic, PlayActionType.PlayFromHand,
                    card.ControlOwner, "CardEffect_Card01686-2", resourceCost: playableFromHand.ResourceCost);

                async Task DecrementLogic()
                {
                    await _gameActionsProvider.Create<DecrementStatGameAction>().SetWith(Charge.Amount, 1).Execute();
                    await playableFromHand.PlayFromHandCommand.RunWith(interactableGameAction);
                }

                await interactableGameAction.Execute();
            }
            await _gameActionsProvider.Create<IncrementStatGameAction>().SetWith(playableFromHand.ResourceCost, 2).Execute();
        }
    }
}