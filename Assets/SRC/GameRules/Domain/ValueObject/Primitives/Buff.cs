using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class Buff : IViewEffect
    {
        private bool _isBuffing;
        private string _description;

        public Card CardMaster { get; private set; }
        public Func<IEnumerable<Card>> CardsToBuff { get; private set; }
        public Func<IEnumerable<Card>, Task> ActivationLogic { get; private set; }
        public Func<IEnumerable<Card>, Task> DeactivationLogic { get; private set; }
        public List<Card> CurrentCardsAffected { get; private set; } = new();
        public bool IsDisabled { get; private set; }

        string IViewEffect.CardCode => CardMaster.Info.Code;
        string IViewEffect.Description => _description;
        string IViewEffect.CardCodeSecundary => CardMaster.ControlOwner?.Code;

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

        public async Task Deactive()
        {
            if (CurrentCardsAffected.Count < 1) return;
            await DeactivationLogic.Invoke(CurrentCardsAffected);
            CurrentCardsAffected.Clear();
        }

        private async Task Apply()
        {
            IEnumerable<Card> cardsAffected = CardsToBuff.Invoke().ToList();
            IEnumerable<Card> cardsToActivate = cardsAffected.Except(CurrentCardsAffected);
            IEnumerable<Card> cardsToDeactivate = CurrentCardsAffected.Except(cardsAffected);

            if (cardsToActivate.Any())
            {
                await ActivationLogic.Invoke(cardsToActivate);
                CurrentCardsAffected.AddRange(cardsToActivate);
            }

            if (cardsToDeactivate.Any())
            {
                await DeactivationLogic.Invoke(cardsToDeactivate);
                CurrentCardsAffected.RemoveAll(card => cardsToDeactivate.Contains(card));
            }
        }

        private readonly Stack<List<Card>> _undoCardsAfeccted = new();

        public void Undo()
        {
            if (_undoCardsAfeccted.Count < 1) return;
            CurrentCardsAffected = _undoCardsAfeccted.Pop().ToList();
        }

        /*******************************************************************/
        public Buff SetCard(Card cardMaster)
        {
            CardMaster = cardMaster;
            return this;
        }

        public Buff SetDescription(string description)
        {
            _description = description;
            return this;
        }

        public Buff SetCardsToBuff(Func<IEnumerable<Card>> cardsToBuff)
        {
            CardsToBuff = cardsToBuff;
            return this;
        }

        public Buff SetAddBuff(Func<IEnumerable<Card>, Task> activationLogic)
        {
            ActivationLogic = activationLogic;
            return this;
        }

        public Buff SetRemoveBuff(Func<IEnumerable<Card>, Task> deactivationLogic)
        {
            DeactivationLogic = deactivationLogic;
            return this;
        }

        public void Enable() => IsDisabled = false;

        public void Disable() => IsDisabled = true;
    }
}
