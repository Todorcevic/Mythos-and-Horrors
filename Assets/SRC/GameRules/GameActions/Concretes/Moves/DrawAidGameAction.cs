using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class DrawAidGameAction : GameAction
    {
        [Inject] private readonly GameActionsProvider _gameActionRepository;

        public Investigator Investigator { get; }
        public override bool CanBeExecuted => Investigator.CardAidToDraw != null;
        public override bool CanUndo => false;

        /*******************************************************************/
        public DrawAidGameAction(Investigator investigator)
        {
            Investigator = investigator;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionRepository.Create(new MoveCardsGameAction(Investigator.CardAidToDraw, Investigator.HandZone));
        }
    }
}
