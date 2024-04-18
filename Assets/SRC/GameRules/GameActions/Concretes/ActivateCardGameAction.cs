using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class ActivateCardGameAction : GameAction
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public IActivable ActivableCard { get; init; }
        public Investigator Investigator { get; init; }

        /*******************************************************************/
        public ActivateCardGameAction(IActivable playableCard, Investigator investigator)
        {
            ActivableCard = playableCard;
            Investigator = investigator;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionsProvider.Create(new DecrementStatGameAction(Investigator.CurrentTurns, ActivableCard.ActivateTurnsCost.Value));
            await ActivableCard.Activate();
        }
    }
}
