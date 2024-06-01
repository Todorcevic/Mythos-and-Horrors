using System;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class GameCommand<T>
    {
        private readonly Func<T, Task> _originalGameAction;

        public Func<T, Task> Logic { get; private set; }

        /*******************************************************************/
        public GameCommand(Func<T, Task> gameAction)
        {
            _originalGameAction = Logic = gameAction;
        }

        /*******************************************************************/
        public void Reset()
        {
            Logic = _originalGameAction;
        }

        public void UpdateWith(Func<T, Task> gameAction)
        {
            Logic = gameAction;
        }

        public Task RunWith(T element)
        {
            return Logic(element);
        }
    }
}
