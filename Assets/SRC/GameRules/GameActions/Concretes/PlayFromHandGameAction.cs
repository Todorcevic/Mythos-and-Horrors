using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class PlayFromHandGameAction : GameAction
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public IPlayableFromHand PlayableCard { get; init; }
        public Investigator Investigator { get; init; }

        /*******************************************************************/
        public PlayFromHandGameAction(IPlayableFromHand playableCard, Investigator investigator)
        {
            PlayableCard = playableCard;
            Investigator = investigator;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionsProvider.Create(new DecrementStatGameAction(Investigator.CurrentTurns, PlayableCard.TurnsCost.Value));
            await _gameActionsProvider.Create(new PayResourceGameAction(Investigator, PlayableCard.ResourceCost.Value));
            await PlayableCard.PlayFromHand();
        }
    }
}
