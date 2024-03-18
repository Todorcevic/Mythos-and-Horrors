using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class DrawAidGameAction : GameAction
    {
        private Card _realCardDrawed;

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
            _realCardDrawed = Investigator.CardAidToDraw;
            await _gameActionRepository.Create(new UpdateStatesGameAction(Investigator.CardAidToDraw.FaceDown, false));
            await _gameActionRepository.Create(new MoveCardsGameAction(Investigator.CardAidToDraw, Investigator.HandZone));
        }

        public override async Task Undo()
        {
            _realCardDrawed.FaceDown.UpdateValueTo(true);
            await Task.CompletedTask;
        }
    }
}
