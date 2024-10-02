using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class PayKeysToGoalGameAction : InteractableGameAction
    {
        public CardGoal CardGoal { get; private set; }
        public IEnumerable<Investigator> InvestigatorsToPay { get; private set; }
        public override bool CanBeExecuted => CardGoal.IsInPlay.IsTrue && !CardGoal.Revealed.IsActive && CardGoal.Keys.Value > 0;
        public List<CardEffect> EffectsToPay { get; } = new();

        /*******************************************************************/
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Parent method must be hide")]
        private new InteractableGameAction SetWith(bool canBackToThisInteractable, bool mustShowInCenter, Localization localization)
        => throw new NotImplementedException();

        public PayKeysToGoalGameAction SetWith(CardGoal cardGoal, IEnumerable<Investigator> investigatorsToPay)
        {
            base.SetWith(canBackToThisInteractable: false, mustShowInCenter: true, new Localization("Interactable_PayKeysToGoal", cardGoal.Keys.Value.ToString(), cardGoal.CurrentName));
            CardGoal = cardGoal;
            InvestigatorsToPay = investigatorsToPay;
            ExecuteSpecificInitialization();
            return this;
        }

        /*******************************************************************/
        private void ExecuteSpecificInitialization()
        {
            foreach (Investigator investigator in InvestigatorsToPay.Where(investigator => investigator.CanPayKeys.IsTrue))
            {
                EffectsToPay.Add(CreateCardEffect(investigator.AvatarCard, new Stat(0, false), PayKey, PlayActionType.Choose, playedBy: investigator,
                    new Localization("CreateEffect_PayKeysToGoal", investigator.Keys.Value.ToString())));

                /*******************************************************************/
                async Task PayKey()
                {
                    int amoutToPay = investigator.Keys.Value > CardGoal.Keys.Value ? CardGoal.Keys.Value : investigator.Keys.Value;
                    await _gameActionsProvider.Create<PayKeyGameAction>().SetWith(investigator, CardGoal.Keys, amoutToPay).Execute();
                    await _gameActionsProvider.Create<PayKeysToGoalGameAction>().SetWith(CardGoal, InvestigatorsToPay).Execute();
                }
            }
        }
    }
}
