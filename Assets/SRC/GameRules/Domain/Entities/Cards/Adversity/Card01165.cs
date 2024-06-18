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
            CreateForceReaction<RoundGameAction>(DiscardCondition, DiscardLogic, GameActionTime.After);
            CreateForceReaction<InteractableGameAction>(CantPlayCondition, CantPlayLogic, GameActionTime.Before);
        }

        /*******************************************************************/
        public override sealed Zone ZoneToMoveWhenDraw(Investigator investigator) => investigator.DangerZone;

        public override async Task PlayRevelationFor(Investigator investigator) => await Task.CompletedTask;

        /*******************************************************************/
        private async Task CantPlayLogic(InteractableGameAction interactableGameAction)
        {
            IEnumerable<CardEffect> effectesToRemove = interactableGameAction.AllEffects.Where(effect => ControlOwner.HandZone.Cards
                  .Where(card => card is CardSupply || card is CardCondition).Contains(effect.CardOwner));

            interactableGameAction.RemoveEffects(effectesToRemove);
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
    }
}
