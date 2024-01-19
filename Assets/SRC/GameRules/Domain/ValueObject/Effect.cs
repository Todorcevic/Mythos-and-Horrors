using System;
using System.Threading.Tasks;

namespace MythsAndHorrors.GameRules
{
    public record Effect
    {
        public Effect(Card card, string description, Func<Task> logic)
        {
            Card = card;
            Description = description;
            Logic = logic;
        }

        /*******************************************************************/
        public Card Card { get; init; }
        public string Description { get; init; }
        public Func<Task> Logic { get; init; }
    }
}
