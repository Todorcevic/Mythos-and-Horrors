using System.Threading.Tasks;

namespace MythsAndHorrors.EditMode
{
    public interface ICardRotator
    {
        void Rotate(Card card);
        Task RotateAsync(Card card);
    }
}
