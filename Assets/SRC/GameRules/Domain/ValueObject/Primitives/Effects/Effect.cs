using System;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public record Effect : BaseEffect, IViewEffect
    {
        private readonly Investigator _investigator;

        public Card Card { get; private set; }
        public PlayActionType PlayActionType { get; private set; }
        public Card CardAffected { get; private set; }

        public string CardCode => _investigator?.Code;
        public string CardCodeSecundary => CardAffected?.Info.Code;

        /*******************************************************************/
        public Effect(Card card, Func<Task> logic, PlayActionType playActionType, Investigator investigator = null, Card cardAffected = null, string description = null)
            : base(logic, description)
        {
            Card = card;
            PlayActionType = playActionType;
            _investigator = investigator;
            CardAffected = cardAffected;
        }
    }
}
