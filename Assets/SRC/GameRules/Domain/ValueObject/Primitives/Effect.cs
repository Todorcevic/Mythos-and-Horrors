using System;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public record Effect : IViewEffect
    {
        private Investigator _investigator;
        private Investigator _investigatorAffected;
        public Card CardAffected { get; private set; }
        public Func<Task> Logic { get; private set; }
        public string CardCode => _investigator?.Code;
        public string Description { get; private set; }
        public string CardCodeSecundary => _investigatorAffected?.Code;

        /*******************************************************************/
        public Effect SetCard(Card cardAffected)
        {
            CardAffected = cardAffected;
            return this;
        }

        public Effect SetInvestigator(Investigator investigator)
        {
            _investigator = investigator;
            return this;
        }

        public Effect SetInvestigatorAffected(Investigator investigatorAffected)
        {
            _investigatorAffected = investigatorAffected;
            return this;
        }

        public Effect SetLogic(Func<Task> logic)
        {
            Logic = logic;
            return this;
        }

        public Effect SetDescription(string description)
        {
            Description = description;
            return this;
        }
    }
}
