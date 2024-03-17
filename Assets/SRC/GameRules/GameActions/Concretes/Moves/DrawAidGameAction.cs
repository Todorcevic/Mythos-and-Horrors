using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class DrawAidGameAction : GameAction
    {
        private UpdateStatesGameAction _updateStatesGameAction;
        private MoveCardsGameAction _moveCardsGameAction;
        [Inject] private readonly GameActionsProvider _gameActionRepository;

        public Investigator Investigator { get; }

        /*******************************************************************/
        public DrawAidGameAction(Investigator investigator)
        {
            Investigator = investigator;
            CanBeExecuted = Investigator.CardAidToDraw != null;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            _updateStatesGameAction = await _gameActionRepository.Create(new UpdateStatesGameAction(Investigator.CardAidToDraw.FaceDown, false));
            _moveCardsGameAction = await _gameActionRepository.Create(new MoveCardsGameAction(Investigator.CardAidToDraw, Investigator.HandZone));
        }

        protected override async Task UndoThisLogic()
        {
            await _updateStatesGameAction.Undo();
            await _moveCardsGameAction.Undo();
        }
    }
}
