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
        public GameCondition<GameAction> PlayFromHandCondition { get; protected set; }

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            ResourceCost = CreateStat(Info.Cost ?? 0);
            PlayFromHandTurnsCost = CreateStat(1);
            PlayFromHandCondition = new GameCondition<GameAction>(CanPlayFromHandWith);
        }

        /*******************************************************************/
        int ICommitable.GetChallengeValue(ChallengeType challengeType)
        {
            int amount = Info.Wild ?? 0;
            return challengeType switch
            {
                ChallengeType.Strength => amount + Info.Strength ?? 0,
                ChallengeType.Agility => amount + Info.Agility ?? 0,
                ChallengeType.Intelligence => amount + Info.Intelligence ?? 0,
                ChallengeType.Power => amount + Info.Power ?? 0,
                _ => amount
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

        async Task IPlayableFromHand.PlayFromHand()
        {
            await _gameActionsProvider.Create(new MoveCardsGameAction(this, _chaptersProvider.CurrentScene.LimboZone));
            await ExecuteConditionEffect();
            await _gameActionsProvider.Create(new DiscardGameAction(this));
        }

        protected abstract Task ExecuteConditionEffect();
    }
}
