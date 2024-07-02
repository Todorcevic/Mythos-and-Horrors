using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class DiscardCommitsCards : GameAction
    {
        [Inject] private readonly ChaptersProvider _chaptersProvider;

        public override bool CanBeExecuted => AllCommitableCards().Count() > 0;

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionsProvider.Create<SafeForeach<CommitableCard>>().SetWith(AllCommitableCards, Discard).Start();
        }

        /*******************************************************************/
        private IEnumerable<CommitableCard> AllCommitableCards() => _chaptersProvider.CurrentScene.LimboZone.Cards
            .OfType<CommitableCard>().Where(comiitable => comiitable.Commited.IsActive);

        private async Task Discard(CommitableCard card)
        {
            await _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(card.Commited, false).Start();
            await _gameActionsProvider.Create<DiscardGameAction>().SetWith(card).Start();
        }
    }
}
