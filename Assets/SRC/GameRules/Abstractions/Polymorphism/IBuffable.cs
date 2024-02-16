using System.Threading.Tasks;

namespace MythsAndHorrors.GameRules
{
    public interface IRevellable
    {
        void Reveal();
    }

    public interface IBuffable : IViewEffect
    {
        void ActivateBuff();
        void DeactivateBuff();
        Task BuffAffectTo(Card cardAffected);
        Task BuffDeaffectTo(Card cardAffected);
    }
}
