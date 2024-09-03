using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01692 : CardConditionFast
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Fortune, Tag.Blessed };
        protected override GameActionTime FastReactionAtStart => GameActionTime.After;
        protected override Localization Localization => new("OptativeReaction_Card01692");

        /*******************************************************************/
        protected override async Task ExecuteConditionEffect(GameAction gameAction, Investigator investigator)
        {
            if (gameAction is not RevealChallengeTokenGameAction revealChallengeTokenGameAction) return;
            await _gameActionsProvider.Create<RestoreChallengeTokenGameAction>().SetWith(revealChallengeTokenGameAction.ChallengeTokenRevealed).Execute();
            await _gameActionsProvider.Create<RevealChallengeTokenGameAction>().SetWith(_chaptersProvider.CurrentScene.StarToken, investigator).Execute();
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
