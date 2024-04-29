using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{

    public class ActivateCardGameAction : GameAction //Esta gameAction es sobretodo para la Animacion
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public Activation ActivationCard { get; }
        public Investigator Investigator { get; }

        /*******************************************************************/
        public ActivateCardGameAction(Activation playableCard, Investigator investigator)
        {
            ActivationCard = playableCard;
            Investigator = investigator;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionsProvider.Create(new DecrementStatGameAction(Investigator.CurrentTurns, ActivationCard.ActivateTurnsCost.Value));
            await ActivationCard.Logic.Invoke(Investigator);
        }
    }
}
