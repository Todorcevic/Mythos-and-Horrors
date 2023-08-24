using Zenject;

namespace GameRules
{
    public class InitializeGameUseCase
    {
        [Inject] private readonly ZoneFactory _zoneFactory;
        [Inject] private readonly CardFactory _cardFactory2;
        [Inject] private readonly ICardGenerator _cardFactory;

        public void Execute()
        {
            _zoneFactory.CreateZones();
            _cardFactory2.CreateCards();
            _cardFactory.BuildCards();
        }
    }
}
