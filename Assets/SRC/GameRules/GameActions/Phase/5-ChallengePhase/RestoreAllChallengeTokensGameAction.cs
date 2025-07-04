﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class RestoreAllChallengeTokensGameAction : GameAction
    {
        [Inject] private readonly ChallengeTokensProvider _challengeTokensProvider;

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionsProvider.Create<SafeForeach<ChallengeToken>>().SetWith(TokensRevealed, Restore).Execute();
        }

        /*******************************************************************/
        private IEnumerable<ChallengeToken> TokensRevealed() => _challengeTokensProvider.ChallengeTokensRevealed.AsEnumerable().Reverse();

        private async Task Restore(ChallengeToken challengeToken) =>
            await _gameActionsProvider.Create<RestoreChallengeTokenGameAction>().SetWith(challengeToken).Execute();
    }
}
