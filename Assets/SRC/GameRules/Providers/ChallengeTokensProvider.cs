using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class ChallengeTokensProvider
    {
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProvider;

        public List<ChallengeTokenType> TokensRevealed { get; private set; } = new();
        public List<ChallengeTokenType> Tokens { get; private set; }
        public List<ChallengeToken> ChallengeTokens => Tokens.ConvertAll(ResolveToken);

        /*******************************************************************/
        public void CreateTokens(List<ChallengeTokenType> tokens)
        {
            Tokens = tokens;
        }

        public ChallengeToken GetRandomToken()
        {
            int index = new Random().Next(Tokens.Count);
            ChallengeTokenType tokenType = Tokens[index];
            TokensRevealed.Add(tokenType);
            Tokens.Remove(tokenType);
            return ResolveToken(tokenType);
        }

        public void RestoreSingleToken(ChallengeTokenType token)
        {
            Tokens.Add(token);
            TokensRevealed.Remove(token);
        }

        public void RestoreTokens()
        {
            Tokens.AddRange(TokensRevealed);
            TokensRevealed.Clear();
        }

        private ChallengeToken ResolveToken(ChallengeTokenType token)
        {
            return token switch
            {
                ChallengeTokenType.Ancient => _chaptersProvider.CurrentScene.AncientToken,
                ChallengeTokenType.Creature => _chaptersProvider.CurrentScene.CreatureToken,
                ChallengeTokenType.Danger => _chaptersProvider.CurrentScene.DangerToken,
                ChallengeTokenType.Cultist => _chaptersProvider.CurrentScene.CultistToken,
                ChallengeTokenType.Fail => _chaptersProvider.CurrentScene.FailToken,
                ChallengeTokenType.Star => _investigatorsProvider.ActiveInvestigator.InvestigatorCard.StarToken,
                _ => new ChallengeToken(token, () => (int)token)
            };
        }
    }
}
