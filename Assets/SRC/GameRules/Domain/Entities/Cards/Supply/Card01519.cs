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
            CreateActivation(1, HealthActivate, HealConditionToActivate, PlayActionType.Activate, new Localization("Activation_Card01519"));
            CreateForceReaction<UpdateStatGameAction>(DiscardCondition, DiscardLogic, GameActionTime.After);
        }

        /*******************************************************************/
        private async Task DiscardLogic(UpdateStatGameAction updateStatGameAction)
        {
            await _gameActionsProvider.Create<DiscardGameAction>().SetWith(this).Execute();
        }

        private bool DiscardCondition(UpdateStatGameAction updateStatGameAction)
        {
            if (IsInPlay.IsFalse) return false;
            if (!updateStatGameAction.HasThisStat(Charge.Amount)) return false;
            if (Charge.Amount.Value > 0) return false;
            return true;
        }

        /*******************************************************************/
        public async Task HealthActivate(Investigator activeInvestigator)
        {
            await _gameActionsProvider.Create<DecrementStatGameAction>().SetWith(Charge.Amount, 1).Execute();

            InteractableGameAction interactableGameAction = _gameActionsProvider.Create<InteractableGameAction>()
                .SetWith(canBackToThisInteractable: false, mustShowInCenter: true, new Localization("Interactable_Card01519"));

            foreach (Investigator investigator in activeInvestigator.CurrentPlace.InvestigatorsInThisPlace
                .Where(investigator => investigator.CanBeHealed.IsTrue))
            {
                interactableGameAction.CreateCardEffect(investigator.InvestigatorCard,
                    new Stat(0, false),
                    RestoreHealthInvestigator,
                    PlayActionType.Choose,
                    playedBy: activeInvestigator,
                    new Localization("CardEffect_Card01519"));

                /*******************************************************************/
                async Task RestoreHealthInvestigator() => await _gameActionsProvider.Create<RecoverGameAction>().SetWith(investigator.InvestigatorCard, amountDamageToRecovery: 1).Execute();
            }

            foreach (Investigator investigator in activeInvestigator.CurrentPlace.InvestigatorsInThisPlace
                .Where(investigator => investigator.CanBeRestoreSanity.IsTrue))
            {
                interactableGameAction.CreateCardEffect(investigator.InvestigatorCard,
                    new Stat(0, false),
                    RestoreSanityInvestigator,
                    PlayActionType.Choose,
                    playedBy: activeInvestigator,
                    new Localization("CardEffect_Card01519-1"));

                /*******************************************************************/
                async Task RestoreSanityInvestigator() => await _gameActionsProvider.Create<RecoverGameAction>().SetWith(investigator.InvestigatorCard, amountFearToRecovery: 1).Execute();
            }

            await interactableGameAction.Execute();
        }

        public bool HealConditionToActivate(Investigator activeInvestigator)
        {
            if (IsInPlay.IsFalse) return false;
            if (ControlOwner != activeInvestigator) return false;
            if (!activeInvestigator.CurrentPlace.InvestigatorsInThisPlace.Any(investigator => investigator.CanBeHealed.IsTrue || investigator.CanBeRestoreSanity.IsTrue)) return false;
            return true;
        }
    }
}