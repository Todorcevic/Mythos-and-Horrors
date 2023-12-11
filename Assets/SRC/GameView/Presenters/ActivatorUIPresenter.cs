using MythsAndHorrors.GameRules;
using Sirenix.Utilities;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class ActivatorUIPresenter : IUAActivator
    {
        private Card[] _cards;
        [Inject] private readonly IOActivatorComponent _ioActivatorComponent;
        [Inject] private readonly CardViewsManager _cardViewsManager;

        /*******************************************************************/
        public void HardActivate(Card[] cards)
        {
            _cards = cards;
            _ioActivatorComponent.ActivateSensor();

            cards?.ForEach(card => _cardViewsManager.Get(card).GlowView.SetGreenGlow());
        }

        public void HardActivate()
        {
            HardActivate(_cards);
        }

        public void HardDeactivate()
        {
            _ioActivatorComponent.DeactivateSensor();
            _cards?.ForEach(card => _cardViewsManager.Get(card).GlowView.Off());
        }
    }
}
