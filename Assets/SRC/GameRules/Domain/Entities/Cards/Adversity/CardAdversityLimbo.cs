using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public abstract class CardAdversityLimbo : CardAdversity
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProvider;

        /*******************************************************************/
        public override sealed Zone ZoneToMoveWhenDraw(Investigator investigator) => _chaptersProvider.CurrentScene.LimboZone;

        public override async Task PlayRevelationFor(Investigator investigator)
        {
            if (HasThisTag(Tag.Isolate)) await _gameActionsProvider.Create(new UpdateStatesGameAction(investigator.Isolated, true));
            await ObligationLogic(investigator);
            if (HasThisTag(Tag.Isolate)) await _gameActionsProvider.Create(new UpdateStatesGameAction(investigator.Isolated, false));
            await _gameActionsProvider.Create(new DiscardGameAction(this));
        }

        /*******************************************************************/
        protected abstract Task ObligationLogic(Investigator investigator);
    }
}
