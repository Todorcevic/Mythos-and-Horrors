using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01576 : CardSupply, IDamageable
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public Stat Health { get; private set; }
        public Stat DamageRecived { get; private set; }
        public override IEnumerable<Tag> Tags => new[] { Tag.Ally, Tag.Creature };

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            Health = CreateStat(Info.Health ?? 0);
            DamageRecived = CreateStat(0);
            CreateFastActivation(Logic, Condition, PlayActionType.Activate, "Activation_Card01576");
        }

        /*******************************************************************/
        private bool Condition(Investigator investigator)
        {
            if (!IsInPlay) return false;
            if (investigator != ControlOwner) return false;
            if (!investigator.AllTypeCreaturesConfronted.Any()) return false;
            return true;
        }

        private async Task Logic(Investigator investigator)
        {
            InteractableGameAction interactableGameAction = _gameActionsProvider.Create<InteractableGameAction>()
                .SetWith(canBackToThisInteractable: false, mustShowInCenter: true, "Interactable_Card01576");

            foreach (CardCreature creature in investigator.AllTypeCreaturesConfronted)
            {
                interactableGameAction.CreateEffect(creature, new Stat(0, false), Elude, PlayActionType.Elude, investigator,
                    "CardEffect_Card01576", cardAffected: this);

                /*******************************************************************/
                async Task Elude() => await _gameActionsProvider.Create<EludeGameAction>().SetWith(creature, ControlOwner).Execute();
            }

            await _gameActionsProvider.Create<DiscardGameAction>().SetWith(this).Execute();
            await interactableGameAction.Execute();
        }
    }
}
