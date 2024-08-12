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

        /*******************************************************************/
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Parent method must be hide")]
        private new InteractableGameAction SetWith(bool canBackToThisInteractable, bool mustShowInCenter, string code, params string[] descriptionArgs)
        => throw new NotImplementedException();

        public ShareDamageAndFearGameAction SetWith(Investigator investigator, Card bythisCard, int amountDamage = 0, int amountFear = 0)
        {
            base.SetWith(canBackToThisInteractable: true, mustShowInCenter: true, code: "Interactable_ShareDamageAndFear", DescriptionParams(amountDamage, amountFear));
            ActiveInvestigator = investigator;
            ByThisCard = bythisCard;
            AmountDamage = amountDamage;
            AmountFear = amountFear;
            ExecuteSpecificInitialization();
            return this;

            /*******************************************************************/
            static string[] DescriptionParams(int amountDamage, int amountFear)
            {
                return new[] { amountDamage.ToString(), amountFear.ToString() };
            }
        }

        /*******************************************************************/
        private void ExecuteSpecificInitialization()
        {
            List<Card> allSelectables = new() { ActiveInvestigator.InvestigatorCard };

            if (AmountDamage > 0)
                allSelectables.AddRange(ActiveInvestigator.AidZone.Cards.OfType<IDamageable>().Cast<Card>().Except(allSelectables));

            if (AmountFear > 0)
                allSelectables.AddRange(ActiveInvestigator.AidZone.Cards.OfType<IFearable>().Cast<Card>().Except(allSelectables));


            foreach (Card cardSelectable in allSelectables)
            {
                int damage = AmountDamage > 0 && cardSelectable is IDamageable damageable ? Math.Min(AmountDamage, damageable.HealthLeft) : 0;
                int fear = AmountFear > 0 && cardSelectable is IFearable fearable ? Math.Min(AmountFear, fearable.SanityLeft) : 0;
                string localizableCode = damage > 0 && fear > 0 ? "CardEffect_ShareDamageAndFear" : damage > 0 ? "CardEffect_ShareDamage" : "CardEffect_ShareFear";
                string[] localizableArgs = damage > 0 && fear > 0 ? new[] { damage.ToString(), fear.ToString() } : damage > 0 ? new[] { damage.ToString() } : new[] { fear.ToString() };

                CreateEffect(cardSelectable, new Stat(0, false), DoDamageAndFear, PlayActionType.Choose,
                    cardSelectable.ControlOwner, localizableCode, cardAffected: ByThisCard, localizableArgs: localizableArgs);

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
