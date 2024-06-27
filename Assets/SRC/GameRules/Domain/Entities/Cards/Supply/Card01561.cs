using System.Collections.Generic;
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

        public Stat AmountCharges { get; private set; }

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            AmountCharges = CreateStat(3);
            CreateActivation(1, Logic, Condition, PlayActionType.Activate);
        }

        /*******************************************************************/
        private bool Condition(Investigator investigator)
        {
            if (!IsInPlay) return false;
            if (investigator != ControlOwner) return false;
            if (AmountCharges.Value < 1) return false;
            return true;
        }

        private async Task Logic(Investigator investigator)
        {
            InteractableGameAction interactableGameAction = new(canBackToThisInteractable: false, mustShowInCenter: true, "Choose deck");
            interactableGameAction.CreateCancelMainButton();
            interactableGameAction.CreateEffect(_chaptersProvider.CurrentScene.CardDangerToDraw, new Stat(0, false), SelectDangerDeck, PlayActionType.Choose, investigator);

            foreach (Investigator inv in _investigatorsProvider.AllInvestigatorsInPlay)
            {
                interactableGameAction.CreateEffect(inv.InvestigatorCard, new Stat(0, false), SelectDeck, PlayActionType.Choose, investigator);

                async Task SelectDeck()
                {
                    IEnumerable<Card> cards = inv.DeckZone.Cards.TakeLast(3);
                    await _gameActionsProvider.Create(new MoveCardsGameAction(cards, _chaptersProvider.CurrentScene.LimboZone));
                    await SortCards(inv);
                }
            }

            await _gameActionsProvider.Create(new DecrementStatGameAction(AmountCharges, 1));
            await _gameActionsProvider.Create(interactableGameAction);

            /*******************************************************************/
            async Task SelectDangerDeck()
            {
                IEnumerable<Card> cards = _chaptersProvider.CurrentScene.DangerDeckZone.Cards.TakeLast(3);
                await _gameActionsProvider.Create(new MoveCardsGameAction(cards, _chaptersProvider.CurrentScene.LimboZone));
                await SortCards(null);
            }
        }

        private async Task SortCards(Owner owner)
        {
            Zone zoneToReturn = owner is Investigator investigator ? investigator.DeckZone : _chaptersProvider.CurrentScene.DangerDeckZone;
            Card cardAffected = owner is Investigator investigator2 ? investigator2.InvestigatorCard : null;

            InteractableGameAction interactableGameAction = new(canBackToThisInteractable: false, mustShowInCenter: true, "Choose card");
            interactableGameAction.CreateCancelMainButton();
            foreach (Card card in _chaptersProvider.CurrentScene.LimboZone.Cards)
            {
                interactableGameAction.CreateEffect(card, new Stat(0, false), SelectCard, PlayActionType.Choose, ControlOwner, cardAffected: cardAffected);

                async Task SelectCard()
                {
                    await _gameActionsProvider.Create(new MoveCardsGameAction(card, zoneToReturn, isFaceDown: true));
                    if (_chaptersProvider.CurrentScene.LimboZone.Cards.Any()) await SortCards(owner);
                }
            }

            await _gameActionsProvider.Create(interactableGameAction);
        }
    }
}
