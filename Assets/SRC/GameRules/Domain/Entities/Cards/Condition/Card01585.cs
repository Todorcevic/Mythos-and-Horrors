using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01585 : CardConditionPlayFromHand
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Spirit };
        public State Played { get; private set; }

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            Played = new State(false);
            CreateBuff(CardsToBuff, ActivationBuff, DeactivationBuff);
            CreateForceReaction<PlayInvestigatorGameAction>(RemovePlayedCondition, RemovePlayedLogic, GameActionTime.After);
            CreateForceReaction<RevealChallengeTokenGameAction>(RevealTokenCondition, RevealTokenReaction, GameActionTime.Before);
        }

        /*******************************************************************/
        private async Task RevealTokenReaction(RevealChallengeTokenGameAction revealChallengeTaokenGameAction)
        {
            revealChallengeTaokenGameAction.Cancel();
            await Task.CompletedTask;
        }

        private bool RevealTokenCondition(RevealChallengeTokenGameAction revealChallengeTaokenGameAction)
        {
            if (!Played.IsActive) return false;
            if (revealChallengeTaokenGameAction.Investigator != Owner) return false;
            return true;
        }

        /*******************************************************************/
        private async Task RemovePlayedLogic(PlayInvestigatorGameAction action)
        {
            await _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(Played, false).Start();
        }

        private bool RemovePlayedCondition(PlayInvestigatorGameAction action)
        {
            if (!Played.IsActive) return false;
            return true;
        }

        /*******************************************************************/
        private async Task DeactivationBuff(IEnumerable<Card> enumerable) => await Task.CompletedTask;

        private async Task ActivationBuff(IEnumerable<Card> enumerable) => await Task.CompletedTask;

        private IEnumerable<Card> CardsToBuff() => Played.IsActive ? new List<CardInvestigator>() { Owner.InvestigatorCard } : Enumerable.Empty<Card>();

        /*******************************************************************/
        protected override bool CanPlayFromHandSpecific(Investigator investigator) => true;

        protected override async Task ExecuteConditionEffect(GameAction gameAction, Investigator investigator) =>
            await _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(Played, true).Start();
    }
}
