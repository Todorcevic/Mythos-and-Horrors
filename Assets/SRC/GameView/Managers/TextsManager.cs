using System;
using System.Collections.Generic;

namespace MythosAndHorrors.GameView
{
    public class TextsManager
    {
        public ViewText ViewText { get; private set; }
        public Dictionary<string, InteractableText> InteractableTexts { get; private set; }

        /*******************************************************************/
        public void AddTexts(ViewText viewText)
        {
            ViewText = viewText ?? throw new ArgumentNullException(nameof(viewText) + " gameText cant be null");
        }

        public void AddInteractableTexts(Dictionary<string, InteractableText> interactableText)
        {
            InteractableTexts = interactableText ?? throw new ArgumentNullException(nameof(interactableText) + " gameText cant be null");
        }
    }
}
