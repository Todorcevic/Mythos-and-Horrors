using MythsAndHorrors.GameRules;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class MultiEffectHandler
    {
        [Inject] private readonly CardViewGeneratorComponent _cardViewGeneratorComponent;
        [Inject] private readonly ShowSelectorComponent _showSelectorComponent;
        [Inject] private readonly ActivateCardViewsHandler _showCardHandler;
        [Inject] private readonly ClickHandler<CardView> _clickHandler;

        //private Dictionary<CardView, Effect> clonesCardViewDictionary;

        private List<CardView> clonesCardViewList;

        /*******************************************************************/
        public async Task<Effect> ShowMultiEffects(CardView cardViewWithMultiEffecs)
        {
            clonesCardViewList = CreateCardViewDictionary(cardViewWithMultiEffecs);
            await _showSelectorComponent.ShowMultiEffects(clonesCardViewList);
            _showCardHandler.ActiavateCardViewsPlayables(clonesCardViewList, withMainButton: true);

            return await FinishMultiEffect(await _clickHandler.WaitingClick());
        }

        private async Task<Effect> FinishMultiEffect(CardView cardViewSelected)
        {
            await _showCardHandler.DeactivateCardViewsPlayables(clonesCardViewList);
            Effect effectSelected = cardViewSelected == null ? null : cardViewSelected.UniqueEffect;

            clonesCardViewList.First().UniqueEffect = null;

            if (effectSelected == null) await _showSelectorComponent.ReturnClones();
            else await _showSelectorComponent.DestroyClones(cardViewSelected);
            return effectSelected;
        }

        private List<CardView> CreateCardViewDictionary(CardView originalCardView)
        {
            List<Effect> effects = originalCardView.Card.PlayableEffects.ToList();
            originalCardView.HideBuffsAndEffects();
            originalCardView.UniqueEffect = effects.First();
            List<CardView> newClonesCardView = new() { { originalCardView } };
            foreach (Effect effect in effects.Skip(1))
            {
                CardView cloneCardView = _cardViewGeneratorComponent.CloneCardView(originalCardView, originalCardView.CurrentZoneView.transform);
                cloneCardView.UniqueEffect = effect;
                newClonesCardView.Add(cloneCardView);
            }
            return newClonesCardView;
        }
    }
}
