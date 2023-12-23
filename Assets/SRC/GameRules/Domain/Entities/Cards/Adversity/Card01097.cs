using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class Card01097 : CardAdversity, IEndReactionable, IWeakness
    {
        [Inject] private readonly GameActionFactory _gameActionRepository;

        /*******************************************************************/
        public async Task WhenFinish(GameAction gameAction)
        {
            if (CanActivate(gameAction))
            {
                await _gameActionRepository.Create<MoveCardGameAction>().Run(this, null); //TODO Remove Resource
            }
        }

        private bool CanActivate(GameAction gameAction) //TODO do correctly
        {
            if (gameAction is not DrawGameAction drawGameAction) return false;
            if (drawGameAction.CardDrawed != this) return false;
            return true;
        }
    }
}
