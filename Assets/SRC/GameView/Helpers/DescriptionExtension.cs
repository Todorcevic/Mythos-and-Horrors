using MythosAndHorrors.GameRules;
using System;
using System.Text.RegularExpressions;

namespace MythosAndHorrors.GameView
{
    public static class DescriptionExtension
    {
        public static string ParseDescription(this string text, CardsProvider _cardsProvider)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));

            return ParseView(ParseTags(text), _cardsProvider);
        }

        private static string ParseTags(string text)
        {
            string pattern = @"\[\[(.*?)\]\]";
            return Regex.Replace(text, pattern, match =>
            {
                string tagName = match.Groups[1].Value;
                if (Enum.TryParse<Tag>(tagName, true, out var tag)) return $"<i>{tag}</i>";

                return match.Value;
            });
        }

        private static string ParseView(this string text, CardsProvider _cardsProvider)
        {
            string pattern = @"<<(.+?)>>";
            return Regex.Replace(text, pattern, match =>
            {
                string cardCode = match.Groups[1].Value;

                return $"<b>{_cardsProvider.TryGetCardByCode(cardCode)?.Info.Name ?? match.Value}</b>";
            });
        }
    }
}
