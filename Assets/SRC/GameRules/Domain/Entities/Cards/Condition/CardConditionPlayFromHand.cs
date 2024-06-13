using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public abstract class CardConditionPlayFromHand : CardCondition, IPlayableFromHand
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProvider;

        public GameCommand<GameAction> PlayFromHandCommand { get; private set; }

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            PlayFromHandCommand = new GameCommand<GameAction>(PlayFromHand);
        }

        /*******************************************************************/
        protected sealed override bool CanPlayFromHandWith(GameAction gameAction)
        {
            if (gameAction is not OneInvestigatorTurnGameAction oneInvestigatorTurnGameAction) return false;
            if (CurrentZone.ZoneType != ZoneType.Hand) return false;
            if (ControlOwner != oneInvestigatorTurnGameAction.ActiveInvestigator) return false;
            if (ResourceCost.Value > ControlOwner.Resources.Value) return false;
            if (PlayFromHandTurnsCost.Value > ControlOwner.CurrentTurns.Value) return false;
            return CanPlayFromHandSpecific(gameAction);
        }

        protected abstract bool CanPlayFromHandSpecific(GameAction gameAction);

        /*******************************************************************/
        protected override async Task PlayFromHand(GameAction _)
        {
            Investigator currentInvestigator = ControlOwner;
            await _gameActionsProvider.Create(new MoveCardsGameAction(this, _chaptersProvider.CurrentScene.LimboZone));
            await ExecuteConditionEffect(currentInvestigator);
            await _gameActionsProvider.Create(new DiscardGameAction(this));
        }

        protected abstract Task ExecuteConditionEffect(Investigator investigator);
    }
}
