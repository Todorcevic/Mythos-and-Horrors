using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01172 : CardCreature, IStalker
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly ChallengeTokensProvider _challengeTokensProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Monster, Tag.Nightgaunt };

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            CreateForceReaction<RevealChallengeTokenGameAction>(DoubleModifierCondition, DoubleModifierLogic, GameActionTime.After);
        }

        /*******************************************************************/
        private async Task DoubleModifierLogic(RevealChallengeTokenGameAction revealChallengeToken)
        {
            Func<Investigator, int> original = revealChallengeToken.ChallengeTokenRevealed.Value;
            await _gameActionsProvider.Create<UpdateChallengeTokenGameAction>()
                .SetWith(revealChallengeToken.ChallengeTokenRevealed, DoubleModifier, revealChallengeToken.ChallengeTokenRevealed.Effect)
                .Execute();

            CreateOneTimeReaction<RestoreChallengeTokenGameAction>(RestoreCondition, RestoreLogic, GameActionTime.After);

            /*******************************************************************/
            int DoubleModifier(Investigator investigator) => original.Invoke(investigator) * 2;

            bool RestoreCondition(RestoreChallengeTokenGameAction restoreChallengeToken)
            {
                if (restoreChallengeToken.ChallengeTokenToRestore != revealChallengeToken.ChallengeTokenRevealed) return false;
                return true;
            }

            async Task RestoreLogic(RestoreChallengeTokenGameAction restoreChallengeToken)
            {
                await _gameActionsProvider.Create<ResetChallengeTokenGameAction>().SetWith(restoreChallengeToken.ChallengeTokenToRestore).Execute();
            }
        }

        private bool DoubleModifierCondition(RevealChallengeTokenGameAction revealChallengeTokenGameAction)
        {
            if (_gameActionsProvider.CurrentChallenge is not EludeCreatureGameAction eludeCreatureGameAction) return false;
            if (eludeCreatureGameAction.CardToChallenge != this) return false;
            if (_challengeTokensProvider.ChallengeTokensInBag.Contains(revealChallengeTokenGameAction.ChallengeTokenRevealed)) return false;
            if (revealChallengeTokenGameAction.ChallengeTokenRevealed.Value.Invoke(eludeCreatureGameAction.ActiveInvestigator) >= 0) return false;
            return true;
        }
    }
}
