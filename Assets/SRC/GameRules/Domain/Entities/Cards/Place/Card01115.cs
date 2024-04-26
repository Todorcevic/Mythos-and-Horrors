using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class Card01115 : CardPlace
    {
        protected override async Task WhenBegin(GameAction gameAction)
        {
            await base.WhenBegin(gameAction);

            if (gameAction is OneInvestigatorTurnGameAction oneInvestigatorTurnGameAction)
            {
                Effect moveEffect = oneInvestigatorTurnGameAction.MoveEffects.Find(effect => effect.Card == this);
                if (!CanMove()) oneInvestigatorTurnGameAction.RemoveEffect(moveEffect);
            }
        }

        private bool CanMove()
        {
            if (!Revealed.IsActive) return false;
            return true;
        }
    }
}
