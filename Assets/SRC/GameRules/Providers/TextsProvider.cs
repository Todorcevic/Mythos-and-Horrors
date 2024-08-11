using System;
using System.Collections.Generic;

namespace MythosAndHorrors.GameRules
{
    public class TextsProvider
    {
        private Dictionary<string, string> _localizableTexts;

        public GameText GameText { get; private set; }

        /*******************************************************************/
        public void AddTexts(GameText gameText)
        {
            GameText = gameText ?? throw new ArgumentNullException(nameof(gameText) + " gameText cant be null");
        }

        public void AddLocalizableDictionary(Dictionary<string, string> interactableText)
        {
            _localizableTexts = interactableText ?? throw new ArgumentNullException(nameof(interactableText) + " interactableText cant be null");
        }

        /*******************************************************************/
        public string GetLocalizableText(string code, string[] descriptionArgs)
        {
            if (!_localizableTexts.TryGetValue(code, out string text)) throw new ArgumentException("Location text not found for code: " + code);
            return text.ParseViewWith(descriptionArgs);
        }
    }
}
