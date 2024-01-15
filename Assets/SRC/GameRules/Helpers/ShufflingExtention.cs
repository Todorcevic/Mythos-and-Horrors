using System;
using System.Collections.Generic;

namespace MythsAndHorrors.GameRules
{
    public static class ShufflingExtention
    {
        private readonly static Random rng = new();

        public static void Shuffle<T>(this IList<T> list)
        {
            int elementAmount = list.Count;
            while (elementAmount > 1)
            {
                elementAmount--;
                int randomNumber = rng.Next(elementAmount + 1);
                (list[elementAmount], list[randomNumber]) = (list[randomNumber], list[elementAmount]);
            }
        }
    }
}
