using System.Diagnostics.CodeAnalysis;
using System.Linq;
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
        public Reaction<GameAction> PlayFromHandReaction { get; protected set; }

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            ResourceCost = CreateStat(Info.Cost ?? 0);
            PlayFromHandTurnsCost = CreateStat(1);
            PlayFromHandReaction = CreateReaction<GameAction>(ConditionToPlayFromHand, PlayFromHand, isAtStart: true);
        }

        /*******************************************************************/
        int ICommitable.GetChallengeValue(ChallengeType challengeType)
        {
            int amount = Info.Wild ?? 0;
            if (challengeType == ChallengeType.Strength) return amount + Info.Strength ?? 0;
            if (challengeType == ChallengeType.Agility) return amount + Info.Agility ?? 0;
            if (challengeType == ChallengeType.Intelligence) return amount + Info.Intelligence ?? 0;
            if (challengeType == ChallengeType.Power) return amount + Info.Power ?? 0;
            return amount;
        }

        /*******************************************************************/
        public virtual async Task PlayFromHand(GameAction gameAction)
        {
            if (gameAction is not OneInvestigatorTurnGameAction oneInvestigatorTurnGameAction) return;
            oneInvestigatorTurnGameAction.Create().SetCard(this)
                .SetInvestigator(oneInvestigatorTurnGameAction.ActiveInvestigator)
                .SetLogic(OneInvestigatorPlayFromHand);
            await Task.CompletedTask;

            /*******************************************************************/
            async Task OneInvestigatorPlayFromHand() =>
                 await _gameActionsProvider.Create(new PlayFromHandGameAction(this, oneInvestigatorTurnGameAction.ActiveInvestigator));
        }

        public virtual bool ConditionToPlayFromHand(GameAction gameAction)
        {
            if (gameAction is not OneInvestigatorTurnGameAction oneInvestigatorTurnGameAction) return false;
            if (CurrentZone.ZoneType != ZoneType.Hand) return false;
            if (ControlOwner != oneInvestigatorTurnGameAction.ActiveInvestigator) return false;
            if (ResourceCost.Value > ControlOwner.Resources.Value) return false;
            if (PlayFromHandTurnsCost.Value > ControlOwner.CurrentTurns.Value) return false;
            return true;
        }

        /*******************************************************************/
        public abstract Task ExecuteConditionEffect();

        public async Task PlayFromHand()
        {
            await _gameActionsProvider.Create(new MoveCardsGameAction(this, _chaptersProvider.CurrentScene.LimboZone));
            await ExecuteConditionEffect();
            await _gameActionsProvider.Create(new DiscardGameAction(this));
        }

        public virtual bool SpecificConditionToPlayFromHand() => false;

    }
}
