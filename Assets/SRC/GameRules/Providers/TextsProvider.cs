using System;

namespace MythsAndHorrors.GameRules
{
    public class TextsProvider
    {
        public GameText GameText { get; private set; }

        /*******************************************************************/
        public void AddTexts(GameText gameText)
        {
            GameText = gameText ?? throw new ArgumentNullException(nameof(gameText) + " gameText cant be null");
        }
    }   
}
