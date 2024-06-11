using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    internal class CommitGameAction : GameAction
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly ChaptersProvider _chapterProvider;

        public ICommitable CommitableCard { get; init; }

        /*******************************************************************/
        public CommitGameAction(ICommitable card)
        {
            CommitableCard = card;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionsProvider.Create(new UpdateStatesGameAction(CommitableCard.Commited, true));
            await _gameActionsProvider.Create(new MoveCardsGameAction((Card)CommitableCard, _chapterProvider.CurrentScene.LimboZone));
        }
    }
}