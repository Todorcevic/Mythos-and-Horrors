using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public interface IBuffable : IViewEffect
    {
        void ActivateBuff();
        void DeactivateBuff();
        Task BuffAffectTo(Card cardAffected);
        Task BuffDeaffectTo(Card cardAffected);
    }
}
