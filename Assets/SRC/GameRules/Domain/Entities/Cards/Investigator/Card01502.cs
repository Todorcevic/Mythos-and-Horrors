using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01502 : CardInvestigator, IStartReactionable, IEndReactionable
    {
        [Inject] private readonly GameActionProvider _gameActionRepository;
        [Inject] private readonly CardsProvider _cardRepository;

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
