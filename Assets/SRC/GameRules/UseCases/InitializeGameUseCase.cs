using Zenject;

namespace GameRules
{
    public class InitializeGameUseCase
    {
        [Inject] private readonly ZoneFactory _zoneFactory;
        [Inject] private readonly CardFactory _cardFactory;
        [Inject] private readonly ICardGenerator _cardGenerator;

        public void Execute()
        {
            _zoneFactory.CreateZones();
            _cardFactory.CreateCards();
            _cardGenerator.BuildCards();
        }
    }
}
