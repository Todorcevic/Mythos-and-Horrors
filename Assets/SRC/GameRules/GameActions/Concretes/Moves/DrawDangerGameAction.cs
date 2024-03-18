using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class DrawDangerGameAction : GameAction
    {
        private Card _realCardDrawed;
        [Inject] private readonly GameActionsProvider _gameActionRepository;
        [Inject] private readonly ChaptersProvider _chaptersProvider;

        public Investigator Investigator { get; }

        /*******************************************************************/
        public DrawDangerGameAction(Investigator investigator)
        {
            Investigator = investigator;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            _realCardDrawed = _chaptersProvider.CurrentScene.CardDangerToDraw;
            await _gameActionRepository.Create(new UpdateStatesGameAction(_chaptersProvider.CurrentScene.CardDangerToDraw.FaceDown, false));
            await _gameActionRepository.Create(new MoveCardsGameAction(_chaptersProvider.CurrentScene.CardDangerToDraw, _chaptersProvider.CurrentScene.LimboZone));
            //TODO: Resolve card (Revelation, Creature, etc...)
        }

        public override async Task Undo()
        {
            _realCardDrawed.FaceDown.UpdateValueTo(true);
            await Task.CompletedTask;
        }
    }
}
