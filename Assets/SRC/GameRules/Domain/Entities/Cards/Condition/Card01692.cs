using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01692 : CardConditionTrigged
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Fortune, Tag.Blessed };
        protected override bool IsFast => true;
        protected override bool FastReactionAtStart => false;

        /*******************************************************************/
        protected override async Task ExecuteConditionEffect(GameAction gameAction, Investigator investigator)
        {
            if (gameAction is not RevealChallengeTokenGameAction revealChallengeTokenGameAction) return;
            await _gameActionsProvider.Create(new RestoreChallengeTokenGameAction(revealChallengeTokenGameAction.ChallengeTokenRevealed));
            await _gameActionsProvider.Create(new RevealChallengeTokenGameAction(_chaptersProvider.CurrentScene.StarToken, investigator));
        }

        protected override bool CanPlayFromHandSpecific(GameAction gameAction)
        {
            if (gameAction is not RevealChallengeTokenGameAction revealChallengeTokenGameAction) return false;
            if (revealChallengeTokenGameAction.Investigator != ControlOwner) return false;
            if (revealChallengeTokenGameAction.ChallengeTokenRevealed.TokenType != ChallengeTokenType.Fail) return false;
            return true;
        }
    }
}
