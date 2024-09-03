using MythosAndHorrors.GameRules;
using System;
using System.Collections.Generic;

namespace MythosAndHorrors.GameView
{
    public class TextsManager
    {
        private Dictionary<string, string> _localizableTexts;

        /*******************************************************************/
        public void AddLocalizableDictionary(Dictionary<string, string> interactableText)
        {
            _localizableTexts = interactableText ?? throw new ArgumentNullException(nameof(interactableText) + " interactableText cant be null");
        }

        /*******************************************************************/
        public string GetLocalizableText(Localization localization)
        {
            if (!_localizableTexts.TryGetValue(localization.Code, out string text)) throw new ArgumentException("Location text not found for code: " + localization.Code);
            return text.ParseViewWith(localization.Args);
        }
    }
}
