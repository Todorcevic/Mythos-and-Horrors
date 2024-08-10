using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01691 : CardSupply, IDamageable, IFearable
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public Stat Health { get; private set; }
        public Stat DamageRecived { get; private set; }
        public Stat Sanity { get; private set; }
        public Stat FearRecived { get; private set; }
        public override IEnumerable<Tag> Tags => new[] { Tag.Ally };

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            Health = CreateStat(Info.Health ?? 0);
            DamageRecived = CreateStat(0);
            Sanity = CreateStat(Info.Sanity ?? 0);
            FearRecived = CreateStat(0);
            CreateOptativeReaction<HarmToInvestigatorGameAction>(Condition, Logic, GameActionTime.Before);
        }

        /*******************************************************************/
        private async Task Logic(HarmToInvestigatorGameAction harmToInvestigatorGameAction)
        {
            if (harmToInvestigatorGameAction.Parent is not CreatureAttackGameAction creatureAttackGameAction) return;
            int creatureDamage = creatureAttackGameAction.Creature.Damage.Value;
            harmToInvestigatorGameAction.AddAmountDamage(-creatureDamage);

            InteractableGameAction interactableGameAction = _gameActionsProvider.Create<InteractableGameAction>()
                .SetWith(canBackToThisInteractable: false, mustShowInCenter: true, "Interactable_Card01691");

            foreach (CardCreature creature in creatureAttackGameAction.Creature.CurrentPlace.CreaturesInThisPlace)
            {
                interactableGameAction.CreateEffect(creature, new Stat(0, false), DamageLogic, PlayActionType.Choose, ControlOwner);

                /*******************************************************************/
                async Task DamageLogic()
                {
                    await _gameActionsProvider.Create<HarmToCardGameAction>().SetWith(creature, creatureAttackGameAction.Creature, amountDamage: creatureDamage).Execute();
                }
            }

            await _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(Exausted, true).Execute();
            await _gameActionsProvider.Create<HarmToCardGameAction>().SetWith(this, this, amountFear: 1).Execute();
            await interactableGameAction.Execute();
        }

        private bool Condition(HarmToInvestigatorGameAction harmToInvestigatorGameAction)
        {
            if (harmToInvestigatorGameAction.Parent is not CreatureAttackGameAction creatureAttackGameAction) return false;
            if (!IsInPlay) return false;
            if (Exausted.IsActive) return false;
            if (harmToInvestigatorGameAction.Investigator != ControlOwner) return false;
            if (!creatureAttackGameAction.Creature.CurrentPlace.CreaturesInThisPlace.Any()) return false;
            return true;
        }

    }
}
