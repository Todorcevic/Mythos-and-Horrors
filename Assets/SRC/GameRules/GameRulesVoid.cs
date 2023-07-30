using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameRules
{
    public class GameRulesVoid
    {
        int AmountCharactersInWord(string word)
        {
            int amount = 0;
            for (int i = 0; i < word.Length; i++)
            {
                if (word[i] != ' ')
                {
                    amount++;
                }
            }
            return amount;
        }
    }
}
