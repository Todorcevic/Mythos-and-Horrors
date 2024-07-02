using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public abstract class CardAdversityLimbo : CardAdversity
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProvider;

        public override bool IsInPlay => CurrentZone.ZoneType == ZoneType.Limbo;

        /*******************************************************************/
        public override sealed Zone ZoneToMoveWhenDraw(Investigator investigator) => _chaptersProvider.CurrentScene.LimboZone;

        public override async Task PlayRevelationFor(Investigator investigator)
        {
            if (HasThisTag(Tag.Isolate)) await _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(investigator.Isolated, true).Start();
            await ObligationLogic(investigator);
            if (HasThisTag(Tag.Isolate)) await _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(investigator.Isolated, false).Start();
            await _gameActionsProvider.Create<DiscardGameAction>().SetWith(this).Start();
        }

        /*******************************************************************/
        protected abstract Task ObligationLogic(Investigator investigator);
    }
}
