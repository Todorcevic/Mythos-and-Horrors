using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class DiscardGameAction : GameAction
    {
        private Card _card;
        [Inject] private readonly AdventurersProvider _adventurersProvider;
        [Inject] private readonly ZonesProvider _zonesProvider;
        [Inject] private readonly ChaptersProvider _chaptersProvider;
        [Inject] private readonly GameActionFactory _gameActionRepository;

        /*******************************************************************/
        public async Task Run(Card card)
        {
            _card = card;
            await Start();
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionRepository.Create<MoveCardGameAction>().Run(_card, GetDiscardZone());
        }

        private Zone GetDiscardZone()
        {
            if (_chaptersProvider.CurrentScene.Info.DangerCards.Contains(_card))
                return _zonesProvider.DangerDiscardZone;

            return _adventurersProvider.GetAdventurerWithThisCard(_card)?.DiscardZone ?? _zonesProvider.OutZone;
        }
    }
}
