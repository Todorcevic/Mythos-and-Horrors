using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public abstract class CardAdversity : Card, IObligate
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProvider;

        public abstract Zone ZoneToMove { get; }

        /*******************************************************************/
        public async Task Obligation()
        {
            await ObligationLogic();
            if (IsVictory)
                await _gameActionsProvider.Create(new MoveCardsGameAction(this, _chaptersProvider.CurrentScene.VictoryZone));
            else await _gameActionsProvider.Create(new DiscardGameAction(this));
        }

        protected abstract Task ObligationLogic();
    }
}
