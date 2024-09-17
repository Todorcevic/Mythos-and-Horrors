using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01556 : CardConditionReaction
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly ChallengeTokensProvider _challengeTokensProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Fortune, Tag.Insight };
        protected override GameActionTime FastReactionAtStart => GameActionTime.After;
        protected override Localization Localization => new("OptativeReaction_Card01556");

        /*******************************************************************/
        protected override bool CanPlayFromHandSpecific(GameAction gameAction)
        {
            if (gameAction is not RevealChallengeTokenGameAction revealChallengeToken) return false;
            if (revealChallengeToken.Investigator != ControlOwner) return false;
            if (revealChallengeToken.ChallengeTokenRevealed.Value.Invoke(revealChallengeToken.Investigator) >= 0) return false;
            if (!_challengeTokensProvider.ChallengeTokensRevealed.Contains(revealChallengeToken.ChallengeTokenRevealed)) return false;
            return true;
        }

        protected override async Task ExecuteConditionEffect(GameAction gameAction, Investigator investigator)
        {
            if (gameAction is not RevealChallengeTokenGameAction revealChallengeToken) return;
            ChallengeToken tokenUpdated = revealChallengeToken.ChallengeTokenRevealed;
            Func<Investigator, int> originalTokenValue = tokenUpdated.Value;
            await _gameActionsProvider.Create<UpdateChallengeTokenGameAction>()
                .SetWith(tokenUpdated, (_) => Math.Abs(originalTokenValue.Invoke(revealChallengeToken.Investigator)), tokenUpdated.Effect).Execute();
            CreateOneTimeReaction<RestoreChallengeTokenGameAction>(RestoreCondition, RestoreLogic, GameActionTime.After);

            /*******************************************************************/
            bool RestoreCondition(RestoreChallengeTokenGameAction restoreChallengeTokenGameAction)
            {
                if (restoreChallengeTokenGameAction.ChallengeTokenToRestore != tokenUpdated) return false;
                return true;
            }

            async Task RestoreLogic(RestoreChallengeTokenGameAction restoreChallengeTokenGameAction)
            {
                await _gameActionsProvider.Create<ResetChallengeTokenGameAction>().SetWith(tokenUpdated).Execute();
            }
        }
    }
}
