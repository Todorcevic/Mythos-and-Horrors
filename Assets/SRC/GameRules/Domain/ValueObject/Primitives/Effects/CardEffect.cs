using System;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class CardEffect : BaseEffect, IViewEffect
    {
        public Stat ResourceCost { get; }
        public Card CardOwner { get; }
        public Card CardAffected { get; }
        public override bool CanBePlayed => base.CanBePlayed && ResourceCost.Value <= Investigator.Resources.Value;
        public string CardCode => Investigator?.Code;
        public string CardCodeSecundary => CardAffected?.Info.Code;


        /*******************************************************************/
        public CardEffect(Card card, Stat activateTurnCost, Func<Task> logic, PlayActionType playActionType, Investigator playedBy, Localization localization,
            Card cardAffected = null, Stat resourceCost = null) : base(activateTurnCost, logic, playActionType, playedBy, localization)
        {
            CardOwner = card;
            ResourceCost = resourceCost ?? new Stat(0, false);
            CardAffected = cardAffected;
        }
    }
}
