using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public abstract class CardAdversityLimbo : CardAdversity
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProvider;

        public Reaction<DrawGameAction> Play => _reactionablesProvider.FindReactionByLogic<DrawGameAction>(PlayLogic);
        public override Zone ZoneToMove => _chaptersProvider.CurrentScene.LimboZone;

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            CreateReaction<DrawGameAction>(PlayCondition, PlayLogic, false);
        }

        /*******************************************************************/
        private async Task PlayLogic(DrawGameAction drawGameAction)
        {
            await ObligationLogic();
            await _gameActionsProvider.Create(new DefeatCardGameAction(this, drawGameAction.Investigator.InvestigatorCard));
        }

        private bool PlayCondition(DrawGameAction drawGameAction)
        {
            if (drawGameAction.CardDrawed != this) return false;
            return true;
        }

        /*******************************************************************/
        protected abstract Task ObligationLogic();
    }
}
