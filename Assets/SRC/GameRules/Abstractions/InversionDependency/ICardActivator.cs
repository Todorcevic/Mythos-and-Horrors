using System.Threading.Tasks;

namespace MythsAndHorrors.EditMode
{
    public interface ICardActivator
    {
        void ActivateThisCards(params Card[] gameActions);
    }
}
