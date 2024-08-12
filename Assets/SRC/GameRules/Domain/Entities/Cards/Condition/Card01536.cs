using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01536 : CardConditionPlayFromHand
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        public override IEnumerable<Tag> Tags => new[] { Tag.Insight };
        public State Swaped { get; private set; }

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            Swaped = new State(false);
            CreateBuff(CardsToBuff, ActivationBuff, DeactivationBuff, code: "Buff_Card01536");
            CreateForceReaction<RoundGameAction>(RemovePlayedCondition, RemovePlayedLogic, GameActionTime.After);
            CreateOptativeReaction<ChallengePhaseGameAction>(ChangeChallengerCondition, ChangeChallengerReaction, GameActionTime.Before, "OptativeReaction_Card01536");
        }

        /*******************************************************************/
        private async Task ChangeChallengerReaction(ChallengePhaseGameAction challengePhaseGameAction)
        {
            challengePhaseGameAction.ChangeStat(challengePhaseGameAction.ActiveInvestigator.Intelligence);
            await Task.CompletedTask;
        }

        private bool ChangeChallengerCondition(ChallengePhaseGameAction challengePhaseGameAction)
        {
            if (!Swaped.IsActive) return false;
            if (challengePhaseGameAction.ActiveInvestigator != Owner) return false;
            if (challengePhaseGameAction.Stat != challengePhaseGameAction.ActiveInvestigator.Strength &&
                challengePhaseGameAction.Stat != challengePhaseGameAction.ActiveInvestigator.Agility) return false;
            return true;
        }

        /*******************************************************************/
        private async Task RemovePlayedLogic(RoundGameAction roundGameAction)
        {
            await _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(Swaped, false).Execute();
        }

        private bool RemovePlayedCondition(RoundGameAction roundGameAction)
        {
            if (!Swaped.IsActive) return false;
            return true;
        }

        /*******************************************************************/
        private async Task DeactivationBuff(IEnumerable<Card> enumerable) => await Task.CompletedTask;

        private async Task ActivationBuff(IEnumerable<Card> enumerable) => await Task.CompletedTask;

        private IEnumerable<Card> CardsToBuff() =>
            Swaped.IsActive ? new List<CardInvestigator>() { Owner.InvestigatorCard } : Enumerable.Empty<Card>();

        /*******************************************************************/
        protected override async Task ExecuteConditionEffect(GameAction gameAction, Investigator investigator) =>
            await _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(Swaped, true).Execute();

        protected override bool CanPlayFromHandSpecific(Investigator investigator) => true;
    }
}
