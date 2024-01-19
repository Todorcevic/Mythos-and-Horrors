using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public abstract class InteractableGameAction : GameAction
    {
        [Inject] private readonly IInteractableHandler _interactableAnimator;
        [Inject] private readonly GameActionFactory _gameActionFactory;
        [Inject] private readonly CardsProvider _cardsProvider;

        public List<Card> ActivableCards => _cardsProvider.GetPlayableCards();
        public virtual bool IsManadatary => false;
        public Card CardSelected { get; private set; }
        public bool ButtonIsPressed => CardSelected == null;

        /*******************************************************************/
        protected override async Task Start()
        {
            _cardsProvider.AllCards.ForEach(card => card.ClearEffects());
            await base.Start();
        }

        protected override async Task ExecuteThisLogic()
        {
            CardSelected = await _interactableAnimator.Interact(this);
            if (ButtonIsPressed) return;
            bool isPlayed = await _gameActionFactory.Create<PlayCardGameAction>().Run(CardSelected);
            if (!isPlayed) await Start();
        }
    }
}
