using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class InteractableGameAction : GameAction
    {
        [Inject] private readonly IViewLayer _viewLayer;
        [Inject] private readonly GameActionFactory _gameActionFactory;
        [Inject] private readonly CardsProvider _cardsProvider;

        public List<Card> ActivableCards => _cardsProvider.GetPlayableCards();
        public bool IsManadatary { get; private set; }
        public Effect EffectSelected { get; private set; }
        public bool NothingIsSelected => EffectSelected == null;

        /*******************************************************************/
        public InteractableGameAction(bool isMandatary)
        {
            IsManadatary = isMandatary;
        }

        /*******************************************************************/
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
