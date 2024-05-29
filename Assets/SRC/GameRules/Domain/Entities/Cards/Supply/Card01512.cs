using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01512 : CardSupply
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Item, Tag.Relic };

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            CreateReaction<PlayFromHandGameAction>(DrawCondition, DrawLogic, false, isOptative: true);
        }

        /*******************************************************************/
        private async Task DrawLogic(PlayFromHandGameAction action)
        {
            await _gameActionsProvider.Create(new DrawAidGameAction(ControlOwner));
        }

        private bool DrawCondition(PlayFromHandGameAction playFromHandGameAction)
        {
            if (!IsInPlay) return false;
            if (playFromHandGameAction.Investigator != ControlOwner) return false;
            if (!((Card)playFromHandGameAction.PlayableFromHandCard).HasThisTag(Tag.Spell)) return false;
            return true;
        }
    }
}
