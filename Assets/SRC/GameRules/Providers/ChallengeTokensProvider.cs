using System;
using System.Collections.Generic;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class ChallengeTokensProvider
    {
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProvider;

        public List<ChallengeToken> ChallengeTokensInBag { get; private set; }
        public List<ChallengeToken> ChallengeTokensRevealed { get; private set; } = new();

        /*******************************************************************/
        public void CreateTokens(List<ChallengeTokenType> tokens)
        {
            ChallengeTokensInBag = tokens.ConvertAll(ConvertToken);
        }

        public ChallengeToken GetRandomToken() => ChallengeTokensInBag[new Random().Next(ChallengeTokensInBag.Count)];

        public void RestoreSingleToken(ChallengeToken token)
        {
            ChallengeTokensInBag.Add(token);
            ChallengeTokensRevealed.Remove(token);
        }

        public void RevealToken(ChallengeToken token)
        {
            ChallengeTokensInBag.Remove(token);
            ChallengeTokensRevealed.Add(token);
        }

        private ChallengeToken ConvertToken(ChallengeTokenType token)
        {
            return token switch
            {
                ChallengeTokenType.Ancient => _chaptersProvider.CurrentScene.AncientToken,
                ChallengeTokenType.Creature => _chaptersProvider.CurrentScene.CreatureToken,
                ChallengeTokenType.Danger => _chaptersProvider.CurrentScene.DangerToken,
                ChallengeTokenType.Cultist => _chaptersProvider.CurrentScene.CultistToken,
                ChallengeTokenType.Fail => _chaptersProvider.CurrentScene.FailToken,
                ChallengeTokenType.Star => _chaptersProvider.CurrentScene.StarToken,
                _ => new ChallengeToken(token, () => (int)token)
            };
        }
    }
}
