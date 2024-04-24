using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class CardSupply : Card, IPlayableFromHand, ICommitable
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public Stat ResourceCost { get; private set; }
        public Stat PlayFromHandTurnsCost { get; protected set; }
        public Stat Health { get; private set; }
        public Stat Sanity { get; private set; }
        public Reaction<UpdateStatGameAction> Defeat { get; private set; }

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            ResourceCost = new Stat(Info.Cost ?? 0);
            PlayFromHandTurnsCost = new Stat(1);
            Defeat = new Reaction<UpdateStatGameAction>(DefeatCondition, DefeatLogic);
            if (this is IDamageable) Health = new Stat(Info.Health ?? 0);
            if (this is IFearable) Sanity = new Stat(Info.Sanity ?? 0);
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
        protected override async Task WhenFinish(GameAction gameAction)
        {
            await Defeat.Check(gameAction);
        }

        /*******************************************************************/
        private bool DefeatCondition(GameAction gameAction)
        {
            if (!IsInPlay) return false;
            if (!DieByDamage() && !DieByFear()) return false;
            return true;

            bool DieByDamage()
            {
                if (this is not IDamageable) return false;
                if (Health.Value > 0) return false;
                return true; ;
            }

            bool DieByFear()
            {
                if (this is not IFearable) return false;
                if (Sanity.Value > 0) return false;
                return true;
            }
        }

        private async Task DefeatLogic(GameAction gameAction) => await _gameActionsProvider.Create(new DefeatSupplyGameAction(this));

        /*******************************************************************/
        public async Task PlayFromHand()
        {
            await _gameActionsProvider.Create(new MoveCardsGameAction(this, Owner.AidZone));
        }
    }
}
