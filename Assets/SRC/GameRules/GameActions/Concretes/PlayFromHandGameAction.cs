using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class PlayFromHandGameAction : GameAction
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProvider;

        public IPlayableFromHand PlayableCard { get; init; }
        public Investigator Investigator { get; init; }
        public Card CardToPlay => (Card)PlayableCard;

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
            await Condition();
        }

        private async Task Condition()
        {
            if (PlayableCard is CardCondition conditionCard)
            {
                await _gameActionsProvider.Create(new MoveCardsGameAction(CardToPlay, _chaptersProvider.CurrentScene.LimboZone));
                await conditionCard.ConditionEffect.Invoke();
                await _gameActionsProvider.Create(new DiscardGameAction(CardToPlay));
            }

            if (PlayableCard is CardSupply)
                await _gameActionsProvider.Create(new MoveCardsGameAction(CardToPlay, Investigator.AidZone));
        }
    }
}
