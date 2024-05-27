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
        public int StrengModifier { get; }

        /*******************************************************************/
        public PlayAttackGameAction(Investigator investigator, CardCreature cardCreature, int amountDamage = 1, int strengModifier = 0)
        {
            Investigator = investigator;
            CardCreature = cardCreature;
            AmountDamage = amountDamage;
            StrengModifier = strengModifier;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionsProvider.Create(new DecrementStatGameAction(Investigator.CurrentTurns, CardCreature.InvestigatorAttackTurnsCost.Value));
            await _gameActionsProvider.Create(new AttackGameAction(Investigator, CardCreature, AmountDamage, StrengModifier));
        }
    }
}
