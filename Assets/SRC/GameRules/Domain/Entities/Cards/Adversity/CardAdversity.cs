using Zenject;

namespace MythosAndHorrors.GameRules
{
    public abstract class CardAdversity : Card
    {
        public abstract Zone ZoneToMove { get; }
    }
}
