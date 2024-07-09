using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class RestoreDangerDeckGameAction : GameAction
    {
        [Inject] private readonly ChaptersProvider _chaptersProvider;

        public Investigator Investigator { get; private set; }

        /*******************************************************************/
        public RestoreDangerDeckGameAction SetWith(Investigator investigator)
        {
            Investigator = investigator;
            return this;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionsProvider.Create<MoveCardsGameAction>()
                     .SetWith(_chaptersProvider.CurrentScene.DangerDiscardZone.Cards, _chaptersProvider.CurrentScene.DangerDeckZone, isFaceDown: true).Execute();
            await _gameActionsProvider.Create<ShuffleGameAction>().SetWith(_chaptersProvider.CurrentScene.DangerDeckZone).Execute();
        }
    }
}
