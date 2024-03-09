using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class CardCreature : Card, IStartReactionable
    {
        [Inject] private readonly GameActionProvider _gameActionFactory;
        [Inject] private readonly CardsProvider _cardsProvider;
        [Inject] private readonly TextsProvider _textsProvider;
        [Inject] private readonly EffectsProvider _effectProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorProvider;

        public Stat Health { get; private set; }
        public Stat Strength { get; private set; }
        public Stat Agility { get; private set; }
        public Stat FightTurnsCost { get; private set; }
        public int TotalEnemyHits => (Info.EnemyDamage ?? 0) + (Info.EnemyFear ?? 0);
        public bool IsEnganged => _investigatorProvider.GetInvestigatorWithThisZone(CurrentZone)?.DangerZone == CurrentZone;
        public CardPlace CurrentPlace
        {
            get
            {
                if (IsEnganged) return _investigatorProvider.GetInvestigatorWithThisZone(CurrentZone).CurrentPlace;
                return _cardsProvider.GetCardWithThisZone(CurrentZone) as CardPlace;
            }
        }

        public Effect AttackEffect => _effectProvider.GetSpecificEffect(InvestigatorAttack);

        /*******************************************************************/
        [Inject]
        private void Init()
        {
            Health = new Stat(Info.Health ?? 0, Info.Health ?? 0);
            Strength = new Stat(Info.Strength ?? 0);
            Agility = new Stat(Info.Agility ?? 0);
            FightTurnsCost = new Stat(1, 1);
        }

        /*******************************************************************/
        public virtual Task WhenBegin(GameAction gameAction)
        {
            CheckInvestigatorAttack(gameAction);
            return Task.CompletedTask;
        }

        /************************** INVESTIGATOR ATTACK *****************************/
        protected void CheckInvestigatorAttack(GameAction gameAction)
        {
            if (gameAction is not OneInvestigatorTurnGameAction oneTurnGA) return;

            _effectProvider.Create()
                .SetCard(this)
                .SetDescription(_textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(InvestigatorAttack))
                .SetInvestigator(_investigatorProvider.ActiveInvestigator)
                .SetCanPlay(CanInvestigatorAttack)
                .SetLogic(InvestigatorAttack);
        }

        protected bool CanInvestigatorAttack()
        {
            if (_investigatorProvider.ActiveInvestigator.Turns.Value < FightTurnsCost.Value) return false;
            if (_investigatorProvider.ActiveInvestigator.CurrentPlace != CurrentPlace) return false;
            return true;
        }

        protected async Task InvestigatorAttack()
        {
            await _gameActionFactory.Create(new DecrementStatGameAction(_investigatorProvider.ActiveInvestigator.Turns, FightTurnsCost.Value));
            await _gameActionFactory.Create(new DecrementStatGameAction(Health, 1));
        }
    }
}
