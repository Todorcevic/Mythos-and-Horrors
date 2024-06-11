using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01556 : CardConditionTrigged
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        private RevealChallengeTokenGameAction _revealChallengeToken;

        public override IEnumerable<Tag> Tags => new[] { Tag.Fortune, Tag.Insight };
        protected override bool IsFast => true;
        protected override bool FastReactionAtStart => false;

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            CreateReaction<RestoreChallengeTokenGameAction>(RestoreCondition, RestoreLogic, isAtStart: false);
        }

        /*******************************************************************/
        private async Task RestoreLogic(RestoreChallengeTokenGameAction restoreChallengeTokenGameAction)
        {
            await _gameActionsProvider.Create(new UpdateChallengeTokenGameAction(_revealChallengeToken.ChallengeTokenRevealed)); // Restore the token
            _revealChallengeToken = null;
        }

        private bool RestoreCondition(RestoreChallengeTokenGameAction restoreChallengeTokenGameAction)
        {
            if (restoreChallengeTokenGameAction.ChallengeTokenToRestore != _revealChallengeToken?.ChallengeTokenRevealed) return false;
            return true;
        }

        /*******************************************************************/
        protected override async Task ExecuteConditionEffect(Investigator investigator)
        {
            ChallengeToken token = _revealChallengeToken.ChallengeTokenRevealed;
            await _gameActionsProvider.Create(new UpdateChallengeTokenGameAction(token, (_) => Math.Abs((int)token.TokenType), token.Effect));
        }

        protected override bool CanPlayFromHandSpecific(GameAction gameAction)
        {
            if (gameAction is not RevealChallengeTokenGameAction revealChallengeToken) return false;
            if (revealChallengeToken.Investigator != ControlOwner) return false;
            if (revealChallengeToken.ChallengeTokenRevealed.TokenType >= 0) return false;
            _revealChallengeToken = revealChallengeToken;
            return true;
        }

    }
}
