using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public abstract class CardCondition : Card, IPlayableFromHand, ICommitable
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProvider;

        public Stat ResourceCost { get; private set; }
        public Stat PlayFromHandTurnsCost { get; private set; }

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            ResourceCost = new Stat(Info.Cost ?? 0);
            PlayFromHandTurnsCost = new Stat(1);
        }

        /*******************************************************************/
        public int GetChallengeValue(ChallengeType challengeType)
        {
            int amount = Info.Wild ?? 0;
            if (challengeType == ChallengeType.Strength) return amount + Info.Strength ?? 0;
            if (challengeType == ChallengeType.Agility) return amount + Info.Agility ?? 0;
            if (challengeType == ChallengeType.Intelligence) return amount + Info.Intelligence ?? 0;
            if (challengeType == ChallengeType.Power) return amount + Info.Power ?? 0;
            return amount;
        }

        /*******************************************************************/
        public async Task PlayFromHand()
        {
            await _gameActionsProvider.Create(new MoveCardsGameAction(this, _chaptersProvider.CurrentScene.LimboZone));
            await ExecuteConditionEffect();
            await _gameActionsProvider.Create(new DiscardGameAction(this));
        }

        public abstract Task ExecuteConditionEffect();
    }
}
