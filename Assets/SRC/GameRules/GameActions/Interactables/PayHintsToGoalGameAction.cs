using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class PayHintsToGoalGameAction : InteractableGameAction, IInitializable
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public CardGoal CardGoal { get; }
        public IEnumerable<Investigator> InvestigatorsToPay { get; }
        public override bool CanBeExecuted => !CardGoal.Revealed.IsActive &&
            InvestigatorsToPay.Sum(investigator => investigator.Hints.Value) >= CardGoal.Hints.Value;

        /*******************************************************************/
        public PayHintsToGoalGameAction(CardGoal cardGoal, IEnumerable<Investigator> investigatorsToPay) :
            base(canBackToThisInteractable: true, mustShowInCenter: true, "Select Investigator to pay")
        {
            CardGoal = cardGoal;
            InvestigatorsToPay = investigatorsToPay;
        }
        /*******************************************************************/
        public void ExecuteSpecificInitialization()
        {
            foreach (Investigator investigator in InvestigatorsToPay.Where(investigator => investigator.Hints.Value > 0))
            {
                Create().SetCard(investigator.AvatarCard)
                    .SetInvestigator(investigator)
                    .SetCardAffected(CardGoal)
                    .SetLogic(PayHint);

                /*******************************************************************/
                async Task PayHint()
                {
                    int amountHitsToPay = CardGoal.Hints.Value < investigator.Hints.Value ? CardGoal.Hints.Value : investigator.Hints.Value;
                    await _gameActionsProvider.Create(new PayHintGameAction(investigator, CardGoal.Hints, investigator.Hints.Value));
                    await _gameActionsProvider.Create(new PayHintsToGoalGameAction(CardGoal, InvestigatorsToPay));
                }
            }
        }
    }
}
