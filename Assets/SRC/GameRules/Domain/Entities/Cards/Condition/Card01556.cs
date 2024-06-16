using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01556 : CardConditionTrigged
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly ChallengeTokensProvider _challengeTokensProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Fortune, Tag.Insight };
        protected override bool IsFast => true;
        protected override bool FastReactionAtStart => false;

        /*******************************************************************/
        protected override bool CanPlayFromHandSpecific(GameAction gameAction)
        {
            if (gameAction is not RevealChallengeTokenGameAction revealChallengeToken) return false;
            if (revealChallengeToken.Investigator != ControlOwner) return false;
            if (revealChallengeToken.ChallengeTokenRevealed.TokenType >= 0) return false;
            if (!_challengeTokensProvider.ChallengeTokensRevealed.Contains(revealChallengeToken.ChallengeTokenRevealed)) return false;
            return true;
        }

        protected override async Task ExecuteConditionEffect(GameAction gameAction, Investigator investigator)
        {
            if (gameAction is not RevealChallengeTokenGameAction revealChallengeToken) return;
            ChallengeToken tokenUpdated = revealChallengeToken.ChallengeTokenRevealed;
            await _gameActionsProvider.Create(new UpdateChallengeTokenGameAction(tokenUpdated, (_) => Math.Abs((int)tokenUpdated.TokenType), tokenUpdated.Effect));
            CreateOneTimeReaction<RestoreChallengeTokenGameAction>(RestoreCondition, RestoreLogic, isAtStart: false);

            /*******************************************************************/
            bool RestoreCondition(RestoreChallengeTokenGameAction restoreChallengeTokenGameAction)
            {
                if (restoreChallengeTokenGameAction.ChallengeTokenToRestore != tokenUpdated) return false;
                return true;
            }

            async Task RestoreLogic(RestoreChallengeTokenGameAction restoreChallengeTokenGameAction)
            {
                await _gameActionsProvider.Create(new ResetChallengeTokenGameAction(tokenUpdated));
            }
        }
    }
}
