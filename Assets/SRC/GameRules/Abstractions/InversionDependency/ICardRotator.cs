using System.Threading.Tasks;

namespace MythsAndHorrors.GameRules
{
    public interface ICardRotator
    {
        void Rotate(Card card);
        Task RotateAsync(Card card);
    }
}
