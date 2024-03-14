using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01115 : CardPlace
    {
        [Inject] private readonly EffectsProvider _effectsProvider;

        /*******************************************************************/
        protected override async Task WhenBegin(GameAction gameAction)
        {
            await base.WhenBegin(gameAction);

            if (gameAction is InteractableGameAction interactableGameAction
                && interactableGameAction.Parent is OneInvestigatorTurnGameAction oneInvestigatorTurnGameAction)
            {
                Effect moveEffect = oneInvestigatorTurnGameAction.MoveEffects.Find(effect => effect.CardAffected == this);
                if (!CanMove()) _effectsProvider.RemoveEffect(moveEffect);
            }
        }

        private bool CanMove()
        {
            if (!Revealed.IsActive) return false;
            return true;
        }
    }
}
