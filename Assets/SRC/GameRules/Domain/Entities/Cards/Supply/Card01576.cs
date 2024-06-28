using System;
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
            CreateFastActivation(Logic, Condition, PlayActionType.Activate);
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
            InteractableGameAction interactableGameAction = new(canBackToThisInteractable: false, mustShowInCenter: true, "Choose Creature");
            interactableGameAction.CreateCancelMainButton();

            foreach (CardCreature creature in investigator.AllTypeCreaturesConfronted)
            {
                interactableGameAction.CreateEffect(creature, new Stat(0, false), Elude, PlayActionType.Elude, investigator);

                async Task Elude() => await _gameActionsProvider.Create(new EludeGameAction(creature, ControlOwner));
            }

            await _gameActionsProvider.Create(new DiscardGameAction(this));
            await _gameActionsProvider.Create(interactableGameAction);
        }
    }
}
