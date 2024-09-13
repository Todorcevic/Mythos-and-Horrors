using System;
using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class ChallengeTokensProvider
    {
        [Inject] private readonly ChaptersProvider _chaptersProvider;

        public List<ChallengeToken> ChallengeTokensInBag { get; private set; }
        public List<ChallengeToken> ChallengeTokensRevealed { get; private set; } = new();
        public IEnumerable<ChallengeToken> BasicChallengeTokensInBag => ChallengeTokensInBag.Where(token => ((int)token.TokenType) < 10);
        public IEnumerable<ChallengeToken> SpecialChallengeTokensInBag => ChallengeTokensInBag.Where(token => ((int)token.TokenType) >= 10);
        public IEnumerable<ChallengeToken> BasicChallengeTokensRevealed => ChallengeTokensRevealed.Where(token => ((int)token.TokenType) < 10);
        public IEnumerable<ChallengeToken> SpecialChallengeTokensRevealed => ChallengeTokensRevealed.Where(token => ((int)token.TokenType) >= 10);
        public IEnumerable<ChallengeToken> AllBasicChallengeTokens => BasicChallengeTokensInBag.Concat(BasicChallengeTokensRevealed);
        public IEnumerable<ChallengeToken> AllSpecialChallengeTokens => SpecialChallengeTokensInBag.Concat(SpecialChallengeTokensRevealed);

        /*******************************************************************/
        public void CreateTokens(List<ChallengeTokenType> tokens)
        {
            ChallengeTokensInBag = tokens.ConvertAll(ConvertToken);
        }

        public ChallengeToken GetRandomToken() => ChallengeTokensInBag[new Random().Next(ChallengeTokensInBag.Count)];

        public ChallengeToken GetSpecificToken(ChallengeTokenType tokenType) => ChallengeTokensInBag.Find(token => token.TokenType == tokenType);

        public void RestoreSingleToken(ChallengeToken token)
        {
            ChallengeTokensInBag.Add(token);
            if (!ChallengeTokensRevealed.Contains(token)) throw new InvalidOperationException("Token not exist");
            ChallengeTokensRevealed.Remove(token);
        }

        public void RevealToken(ChallengeToken token)
        {
            if (!ChallengeTokensInBag.Contains(token)) throw new InvalidOperationException("Token not exist");
            ChallengeTokensInBag.Remove(token);
            ChallengeTokensRevealed.Add(token);
        }

        private ChallengeToken ConvertToken(ChallengeTokenType token)
        {
            return token switch
            {
                ChallengeTokenType.Ancient => _chaptersProvider.CurrentScene.GetNewAncientToken(),
                ChallengeTokenType.Creature => _chaptersProvider.CurrentScene.GetNewCreatureToken(),
                ChallengeTokenType.Danger => _chaptersProvider.CurrentScene.GetNewDangerToken(),
                ChallengeTokenType.Cultist => _chaptersProvider.CurrentScene.GetNewCultistToken(),
                ChallengeTokenType.Fail => _chaptersProvider.CurrentScene.GetNewFailToken(),
                ChallengeTokenType.Star => _chaptersProvider.CurrentScene.GetNewStarToken(),
                _ => new ChallengeToken(token, (_) => (int)token)
            };
        }
    }
}
