﻿using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01561 : CardSupply, IChargeable
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProvider;
        public override IEnumerable<Tag> Tags => new[] { Tag.Spell };

        public Charge Charge { get; private set; }

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            Charge = new Charge(3, ChargeType.MagicCharge);
            CreateActivation(1, Logic, Condition, PlayActionType.Activate, new Localization("Activation_Card01561"));
        }

        /*******************************************************************/
        private bool Condition(Investigator investigator)
        {
            if (IsInPlay.IsFalse) return false;
            if (Exausted.IsActive) return false;
            if (investigator != ControlOwner) return false;
            if (Charge.IsEmpty) return false;
            return true;
        }

        private async Task Logic(Investigator investigator)
        {
            InteractableGameAction interactableGameAction = _gameActionsProvider.Create<InteractableGameAction>()
                .SetWith(canBackToThisInteractable: false, mustShowInCenter: true, new Localization("Interactable_Card01561"));
            interactableGameAction.CreateCardEffect(_chaptersProvider.CurrentScene.CardDangerToDraw, new Stat(0, false),
                SelectDangerDeck, PlayActionType.Choose, investigator, new Localization("CardEffect_Card01561"));

            foreach (Investigator inv in _investigatorsProvider.AllInvestigatorsInPlay)
            {
                interactableGameAction.CreateCardEffect(inv.InvestigatorCard, new Stat(0, false), SelectDeck, PlayActionType.Choose, investigator, new Localization("CardEffect_Card01561-1"));

                async Task SelectDeck()
                {
                    IEnumerable<Card> cards = inv.DeckZone.Cards.TakeLast(3);
                    await _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(cards, _chaptersProvider.CurrentScene.LimboZone).Execute();
                    await SortCards(inv);
                }
            }
            await _gameActionsProvider.Create<DecrementStatGameAction>().SetWith(Charge.Amount, 1).Execute();
            await _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(Exausted, true).Execute();
            await interactableGameAction.Execute();
            await _gameActionsProvider.Create<StopUndoGameAction>().Execute();

            /*******************************************************************/
            async Task SelectDangerDeck()
            {
                IEnumerable<Card> cards = _chaptersProvider.CurrentScene.DangerDeckZone.Cards.TakeLast(3);
                await _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(cards, _chaptersProvider.CurrentScene.LimboZone).Execute();
                await SortCards(null);
            }
        }

        private async Task SortCards(Owner owner)
        {
            Zone zoneToReturn = owner is Investigator investigator ? investigator.DeckZone : _chaptersProvider.CurrentScene.DangerDeckZone;
            Card cardAffected = owner is Investigator investigator2 ? investigator2.InvestigatorCard : null;

            InteractableGameAction interactableGameAction = _gameActionsProvider.Create<InteractableGameAction>()
                .SetWith(canBackToThisInteractable: true, mustShowInCenter: true, new Localization("Interactable_Card01561-1"));

            Card cardSelected = null;
            foreach (Card card in _chaptersProvider.CurrentScene.LimboZone.Cards)
            {
                interactableGameAction.CreateCardEffect(card, new Stat(0, false), SelectCard, PlayActionType.Choose, ControlOwner, new Localization("CardEffect_Card01561-2"), cardAffected: cardAffected);

                async Task SelectCard()
                {
                    cardSelected = card;
                    await Task.CompletedTask;
                }
            }

            await interactableGameAction.Execute();
            if (cardSelected != null) await _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(cardSelected, zoneToReturn, isFaceDown: true).Execute();
            if (_chaptersProvider.CurrentScene.LimboZone.Cards.Any()) await SortCards(owner);
        }
    }
}
