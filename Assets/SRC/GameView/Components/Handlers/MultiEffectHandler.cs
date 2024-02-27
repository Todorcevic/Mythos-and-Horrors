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
        [Inject] private readonly ActivatePlayablesHandler _showCardHandler;
        [Inject] private readonly ClickHandler<IPlayable> _clickHandler;
        private List<CardView> cardViewClones;

        /*******************************************************************/
        public async Task<Effect> ShowMultiEffects(CardView cardViewWithMultiEffecs)
        {
            cardViewClones = CreateCardViewClones(cardViewWithMultiEffecs);
            await _showSelectorComponent.ShowMultiEffects(cardViewClones);
            _showCardHandler.ActiavatePlayables(withMainButton: true, cardViewClones);

            return await FinishMultiEffect(await _clickHandler.WaitingClick() as CardView);
        }

        private async Task<Effect> FinishMultiEffect(CardView cardViewSelected)
        {
            await _showCardHandler.DeactivatePlayables();
            Effect effectSelected = cardViewSelected == null ? null : cardViewSelected.UniqueEffect;
            cardViewClones.First().ClearUniqueEffect();

            if (effectSelected == null) await _showSelectorComponent.ReturnClones();
            else await _showSelectorComponent.DestroyClones(cardViewSelected);
            return effectSelected;
        }

        private List<CardView> CreateCardViewClones(CardView originalCardView)
        {
            List<Effect> effects = originalCardView.Card.PlayableEffects.ToList();
            originalCardView.HideBuffsAndEffects();
            originalCardView.SetUniqueEffect(effects.First());
            List<CardView> newClonesCardView = new() { { originalCardView } };
            foreach (Effect effect in effects.Skip(1))
            {
                CardView cloneCardView = _cardViewGeneratorComponent.CloneCardView(originalCardView, originalCardView.CurrentZoneView.transform);
                cloneCardView.SetUniqueEffect(effect);
                newClonesCardView.Add(cloneCardView);
            }
            return newClonesCardView;
        }
    }
}
