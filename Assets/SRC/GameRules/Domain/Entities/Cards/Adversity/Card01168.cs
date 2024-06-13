using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01168 : CardAdversity
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly CardsProvider _cardsProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Hazard };

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            CreateReaction<InvestigatePlaceGameAction>(FinishCondition, FinishLogic, isAtStart: false);
            CreateBuff(CardsToBuff, ActivationBuff, DeactivationBuff);
        }

        /*******************************************************************/
        private async Task ActivationBuff(IEnumerable<Card> cards)
        {
            await _gameActionsProvider.Create(new IncrementStatGameAction(cards.Cast<CardPlace>().Unique().Enigma, 2));
        }

        private async Task DeactivationBuff(IEnumerable<Card> cards)
        {
            await _gameActionsProvider.Create(new DecrementStatGameAction(cards.Cast<CardPlace>().Unique().Enigma, 2));

        }

        private IEnumerable<Card> CardsToBuff()
        {
            Card place = _cardsProvider.GetCardWithThisZone(CurrentZone);
            return place != null ? new[] { place } : Enumerable.Empty<Card>();
        }

        /*******************************************************************/
        private async Task FinishLogic(InvestigatePlaceGameAction investigateGameAction)
        {
            await _gameActionsProvider.Create(new DiscardGameAction(this));
        }

        private bool FinishCondition(InvestigatePlaceGameAction investigateGameAction)
        {
            if (CurrentZone != investigateGameAction.CardPlace.OwnZone) return false;
            if (!(bool)investigateGameAction.IsSuccessful) return false;
            return true;
        }

        /*******************************************************************/
        protected override async Task PlayAdversityFor(Investigator investigator)
        {
            Zone destiny = investigator.CurrentPlace.OwnZone;
            if (destiny.Cards.Exists(card => card is Card01168))
                await _gameActionsProvider.Create(new DiscardGameAction(this));
            else await _gameActionsProvider.Create(new MoveCardsGameAction(this, destiny));
        }
    }
}
