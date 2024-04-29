using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01115 : CardPlace, IActivable
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly CardsProvider _cardsProvider;

        public List<Activation> Activations { get; private set; }
        private CardSupply Lita => _cardsProvider.GetCard<Card01117>();

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            Activations = new()
            {
                new(CreateStat(1), ResignActivate, ResignConditionToActivate),
                new(CreateStat(1), ParleyActivate, ParleyConditionToActivate)
            };
        }

        /*******************************************************************/
        private async Task ResignActivate(Investigator activeInvestigator)
        {
            await _gameActionsProvider.Create(new ResignGameAction(activeInvestigator));
        }

        private bool ResignConditionToActivate(Investigator activeInvestigator)
        {
            if (!IsInPlay) return false;
            if (activeInvestigator.CurrentPlace != this) return false;
            return true;
        }

        private async Task ParleyActivate(Investigator activeInvestigator)
        {
            await _gameActionsProvider.Create(new ParleyGameAction(TakeLita));

            /*******************************************************************/
            async Task TakeLita() => await _gameActionsProvider.Create(new ChallengePhaseGameAction(
                    activeInvestigator.Intelligence, 4, "Parley with Lita", ParleySucceed, null, Lita));

            async Task ParleySucceed() => await _gameActionsProvider.Create(new MoveCardsGameAction(Lita, activeInvestigator.AidZone));
        }

        private bool ParleyConditionToActivate(Investigator activeInvestigator)
        {
            if (!IsInPlay) return false;
            if (activeInvestigator.CurrentPlace != this) return false;
            if (Lita.CurrentZone != OwnZone) return false;
            return true;
        }

        /*******************************************************************/
        protected override async Task WhenBegin(GameAction gameAction)
        {
            await base.WhenBegin(gameAction);

            if (gameAction is OneInvestigatorTurnGameAction oneInvestigatorTurnGameAction)
            {
                Effect moveEffect = oneInvestigatorTurnGameAction.MoveEffects.Find(effect => effect.Card == this);
                if (!CanMove()) oneInvestigatorTurnGameAction.RemoveEffect(moveEffect);
            }
        }
        /*******************************************************************/
        private bool CanMove()
        {
            if (!Revealed.IsActive) return false;
            return true;
        }
    }
}
