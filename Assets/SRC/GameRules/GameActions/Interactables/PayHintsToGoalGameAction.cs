using System.Collections.Generic;
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
        public PayHintsToGoalGameAction SetWith(CardGoal cardGoal, IEnumerable<Investigator> investigatorsToPay)
        {
            SetWith(canBackToThisInteractable: false, mustShowInCenter: true, "Select Investigator to pay");
            CardGoal = cardGoal;
            InvestigatorsToPay = investigatorsToPay;
            return this;
        }

        /*******************************************************************/
        public override void ExecuteSpecificInitialization()
        {
            foreach (Investigator investigator in InvestigatorsToPay.Where(investigator => investigator.Hints.Value > 0))
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
