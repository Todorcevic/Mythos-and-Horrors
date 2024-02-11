using System;
using System.Threading.Tasks;

namespace MythsAndHorrors.GameRules
{
    public class Buff : IEffect
    {
        public Card CardParent { get; init; }
        public string Description { get; }
        public Func<Card, Task> ActivateBuff { get; }
        public Func<Card, Task> DeactivateBuff { get; }
        public string CardCode => CardParent.Info.Code;
        public string CardCodeAffected => CardParent.Owner.Code;

        /*******************************************************************/
        public Buff(Card cardParent, string description, Func<Card, Task> activate, Func<Card, Task> deactivate)
        {
            CardParent = cardParent;
            Description = description;
            ActivateBuff = activate;
            DeactivateBuff = deactivate;
        }
    }
}
