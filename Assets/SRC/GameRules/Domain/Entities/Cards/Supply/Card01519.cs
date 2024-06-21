using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01519 : CardSupply, ISupplietable
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Talent, Tag.Science };
        public Stat AmountSupplies { get; private set; }

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            AmountSupplies = CreateStat(3);
            CreateActivation(1, HealActivate, HealConditionToActivate, PlayActionType.Activate);
            CreateForceReaction<UpdateStatGameAction>(DiscardCondition, DiscardLogic, GameActionTime.After);
        }

        /*******************************************************************/
        private async Task DiscardLogic(UpdateStatGameAction updateStatGameAction)
        {
            await _gameActionsProvider.Create(new DiscardGameAction(this));
        }

        private bool DiscardCondition(UpdateStatGameAction updateStatGameAction)
        {
            if (!IsInPlay) return false;
            if (!updateStatGameAction.HasThisStat(AmountSupplies)) return false;
            if (AmountSupplies.Value > 0) return false;
            return true;
        }

        /*******************************************************************/
        public async Task HealActivate(Investigator activeInvestigator)
        {
            await _gameActionsProvider.Create(new DecrementStatGameAction(AmountSupplies, 1));

            InteractableGameAction interactableGameAction = new(canBackToThisInteractable: false, mustShowInCenter: true, "Select Investigator");

            foreach (Investigator investigator in _investigatorsProvider.GetInvestigatorsInThisPlace(activeInvestigator.CurrentPlace)
                .Where(investigator => investigator.CanBeHealed))
            {
                interactableGameAction.CreateEffect(investigator.AvatarCard,
                    new Stat(0, false),
                    RestoreHealthInvestigator,
                    PlayActionType.Choose,
                    playedBy: activeInvestigator,
                    cardAffected: investigator.InvestigatorCard);

                /*******************************************************************/
                async Task RestoreHealthInvestigator() => await _gameActionsProvider.Create(new HealthGameAction(investigator.InvestigatorCard, amountDamageToRecovery: 1));
            }

            foreach (Investigator investigator in _investigatorsProvider.GetInvestigatorsInThisPlace(activeInvestigator.CurrentPlace)
                .Where(investigator => investigator.CanBeRestoreSanity))
            {
                interactableGameAction.CreateEffect(investigator.AvatarCard,
                    new Stat(0, false),
                    RestoreSanityInvestigator,
                    PlayActionType.Choose,
                    playedBy: activeInvestigator,
                    cardAffected: investigator.InvestigatorCard);

                /*******************************************************************/
                async Task RestoreSanityInvestigator() => await _gameActionsProvider.Create(new HealthGameAction(investigator.InvestigatorCard, amountFearToRecovery: 1));
            }

            await _gameActionsProvider.Create(interactableGameAction);
        }

        public bool HealConditionToActivate(Investigator activeInvestigator)
        {
            if (!IsInPlay) return false;
            if (ControlOwner != activeInvestigator) return false;
            return true;
        }
    }
}