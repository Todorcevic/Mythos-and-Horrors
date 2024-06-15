using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public record CardEffect : BaseEffect, IViewEffect
    {
        public Stat ResourceCost { get; }
        public Card CardOwner { get; }
        public Card CardAffected { get; }
        public string CardCode => Investigator?.Code;
        public string CardCodeSecundary => CardAffected?.Info.Code;

        /*******************************************************************/
        public CardEffect(Card card, Stat activateTurnCost, Func<Task> logic, PlayActionType playActionType, Investigator playedBy,
            Card cardAffected = null, string description = null, Stat resourceCost = null)
            : base(activateTurnCost, logic, playActionType, playedBy, description)
        {
            CardOwner = card;
            ResourceCost = resourceCost ?? new Stat(0, false);
            CardAffected = cardAffected;
        }
    }
}
