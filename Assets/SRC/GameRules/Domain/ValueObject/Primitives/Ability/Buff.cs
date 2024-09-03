using ModestTree;
using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class Buff : IViewEffect, IAbility
    {
        private bool _isBuffing;
        private readonly Stack<List<Card>> _undoCardsAfeccted = new();

        public Card CardMaster { get; }
        public Func<IEnumerable<Card>> CardsToBuff { get; }
        public GameCommand<IEnumerable<Card>> BuffOnLogic { get; }
        public GameCommand<IEnumerable<Card>> BuffOffLogic { get; }
        public List<Card> CurrentCardsAffected { get; private set; } = new();
        public bool IsDisabled { get; private set; }
        //public string Description { get; private set; }

        string IViewEffect.CardCode => CardMaster.Info.Code;
        string IViewEffect.CardCodeSecundary => CardMaster.ControlOwner?.Code;
        public Localization Localization { get; private set; }

        /*******************************************************************/
        public Buff(Card card, Func<IEnumerable<Card>> cardsToBuff, GameCommand<IEnumerable<Card>> buffOnLogic,
            GameCommand<IEnumerable<Card>> buffOffLogic, Localization localization)
        {
            CardMaster = card;
            CardsToBuff = cardsToBuff;
            BuffOnLogic = buffOnLogic;
            BuffOffLogic = buffOffLogic;
            Localization = localization;
        }

        /*******************************************************************/
        public async Task Execute()
        {
            if (_isBuffing) return;
            _undoCardsAfeccted.Push(CurrentCardsAffected.ToList());
            _isBuffing = true;

            if (IsDisabled) await Deactive();
            else await Apply();

            _isBuffing = false;
        }

        private async Task Apply()
        {
            IEnumerable<Card> cardsAffected = CardsToBuff.Invoke().ToList();
            IEnumerable<Card> cardsToActivate = cardsAffected.Except(CurrentCardsAffected);
            IEnumerable<Card> cardsToDeactivate = CurrentCardsAffected.Except(cardsAffected);

            if (cardsToActivate.Any())
            {
                await BuffOnLogic.RunWith(cardsToActivate);
                CurrentCardsAffected.AddRange(cardsToActivate);
            }

            if (cardsToDeactivate.Any())
            {
                await BuffOffLogic.RunWith(cardsToDeactivate);
                CurrentCardsAffected.RemoveAll(card => cardsToDeactivate.Contains(card));
            }
        }

        public async Task Deactive()
        {
            if (CurrentCardsAffected.Count < 1) return;
            await BuffOffLogic.RunWith(CurrentCardsAffected);
            CurrentCardsAffected.Clear();
        }

        public void Undo()
        {
            if (_undoCardsAfeccted.Count < 1) return;
            CurrentCardsAffected = _undoCardsAfeccted.Pop().ToList();
        }

        /*******************************************************************/
        public void Enable() => IsDisabled = false;

        public void Disable() => IsDisabled = true;
    }
}
