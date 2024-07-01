using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01519 : CardSupply, IChargeable
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Talent, Tag.Science };
        public Charge Charge { get; private set; }

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            Charge = new Charge(3, ChargeType.Supplie);
            CreateActivation(1, HealthActivate, HealConditionToActivate, PlayActionType.Activate);
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
            if (!updateStatGameAction.HasThisStat(Charge.Amount)) return false;
            if (Charge.Amount.Value > 0) return false;
            return true;
        }

        /*******************************************************************/
        public async Task HealthActivate(Investigator activeInvestigator)
        {
            await _gameActionsProvider.Create<DecrementStatGameAction>().SetWith(Charge.Amount, 1).Start();

            InteractableGameAction interactableGameAction = _gameActionsProvider.Create<InteractableGameAction>()
                .SetWith(canBackToThisInteractable: false, mustShowInCenter: true, "Select Investigator");

            foreach (Investigator investigator in activeInvestigator.CurrentPlace.InvestigatorsInThisPlace
                .Where(investigator => investigator.CanBeHealed))
            {
                interactableGameAction.CreateEffect(investigator.InvestigatorCard,
                    new Stat(0, false),
                    RestoreHealthInvestigator,
                    PlayActionType.Choose,
                    playedBy: activeInvestigator);

                /*******************************************************************/
                async Task RestoreHealthInvestigator() => await _gameActionsProvider.Create(new HealthGameAction(investigator.InvestigatorCard, amountDamageToRecovery: 1));
            }

            foreach (Investigator investigator in activeInvestigator.CurrentPlace.InvestigatorsInThisPlace
                .Where(investigator => investigator.CanBeRestoreSanity))
            {
                interactableGameAction.CreateEffect(investigator.InvestigatorCard,
                    new Stat(0, false),
                    RestoreSanityInvestigator,
                    PlayActionType.Choose,
                    playedBy: activeInvestigator);

                /*******************************************************************/
                async Task RestoreSanityInvestigator() => await _gameActionsProvider.Create(new HealthGameAction(investigator.InvestigatorCard, amountFearToRecovery: 1));
            }

            await interactableGameAction.Start();
        }

        public bool HealConditionToActivate(Investigator activeInvestigator)
        {
            if (!IsInPlay) return false;
            if (ControlOwner != activeInvestigator) return false;
            if (!activeInvestigator.CurrentPlace.InvestigatorsInThisPlace.Any(investigator => investigator.CanBeHealed || investigator.CanBeRestoreSanity)) return false;
            return true;
        }
    }
}