using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01115 : CardPlace
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly CardsProvider _cardsProvider;

        private CardSupply Lita => _cardsProvider.GetCard<Card01117>();

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            CreateActivation(CreateStat(1), ResignActivate, ResignConditionToActivate, withOpportunityAttck: false);
            CreateActivation(CreateStat(1), ParleyActivate, ParleyConditionToActivate, withOpportunityAttck: false);
            CreateReaction<OneInvestigatorTurnGameAction>(AvoidMoveCondition, AvoidMoveLogic, isAtStart: true);
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
                    activeInvestigator.Intelligence, 4, "Parley with Lita", cardToChallenge: Lita, succesEffect: ParleySucceed, failEffect: null));

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
        private async Task AvoidMoveLogic(OneInvestigatorTurnGameAction oneInvestigatorTurnGameAction)
        {
            Effect moveEffect = oneInvestigatorTurnGameAction.MoveEffects.Find(effect => effect.Card == this);
            oneInvestigatorTurnGameAction.RemoveEffect(moveEffect);
            await Task.CompletedTask;
        }

        private bool AvoidMoveCondition(OneInvestigatorTurnGameAction oneInvestigatorTurnGameAction)
        {
            if (Revealed.IsActive) return false;
            return true;
        }
    }
}
