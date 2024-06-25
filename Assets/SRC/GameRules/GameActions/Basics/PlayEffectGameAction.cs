using System;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{

    public class PlayEffectGameAction : GameAction
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public BaseEffect Effect { get; }

        /*******************************************************************/
        public PlayEffectGameAction(BaseEffect effect)
        {
            Effect = effect ?? throw new ArgumentNullException("Effect cant be null");
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            if (Effect.ActivateTurnsCost.Value > 0)
            {
                await _gameActionsProvider.Create(new DecrementStatGameAction(Effect.Investigator.CurrentTurns, Effect.ActivateTurnsCost.Value));
            }
            if (Effect is CardEffect cardEffec && cardEffec.ResourceCost.Value > 0)
            {
                await _gameActionsProvider.Create(new DecrementStatGameAction(cardEffec.Investigator.Resources, cardEffec.ResourceCost.Value));
            }
            if (Effect.WithOpportunityAttack)
            {
                await _gameActionsProvider.Create(new OpportunityAttackGameAction(Effect.Investigator));
            }
            await Effect.Logic();
        }
    }
}

