using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class PlayAttackGameAction : GameAction
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public Investigator Investigator { get; }
        public CardCreature CardCreature { get; }
        public int AmountDamage { get; }

        /*******************************************************************/
        public PlayAttackGameAction(Investigator investigator, CardCreature cardCreature, int amountDamage = 1)
        {
            Investigator = investigator;
            CardCreature = cardCreature;
            AmountDamage = amountDamage;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionsProvider.Create(new DecrementStatGameAction(Investigator.CurrentTurns, CardCreature.InvestigatorAttackTurnsCost.Value));
            await _gameActionsProvider.Create(new AttackGameAction(Investigator, CardCreature, AmountDamage));
        }
    }
}
