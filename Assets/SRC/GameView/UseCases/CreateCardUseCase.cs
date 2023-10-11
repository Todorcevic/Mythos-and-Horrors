using MythsAndHorrors.GameRules;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class CreateCardUseCase
    {
        [Inject] private readonly CardProvider _cardProvider;
        [Inject] private readonly CardFactory _cardFactory;

        /*******************************************************************/
        public Card CreateCard(string cardCode)
        {
            Card newCard = _cardFactory.CreateCard(cardCode);
            _cardProvider.AddCard(newCard);
            return newCard;
        }
    }
}
