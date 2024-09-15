using MythosAndHorrors.GameRules;
using System;
using System.Text.RegularExpressions;

namespace MythosAndHorrors.GameView
{
    public static class DescriptionExtension
    {
        public static string ParseDescription(this string text, CardsProvider _cardsProvider, TextsManager textsManager)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));

            return text.ParseTags(textsManager).ParseCardNames(_cardsProvider);
        }

        private static string ParseTags(this string text, TextsManager textsManager)
        {
            string pattern = @"\[\[(.*?)\]\]";
            return Regex.Replace(text, pattern, match =>
            {
                string tagName = match.Groups[1].Value;
                if (Enum.TryParse<Tag>(tagName, true, out var tag)) return $"<i>{textsManager.GetTagText(tag)}</i>";

                return match.Value;
            });
        }

        private static string ParseCardNames(this string text, CardsProvider _cardsProvider)
        {
            string pattern = @"<<(.+?)>>";
            return Regex.Replace(text, pattern, match =>
            {
                string stringCode = match.Groups[1].Value;
                return $"<b>{_cardsProvider.TryGetCardByCode(stringCode)?.Info.Name ?? match.Value}</b>";
            });
        }
    }
}
