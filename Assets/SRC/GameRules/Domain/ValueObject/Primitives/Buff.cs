using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class Buff : IViewEffect
    {
        private readonly Card _cardMaster;
        private readonly List<Card> _currentCardsAffected = new();

        public Func<List<Card>> CardsAffected { get; private set; }
        public Func<Card, Task> ActivationLogic { get; private set; }
        public Func<Card, Task> DeactivationLogic { get; private set; }

        string IViewEffect.CardCode => _cardMaster.Info.Code;

        public string Description { get; private set; }

        string IViewEffect.CardCodeSecundary => _cardMaster.Owner.Code;

        /*******************************************************************/

        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private Buff(Card cardMaster, string description, Func<List<Card>> cardsAffected, Func<Card, Task> activationLogic, Func<Card, Task> deactivationLogic)
        {
            _cardMaster = cardMaster;
            Description = description;
            CardsAffected = cardsAffected;
            ActivationLogic = activationLogic;
            DeactivationLogic = deactivationLogic;
        }

        /*******************************************************************/
        public async Task Check()
        {
            List<Card> cardsAffected = CardsAffected.Invoke();
            List<Card> cardsToActivate = cardsAffected.Except(_currentCardsAffected).ToList();
            List<Card> cardsToDeactivate = _currentCardsAffected.Except(cardsAffected).ToList();

            foreach (Card card in cardsToActivate)
            {
                await ActivationLogic.Invoke(card);
                _currentCardsAffected.Add(card);
            }

            foreach (Card card in cardsToDeactivate)
            {
                await DeactivationLogic.Invoke(card);
                _currentCardsAffected.Remove(card);
            }
        }
    }
}
