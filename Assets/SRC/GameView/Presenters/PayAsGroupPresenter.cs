using DG.Tweening;
using MythosAndHorrors.GameRules;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class PayAsGroupPresenter : IAsGroupPresenter
    {
        [Inject] private readonly ClickHandler<IPlayable> _clickHandler;
        [Inject] private readonly BasicShowSelectorComponent _basicShowSelectorComponent;
        [Inject] private readonly CardViewsManager _cardViewsManager;
        [Inject] private readonly MoveCardHandler _moveCardHandler;
        [Inject] private readonly MainButtonComponent _mainButtonComponent;
        [Inject] private readonly ActivatePlayablesHandler _showCardHandler;

        private PayHintsToGoalGameAction _payHintsToGoalGameAction;

        public int PayAsGroupValue { get; private set; }

        private List<AvatarCardView> _allAvatars;

        /*******************************************************************/
        public async Task<Dictionary<Card, int>> SelectWith(GameAction gamAction)
        {
            if (gamAction is not PayHintsToGoalGameAction payHintsToGoalGameAction) return default;
            _payHintsToGoalGameAction = payHintsToGoalGameAction;
            PayAsGroupValue = payHintsToGoalGameAction.CardGoal.Hints.Value;

            await DotweenExtension.WaitForMoveToZoneComplete();
            IEnumerable<CardAvatar> cardsToPay = payHintsToGoalGameAction.InvestigatorsToPay.Select(investigator => investigator.AvatarCard);
            IEnumerable<CardView> cardsToShow = _cardViewsManager.GetCardViews(cardsToPay);
            _allAvatars = cardsToShow.Cast<AvatarCardView>().ToList();
            UpdatePayAsGroup(0);
            _mainButtonComponent.MainButtonEffect = new Effect().SetLogic(() => null).SetDescription("Continue");
            await _basicShowSelectorComponent.ShowCards(cardsToShow.ToList(), $"Pay {PayAsGroupValue} Hints");
            _allAvatars.ForEach(cardView => cardView.ShowPayAsGroup());

            IPlayable playableChoose = await _clickHandler.WaitingClick();

            await _showCardHandler.DeactivatePlayables();
            Tween returnSequence = _moveCardHandler.MoveCardsToCurrentZones(cardsToShow.Select(cardView => cardView.Card), ease: Ease.OutSine);
            await _basicShowSelectorComponent.ShowDown(returnSequence, withActivation: false);

            return GetResult();
        }

        public void UpdatePayAsGroup(int increment)
        {
            PayAsGroupValue += increment;
            _basicShowSelectorComponent.ActivateTitle($"Pay {PayAsGroupValue} Hints");
            _allAvatars.ForEach(cardView => cardView.PayAsGroupController.Refresh());
            MainButtonActivator();
        }

        private void MainButtonActivator()
        {
            if (PayAsGroupValue <= 0)
            {
                _mainButtonComponent.MainButtonEffect = new Effect().SetLogic(null).SetDescription("Continue");
                _mainButtonComponent.ActivateToClick();
            }
            else if (_payHintsToGoalGameAction.ButtonCanUndo)
            {
                _mainButtonComponent.MainButtonEffect = new Effect().SetLogic(null).SetDescription("Cancel");
                _mainButtonComponent.ActivateToCancelClick();
            }
            else
            {
                _mainButtonComponent.MainButtonEffect = new Effect().SetLogic(null).SetDescription(string.Empty);
                _mainButtonComponent.DeactivateToClick();
            }
        }

        private Dictionary<Card, int> GetResult()
        {
            if (PayAsGroupValue <= 0) return _allAvatars.ToDictionary(avatar => avatar.Card, avatar => avatar.PayAsGroupController.CurrentValue);
            else return null;
        }
    }
}
