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
        [Inject] private readonly ShowSelectorComponent _showSelectorComponent;
        [Inject] private readonly ActivatePlayablesHandler _showCardHandler;
        [Inject] private readonly ClickHandler<IPlayable> _clickHandler;
        private CardView originalCardView;
        private List<IPlayable> cardViewClones;

        /*******************************************************************/
        public async Task<Effect> ShowMultiEffects(CardView cardViewWithMultiEffecs)
        {
            if (cardViewWithMultiEffecs == null) throw new ArgumentNullException(nameof(cardViewWithMultiEffecs));
            originalCardView = cardViewWithMultiEffecs;
            cardViewClones = CreateCardViewClones();
            await _showSelectorComponent.ShowMultiEffects(cardViewClones.Cast<CardView>().ToList());
            _showCardHandler.ActiavatePlayables(cardViewClones);
            IPlayable playableSelected = await _clickHandler.WaitingClick();
            return await FinishMultiEffect(playableSelected);
        }

        private async Task<Effect> FinishMultiEffect(IPlayable playableSelected)
        {
            await _showCardHandler.DeactivatePlayables();

            if (playableSelected is MainButtonComponent)
            {
                await _showSelectorComponent.ReturnClones();
                originalCardView.ClearCloneEffect();
                return null;
            }
            else
            {
                Effect effectSelected = playableSelected.EffectsSelected.Single();
                originalCardView.ClearCloneEffect();
                await _showSelectorComponent.DestroyClones((CardView)playableSelected);
                return effectSelected;
            }
        }

        private List<IPlayable> CreateCardViewClones()
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
