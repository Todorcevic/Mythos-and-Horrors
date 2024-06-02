using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class PlayActivateCardGameAction : GameAction
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public Activation ActivationCard { get; }
        public Investigator Investigator { get; }

        /*******************************************************************/
        public PlayActivateCardGameAction(Activation playableCard, Investigator investigator)
        {
            ActivationCard = playableCard;
            Investigator = investigator;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionsProvider.Create(new DecrementStatGameAction(Investigator.CurrentTurns, ActivationCard.ActivateTurnsCost.Value));
            if (ActivationCard.WithOportunityAttack) await _gameActionsProvider.Create(new OpportunityAttackGameAction(Investigator));
            await ActivationCard.PlayFor(Investigator);
        }
    }
}
