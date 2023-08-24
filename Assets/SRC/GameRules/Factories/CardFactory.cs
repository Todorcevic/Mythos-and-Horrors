using Zenject;

namespace GameRules
{
    public class CardFactory
    {
        [Inject] private readonly DiContainer _container;
        [Inject] private readonly CardRepository _cardRepository;

        /*******************************************************************/
        public void CreateCards()
        {
            for (int i = 0; i < 10; i++)
            {
                Card newCard = new() { Id = i.ToString(), Name = i.ToString(), Type = CardType.Asset };
                _container.Inject(newCard);
                _cardRepository.AddCard(newCard);
            }

            Card00001 otherCard = new() { Id = "00001", Name = "00001", Type = CardType.Encounter };
            _container.Inject(otherCard);
            _container.Bind<IEndReactionable>().FromInstance(otherCard).NonLazy();
            _cardRepository.AddCard(otherCard);
        }
    }
}
