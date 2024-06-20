using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class DiscardCommitsCards : GameAction
    {
        [Inject] private readonly ChaptersProvider _chaptersProvider;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public override bool CanBeExecuted => AllCommitableCards().Count() > 0;

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionsProvider.Create(new SafeForeach<CommitableCard>(AllCommitableCards, Discard));
        }

        /*******************************************************************/
        private IEnumerable<CommitableCard> AllCommitableCards() => _chaptersProvider.CurrentScene.LimboZone.Cards
            .OfType<CommitableCard>().Where(comiitable => comiitable.Commited.IsActive);

        private async Task Discard(CommitableCard card)
        {
            await _gameActionsProvider.Create(new UpdateStatesGameAction(card.Commited, false));
            await _gameActionsProvider.Create(new DiscardGameAction(card));
        }
    }
}
