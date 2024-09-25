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
            RemoveStat(Keys);
            Keys = CreateStat(_investigatorsProvider.AllInvestigators.Count() * 2);
            PayKeys.Disable();
            CreateActivation(1, PayKeysWithChallengeActivate, PayKeysWithChallengeConditionToActivate, PlayActionType.Activate, new Localization("Activation_Card01148"));
        }

        /*******************************************************************/
        private bool PayKeysWithChallengeConditionToActivate(Investigator investigator)
        {
            if (IsInPlay.IsFalse) return false;
            if (Revealed.IsActive) return false;
            if (investigator.CanPayKeys.IsFalse) return false;
            return true;
        }

        private async Task PayKeysWithChallengeActivate(Investigator investigator)
        {
            await _gameActionsProvider.Create<ChallengePhaseGameAction>()
                .SetWith(investigator.Power, difficultValue: 3, new Localization("Challenge_Card01148", CurrentName), cardToChallenge: this, succesEffect: SuccessEffect, failEffect: FailEffect)
                .Execute();

            /*******************************************************************/
            async Task SuccessEffect() => await _gameActionsProvider.Create<PayKeyGameAction>().SetWith(investigator, Keys, 1).Execute();
            async Task FailEffect() => await _gameActionsProvider.Create<DropKeyGameAction>().SetWith(investigator, investigator.CurrentPlace.Keys, 1).Execute();
        }

        /*******************************************************************/
        protected override async Task CompleteEffect()
        {
            await _gameActionsProvider.Create<MoveCardsGameAction>()
                .SetWith(this, _chaptersProvider.CurrentScene.VictoryZone).Execute();
            await _gameActionsProvider.Create<FinalizeGameAction>().SetWith(_chaptersProvider.CurrentScene.FullResolutions[1]).Execute();
        }
    }
}
