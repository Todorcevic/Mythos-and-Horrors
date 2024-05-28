using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class ShareDamageAndFearGameAction : InteractableGameAction
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public Card ByThisCard { get; }
        public int AmountDamage { get; private set; }
        public int AmountFear { get; private set; }

        public override bool CanBeExecuted => (AmountDamage > 0 || AmountFear > 0) && ActiveInvestigator.IsInPlay;
        public override string Description => $"Recived {AmountDamage}Damage {AmountFear}Fear";

        /*******************************************************************/
        public ShareDamageAndFearGameAction(Investigator investigator, Card bythisCard, int amountDamage = 0, int amountFear = 0) :
            base(canBackToThisInteractable: true, mustShowInCenter: true, "Share harm", investigator)
        {
            ByThisCard = bythisCard;
            AmountDamage = amountDamage;
            AmountFear = amountFear;
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
                Create()
                    .SetCard(cardSelectable)
                    .SetInvestigator(cardSelectable.ControlOwner)
                    .SetCardAffected(ByThisCard)
                    .SetLogic(DoDamageAndFear);

                /*******************************************************************/
                async Task DoDamageAndFear()
                {
                    HarmToCardGameAction harm = await _gameActionsProvider.Create(new HarmToCardGameAction(cardSelectable, ByThisCard, AmountDamage, AmountFear));
                    await _gameActionsProvider.Create(new ShareDamageAndFearGameAction(ActiveInvestigator, ByThisCard, AmountDamage - harm.TotalDamageApply, AmountFear - harm.TotalFearApply));
                }
            }
        }
    }
}
