using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public abstract class CardCondition : CommitableCard
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProvider;

        public Stat ResourceCost { get; private set; }
        public virtual PlayActionType PlayFromHandActionType => PlayActionType.PlayFromHand;
        public GameCommand<GameAction> PlayFromHandCommand { get; private set; }

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            ResourceCost = CreateStat(Info.Cost ?? 0);
            PlayFromHandCommand = new GameCommand<GameAction>(PlayFromHand);
        }

        /*******************************************************************/
        protected abstract Task ExecuteConditionEffect(GameAction gameAction, Investigator investigator);

        /*******************************************************************/
        private async Task PlayFromHand(GameAction gameAction)
        {
            Investigator currentInvestigator = ControlOwner; // Bcz when card go to Limbo ControlOwner==null
            await _gameActionsProvider.Create<MoveCardsGameAction>()
                .SetWith(this, _chaptersProvider.CurrentScene.LimboZone).Start();
            await ExecuteConditionEffect(gameAction, currentInvestigator);
            await _gameActionsProvider.Create<DiscardGameAction>().SetWith(this).Start();
        }
    }
}
