using System.Collections.Generic;
using Zenject;

namespace GameRules
{
    public abstract class GameAction
    {
        //[Inject] private static readonly DiContainer diContainer;
        protected readonly Queue<GameAction> gameActions = new();

        /*******************************************************************/
        //public static GameAction GiveMe<T>() where T : GameAction => diContainer.Instantiate<T>();

    }
}
