using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public abstract class InteractableGameAction : GameAction
    {
        [Inject] private readonly IViewLayer _viewLayer;
        [Inject] private readonly GameActionFactory _gameActionFactory;
        [Inject] private readonly CardsProvider _cardsProvider;

        public List<Card> ActivableCards => _cardsProvider.GetPlayableCards();
        public virtual bool IsManadatary => false;
        public Effect EffectSelected { get; private set; }
        public bool NothingIsSelected => EffectSelected == null;

        /*******************************************************************/
        protected override async Task Start()
        {
            await base.Start();
            _cardsProvider.AllCards.ForEach(card => card.ClearEffects());
        }

        protected override async Task ExecuteThisLogic()
        {
            EffectSelected = await _viewLayer.StartSelectionWith(this);
            ClearEffectsInAllCards();
            if (NothingIsSelected) return;
            await _gameActionFactory.Create<PlayEffectGameAction>().Run(EffectSelected);
        }

        private void ClearEffectsInAllCards() => _cardsProvider.AllCards.ForEach(card => card.ClearEffects());
    }
}
