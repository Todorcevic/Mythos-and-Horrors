using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01690 : CardSupply
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
            CreateFastActivation(Logic, Condition, PlayActionType.Activate);
        }

        /*******************************************************************/
        private bool Condition(Investigator investigator)
        {
            if (!IsInPlay) return false;
            if (investigator != ControlOwner) return false;
            if (Charge.IsEmpty) return false;
            return true;
        }

        private async Task Logic(Investigator investigator)
        {
            InteractableGameAction interactableGameAction = _gameActionsProvider.Create<InteractableGameAction>()
                .SetWith(canBackToThisInteractable: true, mustShowInCenter: true, "Choose deck");
            interactableGameAction.CreateEffect(_chaptersProvider.CurrentScene.CardDangerToDraw, new Stat(0, false), SelectDangerDeck, PlayActionType.Choose, investigator);

            foreach (Investigator inv in _investigatorsProvider.AllInvestigatorsInPlay)
            {
                interactableGameAction.CreateEffect(inv.InvestigatorCard, new Stat(0, false), SelectDeck, PlayActionType.Choose, investigator);

                async Task SelectDeck()
                {
                    IEnumerable<Card> cards = inv.DeckZone.Cards.TakeLast(3);
                    await _gameActionsProvider.Create<ShowCardsGameAction>().SetWith(cards).Start();
                    await SortCards(cards, inv);
                }
            }

            await _gameActionsProvider.Create<DecrementStatGameAction>().SetWith(Charge.Amount, 1).Start();
            await interactableGameAction.Start();

            /*******************************************************************/
            async Task SelectDangerDeck()
            {
                IEnumerable<Card> cards = _chaptersProvider.CurrentScene.DangerDeckZone.Cards.TakeLast(3);
                await _gameActionsProvider.Create<ShowCardsGameAction>().SetWith(cards).Start();
                await SortCards(cards, null);
                await TakeHorror(cards);
            }
        }

        private async Task SortCards(IEnumerable<Card> cards, Owner owner)
        {
            Zone zoneToReturn = owner is Investigator investigator ? investigator.DeckZone : _chaptersProvider.CurrentScene.DangerDeckZone;
            Card cardAffected = owner is Investigator investigator2 ? investigator2.InvestigatorCard : null;

            InteractableGameAction interactableGameAction = _gameActionsProvider.Create<InteractableGameAction>()
                .SetWith(canBackToThisInteractable: true, mustShowInCenter: true, "Choose card");
            foreach (Card card in cards)
            {
                interactableGameAction.CreateEffect(card, new Stat(0, false), SelectCard, PlayActionType.Choose, ControlOwner, cardAffected: cardAffected);

                async Task SelectCard()
                {
                    await _gameActionsProvider.Create<MoveCardsGameAction>()
                        .SetWith(card, zoneToReturn, isFaceDown: true).Start();
                    IEnumerable<Card> newCards = cards.Except(new[] { card });
                    if (newCards.Any()) await SortCards(newCards, owner);
                }
            }

            await interactableGameAction.Start();
        }

        private async Task TakeHorror(IEnumerable<Card> cards)
        {
            if (!cards.Any(card => card.HasThisTag(Tag.Terror) || card.HasThisTag(Tag.Omen))) return;
            await _gameActionsProvider.Create(new HarmToInvestigatorGameAction(ControlOwner, this, amountFear: 1));
        }
    }
}
