using System;
using System.Collections.Generic;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class ChallengeTokensProvider
    {
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly ChaptersProvider chaptersProvider;

        private List<ChallengeTokenType> _tokens;

        /*******************************************************************/
        public void CreateTokens(List<ChallengeTokenType> tokens)
        {
            _tokens = tokens;
        }


        public ChallengeToken ResolveToken(ChallengeTokenType token)
        {
            switch (token)
            {
                case ChallengeTokenType.Ancient:
                    break;
                case ChallengeTokenType.Creature:
                    break;
                case ChallengeTokenType.Danger:
                    break;
                case ChallengeTokenType.Cultist:
                    break;
                case ChallengeTokenType.Fail:
                    break;
                case ChallengeTokenType.Star:
                    break;
                default:
                    break;
            }

            return new ChallengeToken();
        }
    }
}
