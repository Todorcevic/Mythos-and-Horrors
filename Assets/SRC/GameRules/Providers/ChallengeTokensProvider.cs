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

        private List<ChallengeTokenType> _tokens;

        /*******************************************************************/
        public void CreateTokens(List<ChallengeTokenType> tokens)
        {
            _tokens = tokens;
        }


        public ChallengeToken GetRandomToken()
        {
            int index = new Random().Next(_tokens.Count);
            return ResolveToken(_tokens[index]);
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
                _ => new ChallengeToken(token, () => (int)token, () => Task.CompletedTask),
            };
        }
    }
}
