using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class PlayFromHandGameAction : GameAction
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public IPlayableFromHand PlayableFromHandCard { get; init; }
        public Investigator Investigator { get; init; }

        /*******************************************************************/
        public PlayFromHandGameAction(IPlayableFromHand playableCard, Investigator investigator)
        {
            PlayableFromHandCard = playableCard;
            Investigator = investigator;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionsProvider.Create(new DecrementStatGameAction(Investigator.CurrentTurns, PlayableFromHandCard.PlayFromHandTurnsCost.Value));
            await _gameActionsProvider.Create(new PayResourceGameAction(Investigator, PlayableFromHandCard.ResourceCost.Value));
            await PlayableFromHandCard.PlayFromHand();
        }
    }
}
