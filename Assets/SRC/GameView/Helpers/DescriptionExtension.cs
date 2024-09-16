using MythosAndHorrors.GameRules;
using System;
using System.Text.RegularExpressions;

namespace MythosAndHorrors.GameView
{
    public static class DescriptionExtension
    {
        public static string ParseDescription(this string text, CardsProvider cardsProvider, TextsManager textsManager, Investigator investigator)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));

            return text.ParseTags(textsManager).ParseCardNames(cardsProvider).ParseInvestigatorNames(investigator);
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

        private static string ParseCardNames(this string text, CardsProvider cardsProvider)
        {
            string pattern = @"<<(.+?)>>";
            return Regex.Replace(text, pattern, match =>
            {
                string stringCode = match.Groups[1].Value;
                return $"<b>{cardsProvider.TryGetCardByCode(stringCode)?.Info.Name ?? match.Value}</b>";
            });
        }

        private static string ParseInvestigatorNames(this string text, Investigator investigator)
        {
            string pattern = @"{{(.+?)}}";
            return Regex.Replace(text, pattern, match =>
            {
                string stringCode = match.Groups[1].Value;
                if (investigator == null) return stringCode;
                if (stringCode == "He") return investigator.InvestigatorCard.Info.Genre == Genre.Male ? "He" : "She";
                if (stringCode == "he") return investigator.InvestigatorCard.Info.Genre == Genre.Male ? "he" : "she";
                if (stringCode == "His") return investigator.InvestigatorCard.Info.Genre == Genre.Male ? "His" : "Her";
                if (stringCode == "his") return investigator.InvestigatorCard.Info.Genre == Genre.Male ? "his" : "her";
                if (stringCode.Contains("the investigator", StringComparison.OrdinalIgnoreCase)) return $"<b>{investigator.InvestigatorCard.Info.Name}</b>";
                return match.Value;
            });
        }
    }
}
