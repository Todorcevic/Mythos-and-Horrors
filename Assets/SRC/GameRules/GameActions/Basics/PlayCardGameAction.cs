using System;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class PlayCardGameAction : GameAction
    {
        private Effect _effectSelected;
        [Inject] private readonly GameActionFactory _gameActionFactory;

        public Card CardToPlay { get; private set; }

        private bool IsCanceled => _effectSelected == null;

        /*******************************************************************/
        public async Task<bool> Run(Card cardToPlay)
        {
            CardToPlay = cardToPlay ?? throw new ArgumentNullException("Card cant be null");
            await Start();
            return !IsCanceled;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            _effectSelected = CardToPlay.PlayableEffects.Count > 1 ?
               await _gameActionFactory.Create<MultiEffectSelectionGameAction>().Run(CardToPlay.PlayableEffects.ToList()) :
               CardToPlay.PlayableEffects.Single();
            if (IsCanceled) return;
            await _gameActionFactory.Create<PlayEffectGameAction>().Run(_effectSelected);
        }
    }
}

