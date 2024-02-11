using System;
using System.Threading.Tasks;

namespace MythsAndHorrors.GameRules
{
    public record Effect : IViewEffect
    {
        public Effect(Investigator investigator, Card card, string description, Func<Task> logic, Investigator investigatorAffected = null)
        {
            Investigator = investigator;
            InvestigatorAffected = investigatorAffected;
            Card = card;
            Description = description;
            Logic = logic;
        }

        /*******************************************************************/
        public Investigator Investigator { get; init; }
        public Investigator InvestigatorAffected { get; init; }
        public Card Card { get; init; }
        public string Description { get; init; }
        public Func<Task> Logic { get; init; }

        public string CardCode => Investigator?.Code;
        public string CardCodeSecundary => InvestigatorAffected?.Code;
    }
}
