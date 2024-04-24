using System;
using System.Linq;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public record Effect : IViewEffect
    {
        private Investigator _investigator;
        private Card _cardAffected;
        public Card Card { get; private set; }
        public Func<Task> Logic { get; private set; }
        public string CardCode => _investigator?.Code;
        public string Description => Logic.GetInvocationList().First().Method.Name;
        public string CardCodeSecundary => _cardAffected?.Info.Code;

        /*******************************************************************/
        public Effect SetCard(Card cardAffected)
        {
            Card = cardAffected;
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

        //public Effect SetDescription(string description)
        //{
        //    Description = description;
        //    return this;
        //}
    }
}
