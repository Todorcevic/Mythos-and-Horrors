using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class SafeForeach<T>
    {
        private readonly List<T> _elementsExecuted = new();
        public Func<T, Task> Logic { get; }
        public Func<IEnumerable<T>> Collection { get; }

        /*******************************************************************/
        public SafeForeach(Func<T, Task> logic, Func<IEnumerable<T>> collection)
        {
            Logic = logic;
            Collection = collection;
        }

        /*******************************************************************/
        public async Task Execute()
        {
            T element = Collection.Invoke().FirstOrDefault();
            while (element != null)
            {
                await Logic.Invoke(element);
                _elementsExecuted.Add(element);
                element = Collection.Invoke().Except(_elementsExecuted).FirstOrDefault();
            }
        }
    }
}
