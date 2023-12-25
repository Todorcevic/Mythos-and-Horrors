using System.Threading.Tasks;

namespace MythsAndHorrors.GameRules
{
    public interface ICardRotator
    {
        Task Rotate(Card card);
    }
}
