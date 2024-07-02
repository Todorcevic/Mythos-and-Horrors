using System;
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
            CreateActivation(1, Logic, Condition, PlayActionType.Activate);
        }

        /*******************************************************************/
        private bool Condition(Investigator investigator)
        {
            if (!IsInPlay) return false;
            if (ControlOwner != investigator) return false;
            if (Exausted.IsActive) return false;
            return true;
        }

        private async Task Logic(Investigator investigator)
        {
            InteractableGameAction interactableGameAction = _gameActionsProvider.Create<InteractableGameAction>()
                .SetWith(canBackToThisInteractable: false, mustShowInCenter: true, "Choose Investigator");
            foreach (Investigator inv in _investigatorsProvider.GetInvestigatorsInThisPlace(investigator.CurrentPlace))
            {
                interactableGameAction.CreateEffect(inv.InvestigatorCard, new Stat(0, false), SelecteInvestigator, PlayActionType.Choose, investigator);

                /*******************************************************************/
                async Task SelecteInvestigator()
                {
                    List<Card> cardsToShow = inv.DeckZone.Cards.TakeLast(3).ToList();
                    await _gameActionsProvider.Create<ShowCardsGameAction>().SetWith(cardsToShow).Start();
                    InteractableGameAction interactableGameAction2 = _gameActionsProvider.Create<InteractableGameAction>()
                        .SetWith(canBackToThisInteractable: false, mustShowInCenter: true, "Choose Card");

                    foreach (Card card in cardsToShow)
                    {
                        interactableGameAction2.CreateEffect(card, new Stat(0, false), Draw, PlayActionType.Choose, inv);

                        /*******************************************************************/
                        async Task Draw()
                        {
                            await _gameActionsProvider.Create<DrawGameAction>().SetWith(inv, card).Start();
                            await _gameActionsProvider.Create<ShuffleGameAction>().SetWith(inv.DeckZone).Start();
                            await _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(Exausted, true).Start();
                            await _gameActionsProvider.Create<HideCardsGameAction>().SetWith(cardsToShow.Except(new[] { card })).Start();
                            await DecrementCost(card, inv);
                        }
                    }

                    await interactableGameAction2.Start();
                }
            }

            await interactableGameAction.Start();
        }

        private async Task DecrementCost(Card card, Investigator investigator) //TODO: Implementar como original
        {
            if (Charge.IsEmpty) return;
            if (card is not IPlayableFromHand playableFromHand) return;
            await _gameActionsProvider.Create<DecrementStatGameAction>().SetWith(playableFromHand.ResourceCost, 2).Start();

            if (playableFromHand.PlayFromHandCondition.IsTrueWith(investigator))
            {
                InteractableGameAction interactableGameAction = _gameActionsProvider.Create<InteractableGameAction>()
                    .SetWith(canBackToThisInteractable: true, mustShowInCenter: true, "SpendCharge");
                interactableGameAction.CreateContinueMainButton();
                interactableGameAction.CreateEffect(this, new Stat(0, false), DecrementLogic, PlayActionType.Choose, investigator, resourceCost: playableFromHand.ResourceCost);

                async Task DecrementLogic()
                {
                    await _gameActionsProvider.Create<DecrementStatGameAction>().SetWith(Charge.Amount, 1).Start();
                    await playableFromHand.PlayFromHandCommand.RunWith(interactableGameAction);
                }

                await interactableGameAction.Start();
            }
            await _gameActionsProvider.Create<IncrementStatGameAction>().SetWith(playableFromHand.ResourceCost, 2).Start();
        }
    }
}