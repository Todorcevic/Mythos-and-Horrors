using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01157 : CardColosus, IStalker, IVictoriable, IResetable
    {
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProvider;
        [Inject] private readonly CardsProvider _cardsProvider;

        public IEnumerable<Investigator> InvestigatorsVictoryAffected => _investigatorsProvider.AllInvestigators;
        int IVictoriable.Victory => 10;
        bool IVictoriable.IsVictoryComplete => Defeated.IsActive;
        public override IEnumerable<Tag> Tags => new[] { Tag.AncientOne, Tag.Elite };
        public CardSupply Lita => _cardsProvider.TryGetCard<Card01117>();
        public State Defeated { get; private set; }

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            RemoveStat(Health);
            Health = CreateStat((Info.Health ?? 0) + _investigatorsProvider.AllInvestigators.Count() * 4);
            Defeated = CreateState(false);
            CreateForceReaction<InvestigatorsPhaseGameAction>(ReadyCondition, ReadyLogic, GameActionTime.After);
            CreateActivation(1, ThrowLitaActivate, ThrowLitaConditionToActivate, PlayActionType.Activate, "Activation_Card01157", localizableArgs: Lita.Info.Name);
        }

        /*******************************************************************/
        private bool ThrowLitaConditionToActivate(Investigator investigator)
        {
            if (!IsInPlay) return false;
            if (!Lita?.IsInPlay ?? true) return false;
            if (Lita?.ControlOwner.CurrentPlace != CurrentPlace) return false;
            if (CurrentPlace != investigator.CurrentPlace) return false;
            return true;
        }

        private async Task ThrowLitaActivate(Investigator investigator)
        {
            await _gameActionsProvider.Create<FinalizeGameAction>().SetWith(_chaptersProvider.CurrentScene.FullResolutions[3]).Execute();
        }

        /*******************************************************************/
        private async Task ReadyLogic(InvestigatorsPhaseGameAction action)
        {
            await _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(Exausted, false).Execute();
        }

        private bool ReadyCondition(InvestigatorsPhaseGameAction action)
        {
            if (!IsInPlay) return false;
            if (!Exausted.IsActive) return false;
            return true;
        }

        public async Task Reset()
        {
            if (HealthLeft < 1) await _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(Defeated, true).Execute();
        }
    }
}
