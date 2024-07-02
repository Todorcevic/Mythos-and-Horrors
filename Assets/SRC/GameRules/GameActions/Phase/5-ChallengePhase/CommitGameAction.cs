using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    internal class CommitGameAction : GameAction
    {
        [Inject] private readonly ChaptersProvider _chapterProvider;

        public CommitableCard CommitableCard { get; private set; }

        /*******************************************************************/
        public CommitGameAction SetWith(CommitableCard card)
        {
            CommitableCard = card;
            return this;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(CommitableCard.Commited, true).Execute();
            await _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(CommitableCard, _chapterProvider.CurrentScene.LimboZone).Execute();
        }
    }
}