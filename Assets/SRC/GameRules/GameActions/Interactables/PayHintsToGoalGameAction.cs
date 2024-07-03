using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class PayHintsToGoalGameAction : InteractableGameAction
    {
        public CardGoal CardGoal { get; private set; }
        public IEnumerable<Investigator> InvestigatorsToPay { get; private set; }
        public override bool CanBeExecuted => CardGoal.IsInPlay && !CardGoal.Revealed.IsActive && CardGoal.Hints.Value > 0;
        public List<CardEffect> EffectsToPay { get; } = new();

        /*******************************************************************/
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Parent method must be hide")]
        private new InteractableGameAction SetWith(bool canBackToThisInteractable, bool mustShowInCenter, string description)
        => throw new NotImplementedException();

        public PayHintsToGoalGameAction SetWith(CardGoal cardGoal, IEnumerable<Investigator> investigatorsToPay)
        {
            base.SetWith(canBackToThisInteractable: false, mustShowInCenter: true, "Select Investigator to pay");
            CardGoal = cardGoal;
            InvestigatorsToPay = investigatorsToPay;
            return this;
        }

        /*******************************************************************/
        public override void ExecuteSpecificInitialization()
        {
            foreach (Investigator investigator in InvestigatorsToPay.Where(investigator => investigator.CanPayHints.IsActive))
            {
                EffectsToPay.Add(CreateEffect(investigator.AvatarCard, new Stat(0, false), PayHint, PlayActionType.Choose, playedBy: investigator));

                /*******************************************************************/
                async Task PayHint()
                {
                    int amoutToPay = investigator.Hints.Value > CardGoal.Hints.Value ? CardGoal.Hints.Value : investigator.Hints.Value;
                    await _gameActionsProvider.Create<PayHintGameAction>().SetWith(investigator, CardGoal.Hints, amoutToPay).Execute();
                    await _gameActionsProvider.Create<PayHintsToGoalGameAction>().SetWith(CardGoal, InvestigatorsToPay).Execute();
                }
            }
        }

        public void UpdateInvestigatorsToPay(IEnumerable<Investigator> investigatorsToPay)
        {
            InvestigatorsToPay = investigatorsToPay;
            IEnumerable<CardEffect> effectsToRemove = EffectsToPay.Where(effect => !investigatorsToPay.Contains(effect.CardOwner.Owner));

            RemoveEffects(effectsToRemove);
            EffectsToPay.Clear();
            EffectsToPay.AddRange(effectsToRemove);
        }
    }
}
