using System;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class PayRequerimentsEffectGameAction : GameAction
    {
        public BaseEffect Effect { get; private set; }

        /*******************************************************************/
        public PayRequerimentsEffectGameAction SetWith(BaseEffect effect)
        {
            Effect = effect ?? throw new ArgumentNullException("Effect cant be null");
            return this;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            if (!Effect.IsFreeActivation)
            {
                await _gameActionsProvider.Create<DecrementStatGameAction>().SetWith(Effect.Investigator.CurrentActions, Effect.ActivateTurnsCost.Value).Execute();
            }
            if (Effect is CardEffect cardEffec && cardEffec.ResourceCost.Value > 0)
            {
                await _gameActionsProvider.Create<PayResourceGameAction>().SetWith(cardEffec.Investigator, cardEffec.ResourceCost.Value).Execute();
            }
            if (Effect.WithOpportunityAttack)
            {
                await _gameActionsProvider.Create<OpportunityAttackGameAction>().SetWith(Effect.Investigator).Execute();
            }
        }
    }
}

