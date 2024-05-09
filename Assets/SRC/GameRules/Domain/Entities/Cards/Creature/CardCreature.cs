using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class CardCreature : Card, IDamageable
    {
        [Inject] private readonly CardsProvider _cardsProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorProvider;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public Stat Health { get; private set; }
        public Stat Strength { get; private set; }
        public Stat Agility { get; private set; }
        public Stat Damage { get; private set; }
        public Stat Fear { get; private set; }
        public Stat InvestigatorAttackTurnsCost { get; private set; }
        public Stat InvestigatorConfronTurnsCost { get; private set; }
        public Stat EludeTurnsCost { get; private set; }
        public IReaction DefeatReaction => _reactionablesProvider.FindReactionByLogic<UpdateStatGameAction>(DefeatLogic);
        public IReaction ConfrontReaction => _reactionablesProvider.FindReactionByLogic<MoveCardsGameAction>(ConfrontLogic);
        public IReaction ConfrontReaction2 => _reactionablesProvider.FindReactionByLogic<UpdateStatesGameAction>(ConfrontLogic);

        /*******************************************************************/
        public int TotalEnemyHits => (Info.CreatureDamage ?? 0) + (Info.CreatureFear ?? 0);
        public bool IsConfronted => ConfrontedInvestigator != null;
        public Investigator ConfrontedInvestigator
        {
            get
            {
                Investigator investigator = _investigatorProvider.GetInvestigatorWithThisZone(CurrentZone);
                return CurrentZone == investigator?.DangerZone ? investigator : null;
            }
        }

        public CardPlace CurrentPlace => _cardsProvider.GetCardWithThisZone(CurrentZone) as CardPlace ?? ConfrontedInvestigator?.CurrentPlace;

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            Health = CreateStat(Info.Health ?? 0);
            Strength = CreateStat(Info.Strength ?? 0);
            Agility = CreateStat(Info.Agility ?? 0);
            Damage = CreateStat(Info.CreatureDamage ?? 0);
            Fear = CreateStat(Info.CreatureFear ?? 0);
            InvestigatorAttackTurnsCost = CreateStat(1);
            InvestigatorConfronTurnsCost = CreateStat(1);
            EludeTurnsCost = CreateStat(1);
            CreateReaction<UpdateStatGameAction>(DefeatCondition, DefeatLogic, false);
            CreateReaction<MoveCardsGameAction>(ConfrontCondition, ConfrontLogic, false);
            CreateReaction<UpdateStatesGameAction>(ConfrontCondition, ConfrontLogic, false);
        }

        /*******************************************************************/
        private bool DefeatCondition(UpdateStatGameAction updateStatGameAction)
        {
            if (!IsInPlay) return false;
            if (Health.Value > 0) return false;
            return true;
        }

        private async Task DefeatLogic(UpdateStatGameAction updateStatGameAction)
        {
            Card byThisCard = null;
            if (updateStatGameAction.Parent is HarmToCardGameAction harmToCardGameAction) byThisCard = harmToCardGameAction.ByThisCard;
            await _gameActionsProvider.Create(new DefeatCardGameAction(this, byThisCard));
        }

        /*******************************************************************/
        private bool ConfrontCondition(GameAction gameAction)
        {
            if (!IsInPlay) return false;
            if (Exausted.IsActive) return false;
            if (IsConfronted) return false;
            if (_investigatorProvider.GetInvestigatorsInThisPlace(CurrentPlace).Count() < 1) return false;
            if (this is ITarget target && target.IsOnlyOneTarget && target.TargetInvestigator.CurrentPlace != CurrentPlace) return false;
            return true;
        }

        private async Task ConfrontLogic(GameAction gameAction)
        {
            await _gameActionsProvider.Create(new ConfrontCreatureGameAction(this));
        }
    }
}
