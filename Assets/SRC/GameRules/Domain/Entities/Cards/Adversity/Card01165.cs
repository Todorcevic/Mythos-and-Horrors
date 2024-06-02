using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01165 : CardAdversity
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Terror };

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            CreateReaction<RoundGameAction>(DiscardCondition, DiscardLogic, isAtStart: false);
            CreateReaction<InteractableGameAction>(CantPlayCondition, CantPlayLogicLogic, isAtStart: true);
        }

        private async Task CantPlayLogicLogic(InteractableGameAction interactableGameAction)
        {
            interactableGameAction.AllEffects.Where(effect => ControlOwner.HandZone.Cards
                .Where(card => card is CardSupply || card is CardCondition).Contains(effect.Card))
                .ForEach(effect => interactableGameAction.RemoveEffect(effect));
            await Task.CompletedTask;
        }

        private bool CantPlayCondition(InteractableGameAction interactableGameAction)
        {
            if (!IsInPlay) return false;
            return true;
        }

        /*******************************************************************/
        private async Task DiscardLogic(RoundGameAction action)
        {
            await _gameActionsProvider.Create(new DiscardGameAction(this));
        }

        private bool DiscardCondition(RoundGameAction action)
        {
            if (!IsInPlay) return false;
            return true;
        }

        /*******************************************************************/

    }
}
