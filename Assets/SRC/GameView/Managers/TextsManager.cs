using System;

namespace MythsAndHorrors.GameView
{
    public class TextsManager
    {
        public ViewText ViewText { get; private set; }

        /*******************************************************************/
        public void AddTexts(ViewText viewText)
        {
            ViewText = viewText ?? throw new ArgumentNullException(nameof(viewText) + " gameText cant be null");
        }
    }
}
