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
            CreateOptativeReaction<PlayEffectGameAction>(DrawCondition, DrawLogic, GameActionTime.After, new Localization("OptativeReaction_Card01512"));
        }

        /*******************************************************************/
        private async Task DrawLogic(PlayEffectGameAction playEffectGameAction)
        {
            await _gameActionsProvider.Create<DrawAidGameAction>().SetWith(ControlOwner).Execute();
        }

        private bool DrawCondition(PlayEffectGameAction playEffectGameAction)
        {
            if (IsInPlay.IsFalse) return false;
            if (playEffectGameAction.Effect is not CardEffect cardEffect) return false;
            if (playEffectGameAction.Effect.Investigator != ControlOwner) return false;
            if (!playEffectGameAction.Effect.IsThatActionType(PlayActionType.PlayFromHand)) return false;
            if (!cardEffect.CardOwner.HasThisTag(Tag.Spell)) return false;
            return true;
        }
    }
}
