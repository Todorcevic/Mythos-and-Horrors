using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class DrawAidGameAction : GameAction
    {
        [Inject] private readonly GameActionsProvider _gameActionRepository;

        public Investigator Investigator { get; }
        public Card CardDrawed { get; private set; }

        /*******************************************************************/
        public DrawAidGameAction(Investigator investigator)
        {
            Investigator = investigator;
            CanBeExecuted = Investigator.CardAidToDraw != null;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            CardDrawed = Investigator.CardAidToDraw;
            await _gameActionRepository.Create(new UpdateStatesGameAction(CardDrawed.FaceDown, false));
            await _gameActionRepository.Create(new MoveCardsGameAction(CardDrawed, Investigator.HandZone));
        }
    }
}
