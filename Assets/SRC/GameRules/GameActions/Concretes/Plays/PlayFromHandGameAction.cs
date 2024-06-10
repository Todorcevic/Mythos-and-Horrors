using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class PlayFromHandGameAction : GameAction
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public IPlayableFromHand PlayableFromHandCard { get; init; }
        public Investigator Investigator { get; init; }
        public Card Card => (Card)PlayableFromHandCard;

        /*******************************************************************/
        public PlayFromHandGameAction(IPlayableFromHand playableCard, Investigator investigator)
        {
            PlayableFromHandCard = playableCard;
            Investigator = investigator;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionsProvider.Create(new PayResourceGameAction(Investigator, PlayableFromHandCard.ResourceCost.Value));

            if (PlayableFromHandCard.IsJustPlayFromHand)
            {
                await _gameActionsProvider.Create(new DecrementStatGameAction(Investigator.CurrentTurns, PlayableFromHandCard.PlayFromHandTurnsCost.Value));
            }

            if (PlayableFromHandCard.WithOppotunityAttack)
            {
                await _gameActionsProvider.Create(new OpportunityAttackGameAction(Investigator));
            }
            await PlayableFromHandCard.PlayFromHandCommand.RunWith(this);
        }
    }
}
