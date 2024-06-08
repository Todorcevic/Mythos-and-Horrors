using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public abstract class CardCondition : Card, ICommitable, IPlayableFromHand
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProvider;

        public Stat ResourceCost { get; private set; }
        public Stat PlayFromHandTurnsCost { get; protected set; }
        public GameCondition<GameAction> PlayFromHandCondition { get; private set; }
        public GameCommand<PlayFromHandGameAction> PlayFromHandCommand { get; private set; }

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            ResourceCost = CreateStat(Info.Cost ?? 0);
            PlayFromHandTurnsCost = CreateStat(1);
            PlayFromHandCondition = new GameCondition<GameAction>(CanPlayFromHandWith);
            PlayFromHandCommand = new GameCommand<PlayFromHandGameAction>(PlayFromHand);
        }

        /*******************************************************************/
        int ICommitable.GetChallengeValue(ChallengeType challengeType)
        {
            int wildAmount = Info.Wild ?? 0;
            return challengeType switch
            {
                ChallengeType.Strength => wildAmount + Info.Strength ?? 0,
                ChallengeType.Agility => wildAmount + Info.Agility ?? 0,
                ChallengeType.Intelligence => wildAmount + Info.Intelligence ?? 0,
                ChallengeType.Power => wildAmount + Info.Power ?? 0,
                _ => wildAmount
            };
        }

        /*******************************************************************/
        protected virtual bool CanPlayFromHandWith(GameAction gameAction)
        {
            if (gameAction is not OneInvestigatorTurnGameAction oneInvestigatorTurnGameAction) return false;
            if (CurrentZone.ZoneType != ZoneType.Hand) return false;
            if (ControlOwner != oneInvestigatorTurnGameAction.ActiveInvestigator) return false;
            if (ResourceCost.Value > ControlOwner.Resources.Value) return false;
            if (PlayFromHandTurnsCost.Value > ControlOwner.CurrentTurns.Value) return false;
            return true;
        }

        private async Task PlayFromHand(PlayFromHandGameAction playFromHandGameAction)
        {
            await _gameActionsProvider.Create(new MoveCardsGameAction(this, _chaptersProvider.CurrentScene.LimboZone));
            await ExecuteConditionEffect(playFromHandGameAction.Investigator);
            await _gameActionsProvider.Create(new DiscardGameAction(this));
        }

        protected abstract Task ExecuteConditionEffect(Investigator investigator);
    }
}
