using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class DrawAidGameAction : GameAction
    {
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
            await _gameActionRepository.Create(new UpdateStatesGameAction(Investigator.CardAidToDraw.FaceDown, false));
            await _gameActionRepository.Create(new MoveCardsGameAction(Investigator.CardAidToDraw, Investigator.HandZone));
        }
    }
}
