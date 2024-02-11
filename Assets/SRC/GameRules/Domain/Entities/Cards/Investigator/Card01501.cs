using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class Card01501 : CardInvestigator, IEndReactionable
    {
        [Inject] private readonly GameActionFactory _gameActionRepository;
        [Inject] private readonly CardsProvider _cardsProvider;

        /*******************************************************************/

        public async Task WhenFinish(GameAction gameAction)
        {
            if (gameAction is MoveCardsGameAction)
            {
                ActivateBuffExample();
                DeactivateBuffExample();
            }

            await Task.CompletedTask;
        }

        /*******************************************************************/
        public void ActivateBuffExample()
        {
            if (CurrentZone != Owner.InvestigatorZone) return;
            List<Card> cardsAffected = Owner.HandZone.Cards
                .FindAll(card => !card.HasThisBuff(ActivateBuff) && ((card is CardCondition cardTalent && cardTalent.Cost.Value > 0) || (card is CardSupply cardSupply && cardSupply.Cost.Value > 0)));

            cardsAffected.ForEach(async card => await card.AddBuff(new Buff(this, "Reduce cost by 1", ActivateBuff, DeactivateBuff)));
        }

        public void DeactivateBuffExample()
        {
            List<Card> cardsAffected = _cardsProvider.GetCardsWithThisBuff(ActivateBuff);

            if (CurrentZone != Owner.InvestigatorZone)
            {
                cardsAffected.ForEach(async card => await card.RemoveBuff(ActivateBuff));
            }
            else
            {
                cardsAffected.FindAll(card => card.CurrentZone != Owner.HandZone)
                    .ForEach(async card => await card.RemoveBuff(ActivateBuff));
            }
        }

        private async Task ActivateBuff(Card cardAffected)
        {
            if (cardAffected is CardCondition cardTalent)
                await _gameActionRepository.Create(new DecrementStatGameAction(cardTalent.Cost, 1));
            else if (cardAffected is CardSupply cardSupply)
                await _gameActionRepository.Create(new DecrementStatGameAction(cardSupply.Cost, 1));
        }

        private async Task DeactivateBuff(Card cardAffected)
        {
            if (cardAffected is CardCondition cardTalent)
                await _gameActionRepository.Create(new IncrementStatGameAction(cardTalent.Cost, 1));
            else if (cardAffected is CardSupply cardSupply)
                await _gameActionRepository.Create(new IncrementStatGameAction(cardSupply.Cost, 1));
        }
    }
}
