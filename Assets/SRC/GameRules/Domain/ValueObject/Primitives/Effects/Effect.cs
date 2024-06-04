using System;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public record Effect : BaseEffect, IViewEffect
    {
        public Card CardOwner { get; }
        public PlayActionType PlayActionType { get; }
        public Investigator Investigator { get; }
        public Card CardAffected { get; }

        public string CardCode => Investigator?.Code;
        public string CardCodeSecundary => CardAffected?.Info.Code;

        /*******************************************************************/
        public Effect(Card card, Func<Task> logic, PlayActionType playActionType, Investigator playedBy, Card cardAffected = null, string description = null)
            : base(logic, description)
        {
            CardOwner = card;
            PlayActionType = playActionType;
            Investigator = playedBy;
            CardAffected = cardAffected;
        }
    }
}
