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
        public IEnumerable<Investigator> InvestigatorsToPay { get; private set; }
        public override bool CanBeExecuted => CardGoal.IsInPlay && !CardGoal.Revealed.IsActive && CardGoal.Hints.Value > 0;
        public List<CardEffect> EffectsToPay { get; } = new();

        /*******************************************************************/
        public PayHintsToGoalGameAction(CardGoal cardGoal, IEnumerable<Investigator> investigatorsToPay) :
            base(canBackToThisInteractable: false, mustShowInCenter: true, "Select Investigator to pay")
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
                EffectsToPay.Add(CreateEffect(investigator.AvatarCard, new Stat(0, false), PayHint, PlayActionType.Choose, playedBy: investigator));

                /*******************************************************************/
                async Task PayHint()
                {
                    int amoutToPay = investigator.Hints.Value > CardGoal.Hints.Value ? CardGoal.Hints.Value : investigator.Hints.Value;
                    await _gameActionsProvider.Create(new PayHintGameAction(investigator, CardGoal.Hints, amoutToPay));
                    await _gameActionsProvider.Create(new PayHintsToGoalGameAction(CardGoal, InvestigatorsToPay));
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
