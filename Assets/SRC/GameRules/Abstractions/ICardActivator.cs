using System.Threading.Tasks;

namespace GameRules
{
    public interface ICardActivator
    {
        void ActivateThisCards(params Card[] gameActions);
    }
}
