using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class PayHintsToGoalGameAction : InteractableGameAction
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public CardGoal CardGoal { get; }
        public IEnumerable<Investigator> InvestigatorsToPay { get; }
        public override bool CanBeExecuted => CardGoal.IsInPlay && !CardGoal.Revealed.IsActive && CardGoal.Hints.Value > 0;
        public List<Effect> EffectsToPay { get; } = new();

        /*******************************************************************/
        public PayHintsToGoalGameAction(CardGoal cardGoal, IEnumerable<Investigator> investigatorsToPay, Investigator activeInvestigator) :
            base(canBackToThisInteractable: false, mustShowInCenter: true, "Select Investigator to pay", activeInvestigator)
        {
            CardGoal = cardGoal;
            InvestigatorsToPay = investigatorsToPay;
        }
        /*******************************************************************/
        public override void ExecuteSpecificInitialization()
        {
            CreateCancelMainButton();

            foreach (Investigator investigator in InvestigatorsToPay.Where(investigator => investigator.Hints.Value > 0))
            {
                EffectsToPay.Add(Create(investigator.AvatarCard, PayHint, PlayActionType.Choose, playedBy: investigator));

                /*******************************************************************/
                async Task PayHint()
                {
                    int amoutToPay = investigator.Hints.Value > CardGoal.Hints.Value ? CardGoal.Hints.Value : investigator.Hints.Value;
                    await _gameActionsProvider.Create(new PayHintGameAction(investigator, CardGoal.Hints, amoutToPay));
                    await _gameActionsProvider.Create(new PayHintsToGoalGameAction(CardGoal, InvestigatorsToPay, ActiveInvestigator));
                }
            }
        }
    }
}
