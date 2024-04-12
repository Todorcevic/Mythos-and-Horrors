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
        public HarmToInvestigatorGameAction(Investigator investigator, int amountDamage = 0, int amountFear = 0, bool isDirect = false, Card fromCard = null)
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
            if (IsDirect) await _gameActionsProvider.Create(new HarmToCardGameAction(Investigator.InvestigatorCard, AmountDamage, AmountFear));
            else await _gameActionsProvider.Create(new ShareDamageAndFearGameAction(Investigator, AmountDamage, AmountFear));
        }
    }
}
