using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class Card01502 : CardAdventurer, IStartReactionable, IEndReactionable
    {
        [Inject] private readonly GameActionFactory _gameActionRepository;
        [Inject] private readonly CardsProvider _cardRepository;
        [Inject] private readonly ZonesProvider _zoneRepository;

        /*******************************************************************/
        public async Task WhenBegin(GameAction gameAction)
        {
            await Task.CompletedTask;
        }

        public async Task WhenFinish(GameAction gameAction)
        {
            await Task.CompletedTask;
        }
    }
}
