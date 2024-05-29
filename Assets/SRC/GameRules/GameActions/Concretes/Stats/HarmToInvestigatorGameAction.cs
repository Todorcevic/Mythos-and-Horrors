using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class HarmToInvestigatorGameAction : GameAction
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public Investigator Investigator { get; }
        public Card FromCard { get; }
        public int AmountDamage { get; private set; }
        public int AmountFear { get; private set; }
        public bool IsDirect { get; }

        public override bool CanBeExecuted => AmountDamage > 0 || AmountFear > 0;

        /*******************************************************************/
        public HarmToInvestigatorGameAction(Investigator investigator, Card fromCard, int amountDamage = 0, int amountFear = 0, bool isDirect = false)
        {
            Investigator = investigator;
            FromCard = fromCard;
            AmountDamage = amountDamage;
            AmountFear = amountFear;
            IsDirect = isDirect;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            List<Card> allSelectables = new();
            if (AmountDamage > 0)
                allSelectables.AddRange(Investigator.AidZone.Cards.OfType<IDamageable>().Cast<Card>().Except(allSelectables));
            if (AmountFear > 0)
                allSelectables.AddRange(Investigator.AidZone.Cards.OfType<IFearable>().Cast<Card>().Except(allSelectables));

            if (IsDirect || !allSelectables.Any()) await _gameActionsProvider.Create(new HarmToCardGameAction(Investigator.InvestigatorCard, FromCard, AmountDamage, AmountFear));
            else await _gameActionsProvider.Create(new ShareDamageAndFearGameAction(Investigator, FromCard, AmountDamage, AmountFear));
        }
    }
}
