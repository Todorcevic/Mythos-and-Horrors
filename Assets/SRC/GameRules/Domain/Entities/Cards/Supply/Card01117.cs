using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01117 : CardSupply
    {
        [Inject] private readonly CardsProvider _cardsProvider;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Ally };
        public CardCreature Urmodoth => _cardsProvider.TryGetCard<Card01157>();

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            CreateActivation(CreateStat(1), ParleyActivate, ParleyConditionToActivate, withOpportunityAttck: false);
            CreateActivation(CreateStat(1), ThrowLitaActivate, ThrowLitaConditionToActivate);
        }

        /*******************************************************************/
        private bool ThrowLitaConditionToActivate(Investigator investigator)
        {
            if (_chaptersProvider.CurrentScene is not SceneCORE3) return false;
            if (!IsInPlay) return false;
            if (!Urmodoth?.IsInPlay ?? true) return false;
            if (ControlOwner.CurrentPlace != Urmodoth.CurrentPlace) return false;
            if (investigator.CurrentPlace != Urmodoth.CurrentPlace) return false;
            return true;
        }

        private async Task ThrowLitaActivate(Investigator investigator)
        {
            await _gameActionsProvider.Create(new FinalizeGameAction(_chaptersProvider.CurrentScene.Resolutions[3]));
        }

        /*******************************************************************/
        private async Task ParleyActivate(Investigator activeInvestigator)
        {
            await _gameActionsProvider.Create(new ParleyGameAction(TakeLita));

            /*******************************************************************/
            async Task TakeLita() => await _gameActionsProvider.Create(new ChallengePhaseGameAction(
                    activeInvestigator.Intelligence, 4, "Parley with Lita", ParleySucceed, null, this));

            async Task ParleySucceed() => await _gameActionsProvider.Create(new MoveCardsGameAction(this, activeInvestigator.AidZone));
        }

        private bool ParleyConditionToActivate(Investigator activeInvestigator)
        {
            if (_chaptersProvider.CurrentScene is not SceneCORE1) return false;
            if (!IsInPlay) return false;
            if (activeInvestigator.AvatarCard.CurrentZone != CurrentZone) return false;
            if (Owner != null) return false;
            return true;
        }
    }
}
