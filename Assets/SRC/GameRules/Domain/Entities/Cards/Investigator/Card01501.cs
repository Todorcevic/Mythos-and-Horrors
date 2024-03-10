using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01501 : CardInvestigator
    {
        [Inject] private readonly GameActionProvider _gameActionRepository;
        [Inject] private readonly CardsProvider _cardsProvider;
        [Inject] private readonly TextsProvider _textsProvider;
        [Inject] private readonly BuffsProvider _buffsProvider;

        public Buff AddResourceCost { get; private set; }

        /*******************************************************************/
        [Inject]
        public void Init()
        {
            AddResourceCost = _buffsProvider.Create(this, _textsProvider.GameText.CARD01501_BUFF, CardsToBuff, AddReductionCostBuff, RemoveReductionCostBuff);
        }

        /*******************************************************************/

        private List<Card> CardsToBuff()
        {
            if (CurrentZone != Owner.InvestigatorZone) return new List<Card>();
            return Owner.HandZone.Cards;

        }

        private async Task AddReductionCostBuff(Card card)
        {
            if (card is CardCondition cardTalent)
                await _gameActionRepository.Create(new DecrementStatGameAction(cardTalent.ResourceCost, 1));

            if (card is CardSupply cardSupply)
                await _gameActionRepository.Create(new DecrementStatGameAction(cardSupply.ResourceCost, 1));
        }

        private async Task RemoveReductionCostBuff(Card card)
        {
            if (card is CardCondition cardTalent)
                await _gameActionRepository.Create(new IncrementStatGameAction(cardTalent.ResourceCost, 1));

            if (card is CardSupply cardSupply)
                await _gameActionRepository.Create(new IncrementStatGameAction(cardSupply.ResourceCost, 1));
        }


        //void IBuffable.ActivateBuff()
        //{
        //    if (CurrentZone != Owner.InvestigatorZone) return;
        //    List<Card> cardsAffected = Owner.HandZone.Cards.Concat(Owner.DiscardZone.Cards).Where(card => !card.HasThisBuff(this)
        //    && ((card is CardCondition cardTalent && cardTalent.ResourceCost.Value > 0)
        //    || (card is CardSupply cardSupply && cardSupply.ResourceCost.Value > 0))).ToList();

        //    cardsAffected.ForEach(async card => await card.AddBuff(this));
        //}

        //void IBuffable.DeactivateBuff()
        //{
        //    List<Card> cardsAffected = _cardsProvider.GetCardsBuffedWith(this);

        //    if (CurrentZone != Owner.InvestigatorZone)
        //    {
        //        cardsAffected.ForEach(async card => await card.RemoveBuff(this));
        //    }
        //    else
        //    {
        //        cardsAffected.FindAll(card => card.CurrentZone != Owner.HandZone && card.CurrentZone != Owner.DiscardZone)
        //            .ForEach(async card => await card.RemoveBuff(this));
        //    }
        //}

        //async Task IBuffable.BuffAffectTo(Card cardAffected)
        //{
        //    if (cardAffected is CardCondition cardTalent)
        //        await _gameActionRepository.Create(new DecrementStatGameAction(cardTalent.ResourceCost, 1));
        //    else if (cardAffected is CardSupply cardSupply)
        //        await _gameActionRepository.Create(new DecrementStatGameAction(cardSupply.ResourceCost, 1));
        //}

        //async Task IBuffable.BuffDeaffectTo(Card cardAffected)
        //{
        //    if (cardAffected is CardCondition cardTalent)
        //        await _gameActionRepository.Create(new IncrementStatGameAction(cardTalent.ResourceCost, 1));
        //    else if (cardAffected is CardSupply cardSupply)
        //        await _gameActionRepository.Create(new IncrementStatGameAction(cardSupply.ResourceCost, 1));
        //}
    }
}
