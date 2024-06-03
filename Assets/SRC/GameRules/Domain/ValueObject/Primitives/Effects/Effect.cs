using System;
using System.Linq;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public record Effect : BaseEffect, IViewEffect
    {
        private readonly Investigator _investigator;
        private readonly Card _cardAffected;

        public Card Card { get; private set; }
        public PlayActionType PlayActionType { get; private set; }

        public override string CardCode => _investigator?.Code;
        public override string CardCodeSecundary => _cardAffected?.Info.Code;

        /*******************************************************************/
        public Effect(Card card, Func<Task> logic, PlayActionType playActionType, Investigator investigator = null, Card cardAffected = null, string description = null)
            : base(logic, description)
        {
            Card = card;
            PlayActionType = playActionType;
            _investigator = investigator;
            _cardAffected = cardAffected;
        }
    }
}
