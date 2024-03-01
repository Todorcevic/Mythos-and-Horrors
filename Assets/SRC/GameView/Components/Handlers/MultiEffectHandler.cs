using MythsAndHorrors.GameRules;
using System;
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
        private List<IPlayable> cardViewClones;



        [Inject] private readonly MainButtonComponent _mainButtonComponent;
        [Inject] private readonly TextsManager _textsManager;

        /*******************************************************************/
        public async Task<Effect> ShowMultiEffects(CardView cardViewWithMultiEffecs)
        {
            if (cardViewWithMultiEffecs == null) throw new ArgumentNullException(nameof(cardViewWithMultiEffecs));


            _mainButtonComponent.SetButton(_textsManager.ViewText.BUTTON_DONE, new() { Effect.NullEffect });
            cardViewClones = CreateCardViewClones(cardViewWithMultiEffecs);


            await _showSelectorComponent.ShowMultiEffects(cardViewClones.Cast<CardView>().ToList());
            _showCardHandler.ActiavatePlayables(cardViewClones);

            return await FinishMultiEffect(await _clickHandler.WaitingClick() as CardView); // If not is a CardView, was MainButton Pressed and return null
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
                CardView cloneCardView = _cardViewGeneratorComponent.CloneCardView(originalCardView, originalCardView.CurrentZoneView.transform);
                cloneCardView.SetCloneEffect(effect);
                newClonesCardView.Add(cloneCardView);
            }
            return newClonesCardView;
        }
    }
}
