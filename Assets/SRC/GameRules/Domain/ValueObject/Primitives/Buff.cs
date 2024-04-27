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
        private Card _cardMaster;

        public Func<IEnumerable<Card>> CardsToBuff { get; private set; }
        public Func<IEnumerable<Card>, Task> ActivationLogic { get; private set; }
        public Func<IEnumerable<Card>, Task> DeactivationLogic { get; private set; }
        public List<Card> CurrentCardsAffected { get; private set; } = new();

        string IViewEffect.CardCode => _cardMaster.Info.Code;
        string IViewEffect.Description => _description;
        string IViewEffect.CardCodeSecundary => _cardMaster.Owner.Code;

        /*******************************************************************/
        public async Task Execute()
        {
            if (_isBuffing) return;
            _isBuffing = true;

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

            _isBuffing = false;
        }
        /*******************************************************************/

        public Buff SetCard(Card cardMaster)
        {
            _cardMaster = cardMaster;
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
    }
}
