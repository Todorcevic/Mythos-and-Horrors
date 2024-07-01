using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
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
            CreateActivation(1, ResignActivate, ResignConditionToActivate, PlayActionType.Resign);
            CreateActivation(1, ParleyActivate, ParleyConditionToActivate, PlayActionType.Parley);
            CreateForceReaction<InteractableGameAction>(AvoidMoveCondition, AvoidMoveLogic, GameActionTime.Before);
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

            async Task ParleySucceed() => await _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(Lita, activeInvestigator.AidZone).Start();
        }

        private bool ParleyConditionToActivate(Investigator activeInvestigator)
        {
            if (!IsInPlay) return false;
            if (activeInvestigator.CurrentPlace != this) return false;
            if (Lita.CurrentZone != OwnZone) return false;
            return true;
        }

        /*******************************************************************/
        private async Task AvoidMoveLogic(InteractableGameAction interactableGameAction)
        {
            IEnumerable<CardEffect> moveEffects = interactableGameAction.AllEffects
                .Where(effects => effects.IsActionType(PlayActionType.Move) && (effects.CardOwner == this || effects.CardAffected == this));
            interactableGameAction.RemoveEffects(moveEffects);
            await Task.CompletedTask;
        }

        private bool AvoidMoveCondition(InteractableGameAction oneInvestigatorTurnGameAction)
        {
            if (Revealed.IsActive) return false;
            return true;
        }
    }
}
