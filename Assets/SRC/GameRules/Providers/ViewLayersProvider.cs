using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class ViewLayersProvider
    {
        [Inject] private readonly List<IPresenter> _allPresenters;
        [Inject] private readonly List<IInteractablePresenter> _allInteractablePresenters;

        /*******************************************************************/
        public async Task PlayAnimationWith(GameAction gameAction)
        {
            foreach (IPresenter presenter in _allPresenters)
            {
                await presenter.CheckGameAction(gameAction);
            }

            foreach (IInteractablePresenter presenter in _allInteractablePresenters)
            {
                await presenter.CheckGameAction(gameAction);
            }
        }

        public async Task<Effect> StartSelectionWith(InteractableGameAction interactableGameAction)
        {
            foreach (IInteractablePresenter presenter in _allInteractablePresenters)
            {
                return await presenter.CheckGameAction(interactableGameAction);
            }
            return default;
        }
    }
}
