using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01148 : CardGoal, IVictoriable
    {
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProvider;

        public IEnumerable<Investigator> InvestigatorsVictoryAffected => _investigatorsProvider.AllInvestigators;
        int IVictoriable.Victory => 5;
        bool IVictoriable.IsVictoryComplete => Revealed.IsActive;

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Used by Injection")]
        private void Init()
        {
            RemoveStat(Hints);
            Hints = CreateStat(_investigatorsProvider.AllInvestigators.Count() * 2);
            PayHints.Disable();
            CreateActivation(1, PayHintsWithChallengeActivate, PayHintsWithChallengeConditionToActivate, PlayActionType.Activate);
        }

        /*******************************************************************/
        private bool PayHintsWithChallengeConditionToActivate(Investigator investigator)
        {
            if (!IsInPlay) return false;
            if (Revealed.IsActive) return false;
            if (investigator.Hints.Value < 1) return false;
            return true;
        }

        private async Task PayHintsWithChallengeActivate(Investigator investigator)
        {
            await _gameActionsProvider.Create(new ChallengePhaseGameAction(investigator.Power,
                 difficultValue: 3,
                "Pay Hint To Goal",
                cardToChallenge: this,
                succesEffect: SuccessEffect,
                failEffect: FailEffect));

            /*******************************************************************/
            async Task SuccessEffect()
            {
                await _gameActionsProvider.Create(new PayHintGameAction(investigator, Hints, 1));
            }

            async Task FailEffect()
            {
                await _gameActionsProvider.Create(new DropHintGameAction(investigator, investigator.CurrentPlace.Hints, 1));
            }
        }

        /*******************************************************************/
        protected override async Task CompleteEffect()
        {
            await _gameActionsProvider.Create(new MoveCardsGameAction(this, _chaptersProvider.CurrentScene.VictoryZone));
            await _gameActionsProvider.Create(new FinalizeGameAction(_chaptersProvider.CurrentScene.FullResolutions[1]));
        }
    }
}
