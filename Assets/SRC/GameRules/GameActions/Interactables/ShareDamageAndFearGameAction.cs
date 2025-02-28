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
        public Stat AmountDamage { get; private set; }
        public Stat AmountFear { get; private set; }
        public override bool CanBeExecuted => (AmountDamage.Value > 0 || AmountFear.Value > 0) && ActiveInvestigator.IsInPlay.IsTrue;

        /*******************************************************************/
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Parent method must be hide")]
        private new InteractableGameAction SetWith(bool canBackToThisInteractable, bool mustShowInCenter, Localization localization) => throw new NotImplementedException();

        public ShareDamageAndFearGameAction SetWith(Investigator investigator, Card bythisCard, int amountDamage = 0, int amountFear = 0)
        {
            base.SetWith(canBackToThisInteractable: false, mustShowInCenter: true, new Localization("Interactable_ShareDamageAndFear", amountDamage.ToString(), amountFear.ToString()));
            ActiveInvestigator = investigator;
            ByThisCard = bythisCard;
            AmountDamage = new(amountDamage, false);
            AmountFear = new(amountFear, false);
            ExecuteSpecificInitialization();
            return this;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await base.ExecuteThisLogic();
            if (IsMainButtonPressed || IsUndoPressed) return;
            await _gameActionsProvider.Create<ShareDamageAndFearGameAction>().SetWith(ActiveInvestigator, ByThisCard, AmountDamage.Value, AmountFear.Value).Execute();
        }

        /*******************************************************************/
        private void ExecuteSpecificInitialization()
        {
            List<Card> allSelectables = new() { ActiveInvestigator.InvestigatorCard };

            if (AmountDamage.Value > 0)
                allSelectables.AddRange(ActiveInvestigator.AidZone.Cards.OfType<IDamageable>().Cast<Card>().Except(allSelectables));

            if (AmountFear.Value > 0)
                allSelectables.AddRange(ActiveInvestigator.AidZone.Cards.OfType<IFearable>().Cast<Card>().Except(allSelectables));


            foreach (Card cardSelectable in allSelectables)
            {
                int damage = AmountDamage.Value > 0 && cardSelectable is IDamageable damageable ? Math.Min(AmountDamage.Value, damageable.HealthLeft) : 0;
                int fear = AmountFear.Value > 0 && cardSelectable is IFearable fearable ? Math.Min(AmountFear.Value, fearable.SanityLeft) : 0;
                string localizableCode = damage > 0 && fear > 0 ? "CardEffect_ShareDamageAndFear" : damage > 0 ? "CardEffect_ShareDamage" : "CardEffect_ShareFear";
                string[] localizableArgs = damage > 0 && fear > 0 ? new[] { damage.ToString(), fear.ToString() } : damage > 0 ? new[] { damage.ToString() } : new[] { fear.ToString() };

                CreateCardEffect(cardSelectable, new Stat(0, false), DoDamageAndFear, PlayActionType.Choose,
                    cardSelectable.ControlOwner, new Localization(localizableCode, localizableArgs), cardAffected: ByThisCard);

                /*******************************************************************/
                async Task DoDamageAndFear()
                {
                    HarmToCardGameAction harm = _gameActionsProvider.Create<HarmToCardGameAction>().SetWith(cardSelectable, ByThisCard, AmountDamage.Value, AmountFear.Value);
                    await harm.Execute();
                    Dictionary<Stat, int> statsToDecrement = new()
                    {
                        { AmountDamage, harm.TotalDamageApply },
                        { AmountFear, harm.TotalFearApply }
                    };
                    await _gameActionsProvider.Create<DecrementStatGameAction>().SetWith(statsToDecrement).Execute();

                    //await _gameActionsProvider.Create<ShareDamageAndFearGameAction>().SetWith(ActiveInvestigator, ByThisCard, AmountDamage - harm.TotalDamageApply, AmountFear - harm.TotalFearApply).Execute();
                }
            }
        }
    }
}
