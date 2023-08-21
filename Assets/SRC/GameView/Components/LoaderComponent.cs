using GameRules;
using UnityEngine;
using Zenject;

namespace GameView
{
    public class LoaderComponent : MonoBehaviour
    {
        [Inject] private readonly InitializeGameUseCase _initializeGameUseCase;
        [Inject] private readonly MoveCardAction _moveCardAction;
        [Inject] private readonly CardRepository _cardRepository;
        [Inject] private readonly ZoneRepository _zoneRepository;

        /*******************************************************************/
        private void Start()
        {
            _initializeGameUseCase.Execute();

            _moveCardAction.Set(_cardRepository.GetCard("1"), _zoneRepository.GetZone(ZoneType.AssetsDeck));
            _moveCardAction.Execute();
        }
    }
}
