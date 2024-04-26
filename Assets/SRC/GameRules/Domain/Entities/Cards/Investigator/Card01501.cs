using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01501 : CardInvestigator
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        private bool _reactionUsed;

        public Reaction<DefeatCardGameAction> DiscoverHint { get; private set; }

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Zenject injects this method")]
        private void Init()
        {
            DiscoverHint = new Reaction<DefeatCardGameAction>(DiscoverHintCondition, DiscoverHintLogic);
        }
        /*******************************************************************/

        public override async Task StarEffect() => await Task.CompletedTask;

        public override int StarValue() => Owner.CurrentPlace.Hints.Value;


        /*******************************************************************/
        protected override async Task WhenFinish(GameAction gameAction)
        {
            await base.WhenFinish(gameAction);
            await DiscoverHint.Check(gameAction);
        }

        protected override async Task WhenBegin(GameAction gameAction)
        {
            await base.WhenBegin(gameAction);
            if (gameAction is RoundGameAction) _reactionUsed = false;
        }

        /*******************************************************************/
        private async Task DiscoverHintLogic(DefeatCardGameAction defeatCardGameAction)
        {
            _reactionUsed = true;
            await _gameActionsProvider.Create(new GainHintGameAction(Owner, Owner.CurrentPlace.Hints, 1));
        }

        private bool DiscoverHintCondition(DefeatCardGameAction defeatCardGameAction)
        {
            if (defeatCardGameAction.ByThisInvestigator != Owner) return false;
            if (!IsInPlay) return false;
            if (Owner.CurrentPlace.Hints.Value < 1) return false;
            if (_reactionUsed) return false;
            return true;
        }
    }
}
