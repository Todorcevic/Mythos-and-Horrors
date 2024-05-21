using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public abstract class CardAdversityLimbo : CardAdversity
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProvider;


        /*******************************************************************/
        public override async Task PlayAdversityFor(Investigator investigator)
        {
            await _gameActionsProvider.Create(new MoveCardsGameAction(this, _chaptersProvider.CurrentScene.LimboZone));
            await ObligationLogic(investigator);
            await _gameActionsProvider.Create(new DefeatCardGameAction(this, investigator.InvestigatorCard));
        }

        /*******************************************************************/
        protected abstract Task ObligationLogic(Investigator investigator);
    }
}
