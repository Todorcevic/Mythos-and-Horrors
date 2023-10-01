using System.Threading.Tasks;

namespace Tuesday.GameRules
{
    public interface ICardActivator
    {
        void ActivateThisCards(params Card[] gameActions);
    }
}
