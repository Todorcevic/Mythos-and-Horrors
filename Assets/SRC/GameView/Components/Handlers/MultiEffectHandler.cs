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

        private Dictionary<CardView, Effect> clonesCardViewDictionary;
        /*******************************************************************/
        public async Task<Effect> ShowMultiEffects(CardView cardViewWithMultiEffecs)
        {
            clonesCardViewDictionary = CreateCardViewDictionary(cardViewWithMultiEffecs);
            await _showSelectorComponent.ShowMultiEffects(clonesCardViewDictionary);
            _showCardHandler.ActiavateCardViewsPlayables(clonesCardViewDictionary.Keys.ToList(), withMainButton: true);

            return await FinishMultiEffect(await _clickHandler.WaitingClick());
        }

        private async Task<Effect> FinishMultiEffect(CardView cardViewSelected)
        {
            await _showCardHandler.DeactivateCardViewsPlayables(clonesCardViewDictionary.Keys.ToList());
            Effect effectSelected = cardViewSelected == null ? null : clonesCardViewDictionary[cardViewSelected];
            if (effectSelected == null) await _showSelectorComponent.ReturnClones();
            else await _showSelectorComponent.DestroyClones(cardViewSelected);
            return effectSelected;
        }

        private Dictionary<CardView, Effect> CreateCardViewDictionary(CardView originalCardView)
        {
            List<Effect> effects = originalCardView.Card.PlayableEffects.ToList();
            Dictionary<CardView, Effect> newClonesCardView = new() { { originalCardView, effects.First() } };
            foreach (Effect effect in effects.Skip(1))
            {
                CardView cloneCardView = _cardViewGeneratorComponent.Clone(originalCardView, originalCardView.CurrentZoneView.transform);
                newClonesCardView.Add(cloneCardView, effect);
            }
            return newClonesCardView;
        }
    }
}
