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

        IEnumerable<Card> AllCommitableCards => _chaptersProvider.CurrentScene.LimboZone.Cards
                 .OfType<ICommitable>().Cast<Card>();
        public override bool CanBeExecuted => AllCommitableCards.Count() > 0;

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionsProvider.Create(new DiscardGameAction(AllCommitableCards.First()));
            await _gameActionsProvider.Create(new DiscardCommitsCards());
        }
    }
}
