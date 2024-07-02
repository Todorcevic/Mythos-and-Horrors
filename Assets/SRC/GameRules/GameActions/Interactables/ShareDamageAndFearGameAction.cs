using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class ShareDamageAndFearGameAction : InteractableGameAction, IPersonalInteractable
    {
        public Investigator ActiveInvestigator { get; private set; }
        public Card ByThisCard { get; private set; }
        public int AmountDamage { get; private set; }
        public int AmountFear { get; private set; }

        public override bool CanBeExecuted => (AmountDamage > 0 || AmountFear > 0) && ActiveInvestigator.IsInPlay;
        public override string Description => $"Recived {AmountDamage}Damage {AmountFear}Fear";

        /*******************************************************************/
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Parent method must be hide")]
        private new InteractableGameAction SetWith(bool canBackToThisInteractable, bool mustShowInCenter, string description)
        => throw new NotImplementedException();

        public ShareDamageAndFearGameAction SetWith(Investigator investigator, Card bythisCard, int amountDamage = 0, int amountFear = 0)
        {
            base.SetWith(canBackToThisInteractable: true, mustShowInCenter: true, "Share harm");
            ActiveInvestigator = investigator;
            ByThisCard = bythisCard;
            AmountDamage = amountDamage;
            AmountFear = amountFear;
            return this;
        }

        /*******************************************************************/
        public override void ExecuteSpecificInitialization()
        {
            List<Card> allSelectables = new() { ActiveInvestigator.InvestigatorCard };

            if (AmountDamage > 0)
                allSelectables.AddRange(ActiveInvestigator.AidZone.Cards.OfType<IDamageable>().Cast<Card>().Except(allSelectables));

            if (AmountFear > 0)
                allSelectables.AddRange(ActiveInvestigator.AidZone.Cards.OfType<IFearable>().Cast<Card>().Except(allSelectables));


            foreach (Card cardSelectable in allSelectables)
            {
                CreateEffect(cardSelectable, new Stat(0, false), DoDamageAndFear, PlayActionType.Choose, cardSelectable.ControlOwner, cardAffected: ByThisCard);

                /*******************************************************************/
                async Task DoDamageAndFear()
                {
                    HarmToCardGameAction harm = _gameActionsProvider.Create<HarmToCardGameAction>().SetWith(cardSelectable, ByThisCard, AmountDamage, AmountFear);
                    await harm.Execute();
                    await _gameActionsProvider.Create<ShareDamageAndFearGameAction>().SetWith(ActiveInvestigator, ByThisCard, AmountDamage - harm.TotalDamageApply, AmountFear - harm.TotalFearApply).Execute();
                }
            }
        }
    }
}
