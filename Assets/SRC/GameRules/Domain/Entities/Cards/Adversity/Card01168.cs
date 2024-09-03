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
            CreateForceReaction<InvestigatePlaceGameAction>(FinishCondition, FinishLogic, GameActionTime.After);
            CreateBuff(CardsToBuff, ActivationBuff, DeactivationBuff, new Localization("Buff_Card01168"));
        }

        /*******************************************************************/
        public override sealed Zone ZoneToMoveWhenDraw(Investigator investigator) => investigator.CurrentPlace.OwnZone;

        public override async Task PlayRevelationFor(Investigator investigator)
        {
            if (CurrentZone.Cards.Exists(card => card is Card01168 && card != this))
                await _gameActionsProvider.Create<DiscardGameAction>().SetWith(this).Execute();
        }

        /*******************************************************************/
        private async Task ActivationBuff(IEnumerable<Card> cards)
        {
            await _gameActionsProvider.Create<IncrementStatGameAction>().SetWith(cards.Cast<CardPlace>().Unique().Enigma, 2).Execute();
        }

        private async Task DeactivationBuff(IEnumerable<Card> cards)
        {
            await _gameActionsProvider.Create<DecrementStatGameAction>().SetWith(cards.Cast<CardPlace>().Unique().Enigma, 2).Execute();

        }

        private IEnumerable<Card> CardsToBuff()
        {
            Card place = _cardsProvider.GetCardWithThisZone(CurrentZone);
            return place != null ? new[] { place } : Enumerable.Empty<Card>();
        }

        /*******************************************************************/
        private async Task FinishLogic(InvestigatePlaceGameAction investigateGameAction)
        {
            await _gameActionsProvider.Create<DiscardGameAction>().SetWith(this).Execute();
        }

        private bool FinishCondition(InvestigatePlaceGameAction investigateGameAction)
        {
            if (CurrentZone != investigateGameAction.CardPlace.OwnZone) return false;
            if (!investigateGameAction.IsSucceed) return false;
            return true;
        }


    }
}
