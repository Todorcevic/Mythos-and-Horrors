using System;
using System.Threading.Tasks;

namespace MythsAndHorrors.GameRules
{
    public record Effect : IViewEffect
    {
        public Effect(Card card, Investigator investigator, string description, Func<Task> logic, Investigator investigatorAffected = null)
        {
            Card = card;
            Investigator = investigator;
            InvestigatorAffected = investigatorAffected;
            Logic = logic;
            Description = description;
        }

        /*******************************************************************/
        public Card Card { get; init; }
        public Investigator Investigator { get; init; }
        public Investigator InvestigatorAffected { get; init; }

        public Func<Task> Logic { get; init; }

        public string CardCode => Investigator?.Code;
        public string Description { get; init; }
        public string CardCodeSecundary => InvestigatorAffected?.Code;
    }
}
