using Zenject;

namespace GameRules
{
    public class InitializeGameUseCase
    {
        [Inject] private readonly ZoneRepository _zoneRepository;
        [Inject] private readonly CardRepository _cardRepository;
        [Inject] private readonly ICardFactory _cardFactory;

        public void Execute()
        {
            _zoneRepository.CreateZones();
            _cardRepository.CreateCards();
            _cardFactory.BuildCards();
        }
    }
}
