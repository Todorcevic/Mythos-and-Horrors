using System;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class Card01115 : CardPlace
    {
        protected override async Task WhenBegin(GameAction gameAction)
        {
            await base.WhenBegin(gameAction);

            if (gameAction is InteractableGameAction interactableGameAction
                && interactableGameAction.Parent is OneInvestigatorTurnGameAction oneInvestigatorTurnGameAction)
            {
                Effect moveEffect = oneInvestigatorTurnGameAction.MoveEffects.Find(effect => effect.CardAffected == this);
                moveEffect?.ConcatNewCanPlay(CanMove);
            }
        }

        private bool CanMove()
        {
            if (!Revealed.IsActive) return false;
            return true;
        }
    }
}
