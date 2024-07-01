using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    internal class CommitGameAction : GameAction
    {
        [Inject] private readonly ChaptersProvider _chapterProvider;

        public CommitableCard CommitableCard { get; init; }

        /*******************************************************************/
        public CommitGameAction(CommitableCard card)
        {
            CommitableCard = card;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionsProvider.Create(new UpdateStatesGameAction(CommitableCard.Commited, true));
            await _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(CommitableCard, _chapterProvider.CurrentScene.LimboZone).Start();
        }
    }
}