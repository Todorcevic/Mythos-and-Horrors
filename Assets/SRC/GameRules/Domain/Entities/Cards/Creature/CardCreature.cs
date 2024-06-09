using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class CardCreature : Card, IDamageable, IEldritchable
    {
        [Inject] private readonly CardsProvider _cardsProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly ReactionablesProvider _reactionablesProvider;

        public Stat Health { get; protected set; }
        public Stat DamageRecived { get; private set; }
        public Stat Strength { get; private set; }
        public Stat Agility { get; private set; }
        public Stat Damage { get; private set; }
        public Stat Fear { get; private set; }
        public Stat InvestigatorAttackTurnsCost { get; private set; }
        public Stat InvestigatorConfronTurnsCost { get; private set; }
        public Stat EludeTurnsCost { get; private set; }
        public Stat Eldritch { get; private set; }
        public Reaction<MoveCardsGameAction> ConfrontWhenMoveReaction { get; private set; }
        public Reaction<UpdateStatesGameAction> ConfrontWhenReadyReaction { get; private set; }

        /*******************************************************************/
        public int TotalEnemyHits => (Info.CreatureDamage ?? 0) + (Info.CreatureFear ?? 0);
        public int HealthLeft => Health.Value - DamageRecived.Value;
        public virtual bool IsConfronted => ConfrontedInvestigator != null;
        public Investigator ConfrontedInvestigator =>
            CurrentZone.ZoneType == ZoneType.Danger ? _investigatorsProvider.GetInvestigatorWithThisZone(CurrentZone) : null;
        public CardPlace CurrentPlace => _cardsProvider.GetCardWithThisZone(CurrentZone) as CardPlace ?? ConfrontedInvestigator?.CurrentPlace;

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            Health = CreateStat(Info.Health ?? 0);
            DamageRecived = CreateStat(0);
            Strength = CreateStat(Info.Strength ?? 0);
            Agility = CreateStat(Info.Agility ?? 0);
            Damage = CreateStat(Info.CreatureDamage ?? 0);
            Fear = CreateStat(Info.CreatureFear ?? 0);
            Eldritch = CreateStat(0);
            InvestigatorAttackTurnsCost = CreateStat(1);
            InvestigatorConfronTurnsCost = CreateStat(1);
            EludeTurnsCost = CreateStat(1);
            ConfrontWhenMoveReaction = CreateReaction<MoveCardsGameAction>(ConfrontCondition, ConfrontLogic, false);
            ConfrontWhenReadyReaction = CreateReaction<UpdateStatesGameAction>(ConfrontCondition, ConfrontLogic, false);
        }

        /*******************************************************************/
        private bool ConfrontCondition(GameAction gameAction)
        {
            if (!IsInPlay) return false;
            if (Exausted.IsActive) return false;
            if (IsConfronted) return false;
            if (_investigatorsProvider.GetInvestigatorsInThisPlace(CurrentPlace).Count() < 1) return false;
            if (this is ITarget target && target.IsUniqueTarget && target.TargetInvestigator.CurrentPlace != CurrentPlace) return false;
            return true;
        }

        private async Task ConfrontLogic(GameAction gameAction)
        {
            await _gameActionsProvider.Create(new ConfrontCreatureGameAction(this));
        }

        /*******************************************************************/
        public CardPlace GetPlaceToStalkerMove()
        {
            if (this is ITarget target && target.IsUniqueTarget) return target.TargetInvestigator.IsInPlay ?
                    CurrentPlace.DistanceTo(target.TargetInvestigator.CurrentPlace).path :
                    CurrentPlace;

            Dictionary<Investigator, CardPlace> finalResult = new();
            (CardPlace path, int distance) winner = (default, int.MaxValue);

            foreach (Investigator investigator in _investigatorsProvider.AllInvestigatorsInPlay)
            {
                (CardPlace path, int distance) result = CurrentPlace.DistanceTo(investigator.CurrentPlace);
                if (result.distance == winner.distance) finalResult.Add(investigator, result.path);
                else if (result.distance < winner.distance)
                {
                    finalResult.Clear();
                    finalResult.Add(investigator, result.path);
                    winner = result;
                }
            }

            if (this is ITarget targetCreature && finalResult.TryGetValue(targetCreature.TargetInvestigator, out CardPlace place))
                return place;

            return finalResult.First().Value;
        }
    }
}
