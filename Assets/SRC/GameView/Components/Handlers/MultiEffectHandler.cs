using MythosAndHorrors.GameRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class MultiEffectHandler
    {
        [Inject] private readonly CardViewGeneratorComponent _cardViewGeneratorComponent;
        [Inject] private readonly ShowSelectorComponent _showSelectorComponent;
        [Inject] private readonly ActivatePlayablesHandler _showCardHandler;
        [Inject] private readonly ClickHandler<IPlayable> _clickHandler;
        [Inject] private readonly MainButtonComponent _mainButtonComponent;
        private List<IPlayable> cardViewClones;

        /*******************************************************************/
        public async Task<Effect> ShowMultiEffects(CardView cardViewWithMultiEffecs)
        {
            if (cardViewWithMultiEffecs == null) throw new ArgumentNullException(nameof(cardViewWithMultiEffecs));

            _mainButtonComponent.SetButton(Effect.ContinueEffect);
            cardViewClones = CreateCardViewClones(cardViewWithMultiEffecs);
            await _showSelectorComponent.ShowMultiEffects(cardViewClones.Cast<CardView>().ToList());
            _showCardHandler.ActiavatePlayables(cardViewClones);

            IPlayable playableSelected = await _clickHandler.WaitingClick();

            return await FinishMultiEffect(playableSelected as CardView); // If not is a CardView, was MainButton Pressed and return null
        }

        private async Task<Effect> FinishMultiEffect(CardView cardViewSelected)
        {
            await _showCardHandler.DeactivatePlayables();
            Effect effectSelected = cardViewSelected == null ? null : cardViewSelected.CloneEffect;
            ((CardView)cardViewClones.First()).ClearCloneEffect();

            if (effectSelected == null) await _showSelectorComponent.ReturnClones();
            else await _showSelectorComponent.DestroyClones(cardViewSelected);
            return effectSelected;
        }

        private List<IPlayable> CreateCardViewClones(CardView originalCardView)
        {
            List<Effect> effects = originalCardView.Card.PlayableEffects.ToList();
            originalCardView.SetCloneEffect(effects.First());
            List<IPlayable> newClonesCardView = new() { originalCardView };
            foreach (Effect effect in effects.Skip(1))
            {
                CardView cloneCardView = originalCardView.CloneToMultiEffect(originalCardView.CurrentZoneView.transform);
                cloneCardView.SetCloneEffect(effect);
                newClonesCardView.Add(cloneCardView);
            }
            return newClonesCardView;
        }
    }
}
