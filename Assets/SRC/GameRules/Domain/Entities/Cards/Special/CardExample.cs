//using System.Collections.Generic;
//using System.Threading.Tasks;
//using Zenject;

//namespace MythosAndHorrors.GameRules
//{
//    public class CardExample : CardInvestigator, IBuffable
//    {
//        [Inject] private readonly GameActionProvider _gameActionRepository;
//        [Inject] private readonly CardsProvider _cardsProvider;

//        string IViewEffect.CardCode => Info.Code;

//        string IViewEffect.CardCodeSecundary => Owner.Code;

//        string IViewEffect.Description => "Reduce cost by 1";

//        /*******************************************************************/
//        void IBuffable.ActivateBuff()
//        {
//            if (CurrentZone != Owner.InvestigatorZone) return;
//            List<Card> cardsAffected = Owner.HandZone.Cards
//                .FindAll(card => !card.HasThisBuff(this) && ((card is CardCondition cardTalent && cardTalent.ResourceCost.Value > 0) || (card is CardSupply cardSupply && cardSupply.ResourceCost.Value > 0)));

//            cardsAffected.ForEach(async card => await card.AddBuff(this));
//        }

//        void IBuffable.DeactivateBuff()
//        {
//            List<Card> cardsAffected = _cardsProvider.GetCardsBuffedWith(this);

//            if (CurrentZone != Owner.InvestigatorZone)
//            {
//                cardsAffected.ForEach(async card => await card.RemoveBuff(this));
//            }
//            else
//            {
//                cardsAffected.FindAll(card => card.CurrentZone != Owner.HandZone)
//                    .ForEach(async card => await card.RemoveBuff(this));
//            }
//        }

//        async Task IBuffable.BuffAffectTo(Card cardAffected)
//        {
//            if (cardAffected is CardCondition cardTalent)
//                await _gameActionRepository.Create(new DecrementStatGameAction(cardTalent.ResourceCost, 1));
//            else if (cardAffected is CardSupply cardSupply)
//                await _gameActionRepository.Create(new DecrementStatGameAction(cardSupply.ResourceCost, 1));
//        }

//        async Task IBuffable.BuffDeaffectTo(Card cardAffected)
//        {
//            if (cardAffected is CardCondition cardTalent)
//                await _gameActionRepository.Create(new IncrementStatGameAction(cardTalent.ResourceCost, 1));
//            else if (cardAffected is CardSupply cardSupply)
//                await _gameActionRepository.Create(new IncrementStatGameAction(cardSupply.ResourceCost, 1));
//        }
//    }
//}
