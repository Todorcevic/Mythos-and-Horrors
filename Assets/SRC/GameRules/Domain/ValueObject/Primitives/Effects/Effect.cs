using System;
using System.Linq;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public record Effect : IViewEffect
    {
        private string _description;
        private Investigator _investigator;
        private Card _cardAffected;

        public Card Card { get; private set; }
        public Func<Task> Logic { get; private set; }
        public PlayActionType PlayActionType { get; private set; }

        public string CardCode => _investigator?.Code;
        public string Description => _description ?? Logic.GetInvocationList().First().Method.Name;
        public string CardCodeSecundary => _cardAffected?.Info.Code;

        /*******************************************************************/
        public Effect()
        {
        }


        public Effect(Card card, Func<Task> logic, PlayActionType playActionType, Investigator investigator = null, Card cardAffected = null, string description = null)
        {
            Card = card;
            Logic = logic;
            PlayActionType = playActionType;
            _investigator = investigator;
            _cardAffected = cardAffected;
            _description = description;
        }

        /*******************************************************************/


        public Effect SetCard(Card card)
        {
            Card = card;
            return this;
        }

        public Effect SetInvestigator(Investigator investigator)
        {
            _investigator = investigator;
            return this;
        }

        public Effect SetCardAffected(Card cardAffected)
        {
            _cardAffected = cardAffected;
            return this;
        }

        public Effect SetLogic(Func<Task> logic)
        {
            Logic = logic;
            return this;
        }

        public Effect SetDescription(string description)
        {
            _description = description;
            return this;
        }
    }
}
