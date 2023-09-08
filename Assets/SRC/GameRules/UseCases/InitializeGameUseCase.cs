using Zenject;

namespace GameRules
{
    public class InitializeGameUseCase
    {
        [Inject] private readonly ZoneFactory _zoneFactory;
        [Inject] private readonly ICardLoader _cardLoader;
        [Inject] private readonly ICardGenerator _cardGenerator;

        public void Execute()
        {
            _zoneFactory.CreateZones();
            _cardLoader.LoadCards();
            _cardGenerator.BuildCards();
        }
    }
}
