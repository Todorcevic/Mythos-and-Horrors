using System;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class PlayEffectGameAction : GameAction
    {
        public BaseEffect Effect { get; private set; }

        /*******************************************************************/
        public PlayEffectGameAction SetWith(BaseEffect effect)
        {
            Effect = effect ?? throw new ArgumentNullException("Effect cant be null");
            return this;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            if (Effect.ActivateTurnsCost.Value > 0)
            {
                await _gameActionsProvider.Create<DecrementStatGameAction>().SetWith(Effect.Investigator.CurrentTurns, Effect.ActivateTurnsCost.Value).Execute();
            }
            if (Effect is CardEffect cardEffec && cardEffec.ResourceCost.Value > 0)
            {
                await _gameActionsProvider.Create<DecrementStatGameAction>().SetWith(cardEffec.Investigator.Resources, cardEffec.ResourceCost.Value).Execute();
            }
            if (Effect.WithOpportunityAttack)
            {
                await _gameActionsProvider.Create<OpportunityAttackGameAction>().SetWith(Effect.Investigator).Execute();
            }
            await Effect.Logic();
        }
    }
}

