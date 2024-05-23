using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01157 : CardColosus, IStalker, IVictoriable
    {
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProvider;
        [Inject] private readonly CardsProvider _cardsProvider;

        public IEnumerable<Investigator> InvestigatorsVictoryAffected => _investigatorsProvider.AllInvestigators;
        int IVictoriable.Victory => 10;
        bool IVictoriable.IsVictoryComplete => Health.Value <= 0; //TODO: revisar el reseteo cuando se descarta
        public override IEnumerable<Tag> Tags => new[] { Tag.AncientOne, Tag.Elite };
        public CardSupply Lita => _cardsProvider.TryGetCard<Card01117>();

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            Health = CreateStat((Info.Health ?? 0) + _investigatorsProvider.AllInvestigators.Count * 4);
            CreateReaction<InvestigatorsPhaseGameAction>(ReadyCondition, ReadyLogic, false);
            CreateActivation(CreateStat(1), ThrowLitaActivate, ThrowLitaConditionToActivate);
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
            await _gameActionsProvider.Create(new FinalizeGameAction(_chaptersProvider.CurrentScene.FullResolutions[3]));
        }

        /*******************************************************************/
        private async Task ReadyLogic(InvestigatorsPhaseGameAction action)
        {
            await _gameActionsProvider.Create(new UpdateStatesGameAction(Exausted, false));
        }

        private bool ReadyCondition(InvestigatorsPhaseGameAction action)
        {
            if (!IsInPlay) return false;
            if (!Exausted.IsActive) return false;
            return true;
        }
    }
}
