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
        private readonly List<Card> _currentCardsAffected = new();

        public Func<IEnumerable<Card>> CardsToBuff { get; private set; }
        public Func<Card, Task> ActivationLogic { get; private set; }
        public Func<Card, Task> DeactivationLogic { get; private set; }

        string IViewEffect.CardCode => _cardMaster.Info.Code;
        string IViewEffect.Description => _description;
        string IViewEffect.CardCodeSecundary => _cardMaster.Owner.Code;

        /*******************************************************************/
        public async Task Check()
        {
            if (_isBuffing) return;
            _isBuffing = true;
            IEnumerable<Card> cardsAffected = CardsToBuff.Invoke();
            List<Card> cardsToActivate = cardsAffected.Except(_currentCardsAffected).ToList();
            List<Card> cardsToDeactivate = _currentCardsAffected.Except(cardsAffected).ToList();

            foreach (Card card in cardsToActivate)
            {
                _currentCardsAffected.Add(card);
                await ActivationLogic.Invoke(card);
            }

            foreach (Card card in cardsToDeactivate)
            {
                _currentCardsAffected.Remove(card);
                await DeactivationLogic.Invoke(card);
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

        public Buff SetAddBuff(Func<Card, Task> activationLogic)
        {
            ActivationLogic = activationLogic;
            return this;
        }

        public Buff SetRemoveBuff(Func<Card, Task> deactivationLogic)
        {
            DeactivationLogic = deactivationLogic;
            return this;
        }
    }
}
