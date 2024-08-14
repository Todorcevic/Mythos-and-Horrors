using System;
using System.Collections.Generic;

namespace MythosAndHorrors.GameRules
{
    public class TextsProvider
    {
        private Dictionary<string, string> _localizableTexts;

        /*******************************************************************/
        public void AddLocalizableDictionary(Dictionary<string, string> interactableText)
        {
            _localizableTexts = interactableText ?? throw new ArgumentNullException(nameof(interactableText) + " interactableText cant be null");
        }

        /*******************************************************************/
        public string GetLocalizableText(string code, params string[] descriptionArgs)
        {
            if (!_localizableTexts.TryGetValue(code, out string text)) throw new ArgumentException("Location text not found for code: " + code);
            return text.ParseViewWith(descriptionArgs);
        }
    }
}
