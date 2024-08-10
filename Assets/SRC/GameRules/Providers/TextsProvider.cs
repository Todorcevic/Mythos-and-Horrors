using System;
using System.Collections.Generic;

namespace MythosAndHorrors.GameRules
{
    public class TextsProvider
    {
        private Dictionary<string, string> _interactableTexts;

        public GameText GameText { get; private set; }

        /*******************************************************************/
        public void AddTexts(GameText gameText)
        {
            GameText = gameText ?? throw new ArgumentNullException(nameof(gameText) + " gameText cant be null");
        }

        public void AddInteractableTexts(Dictionary<string, string> interactableText)
        {
            _interactableTexts = interactableText ?? throw new ArgumentNullException(nameof(interactableText) + " gameText cant be null");
        }

        public string GetInteractableText(string code, string[] args) => _interactableTexts[code].ParseViewWith(args);
    }
}
